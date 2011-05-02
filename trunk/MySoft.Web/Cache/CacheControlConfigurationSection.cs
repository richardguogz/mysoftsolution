using System.Configuration;

namespace MySoft.Web.Configuration
{
    public sealed class CacheControlConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 默认过期时间为60秒
        /// </summary>
        public const int DEFAULT_EXPIRE_SECONDS = 60;
        private bool enabled = false;

        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return this["enabled"] == null ? enabled : (bool)this["enabled"]; }
            set { this["enabled"] = value; }
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
