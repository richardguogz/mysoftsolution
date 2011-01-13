using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using MySoft.Remoting;
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

        /// <summary>
        /// The default  message expire minutes.
        /// </summary>
        public static int DefaultExpireMinutes = 30;
        private IServiceContainer container;
        private Type serviceInterfaceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType)
            : base(serviceInterfaceType.FullName, container.MQ)
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
            resMsg.Request = msg;
            resMsg.Expiration = DateTime.Now.AddMinutes(DefaultExpireMinutes);
            resMsg.MessageId = Guid.NewGuid();
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = msg.SubServiceName;
            resMsg.Timestamp = DateTime.Now;
            resMsg.TransactionId = msg.TransactionId;
            resMsg.Transfer = msg.Transfer;
            resMsg.Compress = msg.Compress;

            object service = null;
            try
            {
                service = container[serviceInterfaceType];
            }
            catch { }
            if (service == null)
            {
                resMsg.Data = new IoCException(string.Format("服务端未找到对应的服务({0}).", resMsg.ServiceName));
                return resMsg;
            }

            try
            {
                MethodInfo method = null;
                if (dictMethods.ContainsKey(msg.SubServiceName))
                {
                    method = dictMethods[msg.SubServiceName];
                }
                else
                {
                    method = serviceInterfaceType.GetMethods()
                               .Where(p => p.ToString() == msg.SubServiceName)
                               .FirstOrDefault();

                    if (method == null)
                    {
                        foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                        {
                            method = inheritedInterface.GetMethods()
                                    .Where(p => p.ToString() == msg.SubServiceName)
                                    .FirstOrDefault();

                            if (method != null) break;
                        }
                    }

                    if (method == null)
                    {
                        resMsg.Data = new IoCException(string.Format("服务端未找到调用的方法({0},{1}).", resMsg.ServiceName, resMsg.SubServiceName));
                        return resMsg;
                    }
                    else
                    {
                        dictMethods[msg.SubServiceName] = method;
                    }
                }

                ParameterInfo[] pis = method.GetParameters();
                object[] parms = new object[pis.Length];

                for (int i = 0; i < pis.Length; i++)
                {
                    Type type = pis[i].ParameterType;
                    object val = SerializationManager.DeserializeJson(type, msg.Parameters[pis[i].Name]);
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
                    Type returnType = method.ReturnType;
                    switch (resMsg.Transfer)
                    {
                        case TransferType.Binary:
                            byte[] buffer = SerializationManager.SerializeBin(returnValue);

                            //将数据进行压缩
                            if (resMsg.Compress != CompressType.None)
                            {
                                switch (resMsg.Compress)
                                {
                                    case CompressType.Zip:
                                        resMsg.Data = CompressionManager.Compress7Zip(buffer);
                                        break;
                                    case CompressType.GZip:
                                        resMsg.Data = CompressionManager.CompressGZip(buffer);
                                        break;
                                }
                            }
                            else
                            {
                                resMsg.Data = buffer;
                            }
                            break;
                        case TransferType.Json:
                            string jsonString = SerializationManager.SerializeJson(returnValue);

                            //将数据进行压缩
                            if (resMsg.Compress != CompressType.None)
                            {
                                switch (resMsg.Compress)
                                {
                                    case CompressType.Zip:
                                        resMsg.Data = CompressionManager.Compress7Zip(jsonString);
                                        break;
                                    case CompressType.GZip:
                                        resMsg.Data = CompressionManager.CompressGZip(jsonString);
                                        break;
                                }
                            }
                            else
                            {
                                resMsg.Data = jsonString;
                            }
                            break;
                        case TransferType.Xml:
                            string xmlString = SerializationManager.SerializeXml(returnValue);

                            //将数据进行压缩
                            if (resMsg.Compress != CompressType.None)
                            {
                                switch (resMsg.Compress)
                                {
                                    case CompressType.Zip:
                                        resMsg.Data = CompressionManager.Compress7Zip(xmlString);
                                        break;
                                    case CompressType.GZip:
                                        resMsg.Data = CompressionManager.CompressGZip(xmlString);
                                        break;
                                }
                            }
                            else
                            {
                                resMsg.Data = xmlString;
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
