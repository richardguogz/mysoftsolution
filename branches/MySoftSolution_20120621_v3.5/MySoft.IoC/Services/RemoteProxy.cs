using System;
using System.Collections;
using MySoft.IoC.Messages;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �������
    /// </summary>
    public class RemoteProxy : IService
    {
        protected ILog logger;
        protected ServerNode node;
        protected ServiceRequestPool reqPool;
        private volatile int poolSize;
        private Hashtable hashtable = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ʵ����RemoteProxy
        /// </summary>
        /// <param name="node"></param>
        /// <param name="logger"></param>
        public RemoteProxy(ServerNode node, ILog logger)
        {
            this.node = node;
            this.logger = logger;

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
            var reqService = new ServiceRequest(node, logger, true);
            reqService.OnCallback += reqService_OnCallback;
            reqService.OnError += reqService_OnError;

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
                var resMsg = new ResponseMessage
                {
                    TransactionId = reqMsg.TransactionId,
                    ReturnType = reqMsg.ReturnType,
                    ServiceName = reqMsg.ServiceName,
                    MethodName = reqMsg.MethodName,
                    Error = error
                };

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// �����Ϣ������
        /// </summary>
        /// <param name="resMsg"></param>
        protected void QueueMessage(ResponseMessage resMsg)
        {
            if (hashtable.ContainsKey(resMsg.TransactionId))
            {
                var waitResult = hashtable[resMsg.TransactionId] as WaitResult;

                //������Ӧ
                waitResult.Set(resMsg);
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
                    hashtable[reqMsg.TransactionId] = waitResult;

                    //��ȡһ������
                    reqProxy = GetServiceRequest();

                    //������Ϣ
                    reqProxy.SendMessage(reqMsg);

                    //�ȴ��ź���Ӧ
                    var elapsedTime = TimeSpan.FromSeconds(node.Timeout);

                    if (!waitResult.Wait(elapsedTime))
                    {
                        throw new WarningException(string.Format("��{0}:{1}�� => Call remote service ({2}, {3}) timeout ({4}) ms.\r\nParameters => {5}"
                           , node.IP, node.Port, reqMsg.ServiceName, reqMsg.MethodName, (int)elapsedTime.TotalMilliseconds, reqMsg.Parameters.ToString()));
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

                //������Ƴ�
                hashtable.Remove(reqMsg.TransactionId);
            }
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
                        //һ���Դ���5�������
                        for (int i = 0; i < 5; i++)
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
        public ServerNode Node
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
                return string.Format("{0}_{1}", typeof(RemoteProxy).FullName, node.Key);
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
        }
    }
}