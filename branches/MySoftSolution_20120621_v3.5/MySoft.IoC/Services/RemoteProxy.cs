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
        private IDictionary<string, WaitResult> hashtable = new Dictionary<string, WaitResult>();

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
        /// <param name="e"></param>
        protected virtual void OnMessageCallback(object sender, ServiceMessageEventArgs e)
        {
            if (e.Result is ResponseMessage)
            {
                var resMsg = e.Result as ResponseMessage;

                QueueMessage(e.MessageId, resMsg);
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

                QueueMessage(e.MessageId, resMsg);
            }
        }

        /// <summary>
        /// �����Ϣ������
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="resMsg"></param>
        private void QueueMessage(string messageId, ResponseMessage resMsg)
        {
            lock (hashtable)
            {
                if (hashtable.ContainsKey(messageId))
                {
                    var waitResult = hashtable[messageId];

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

            //��ϢId
            var messageId = reqMsg.TransactionId.ToString();

            try
            {
                //��������
                using (var waitResult = new WaitResult(reqMsg))
                {
                    lock (hashtable)
                    {
                        hashtable[messageId] = waitResult;
                    }

                    //������Ϣ
                    reqProxy.SendRequest(messageId, reqMsg);

                    //�ȴ��ź���Ӧ
                    if (!waitResult.WaitOne(TimeSpan.FromSeconds(node.Timeout)))
                    {
                        return GetTimeoutResponse(reqMsg);
                    }

                    return waitResult.Message;
                }
            }
            finally
            {
                lock (hashtable)
                {
                    //������Ƴ�
                    hashtable.Remove(messageId);
                }

                //�������
                reqPool.Push(reqProxy);
            }
        }

        /// <summary>
        /// ��ȡ��ʱ��Ӧ��Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetTimeoutResponse(RequestMessage reqMsg)
        {
            var title = string.Format("��{0}:{1}�� => Call remote service ({2}, {3}) timeout ({4}) ms.\r\nParameters => {5}"
               , node.IP, node.Port, reqMsg.ServiceName, reqMsg.MethodName, node.Timeout * 1000, reqMsg.Parameters.ToString());

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