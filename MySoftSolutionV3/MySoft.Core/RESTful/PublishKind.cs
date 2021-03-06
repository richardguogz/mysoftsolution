﻿using System;

namespace MySoft.RESTful
{
    /// <summary>
    /// 发布REST分类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class PublishKindAttribute : Attribute
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类别描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 实例化PublishKind
        /// </summary>
        public PublishKindAttribute() { }

        /// <summary>
        /// 实例化PublishKind
        /// </summary>
        /// <param name="name"></param>
        public PublishKindAttribute(string name)
        {
            this.Name = name;
        }
    }
}
