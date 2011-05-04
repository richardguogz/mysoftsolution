using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Core;
using Castle.Facilities.Startable;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using MySoft.IoC.Services;
using Castle.MicroKernel;
using System.Text;
using System.IO;
using MySoft.Logger;
using MySoft.Cache;
using System.Reflection;

namespace MySoft.IoC
{
    /// <summary>
    /// The simple service container.
    /// </summary>
    public sealed class SimpleServiceContainer : IServiceContainer
    {
        #region Const Members

        /// <summary>
        /// The default timeout number. 
        /// </summary>
        public const int DEFAULT_TIMEOUT_NUMBER = 10000;

        /// <summary>
        /// The default cachetime number.
        /// </summary>
        public const int DEFAULT_CACHETIME_NUMBER = 60000;

        /// <summary>
        /// The default showlogtime number.
        /// </summary>
        public const int DEFAULT_SHOWLOGTIME_NUMBER = 1000;

        /// <summary>
        /// The default maxconnect number.
        /// </summary>
        public const int DEFAULT_MAXCONNECT_NUMBER = 10000;

        /// <summary>
        /// The default maxbuffer number.
        /// </summary>
        public const int DEFAULT_MAXBUFFER_NUMBER = 4096;

        #endregion

        #region Private Members

        private IWindsorContainer container;
        private IServiceProxy serviceProxy;
        private ICacheDependent cache;
        private int showlogtime;

        private void Init(int showlogtime, IDictionary serviceKeyTypes)
        {
            this.showlogtime = showlogtime;
            if (System.Configuration.ConfigurationManager.GetSection("castle") != null)
            {
                container = new WindsorContainer(new XmlInterpreter());
            }
            else
            {
                container = new WindsorContainer();
            }

            //加载自启动注入
            container.AddFacility("startable", new StartableFacility());

            if (serviceKeyTypes != null && serviceKeyTypes.Count > 0)
            {
                RegisterComponents(serviceKeyTypes);
            }

            this.DiscoverServices();
        }

        private ServiceNodeInfo[] ParseServiceNodes(GraphNode[] nodes)
        {
            if (nodes == null)
            {
                return null;
            }

            List<ServiceNodeInfo> serviceNodes = new List<ServiceNodeInfo>();

            for (int i = 0; i < nodes.Length; i++)
            {
                ComponentModel node = (ComponentModel)nodes[i];
                if (typeof(IService).IsAssignableFrom(node.Service))
                {
                    ServiceNodeInfo serviceNode = new ServiceNodeInfo();
                    serviceNode.Key = node.Name;
                    serviceNode.Sevice = node.Service.FullName;
                    serviceNode.Implementation = node.Implementation.FullName;
                    serviceNodes.Add(serviceNode);
                }
            }

            return serviceNodes.ToArray();
        }

        private ComponentModel GetComponentModelByKey(string key)
        {
            GraphNode[] nodes = Kernel.GraphNodes;
            foreach (ComponentModel node in nodes)
            {
                if (node.Name.Equals(key))
                {
                    return node;
                }
            }
            return null;
        }

        private IService GetLocalService(string serviceName)
        {
            ServiceNodeInfo[] serviceNodes = GetServiceNodes();
            foreach (ServiceNodeInfo serviceNode in serviceNodes)
            {
                IService obj = (IService)container[serviceNode.Key];
                if ((obj).ServiceName == serviceName)
                {
                    return obj;
                }
            }

            return null;
        }

        private void DiscoverServices()
        {
            GraphNode[] nodes = this.Kernel.GraphNodes;
            foreach (ComponentModel model in nodes)
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(model.Service);
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (markedWithServiceContract)
                {
                    DynamicService service = new DynamicService(this, model.Service);
                    Kernel.AddComponentInstance(service.ServiceName, service);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleServiceContainer"/> class.
        /// </summary>
        /// <param name="config"></param>
        public SimpleServiceContainer(int showlogtime)
        {
            Init(showlogtime, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleServiceContainer"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serviceKeyTypes">The service key types.</param>
        public SimpleServiceContainer(int showlogtime, IDictionary serviceKeyTypes)
        {
            Init(showlogtime, serviceKeyTypes);
        }

        #endregion

        #region IServiceContainer Members

        /// <summary>
        /// 设置服务代理
        /// </summary>
        public IServiceProxy Proxy
        {
            get
            {
                return this.serviceProxy;
            }
            set
            {
                this.serviceProxy = value;
            }
        }

        /// <summary>
        /// 缓存依赖
        /// </summary>
        public ICacheDependent Cache
        {
            get
            {
                return this.cache;
            }
            set
            {
                this.cache = value;
            }
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public IKernel Kernel
        {
            get { return container.Kernel; }
        }

        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the class.</param>
        public void RegisterComponent(string key, Type serviceType, Type classType)
        {
            container.AddComponent(key, serviceType, classType);
        }

        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="classType">Type of the class.</param>
        public void RegisterComponent(string key, Type classType)
        {
            container.AddComponent(key, typeof(IService), classType);
        }

        /// <summary>
        /// Registers the components.
        /// </summary>
        /// <param name="serviceKeyTypes">The service key types.</param>
        public void RegisterComponents(IDictionary serviceKeyTypes)
        {
            System.Collections.IDictionaryEnumerator en = serviceKeyTypes.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Value != null)
                {
                    DynamicService service = new DynamicService(this, (Type)en.Key, en.Value);
                    Kernel.AddComponentInstance(service.ServiceName, service);
                }
            }
        }

        /// <summary>
        /// Releases the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Release(object obj)
        {
            container.Release(obj);
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        public object this[string key]
        {
            get { return container[key]; }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified service type.
        /// </summary>
        /// <value></value>
        public object this[Type serviceType]
        {
            get { return container[serviceType]; }
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            //check local service first
            IService localService = (IService)GetLocalService(reqMsg.ServiceName);
            if (localService != null)
            {
                return localService.CallService(reqMsg, showlogtime);
            }

            //判断代理是否为空
            if (serviceProxy == null)
            {
                throw new IoCException(string.Format("Call remote service failure, serviceProxy undefined！({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName));
            }
            else
            {
                try
                {
                    //如果需要重连接
                    if (serviceProxy.NeedConnect)
                    {
                        serviceProxy.ConnectServer();
                    }

                    //通过代理调用
                    return serviceProxy.CallMethod(reqMsg, showlogtime);
                }
                catch (Exception ex)
                {
                    WriteError(ex);

                    //如果要抛出错误，则抛出
                    if (serviceProxy.ThrowError)
                        throw ex;
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Gets the service nodes.
        /// </summary>
        /// <returns>The service nodes.</returns>
        public ServiceNodeInfo[] GetServiceNodes()
        {
            return ParseServiceNodes(Kernel.GraphNodes);
        }

        /// <summary>
        /// Gets the depender service nodes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The service nodes.</returns>
        public ServiceNodeInfo[] GetDependerServiceNodes(string key)
        {
            ComponentModel node = GetComponentModelByKey(key);
            if (node == null)
            {
                return null;
            }
            return ParseServiceNodes(node.Dependers);
        }

        /// <summary>
        /// Gets the dependent service nodes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The service nodes.</returns>
        public ServiceNodeInfo[] GetDependentServiceNodes(string key)
        {
            ComponentModel node = GetComponentModelByKey(key);
            if (node == null)
            {
                return null;
            }
            return ParseServiceNodes(node.Dependents);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
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

        #region IServiceContainer 成员

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="log"></param>
        public void WriteLog(string log, LogType type)
        {
            if (OnLog != null) OnLog(log, type);
        }

        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="exception"></param>
        public void WriteError(Exception exception)
        {
            if (OnError != null) OnError(exception);
        }

        #endregion
    }
}
