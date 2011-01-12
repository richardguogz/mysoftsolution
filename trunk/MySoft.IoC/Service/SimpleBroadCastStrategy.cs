using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Linq;

namespace MySoft.IoC
{
    /// <summary>
    /// SimpleBroadCastStrategy for service request.
    /// </summary>
    public class SimpleBroadCastStrategy : IBroadCastStrategy, ILogable, IErrorLogable
    {
        #region Private Members

        private Dictionary<Guid, ServiceRequestNotifyHandler> RemoveNullHandlers(Dictionary<Guid, ServiceRequestNotifyHandler> handlers)
        {
            Dictionary<Guid, ServiceRequestNotifyHandler> newHandlers = new Dictionary<Guid, ServiceRequestNotifyHandler>();
            foreach (Guid key in handlers.Keys)
            {
                if (handlers[key] != null)
                {
                    newHandlers.Add(key, handlers[key]);
                }
            }
            return newHandlers;
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Does the broad cast.
        /// </summary>
        /// <param name="reqMsg">The req MSG.</param>
        /// <param name="handlers">The handlers.</param>
        /// <returns>Is needed to clean null handlers</returns>
        protected virtual bool DoBroadCast(RequestMessage reqMsg, Dictionary<Guid, ServiceRequestNotifyHandler> handlers)
        {
            bool needCleanHandlers = false;

            if (handlers != null && handlers.Count > 0)
            {
                Random random = new Random();
                KeyValuePair<Guid, ServiceRequestNotifyHandler> temp = handlers.ElementAt(random.Next(handlers.Count));
                Guid tempClientId = temp.Key;
                ServiceRequestNotifyHandler tempHandler = temp.Value;
                if (tempHandler != null)
                {
                    string log = "Notify service host: (" + reqMsg.ServiceName + "," + reqMsg.SubServiceName + ")[" + tempClientId.ToString() + "].";
                    try
                    {
                        IService service = ((Services.MessageRequestCallbackHandler)tempHandler.Target).Service;
                        if (OnLog != null) OnLog(log);
                        tempHandler(reqMsg);
                    }
                    catch (Exception ex)
                    {
                        string error = "Notify service host: (" + reqMsg.ServiceName + "," + reqMsg.SubServiceName + ")[" + tempClientId.ToString() + "] error! Reason: " + ErrorHelper.GetErrorWithoutHtml(ex);
                        if (OnLog != null) OnLog(error);

                        var exception = new IoCException(log + "\r\n" + error);
                        if (OnError != null) OnError(exception);

                        //如果socket错误，表示连接失败
                        if (ex is SocketException)
                        {
                            handlers[tempClientId] = null;
                            needCleanHandlers = true;
                        }
                    }
                }
                else
                {
                    needCleanHandlers = true;
                }
            }

            return needCleanHandlers;
        }

        #endregion

        #region IBroadCastStrategy Members

        /// <summary>
        /// Broads the cast.
        /// </summary>
        /// <param name="reqMsg">The req MSG.</param>
        /// <param name="onServiceRequests">The on service requests.</param>
        public void BroadCast(RequestMessage reqMsg, Dictionary<string, Dictionary<Guid, ServiceRequestNotifyHandler>> onServiceRequests)
        {
            if (reqMsg == null)
            {
                return;
            }

            if (onServiceRequests != null && onServiceRequests.ContainsKey(reqMsg.ServiceName))
            {
                Dictionary<Guid, ServiceRequestNotifyHandler> handlers = onServiceRequests[reqMsg.ServiceName];

                bool needCleanHandlers = DoBroadCast(reqMsg, handlers);

                if (needCleanHandlers)
                {
                    onServiceRequests[reqMsg.ServiceName] = RemoveNullHandlers(handlers);
                }
            }
            else
            {
                string error = "Call service (" + reqMsg.ServiceName + "," + reqMsg.SubServiceName + ") error. have not any subscriber!";
                if (OnLog != null) OnLog(error);

                if (OnError != null) OnError(new IoCException(error));
            }
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
