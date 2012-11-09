using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySoft.Cache;
using MySoft.IoC.Configuration;
using MySoft.IoC.Logger;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;
using MySoft.Logger;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory.
    /// </summary>
    public class CastleFactory : IServerConnect, ILogable, IErrorLogable
    {
        //�߳�ͬ������
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
        private static CastleFactory singleton = null;

        #region Create Service Factory

        private CastleFactoryConfiguration config;
        private IServiceContainer container;
        private IDictionary<string, IService> proxies;
        private IServiceResolver resolver;
        private ICacheStrategy cache;
        private IServiceLog logger;

        /// <summary>
        /// Gets the proxies.
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
        public IContainer Container { get { return container; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleFactory"/> class.
        /// </summary>
        /// <param name="config">The container.</param>
        protected CastleFactory(CastleFactoryConfiguration config)
        {
            this.config = config;
            this.container = new SimpleServiceContainer(config.Type);

            container.OnLog += (log, type) =>
            {
                if (this.OnLog != null) OnLog(log, type);
            };
            container.OnError += error =>
            {
                if (OnError != null) OnError(error);
                else SimpleLog.Instance.WriteLog(error);
            };

            this.proxies = new Dictionary<string, IService>();

            if (config.Nodes.Count > 0)
            {
                foreach (var p in config.Nodes)
                {
                    if (p.Value.MaxPool < 10) throw new WarningException("Minimum pool size 10.");
                    if (p.Value.MaxPool > 500) throw new WarningException("Maximum pool size 500.");

                    RemoteProxy proxy = null;
                    if (p.Value.RespType == ResponseType.Json)
                        proxy = new InvokeProxy(p.Value, container);
                    else
                        proxy = new RemoteProxy(p.Value, container);

                    proxy.OnConnected += (sender, args) =>
                    {
                        if (OnConnected != null) OnConnected(sender, args);
                    };
                    proxy.OnDisconnected += (sender, args) =>
                    {
                        if (OnDisconnected != null) OnDisconnected(sender, args);
                    };

                    this.proxies[p.Key.ToLower()] = proxy;
                }
            }
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
                            throw new WarningException("Not find configuration section castleFactory.");

                        singleton = new CastleFactory(config);
                    }
                }
            }

            return singleton;
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
            return GetServerNode(config.Default);
        }

        /// <summary>
        /// ͨ��nodeKey���ҽڵ�
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public ServerNode GetServerNode(string nodeKey)
        {
            if (singleton.proxies.Count == 0)
            {
                throw new WarningException("Not find any server node.");
            }

            return GetServerNodes().FirstOrDefault(p => string.Compare(p.Key, nodeKey, true) == 0);
        }

        /// <summary>
        /// ��ȡ����Զ�̽ڵ�
        /// </summary>
        /// <returns></returns>
        public IList<ServerNode> GetServerNodes()
        {
            return proxies.Values.Cast<RemoteProxy>().Select(p => p.Node).ToList();
        }

        /// <summary>
        /// ע�Ỻ��
        /// </summary>
        /// <param name="cache"></param>
        public void RegisterCache(ICacheStrategy cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// ע����־����
        /// </summary>
        /// <param name="logger"></param>
        public void RegisterLogger(IServiceLog logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// ע����������
        /// </summary>
        /// <param name="resolver"></param>
        public void RegisterResolver(IServiceResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <typeparam name="IServiceInterfaceType"></typeparam>
        /// <returns></returns>
        public IServiceInterfaceType GetProxyChannel<IServiceInterfaceType>()
        {
            if (proxies.Count == 0)
            {
                //��ȡ���ط���
                var service = GetLocalService<IServiceInterfaceType>();
                if (service != null) return service;

                throw new WarningException(string.Format("Did not find the service {0}.", typeof(IServiceInterfaceType).FullName));
            }

            IService s = new DiscoverProxy(this, container);
            return GetProxyChannel<IServiceInterfaceType>(s, true);
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <returns>The service implemetation instance.</returns>
        public IServiceInterfaceType GetChannel<IServiceInterfaceType>()
        {
            if (proxies.Count == 0)
            {
                //��ȡ���ط���
                var service = GetLocalService<IServiceInterfaceType>();
                if (service != null) return service;

                throw new WarningException(string.Format("Did not find the service {0}.", typeof(IServiceInterfaceType).FullName));
            }

            return GetChannel<IServiceInterfaceType>(config.Default);
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <param name="nodeKey">The node key.</param>
        /// <returns></returns>
        public IServiceInterfaceType GetChannel<IServiceInterfaceType>(string nodeKey)
        {
            var node = GetServerNode(nodeKey);

            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}.", nodeKey));
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
                throw new WarningException("Server node can't for empty.");

            //��ȡ����ڵ�
            if (resolver != null)
            {
                node = resolver.GetServerNode<IServiceInterfaceType>(node);
            }

            //��ȡ���ط���
            var channel = GetLocalService<IServiceInterfaceType>();

            if (channel == null)
            {
                IService service = null;

                var isCacheService = true;
                if (singleton.proxies.ContainsKey(node.Key.ToLower()))
                    service = singleton.proxies[node.Key.ToLower()];
                else
                {
                    RemoteProxy proxy = null;
                    if (node.RespType == ResponseType.Json)
                        proxy = new InvokeProxy(node, container);
                    else
                        proxy = new RemoteProxy(node, container);

                    proxy.OnConnected += (sender, args) =>
                    {
                        if (OnConnected != null) OnConnected(sender, args);
                    };
                    proxy.OnDisconnected += (sender, args) =>
                    {
                        if (OnDisconnected != null) OnDisconnected(sender, args);
                    };

                    service = proxy;

                    isCacheService = false;
                }

                channel = GetProxyChannel<IServiceInterfaceType>(service, isCacheService);
            }

            //����ͨ������
            return channel;
        }

        /// <summary>
        /// Create service channel.
        /// </summary>
        /// <typeparam name="IServiceInterfaceType"></typeparam>
        /// <param name="proxy"></param>
        /// <param name="isCacheService"></param>
        /// <returns></returns>
        private IServiceInterfaceType GetProxyChannel<IServiceInterfaceType>(IService proxy, bool isCacheService)
        {
            Type serviceType = typeof(IServiceInterfaceType);
            string serviceKey = string.Format("{0}${1}", serviceType.FullName, proxy.ServiceName);

            lock (hashtable.SyncRoot)
            {
                var handler = new ServiceInvocationHandler(this.config, this.container, proxy, serviceType, cache, logger);
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

        #region �ص�����

        /// <summary>
        /// ��ȡ�ص���������
        /// </summary>
        /// <typeparam name="IPublishService"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IPublishService GetChannel<IPublishService>(object callback)
        {
            return GetChannel<IPublishService>(config.Default, callback);
        }

        /// <summary>
        /// ��ȡ�ص���������
        /// </summary>
        /// <typeparam name="IPublishService"></typeparam>
        /// <param name="nodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IPublishService GetChannel<IPublishService>(string nodeKey, object callback)
        {
            var node = GetServerNode(nodeKey);

            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}.", nodeKey));
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
                throw new WarningException("Server node can't for empty.");

            //��ȡ����ڵ�
            if (resolver != null)
            {
                node = resolver.GetServerNode<IPublishService>(node);
            }

            if (callback == null) throw new IoCException("Callback cannot be the null.");
            var contract = CoreHelper.GetMemberAttribute<ServiceContractAttribute>(typeof(IPublishService));
            if (contract != null && contract.CallbackType != null)
            {
                if (!contract.CallbackType.IsAssignableFrom(callback.GetType()))
                {
                    throw new IoCException("Callback must assignable from " + callback.GetType().FullName + ".");
                }
            }
            else
                throw new IoCException("Callback type cannot be the null.");

            CallbackProxy proxy = new CallbackProxy(callback, node, container);
            return GetProxyChannel<IPublishService>(proxy, false);
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
            if (proxies.Count == 0)
            {
                //��ȡ���ط���
                IService service = GetLocalService(message);
                if (service != null)
                {
                    return GetInvokeData(service, message);
                }

                throw new WarningException(string.Format("Did not find the service {0}.", message.ServiceName));
            }

            return Invoke(config.Default, message);
        }

        /// <summary>
        /// ���÷ֲ�ʽ����
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public InvokeData Invoke(string nodeKey, InvokeMessage message)
        {
            var node = GetServerNode(nodeKey);

            if (node == null)
            {
                throw new WarningException(string.Format("Did not find the node {0}.", nodeKey));
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
                throw new WarningException("Server node can't for empty.");

            //��ȡ���ط���
            IService service = GetLocalService(message);

            if (service == null)
            {
                if (singleton.proxies.ContainsKey(node.Key.ToLower()))
                    service = singleton.proxies[node.Key.ToLower()];
                else
                {
                    RemoteProxy proxy = null;
                    if (node.RespType == ResponseType.Json)
                        proxy = new InvokeProxy(node, container);
                    else
                        proxy = new RemoteProxy(node, container);

                    proxy.OnConnected += (sender, args) =>
                    {
                        if (OnConnected != null) OnConnected(sender, args);
                    };
                    proxy.OnDisconnected += (sender, args) =>
                    {
                        if (OnDisconnected != null) OnDisconnected(sender, args);
                    };

                    service = proxy;
                }
            }

            //��ȡ��������
            return GetInvokeData(service, message);
        }

        #endregion

        #region Private Service

        /// <summary>
        /// ��ȡ���õ�����
        /// </summary>
        /// <param name="service"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private InvokeData GetInvokeData(IService service, InvokeMessage message)
        {
            //���÷ֲ�ʽ����
            var timeout = TimeSpan.FromSeconds(ServiceConfig.DEFAULT_CLIENT_CALL_TIMEOUT * 5);
            using (var caller = new InvokeCaller(config.AppName, container, service, timeout, cache))
            {
                return caller.InvokeResponse(message);
            }
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
                var attr = CoreHelper.GetMemberAttribute<ServiceContractAttribute>(serviceType);
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (!markedWithServiceContract)
                {
                    throw ex;
                }
            }

            //���屾�ط���
            IServiceInterfaceType ls = default(IServiceInterfaceType);

            //����Ǳ������ã����׳��쳣
            if (config.Type != CastleFactoryType.Remote)
            {
                if (resolver != null)
                {
                    ls = resolver.ResolveService<IServiceInterfaceType>(container);
                }

                if (ls == null)
                {
                    var serviceKey = "Service_" + serviceType.FullName;

                    //���ط���
                    if (container.Kernel.HasComponent(serviceKey))
                    {
                        lock (hashtable.SyncRoot)
                        {
                            if (!hashtable.ContainsKey(serviceKey))
                            {
                                //���ر��ط���
                                var service = container.Resolve<IService>(serviceKey);
                                var handler = new ServiceInvocationHandler(config, container, service, serviceType, cache, logger);
                                var dynamicProxy = ProxyFactory.GetInstance().Create(handler, serviceType, true);

                                hashtable[serviceKey] = dynamicProxy;
                            }
                        }

                        ls = (IServiceInterfaceType)hashtable[serviceKey];
                    }
                }

                if (ls == null)
                {
                    if (config.Type == CastleFactoryType.Local)
                        throw new WarningException(string.Format("The local��{1}({2})��not find matching service ({0})."
                            , serviceType.FullName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress()));
                }
            }

            return ls;
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

        #region ITcpConnection ��Ա

        /// <summary>
        /// OnConnected event
        /// </summary>
        public event EventHandler<ConnectEventArgs> OnConnected;

        /// <summary>
        /// OnDisconnected event
        /// </summary>
        public event EventHandler<ConnectEventArgs> OnDisconnected;

        #endregion
    }
}
