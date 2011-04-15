using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Sockets;

namespace MySoft.Net
{
    /// <summary>
    /// 当前实体用于请求传输
    /// </summary>
    [Serializable]
    [BufferType(-10000)]
    public class RequestMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid MessageID { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object[] Parameters { get; set; }
    }
}
