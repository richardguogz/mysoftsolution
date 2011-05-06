using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 发送消息委托
    /// </summary>
    /// <param name="message"></param>
    public delegate void SendMessageEventHandler<T>(ServiceRequestEventArgs<T> message);
}
