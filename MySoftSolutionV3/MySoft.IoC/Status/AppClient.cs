﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC.Status
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    [Serializable]
    public class AppClient
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string HostName { get; set; }
    }
}
