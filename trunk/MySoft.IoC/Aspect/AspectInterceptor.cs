﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;

namespace MySoft.IoC
{
    /// <summary>
    /// AOP拦截器
    /// </summary>
    public abstract class AspectInterceptor : StandardInterceptor
    {
        /// <summary>
        /// 进行时的方法
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PerformProceed(IInvocation invocation)
        {
            base.PerformProceed(invocation);
        }

        /// <summary>
        /// 处理后的方法
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PostProceed(IInvocation invocation)
        {
            base.PostProceed(invocation);
        }

        /// <summary>
        /// 处理前的方法
        /// </summary>
        /// <param name="invocation"></param>
        protected override void PreProceed(IInvocation invocation)
        {
            base.PreProceed(invocation);
        }
    }
}
