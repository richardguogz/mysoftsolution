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
            object obj = ConfigurationManager.GetSection("mysoft.framework/installer");

            if (obj != null)
                return (InstallerConfiguration)obj;
            else
                return null;
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
        [XmlElement("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [XmlElement("password")]
        public string Password { get; set; }

        #endregion
    }
}
