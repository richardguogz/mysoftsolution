using MySoft.IoC.Messages;
using MySoft.Logger;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

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

        private Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
        private Semaphore semaphore;
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
            this.semaphore = new Semaphore(node.MaxCaller, node.MaxCaller);

            if (subscribed)
            {
                this.pool = new ServiceRequestPool(1);

                //�������
                pool.Push(new ServiceRequest(this, node, true));
            }
            else
            {
                this.pool = new ServiceRequestPool(node.MaxCaller);

                //����Ϊ100
                for (int i = 0; i < node.MaxCaller; i++)
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
            if (hashtable.ContainsKey(e.MessageId))
            {
                //������Ӧ��Ϣ
                var waitResult = hashtable[e.MessageId] as WaitResult;

                if (e.Error != null)
                    waitResult.Set(e.Error);
                else
                    waitResult.Set(e.Response);
            }
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public virtual ResponseMessage CallService(RequestMessage reqMsg)
        {
            //����һ��������
            semaphore.WaitOne();

            try
            {
                //��ȡ�������
                var reqProxy = pool.Pop();

                try
                {
                    //��ȡ��Ӧ��Ϣ
                    return GetResponse(reqProxy.Send, reqMsg);
                }
                catch (SocketException ex)
                {
                    throw GetException(node, ex);
                }
                finally
                {
                    pool.Push(reqProxy);
                }
            }
            finally
            {
                //�ͷ�һ��������
                semaphore.Release();
            }
        }

        /// <summary>
        /// ��ȡͨѶ�쳣
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Exception GetException(ServerNode node, SocketException ex)
        {
            var message = string.Format("Can't connect to server ({0}:{1})��Server node : {2} -> ({3}) {4}"
                    , node.IP, node.Port, node.Key, ex.ErrorCode, ex.SocketErrorCode);

            return new WarningException(ex.ErrorCode, message);
        }

        /// <summary>
        /// ��ȡ��Ӧ��Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage GetResponse(Action<string, RequestMessage> sender, RequestMessage reqMsg)
        {
            using (var waitResult = new WaitResult(reqMsg))
            {
                //��ϢId
                var messageId = Guid.NewGuid().ToString();

                //����ź�������
                hashtable[messageId] = waitResult;

                try
                {
                    //��������
                    sender(messageId, reqMsg);

                    var timeout = TimeSpan.FromSeconds(node.Timeout);

                    //�ȴ��ź���Ӧ
                    if (!waitResult.WaitOne(timeout))
                    {
                        throw GetTimeoutException(reqMsg, timeout);
                    }

                    //������Ӧ����Ϣ
                    return waitResult.Response;
                }
                finally
                {
                    //�Ƴ��ź�������
                    hashtable.Remove(messageId);
                }
            }
        }

        /// <summary>
        /// ��ȡ��ʱ��Ӧ��Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private Exception GetTimeoutException(RequestMessage reqMsg, TimeSpan timeout)
        {
            var title = string.Format("��{0}:{1}�� => Remote service ({2}, {3}) invoke timeout ({4}) ms.\r\nParameters => {5}"
               , node.IP, node.Port, reqMsg.ServiceName, reqMsg.MethodName, (int)timeout.TotalMilliseconds, reqMsg.Parameters.ToString());

            //��ȡ�쳣
            return new TimeoutException(title);
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