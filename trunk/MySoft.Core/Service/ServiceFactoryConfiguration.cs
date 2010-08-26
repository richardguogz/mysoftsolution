using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace MySoft.Core.Service
{
    /// <summary>
    /// 服务工厂
    /// </summary>
    public sealed class ServiceFactoryConfiguration : ConfigurationSection
    {
        private ServiceConfigCollection services = new ServiceConfigCollection();
        internal ServiceConfigCollection Services
        {
            get { return services; }
        }

        /// <summary>
        /// 获取本地对象配置
        /// </summary>
        /// <returns></returns>
        public static ServiceFactoryConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("serviceFramework/serviceFactory");

            if (obj != null)
                return (ServiceFactoryConfiguration)obj;
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

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Comment) continue;

                if (n.Name == "serviceObject")
                {
                    ServiceConfig config = new ServiceConfig();
                    if (n.Attributes["key"] != null && n.Attributes["key"].Value != null)
                        config.Key = n.Attributes["key"].Value;

                    if (n.Attributes["service"] != null && n.Attributes["service"].Value != null)
                        config.Key = n.Attributes["service"].Value;

                    services.Add(config);
                }
            }
        }
    }

    /// <summary>
    /// 服务配置集合
    /// </summary>
    internal sealed class ServiceConfigCollection : List<ServiceConfig>
    { }

    /// <summary>
    /// 服务配置
    /// </summary>
    internal sealed class ServiceConfig
    {
        private string key;
        private string service;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Service
        {
            get { return service; }
            set { service = value; }
        }
    }
}
