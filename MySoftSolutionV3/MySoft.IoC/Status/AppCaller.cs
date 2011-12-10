﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC.Status
{
    /// <summary>
    /// 应用服务调用信息
    /// </summary>
    [Serializable]
    public class AppCaller : AppClient
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string SubServiceName { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public string Parameters { get; set; }
    }
}
