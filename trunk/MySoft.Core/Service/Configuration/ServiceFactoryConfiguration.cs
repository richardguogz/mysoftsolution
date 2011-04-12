using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MySoft
{
    /// <summary>
    /// 服务工厂
    /// </summary>
    public sealed class ServiceFactoryConfiguration : ConfigurationSection
    {
        private List<ServiceProfile> services = new List<ServiceProfile>();
        public List<ServiceProfile> Services
        {
            get { return services; }
        }

        /// <summary>
        /// 获取本地对象配置
        /// </summary>
        /// <returns></returns>
        public static ServiceFactoryConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("mysoft.framework/serviceFactory");

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
                    ServiceProfile config = new ServiceProfile();
                    if (n.Attributes["name"] != null && n.Attributes["name"].Value.Trim() != string.Empty)
                        config.Name = n.Attributes["name"].Value;

                    if (n.Attributes["assemblyName"] != null && n.Attributes["assemblyName"].Value.Trim() != string.Empty)
                        config.AssemblyName = n.Attributes["assemblyName"].Value;

                    if (n.Attributes["className"] != null && n.Attributes["className"].Value.Trim() != string.Empty)
                        config.ClassName = n.Attributes["className"].Value;

                    services.Add(config);
                }
            }
        }
    }
}
