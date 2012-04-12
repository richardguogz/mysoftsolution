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
        private void Init(CastleFactoryType type)
        {
            this.container = new WindsorContainer();

            lock (container)
            {
                //����������ע��
                this.container.AddFacility(new StartableFacility());

                //���ط������
                this.container.AddFacility(new ServiceDiscoverFacility(this));

                //�������Զ��ģʽ����������ý�
                var sectionKey = "mysoft.framework/castle";
                var castle = ConfigurationManager.GetSection(sectionKey);
                if (castle != null)
                {
                    //ֻ�������ط���
                    if (type != CastleFactoryType.Remote)
                    {
                        //��������
                        this.DiscoverServices(sectionKey);
                    }
                }
            }
        }

        private void DiscoverServices(string sectionKey)
        {
            //���ط���
            var windsorContainer = new WindsorContainer(new XmlInterpreter(new ConfigResource(sectionKey)));

            //�������Ϊ�գ��򲻼���
            if (windsorContainer.Kernel.GraphNodes.Length > 0)
            {
                var models = GetComponentModels<ServiceContractAttribute>(windsorContainer);
                foreach (var model in models)
                {
                    //ע�����
                    var component = Component.For(model.Services.First()).Named(model.Name).ImplementedBy(model.Implementation);
                    if (model.LifestyleType == LifestyleType.Undefined)
                        component = component.LifeStyle.Singleton;
                    else
                        component = component.LifeStyle.Is(model.LifestyleType);

                    container.Register(component);
                }

                //������Դ
                windsorContainer.Dispose();
            }
        }

        /// <summary>
        /// ��ȡԼ����ʵ��
        /// </summary>
        /// <typeparam name="ContractType"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        private ComponentModel[] GetComponentModels<ContractType>(WindsorContainer container)
        {
            var typelist = new List<ComponentModel>();
            var nodes = container.Kernel.GraphNodes;
            nodes.Cast<ComponentModel>().ForEach(model =>
            {
                bool markedWithServiceContract = false;
                var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Services.First());
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleServiceContainer"/> class.
        /// </summary>
        /// <param name="config"></param>
        public SimpleServiceContainer(CastleFactoryType type)
        {
            Init(type);
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

        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        /// <param name="classType"></param>
        public void RegisterComponent(string key, Type serviceType, Type classType)
        {
            if (!container.Kernel.HasComponent(key))
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
            if (!container.Kernel.HasComponent(key))
                container.Register(Component.For(serviceType).Named(key).Instance(instance).LifeStyle.Singleton);
        }

        /// <summary>
        /// Registers the components.
        /// </summary>
        /// <param name="serviceKeyTypes">The service key types.</param>
        public void RegisterComponents(IDictionary<Type, object> serviceKeyTypes)
        {
            foreach (var kvp in serviceKeyTypes)
            {
                //ע�����
                RegisterComponent(kvp.Key.FullName, kvp.Key, kvp.Value);
            }
        }

        #region Register component

        /// <summary>
        /// Register ocal service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="classType"></param>
        public void Register(Type serviceType, Type classType)
        {
            this.Register(serviceType.FullName, serviceType, classType);
        }

        /// <summary>
        /// Register ocal service
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        public void Register(string key, Type serviceType, Type classType)
        {
            this.RegisterComponent(key, serviceType, classType);
        }

        /// <summary>
        /// Register ocal service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="classType"></param>
        public void Register(Type serviceType, object instance)
        {
            this.Register(serviceType.FullName, serviceType, instance);
        }

        /// <summary>
        /// Register ocal service
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        public void Register(string key, Type serviceType, object instance)
        {
            this.RegisterComponent(key, serviceType, instance);
        }

        #endregion

        #region Resolve component

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

        #endregion

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
                  var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Services.First());
                  if (attr != null)
                  {
                      markedWithServiceContract = true;
                  }

                  return markedWithServiceContract
                      && model.Services.First().FullName == serviceName;
              });
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
                var attr = CoreHelper.GetTypeAttribute<ContractType>(model.Services.First());
                if (attr != null)
                {
                    markedWithServiceContract = true;
                }

                if (markedWithServiceContract)
                {
                    typelist.Add(model.Services.First());
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
