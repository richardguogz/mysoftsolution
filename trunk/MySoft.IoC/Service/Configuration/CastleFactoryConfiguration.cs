using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using MySoft.IoC;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleFactoryConfiguration : ConfigurationBase
    {
        private CastleFactoryType type = CastleFactoryType.Local;
        private TransferType transfer = TransferType.Binary;

        private string server = "127.0.0.1";
        private int port = 8888;
        private int timeout = SimpleServiceContainer.DEFAULT_TIMEOUT_NUMBER;

        /// <summary>
        /// 获取远程对象配置
        /// </summary>
        /// <returns></returns>
        public static CastleFactoryConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("mysoft.framework/castleFactory");

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

            if (node.Attributes["server"] != null && node.Attributes["server"].Value.Trim() != string.Empty)
                server = node.Attributes["server"].Value;

            if (node.Attributes["port"] != null && node.Attributes["port"].Value.Trim() != string.Empty)
                port = Convert.ToInt32(node.Attributes["port"].Value);

            if (node.Attributes["timeout"] != null && node.Attributes["timeout"].Value.Trim() != string.Empty)
                timeout = Convert.ToInt32(node.Attributes["timeout"].Value);
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
        /// Gets or sets the timeout
        /// </summary>
        /// <value>The timeout.</value>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
    }
}
