using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MySoft.Core;
using System.Net.Sockets;

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
                List<Guid> clientIdList = new List<Guid>(handlers.Keys);
                Random random = new Random();
                int start = random.Next(clientIdList.Count);
                for (int i = 0; i < clientIdList.Count; i++)
                {
                    Guid tempClientId = clientIdList[(i + start) % clientIdList.Count];
                    ServiceRequestNotifyHandler tempHandler = handlers[tempClientId];
                    if (tempHandler != null)
                    {
                        string log = "Notify service host: (" + reqMsg.ServiceName + ")[" + tempClientId.ToString() + "].";
                        try
                        {
                            IService service = ((Services.MessageRequestCallbackHandler)tempHandler.Target).Service;
                            if (OnLog != null) OnLog(log);
                            tempHandler(reqMsg);

                            //if calling ok, skip other subscribers, easily exit loop
                            break;
                        }
                        catch (SocketException ex)  //如果socket错误，表示连接失败
                        {
                            string error = "Notify service host: (" + reqMsg.ServiceName + ")[" + tempClientId.ToString() + "] shutdown! Reason: " + ex.Message;
                            if (OnLog != null) OnLog(error);

                            var exception = new IoCException(log + "\r\n" + error, ex);
                            if (OnError != null) OnError(exception);

                            handlers[tempClientId] = null;
                            needCleanHandlers = true;
                        }
                        catch (Exception ex)
                        {
                            string error = "Notify service host: (" + reqMsg.ServiceName + ")[" + tempClientId.ToString() + "] error! Reason: " + ex.Message;
                            if (OnLog != null) OnLog(error);

                            var exception = new IoCException(log + "\r\n" + error, ex);
                            if (OnError != null) OnError(exception);
                        }
                    }
                    else
                    {
                        needCleanHandlers = true;
                    }
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
                string error = "Call service " + reqMsg.ServiceName + " error. have not any subscriber!";
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
