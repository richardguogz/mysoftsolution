using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using MySoft.IoC;

namespace MySoft.IoC.Configuration
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleFactoryConfiguration : ConfigurationBase
    {
        private CastleFactoryType type = CastleFactoryType.Local;
        private bool encrypt = false;
        private bool compress = false;

        private IDictionary<string, ServiceNode> hosts = new Dictionary<string, ServiceNode>();
        private string defaultservice;  //默认服务
        private bool throwerror = true; //抛出异常
        private int showlogtime = SimpleServiceContainer.DEFAULT_SHOWLOGTIME_NUMBER;  //超时多长输出日志，默认为1秒
        private int timeout = SimpleServiceContainer.DEFAULT_TIMEOUT_NUMBER;       //默认超时时间        10秒
        private int cachetime = SimpleServiceContainer.DEFAULT_CACHETIME_NUMBER;   //默认缓存时间        60秒

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

            if (xmlnode["encrypt"] != null && xmlnode["encrypt"].Value.Trim() != string.Empty)
                encrypt = Convert.ToBoolean(xmlnode["encrypt"].Value);

            if (xmlnode["compress"] != null && xmlnode["compress"].Value.Trim() != string.Empty)
                compress = Convert.ToBoolean(xmlnode["compress"].Value);

            if (xmlnode["timeout"] != null && xmlnode["timeout"].Value.Trim() != string.Empty)
                timeout = Convert.ToInt32(xmlnode["timeout"].Value);

            if (xmlnode["cachetime"] != null && xmlnode["cachetime"].Value.Trim() != string.Empty)
                cachetime = Convert.ToInt32(xmlnode["cachetime"].Value);

            if (xmlnode["showlogtime"] != null && xmlnode["showlogtime"].Value.Trim() != string.Empty)
                showlogtime = Convert.ToInt32(xmlnode["showlogtime"].Value);

            if (xmlnode["throwerror"] != null && xmlnode["throwerror"].Value.Trim() != string.Empty)
                throwerror = Convert.ToBoolean(xmlnode["throwerror"].Value);

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
        /// Gets or sets the encrypt.
        /// </summary>
        /// <value>The encrypt.</value>
        public bool Encrypt
        {
            get { return encrypt; }
            set { encrypt = value; }
        }

        /// <summary>
        /// Gets or sets the compress.
        /// </summary>
        /// <value>The format.</value>
        public bool Compress
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
        /// Gets or sets the cachetime
        /// </summary>
        /// <value>The cachetime.</value>
        public int CacheTime
        {
            get { return cachetime; }
            set { cachetime = value; }
        }

        /// <summary>
        /// Gets or sets the showlogtime
        /// </summary>
        /// <value>The showlogtime.</value>
        public int ShowlogTime
        {
            get { return showlogtime; }
            set { showlogtime = value; }
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
        /// Gets or sets the throwerror
        /// </summary>
        /// <value>The throwerror.</value>
        public bool ThrowError
        {
            get { return throwerror; }
            set { throwerror = value; }
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
