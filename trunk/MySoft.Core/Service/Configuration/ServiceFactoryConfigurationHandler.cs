using System.Configuration;

namespace MySoft
{
    /// <summary>
    /// 本地服务
    /// </summary>
    public class ServiceFactoryConfigurationHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            ServiceFactoryConfiguration config = new ServiceFactoryConfiguration();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #endregion
    }
}
