using System.Configuration;

namespace MySoft.Web.Configuration
{
    public sealed class CacheControlConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Ĭ�Ϲ���ʱ��Ϊ60��
        /// </summary>
        public const int DEFAULT_EXPIRE_SECONDS = 60;
        private bool enable = false;

        [ConfigurationProperty("enable")]
        public bool Enable
        {
            get { return this["enable"] == null ? enable : (bool)this["enable"]; }
            set { this["enable"] = value; }
        }

        [ConfigurationProperty("controls", IsRequired = true, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KeyValueConfigurationCollection))]
        public KeyValueConfigurationCollection Controls
        {
            get
            {
                return (KeyValueConfigurationCollection)this["controls"];
            }
        }
    }
}
