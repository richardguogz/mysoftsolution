using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace MySoft.Core.Service
{
    /// <summary>
    /// 创建本地服务
    /// </summary>
    public sealed class ServiceFactory
    {
        private Dictionary<string, object> services = new Dictionary<string, object>();
        private static ServiceFactory singleton;

        private ServiceFactory(ServiceFactoryConfiguration config)
        {
            if (config == null) return;

            foreach (ServiceConfig service in config.Services)
            {
                object obj = null;
                string[] values = service.Service.Split(',');
                if (values.Length < 2) continue;

                string assemblyName = values[1];
                Assembly ass = Assembly.Load(assemblyName);
                if (ass != null)
                {
                    obj = ass.CreateInstance(values[0]);
                }
                if (obj != null)
                {
                    services.Add(service.Key, obj);
                }
            }
        }

        /// <summary>
        /// 创建服务工厂
        /// </summary>
        /// <returns></returns>
        public static ServiceFactory Create()
        {
            if (singleton == null)
            {
                ServiceFactoryConfiguration config = ServiceFactoryConfiguration.GetConfig();
                if (config != null)
                {
                    singleton = new ServiceFactory(config);
                }
                else
                {
                    singleton = new ServiceFactory(null);
                }
            }
            return singleton;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TService GetService<TService>(string key)
        {
            try
            {
                if (services.ContainsKey(key))
                {
                    return (TService)services[key];
                }
                else
                {
                    throw new Exception(string.Format("不存在对应KEY【{0}】的程序集！", key));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService GetService<TService>()
        {
            Type interfaceType = typeof(TService);
            if (!interfaceType.IsInterface)
            {
                throw new Exception(interfaceType.FullName + "必须必须是接口类型！");
            }

            return GetService<TService>(interfaceType.FullName);
        }
    }
}
