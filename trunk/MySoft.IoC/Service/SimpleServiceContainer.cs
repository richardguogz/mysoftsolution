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
using System.Linq;
using MySoft.Logger;
using MySoft.Cache;
using System.Reflection;
using System.Configuration;

namespace MySoft.IoC
{
    /// <summary>
    /// The simple service container.
    /// </summary>
    public sealed class SimpleServiceContainer : IServiceContainer
    {
        #region Private Members

        private IWindsorContainer container;
        private ICacheDependent cache;

        private void Init(CastleFactoryType type, IDictionary serviceKeyTypes)
        {
            //如果不是远程模式，则加载配置节
            if (type == CastleFactoryType.Remote || ConfigurationManager.GetSection("castle") == null)
                container = new WindsorContainer();
            else
                container = new WindsorContainer(new XmlInterpreter());

            //加载自启动注入
            container.AddFacility("startable", new StartableFacility());

            if (serviceKeyTypes != null && serviceKeyTypes.Count > 0)
            {
                RegisterComponents(serviceKeyTypes);
            }

            this.DiscoverServices();
        }

        private void DiscoverServices()
        {
            foreach (Type type in GetContractInterfaces())
            {
                object serviceInstance = null;
                try { serviceInstance = this[type]; }
                catch { }

                if (serviceInstance != null)
                {
                    IService service = new DynamicService(this, type, serviceInstance);
                    RegisterComponent(service.ServiceName, service);

                    if (serviceInstance is IStartable)
                    {
                        RegisterComponent("Startable_" + service.ServiceName, serviceInstance.GetType());
                    }
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleServiceContainer"/> class.
        /// </summary>
        /// <param name="config"></param>
        public SimpleServiceContainer(CastleFactoryType type)
        {
            Init(type, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleServiceContainer"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serviceKeyTypes">The service key types.</param>
        public SimpleServiceContainer(CastleFactoryType type, IDictionary serviceKeyTypes)
        {
            Init(type, serviceKeyTypes);
        }

        #endregion

        #region IServiceContainer Members

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
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="instance">Type of the class.</param>
        public void RegisterComponent(string key, object instance)
        {
            container.Kernel.AddComponentInstance(key, instance);
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
                    IService service = new DynamicService(this, (Type)en.Key, en.Value);
                    RegisterComponent(service.ServiceName, service);

                    if (en.Value is IStartable)
                    {
                        RegisterComponent("Startable_" + service.ServiceName, en.Value.GetType());
                    }
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
        public ResponseMessage CallService(RequestMessage reqMsg, double logtime)
        {
            //check local service first
            IService localService = GetLocalService(reqMsg.ServiceName);
            if (localService == null)
            {
                throw new WarningException(string.Format("The server not find matching service ({0}).", reqMsg.ServiceName))
                {
                    ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }
            return localService.CallService(reqMsg, logtime);
        }

        /// <summary>
        /// 获取约束的接口
        /// </summary>
        /// <returns></returns>
        public Type[] GetContractInterfaces()
        {
            List<Type> typelist = new List<Type>();
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
                    typelist.Add(model.Service);
                }
            }
            return typelist.ToArray();
        }

        /// <summary>
        /// get local service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IService GetLocalService(string serviceName)
        {
            ServiceNodeInfo[] serviceNodes = ParseServiceNodes(Kernel.GraphNodes);
            foreach (ServiceNodeInfo serviceNode in serviceNodes)
            {
                var obj = container[serviceNode.Key];
                if (obj is IService)
                {
                    IService service = (IService)obj;
                    if (service.ServiceName == serviceName)
                    {
                        return service;
                    }
                }
            }

            return null;
        }

        private ServiceNodeInfo[] ParseServiceNodes(GraphNode[] nodes)
        {
            List<ServiceNodeInfo> serviceNodes = new List<ServiceNodeInfo>();
            if (nodes == null) return serviceNodes.ToArray();

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
            try { if (OnLog != null) OnLog(log, type); }
            catch { }
        }

        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="exception"></param>
        public void WriteError(Exception exception)
        {
            try { if (OnError != null) OnError(exception); }
            catch { }
        }

        #endregion
    }
}
