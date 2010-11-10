using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;

namespace MySoft.IoC
{
    /// <summary>
    /// AOP工厂类
    /// </summary>
    public static class AspectFactory
    {
        /// <summary>
        /// 代理对象列表
        /// </summary>
        private readonly static IDictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// 创建一个实例方式的拦截器
        /// </summary>
        /// <typeparam name="TServiceType"></typeparam>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static TServiceType CreateProxy<TServiceType>(params AspectInterceptor[] interceptors)
            where TServiceType : class
        {
            if (services.ContainsKey(typeof(TServiceType)))
            {
                return services[typeof(TServiceType)] as TServiceType;
            }
            else
            {
                ProxyGenerator proxy = new ProxyGenerator();
                var service = proxy.CreateClassProxy<TServiceType>(interceptors);
                services.Add(typeof(TServiceType), service);

                return service;
            }
        }

        /// <summary>
        /// 创建一个实例方式的拦截器
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static object CreateProxy(Type serviceType, params AspectInterceptor[] interceptors)
        {
            if (services.ContainsKey(serviceType))
            {
                return services[serviceType];
            }
            else
            {
                ProxyGenerator proxy = new ProxyGenerator();
                var service = proxy.CreateClassProxy(serviceType, interceptors);
                services.Add(serviceType, service);

                return service;
            }
        }

        /// <summary>
        /// 创建一个接口方式的拦截器
        /// </summary>
        /// <typeparam name="IServiceType"></typeparam>
        /// <typeparam name="TServiceType"></typeparam>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static IServiceType CreateProxy<IServiceType, TServiceType>(params AspectInterceptor[] interceptors)
            where TServiceType : class, IServiceType
        {
            return (IServiceType)CreateProxy<TServiceType>(interceptors);
        }
    }
}
