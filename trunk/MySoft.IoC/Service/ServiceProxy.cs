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
        private IServiceLog logger;
        private SocketClientConfiguration config;
        private ServiceMessagePool requestPool;
        private SmartThreadPool pool;

        public ServiceProxy(IServiceLog logger, SocketClientConfiguration config, string displayName)
        {
            this.logger = logger;
            this.config = config;

            #region socketͨѶ

            //ʵ�����̳߳�
            pool = new SmartThreadPool(30 * 1000, 500, 10);

            //ʵ���������
            requestPool = new ServiceMessagePool(config.Pools);
            for (int i = 0; i < config.Pools; i++)
            {
                var request = new ServiceMessage(displayName, config.IP, config.Port);
                request.SendCallback += new ServiceMessageEventHandler(client_SendMessage);

                //�������ջ
                requestPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(object sender, ServiceMessageEventArgs message)
        {
            //��SocketRequest��ջ
            requestPool.Push(sender as ServiceMessage);

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
        public ResponseMessage CallMethod(RequestMessage reqMsg, double logtime)
        {
            //�����Ϊ��
            if (requestPool.Count == 0)
            {
                throw new Exception("Service pool is empty��");
            }

            //�ӳ��е���һ����������
            var request = requestPool.Pop();

            //�������ݰ��������
            bool isSend = request.Send(reqMsg);

            if (isSend)
            {
                //��ʼ��ʱ
                Stopwatch watch = Stopwatch.StartNew();

                //��ȡ��Ϣ
                ResponseMessage resMsg = GetResponse(reqMsg, watch);

                if (resMsg == null)
                {
                    throw new IoCException(string.Format("��{4}��Call ({0}:{1}) remote service ({2},{3}) failure. result is empty��", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.TransactionId));
                }

                //������ݲ�Ϊ��
                if (resMsg.Data != null)
                {
                    watch.Stop();

                    //���ʱ�䳬��Ԥ�����������־
                    if (watch.ElapsedMilliseconds > logtime * 1000)
                    {
                        //SerializationManager.Serialize(retMsg)
                        logger.WriteLog(string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). ==> {5} <==> {6} \r\nParameters ==> {4}", config.IP, config.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData,
                            "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId), LogType.Warning);
                    }
                }

                return resMsg;
            }
            else
            {
                throw new IoCException(string.Format("Send data to ({0}:{1}) failure��", config.IP, config.Port));
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

                throw new IoCException(string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId));
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
    }
}