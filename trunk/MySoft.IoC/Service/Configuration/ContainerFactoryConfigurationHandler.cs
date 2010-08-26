using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace MySoft.IoC.Service
{
    /// <summary>
    /// 服务配置类
    /// </summary>
    public class ContainerFactoryConfigurationHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            ContainerFactoryConfiguration config = new ContainerFactoryConfiguration();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #endregion
    }
}
