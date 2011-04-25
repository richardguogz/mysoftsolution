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
        private ResponseFormat format = ResponseFormat.Binary;
        private CompressType compress = CompressType.None;

        private IDictionary<string, ServiceNode> hosts = new Dictionary<string, ServiceNode>();
        private string defaultservice;
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

            XmlAttributeCollection xmlnode = node.Attributes;

            if (xmlnode["type"] != null && xmlnode["type"].Value.Trim() != string.Empty)
                type = (CastleFactoryType)Enum.Parse(typeof(CastleFactoryType), xmlnode["type"].Value);

            if (xmlnode["format"] != null && xmlnode["format"].Value.Trim() != string.Empty)
            {
                switch (xmlnode["format"].Value.ToLower())
                {
                    case "binary":
                        format = ResponseFormat.Binary;
                        break;
                    case "json":
                        format = ResponseFormat.Json;
                        break;
                    case "xml":
                        format = ResponseFormat.Xml;
                        break;
                    default:
                        format = ResponseFormat.Binary;
                        break;
                }
            }

            if (xmlnode["compress"] != null && xmlnode["compress"].Value.Trim() != string.Empty)
            {
                switch (xmlnode["compress"].Value.ToLower())
                {
                    case "sevenzip":
                        compress = CompressType.SevenZip;
                        break;
                    case "gzip":
                        compress = CompressType.GZip;
                        break;
                    case "deflate":
                        compress = CompressType.Deflate;
                        break;
                    default:
                        compress = CompressType.None;
                        break;
                }
            }

            if (xmlnode["timeout"] != null && xmlnode["timeout"].Value.Trim() != string.Empty)
                timeout = Convert.ToInt32(xmlnode["timeout"].Value);

            if (xmlnode["default"] != null && xmlnode["default"].Value.Trim() != string.Empty)
                defaultservice = xmlnode["default"].Value;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment) continue;

                XmlAttributeCollection childnode = child.Attributes;
                if (child.Name == "service")
                {
                    ServiceNode service = new ServiceNode();
                    service.Name = childnode["name"].Value;
                    service.Server = childnode["server"].Value;
                    service.Port = Convert.ToInt32(childnode["port"].Value);

                    //处理默认的服务
                    if (string.IsNullOrEmpty(defaultservice))
                    {
                        defaultservice = service.Name;
                    }

                    hosts.Add(service.Name, service);
                }
            }

            //判断是否配置了服务信息
            if (hosts.Count == 0)
            {
                throw new IoCException("Not configure any service node！");
            }

            //判断是否包含默认的服务
            if (!hosts.ContainsKey(defaultservice))
            {
                throw new IoCException("Not find the default service node [" + defaultservice + "]！");
            }
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
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public ResponseFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Gets or sets the compress.
        /// </summary>
        /// <value>The format.</value>
        public CompressType Compress
        {
            get { return compress; }
            set { compress = value; }
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

        /// <summary>
        /// Gets or sets the default
        /// </summary>
        /// <value>The default.</value>
        public string Default
        {
            get { return defaultservice; }
            set { defaultservice = value; }
        }

        /// <summary>
        /// Gets or sets the hosts
        /// </summary>
        /// <value>The hosts.</value>
        public IDictionary<string, ServiceNode> Hosts
        {
            get { return hosts; }
            set { hosts = value; }
        }
    }
}
