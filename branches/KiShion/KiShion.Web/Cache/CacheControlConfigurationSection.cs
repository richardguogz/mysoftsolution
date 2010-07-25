using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace KiShion.Web
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
