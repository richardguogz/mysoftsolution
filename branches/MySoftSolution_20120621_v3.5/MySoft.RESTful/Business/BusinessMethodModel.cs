﻿using System;
using System.Reflection;

namespace MySoft.RESTful.Business
{
    /// <summary>
    /// 业务方法模型
    /// </summary>
    public class BusinessMethodModel : BusinessStateModel
    {
        /// <summary>
        /// 认证类型
        /// </summary>
        public AuthorizeType AuthorizeType { get; set; }
        /// <summary>
        /// 方法调用类型
        /// </summary>
        public HttpMethod HttpMethod { get; set; }
        /// <summary>
        /// 业务执行对象
        /// </summary>
        public Type Service { get; set; }
        /// <summary>
        /// 执行的业务实例方法
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 业务示例方法参数
        /// </summary>
        public ParameterInfo[] Parameters { get; set; }
        /// <summary>
        /// 业务实例方法参数个数
        /// </summary>
        public int ParametersCount { get; set; }

        public BusinessMethodModel()
        {
            this.AuthorizeType = AuthorizeType.User;
        }
    }
}
