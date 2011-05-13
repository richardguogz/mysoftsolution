using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MySoft.Net.Client;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory.
    /// </summary>
    public class CastleFactory : ILogable, IErrorLogable
    {
        private static object syncObj = new object();

        #region Create Service Factory

        private IServiceContainer container;
        private CastleFactoryConfiguration configuration;

        /// <summary>
        /// Gets the service container.
        /// </summary>
        /// <value>The service container.</value>
        public IServiceContainer ServiceContainer
        {
            get
            {
                return container;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected CastleFactory(IServiceContainer container)
        {
            if (container == null)
            {
                this.container = new SimpleServiceContainer(SimpleServiceContainer.DEFAULT_LOGTIME_NUMBER);
            }
            else
            {
                this.container = container;
            }
        }

        private void WriteLog(string log, LogType type)
        {
            if (OnLog != null) OnLog(log, type);
        }

        private static CastleFactory singleton = null;
        private static IDictionary<string, CastleFactory> services = new Dictionary<string, CastleFactory>();

        #region ��������

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static CastleFactory Create()
        {
            if (singleton == null)
            {
                singleton = CreateNew();
            }

            return singleton;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CastleFactory Create(string name)
        {
            if (!services.ContainsKey(name))
            {
                services[name] = CreateNew(name);
            }

            return services[name];
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static CastleFactory Create(ServiceNode node)
        {
            if (!services.ContainsKey(node.Name))
            {
                services[node.Name] = CreateNew(node.Name);
            }

            return services[node.Name];
        }

        #endregion

        #region ������ʵ��

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <returns></returns>
        private static CastleFactory CreateNew()
        {
            var config = CastleFactoryConfiguration.GetConfig();
            return CreateNew(config, config.Default);
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="name">service name</param>
        /// <returns></returns>
        private static CastleFactory CreateNew(string name)
        {
            var config = CastleFactoryConfiguration.GetConfig();
            return CreateNew(config, name);
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The service factoru new instance.</returns>
        private static CastleFactory CreateNew(ServiceNode node)
        {
            var config = CastleFactoryConfiguration.GetConfig();
            config.Hosts.Add(node.Name, node); //��ӽڵ�
            return CreateNew(config, node.Name);
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="config"></param>
        /// <param name="name">service name</param>
        /// <returns>The service factoru new instance.</returns>
        private static CastleFactory CreateNew(CastleFactoryConfiguration config, string name)
        {
            CastleFactory instance = null;

            //����ƥ���
            if (config == null || config.Type == CastleFactoryType.Local)
            {
                IServiceContainer container = null;
                if (config == null)
                    container = new SimpleServiceContainer(SimpleServiceContainer.DEFAULT_LOGTIME_NUMBER);
                else
                    container = new SimpleServiceContainer(config.ShowlogTime);

                instance = new CastleFactory(container);
            }
            else
            {
                bool isProxy = true;
                if (config.Hosts.Count == 0 && config.Type == CastleFactoryType.LocalRemote)
                {
                    isProxy = false;
                }

                IServiceContainer container = new SimpleServiceContainer(config.ShowlogTime);
                if (isProxy)
                {
                    if (!config.Hosts.ContainsKey(name))
                    {
                        throw new IoCException("Not find the service node [" + name + "]��");
                    }

                    if (config.MaxPool < 1) throw new IoCException("Minimum pool size 1��");
                    if (config.MaxPool > 500) throw new IoCException("Maximum pool size 500��");

                    var serviceNode = config.Hosts[name];

                    //�ͻ�������
                    SocketClientConfiguration clientconfig = new SocketClientConfiguration();
                    clientconfig.IP = serviceNode.Server;
                    clientconfig.Port = serviceNode.Port;
                    clientconfig.Pools = config.MaxPool;

                    //���÷������
                    var proxy = new ServiceProxy(clientconfig, (serviceNode.Description ?? serviceNode.Name));
                    container.Proxy = proxy;

                    //���ô�����¼�ί��
                    proxy.OnLog += (log, type) =>
                    {
                        if (instance.OnLog != null)
                        {
                            instance.OnLog(log, type);
                        }
                    };
                }

                instance = new CastleFactory(container);

                #region �����¼�ί��

                container.OnLog += (log, type) =>
                {
                    if (instance.OnLog != null)
                    {
                        instance.OnLog(log, type);
                    }
                };

                container.OnError += (exception) =>
                {
                    if (instance.OnError != null)
                    {
                        instance.OnError(exception);
                    }
                };

                #endregion
            }

            //�������ý�
            instance.configuration = config;

            return instance;
        }

        #endregion

        #endregion

        #region ע�뻺��

        /// <summary>
        /// ע�뻺������
        /// </summary>
        /// <param name="cache"></param>
        public void InjectCacheDependent(ICacheDependent cache)
        {
            this.container.Cache = cache;
        }

        #endregion

        #region Get Service

        /// <summary>
        /// Gets the status service.
        /// </summary>
        /// <returns></returns>
        public IStatusService StatusService
        {
            get
            {
                return GetService<IStatusService>();
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns>The service implemetation instance.</returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>()
        {
            return GetService<IServiceInterfaceType>(null);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>(string key)
        {
            Exception ex = new ArgumentException("Generic parameter type - IServiceInterfaceType must be an interface marked with ServiceContractAttribute.");
            if (!typeof(IServiceInterfaceType).IsInterface)
            {
                throw ex;
            }
            else
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(typeof(IServiceInterfaceType));
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (!markedWithServiceContract)
                {
                    throw ex;
                }
            }

            //����Ǳ������ã����׳��쳣
            if (configuration == null || configuration.Type == CastleFactoryType.Local)
            {
                //���ط���
                if (!string.IsNullOrEmpty(key))
                {
                    if (container.Kernel.HasComponent(key))
                    {
                        var service = container[key];

                        //�������ط���
                        return AspectManager.GetService<IServiceInterfaceType>(service);
                    }
                }
                else
                {
                    if (container.Kernel.HasComponent(typeof(IServiceInterfaceType)))
                    {
                        var service = container[typeof(IServiceInterfaceType)];

                        //�������ط���
                        return AspectManager.GetService<IServiceInterfaceType>(service);
                    }
                }

                throw new IoCException(string.Format("Local not find service ({0}).", typeof(IServiceInterfaceType).FullName));
            }
            else
            {
                string serviceKey = "CastleFactory_" + typeof(IServiceInterfaceType).FullName;
                if (container.Kernel.HasComponent(serviceKey))
                {
                    return (IServiceInterfaceType)container[serviceKey];
                }
                else
                {
                    lock (syncObj)
                    {
                        var type = typeof(IServiceInterfaceType);
                        var handler = new ServiceInvocationHandler(this.configuration, this.container, type);
                        var service = (IServiceInterfaceType)DynamicProxy.NewInstance(AppDomain.CurrentDomain, new Type[] { type }, handler);
                        container.Kernel.AddComponentInstance(serviceKey, service);

                        return service;
                    }
                }
            }
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        #endregion

        #region IErrorLogable Members

        /// <summary>
        /// OnError event.
        /// </summary>
        public event ErrorLogEventHandler OnError;

        #endregion
    }
}
