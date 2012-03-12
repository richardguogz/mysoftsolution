using System;
using System.Collections;
using System.Threading;
using MySoft.IoC.Messages;
using MySoft.Logger;
using System.Collections.Generic;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// 服务代理
    /// </summary>
    public class RemoteProxy : IService, IDisposable
    {
        protected ILog logger;
        protected RemoteNode node;
        protected ServiceRequestPool reqPool;
        private WaitResultCollection hashtable;

        public RemoteProxy(RemoteNode node, ILog logger)
        {
            this.node = node;
            this.logger = logger;
            this.hashtable = new WaitResultCollection();

            InitRequest();
        }

        /// <summary>
        /// 初始化请求
        /// </summary>
        protected virtual void InitRequest()
        {
            this.reqPool = new ServiceRequestPool(node.MaxPool);

            lock (this.reqPool)
            {
                //服务请求池化
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

        protected void QueueError(RequestMessage request, Exception error)
        {
            if (request != null)
            {
                var resMsg = new ResponseMessage
                {
                    TransactionId = request.TransactionId,
                    ServiceName = request.ServiceName,
                    MethodName = request.Message,
                    ReturnType = request.ReturnType,
                    Error = error
                };

                QueueMessage(resMsg);
            }
        }

        /// <summary>
        /// 添加消息到队列
        /// </summary>
        /// <param name="resMsg"></param>
        protected void QueueMessage(ResponseMessage resMsg)
        {
            if (hashtable.ContainsKey(resMsg.TransactionId))
            {
                var value = hashtable[resMsg.TransactionId];
                value.Message = resMsg;

                //数据响应
                value.Reset.Set();
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
        /// 调用方法
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            //获取一个请求
            var reqProxy = reqPool.Pop();

            try
            {
                //发送消息
                reqProxy.SendMessage(reqMsg);

                //处理数据
                var waitResult = new WaitResult();
                hashtable[reqMsg.TransactionId] = waitResult;

                var elapsedTime = TimeSpan.FromSeconds(node.Timeout);
                ResponseMessage resMsg = null;

                //等待信号响应
                if (waitResult.Reset.WaitOne(elapsedTime))
                {
                    var value = hashtable[reqMsg.TransactionId];
                    resMsg = value.Message;
                }

                //用完后移除
                hashtable.Remove(reqMsg.TransactionId);

                return resMsg;
            }
            finally
            {
                this.reqPool.Push(reqProxy);
            }
        }

        #region IService 成员

        /// <summary>
        /// 远程节点
        /// </summary>
        public RemoteNode Node
        {
            get { return node; }
        }

        /// <summary>
        /// 服务名称
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
        /// 清理资源
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