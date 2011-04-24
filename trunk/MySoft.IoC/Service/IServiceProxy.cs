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
        /// 传输类型
        /// </summary>
        ResponseFormat Format { get; set; }

        /// <summary>
        /// 压缩类型
        /// </summary>
        CompressType Compress { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        int Timeout { get; set; }
    }
}
