using System;
using System.Collections.Generic;
using MySoft.IoC.Communication;
using MySoft.IoC.Messages;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �������
    /// </summary>
    public class RemoteProxy : IService, IServerConnect, IDisposable
    {
        #region IServerConnect ��Ա

        /// <summary>
        /// ���ӷ�����
        /// </summary>
        public event EventHandler<ConnectEventArgs> OnConnected;

        /// <summary>
        /// �Ͽ�������
        /// </summary>
        public event EventHandler<ConnectEventArgs> OnDisconnected;

        #endregion

        private IDictionary<Guid, WaitResult> hashtable = new Dictionary<Guid, WaitResult>();

        protected ServiceRequestPool reqPool;
        private volatile int poolSize;
        protected IServiceContainer container;
        protected ServerNode node;

        /// <summary>
        /// ʵ����RemoteProxy
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public RemoteProxy(ServerNode node, IServiceContainer container)
        {
            this.node = node;
            this.container = container;

            InitServiceRequest();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        protected virtual void InitServiceRequest()
        {
            this.reqPool = new ServiceRequestPool(node.MaxPool);

            lock (this.reqPool)
            {
                this.poolSize = node.MinPool;

                //��������ػ���ʹ����С�ĳس�ʼ��
                for (int i = 0; i < node.MinPool; i++)
                {
                    this.reqPool.Push(CreateServiceRequest());
                }
            }
        }

        /// <summary>
        /// ����һ������������
        /// </summary>
        /// <returns></returns>
        private ServiceRequest CreateServiceRequest()
        {
            var reqService = new ServiceRequest(node, container, false);
            reqService.OnCallback += reqService_OnCallback;
            reqService.OnError += reqService_OnError;
            reqService.OnConnected += (sender, args) =>
            {
                if (OnConnected != null) OnConnected(sender, args);
            }; ;

            reqService.OnDisconnected += (sender, args) =>
            {
                if (OnDisconnected != null) OnDisconnected(sender, args);
            }; ;

            return reqService;
        }

        void reqService_OnError(object sender, ErrorMessageEventArgs e)
        {
            QueueError(e.Request, e.Error);
        }

        protected void QueueError(RequestMessage reqMsg, Exception error)
        {
            if (reqMsg != null)
            {
                var resMsg = IoCHelper.GetResponse(reqMsg, error);

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// ������Ϣ������
        /// </summary>
        /// <param name="resMsg"></param>
        protected void QueueMessage(ResponseMessage resMsg)
        {
            lock (hashtable)
            {
                if (hashtable.ContainsKey(resMsg.TransactionId))
                {
                    var waitResult = hashtable[resMsg.TransactionId];

                    //������Ӧ
                    waitResult.SetResponse(resMsg);
                }
            }
        }

        void reqService_OnCallback(object sender, ServiceMessageEventArgs args)
        {
            if (args.Result is ResponseMessage)
            {
                var resMsg = args.Result as ResponseMessage;

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public virtual ResponseMessage CallService(RequestMessage reqMsg)
        {
            //��ȡһ������
            ServiceRequest reqProxy = null;

            try
            {
                //��������
                using (var waitResult = new WaitResult(reqMsg))
                {
                    lock (hashtable)
                    {
                        hashtable[reqMsg.TransactionId] = waitResult;
                    }

                    //��ȡһ������
                    reqProxy = GetServiceRequest();

                    //������Ϣ
                    reqProxy.SendMessage(reqMsg);

                    var elapsedTime = TimeSpan.FromSeconds(node.Timeout);

                    //�ȴ��ź���Ӧ
                    if (!waitResult.WaitOne(elapsedTime))
                    {
                        return GetTimeoutResponse(reqMsg, (int)elapsedTime.TotalMilliseconds);
                    }

                    return waitResult.Message;
                }
            }
            finally
            {
                //�������
                if (reqProxy != null)
                {
                    reqPool.Push(reqProxy);
                }

                lock (hashtable)
                {
                    //������Ƴ�
                    hashtable.Remove(reqMsg.TransactionId);
                }
            }
        }

        /// <summary>
        /// ��ȡ��ʱ��Ӧ��Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        private ResponseMessage GetTimeoutResponse(RequestMessage reqMsg, int elapsedTime)
        {
            var title = string.Format("��{0}:{1}�� => Call remote service ({2}, {3}) timeout ({4}) ms.\r\nParameters => {5}"
               , node.IP, node.Port, reqMsg.ServiceName, reqMsg.MethodName, elapsedTime, reqMsg.Parameters.ToString());

            //��ȡ�쳣
            return IoCHelper.GetResponse(reqMsg, new System.TimeoutException(title));
        }

        /// <summary>
        /// ��ȡһ����������
        /// </summary>
        /// <returns></returns>
        protected virtual ServiceRequest GetServiceRequest()
        {
            var reqProxy = reqPool.Pop();

            if (reqProxy == null)
            {
                if (poolSize < node.MaxPool)
                {
                    lock (reqPool)
                    {
                        //һ���Դ���10�������
                        for (int i = 0; i < 10; i++)
                        {
                            if (poolSize < node.MaxPool)
                            {
                                poolSize++;

                                //����һ���µ�����
                                reqPool.Push(CreateServiceRequest());
                            }
                        }

                        //���Ӻ��ٴӳ��ﵯ��һ��
                        reqProxy = reqPool.Pop();
                    }
                }
                else
                {
                    throw new WarningException(string.Format("Service request pool beyond the {0} limit.", node.MaxPool));
                }
            }

            return reqProxy;
        }

        #region IService ��Ա

        /// <summary>
        /// Զ�̽ڵ�
        /// </summary>
        public ServerNode Node { get { return node; } }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual string ServiceName
        {
            get
            {
                return string.Format("{0}_{1}", typeof(RemoteProxy).FullName, node.Key);
            }
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// Disposes this object and closes underlying connection.
        /// </summary>
        public void Dispose()
        {
            while (reqPool.Count > 0)
            {
                var req = reqPool.Pop();

                try
                {
                    req.Dispose();
                }
                catch (Exception ex)
                {
                }
            }

            hashtable.Clear();
        }

        #endregion
    }
}