using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MySoft.Net.Client;
using System.Threading;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory.
    /// </summary>
    public class CastleFactory : ILogable, IErrorLogable
    {
        #region Emit DynamicServiceImpl

        private object syncObj = new object();
        private static AssemblyBuilder assBuilder = null;
        private static ModuleBuilder modBuilder = null;

        private const string DYNAMIC_INTERFACEIMPL_NAMESPACE = "MySoft.IoC.DynamicInterfaceImpl";

        private static ResolveEventHandler _ResolveEventHandler = null;

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return assBuilder;
        }

        private static void EmitConstructor<IServiceInterfaceType>(TypeBuilder typeBuilder, Type baseType)
        //where IServiceInterfaceType : IServiceInterface
        {
            //define default constructor
            ConstructorBuilder consBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(IServiceContainer) });
            ILGenerator ctorIL = consBuilder.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Ldtoken, typeof(IServiceInterfaceType));
            ctorIL.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public));
            ctorIL.Emit(OpCodes.Call, baseType.GetConstructor(new Type[] { typeof(IServiceContainer), typeof(Type) }));
            ctorIL.Emit(OpCodes.Ret);
        }

        private static void EmitLoadInt32Value(ILGenerator methodIL, int int32Value)
        {
            EmitHelper.LoadInt32(methodIL, int32Value);
        }

        private static void EmitMethods<IServiceInterfaceType>(TypeBuilder typeBuilder, Type baseType, MethodInfo[] mis)
        //where IServiceInterfaceType : IServiceInterface
        {
            if (mis != null && mis.Length > 0)
            {
                foreach (MethodInfo mi in mis)
                {
                    ParameterInfo[] paramInfos = mi.GetParameters();
                    int paramlength = paramInfos.Length;
                    Type[] paramTypes = new Type[paramlength];
                    for (int i = 0; i < paramlength; i++)
                    {
                        paramTypes[i] = paramInfos[i].ParameterType;
                    }
                    MethodBuilder methodBuilder = typeBuilder.DefineMethod(mi.Name, mi.Attributes & (~MethodAttributes.Abstract) | MethodAttributes.Public, mi.CallingConvention, mi.ReturnType, paramTypes);
                    for (int i = 0; i < paramlength; i++)
                    {
                        ParameterInfo pi = paramInfos[i];
                        methodBuilder.DefineParameter(i + 1, pi.Attributes, pi.Name);
                    }
                    typeBuilder.DefineMethodOverride(methodBuilder, mi);
                    ILGenerator methodIL = methodBuilder.GetILGenerator();

                    if (paramlength > 0)
                    {
                        methodIL.DeclareLocal(typeof(object[]));
                    }

                    methodIL.Emit(OpCodes.Ldarg_0);
                    methodIL.Emit(OpCodes.Ldstr, mi.ToString());
                    methodIL.Emit(OpCodes.Ldtoken, mi.ReturnType);
                    methodIL.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public));
                    EmitLoadInt32Value(methodIL, paramlength);
                    methodIL.Emit(OpCodes.Newarr, typeof(object));

                    if (paramlength > 0)
                    {
                        methodIL.Emit(OpCodes.Stloc_0);
                        for (int i = 0; i < paramlength; i++)
                        {
                            methodIL.Emit(OpCodes.Ldloc_0);
                            EmitLoadInt32Value(methodIL, i);

                            EmitHelper.LoadArgument(methodIL, i + 1);

                            if (paramInfos[i].ParameterType.IsValueType)
                            {
                                methodIL.Emit(OpCodes.Box, paramInfos[i].ParameterType);
                            }

                            methodIL.Emit(OpCodes.Stelem_Ref);
                        }
                        methodIL.Emit(OpCodes.Ldloc_0);
                    }
                    methodIL.Emit(OpCodes.Callvirt, baseType.GetMethod("CallService", BindingFlags.Instance | BindingFlags.NonPublic));
                    if (mi.ReturnType == typeof(void))
                    {
                        methodIL.Emit(OpCodes.Pop);
                    }
                    else
                    {
                        if (mi.ReturnType.IsValueType)
                        {
                            methodIL.Emit(OpCodes.Unbox_Any, mi.ReturnType);
                        }
                        else
                        {
                            methodIL.Emit(OpCodes.Castclass, mi.ReturnType);
                        }
                    }
                    methodIL.Emit(OpCodes.Ret);
                }
            }
        }

        private IServiceInterfaceType EmitDynamicServiceInterfaceImplType<IServiceInterfaceType>(AssemblyBuilder assBuilder, ModuleBuilder modBuilder)
        //where IServiceInterfaceType : IServiceInterface
        {
            TypeBuilder typeBuilder = modBuilder.DefineType(DYNAMIC_INTERFACEIMPL_NAMESPACE + "." + typeof(IServiceInterfaceType).FullName, TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeof(IServiceInterfaceType));
            MethodInfo[] mis = typeof(IServiceInterfaceType).GetMethods();
            List<MethodInfo> miList = new List<MethodInfo>();
            foreach (MethodInfo mi in mis)
            {
                miList.Add(mi);
            }
            foreach (Type inheritedInterface in typeof(IServiceInterfaceType).GetInterfaces())
            {
                typeBuilder.AddInterfaceImplementation(inheritedInterface);
                foreach (MethodInfo mi in inheritedInterface.GetMethods())
                {
                    miList.Add(mi);
                }
            }
            Type baseType = typeof(BaseServiceInterfaceImpl);
            typeBuilder.SetParent(baseType);

            EmitConstructor<IServiceInterfaceType>(typeBuilder, baseType);

            EmitMethods<IServiceInterfaceType>(typeBuilder, baseType, miList.ToArray());

            return (IServiceInterfaceType)Activator.CreateInstance(typeBuilder.CreateType(), container);
        }

        private IServiceInterfaceType DynamicServiceImpl<IServiceInterfaceType>()
        {
            Type t = null;
            if (assBuilder != null)
            {
                t = assBuilder.GetType(DYNAMIC_INTERFACEIMPL_NAMESPACE + "." + typeof(IServiceInterfaceType).FullName);
            }
            if (t != null)
            {
                return (IServiceInterfaceType)Activator.CreateInstance(t, container);
            }

            lock (syncObj)
            {
                //create dynamic IEntityType Assembly & Type through Emit
                if (assBuilder == null)
                {
                    AssemblyName assName = new AssemblyName();
                    assName.Name = DYNAMIC_INTERFACEIMPL_NAMESPACE;
                    assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assName, AssemblyBuilderAccess.Run);

                    //add dynamic assembly to current appdomain
                    if (_ResolveEventHandler == null)
                    {
                        _ResolveEventHandler = new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                        AppDomain.CurrentDomain.AssemblyResolve += _ResolveEventHandler;
                    }
                }

                if (modBuilder == null)
                {
                    modBuilder = assBuilder.DefineDynamicModule(assBuilder.GetName().Name);
                }

                return EmitDynamicServiceInterfaceImplType<IServiceInterfaceType>(assBuilder, modBuilder);
            }
        }

        #endregion

        #region Create Service Factory

        private IServiceContainer container;
        private CastleFactoryConfiguration Config;

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
                this.container = new SimpleServiceContainer();
            }
            else
            {
                this.container = container;
            }
        }

        private void WriteLog(string logInfo)
        {
            if (OnLog != null) OnLog(logInfo);
        }

        private static CastleFactory singleton = null;

        #region 创建单例

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

        #endregion

        #region 创建新实例

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <returns></returns>
        public static CastleFactory CreateNew()
        {
            var config = CastleFactoryConfiguration.GetConfig();
            return CreateNew(config, config.Default);
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="name">service name</param>
        /// <returns></returns>
        public static CastleFactory CreateNew(string name)
        {
            var config = CastleFactoryConfiguration.GetConfig();
            return CreateNew(config, name);
        }

        /// <summary>
        /// Creates this instance. Used in a multithreaded environment
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The service factoru new instance.</returns>
        public static CastleFactory CreateNew(ServiceNode node)
        {
            var config = CastleFactoryConfiguration.GetConfig();
            config.Hosts.Add(node.Name, node); //添加节点
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

            //本地匹配节
            if (config == null || config.Type == CastleFactoryType.Local)
            {
                instance = new CastleFactory(new SimpleServiceContainer());
            }
            else
            {
                IServiceContainer container = new SimpleServiceContainer();

                //客户端配置
                SocketClientConfiguration scc = new SocketClientConfiguration();
                scc.IP = config.Hosts[name].Server;
                scc.Port = config.Hosts[name].Port;

                //设置服务代理
                IServiceProxy serviceProxy = new ServiceProxy(scc);
                serviceProxy.Timeout = config.Timeout;
                serviceProxy.Format = config.Format;
                serviceProxy.Compress = config.Compress;

                container.Proxy = serviceProxy;

                instance = new CastleFactory(container);

                container.OnLog += (log) =>
                {
                    if (instance.OnLog != null)
                    {
                        instance.OnLog(log);
                    }
                };

                container.OnError += (exception) =>
                {
                    if (instance.OnError != null)
                    {
                        instance.OnError(exception);
                    }
                };

                serviceProxy.OnLog += (log) =>
                {
                    if (instance.OnLog != null)
                    {
                        instance.OnLog(log);
                    }
                };
            }

            //处理配置节
            instance.Config = config;

            return instance;
        }

        #endregion

        #endregion

        #region 注入缓存

        /// <summary>
        /// 注入缓存
        /// </summary>
        /// <param name="cache"></param>
        public void InjectCache(IDependentCache cache)
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

            //本地服务
            if (!string.IsNullOrEmpty(key))
            {
                if (container.Kernel.HasComponent(key))
                {
                    var service = container[key];

                    //返回拦截服务
                    return AspectManager.GetService<IServiceInterfaceType>(service);
                }
            }
            else
            {
                if (container.Kernel.HasComponent(typeof(IServiceInterfaceType)))
                {
                    var service = container[typeof(IServiceInterfaceType)];

                    //返回拦截服务
                    return AspectManager.GetService<IServiceInterfaceType>(service);
                }
            }

            //如果是本地配置，则抛出异常
            if (Config == null || Config.Type == CastleFactoryType.Local)
            {
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
                    var service = DynamicServiceImpl<IServiceInterfaceType>();
                    container.Kernel.AddComponentInstance(serviceKey, service);

                    return service;
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
