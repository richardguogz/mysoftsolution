using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MySoft.IoC
{
    /// <summary>
    /// 状态服务 (超时为30秒，缓存1秒)
    /// </summary>
    [ServiceContract(Timeout = 30000, CacheTime = 1000)]
    public interface IStatusService
    {
        /// <summary>
        /// 获取状态信息
        /// </summary>
        /// <returns></returns>
        ServerStatus GetServerStatus();

        /// <summary>
        /// 获取所有的终结点
        /// </summary>
        /// <returns></returns>
        IList<EndPoint> GetEndPoints();
    }
}
