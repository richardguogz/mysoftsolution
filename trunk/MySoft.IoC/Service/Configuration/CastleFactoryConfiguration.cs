using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using MySoft.IoC;
using MySoft.Remoting;
using MySoft.Core;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleFactoryConfiguration : ConfigurationBase
    {
        private CastleFactoryType type = CastleFactoryType.Local;
        private RemotingChannelType protocol = RemotingChannelType.Tcp;
        private TransferType transfer = TransferType.Binary;
        private CompressType compress = CompressType.None;

        private string server = "127.0.0.1";
        private int port = 8888;
        private string serviceMQName = "MMQ";
        private int maxTry = SimpleServiceContainer.DEFAULT_MAX_TRY_NUMBER;

        /// <summary>
        /// 获取远程对象配置
        /// </summary>
        /// <returns></returns>
        public static CastleFactoryConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("serviceFramework/castleFactory");

            if (obj != null)
                return (CastleFactoryConfiguration)obj;
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
                type = (CastleFactoryType)Enum.Parse(typeof(CastleFactoryType), node.Attributes["type"].Value);

            if (node.Attributes["protocol"] != null && node.Attributes["protocol"].Value != null)
            {
                switch (node.Attributes["protocol"].Value.ToLower())
                {
                    case "http":
                        protocol = RemotingChannelType.Http;
                        break;
                    case "tcp":
                        protocol = RemotingChannelType.Tcp;
                        break;
                    default:
                        protocol = RemotingChannelType.Tcp;
                        break;
                }
            }

            if (node.Attributes["transfer"] != null && node.Attributes["transfer"].Value.Trim() != string.Empty)
            {
                switch (node.Attributes["transfer"].Value.ToLower())
                {
                    case "binary":
                        transfer = TransferType.Binary;
                        break;
                    case "json":
                        transfer = TransferType.Json;
                        break;
                    case "xml":
                        transfer = TransferType.Xml;
                        break;
                    default:
                        transfer = TransferType.Binary;
                        break;
                }
            }

            if (node.Attributes["compress"] != null && node.Attributes["compress"].Value.Trim() != string.Empty)
            {
                switch (node.Attributes["compress"].Value.ToLower())
                {
                    case "none":
                        compress = CompressType.None;
                        break;
                    case "zip":
                        compress = CompressType.Zip;
                        break;
                    case "gzip":
                        compress = CompressType.GZip;
                        break;
                    case "auto":
                        compress = CompressType.Auto;
                        break;
                    default:
                        compress = CompressType.None;
                        break;
                }
            }

            if (node.Attributes["server"] != null && node.Attributes["server"].Value.Trim() != string.Empty)
                server = node.Attributes["server"].Value;

            if (node.Attributes["port"] != null && node.Attributes["port"].Value.Trim() != string.Empty)
                port = Convert.ToInt32(node.Attributes["port"].Value);

            if (node.Attributes["name"] != null && node.Attributes["name"].Value.Trim() != string.Empty)
                serviceMQName = node.Attributes["name"].Value;

            if (node.Attributes["maxTry"] != null && node.Attributes["maxTry"].Value.Trim() != string.Empty)
                maxTry = Convert.ToInt32(node.Attributes["maxTry"].Value);
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public CastleFactoryType Type
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
        /// Gets or sets the transfer.
        /// </summary>
        /// <value>The transfer.</value>
        public TransferType Transfer
        {
            get { return transfer; }
            set { transfer = value; }
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
        /// Gets or sets a value indicating whether data dispatched by this service factory is compressed.
        /// </summary>
        /// <value><c>true</c> if compress; otherwise, <c>false</c>.</value>
        public CompressType Compress
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
    }
}
