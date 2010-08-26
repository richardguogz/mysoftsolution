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
    public sealed class ServiceConfig
    {
        private Dictionary<string, object> services;
        public Dictionary<string, object> Services
        {
            get
            {
                return services;
            }
        }

        private static ServiceConfig singleton;
        private ServiceConfig(ServiceConfigurationSection config)
        {
            if (config == null) return;
            services = new Dictionary<string, object>();
            foreach (ServiceConfigurationElement serviceElement in config.Services)
            {
                object obj = null;
                string[] values = serviceElement.Service.Split(',');
                string assemblyName = values[1];
                Assembly ass = Assembly.Load(assemblyName);
                if (ass != null)
                {
                    obj = ass.CreateInstance(values[0]);
                }
                if (obj != null)
                {
                    services.Add(serviceElement.Key, obj);
                }
            }
        }

        public static ServiceConfig Create()
        {
            if (singleton == null)
            {
                object serviceConfig = System.Configuration.ConfigurationManager.GetSection("serviceConfig");
                ServiceConfigurationSection config = null;
                if (serviceConfig != null)
                {
                    config = (ServiceConfigurationSection)serviceConfig;
                    singleton = new ServiceConfig(config);
                }
                else
                {
                    singleton = new ServiceConfig(null);
                }
            }
            return singleton;
        }

        public TService GetService<TService>()
        {
            try
            {
                Type interfaceType = typeof(TService);
                if (!interfaceType.IsInterface)
                {
                    throw new Exception(interfaceType.FullName + "必须必须是接口类型！");
                }
                if (services.ContainsKey(interfaceType.FullName))
                {
                    return (TService)services[interfaceType.FullName];
                }
                else
                {
                    throw new Exception(interfaceType.FullName + "不存在对应的程序集！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
