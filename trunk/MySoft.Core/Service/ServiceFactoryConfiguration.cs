using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace MySoft.Core.Service
{
    /// <summary>
    /// ���񹤳�
    /// </summary>
    public sealed class ServiceFactoryConfiguration : ConfigurationSection
    {
        private ServiceConfigCollection services = new ServiceConfigCollection();
        internal ServiceConfigCollection Services
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
    /// �������ü���
    /// </summary>
    internal sealed class ServiceConfigCollection : List<ServiceConfig>
    { }

    /// <summary>
    /// ��������
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
