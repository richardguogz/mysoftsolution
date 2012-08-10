﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace MySoft.IoC.Aspect
{
    /// <summary>
    /// 拦截器选择
    /// </summary>
    public class InterceptorSelector : Castle.DynamicProxy.IInterceptorSelector
    {
        #region IInterceptorSelector 成员

        public Castle.DynamicProxy.IInterceptor[] SelectInterceptors(Type type, MethodInfo method, Castle.DynamicProxy.IInterceptor[] interceptors)
        {
            if (interceptors == null || interceptors.Length == 0)
                return interceptors;

            Castle.DynamicProxy.IInterceptor[] ilist = null;
            var att = CoreHelper.GetMemberAttribute<AspectSwitcherAttribute>(method);
            if (att != null && att.UseAspect)
            {
                if (att.InterceptorTypes == null)
                    ilist = interceptors;
                else
                    ilist = interceptors.Where(p => att.InterceptorTypes.Contains(p.GetType())).ToArray();
            }

            return ilist;
        }

        #endregion
    }
}
