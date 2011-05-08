﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Logger;

namespace MySoft
{
    /// <summary>
    /// 服务运行基类
    /// </summary>
    public interface IServiceRun
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();

        /// <summary>
        /// 设置运行类型
        /// </summary>
        StartMode StartMode { set; get; }
    }

    /// <summary>
    /// 运行类型
    /// </summary>
    public enum StartMode
    {
        /// <summary>
        /// 服务
        /// </summary>
        Service,

        /// <summary>
        /// 控制台
        /// </summary>
        Console
    }
}
