using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using System.Reflection;
using MySoft.Core;

namespace MySoft.IoC
{
    public class ProxyGenerationHook : IProxyGenerationHook
    {
        #region IProxyGenerationHook 成员

        public void MethodsInspected() { }

        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo) { }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            var att = CoreUtils.GetMemberAttribute<AspectSwitcherAttribute>(methodInfo);
            if (att == null) return true;
            return att.UseAspect;
        }

        #endregion
    }
}
