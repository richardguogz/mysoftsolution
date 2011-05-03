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

        private DefaultCacheDependent(CacheType type, int timeout)
        {
            this.strategy = CacheFactory.CreateCache("DefaultCacheDependent", type);
            this.timeout = timeout;
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <returns></returns>
        public static ICacheDependent Create()
        {
            return Create(CacheType.Local);
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICacheDependent Create(CacheType type)
        {
            return Create(type, DefaultCacheDependent.DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// 创建一个默认的缓存依赖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ICacheDependent Create(CacheType type, int timeout)
        {
            return new DefaultCacheDependent(CacheType.Local, timeout);
        }

        #region ICacheDependent 成员

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="cacheTime"></param>
        public void AddCache(string cacheKey, object cacheValue, int cacheTime)
        {
            lock (strategy)
            {
                var timer = cacheTime > 0 ? cacheTime : timeout;
                strategy.AddObject(cacheKey, cacheValue, TimeSpan.FromMilliseconds(timer));
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public void RemoveCache(string cacheKey)
        {
            lock (strategy)
            {
                strategy.RemoveObject(cacheKey);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public object GetCache(string cacheKey)
        {
            lock (strategy)
            {
                return strategy.GetObject(cacheKey);
            }
        }

        #endregion

        #region 处理一组缓存

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="serviceType"></param>
        public void RemoveCache(Type serviceType)
        {
            lock (strategy)
            {
                strategy.RemoveMatchObjects(serviceType.FullName);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IList<object> GetCache(Type serviceType)
        {
            lock (strategy)
            {
                return strategy.GetMatchObjects(serviceType.FullName).Values.ToList();
            }
        }

        #endregion
    }
}
