﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MySoft.IoC
{
    /// <summary>
    /// 状态服务 (超时为10秒，缓存1秒)
    /// </summary>
    [ServiceContract(Timeout = 10000, CacheTime = 1000)]
    public interface IStatusService
    {
        /// <summary>
        /// 清除服务器状态
        /// </summary>
        void ClearStatus();

        /// <summary>
        /// 获取服务状态信息（包括SummaryStatus，HighestStatus，TimeStatus）
        /// </summary>
        /// <returns></returns>
        ServerStatus GetServerStatus();

        /// <summary>
        /// 获取汇总状态信息
        /// </summary>
        /// <returns></returns>
        SummaryStatus GetSummaryStatus();

        /// <summary>
        /// 获取最高状态信息
        /// </summary>
        /// <returns></returns>
        HighestStatus GetHighestStatus();

        /// <summary>
        /// 获取最后一次服务状态
        /// </summary>
        /// <returns></returns>
        TimeStatus GetLatestStatus();

        /// <summary>
        /// 获取时段的服务状态信息
        /// </summary>
        /// <returns></returns>
        IList<TimeStatus> GetTimeStatusList();

        /// <summary>
        /// 获取所有的客户端信息
        /// </summary>
        /// <returns></returns>
        IList<ConnectInfo> GetConnectInfoList();
    }
}
