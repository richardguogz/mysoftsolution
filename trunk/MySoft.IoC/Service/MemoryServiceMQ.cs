using System;
using System.Collections;
using System.Collections.Generic;
using MySoft.Core;

namespace MySoft.IoC
{
    /// <summary>
    /// A memory service mq impl.
    /// </summary>
    public class MemoryServiceMQ : MarshalByRefObject, IServiceMQ
    {
        #region Private Members

        private Dictionary<Guid, RequestMessage> requests = new Dictionary<Guid, RequestMessage>();
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private Dictionary<string, Dictionary<Guid, ServiceRequestNotifyHandler>> onServiceRequests = new Dictionary<string, Dictionary<Guid, ServiceRequestNotifyHandler>>();
        private IBroadCastStrategy broadCastStrategy;

        private object GetData(IDictionary map, Guid transactionId)
        {
            lock (map)
            {
                if (map.Contains(transactionId))
                {
                    object retObj = map[transactionId];
                    map.Remove(transactionId);
                    return retObj;
                }
                else
                {
                    return null;
                }
            }
        }

        private void BroadCast(RequestMessage reqMsg)
        {
            if (broadCastStrategy != null)
            {
                broadCastStrategy.BroadCast(reqMsg, onServiceRequests);
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Adds the request to queue.
        /// </summary>
        /// <param name="tid">The tid.</param>
        /// <param name="msg">The MSG.</param>
        protected virtual void AddRequestToQueue(Guid tid, RequestMessage msg)
        {
            lock (requests)
            {
                //foreach (Guid key in requests.Keys)
                //{
                //    RequestMessage reqMsg = (RequestMessage)requests[key];
                //    if (reqMsg == null || reqMsg.Expiration < DateTime.Now)
                //    {
                //        requests.Remove(key);
                //    }
                //}

                if (!requests.ContainsKey(tid))
                {
                    requests.Add(tid, msg);
                }
                else
                {
                    requests[tid] = msg;
                }
            }
        }

        /// <summary>
        /// Adds the response to queue.
        /// </summary>
        /// <param name="tid">The tid.</param>
        /// <param name="msg">The MSG.</param>
        protected virtual void AddResponseToQueue(Guid tid, ResponseMessage msg)
        {
            lock (responses)
            {
                //foreach (Guid key in responses.Keys)
                //{
                //    ResponseMessage resMsg = (ResponseMessage)responses[key];
                //    if (resMsg == null || resMsg.Expiration < DateTime.Now)
                //    {
                //        responses.Remove(key);
                //    }
                //}

                if (!responses.ContainsKey(tid))
                {
                    responses.Add(tid, msg);
                }
                else
                {
                    responses[tid] = msg;
                }
            }
        }

        /// <summary>
        /// Gets the request from queue.
        /// </summary>
        /// <param name="tid">The tid.</param>
        /// <returns>The msg.</returns>
        protected virtual RequestMessage GetRequestFromQueue(Guid tid)
        {
            return (RequestMessage)GetData(requests, tid);
        }

        /// <summary>
        /// Gets the response from queue.
        /// </summary>
        /// <param name="tid">The tid.</param>
        /// <returns>The msg.</returns>
        protected virtual ResponseMessage GetResponseFromQueue(Guid tid)
        {
            return (ResponseMessage)GetData(responses, tid);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryServiceMQ"/> class.
        /// </summary>
        public MemoryServiceMQ()
        {
            SimpleBroadCastStrategy strategy = new SimpleBroadCastStrategy();
            strategy.OnLog += new LogEventHandler(strategy_OnLog);
            strategy.OnError += new ErrorLogEventHandler(strategy_OnError);
            this.broadCastStrategy = strategy;
        }

        void strategy_OnError(Exception exception)
        {
            if (OnError != null) OnError(exception);
        }

        void strategy_OnLog(string log)
        {
            if (OnLog != null) OnLog(log);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryServiceMQ"/> class.
        /// </summary>
        /// <param name="broadCastStrategy">The broad cast strategy.</param>
        public MemoryServiceMQ(IBroadCastStrategy broadCastStrategy)
        {
            this.broadCastStrategy = broadCastStrategy;
        }

        #endregion

        #region IServiceMQ Members

        /// <summary>
        /// Sends the request to queue.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns>The id of the msg.</returns>
        public Guid SendRequestToQueue(string serviceName, RequestMessage msg)
        {
            if (msg.TransactionId == default(Guid))
            {
                msg.TransactionId = Guid.NewGuid();
            }

            AddRequestToQueue(msg.TransactionId, msg);

            if (OnLog != null) OnLog(string.Format("AddRequestToQueue({0}:{1},{2}). -->{3}", msg.TransactionId, serviceName, msg.SubServiceName, msg.Parameters.SerializedData));

            BroadCast(msg);

            return msg.TransactionId;
        }

        /// <summary>
        /// Sends the response to queue.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void SendResponseToQueue(ResponseMessage msg)
        {
            if (msg != null)
            {
                AddResponseToQueue(msg.TransactionId, msg);
                if (OnLog != null) OnLog(string.Format("AddResponseToQueue({0}:{1},{2}). -->(result success)", msg.TransactionId, msg.ServiceName, msg.SubServiceName));
            }
        }

        /// <summary>
        /// Receives the request from queue.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns>The msg.</returns>
        public RequestMessage ReceiveRequestFromQueue(Guid transactionId)
        {
            RequestMessage msg = GetRequestFromQueue(transactionId);

            if (msg != null)
            {
                if (OnLog != null) OnLog(string.Format("GetRequestFromQueue({0}:{1},{2}). -->{3}", transactionId, msg.ServiceName, msg.SubServiceName, msg.Parameters.SerializedData));
            }

            return msg;
        }

        /// <summary>
        /// Receieves the response from queue.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns>The msg.</returns>
        public virtual ResponseMessage ReceieveResponseFromQueue(Guid transactionId)
        {
            ResponseMessage msg = GetResponseFromQueue(transactionId);

            if (msg != null)
            {
                if (OnLog != null) OnLog(string.Format("GetResponseFromQueue({0}:{1},{2}). -->(result success)", msg.TransactionId, msg.ServiceName, msg.SubServiceName));
            }

            return msg;
        }

        /// <summary>
        /// Subscribes the service request.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="handler">The handler.</param>
        public void SubscribeServiceRequest(string serviceName, Guid clientId, ServiceRequestNotifyHandler handler)
        {
            if (handler == null)
            {
                if (OnError != null)
                {
                    var exception = new IoCException("Service " + serviceName + " subscribing failed!");
                    OnError(exception);
                }
                return;
            }

            lock (onServiceRequests)
            {
                if (!onServiceRequests.ContainsKey(serviceName))
                {
                    onServiceRequests.Add(serviceName, new Dictionary<Guid, ServiceRequestNotifyHandler>());
                }
            }
            onServiceRequests[serviceName].Add(clientId, handler);

            string message = string.Format("Added new service reqMsg subscribing: {0} [{1}]", serviceName, clientId);
            if (OnLog != null)
                OnLog(message);
            else
            {
                string msg = "[" + DateTime.Now.ToString() + "] " + message;
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// Sets the broad cast strategy.
        /// </summary>
        /// <param name="broadCastStrategy">The broad cast strategy.</param>
        public void SetBroadCastStrategy(IBroadCastStrategy broadCastStrategy)
        {
            this.broadCastStrategy = broadCastStrategy;
        }

        #endregion

        #region MarshalByRefObject

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"></see> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"></see> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/></PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null;
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
