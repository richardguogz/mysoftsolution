using System;

namespace MySoft.IoC.Messages
{
    /// <summary>
    /// request base
    /// </summary>
    [Serializable]
    public abstract class MessageBase
    {
        private string serviceName;
        private string methodName;
        private ParameterCollection parameters = new ParameterCollection();

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
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the sub service.</value>
        public string MethodName
        {
            get
            {
                return methodName;
            }
            set
            {
                methodName = value;
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

        /// <summary>
        /// ��Ӧ����Ϣ
        /// </summary>
        public abstract string Message { get; }
    }

    /// <summary>
    /// The request msg.
    /// </summary>
    [Serializable]
    public class RequestMessage : MessageBase
    {
        #region Private Members

        private string appName;
        private string appPath;
        private string appVersion;
        private string hostName;
        private string requestAddress;
        private bool invokeMethod;
        private bool enableCache = true;

        #endregion

        /// <summary>
        /// Gets or sets the name of the appName.
        /// </summary>
        /// <value>The name of the appName.</value>
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                appName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the appPath.
        /// </summary>
        /// <value>The name of the appPath.</value>
        public string AppPath
        {
            get
            {
                return appPath;
            }
            set
            {
                appPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the appVersion.
        /// </summary>
        public string AppVersion
        {
            get
            {
                return appVersion;
            }
            set
            {
                appVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the hostName.
        /// </summary>
        /// <value>The name of the hostName.</value>
        public string HostName
        {
            get
            {
                return hostName;
            }
            set
            {
                hostName = value;
            }
        }

        /// <summary>
        /// Gets or sets the request address
        /// </summary>
        public string IPAddress
        {
            get
            {
                return requestAddress;
            }
            set
            {
                requestAddress = value;
            }
        }

        /// <summary>
        /// invoke method
        /// </summary>
        public bool InvokeMethod
        {
            get
            {
                return invokeMethod;
            }
            set
            {
                invokeMethod = value;
            }
        }

        /// <summary>
        /// enable cache
        /// </summary>
        public bool EnableCache
        {
            get
            {
                return enableCache;
            }
            set
            {
                enableCache = value;
            }
        }

        private int cacheTime = -1;
        /// <summary>
        /// ���ݻ���ʱ�䣨��λ���룩
        /// </summary>
        public int CacheTime
        {
            get
            {
                return cacheTime;
            }
            set
            {
                cacheTime = value;
            }
        }

        #region ����Ĳ���

        [NonSerialized]
        private System.Reflection.MethodInfo method;

        /// <summary>
        /// ��Ӧ�ķ���
        /// </summary>
        internal System.Reflection.MethodInfo MethodInfo
        {
            get
            {
                return method;
            }
            set
            {
                method = value;
            }
        }

        [NonSerialized]
        private ResponseType resptype;

        /// <summary>
        /// �������������
        /// </summary>
        internal ResponseType RespType
        {
            get
            {
                return resptype;
            }
            set
            {
                resptype = value;
            }
        }

        #endregion

        /// <summary>
        /// �������Ϣ
        /// </summary>
        public override string Message
        {
            get
            {

                return string.Format("{0}��{1}({2})", this.appName, this.hostName, this.requestAddress);
            }
        }
    }
}
