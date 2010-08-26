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
        private List<ServiceBase> services = new List<ServiceBase>();
        public List<ServiceBase> Services
        {
            get { return services; }
        }

        /// <summary>
        /// 获取本地对象配置
        /// </summary>
        /// <returns></returns>
        public static ServiceFactoryConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("configFramework/serviceFactory");

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
                    ServiceBase config = new ServiceBase();
                    if (n.Attributes["name"] != null && n.Attributes["name"].Value != null)
                        config.Name = n.Attributes["name"].Value;

                    if (n.Attributes["assemblyName"] != null && n.Attributes["assemblyName"].Value != null)
                        config.AssemblyName = n.Attributes["assemblyName"].Value;

                    if (n.Attributes["className"] != null && n.Attributes["className"].Value != null)
                        config.ClassName = n.Attributes["className"].Value;

                    services.Add(config);
                }
            }
        }
    }
}
