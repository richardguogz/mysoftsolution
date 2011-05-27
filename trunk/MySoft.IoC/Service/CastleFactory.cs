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
        private static CastleFactory singleton = null;
        private static IDictionary<Type, object> instances = new Dictionary<Type, object>();

        #region Create Service Factory

        private CastleFactoryConfiguration config;
        private IServiceContainer container;
        private IDictionary<string, IService> proxies = new Dictionary<string, IService>();

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
        /// Initializes a new instance of the <see cref="CastleFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected CastleFactory(CastleFactoryConfiguration config, IServiceContainer container)
        {
            if (config == null) config = new CastleFactoryConfiguration();
            this.config = config;
            this.container = container;
        }

        #region 创建单例

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static CastleFactory Create()
        {
            if (singleton == null)
            {
                var config = CastleFactoryConfiguration.GetConfig();
                singleton = CreateNew(config);
            }

            return singleton;
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="config"></param>
        /// <param name="name">service name</param>
        /// <returns>The service factoru new instance.</returns>
        private static CastleFactory CreateNew(CastleFactoryConfiguration config)
        {
            CastleFactory instance = null;

            //本地匹配节
            if (config == null || config.Type == CastleFactoryType.Local)
            {
                instance = new CastleFactory(config, new SimpleServiceContainer(CastleFactoryType.Local));
            }
            else
            {
                IServiceContainer container = new SimpleServiceContainer(config.Type);
                container.OnLog += (log, type) => { if (instance.OnLog != null) instance.OnLog(log, type); };
                container.OnError += (exception) => { if (instance.OnError != null) instance.OnError(exception); };

                instance = new CastleFactory(config, container);

                if (config.Nodes.Count > 0)
                {
                    foreach (var node in config.Nodes)
                    {
                        if (node.Value.MaxPool < 1) throw new WarningException("Minimum pool size 1！");
                        if (node.Value.MaxPool > 100) throw new WarningException("Maximum pool size 100！");

                        var proxy = new ProxyService(container, node.Value);
                        instance.proxies[node.Key.ToLower()] = proxy;
                    }
                }
            }

            return instance;
        }

        #endregion

        #endregion

        #region 注入缓存

        /// <summary>
        /// 注册缓存依赖
        /// </summary>
        /// <param name="cache"></param>
        public void RegisterCacheDependent(ICacheDependent cache)
        {
            this.container.Cache = cache;
        }

        #endregion

        #region Get Service

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns>The service implemetation instance.</returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>()
        {
            return GetService<IServiceInterfaceType>((string)null);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="node">The node name.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>(ServiceNode node)
        {
            if (!singleton.config.Nodes.ContainsKey(node.Key))
                singleton.config.Nodes[node.Key] = node;

            return GetService<IServiceInterfaceType>(node.Key);
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="nodeKey">The node key.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>(string nodeKey)
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

            //如果是本地配置，则抛出异常
            if (config.Type == CastleFactoryType.Local)
            {
                //本地服务
                if (container.Kernel.HasComponent(typeof(IServiceInterfaceType)))
                {
                    var service = container[typeof(IServiceInterfaceType)];

                    //返回拦截服务
                    return AspectManager.GetService<IServiceInterfaceType>(service);
                }

                throw new WarningException(string.Format("Local not find service ({0}).", typeof(IServiceInterfaceType).FullName));
            }
            else
            {
                Type serviceType = typeof(IServiceInterfaceType);
                if (instances.ContainsKey(serviceType))
                {
                    return (IServiceInterfaceType)instances[serviceType];
                }
                else
                {
                    lock (syncObj)
                    {
                        IService service = container.GetLocalService(typeof(IServiceInterfaceType).FullName);
                        if (service == null)
                        {
                            if (singleton.proxies.Count == 0)
                            {
                                throw new WarningException("Not find any service node！");
                            }

                            if (string.IsNullOrEmpty(nodeKey)) nodeKey = config.Default;

                            //如果不存在当前配置节，则使用默认配置节
                            if (!singleton.proxies.ContainsKey(nodeKey.ToLower()))
                            {
                                nodeKey = "default";
                            }

                            if (singleton.proxies.ContainsKey(nodeKey.ToLower()))
                            {
                                service = singleton.proxies[nodeKey.ToLower()];
                            }
                            else
                            {
                                throw new WarningException("Not find the service node [" + nodeKey + "]！");
                            }
                        }

                        var handler = new ServiceInvocationHandler(this.config, this.container, service, serviceType);
                        var dynamicProxy = (IServiceInterfaceType)DynamicProxy.NewInstance(AppDomain.CurrentDomain, new Type[] { serviceType }, handler);
                        instances[serviceType] = dynamicProxy;

                        return dynamicProxy;
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
