using System.Collections.Generic;
using System.Configuration;

namespace MySoft.Web.Configuration
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
            if (config != null)
            {
                this.config = config;
                //保存控件配置信息
                rules = new Dictionary<string, int>();
                foreach (string key in config.Controls.AllKeys)
                {
                    int expireSeconds = CacheControlConfigurationSection.DEFAULT_EXPIRE_SECONDS;
                    try
                    {
                        expireSeconds = int.Parse(config.Controls[key].Value);
                    }
                    catch { }
                    rules.Add(key.ToLower(), expireSeconds);
                }
            }
        }

        /// <summary>
        /// 加载控件缓存配置
        /// </summary>
        internal static CacheControlConfiguration GetSection()
        {
            if (singleton == null)
            {
                CacheControlConfigurationSection config = null;
                object obj = ConfigurationManager.GetSection("mysoft.framework/cacheControl");
                if (obj != null)
                {
                    config = (CacheControlConfigurationSection)obj;
                }
                singleton = new CacheControlConfiguration(config);
            }

            return singleton;
        }
    }
}
