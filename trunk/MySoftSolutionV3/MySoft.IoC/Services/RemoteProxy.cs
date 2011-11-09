using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �������
    /// </summary>
    public class RemoteProxy : IService, IDisposable
    {
        protected ILog logger;
        protected RemoteNode node;
        protected ServiceRequestPool reqPool;
        private Hashtable hashtable = Hashtable.Synchronized(new Hashtable());

        public RemoteProxy(RemoteNode node, ILog logger)
        {
            this.node = node;
            this.logger = logger;
            this.reqPool = new ServiceRequestPool(node.MaxPool);

            InitRequest();
        }

        protected virtual void InitRequest()
        {
            //��������ػ�
            for (int i = 0; i < node.MaxPool; i++)
            {
                ServiceRequest reqService = new ServiceRequest(node, logger);
                reqService.OnCallback += new EventHandler<ServiceMessageEventArgs>(reqService_OnCallback);

                this.reqPool.Push(reqService);
            }
        }

        /// <summary>
        /// �����Ϣ������
        /// </summary>
        /// <param name="resMsg"></param>
        protected void QueueMessage(ResponseMessage resMsg)
        {
            if (resMsg.Expiration > DateTime.Now)
            {
                hashtable[resMsg.TransactionId] = resMsg;
            }
        }

        void reqService_OnCallback(object sender, ServiceMessageEventArgs args)
        {
            if (args.Result is ResponseMessage)
            {
                var resMsg = args.Result as ResponseMessage;
                QueueMessage(resMsg);
            }

            args = null;
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
                throw new Exception("Service request pool is empty��");
            }

            //�ӳ��е���һ����������
            var reqService = reqPool.Pop();

            try
            {
                //������Ϣ
                reqService.SendMessage(reqMsg);

                //��ʼ��ʱ
                Stopwatch watch = Stopwatch.StartNew();

                //��ȡ��Ϣ
                AsyncMethodCaller handler = new AsyncMethodCaller(GetResponse);

                //�첽����
                IAsyncResult ar = handler.BeginInvoke(OperationContext.Current, reqMsg, r => { }, handler);

                // Wait for the WaitHandle to become signaled.
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(reqMsg.Timeout)))
                {
                    watch.Stop();

                    string title = string.Format("Call ({0}:{1}) remote service ({2},{3}) timeout.", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName);
                    string body = string.Format("��{5}��Call ({0}:{1}) remote service ({2},{3}) timeout ({4} ms)��", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId);
                    throw new WarningException(body)
                    {
                        ApplicationName = reqMsg.AppName,
                        ServiceName = reqMsg.ServiceName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                }

                // Perform additional processing here.
                // Call EndInvoke to retrieve the results.
                ResponseMessage resMsg = handler.EndInvoke(ar);

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
                        ServiceName = reqMsg.ServiceName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };

                    logger.WriteError(exception);
                }

                return resMsg;
            }
            finally
            {
                this.reqPool.Push(reqService);
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ����Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetResponse(OperationContext context, RequestMessage reqMsg)
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
        /// Զ�̽ڵ�
        /// </summary>
        public RemoteNode Node
        {
            get { return node; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual string ServiceName
        {
            get
            {
                return typeof(RemoteProxy).FullName;
            }
        }

        #endregion

        /// <summary>
        /// ������Դ
        /// </summary>
        public void Dispose()
        {
            try
            {
                while (reqPool.Count > 0)
                {
                    var reqService = reqPool.Pop();
                    reqService.Dispose();
                }
            }
            catch (Exception)
            {
            }

            this.reqPool = null;

            GC.SuppressFinalize(this);
        }
    }
}