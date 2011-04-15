using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Sockets;

namespace MySoft.Net
{
    /// <summary>
    /// 当前实体用于数据传输
    /// </summary>
    [Serializable]
    [BufferType(-20000)]
    public class ResponseMessage
    {
        /// <summary>
        /// 数据长度
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 数据信息
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 服务端返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 客户端传入的请求
        /// </summary>
        public RequestMessage Request { get; set; }
    }
}
