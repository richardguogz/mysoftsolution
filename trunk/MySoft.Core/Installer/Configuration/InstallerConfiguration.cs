using System;
using System.Configuration;
using System.Web;
using System.Xml.Serialization;

namespace MySoft.Installer.Configuration
{
    /// <summary>
    /// Specifies the configuration settings in the Web.config for the StaticPageRule.
    /// </summary>
    [Serializable]
    [XmlRoot("installer")]
    public class InstallerConfiguration
    {
        /// <summary>
        /// GetConfig() returns an instance of the <b>StaticPageConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>StaticPageConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="StaticPageConfiguration"/> instance.</returns>
        public static InstallerConfiguration GetConfig()
        {
            string key = "mysoft.framework/installer";
            InstallerConfiguration obj = CacheHelper.Get<InstallerConfiguration>(key);
            if (obj == null)
            {
                var tmp = ConfigurationManager.GetSection(key);
                obj = tmp as InstallerConfiguration;
                CacheHelper.Permanent(key, obj);
            }

            return obj;
        }

        #region Public Properties

        /// <summary>
        /// ������Ϣ
        /// </summary>
        [XmlElement("serviceType")]
        public string ServiceType { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [XmlElement("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// ������ʾ����
        /// </summary>
        [XmlElement("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// �û���
        /// </summary>
        [XmlAttribute("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [XmlAttribute("password")]
        public string Password { get; set; }

        #endregion
    }
}
