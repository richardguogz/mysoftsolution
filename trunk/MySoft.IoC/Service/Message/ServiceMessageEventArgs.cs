﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务响应事件参数
    /// </summary>
    public class ServiceMessageEventArgs
    {
        /// <summary>
        /// 响应的消息
        /// </summary>
        public ResponseMessage Result { get; set; }

        /// <summary>
        /// 返回通讯的Socket对象
        /// </summary>
        public Socket Socket { get; set; }
    }
}
