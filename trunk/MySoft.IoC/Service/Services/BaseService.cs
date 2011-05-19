using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
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
        public ResponseMessage CallService(RequestMessage reqMsg, double logtime)
        {
            Stopwatch watch = Stopwatch.StartNew();

            //运行请求获得结果
            ResponseMessage resMsg = Run(reqMsg);

            if (resMsg != null && resMsg.Exception != null)
            {
                watch.Stop();

                var ex = resMsg.Exception;
                var exception = new IoCException(string.Format("【{5}】Dynamic ({0}) service ({1},{2}) error. {4}\r\nParameters ==> {3}", reqMsg.Message, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.TransactionId), ex)
                {
                    ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress())
                };
                logger.WriteError(exception);
            }
            else
            {
                watch.Stop();

                //如果时间超过预定，则输出日志
                if (watch.ElapsedMilliseconds > logtime * 1000)
                {
                    string log = string.Format("【{6}】Dynamic ({0}) service ({1},{2}). {4}\r\nMessage ==> {5}\r\nParameters ==> {3}", reqMsg.Message, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId);
                    log = string.Format("Elapsed time more than {0} ms, {1}", logtime * 1000, log);
                    logger.WriteLog(log, LogType.Warning);
                }
            }

            return resMsg;
        }

        #endregion
    }
}
