using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MySoft.IoC.Message;
using MySoft.Security;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// 异步调用委托
    /// </summary>
    /// <param name="reqMsg"></param>
    /// <returns></returns>
    public delegate ResponseMessage AsyncMethodCaller(RequestMessage reqMsg);

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
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = reqMsg.SubServiceName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Compress = reqMsg.Compress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Expiration = reqMsg.Expiration;

            #region 获取相应的方法

            MethodInfo method = null;

            string serviceKey = string.Format("{0}|{1}", reqMsg.ServiceName, reqMsg.SubServiceName);
            if (dictMethods.ContainsKey(serviceKey))
            {
                method = dictMethods[serviceKey];
            }
            else
            {
                method = CoreHelper.GetMethodFromType(serviceInterfaceType, reqMsg.SubServiceName);
                if (method == null)
                {
                    string title = string.Format("The server not find called method ({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName);
                    var exception = new WarningException(title)
                    {
                        ApplicationName = reqMsg.AppName,
                        ExceptionHeader = string.Format("Application【{0}】occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };

                    resMsg.Exception = exception;
                    return resMsg;
                }
                else
                {
                    lock (dictMethods)
                    {
                        dictMethods[serviceKey] = method;
                    }
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
                else if (!pis[i].IsOut)
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
                    byte[] keys = null;

                    //判断是否加密
                    if (reqMsg.Encrypt)
                    {
                        int keyLength = 128;

                        //获取加密的字符串
                        var encrypt = BigInteger.GenerateRandom(keyLength).ToString();
                        keys = MD5.Hash(Encoding.UTF8.GetBytes(encrypt));
                    }

                    //返回结果数据
                    resMsg.Data = new ResponseData(reqMsg, keys, returnValue);
                }
            }
            catch (BusinessException ex)
            {
                if (ex.InnerException == null)
                    resMsg.Exception = new BusinessException(ex.Code, ex.Message);
                else
                    resMsg.Exception = new BusinessException(ex.Code, ex.Message, ex.InnerException);
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