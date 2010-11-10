﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Core;

namespace MySoft.IoC
{
    /// <summary>
    /// Aop代理管理器
    /// </summary>
    public static class AspectManager
    {
        private readonly static IDictionary<Type, object> aopServices = new Dictionary<Type, object>();

        /// <summary>
        /// 获取Aop服务
        /// </summary>
        /// <typeparam name="ServiceType"></typeparam>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static IServiceType GetAopService<IServiceType>(Type serviceType)
        {
            return (IServiceType)GetAopService(serviceType);
        }

        /// <summary>
        /// 获取Aop服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static object GetAopService(Type serviceType)
        {
            object service = null;

            if (aopServices.ContainsKey(serviceType))
            {
                return aopServices[serviceType];
            }
            else
            {
                var attributes = CoreUtils.GetTypeAttributes<AspectAttribute>(serviceType);
                if (attributes != null && attributes.Length > 0)
                {
                    IList<object> list = new List<object>();
                    foreach (var attribute in attributes)
                    {
                        if (typeof(AspectInterceptor).IsAssignableFrom(attribute.InterceptorType))
                        {
                            object value = null;
                            if (attribute.Arguments == null)
                                value = Activator.CreateInstance(attribute.InterceptorType);
                            else
                            {
                                if (attribute.Arguments.GetType().IsClass)
                                {
                                    var arg = Activator.CreateInstance(attribute.Arguments.GetType());
                                    value = Activator.CreateInstance(attribute.InterceptorType, arg);
                                }
                                else
                                    value = Activator.CreateInstance(attribute.InterceptorType, attribute.Arguments);
                            }

                            list.Add(value);
                        }
                    }

                    var interceptors = list.Cast<AspectInterceptor>();
                    service = AspectFactory.CreateProxy(serviceType, interceptors.ToArray());

                    aopServices.Add(serviceType, service);
                }
            }

            return service;
        }
    }
}
