using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using MySoft.Core;
using MySoft.Core.Remoting;

namespace MySoft.IoC.Service
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
                return errorReturnValue;
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

            try
            {
                if (resMsg.Data == null) return resMsg.Data;

                switch (container.Transfer)
                {
                    case RemotingDataType.BINARY:
                        byte[] buffer = (byte[])resMsg.Data;

                        //�����ݽ��н�ѹ��
                        if (container.Compress != CompressType.NONE)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.GZIP:
                                    buffer = CompressionManager.DecompressGZip(buffer);
                                    break;
                                case CompressType.ZIP7:
                                    buffer = CompressionManager.Decompress7Zip(buffer);
                                    break;
                                case CompressType.AUTO:
                                    if (buffer.Length > 1024 * 1024 * 5) //5�ײ�ѹ��
                                    {
                                        buffer = CompressionManager.Decompress7Zip(buffer);
                                    }
                                    break;
                            }
                        }

                        return SerializationManager.DeserializeBin(buffer);
                    case RemotingDataType.JSON:
                        string jsonString = resMsg.Data.ToString();

                        //�����ݽ��н�ѹ��
                        if (container.Compress != CompressType.NONE)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.GZIP:
                                    jsonString = CompressionManager.DecompressGZip(jsonString);
                                    break;
                                case CompressType.ZIP7:
                                    jsonString = CompressionManager.Decompress7Zip(jsonString);
                                    break;
                                case CompressType.AUTO:
                                    if (Encoding.Default.GetByteCount(jsonString) > 1024 * 1024 * 5) //5�ײ�ѹ��
                                    {
                                        jsonString = CompressionManager.Decompress7Zip(jsonString);
                                    }
                                    break;
                            }
                        }

                        return SerializationManager.DeserializeJSON(returnType, jsonString);
                    case RemotingDataType.XML:
                        string xmlString = resMsg.Data.ToString();

                        //�����ݽ��н�ѹ��
                        if (container.Compress != CompressType.NONE)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.GZIP:
                                    xmlString = CompressionManager.DecompressGZip(xmlString);
                                    break;
                                case CompressType.ZIP7:
                                    xmlString = CompressionManager.Decompress7Zip(xmlString);
                                    break;
                                case CompressType.AUTO:
                                    if (Encoding.Default.GetByteCount(xmlString) > 1024 * 1024 * 5) //5�ײ�ѹ��
                                    {
                                        xmlString = CompressionManager.Decompress7Zip(xmlString);
                                    }
                                    break;
                            }
                        }

                        return SerializationManager.DeserializeXML(returnType, xmlString);
                }
            }
            catch
            {
            }

            return errorReturnValue;
        }
    }
}
