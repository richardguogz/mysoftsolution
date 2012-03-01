using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Core.Resource;
using Castle.Facilities.Startable;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;
using MySoft.Logger;

namespace MySoft.IoC
{
    /// <summary>
    /// The simple service container.
    /// </summary>
    public sealed class SimpleServiceContainer : IServiceContainer
    {
        #region Private Members

        private IWindsorContainer container;
        private void Init(CastleFactoryType type, IDictionary serviceKeyTypes)
        {
            //�������Զ��ģʽ����������ý�
            if (type == CastleFactoryType.Remote || ConfigurationManager.GetSection("mysoft.framework/castle") == null)
                container = new WindsorContainer();
            else
                container = new WindsorContainer(new XmlInterpreter(new ConfigResource("mysoft.framework/castle")));

            //����������ע��
            container.AddFacility("startable", new StartableFacility());

            //�Ƚ�������
            this.DiscoverServices();

            if (serviceKeyTypes != null && serviceKeyTypes.Count > 0)
            {
                RegisterComponents(serviceKeyTypes);
            }
        }

        private void DiscoverServices()
        {
            foreach (var model in GetComponentModels<ServiceContractAttribute>())
            {
                //�ж�ʵ���Ƿ�ӽӿڷ���
                IService service = new DynamicService(this, model.Service, null);
                if (typeof(Castle.Core.IStartable).IsAssignableFrom(model.Implementation))
                {
                    RegisterComponent("Startable_" + service.ServiceName, model.Service, model.Implementation);
                }
                RegisterComponent("Service_" + service.ServiceName, typeof(IService), service);
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
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public IKernel Kernel
        {
            get { return container.Kernel; }
        }

        private IServiceCache serviceCache;
        /// <summary>
        /// Get the cache
        /// </summary>
        public IServiceCache ServiceCache
        {
            get { return serviceCache; }
            set { serviceCache = value; }
        }

        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the service.</param>
        public void RegisterComponent(string key, Type serviceType, Type classType)
        {
            container.Register(Component.For(serviceType).Named(key).ImplementedBy(classType).LifeStyle.Singleton);
        }

        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        public void RegisterComponent(string key, Type serviceType, object instance)
        {
            container.Register(Component.For(serviceType).Named(key).Instance(instance).LifeStyle.Singleton);
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
                    if (en.Value is Castle.Core.IStartable)
                    {
                        RegisterComponent("Startable_" + service.ServiceName, (Type)en.Key, en.Value.GetType());
                    }
                    RegisterComponent("Service_" + service.ServiceName, (Type)en.Key, service);
                }
            }
        }

        /// <summary>
        /// Releases the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Release(object obj)
        {
            if (obj != null)
            {
                container.Release(obj);
            }
        }

        /// <summary>
        /// Resolve local service
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        /// <summary>
        /// Resolve local service
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Resolve(string key)
        {
            return container.Resolve<object>(key);
        }

        /// <summary>
        /// Resolve local service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Resolve<TService>()
        {
            return container.Resolve<TService>();
        }

        /// <summary>
        /// Resolve local service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Resolve<TService>(string key)
        {
            return container.Resolve<TService>(key);
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            IService service = container.ResolveAll<IService>()
              .SingleOrDefault(model => model.ServiceName == reqMsg.ServiceName);

            if (service == null)
            {
                string body = string.Format("The server not find matching service ({0}).", reqMsg.ServiceName);
                throw new WarningException(body)
                {
                    ApplicationName = reqMsg.AppName,
                    ServiceName = reqMsg.ServiceName,
                    ErrorHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }

            return service.CallService(reqMsg);
        }

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool Contains<ContractType>(string serviceName)
        {
            GraphNode[] nodes = this.Kernel.GraphNodes;
            return nodes.Cast<ComponentModel>().Any(model =>
              {
                  bool markedWithServiceContract = false;
                  var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Service);
                  if (attr != null)
                  {
                      markedWithServiceContract = true;
                  }

                  return markedWithServiceContract
                      && model.Service.FullName == serviceName;
              });
        }

        /// <summary>
        /// ��ȡԼ����ʵ��
        /// </summary>
        /// <typeparam name="ContractType"></typeparam>
        /// <returns></returns>
        private ComponentModel[] GetComponentModels<ContractType>()
        {
            var typelist = new List<ComponentModel>();
            var nodes = this.Kernel.GraphNodes;
            nodes.Cast<ComponentModel>().ForEach(model =>
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Service);
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (markedWithServiceContract)
                {
                    typelist.Add(model);
                }
            });

            return typelist.ToArray();
        }

        /// <summary>
        /// ��ȡԼ���Ľӿ�
        /// </summary>
        /// <returns></returns>
        public Type[] GetServiceTypes<ContractType>()
        {
            var typelist = new List<Type>();
            var nodes = this.Kernel.GraphNodes;
            nodes.Cast<ComponentModel>().ForEach(model =>
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Service);
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (markedWithServiceContract)
                {
                    typelist.Add(model.Service);
                }
            });

            return typelist.ToArray();
        }

        #endregion

        /// <summary>
        /// ��������
        /// </summary>
        public string ServiceName
        {
            get { return typeof(SimpleServiceContainer).FullName; }
        }

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

        #region IServiceContainer ��Ա

        /// <summary>
        /// �����־
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public void WriteLog(string log, LogType type)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    var arr = state as ArrayList;
                    if (OnLog != null) OnLog(arr[0] as string, (LogType)arr[1]);
                }
                catch (Exception)
                {
                }
            }, new ArrayList { log, type });
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="error"></param>
        public void WriteError(Exception error)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    if (OnError != null) OnError(state as Exception);
                }
                catch (Exception)
                {
                }
            }, error);
        }

        #endregion
    }
}
