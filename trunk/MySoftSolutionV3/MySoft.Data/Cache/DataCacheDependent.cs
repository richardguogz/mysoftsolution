﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySoft.Cache;

namespace MySoft.Data.Cache
{
    /// <summary>
    /// 默认缓存依赖
    /// </summary>
    internal class DataCacheDependent : ICacheDependent
    {
        private ICacheStrategy strategy;
        /// <summary>
        /// 实例化默认缓存依赖
        /// </summary>
        /// <param name="strategy"></param>
        public DataCacheDependent(ICacheStrategy strategy)
        {
            this.strategy = strategy;
        }

        #region ICacheDependent 成员

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="cacheTime"></param>
        public void AddCache<T>(string cacheKey, T cacheValue, int cacheTime)
        {
            if (cacheTime > 0)
            {
                //组合CacheKey
                cacheKey = string.Format("{0}_{1}", typeof(T).FullName, cacheKey);
                strategy.AddObject(cacheKey, cacheValue, TimeSpan.FromSeconds(cacheTime));
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        public void RemoveCache<T>(string cacheKey)
        {
            //组合CacheKey
            cacheKey = string.Format("{0}_{1}", typeof(T).FullName, cacheKey);
            strategy.RemoveObject(cacheKey);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="?"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey)
        {
            //组合CacheKey
            cacheKey = string.Format("{0}_{1}", typeof(T).FullName, cacheKey);
            return strategy.GetObject<T>(cacheKey);
        }

        #endregion

        #region 处理一组缓存

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveCache<T>()
        {
            strategy.RemoveMatchObjects(typeof(T).FullName);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetCache<T>()
        {
            return strategy.GetMatchObjects<T>(typeof(T).FullName).Values.ToList();
        }

        #endregion
    }
}
