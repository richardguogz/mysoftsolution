using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;
using Castle.Core.Interceptor;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType)
            : base(serviceInterfaceType.FullName)
        {
            this.container = container;
            this.OnLog += container.WriteLog;
            this.OnError += container.WriteError;
            this.serviceInterfaceType = serviceInterfaceType;
        }


        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage msg)
        {
            if (container == null || msg == null)
            {
                return null;
            }

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = msg.TransactionId;
            resMsg.RequestAddress = msg.RequestAddress;
            resMsg.Format = msg.Format;
            resMsg.Compress = msg.Compress;
            resMsg.Timeout = msg.Timeout;
            resMsg.TransactionId = msg.TransactionId;
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = msg.SubServiceName;
            resMsg.Parameters = msg.Parameters;
            resMsg.Expiration = msg.Expiration;

            object service = null;
            try
            {
                service = container[serviceInterfaceType];
            }
            catch { }
            if (service == null)
            {
                resMsg.Data = new IoCException(string.Format("The server not find matching service ({0}).", resMsg.ServiceName));
                return resMsg;
            }

            try
            {
                MethodInfo method = null;
                if (dictMethods.ContainsKey(resMsg.SubServiceName))
                {
                    method = dictMethods[resMsg.SubServiceName];
                }
                else
                {
                    method = serviceInterfaceType.GetMethods()
                               .Where(p => p.ToString() == resMsg.SubServiceName)
                               .FirstOrDefault();

                    if (method == null)
                    {
                        foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                        {
                            method = inheritedInterface.GetMethods()
                                    .Where(p => p.ToString() == resMsg.SubServiceName)
                                    .FirstOrDefault();

                            if (method != null) break;
                        }
                    }

                    if (method == null)
                    {
                        resMsg.Data = new IoCException(string.Format("The server not find called method ({0},{1}).", resMsg.ServiceName, resMsg.SubServiceName));
                        return resMsg;
                    }
                    else
                    {
                        dictMethods[resMsg.SubServiceName] = method;
                    }
                }

                ParameterInfo[] pis = method.GetParameters();
                object[] parms = new object[pis.Length];

                for (int i = 0; i < pis.Length; i++)
                {
                    Type type = pis[i].ParameterType;
                    object val = SerializationManager.DeserializeJson(type, resMsg.Parameters[pis[i].Name]);
                    parms[i] = val;
                }

                //返回拦截服务
                service = AspectManager.GetService(service);
                object returnValue = null;
                try
                {
                    returnValue = DynamicCalls.GetMethodInvoker(method).Invoke(service, parms);
                }
                catch (Exception ex)
                {
                    resMsg.Data = GetNewException(ex);
                    return resMsg;
                }
                //returnValue = mi.Invoke(service, parms);

                if (returnValue != null)
                {
                    bool isBuffer = false;
                    //Type returnType = method.ReturnType;
                    switch (resMsg.Format)
                    {
                        case ResponseFormat.Binary:
                            {
                                isBuffer = true;
                                byte[] buffer = SerializationManager.SerializeBin(returnValue);
                                resMsg.Data = buffer;
                            }
                            break;
                        case ResponseFormat.Json:
                            {
                                string jsonString = SerializationManager.SerializeJson(returnValue);
                                resMsg.Data = jsonString;
                            }
                            break;
                        case ResponseFormat.Xml:
                            {
                                string xmlString = SerializationManager.SerializeXml(returnValue);
                                resMsg.Data = xmlString;
                            }
                            break;
                    }

                    switch (resMsg.Compress)
                    {
                        case CompressType.Deflate:
                            {
                                if (isBuffer)
                                    resMsg.Data = CompressionManager.CompressDeflate((byte[])resMsg.Data);
                                else
                                    resMsg.Data = CompressionManager.CompressDeflate(resMsg.Data.ToString());
                            }
                            break;
                        case CompressType.GZip:
                            {
                                if (isBuffer)
                                    resMsg.Data = CompressionManager.CompressGZip((byte[])resMsg.Data);
                                else
                                    resMsg.Data = CompressionManager.CompressGZip(resMsg.Data.ToString());
                            }
                            break;
                        case CompressType.SevenZip:
                            {
                                if (isBuffer)
                                    resMsg.Data = CompressionManager.Compress7Zip((byte[])resMsg.Data);
                                else
                                    resMsg.Data = CompressionManager.Compress7Zip(resMsg.Data.ToString());
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //捕获全局错误
                resMsg.Data = GetNewException(ex);
            }

            return resMsg;
        }

        /// <summary>
        /// 获取新的Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private IoCException GetNewException(Exception ex)
        {
            if (ex.InnerException == null)
                return new IoCException(ex.Message);
            else
                return new IoCException(ex.Message, ErrorHelper.GetInnerException(ex.InnerException));
        }
    }
}
