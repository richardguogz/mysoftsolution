using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace MySoft.Service
{
    /// <summary>
    /// �������ط���
    /// </summary>
    public sealed class ServiceFactory
    {
        private Dictionary<string, object> services = new Dictionary<string, object>();
        private static ServiceFactory singleton;

        private ServiceFactory(ServiceFactoryConfiguration config)
        {
            if (config == null) return;

            foreach (ServiceBase service in config.Services)
            {
                object obj = null;
                Assembly ass = Assembly.Load(service.AssemblyName);
                if (ass != null)
                {
                    Type type = ass.GetType(service.ClassName);
                    obj = Activator.CreateInstance(type);
                }
                if (obj != null)
                {
                    services.Add(service.Name, obj);
                }
            }
        }

        /// <summary>
        /// �������񹤳�
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
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public TService GetService<TService>(string name)
        {
            try
            {
                if (services.ContainsKey(name))
                {
                    return (TService)services[name];
                }
                else
                {
                    throw new Exception(string.Format("�����ڶ�Ӧ���ơ�{0}���ĳ��򼯣�", name));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService GetService<TService>()
        {
            Type interfaceType = typeof(TService);
            if (!interfaceType.IsInterface)
            {
                throw new Exception(interfaceType.FullName + "��������ǽӿ����ͣ�");
            }

            return GetService<TService>(interfaceType.FullName);
        }
    }
}
