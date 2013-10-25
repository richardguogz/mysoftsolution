﻿using MySoft.IoC.Communication.Scs.Server;
using MySoft.IoC.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 发送消息项
    /// </summary>
    internal class MessageItem
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 发送对象
        /// </summary>
        public IScsServerClient Channel { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public RequestMessage Request { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public ResponseMessage Response { get; set; }
    }
}
