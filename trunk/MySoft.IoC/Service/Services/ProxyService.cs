using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Threading;

namespace MySoft.IoC
{
    /// <summary>
    /// �������
    /// </summary>
    public class ProxyService : IService
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private ILog logger;
        private RemoteNode node;
        private ServiceMessagePool reqPool;
        private SmartThreadPool pool;

        public ProxyService(ILog logger, RemoteNode node)
        {
            this.logger = logger;
            this.node = node;

            #region socketͨѶ

            //ʵ�����̳߳�
            pool = new SmartThreadPool(30 * 1000, 500, 10);

            //ʵ���������
            reqPool = new ServiceMessagePool(node.MaxPool);
            for (int i = 0; i < node.MaxPool; i++)
            {
                var request = new ServiceMessage(node);
                request.SendCallback += new ServiceMessageEventHandler(client_SendMessage);

                //�������ջ
                reqPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(object sender, ServiceMessageEventArgs message)
        {
            var response = message.Result;

            //���δ���ڣ�����������
            if (response.Expiration >= DateTime.Now)
            {
                lock (responses)
                {
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
        public ResponseMessage CallService(RequestMessage reqMsg, double logtime)
        {
            //�����Ϊ��
            if (reqPool.Count == 0)
            {
                throw new Exception("Service pool is empty��");
            }

            //�ӳ��е���һ����������
            var request = reqPool.Pop();

            try
            {
                //�������ݰ��������
                bool isSend = request.Send(reqMsg, (int)(reqMsg.Timeout * 1000));

                if (isSend)
                {
                    //��ʼ��ʱ
                    Stopwatch watch = Stopwatch.StartNew();

                    //��ȡ��Ϣ
                    var resMsg = GetResponse(reqMsg, watch);

                    //������ݲ�Ϊ��
                    if (resMsg.Data != null)
                    {
                        watch.Stop();

                        //���ʱ�䳬��Ԥ�����������־
                        if (watch.ElapsedMilliseconds > logtime * 1000)
                        {
                            //SerializationManager.Serialize(retMsg)
                            string log = string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). {5}\r\nMessage ==> {6}\r\nParameters ==> {4}", node.IP, node.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId);
                            log = string.Format("Elapsed time more than {0} ms, {1}", logtime * 1000, log);
                            logger.WriteLog(log, LogType.Warning);
                        }
                    }

                    return resMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure��", node.IP, node.Port))
                    {
                        ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                }
            }
            finally
            {
                //��SocketRequest��ջ
                reqPool.Push(request);
            }
        }

        private ResponseMessage GetResponse(RequestMessage reqMsg, Stopwatch watch)
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

            if (!pool.WaitForIdle((int)(reqMsg.Timeout * 1000)))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                watch.Stop();

                throw new IoCException(string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId))
                {
                    ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }

            //���̻߳�ȡ������Ϣ����ʱ�ȴ�
            return wir.GetResult();
        }

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

        #region IService ��Ա

        /// <summary>
        /// ��������
        /// </summary>
        public string ServiceName
        {
            get { return typeof(ProxyService).FullName; }
        }

        #endregion
    }
}