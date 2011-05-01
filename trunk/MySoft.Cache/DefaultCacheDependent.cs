using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.Cache
{
    /// <summary>
    /// 默认缓存依赖
    /// </summary>
    public class DefaultCacheDependent : ICacheDependent
    {
        public static readonly int DEFAULT_TIMEOUT = 30 * 60 * 1000;

        private int timeout = DefaultCacheDependent.DEFAULT_TIMEOUT; //默认30分钟
        private ICacheStrategy strategy;
        private IDictionary<string, Type> services = new Dictionary<string, Type>();

        private DefaultCacheDependent(CacheType type, int timeout)
        {
            this.strategy = CacheFactory.CreateCache("DefaultCacheDependent", type);
            this.timeout = timeout;
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <returns></returns>
        public static DefaultCacheDependent Create()
        {
            return Create(CacheType.Local);
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DefaultCacheDependent Create(CacheType type)
        {
            return Create(type, DefaultCacheDependent.DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static DefaultCacheDependent Create(CacheType type, int timeout)
        {
            return new DefaultCacheDependent(CacheType.Local, timeout);
        }

        /// <summary>
        /// 添加需要缓存的服务类型，不添加默认为全部
        /// </summary>
        /// <param name="type"></param>
        public void AddService(Type type)
        {
            services[type.FullName] = type;
        }

        /// <summary>
        /// 缓存是否存在服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool Contains(Type type)
        {
            return services.Count == 0 || services.ContainsKey(type.FullName);
        }

        #region ICacheDependent 成员

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        public void AddCache(Type serviceType, string cacheKey, object cacheValue)
        {
            if (Contains(serviceType))
            {
                lock (strategy)
                {
                    cacheKey = string.Format("{0}_{1}", serviceType.FullName, cacheKey);
                    strategy.AddObject(cacheKey, cacheValue, TimeSpan.FromMilliseconds(timeout));
                }
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        public void RemoveCache(Type serviceType, string cacheKey)
        {
            if (Contains(serviceType))
            {
                lock (strategy)
                {
                    cacheKey = string.Format("{0}_{1}", serviceType.FullName, cacheKey);
                    strategy.RemoveObject(cacheKey);
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public object GetCache(Type serviceType, string cacheKey)
        {
            if (Contains(serviceType))
            {
                lock (strategy)
                {
                    cacheKey = string.Format("{0}_{1}", serviceType.FullName, cacheKey);
                    return strategy.GetObject(cacheKey);
                }
            }

            return null;
        }

        #endregion

        #region 处理一组缓存

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="serviceType"></param>
        public void RemoveCache(Type serviceType)
        {
            if (Contains(serviceType))
            {
                lock (strategy)
                {
                    strategy.RemoveMatchObjects(serviceType.FullName);
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IList<object> GetCache(Type serviceType)
        {
            if (Contains(serviceType))
            {
                lock (strategy)
                {
                    return strategy.GetMatchObjects(serviceType.FullName).Values.ToList();
                }
            }
            return new List<object>();
        }

        #endregion
    }
}
