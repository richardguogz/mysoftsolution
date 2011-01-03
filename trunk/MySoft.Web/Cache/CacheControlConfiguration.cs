using System.Collections.Generic;
using System.Configuration;

namespace MySoft.Web
{
    public sealed class CacheControlConfiguration
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        private CacheControlConfigurationSection config;
        internal CacheControlConfigurationSection Config
        {
            get
            {
                return config;
            }
        }

        /// <summary>
        /// 获取缓存规则
        /// </summary>
        private Dictionary<string, int> rules;
        internal Dictionary<string, int> Rules
        {
            get
            {
                return rules;
            }
        }

        /// <summary>
        /// 创建单例
        /// </summary>
        private static CacheControlConfiguration singleton;
        private CacheControlConfiguration(CacheControlConfigurationSection config)
        {
            this.config = config;
            //保存控件配置信息
            rules = new Dictionary<string, int>();
            foreach (string key in config.CacheControls.AllKeys)
            {
                int expireSeconds = CacheControlConfigurationSection.DEFAULT_EXPIRE_SECONDS;
                try
                {
                    expireSeconds = int.Parse(config.CacheControls[key].Value);
                }
                catch { }
                rules.Add(key.ToLower(), expireSeconds);
            }
        }

        /// <summary>
        /// 加载控件缓存配置
        /// </summary>
        internal static CacheControlConfiguration GetSection()
        {
            if (singleton == null)
            {
                CacheControlConfigurationSection config = new CacheControlConfigurationSection();
                object controlConfig = ConfigurationManager.GetSection("cacheControlConfig");
                if (controlConfig != null)
                {
                    config = (CacheControlConfigurationSection)controlConfig;
                }
                singleton = new CacheControlConfiguration(config);
            }

            return singleton;
        }
    }
}
