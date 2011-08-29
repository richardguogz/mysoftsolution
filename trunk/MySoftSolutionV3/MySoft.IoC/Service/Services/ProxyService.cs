using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.IoC.Message;
using MySoft.IoC.Services;
using MySoft.Logger;
using MySoft.Threading;
using MySoft.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// �������
    /// </summary>
    public class ProxyService : IService, IDisposable
    {
        private ILog logger;
        private RemoteNode node;
        private ServiceMessagePool reqPool;
        private Hashtable hashtable = Hashtable.Synchronized(new Hashtable());

        public ProxyService(ILog logger, RemoteNode node, int bufferSize)
        {
            this.logger = logger;
            this.node = node;

            #region socketͨѶ

            //ʵ���������
            reqPool = new ServiceMessagePool(node.MaxPool);
            for (int i = 0; i < node.MaxPool; i++)
            {
                var request = new ServiceMessage(node, logger, bufferSize);
                request.SendCallback += new ServiceMessageEventHandler(client_SendMessage);

                //�������ջ
                reqPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(object sender, ServiceMessageEventArgs seviceMsg)
        {
            var resMsg = seviceMsg.Result;
            if (resMsg.Expiration > DateTime.Now)
            {
                //���ݽ�����뵽������
                hashtable[resMsg.TransactionId] = resMsg;
            }

            seviceMsg = null;
        }

        /// <summary>
        /// ��ȡ�����Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetMessageKey(RequestBase value)
        {
            return string.Format("Message_{0}", value.TransactionId);
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="logTimeout"></param>
        /// <returns></returns>
        public ResponseMessage CallService(RequestMessage reqMsg, double logTimeout)
        {
            //�����Ϊ��
            if (reqPool.Count == 0)
            {
                throw new Exception("Service pool is empty��");
            }

            //�ӳ��е���һ����������
            var reqService = reqPool.Pop();

            try
            {
                DataPacket packet = new DataPacket
                {
                    PacketID = reqMsg.TransactionId,
                    PacketObject = reqMsg
                };

                //�������ݰ��������
                reqService.Send(packet, TimeSpan.FromSeconds(reqMsg.Timeout));

                //��ʼ��ʱ
                Stopwatch watch = Stopwatch.StartNew();

                //��ȡ��Ϣ
                AsyncMethodCaller caller = new AsyncMethodCaller(GetResponse);

                //�첽����
                IAsyncResult result = caller.BeginInvoke(reqMsg, null, null);

                ResponseMessage resMsg = null;

                // Wait for the WaitHandle to become signaled.
                if (!result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(reqMsg.Timeout)))
                {
                    result.AsyncWaitHandle.Close();
                    watch.Stop();

                    string title = string.Format("Call ({0}:{1}) remote service ({2},{3}) failure.", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName);
                    string body = string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)��", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId);
                    throw new WarningException(body)
                    {
                        ApplicationName = reqMsg.AppName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                }
                else
                {
                    // Perform additional processing here.
                    // Call EndInvoke to retrieve the results.
                    resMsg = caller.EndInvoke(result);
                }

                watch.Stop();

                //���ʱ�䳬��Ԥ�����������־
                if (watch.ElapsedMilliseconds > logTimeout * 1000)
                {
                    //SerializationManager.Serialize(retMsg)
                    string log = string.Format("��{7}��Call ({0}:{1}) remote service ({2},{3}). {5}\r\nMessage ==> {6}\r\nParameters ==> {4}", node.IP, node.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId);
                    string title = string.Format("Elapsed time ({0}) ms more than ({1}) ms.", watch.ElapsedMilliseconds, logTimeout * 1000);
                    string body = string.Format("{0} {1}", title, log);
                    var exception = new WarningException(body)
                    {
                        ApplicationName = reqMsg.AppName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                    logger.WriteError(exception);
                }

                return resMsg;
            }
            finally
            {
                //��SocketRequest��ջ
                reqPool.Push(reqService);
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ����Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetResponse(RequestMessage reqMsg)
        {
            //�����߳���
            while (true)
            {
                var resMsg = hashtable[reqMsg.TransactionId] as ResponseMessage;

                //��������ݷ��أ�����Ӧ
                if (resMsg != null)
                {
                    //������Ƴ�
                    hashtable.Remove(reqMsg.TransactionId);
                    return resMsg;
                }

                //��ֹcpuʹ���ʹ���
                Thread.Sleep(1);
            }
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

        /// <summary>
        /// ������Դ
        /// </summary>
        public void Dispose()
        {
            try
            {
                for (int i = 0; i < reqPool.Count; i++)
                {
                    ServiceMessage args = reqPool.Pop();
                    args.Dispose();
                }
            }
            catch (Exception)
            {
            }

            GC.SuppressFinalize(this);
        }
    }
}