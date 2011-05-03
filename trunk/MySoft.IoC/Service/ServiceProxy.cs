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
        private string serviceName;
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

        /// <summary>
        /// �Ƿ����ӵ�������
        /// </summary>
        public bool IsConnected
        {
            get { return connected; }
        }

        /// <summary>
        /// ���ӵ�������
        /// </summary>
        public bool ConnectServer(bool isReconnect)
        {
            if (!connected)
            {
                //�������ӵ�������
                manager.Client.BeginConnectTo(config.IP, config.Port);

                IWorkItemResult wir = pool.QueueWorkItem(() =>
                {
                    //���Ϸ��������ܽ������ݴ���
                    while (!connected)
                    {
                        Thread.Sleep(1);
                    }
                });

                //�ȴ�5�룬�������򷵻�false
                if (!pool.WaitForIdle(5000))
                {
                    if (!wir.IsCompleted) wir.Cancel(true);

                    if (isReconnect)
                        throw new IoCException(string.Format("Reconnect to server ({0}:{1}) failure��service: {2}", config.IP, config.Port, serviceName));
                    else
                        throw new IoCException(string.Format("Can't connect to server ({0}:{1})��service: {2}", config.IP, config.Port, serviceName));
                }
            }

            return connected;
        }

        public ServiceProxy(SocketClientConfiguration config, string serviceName)
        {
            this.config = config;
            this.serviceName = serviceName;

            #region socketͨѶ

            //ʵ�����̳߳�
            pool = new SmartThreadPool(30 * 1000, 1000, 0);

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);

            //�������ӵ�������
            ConnectServer(false);

            #endregion
        }

        public ResponseMessage CallMethod(RequestMessage msg, int showlogtime)
        {
            //�������ʱ��
            msg.Expiration = DateTime.Now.AddMilliseconds(msg.Timeout);

            int t1 = System.Environment.TickCount;

            //�������ݰ��������
            bool isSend = manager.Client.SendData(BufferFormat.FormatFCA(msg));

            if (isSend)
            {
                //�����߳�����������
                IWorkItemResult<ResponseMessage> wir = pool.QueueWorkItem(state =>
                {
                    ResponseMessage ret = null;

                    RequestMessage reqMsg = state as RequestMessage;
                    while (true)
                    {
                        ret = GetData<ResponseMessage>(responses, reqMsg.TransactionId);

                        //��������ݷ��أ�����Ӧ
                        if (ret != null) break;

                        //��ֹcpuʹ���ʹ���
                        Thread.Sleep(1);
                    }

                    return ret;
                }, msg);

                if (!pool.WaitForIdle(msg.Timeout))
                {
                    if (!wir.IsCompleted) wir.Cancel(true);
                    int timeout = System.Environment.TickCount - t1;
                    throw new IoCException(string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, timeout, msg.TransactionId));
                }

                //���̻߳�ȡ������Ϣ����ʱ�ȴ�
                ResponseMessage retMsg = wir.GetResult();
                if (retMsg == null)
                {
                    throw new IoCException(string.Format("��{4}��Call ({0}:{1}) remote service ({2},{3}) failure. result is empty��", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, msg.TransactionId));
                }

                //����������쳣�����׳��쳣
                if (retMsg.Exception != null)
                {
                    throw retMsg.Exception;
                }

                int t2 = System.Environment.TickCount - t1;

                //���ʱ�䳬��Ԥ�����������־
                if (t2 > showlogtime)
                {
                    //SerializationManager.Serialize(retMsg)
                    if (OnLog != null) OnLog(string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). ==> {4} {5} <==> {6}", config.IP, config.Port, retMsg.ServiceName, retMsg.SubServiceName, msg.Parameters,
                        "Spent time: (" + t2.ToString() + ") ms.", retMsg.Message, retMsg.TransactionId), LogType.Warning);
                }

                return retMsg;
            }
            else
            {
                throw new IoCException(string.Format("Send data to ({0}:{1}) failure��", config.IP, config.Port));
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion

        #region Socket��Ϣί��

        void SocketClientManager_OnReceived(byte[] buffer, Socket socket)
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
                            responses[result.TransactionId] = result;
                        }
                    }
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message, Socket socket)
        {
            //�Ͽ�������
            connected = false;

            //�Ͽ��׽���
            socket.Disconnect(true);
        }

        void SocketClientManager_OnConnected(string message, bool connected, Socket socket)
        {
            //���Ϸ�����
            this.connected = connected;
        }

        #endregion

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        private TResult GetData<TResult>(IDictionary map, Guid transactionId)
        {
            if (map.Contains(transactionId))
            {
                lock (map)
                {
                    if (map.Contains(transactionId))
                    {
                        object retObj = map[transactionId];
                        map.Remove(transactionId);
                        return (TResult)retObj;
                    }
                }
            }

            return default(TResult);
        }
    }
}