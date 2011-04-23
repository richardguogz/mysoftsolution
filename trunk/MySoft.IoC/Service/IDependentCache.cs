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
        /// 服务名称
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddCache(string key, object value);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetCache(string key);
    }
}
