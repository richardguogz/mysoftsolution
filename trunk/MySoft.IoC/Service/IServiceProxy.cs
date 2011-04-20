using System;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务代理
    /// </summary>
    public interface IServiceProxy : ILogable
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        ResponseMessage CallMethod(RequestMessage msg);

        /// <summary>
        /// 尝试次数
        /// </summary>
        int MaxTryNum { get; set; }
    }
}
