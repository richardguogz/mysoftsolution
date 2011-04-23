using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// The request msg.
    /// </summary>
    [Serializable]
    [BufferType(-10000)]
    public class RequestMessage
    {
        #region Private Members

        private string serviceName;
        private string subServiceName;
        private Guid transactionId;
        private string calledIP;
        private int timeout = -1;
        private ParameterCollection parameters = new ParameterCollection();
        private TransferType transfer = TransferType.Binary;

        #endregion

        /// <summary>
        /// Gets or sets the transfer of the service.
        /// </summary>
        public TransferType Transfer
        {
            get
            {
                return transfer;
            }
            set
            {
                transfer = value;
            }
        }

        /// <summary>
        /// Gets or sets the timeout of the service.
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the called ip.
        /// </summary>
        public string CalledIP
        {
            get
            {
                return calledIP;
            }
            set
            {
                calledIP = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName
        {
            get
            {
                return serviceName;
            }
            set
            {
                serviceName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the sub service.
        /// </summary>
        /// <value>The name of the sub service.</value>
        public string SubServiceName
        {
            get
            {
                return subServiceName;
            }
            set
            {
                subServiceName = value;
            }
        }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id.</value>
        public Guid TransactionId
        {
            get
            {
                return transactionId;
            }
            set
            {
                transactionId = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public ParameterCollection Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }
    }
}
