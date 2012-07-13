using System;
using System.Diagnostics;
using System.Threading;
using MySoft.IoC.Messages;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The base class of services.
    /// </summary>
    [Serializable]
    public abstract class BaseService : IService
    {
        /// <summary>
        ///  The service logger
        /// </summary>
        private IServiceContainer logger;
        private Type serviceType;

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName
        {
            get { return serviceType.FullName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public BaseService(IServiceContainer logger, Type serviceType)
        {
            this.logger = logger;
            this.serviceType = serviceType;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected abstract ResponseMessage Run(RequestMessage reqMsg);

        /// <summary>
        /// Dispose
        /// </summary>
        public abstract void Dispose();

        #region IService Members

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            //��ʼ��ʱ
            var watch = Stopwatch.StartNew();

            var resMsg = Run(reqMsg);

            watch.Stop();

            //���㳬ʱ
            resMsg.ElapsedTime = watch.ElapsedMilliseconds;

            if (resMsg.IsError)
            {
                Exception error = resMsg.Error;

                if (error is ThreadAbortException || error is ThreadInterruptedException)
                {
                    string body = string.Format("Remote client��{0}��call service ({1},{2}) error.\r\nParameters => {3}\r\nMessage => {4}",
                          reqMsg.Message, reqMsg.ServiceName, reqMsg.MethodName, reqMsg.Parameters.ToString(),
                            string.Format("Service call timeout {0} ms, the request was aborted initiative. ", watch.ElapsedMilliseconds));

                    //��ȡ�쳣
                    error = IoCHelper.GetException(OperationContext.Current, reqMsg, new ThreadException(body, error));

                    //�߳��첽�����÷���ֵΪnull
                    resMsg = null;
                }
                else
                {
                    string body = string.Format("Remote client��{0}��call service ({1},{2}) error.\r\nParameters => {3}\r\nMessage => {4}",
                        reqMsg.Message, reqMsg.ServiceName, reqMsg.MethodName, reqMsg.Parameters.ToString(), resMsg.Message);

                    //��ȡ�쳣
                    error = IoCHelper.GetException(OperationContext.Current, reqMsg, body, error);
                }

                if (error != null)
                {
                    logger.WriteError(error);
                }
            }

            //���ؽ������
            if (resMsg != null && reqMsg.InvokeMethod)
            {
                resMsg.Value = new InvokeData
                {
                    Value = SerializationManager.SerializeJson(resMsg.Value),
                    Count = resMsg.Count,
                    ElapsedTime = resMsg.ElapsedTime,
                    OutParameters = resMsg.Parameters.ToString()
                };

                //�����������
                resMsg.Parameters.Clear();
            }

            return resMsg;
        }

        #endregion
    }
}
