﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using System.Reflection;
using MySoft.Core;

namespace MySoft.IoC
{
    /// <summary>
    /// 拦截器选择
    /// </summary>
    public class InterceptorSelector : IInterceptorSelector
    {
        #region IInterceptorSelector 成员

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (interceptors == null) return interceptors;

            var att = CoreHelper.GetMemberAttribute<AspectSwitcherAttribute>(method);
            if (att == null)
            {
                return interceptors;
            }
            else
            {
                if (att.InterceptorTypes == null)
                {
                    return interceptors;
                }
                else
                {
                    return interceptors.Where(p => att.InterceptorTypes.Contains(p.GetType())).ToArray();
                }
            }
        }

        #endregion
    }
}
