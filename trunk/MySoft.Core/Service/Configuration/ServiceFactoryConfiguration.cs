using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MySoft
{
    /// <summary>
    /// ���񹤳�
    /// </summary>
    public sealed class ServiceFactoryConfiguration : ConfigurationSection
    {
        private List<ServiceBase> services = new List<ServiceBase>();
        public List<ServiceBase> Services
        {
            get { return services; }
        }

        /// <summary>
        /// ��ȡ���ض�������
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
        /// �������ļ���������ֵ
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
