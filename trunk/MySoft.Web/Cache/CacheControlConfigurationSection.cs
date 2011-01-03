using System.Configuration;

namespace MySoft.Web
{
    public sealed class CacheControlConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// 默认过期时间为60秒
        /// </summary>
        public const int DEFAULT_EXPIRE_SECONDS = 60;
        private bool enable = false;

        [ConfigurationProperty("enable")]
        public bool Enable
        {
            get { return this["enable"] == null ? enable : (bool)this["enable"]; }
            set { this["enable"] = value; }
        }

        [ConfigurationProperty("cacheControls", IsRequired = true, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KeyValueConfigurationCollection))]
        public KeyValueConfigurationCollection CacheControls
        {
            get
            {
                return (KeyValueConfigurationCollection)this["cacheControls"];
            }
        }
    }
}
