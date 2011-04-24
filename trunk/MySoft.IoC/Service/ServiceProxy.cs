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

namespace MySoft.IoC
{
    internal class ServiceProxy : ILogable, IServiceProxy
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private SocketClientManager manager;
        private ResponseFormat format = ResponseFormat.Binary;
        private CompressType compress = CompressType.None;
        private bool connected = false;
        private int timeout;

        //threading
        private SmartThreadPool pool;

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public ResponseFormat Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// Gets or sets the compress.
        /// </summary>
        public CompressType Compress
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

        public ServiceProxy(SocketClientConfiguration config)
        {
            this.config = config;

            #region socketͨѶ

            //ʵ�����̳߳�
            pool = new SmartThreadPool(30000, 100, 0);

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);

            //����һ������߳�
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (!connected)
                    {
                        //�������ӵ�������
                        manager.Client.BeginConnectTo(config.IP, config.Port);
                    }

                    //ÿ5����һ��
                    Thread.Sleep(5000);
                }
            });

            thread.IsBackground = true;
            thread.Start();

            #endregion
        }

        public ResponseMessage CallMethod(RequestMessage msg)
        {
            if (connected)
            {
                //�������ʱ��
                msg.Expiration = DateTime.Now.AddMilliseconds(msg.Timeout);

                long t1 = System.Environment.TickCount;

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
                        }

                        return ret;
                    }, msg);

                    //���õȴ���ʱʱ��
                    if (!pool.WaitForIdle(msg.Timeout))
                    {
                        //�����߳�
                        wir.Cancel(true);

                        //��ʱ����
                        throw new IoCException(string.Format("��{4}��Call ({0}:{1}) remote service ({2},{3}) timeout��\r\n", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, msg.TransactionId));
                    }

                    //���̻߳�ȡ������Ϣ
                    ResponseMessage retMsg = wir.GetResult();

                    if (retMsg == null)
                    {
                        throw new IoCException(string.Format("��{4}��Call ({0}:{1}) remote service ({2},{3}) failure.\r\n", config.IP, config.Port, msg.ServiceName, msg.SubServiceName, msg.TransactionId));
                    }
                    else
                    {
                        long t2 = System.Environment.TickCount - t1;
                        if (retMsg.Data is Exception)
                        {
                            throw retMsg.Data as Exception;
                        }

                        //SerializationManager.Serialize(retMsg)
                        if (OnLog != null) OnLog(string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). ==> {4} {5} <==> {6}\r\n", config.IP, config.Port, retMsg.ServiceName, retMsg.SubServiceName, retMsg.Parameters, "Spent time: (" + t2.ToString() + ") ms.", retMsg.Message, retMsg.TransactionId));
                    }

                    return retMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure��", config.IP, config.Port));
                }
            }
            else
            {
                throw new IoCException(string.Format("Server ({0}:{1}) not connected��", config.IP, config.Port));
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