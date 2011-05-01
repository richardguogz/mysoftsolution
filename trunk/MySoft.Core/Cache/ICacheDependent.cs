using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.Cache
{
    /// <summary>
    /// 缓存依赖
    /// </summary>
    public interface ICacheDependent
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        void AddCache(Type serviceType, string cacheKey, object cacheValue);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        void RemoveCache(Type serviceType, string cacheKey);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        object GetCache(Type serviceType, string cacheKey);

        #region 处理一组缓存

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="serviceType"></param>
        void RemoveCache(Type serviceType);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IList<object> GetCache(Type serviceType);

        #endregion
    }
}
