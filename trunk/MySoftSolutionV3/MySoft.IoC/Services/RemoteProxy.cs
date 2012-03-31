using System;
using System.Collections;
using System.Threading;
using MySoft.IoC.Messages;
using MySoft.Logger;
using System.Collections.Generic;

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
        private WaitResultCollection hashtable;

        public RemoteProxy(ServerNode node, ILog logger)
        {
            this.node = node;
            this.logger = logger;
            this.hashtable = new WaitResultCollection();

            InitRequest();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        protected virtual void InitRequest()
        {
            this.reqPool = new ServiceRequestPool(node.MaxPool);

            lock (this.reqPool)
            {
                //��������ػ�
                for (int i = 0; i < node.MaxPool; i++)
                {
                    var reqService = new ServiceRequest(node, logger, true);
                    reqService.OnCallback += reqService_OnCallback;
                    reqService.OnError += reqService_OnError;

                    this.reqPool.Push(reqService);
                }
            }
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
                    ServiceName = reqMsg.ServiceName,
                    MethodName = reqMsg.Message,
                    ReturnType = reqMsg.ReturnType,
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
                var waitResult = hashtable[resMsg.TransactionId];

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
            var reqProxy = reqPool.Pop();

            try
            {
                //��������
                var waitResult = new WaitResult();
                hashtable[reqMsg.TransactionId] = waitResult;

                //����ֵ
                var jsonString = reqMsg.Parameters.ToString();
                string queueKey = string.Format("{0}${1}${2}", reqMsg.ServiceName, reqMsg.MethodName, jsonString);
                queueKey = ServiceConfig.FormatJson(queueKey);

                //����Queue

                //������Ϣ
                reqProxy.SendMessage(reqMsg);

                //�ȴ��ź���Ӧ
                var elapsedTime = TimeSpan.FromSeconds(node.Timeout);
                if (!waitResult.Wait(elapsedTime))
                {
                    throw new WarningException(string.Format("��{0}:{1}�� => Call service ({2}, {3}) timeout ({4}) ms.\r\nParameters => {5}"
                       , node.IP, node.Port, reqMsg.ServiceName, reqMsg.MethodName, (int)elapsedTime.TotalMilliseconds, jsonString));
                }

                return waitResult.Message;
            }
            finally
            {
                //�������
                reqPool.Push(reqProxy);

                //������Ƴ�
                hashtable.Remove(reqMsg.TransactionId);
            }
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