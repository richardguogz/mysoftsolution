using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class BaseServiceInterfaceImpl
    {
        /// <summary>
        /// 保存方法
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> dictMethods = new Dictionary<string, MethodInfo>();

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
            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = subServiceName;
            reqMsg.TransactionId = Guid.NewGuid();

            #region 判断数据格式

            if (container.Proxy != null)
            {
                //传递传输与压缩格式
                reqMsg.Format = container.Proxy.Format;

                //设置压缩格式
                reqMsg.Compress = container.Proxy.Compress;

                //设置超时时间
                reqMsg.Timeout = container.Proxy.Timeout;
            }

            //获取约束信息
            var contract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);

            //判断约束
            if (contract != null)
            {
                //设置响应格式
                if (contract.Format != ResponseFormat.Binary)
                    reqMsg.Format = contract.Format;

                //设置压缩方式
                if (contract.Compress != CompressType.None)
                    reqMsg.Compress = contract.Compress;

                //设置超时时间
                if (contract.Timeout > 0)
                    reqMsg.Timeout = contract.Timeout;
            }

            #endregion

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
                    throw new IoCException(string.Format("Not found called method ({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName));
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
                throw new IoCException(string.Format("Invalid parameters ({0},{1}). ==> {2}", reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.Parameters));
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

            //调用服务
            ResponseMessage resMsg = container.CallService(serviceInterfaceType, reqMsg);

            //处理数据
            #region 处理返回的数据

            if (resMsg.Data != null)
            {
                bool isBuffer = false;
                if (resMsg.Format == ResponseFormat.Binary)
                {
                    isBuffer = true;

                    //如果类型不是byte[]，则返回数据
                    if (resMsg.Data.GetType() != typeof(byte[]))
                    {
                        return resMsg.Data;
                    }
                }

                switch (resMsg.Compress)
                {
                    case CompressType.Deflate:
                        {
                            if (isBuffer)
                                resMsg.Data = CompressionManager.DecompressDeflate((byte[])resMsg.Data);
                            else
                                resMsg.Data = CompressionManager.DecompressDeflate(resMsg.Data.ToString());
                        }
                        break;
                    case CompressType.GZip:
                        {
                            if (isBuffer)
                                resMsg.Data = CompressionManager.DecompressGZip((byte[])resMsg.Data);
                            else
                                resMsg.Data = CompressionManager.DecompressGZip(resMsg.Data.ToString());
                        }
                        break;
                    case CompressType.SevenZip:
                        {
                            if (isBuffer)
                                resMsg.Data = CompressionManager.Decompress7Zip((byte[])resMsg.Data);
                            else
                                resMsg.Data = CompressionManager.Decompress7Zip(resMsg.Data.ToString());
                        }
                        break;
                }

                switch (resMsg.Format)
                {
                    case ResponseFormat.Binary:
                        {
                            byte[] buffer = (byte[])resMsg.Data;
                            resMsg.Data = SerializationManager.DeserializeBin(buffer);
                        }
                        break;
                    case ResponseFormat.Json:
                        {
                            string jsonString = resMsg.Data.ToString();
                            resMsg.Data = SerializationManager.DeserializeJson(returnType, jsonString);
                        }
                        break;
                    case ResponseFormat.Xml:
                        {
                            string xmlString = resMsg.Data.ToString();
                            resMsg.Data = SerializationManager.DeserializeXml(returnType, xmlString);
                        }
                        break;
                }
            }

            return resMsg.Data;

            #endregion
        }
    }
}
