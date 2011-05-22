using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;
using Castle.Core.Interceptor;
using System.IO;
using MySoft.Security;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        /// <summary>
        /// 保存方法
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> dictMethods = new Dictionary<string, MethodInfo>();

        private IServiceContainer container;
        private Type serviceInterfaceType;
        private object serviceInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        /// <param name="serviceInstance">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType, object serviceInstance)
            : base(container, serviceInterfaceType.FullName)
        {
            this.container = container;
            this.serviceInterfaceType = serviceInterfaceType;
            this.serviceInstance = serviceInstance;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            //如果服务与请求为null
            if (reqMsg == null) return null;

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = reqMsg.TransactionId;
            //resMsg.RequestAddress = reqMsg.RequestAddress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Compress = reqMsg.Compress;
            resMsg.Encrypt = reqMsg.Encrypt;
            //resMsg.Timeout = reqMsg.Timeout;
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = reqMsg.SubServiceName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Expiration = reqMsg.Expiration;

            //如果没有找到服务
            if (serviceInstance == null)
            {
                try { serviceInstance = container[serviceInterfaceType]; }
                catch { }

                if (serviceInstance == null)
                {
                    var exception = new IoCException(string.Format("The server not find matching service ({0}).", reqMsg.ServiceName))
                    {
                        ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                    resMsg.Exception = exception;
                    return resMsg;
                }
            }

            #region 获取相应的方法

            MethodInfo method = null;
            if (dictMethods.ContainsKey(reqMsg.SubServiceName))
            {
                method = dictMethods[reqMsg.SubServiceName];
            }
            else
            {
                method = CoreHelper.GetMethodFromType(serviceInterfaceType, reqMsg.SubServiceName);
                if (method == null)
                {
                    var exception = new IoCException(string.Format("The server not find called method ({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName))
                    {
                        ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };

                    resMsg.Exception = exception;
                    return resMsg;
                }
                else
                {
                    dictMethods[reqMsg.SubServiceName] = method;
                }
            }

            ParameterInfo[] pis = method.GetParameters();
            object[] paramValues = new object[pis.Length];

            for (int i = 0; i < pis.Length; i++)
            {
                if (!pis[i].ParameterType.IsByRef)
                {
                    paramValues[i] = resMsg.Parameters[pis[i].Name];
                }
                else
                {
                    paramValues[i] = CoreHelper.GetTypeDefaultValue(pis[i].ParameterType);
                }
            }

            #endregion

            //获取服务及方法名称
            resMsg.ServiceName = serviceInstance.GetType().FullName;
            resMsg.SubServiceName = method.ToString();
            resMsg.ReturnType = method.ReturnType;

            //返回拦截服务
            var service = AspectManager.GetService(serviceInstance);

            try
            {
                //调用对应的服务
                object returnValue = DynamicCalls.GetMethodInvoker(method).Invoke(service, paramValues);

                //把返回值传递回去
                for (int i = 0; i < pis.Length; i++)
                {
                    if (pis[i].ParameterType.IsByRef)
                    {
                        //给参数赋值
                        resMsg.Parameters[pis[i].Name] = paramValues[i];
                    }
                }

                if (returnValue != null)
                {
                    #region 处理数据

                    //将对象系列化成byte数组
                    resMsg.Data = SerializationManager.SerializeBin(returnValue);

                    //判断是否压缩
                    if (resMsg.Compress) resMsg.Data = CompressionManager.CompressSharpZip(resMsg.Data);

                    //判断是否加密
                    if (resMsg.Encrypt)
                    {
                        //长度能被16除，表示为有效的key
                        if (reqMsg.KeyLength != 0 && reqMsg.KeyLength % 16 == 0)
                        {
                            resMsg.KeyLength = reqMsg.KeyLength;
                        }
                        else
                        {
                            resMsg.KeyLength = 128;
                        }

                        //获取加密的字符串
                        var encrypt = BigInteger.GenerateRandom(resMsg.KeyLength).ToString();
                        resMsg.Keys = MD5.Hash(Encoding.UTF8.GetBytes(encrypt));

                        //这里暂时不处理
                        resMsg.Data = XXTEA.Encrypt(resMsg.Data, resMsg.Keys);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //捕获全局错误
                resMsg.Exception = ex;
            }

            return resMsg;
        }
    }
}
