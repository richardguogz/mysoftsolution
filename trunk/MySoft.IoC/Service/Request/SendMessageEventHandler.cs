using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 发送消息委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    public delegate void SendMessageEventHandler<T>(object sender, ServiceRequestEventArgs<T> message);
}
