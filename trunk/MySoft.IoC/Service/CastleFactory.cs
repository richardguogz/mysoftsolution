using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MySoft.Net.Client;

namespace MySoft.IoC
{
    /// <summary>
    /// The service factory.
    /// </summary>
    public class CastleFactory : ILogable, IErrorLogable
    {
        #region Emit DynamicServiceImpl

        private object syncObj = new object();
        private static bool localService = false;
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
        //where IServiceInterfaceType : IServiceInterface
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

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static CastleFactory Create()
        {
            if (singleton == null)
            {
                var config = CastleFactoryConfiguration.GetConfig();
                singleton = Create(config);
            }

            return singleton;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="config"></param>
        /// <returns>The service factoru singleton instance.</returns>
        public static CastleFactory Create(CastleFactoryConfiguration config)
        {
            if (singleton == null)
            {
                //本地匹配节
                if (config == null || config.Type == CastleFactoryType.Local)
                {
                    localService = true;
                    singleton = new CastleFactory(new SimpleServiceContainer());

                    if (config == null)
                    {
                        config = new CastleFactoryConfiguration();
                    }
                }
                else
                {
                    IServiceContainer container = new SimpleServiceContainer();
                    container.OnLog += new LogEventHandler(msg_OnLog);
                    container.OnError += new ErrorLogEventHandler(container_OnError);

                    //设置配置信息
                    SocketClientConfiguration scc = new SocketClientConfiguration();
                    scc.IP = config.Server;
                    scc.Port = config.Port;

                    //设置服务代理
                    IServiceProxy serviceProxy = new ServiceProxy(scc, config.MaxTry);
                    serviceProxy.OnLog += new LogEventHandler(msg_OnLog);
                    container.Proxy = serviceProxy;

                    singleton = new CastleFactory(container);
                }

                singleton.ServiceContainer.Transfer = config.Transfer;
            }

            return singleton;
        }

        static void msg_OnLog(string log)
        {
            if (singleton != null)
            {
                if (singleton.OnLog != null) singleton.OnLog(log);
            }
            else
            {
                //未设置委托之前，从控制台输出
                log = "[" + DateTime.Now.ToString() + "] " + log;
                Console.WriteLine(log);
            }
        }

        static void container_OnError(Exception exception)
        {
            if (singleton != null)
            {
                if (singleton.OnError != null) singleton.OnError(exception);
            }
            else
            {
                //未设置委托之前，从控制台输出
                string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
                Console.WriteLine(message);
            }
        }

        #endregion

        #region Get Service

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns>The service implemetation instance.</returns>
        public IServiceInterfaceType GetService<IServiceInterfaceType>()
        //where IServiceInterfaceType : IServiceInterface
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

            //如果不是本地服务
            if (!localService)
            {
                lock (this)
                {
                    if (container != null)
                    {
                        return DynamicServiceImpl<IServiceInterfaceType>();
                    }
                }
            }

            return default(IServiceInterfaceType);
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
