using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MySoft.Core;
using MySoft.Remoting;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class BaseServiceInterfaceImpl
    {
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
            object errorReturnValue = null;

            if (returnType != typeof(void))
            {
                errorReturnValue = null;
            }

            RequestMessage reqMsg = new RequestMessage();
            reqMsg.Expiration = DateTime.Now.AddMinutes(DefaultExpireMinutes);
            reqMsg.MessageId = Guid.NewGuid();
            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = subServiceName;
            reqMsg.Timestamp = DateTime.Now;
            reqMsg.TransactionId = Guid.NewGuid();

            MethodInfo mi = null;
            foreach (MethodInfo item in serviceInterfaceType.GetMethods())
            {
                if (item.ToString() == subServiceName)
                {
                    mi = item;
                    break;
                }
            }

            if (mi == null)
            {
                foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                {
                    foreach (MethodInfo item in inheritedInterface.GetMethods())
                    {
                        if (item.ToString() == subServiceName)
                        {
                            mi = item;
                            break;
                        }
                    }
                }
            }

            if (mi == null)
            {
                //return errorReturnValue;
                throw new Exception("Method not found " + subServiceName + "!");
            }

            ParameterInfo[] pis = mi.GetParameters();

            if ((pis.Length == 0 && paramValues != null && paramValues.Length > 0) || (paramValues != null && pis.Length != paramValues.Length))
            {
                return errorReturnValue;
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    if (paramValues[i] == null)
                    {
                        continue;
                    }

                    string val = SerializationManager.SerializeJSON(paramValues[i]);

                    reqMsg.Parameters[pis[i].Name] = val;
                }
            }

            ResponseMessage resMsg = container.CallService(serviceInterfaceType.FullName, reqMsg);
            if (resMsg == null || returnType == typeof(void))
            {
                return errorReturnValue;
            }

            if (resMsg.Data == null) return resMsg.Data;

            switch (container.Transfer)
            {
                case TransferType.Binary:
                    byte[] buffer = (byte[])resMsg.Data;

                    //将数据进行解压缩
                    if (container.Compress != CompressType.None)
                    {
                        switch (container.Compress)
                        {
                            case CompressType.GZip:
                                buffer = CompressionManager.DecompressGZip(buffer);
                                break;
                            case CompressType.Zip:
                                buffer = CompressionManager.Decompress7Zip(buffer);
                                break;
                            case CompressType.Auto:
                                if (buffer.Length > 1024 * 1024) //大于1兆才压缩
                                {
                                    buffer = CompressionManager.Decompress7Zip(buffer);
                                }
                                break;
                        }
                    }

                    return SerializationManager.DeserializeBin(buffer);
                case TransferType.Json:
                    string jsonString = resMsg.Data.ToString();

                    //将数据进行解压缩
                    if (container.Compress != CompressType.None)
                    {
                        switch (container.Compress)
                        {
                            case CompressType.GZip:
                                jsonString = CompressionManager.DecompressGZip(jsonString);
                                break;
                            case CompressType.Zip:
                                jsonString = CompressionManager.Decompress7Zip(jsonString);
                                break;
                            case CompressType.Auto:
                                if (Encoding.Default.GetByteCount(jsonString) > 1024 * 1024) //大于1兆才压缩
                                {
                                    jsonString = CompressionManager.Decompress7Zip(jsonString);
                                }
                                break;
                        }
                    }

                    return SerializationManager.DeserializeJSON(returnType, jsonString);
                case TransferType.Xml:
                    string xmlString = resMsg.Data.ToString();

                    //将数据进行解压缩
                    if (container.Compress != CompressType.None)
                    {
                        switch (container.Compress)
                        {
                            case CompressType.GZip:
                                xmlString = CompressionManager.DecompressGZip(xmlString);
                                break;
                            case CompressType.Zip:
                                xmlString = CompressionManager.Decompress7Zip(xmlString);
                                break;
                            case CompressType.Auto:
                                if (Encoding.Default.GetByteCount(xmlString) > 1024 * 1024) //大于1兆才压缩
                                {
                                    xmlString = CompressionManager.Decompress7Zip(xmlString);
                                }
                                break;
                        }
                    }

                    return SerializationManager.DeserializeXML(returnType, xmlString);
            }

            return errorReturnValue;

        }
    }
}
