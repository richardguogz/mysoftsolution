using System;
using MySoft.Logger;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务代理
    /// </summary>
    public interface IServiceProxy
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        ResponseMessage CallMethod(RequestMessage reqMsg, double logtime);
    }
}
