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
    public abstract class BaseService : IService, ILogable, IErrorLogable
    {
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
        public BaseService(string serviceName)
        {
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected abstract ResponseMessage Run(RequestMessage msg);

        #region IService Members

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        public ResponseMessage CallService(RequestMessage msg, int logtimeout)
        {
            int t1 = System.Environment.TickCount;
            ResponseMessage retMsg = Run(msg);
            if (retMsg != null && retMsg.Exception != null)
            {
                var ex = retMsg.Exception;

                int t2 = System.Environment.TickCount - t1;
                string log = string.Format("【{5}】Dynamic ({0}) service ({1},{2}) error. ==> {3} {4}", retMsg.RequestAddress, retMsg.ServiceName, retMsg.SubServiceName, retMsg.Parameters, "Spent time: (" + t2.ToString() + ") ms.", retMsg.TransactionId);
                var exception = new IoCException(log, ex);

                if (OnError != null) OnError(exception);
            }
            else
            {
                int t2 = System.Environment.TickCount - t1;

                //如果时间超过预定，则输出日志
                if (t2 > logtimeout)
                {
                    if (OnLog != null)
                    {
                        string log = string.Format("【{6}】Dynamic ({0}) service ({1},{2}). ==> {3} {4} <==> {5}", retMsg.RequestAddress, retMsg.ServiceName, retMsg.SubServiceName, retMsg.Parameters, "Spent time: (" + t2.ToString() + ") ms.", retMsg.Message, retMsg.TransactionId);
                        OnLog(log, LogType.Warning);
                    }
                }
            }

            return retMsg;
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        #endregion

        #region IErrorLogable Members

        /// <summary>
        /// OnError event.
        /// </summary>
        public event ErrorLogEventHandler OnError;

        #endregion
    }
}
