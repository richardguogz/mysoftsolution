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
        /// ���淽��
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

            //��ȡԼ����ʽ
            var contract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);
            if (contract != null)
            {
                //������Ӧ��ʽ
                switch (contract.Format)
                {
                    case ResponseFormat.Binary:
                        reqMsg.Transfer = TransferType.Binary;
                        break;
                    case ResponseFormat.Json:
                        reqMsg.Transfer = TransferType.Json;
                        break;
                    case ResponseFormat.Xml:
                        reqMsg.Transfer = TransferType.Xml;
                        break;
                }

                //���ó�ʱʱ��
                reqMsg.Timeout = contract.Timeout;
            }
            else
            {
                //���ݴ�����ѹ����ʽ
                reqMsg.Transfer = container.Transfer;
            }

            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = subServiceName;
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
                //��������ȷֱ�ӷ����쳣
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

            //���÷���
            ResponseMessage resMsg = container.CallService(reqMsg);
            if (resMsg.Data == null) return resMsg.Data;

            switch (resMsg.Transfer)
            {
                default:
                case TransferType.Binary:
                    byte[] buffer = (byte[])resMsg.Data;
                    return SerializationManager.DeserializeBin(buffer);
                case TransferType.Json:
                    string jsonString = resMsg.Data.ToString();
                    return SerializationManager.DeserializeJson(returnType, jsonString);
                case TransferType.Xml:
                    string xmlString = resMsg.Data.ToString();
                    return SerializationManager.DeserializeXml(returnType, xmlString);
            }
        }
    }
}
