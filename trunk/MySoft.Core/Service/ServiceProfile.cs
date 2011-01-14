﻿using System;

namespace MySoft
{

    /// <summary>
    /// 服务配置基类
    /// </summary>
    [Serializable]
    public class ServiceProfile
    {
        private string _Name;

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _AssemblyName;

        /// <summary>
        /// 程序集名称字符串
        /// </summary>
        public string AssemblyName
        {
            get { return _AssemblyName; }
            set { _AssemblyName = value; }
        }

        private string _ClassName;

        /// <summary>
        /// 完整类名
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
    }
}