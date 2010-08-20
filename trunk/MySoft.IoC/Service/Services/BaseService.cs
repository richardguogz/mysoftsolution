using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MySoft.Core;

namespace MySoft.IoC.Service.Services
{
    /// <summary>
    /// The base class of services.
    /// </summary>
    [Serializable]
    public abstract class BaseService : MarshalByRefObject, IService, ILogable
    {
        private Guid clientId;

        /// <summary>
        /// The service name.
        /// </summary>
        protected string serviceName;
        /// <summary>
        /// The service mq.
        /// </summary>
        protected IServiceMQ mq;

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
        /// <param name="mq">The mq.</param>
        public BaseService(string serviceName, IServiceMQ mq)
        {
            this.serviceName = serviceName;
            this.mq = mq;
            this.clientId = Guid.NewGuid();
        }

        #region IService Members

        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>The client id.</value>
        public Guid ClientId
        {
            get
            {
                return clientId;
            }
        }

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
            //SerializationManager.Serialize(msg)
            if (OnLog != null) OnLog(string.Format("Dynamic service ({0}):{1} process. -->(name:{2} parameters:{3})", clientId, serviceName, msg.SubServiceName, msg.Parameters.SerializedData));
            ResponseMessage retMsg = null;
            try
            {
                retMsg = Run(msg);
            }
            catch (Exception ex)
            {
                if (OnLog != null) OnLog(string.Format("Error occured at ({0}):{1}. -->(error:{2})", clientId, serviceName, ex.Message));
                return null;
            }
            //SerializationManager.Serialize(retMsg)
            if (OnLog != null) OnLog(string.Format("Dynamic service ({0}):{1} return. -->(result:{2})", clientId, serviceName, retMsg.Data));
            return retMsg;
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
        public event LogHandler OnLog;

        #endregion
    }
}
