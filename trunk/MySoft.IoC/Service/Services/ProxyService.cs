using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Threading;
using System.Linq;
using MySoft.IoC.Services;

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

            //�����߳���������ڵ�����
            ThreadPool.QueueUserWorkItem((state) =>
            {
                //һ�������һ��
                Thread.Sleep(TimeSpan.FromMinutes(1));

                //����������0��
                if (responses.Count > 0)
                {
                    lock (responses)
                    {
                        //�����ڵ������Ƴ���
                        var keys = responses.Where(p => p.Value.Expiration < DateTime.Now).Select(p => p.Key).ToArray();
                        foreach (var key in keys)
                        {
                            responses.Remove(key);
                        }
                    }
                }
            });

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
            lock (responses)
            {
                //���ݽ�����뵽������
                responses[response.TransactionId] = response;
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
                bool isSend = request.Send(reqMsg, TimeSpan.FromSeconds(reqMsg.Timeout));

                if (isSend)
                {
                    //��ʼ��ʱ
                    Stopwatch watch = Stopwatch.StartNew();

                    //��ȡ��Ϣ
                    AsyncMethodCaller caller = new AsyncMethodCaller(GetResponse);

                    //�첽����
                    IAsyncResult result = caller.BeginInvoke(reqMsg, null, null);

                    // Wait for the WaitHandle to become signaled.
                    if (!result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(reqMsg.Timeout)))
                    {
                        watch.Stop();

                        throw new WarningException(string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId))
                        {
                            ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                        };
                    }

                    // Perform additional processing here.
                    // Call EndInvoke to retrieve the results.
                    var resMsg = caller.EndInvoke(result);

                    //������ݲ�Ϊ��
                    if (resMsg.Data != null)
                    {
                        watch.Stop();

                        //���ʱ�䳬��Ԥ�����������־
                        if (watch.ElapsedMilliseconds > logtime * 1000)
                        {
                            //SerializationManager.Serialize(retMsg)
                            string log = string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). {5}\r\nMessage ==> {6}\r\nParameters ==> {4}", node.IP, node.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId);
                            log = string.Format("Elapsed time ({2}) ms more than ({0}) ms, {1}", logtime * 1000, log, watch.ElapsedMilliseconds);
                            var exception = new WarningException(log)
                            {
                                ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                            };
                            logger.WriteError(exception);
                        }
                    }

                    return resMsg;
                }
                else
                {
                    throw new WarningException(string.Format("Send data to ({0}:{1}) failure��", node.IP, node.Port))
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

        /// <summary>
        /// ��ȡ��Ӧ����Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetResponse(RequestMessage reqMsg)
        {
            ResponseMessage resMsg;

            //�����߳���
            while (true)
            {
                resMsg = GetData<ResponseMessage>(responses, reqMsg.TransactionId);

                //��������ݷ��أ�����Ӧ
                if (resMsg != null) break;

                //��ֹcpuʹ���ʹ���
                Thread.Sleep(1);
            }

            return resMsg;
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