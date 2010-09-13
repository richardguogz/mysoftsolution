using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using MySoft.IoC.Service;
using MySoft.Core.Remoting;
using MySoft.Core;

namespace MySoft.IoC.Service
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleClientConfiguration : ConfigurationBase
    {
        private ServiceFactoryType type = ServiceFactoryType.Local;
        private RemotingChannelType protocol = RemotingChannelType.HTTP;
        private string server = "127.0.0.1";
        private int port = 8888;
        private string serviceMQName = "MMQ";
        private bool debug = true;
        private bool compress = false;
        private int timeout = -1;
        private int maxTry = SimpleServiceContainer.DEFAULT_MAX_TRY_NUMBER;

        /// <summary>
        /// 获取远程对象配置
        /// </summary>
        /// <returns></returns>
        public static CastleClientConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("serviceFramework/castleClient");

            if (obj != null)
                return (CastleClientConfiguration)obj;
            else
                return null;
        }

        /// <summary>
        /// 从配置文件加载配置值
        /// </summary>
        /// <param name="node"></param>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) return;

            if (node.Attributes["type"] != null && node.Attributes["type"].Value.Trim() != string.Empty)
                type = (ServiceFactoryType)Enum.Parse(typeof(ServiceFactoryType), node.Attributes["type"].Value);

            if (node.Attributes["protocol"] != null && node.Attributes["protocol"].Value != null)
                protocol = (RemotingChannelType)Enum.Parse(typeof(RemotingChannelType), node.Attributes["protocol"].Value);

            if (node.Attributes["server"] != null && node.Attributes["server"].Value.Trim() != string.Empty)
                server = node.Attributes["server"].Value;

            if (node.Attributes["port"] != null && node.Attributes["port"].Value.Trim() != string.Empty)
                port = Convert.ToInt32(node.Attributes["port"].Value);

            if (node.Attributes["name"] != null && node.Attributes["name"].Value.Trim() != string.Empty)
                serviceMQName = node.Attributes["name"].Value;

            if (node.Attributes["debug"] != null && node.Attributes["debug"].Value.Trim() != string.Empty)
                debug = Convert.ToBoolean(node.Attributes["debug"].Value);

            if (node.Attributes["compress"] != null && node.Attributes["compress"].Value.Trim() != string.Empty)
                compress = Convert.ToBoolean(node.Attributes["compress"].Value);

            if (node.Attributes["maxTry"] != null && node.Attributes["maxTry"].Value.Trim() != string.Empty)
                maxTry = Convert.ToInt32(node.Attributes["maxTry"].Value);

            if (node.Attributes["timeout"] != null && node.Attributes["timeout"].Value.Trim() != string.Empty)
                timeout = Convert.ToInt32(node.Attributes["timeout"].Value);
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ServiceFactoryType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public RemotingChannelType Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// Gets or sets the name of the service MQ.
        /// </summary>
        /// <value>The name of the service MQ.</value>
        public string ServiceMQName
        {
            get { return serviceMQName; }
            set { serviceMQName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ServiceFactoryConfigurationSection"/> is debug.
        /// </summary>
        /// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether data dispatched by this service factory is compressed.
        /// </summary>
        /// <value><c>true</c> if compress; otherwise, <c>false</c>.</value>
        public bool Compress
        {
            get { return compress; }
            set { compress = value; }
        }

        /// <summary>
        /// Gets or sets the max try.
        /// </summary>
        /// <value>The max try.</value>
        public int MaxTry
        {
            get { return maxTry; }
            set { maxTry = value; }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The max try.</value>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
    }
}
