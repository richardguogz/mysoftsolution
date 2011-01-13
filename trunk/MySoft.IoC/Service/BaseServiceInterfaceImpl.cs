using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MySoft.Remoting;
using System.Linq;
using System.Reflection.Emit;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class BaseServiceInterfaceImpl
    {
        delegate object GetValueDelegate(Type t);

        /// <summary>
        /// 保存方法
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> dictMethods = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// The default msg expire time.
        /// </summary>
        public static int DefaultExpireMinutes = 30;

        private IServiceContainer container;
        private Type serviceInterfaceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceInterfaceImpl"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public BaseServiceInterfaceImpl(IServiceContainer container, Type serviceInterfaceType)
        {
            this.container = container;
            this.serviceInterfaceType = serviceInterfaceType;
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="subServiceName">Name of the sub service.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>The result.</returns>
        protected object CallService(string subServiceName, Type returnType, params object[] paramValues)
        {
            RequestMessage reqMsg = new RequestMessage();

            //获取约束格式
            var contract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);
            if (contract != null && contract.Format != DataFormat.Default)
            {
                switch (contract.Format)
                {
                    case DataFormat.Binary:
                        reqMsg.Transfer = TransferType.Binary;
                        break;
                    case DataFormat.Json:
                        reqMsg.Transfer = TransferType.Json;
                        break;
                    case DataFormat.Xml:
                        reqMsg.Transfer = TransferType.Xml;
                        break;
                }
            }
            else
            {
                //传递传输与压缩格式
                reqMsg.Transfer = container.Transfer;
            }

            reqMsg.Compress = container.Compress;
            reqMsg.Expiration = DateTime.Now.AddMinutes(DefaultExpireMinutes);
            reqMsg.MessageId = Guid.NewGuid();
            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = subServiceName;
            reqMsg.Timestamp = DateTime.Now;
            reqMsg.TransactionId = Guid.NewGuid();

            MethodInfo method = null;
            if (dictMethods.ContainsKey(subServiceName))
            {
                method = dictMethods[subServiceName];
            }
            else
            {
                method = serviceInterfaceType.GetMethods()
                              .Where(p => p.ToString() == subServiceName)
                              .FirstOrDefault();

                if (method == null)
                {
                    foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                    {
                        method = inheritedInterface.GetMethods()
                                .Where(p => p.ToString() == subServiceName)
                                .FirstOrDefault();

                        if (method != null) break;
                    }
                }

                if (method == null)
                {
                    throw new IoCException(string.Format("未找到调用的方法({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName));
                }
                else
                {
                    dictMethods[subServiceName] = method;
                }
            }

            ParameterInfo[] pis = method.GetParameters();
            if ((pis.Length == 0 && paramValues != null && paramValues.Length > 0) || (paramValues != null && pis.Length != paramValues.Length))
            {
                //参数不正确直接返回异常
                throw new IoCException(string.Format("无效的参数信息({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName));
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    if (paramValues[i] != null)
                    {
                        reqMsg.Parameters[pis[i].Name] = SerializationManager.SerializeJson(paramValues[i]);
                    }
                }
            }

            ResponseMessage resMsg = container.CallService(serviceInterfaceType.FullName, reqMsg);
            if (resMsg == null)
            {
                throw new IoCException(string.Format("服务调用失败({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName));
            }

            if (resMsg.Data == null) return resMsg.Data;
            switch (resMsg.Transfer)
            {
                case TransferType.Binary:
                    byte[] buffer = (byte[])resMsg.Data;

                    //将数据进行解压缩
                    if (resMsg.Compress != CompressType.None)
                    {
                        switch (resMsg.Compress)
                        {
                            case CompressType.GZip:
                                buffer = CompressionManager.DecompressGZip(buffer);
                                break;
                            case CompressType.Zip:
                                buffer = CompressionManager.Decompress7Zip(buffer);
                                break;
                        }
                    }

                    return SerializationManager.DeserializeBin(buffer);
                case TransferType.Json:
                    string jsonString = resMsg.Data.ToString();

                    //将数据进行解压缩
                    if (resMsg.Compress != CompressType.None)
                    {
                        switch (resMsg.Compress)
                        {
                            case CompressType.GZip:
                                jsonString = CompressionManager.DecompressGZip(jsonString);
                                break;
                            case CompressType.Zip:
                                jsonString = CompressionManager.Decompress7Zip(jsonString);
                                break;
                        }
                    }

                    return SerializationManager.DeserializeJson(returnType, jsonString);
                case TransferType.Xml:
                    string xmlString = resMsg.Data.ToString();

                    //将数据进行解压缩
                    if (resMsg.Compress != CompressType.None)
                    {
                        switch (resMsg.Compress)
                        {
                            case CompressType.GZip:
                                xmlString = CompressionManager.DecompressGZip(xmlString);
                                break;
                            case CompressType.Zip:
                                xmlString = CompressionManager.Decompress7Zip(xmlString);
                                break;
                        }
                    }

                    return SerializationManager.DeserializeXml(returnType, xmlString);
            }

            throw new IoCException(string.Format("服务调用失败({0},{1})，无数据返回.", reqMsg.ServiceName, reqMsg.SubServiceName));
        }
    }
}
