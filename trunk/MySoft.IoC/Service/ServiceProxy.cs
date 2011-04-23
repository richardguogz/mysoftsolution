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
        private ManualResetEvent wait = new ManualResetEvent(false);
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private SocketClientManager manager;
        private bool connected = false;

        private int timeout;
        /// <summary>
        /// Socket��ʱʱ��
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        public ServiceProxy(SocketClientConfiguration config, int timeout)
        {
            this.config = config;
            this.timeout = timeout;

            #region socketͨѶ

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
                //���ӵ�������
                connected = manager.Client.ConnectTo(config.IP, config.Port);
            }

            if (connected)
            {
                long t1 = System.Environment.TickCount;

                //SerializationManager.Serialize(msg)
                if (OnLog != null) OnLog(string.Format("Run reqMsg for ({0},{1}) to service. -->{2}", msg.ServiceName, msg.SubServiceName, msg.Parameters.SerializedData));

                Guid tid = msg.TransactionId;

                //�������ݰ��������
                bool isSend = manager.Client.SendData(BufferFormat.FormatFCA(msg));

                if (isSend)
                {
                    ResponseMessage retMsg = null;

                    //�����̳߳ض�ȡ������Ϣ
                    ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        while (true)
                        {
                            retMsg = GetData<ResponseMessage>(responses, tid);
                            if (retMsg != null)
                            {
                                //����״̬
                                wait.Set();
                                break;
                            }
                        }
                    });

                    //���õȴ���ʱʱ��
                    if (wait.WaitOne(msg.Timeout < 0 ? timeout : msg.Timeout))
                    {
                        //��ʱ����
                    }

                    //����״̬
                    wait.Reset();

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
                    if (OnLog != null) OnLog(string.Format("Send data to ({0}:{1}) error��", config.IP, config.Port));
                    return null;
                }
            }
            else
            {
                if (OnLog != null) OnLog(string.Format("Server ({0}:{1}) not connected��", config.IP, config.Port));
                return null;
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion

        #region Socket��Ϣί��

        void SocketClientManager_OnReceived(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == -20000) //�Զ�������ݰ����������
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
            //�Ͽ�������
            connected = false;
        }

        void SocketClientManager_OnConnected(string message, bool connected, SocketAsyncEventArgs socketAsync)
        {
            //���Ϸ�����
            this.connected = connected;
        }

        #endregion

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        T GetData<T>(IDictionary map, Guid transactionId)
        {
            if (map.Contains(transactionId))
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
            }

            return default(T);
        }
    }
}