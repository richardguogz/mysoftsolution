using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected abstract ResponseMessage Run(RequestMessage msg);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public BaseService(string serviceName)
        {
            this.serviceName = serviceName;
        }

        #region IService Members

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        public ResponseMessage CallService(RequestMessage msg)
        {
            string log = string.Format("Dynamic service ({0},{1}). -->{2}", serviceName, msg.SubServiceName, msg.Parameters.SerializedData);
            if (OnLog != null) OnLog(log);

            long t1 = System.Environment.TickCount;
            ResponseMessage retMsg = Run(msg);
            if (retMsg != null && retMsg.Data is Exception)
            {
                var ex = retMsg.Data as Exception;

                long t2 = System.Environment.TickCount - t1;
                log += " Spent time: (" + t2.ToString() + ") ms.";
                var exception = new IoCException(log, ex);

                if (OnError != null) OnError(exception);
            }
            else
            {
                long t2 = System.Environment.TickCount - t1;
                log = string.Format("Dynamic service ({0},{1}).-->{2}\r\n{3}", serviceName, msg.SubServiceName,
                    retMsg.Message, "Spent time: (" + t2.ToString() + ") ms.");
                if (OnLog != null) OnLog(log);
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
