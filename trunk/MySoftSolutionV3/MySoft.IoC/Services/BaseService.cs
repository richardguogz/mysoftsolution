using System;
using System.Diagnostics;
using MySoft.IoC.Messages;
using MySoft.Logger;

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
        private ILog logger;

        /// <summary>
        /// The service name.
        /// </summary>
        protected string serviceName;

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public BaseService(ILog logger, string serviceName)
        {
            this.logger = logger;
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected abstract ResponseMessage Run(RequestMessage reqMsg);

        #region IService Members

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            //���������ý��
            ResponseMessage resMsg = Run(reqMsg);

            //�����ҵ���쳣�����׳�����
            if (resMsg.IsError && !resMsg.IsBusinessError)
            {
                var ex = resMsg.Error;
                string body = string.Format("��{5}��Dynamic ({0}) service ({1},{2}) error. \r\nMessage ==> {4}\r\nParameters ==> {3}", reqMsg.Message, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters, resMsg.Message, resMsg.TransactionId);
                var exception = new IoCException(body, ex)
                {
                    ApplicationName = reqMsg.AppName,
                    ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
                logger.WriteError(exception);
            }

            return resMsg;
        }

        #endregion
    }
}
