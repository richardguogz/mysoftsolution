using System;
using MySoft.Logger;

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
        ResponseMessage CallMethod(RequestMessage msg, int showlogtime);

        /// <summary>
        /// 是否加密
        /// </summary>
        bool Encrypt { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        bool Compress { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// 是否抛出错误
        /// </summary>
        bool ThrowError { get; set; }
    }
}
