using System;
using System.Collections.Generic;
using System.Linq;
using MySoft.Cache;
using MySoft.IoC.Cache;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;
using MySoft.Logger;
using System.Collections;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory.
    /// </summary>
    public class CastleFactory : ILogable, IErrorLogable
    {
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
        private static CastleFactory singleton = null;

        #region Create Service Factory

        private CastleFactoryConfiguration config;
        private IServiceContainer container;
        private DiscoverProxy discoverProxy;
        private IDictionary<string, IService> proxies;
        private ICacheStrategy cache;

        /// <summary>
        /// ���д�����Ϣ
        /// </summary>
        internal IList<RemoteProxy> Proxies
        {
            get
            {
                return proxies.Values.Cast<RemoteProxy>().ToList();
            }
        }

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
            if (config == null)
                this.config = new CastleFactoryConfiguration();
            else
                this.config = config;
            this.container = container;
            this.discoverProxy = new DiscoverProxy(this, container);
            this.proxies = new Dictionary<string, IService>();
        }

        #region ��������

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static CastleFactory Create()
        {
            if (singleton == null)
            {
                lock (hashtable.SyncRoot)
                {
                    if (singleton == null)
                    {
                        var config = CastleFactoryConfiguration.GetConfig();

                        if (config == null)
                            throw new WarningException("Not find configuration section castleFactory��");

                        singleton = CreateNew(config);
                    }
                }
            }

            return singleton;
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The service factoru new instance.</returns>
        private static CastleFactory CreateNew(CastleFactoryConfiguration config)
        {
            CastleFactory factory = null;

            //����ƥ���
            if (config.Type == CastleFactoryType.Local)
            {
                var sContainer = new SimpleServiceContainer(CastleFactoryType.Local);
                factory = new CastleFactory(config, sContainer);
            }
            else
            {
                var sContainer = new SimpleServiceContainer(config.Type);
                sContainer.OnLog += (log, type) =>
                {
                    if (factory != null && factory.OnLog != null)
                        factory.OnLog(log, type);
                };
                sContainer.OnError += error =>
                {
                    if (factory != null && factory.OnError != null)
                        factory.OnError(error);
                };

                factory = new CastleFactory(config, sContainer);

                if (config.Nodes.Count > 0)
                {
                    foreach (var p in config.Nodes)
                    {
                        if (p.Value.MaxPool < 1) throw new WarningException("Minimum pool size 1��");
                        if (p.Value.MaxPool > 1000) throw new WarningException("Maximum pool size 1000��");

                        IService proxy = null;
                        if (p.Value.Invoke)
                            proxy = new InvokeProxy(p.Value, sContainer);
                        else
                            proxy = new RemoteProxy(p.Value, sContainer);

                        factory.proxies[p.Key.ToLower()] = proxy;
                    }
                }
            }

            return factory;
        }

        #endregion

        #endregion

        #region Get Service

        /// <summary>
        /// ��ȡĬ�ϵĽڵ�
        /// </summary>
        /// <returns></returns>
        public ServerNode GetDefaultNode()
        {
            return GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, config.Default, true) == 0);
        }

        /// <summary>
        /// ͨ��nodeKey���ҽڵ�
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public ServerNode GetServerNode(string nodeKey)
        {
            return GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, nodeKey, true) == 0);
        }

        /// <summary>
        /// ��ȡ����Զ�̽ڵ�
        /// </summary>
        /// <returns></returns>
        public IList<ServerNode> GetServerNodes()
        {
            return this.Proxies.Select(p => p.Node).ToList();
        }

        /// <summary>
        /// ע�Ỻ��
        /// </summary>
        /// <param name="cache"></param>
        public void RegisterCache(ICacheStrategy cache)
        {
            //������������ΪӦ������
            cache.SetRegionName(config.AppName);

            this.cache = cache;
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <returns>The service implemetation instance.</returns>
        public IServiceInterfaceType GetChannel<IServiceInterfaceType>()
        {
            var service = GetLocalService<IServiceInterfaceType>();
            if (service != null) return service;

            return GetChannel<IServiceInterfaceType>(discoverProxy, true);
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <param name="nodeKey">The node key.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetChannel<IServiceInterfaceType>(string nodeKey)
        {
            nodeKey = GetNodeKey(nodeKey);
            var node = GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, nodeKey, true) == 0);
            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}!", nodeKey));
            }

            return GetChannel<IServiceInterfaceType>(node);
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <param name="node">The node name.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetChannel<IServiceInterfaceType>(ServerNode node)
        {
            if (node == null)
                throw new WarningException("Server node can't for empty!");

            IService proxy = null;
            var isCacheService = true;
            if (singleton.proxies.ContainsKey(node.Key.ToLower()))
                proxy = singleton.proxies[node.Key.ToLower()];
            else
            {
                if (node.Invoke)
                    proxy = new InvokeProxy(node, container);
                else
                    proxy = new RemoteProxy(node, container);

                isCacheService = false;
            }

            return GetChannel<IServiceInterfaceType>(proxy, isCacheService);
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <typeparam name="IServiceInterfaceType"></typeparam>
        /// <param name="proxy"></param>
        /// <param name="isCacheService"></param>
        /// <returns></returns>
        private IServiceInterfaceType GetChannel<IServiceInterfaceType>(IService proxy, bool isCacheService)
        {
            Type serviceType = typeof(IServiceInterfaceType);
            string serviceKey = string.Format("{0}${1}", serviceType.FullName, proxy.ServiceName);

            lock (hashtable.SyncRoot)
            {
                var serviceCache = new CastleServiceCache(cache);
                var handler = new ServiceInvocationHandler(this.config, this.container, proxy, serviceType, serviceCache);
                var dynamicProxy = ProxyFactory.GetInstance().Create(handler, serviceType, true);

                if (!isCacheService) //�����棬ֱ�ӷ��ط���
                    return (IServiceInterfaceType)dynamicProxy;

                if (!hashtable.ContainsKey(serviceKey))
                {
                    hashtable[serviceKey] = dynamicProxy;
                }
            }

            return (IServiceInterfaceType)hashtable[serviceKey];
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        /// <summary>
        /// OnError event.
        /// </summary>
        public event ErrorLogEventHandler OnError;

        #endregion

        #region �ص�����

        /// <summary>
        /// ��ȡ�ص���������
        /// </summary>
        /// <typeparam name="IPublishService"></typeparam>
        /// <param name="nodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IPublishService GetChannel<IPublishService>(string nodeKey, object callback)
        {
            nodeKey = GetNodeKey(nodeKey);
            var node = GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, nodeKey, true) == 0);
            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}!", nodeKey));
            }
            return GetChannel<IPublishService>(node, callback);
        }

        /// <summary>
        /// ��ȡ�ص���������
        /// </summary>
        /// <typeparam name="IPublishService"></typeparam>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IPublishService GetChannel<IPublishService>(ServerNode node, object callback)
        {
            if (node == null)
                throw new WarningException("Server node can't for empty!");

            if (callback == null) throw new IoCException("Callback cannot be the null!");
            var contract = CoreHelper.GetMemberAttribute<ServiceContractAttribute>(typeof(IPublishService));
            if (contract != null && contract.CallbackType != null)
            {
                if (!contract.CallbackType.IsAssignableFrom(callback.GetType()))
                {
                    throw new IoCException("Callback must assignable from " + callback.GetType().FullName + "!");
                }
            }
            else
                throw new IoCException("Callback type cannot be the null!");

            CallbackProxy proxy = new CallbackProxy(callback, node, this.ServiceContainer);
            return GetChannel<IPublishService>(proxy, false);
        }

        #endregion

        #region Invoke ��ʽ����

        /// <summary>
        /// ���÷ֲ�ʽ����
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public InvokeData Invoke(InvokeMessage message)
        {
            IService service = GetLocalService(message);
            if (service == null)
            {
                //���ط���Ϊnull����ʹ�÷��ַ������
                service = discoverProxy;
            }

            return GetInvokeData(message, service);
        }

        /// <summary>
        /// ���÷ֲ�ʽ����
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public InvokeData Invoke(string nodeKey, InvokeMessage message)
        {
            nodeKey = GetNodeKey(nodeKey);
            var node = GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, nodeKey, true) == 0);
            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}!", nodeKey));
            }

            return Invoke(node, message);
        }

        /// <summary>
        /// ���÷ֲ�ʽ����
        /// </summary>
        /// <param name="node"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public InvokeData Invoke(ServerNode node, InvokeMessage message)
        {
            if (node == null)
                throw new WarningException("Server node can't for empty!");

            IService service = null;
            if (singleton.proxies.ContainsKey(node.Key.ToLower()))
                service = singleton.proxies[node.Key.ToLower()];
            else
            {
                if (node.Invoke)
                    service = new InvokeProxy(node, container);
                else
                    service = new RemoteProxy(node, container);
            }

            return GetInvokeData(message, service);
        }

        #endregion

        #region Private Service

        /// <summary>
        /// ��ȡ���õ�����
        /// </summary>
        /// <param name="message"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        private InvokeData GetInvokeData(InvokeMessage message, IService service)
        {
            //���÷ֲ�ʽ����
            var caller = new InvokeCaller(config.AppName, service);
            return caller.CallMethod(message);
        }

        /// <summary>
        /// ��ȡ���ط���
        /// </summary>
        /// <typeparam name="IServiceInterfaceType"></typeparam>
        /// <returns></returns>
        private IServiceInterfaceType GetLocalService<IServiceInterfaceType>()
        {
            Type serviceType = typeof(IServiceInterfaceType);

            //�����쳣
            Exception ex = new ArgumentException("Generic parameter type - ��" + serviceType.FullName
                   + "�� must be an interface marked with ServiceContractAttribute.");

            if (!serviceType.IsInterface)
            {
                throw ex;
            }
            else
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceType);
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
            if (config.Type != CastleFactoryType.Remote)
            {
                //���ط���
                if (container.Kernel.HasComponent(serviceType))
                {
                    lock (hashtable.SyncRoot)
                    {
                        if (!hashtable.ContainsKey(serviceType))
                        {
                            //���ر��ط���
                            var serviceCache = new CastleServiceCache(cache);
                            var handler = new LocalInvocationHandler(config, container, serviceType, serviceCache);
                            var dynamicProxy = ProxyFactory.GetInstance().Create(handler, serviceType, true);

                            hashtable[serviceType] = dynamicProxy;
                        }
                    }

                    return (IServiceInterfaceType)hashtable[serviceType];
                }

                if (config.Type == CastleFactoryType.Local)
                    throw new WarningException(string.Format("Local not find service ({0}).", serviceType.FullName));
            }

            return default(IServiceInterfaceType);
        }

        /// <summary>
        /// ��ȡ�ڵ�����
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        private string GetNodeKey(string nodeKey)
        {
            if (singleton.proxies.Count == 0)
            {
                throw new WarningException("Not find any service node��");
            }

            //��������ڵ�ǰ���ýڣ���ʹ��Ĭ�����ý�
            if (string.IsNullOrEmpty(nodeKey) || !singleton.proxies.ContainsKey(nodeKey.ToLower()))
            {
                nodeKey = config.Default;
            }

            return nodeKey;
        }

        /// <summary>
        /// ��ȡ���ط���
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private IService GetLocalService(InvokeMessage message)
        {
            IService service = null;
            string serviceKey = "Service_" + message.ServiceName;
            if (config.Type != CastleFactoryType.Remote)
            {
                if (container.Kernel.HasComponent(serviceKey))
                {
                    service = container.Resolve<IService>(serviceKey);
                }

                if (service == null && config.Type == CastleFactoryType.Local)
                    throw new WarningException(string.Format("Local not find service ({0}).", message.ServiceName));
            }

            return service;
        }

        #endregion
    }
}
