using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 状态服务
    /// </summary>
    [ServiceContract]
    public interface IStatusService
    {
        /// <summary>
        /// 获取状态信息
        /// </summary>
        /// <returns></returns>
        ServerStatus GetStatus();
    }
}
