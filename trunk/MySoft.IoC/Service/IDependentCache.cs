using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 依赖缓存
    /// </summary>
    public interface IDependentCache
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        void AddCache(Type serviceInterfaceType, string cacheKey, object cacheValue);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="serviceInterfaceType"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        object GetCache(Type serviceInterfaceType, string cacheKey);
    }
}
