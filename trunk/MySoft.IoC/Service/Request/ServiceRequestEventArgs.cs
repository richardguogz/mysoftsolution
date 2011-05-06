using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// 服务请求对象
        /// </summary>
        public ServiceRequest<T> Request { get; set; }
    }

}
