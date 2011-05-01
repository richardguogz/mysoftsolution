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
using MySoft.Threading;
using MySoft.Logger;

namespace MySoft.IoC
{
    internal class ServiceProxy : ILogable, IServiceProxy
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private SocketClientManager manager;
        private bool encrypt = false;
        private bool compress = false;
        private bool throwerror = true;
        private bool connected = false;
        private int timeout;

        //threading
        private SmartThreadPool pool;

        /// <summary>
        /// Gets or sets the encrypt.
        /// </summary>
        public bool Encrypt
        {
            get
            {
                return encrypt;
            }
            set
            {
                encrypt = value;
            }
        }

        /// <summary>
        /// Gets or sets the compress.
        /// </summary>
        public bool Compress
        {
            get
            {
                return compress;
            }
            set
            {
                compress = value;
            }
        }

        /// <summary>
        /// Gets or sets the throwerror
        /// </summary>
        /// <value>The throwerror.</value>
        public bool ThrowError
        {
            get
            {
                return throwerror;
            }
            set
            {
                throwerror = value;
            }
        }

        /// <summary>
        /// Socket超时时间
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

        public ServiceProxy(SocketClientConfiguration config)
        {
            this.config = config;

            #region socket通讯

            //实例化线程池
            pool = new SmartThreadPool(30 * 1000, 1000, 0);

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);

            //启动一个检测线程
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (!connected)
                    {
                        //尝试连接到服务器
                        manager.Client.BeginConnectTo(config.IP, config.Port);
                    }

                    //每5秒检测一次
                    Thread.Sleep(5000);
                }
            });

            thread.IsBackground = true;
            thread.Start();

            #endregion
        }

        public ResponseMessage CallMethod(RequestMessage msg, int logtimeout)
        {
            if (connected)
            {
                //处理过期时间
                msg.Expiration = DateTime.Now.AddMilliseconds(msg.Timeout);

                int t1 = System.Environment.TickCount;

                //发送数据包到服务端
                bool isSend = manager.Client.SendData(BufferFormat.FormatFCA(msg));

                if (isSend)
                {
                    //启动线程来处理数据
                    IWorkItemResult<ResponseMessage> wir = pool.QueueWorkItem(state =>
                    {
                        ResponseMessage ret = null;

                        RequestMessage reqMsg = state as RequestMessage;
                        while (true)
                        {
                            ret = GetData<ResponseMessage>(responses, reqMsg.TransactionId);

                            //如果有数据返回，则响应
                            if (ret != null) break;

                            //防止cpu使用率过高
                            Thread.Sleep(1);
                        }

                        return ret;
                    }, msg);

                    //从线程获取返回信息，超时等待
                    ResponseMessage retMsg = null;

                    try
                    {
                        retMsg = wir.GetResult(msg.Timeout, true);

                        if (retMsg.Exception != null)
                        {
                            throw retMsg.Exception;
                        }
                    }
                    catch (WorkItemResultException)  //表示结果为空
                    {
                        throw new IoCException(string.Format("【{4}】Call ({0}:{1}) remote service ({2},{3}) failure. result is empty！", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, msg.TransactionId));
                    }
                    catch (WorkItemTimeoutException) //表示等待超时
                    {
                        int timeout = System.Environment.TickCount - t1;
                        throw new IoCException(string.Format("【{5}】Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)！", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, timeout, msg.TransactionId));
                    }

                    int t2 = System.Environment.TickCount - t1;

                    //如果时间超过预定，则输出日志
                    if (t2 > logtimeout)
                    {
                        //SerializationManager.Serialize(retMsg)
                        if (OnLog != null) OnLog(string.Format("【{7}】Call ({0}:{1}) remote service ({2},{3}). ==> {4} {5} <==> {6}", config.IP, config.Port, retMsg.ServiceName, retMsg.SubServiceName, msg.Parameters,
                            "Spent time: (" + t2.ToString() + ") ms.", retMsg.Message, retMsg.TransactionId), LogType.Warning);
                    }

                    return retMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure！", config.IP, config.Port));
                }
            }
            else
            {
                throw new IoCException(string.Format("Server ({0}:{1}) not connected！", config.IP, config.Port));
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion

        #region Socket消息委托

        void SocketClientManager_OnReceived(byte[] buffer, Socket socket)
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
                            //如果超时，则不返回数据
                            if (result.Expiration > DateTime.Now)
                            {
                                responses[result.TransactionId] = result;
                            }
                        }
                    }
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message, Socket socket)
        {
            //断开服务器
            connected = false;

            //断开套接字
            socket.Disconnect(true);
        }

        void SocketClientManager_OnConnected(string message, bool connected, Socket socket)
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