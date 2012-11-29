using System;
using System.Collections.Generic;
using MySoft.IoC.Messages;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �������
    /// </summary>
    public class RemoteProxy : IService, IServerConnect
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

        /// <summary>
        /// �������
        /// </summary>
        private IDictionary<Guid, WaitResult> hashtable = new Dictionary<Guid, WaitResult>();

        protected ServiceRequestPool reqPool;
        private volatile int poolSize;
        private ILog logger;
        private ServerNode node;

        /// <summary>
        /// ʵ����RemoteProxy
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
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
                    this.reqPool.Push(CreateServiceRequest(false));
                }
            }
        }

        /// <summary>
        /// ����һ������������
        /// </summary>
        /// <param name="subscribed"></param>
        /// <returns></returns>
        protected ServiceRequest CreateServiceRequest(bool subscribed)
        {
            var reqService = new ServiceRequest(node, subscribed);
            reqService.OnCallback += OnMessageCallback;
            reqService.OnError += OnMessageError;
            reqService.OnConnected += (sender, args) =>
            {
                if (OnConnected != null) OnConnected(sender, args);
            };

            reqService.OnDisconnected += (sender, args) =>
            {
                if (OnDisconnected != null) OnDisconnected(sender, args);
            };

            return reqService;
        }

        /// <summary>
        /// ��Ϣ�ص�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnMessageCallback(object sender, ServiceMessageEventArgs args)
        {
            if (args.Result is ResponseMessage)
            {
                var resMsg = args.Result as ResponseMessage;

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// �쳣����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMessageError(object sender, ErrorMessageEventArgs e)
        {
            if (e.Request != null)
            {
                var resMsg = IoCHelper.GetResponse(e.Request, e.Error);

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// �����Ϣ������
        /// </summary>
        /// <param name="resMsg"></param>
        private void QueueMessage(ResponseMessage resMsg)
        {
            lock (hashtable)
            {
                if (hashtable.ContainsKey(resMsg.TransactionId))
                {
                    var waitResult = hashtable[resMsg.TransactionId];

                    //������Ӧ
                    waitResult.Set(resMsg);
                }
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
            var reqProxy = GetServiceRequest();

            try
            {
                return GetResponseMessage(reqProxy, reqMsg);
            }
            finally
            {
                //�������
                reqPool.Push(reqProxy);
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ��Ϣ
        /// </summary>
        /// <param name="reqProxy"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetResponseMessage(ServiceRequest reqProxy, RequestMessage reqMsg)
        {
            //��������
            using (var waitResult = new WaitResult(reqMsg))
            {
                try
                {
                    lock (hashtable)
                    {
                        hashtable[reqMsg.TransactionId] = waitResult;
                    }

                    //������Ϣ
                    reqProxy.SendRequest(reqMsg);

                    var elapsedTime = TimeSpan.FromSeconds(node.Timeout);

                    //�ȴ��ź���Ӧ
                    if (!waitResult.WaitOne(elapsedTime))
                    {
                        return GetTimeoutResponse(reqMsg, (int)elapsedTime.TotalMilliseconds);
                    }

                    return waitResult.Message;
                }
                finally
                {
                    lock (hashtable)
                    {
                        //������Ƴ�
                        hashtable.Remove(reqMsg.TransactionId);
                    }
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
            return IoCHelper.GetResponse(reqMsg, new TimeoutException(title));
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
                                reqPool.Push(CreateServiceRequest(false));
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
                return string.Format("{0}${1}", typeof(RemoteProxy).FullName, node);
            }
        }

        #endregion
    }
}