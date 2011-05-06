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
    internal class ServiceProxy : IServiceProxy
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private ServiceRequestPool<ResponseMessage> requestPool;
        private string displayName;
        private SmartThreadPool pool;

        public ServiceProxy(SocketClientConfiguration config, string displayName)
        {
            this.config = config;
            this.displayName = displayName;

            #region socketͨѶ

            //ʵ�����̳߳�
            pool = new SmartThreadPool(30 * 1000, 100, 10);

            //ʵ���������
            requestPool = new ServiceRequestPool<ResponseMessage>(config.Pools);
            for (int i = 0; i < config.Pools; i++)
            {
                var request = new ServiceRequest<ResponseMessage>(config.IP, config.Port);
                request.SendCallback += new SendMessageEventHandler<ResponseMessage>(client_SendMessage);

                //�������ջ
                requestPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(ServiceRequestEventArgs<ResponseMessage> message)
        {
            //���δ���ڣ�����������
            if (message.Response.Expiration >= DateTime.Now)
            {
                lock (responses)
                {
                    var response = message.Response;
                    responses[response.TransactionId] = response;
                }
            }
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="logtime"></param>
        /// <returns></returns>
        public ResponseMessage CallMethod(RequestMessage reqMsg, int logtime)
        {
            //�����Ϊ�գ�����
            if (requestPool.Count == 0)
            {
                CheckPool(reqMsg.Timeout);
            }

            var request = requestPool.Pop();
            if (request == null) return null;

            try
            {
                //������ӶϿ���ֱ���׳��쳣
                if (!request.Connected)
                {
                    throw new IoCException(string.Format("Can't connect to server ({0}:{1})��service: {2}", config.IP, config.Port, displayName));
                }

                //�������ݰ��������
                bool isSend = request.Send(BufferFormat.FormatFCA(reqMsg));

                if (isSend)
                {
                    //��ʼ��ʱ
                    int t1 = System.Environment.TickCount;

                    //��ȡ��Ϣ
                    ResponseMessage resMsg = GetResponse(reqMsg, t1);

                    if (resMsg == null)
                    {
                        throw new IoCException(string.Format("��{4}��Call ({0}:{1}) remote service ({2},{3}) failure. result is empty��", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.TransactionId));
                    }

                    //����������쳣�����׳��쳣
                    if (resMsg.Exception != null)
                    {
                        throw resMsg.Exception;
                    }

                    int t2 = System.Environment.TickCount - t1;

                    //���ʱ�䳬��Ԥ�����������־
                    if (t2 > logtime)
                    {
                        //SerializationManager.Serialize(retMsg)
                        if (OnLog != null) OnLog(string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). ==> {4} {5} <==> {6}", config.IP, config.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters,
                            "Spent time: (" + t2.ToString() + ") ms.", resMsg.Message, resMsg.TransactionId), LogType.Warning);
                    }

                    return resMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure��", config.IP, config.Port));
                }
            }
            finally
            {
                //��SocketRequest��ջ
                requestPool.Push(request);
            }
        }

        private ResponseMessage GetResponse(RequestMessage reqMsg, int beginTick)
        {
            //�����߳�����������
            IWorkItemResult<ResponseMessage> wir = pool.QueueWorkItem(state =>
            {
                ResponseMessage resMsg = null;
                RequestMessage request = state as RequestMessage;

                while (true)
                {
                    resMsg = GetData<ResponseMessage>(responses, request.TransactionId);

                    //��������ݷ��أ�����Ӧ
                    if (resMsg != null) break;

                    //��ֹcpuʹ���ʹ���
                    Thread.Sleep(1);
                }

                return resMsg;

            }, reqMsg);

            if (!pool.WaitForIdle(reqMsg.Timeout))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                int timeout = System.Environment.TickCount - beginTick;
                throw new IoCException(string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, timeout, reqMsg.TransactionId));
            }

            //���̻߳�ȡ������Ϣ����ʱ�ȴ�
            return wir.GetResult();
        }

        private void CheckPool(int timeout)
        {
            //�����߳�����������
            IWorkItemResult wir = pool.QueueWorkItem(() =>
            {
                while (requestPool.Count == 0)
                {
                    //��ֹcpuʹ���ʹ���
                    Thread.Sleep(1);
                }
            });

            //�ȴ�1�룬���û�гؿ��ã��򷵻�
            if (!pool.WaitForIdle(timeout))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                throw new IoCException("Socket pool is empty��");
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

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