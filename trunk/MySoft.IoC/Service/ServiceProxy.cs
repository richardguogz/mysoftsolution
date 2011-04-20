using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Castle.Windsor;
using MySoft.Net.Client;
using System.Net.Sockets;
using MySoft.Net.Sockets;
using System.Collections;

namespace MySoft.IoC
{
    internal sealed class ServiceProxy : ILogable, IServiceProxy
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private SocketClientManager manager;
        private bool connected = false;

        private int maxTryNum;

        public int MaxTryNum
        {
            get
            {
                return maxTryNum;
            }
            set
            {
                maxTryNum = value;
            }
        }

        public ServiceProxy(SocketClientConfiguration config, int maxTryNum)
        {
            this.config = config;
            this.maxTryNum = maxTryNum;

            #region socket通讯

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);

            #endregion
        }

        public ResponseMessage CallMethod(RequestMessage msg)
        {
            if (!connected)
            {
                //连接到服务器
                connected = manager.Client.ConnectTo(config.IP, config.Port);
            }

            if (connected)
            {
                long t1 = System.Environment.TickCount;

                //SerializationManager.Serialize(msg)
                if (OnLog != null) OnLog(string.Format("Run reqMsg for ({0},{1}) to service. -->{2}", msg.ServiceName, msg.SubServiceName, msg.Parameters.SerializedData));

                Guid tid = msg.TransactionId;

                //发送数据包到服务端
                manager.Client.SendData(BufferFormat.FormatFCA(msg));

                ResponseMessage retMsg = null;

                while (true)
                {
                    retMsg = GetData<ResponseMessage>(responses, tid);
                    if (retMsg != null) break;
                }

                //for (int i = 0; i < maxTryNum; i++)
                //{
                //    retMsg = GetData<ResponseMessage>(responses, tid);
                //    if (retMsg == null)
                //    {
                //        if (OnLog != null) OnLog(string.Format("Try {0} running ({1},{2}) -->{3}", (i + 1), msg.ServiceName, msg.SubServiceName, msg.Parameters.SerializedData));
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}

                long t2 = System.Environment.TickCount - t1;
                if (retMsg != null)
                {
                    if (retMsg.Data is Exception)
                    {
                        throw retMsg.Data as Exception;
                    }

                    //SerializationManager.Serialize(retMsg)
                    if (OnLog != null) OnLog(string.Format("Result -->{0}\r\n{1}", retMsg.Message, "Spent time: (" + t2.ToString() + ") ms"));
                }

                return retMsg;
            }
            else
            {
                if (OnLog != null) OnLog(string.Format("Server ({0}:{1}) not connected！", config.IP, config.Port));

                return null;
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion

        #region Socket消息委托

        void SocketClientManager_OnReceived(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == -20000) //自定义的数据包，输出数据
                {
                    object responseObject;
                    if (read.ReadObject(out responseObject))
                    {
                        ResponseMessage result = responseObject as ResponseMessage;

                        lock (responses)
                        {
                            responses[result.Request.TransactionId] = result;
                        }
                    }
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message, SocketAsyncEventArgs socketAsync)
        {
            //断开服务器
            connected = false;
        }

        void SocketClientManager_OnConnected(string message, bool connected, SocketAsyncEventArgs socketAsync)
        {
            //连上服务器
            this.connected = connected;
        }

        #endregion

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        T GetData<T>(IDictionary map, Guid transactionId)
        {
            lock (map)
            {
                if (map.Contains(transactionId))
                {
                    object retObj = map[transactionId];
                    map.Remove(transactionId);
                    return (T)retObj;
                }
            }

            return default(T);
        }
    }
}