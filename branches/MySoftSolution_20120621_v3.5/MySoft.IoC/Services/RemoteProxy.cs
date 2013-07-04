using MySoft.IoC.Messages;
using MySoft.Logger;
using System;
using System.Collections.Generic;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �������
    /// </summary>
    public class RemoteProxy : IService, IServerConnect, IServiceCallback
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

        private IDictionary<string, WaitResult> hashtable;
        private ServiceRequestPool pool;
        private ServerNode node;
        private ILog logger;

        /// <summary>
        /// ʵ����RemoteProxy
        /// </summary>
        /// <param name="node"></param>
        /// <param name="logger"></param>
        /// <param name="subscribed"></param>
        public RemoteProxy(ServerNode node, ILog logger, bool subscribed)
        {
            this.node = node;
            this.logger = logger;
            this.hashtable = new Dictionary<string, WaitResult>();

            if (subscribed)
            {
                this.pool = new ServiceRequestPool(1);

                //�������
                pool.Push(new ServiceRequest(this, node, true));
            }
            else
            {
                this.pool = new ServiceRequestPool(node.MaxPool);

                //����Ϊ100
                for (int i = 0; i < node.MaxPool; i++)
                {
                    //�������
                    pool.Push(new ServiceRequest(this, node, false));
                }
            }
        }

        /// <summary>
        /// ���ӳɹ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Connected(object sender, ConnectEventArgs e)
        {
            if (OnConnected != null) OnConnected(sender, e);
        }

        /// <summary>
        /// �Ͽ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Disconnected(object sender, ConnectEventArgs e)
        {
            if (OnDisconnected != null) OnDisconnected(sender, e);
        }

        /// <summary>
        /// ��Ϣ�ص�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MessageCallback(object sender, CallbackMessageEventArgs e)
        {
            //TODO
        }

        /// <summary>
        /// ��Ϣ�ص�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MessageCallback(object sender, ResponseMessageEventArgs e)
        {
            lock (hashtable)
            {
                if (hashtable.ContainsKey(e.MessageId))
                {
                    hashtable[e.MessageId].Set(e.Message);
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
            var reqProxy = pool.Pop();

            if (reqProxy == null)
            {
                throw new WarningException("Service request exceeds the maximum number of pool.");
            }

            try
            {
                //��ϢId
                var messageId = reqMsg.TransactionId.ToString();

                using (var waitResult = new WaitResult())
                {
                    //����ź�������
                    lock (hashtable) hashtable[messageId] = waitResult;

                    try
                    {
                        //������Ϣ
                        reqProxy.SendRequest(messageId, reqMsg);

                        //�ȴ��ź���Ӧ
                        if (!waitResult.WaitOne(TimeSpan.FromSeconds(node.Timeout)))
                        {
                            return GetTimeoutResponse(reqMsg);
                        }

                        //������Ӧ����Ϣ
                        return waitResult.Message;
                    }
                    finally
                    {
                        //�Ƴ��ź�������
                        lock (hashtable) hashtable.Remove(messageId);
                    }
                }
            }
            finally
            {
                pool.Push(reqProxy);
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

        /// <summary>
        /// Gets the enable cache of the service.
        /// </summary>
        public bool EnableCache { get { return false; } }

        #endregion
    }
}