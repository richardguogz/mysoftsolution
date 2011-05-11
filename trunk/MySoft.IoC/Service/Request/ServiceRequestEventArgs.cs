using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务请求事件参数
    /// </summary>
    public class ServiceRequestEventArgs<T>
    {
        /// <summary>
        /// 响应的消息
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        /// 返回通讯的Socket对象
        /// </summary>
        public Socket Socket { get; set; }
    }
}
