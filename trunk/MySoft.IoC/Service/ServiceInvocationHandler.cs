using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using System.IO;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IInvocationHandler
    {
        private IServiceContainer container;
        private Type serviceInterfaceType;
        private int cacheTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public ServiceInvocationHandler(IServiceContainer container, Type serviceInterfaceType, int cacheTimeout)
        {
            this.container = container;
            this.serviceInterfaceType = serviceInterfaceType;
            this.cacheTimeout = cacheTimeout;
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="methodInfo">Name of the sub service.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>The result.</returns>
        private object CallService(MethodInfo methodInfo, params object[] paramValues)
        {
            RequestMessage reqMsg = new RequestMessage();
            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = methodInfo.ToString();
            reqMsg.TransactionId = Guid.NewGuid();

            #region �ж����ݸ�ʽ

            if (container.Proxy != null)
            {
                //���ݴ�����ѹ����ʽ
                reqMsg.Encrypt = container.Proxy.Encrypt;

                //����ѹ����ʽ
                reqMsg.Compress = container.Proxy.Compress;

                //���ó�ʱʱ��
                reqMsg.Timeout = container.Proxy.Timeout;
            }

            #endregion

            #region �������

            ParameterInfo[] pis = methodInfo.GetParameters();
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
                        //������ݵ������ã�������
                        if (!pis[i].ParameterType.IsByRef)
                        {
                            reqMsg.Parameters[pis[i].Name] = paramValues[i];
                        }
                    }
                }
            }

            #endregion

            #region ������

            //����cacheKey��Ϣ
            var key = string.Format("{0}_{1}_{2}", reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.Parameters);
            string cacheKey = "IoC_Cache_" + Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
            cacheKey = string.Format("{0}_{1}", serviceInterfaceType.FullName, cacheKey);

            bool isAllowCache = false;
            int cacheTime = cacheTimeout; //Ĭ�ϻ���ʱ����ϵͳ���õ�ʱ��һ��

            #region ��ȡԼ����Ϣ

            //��ȡԼ����Ϣ
            var serviceContract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);

            //��ȡԼ����Ϣ
            var operationContract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(methodInfo);

            //�ж�Լ��
            if (serviceContract != null)
            {
                isAllowCache = serviceContract.AllowCache;
                if (serviceContract.CacheTime > 0) cacheTime = serviceContract.CacheTime;
                if (serviceContract.Timeout > 0) reqMsg.Timeout = serviceContract.Timeout;
            }

            //�ж�Լ��
            if (operationContract != null)
            {
                if (operationContract.CacheTime > 0) cacheTime = operationContract.CacheTime;
                if (operationContract.Timeout > 0) reqMsg.Timeout = operationContract.Timeout;
            }

            #endregion

            //���巵�ص�ֵ
            object returnValue = null;
            ParameterCollection parameters = null;

            //�������
            ServiceCache cacheValue = null;

            //����Ĵ���
            if (isAllowCache && container.Cache != null)
            {
                //�ӻ����ȡ����
                cacheValue = container.Cache.GetCache(cacheKey) as ServiceCache;
            }

            //������治Ϊnull;
            if (cacheValue != null)
            {
                parameters = cacheValue.Parameters;
                returnValue = cacheValue.CacheObject;
            }
            else
            {
                //���÷���
                ResponseMessage resMsg = container.CallService(reqMsg);

                //�������Ϊnull,�򷵻�null
                if (resMsg == null || resMsg.Data == null)
                {
                    return this.GetType().GetMethod("DefaultValue", BindingFlags.Instance | BindingFlags.NonPublic)
                                .MakeGenericMethod(methodInfo.ReturnType).Invoke(this, null);
                }

                #region �����ص�����

                //����
                parameters = resMsg.Parameters;

                //�����Ƿ����
                if (resMsg.Encrypt)
                {
                    //������ʱ������
                    resMsg.Data = XXTEA.Decrypt(resMsg.Data, resMsg.Keys);
                }

                //�����Ƿ�ѹ��
                if (resMsg.Compress)
                {
                    resMsg.Data = CompressionManager.DecompressSharpZip(resMsg.Data);
                }

                //��byte���鷴ϵ�л��ɶ���
                returnValue = SerializationManager.DeserializeBin(resMsg.Data);

                #endregion

                if (cacheValue != null)
                {
                    //����Ĵ���
                    if (isAllowCache && container.Cache != null)
                    {
                        cacheValue = new ServiceCache { CacheObject = returnValue, Parameters = resMsg.Parameters };

                        //��ֵ��ӵ�������
                        container.Cache.AddCache(cacheKey, cacheValue, cacheTime);
                    }
                }
            }

            #endregion

            //�����õĲ�����ֵ
            for (int i = 0; i < pis.Length; i++)
            {
                Type type = pis[i].ParameterType;
                if (type.IsByRef)
                {
                    //��������ֵ
                    paramValues[i] = parameters[type.Name];
                }
            }

            //��������
            return returnValue;
        }

        /// <summary>
        /// Defaults the value.
        /// </summary>
        /// <returns></returns>
        protected object DefaultValue<MemberType>()
        {
            return default(MemberType);
        }

        #region IInvocationHandler ��Ա

        /// <summary>
        /// ��Ӧί��
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(object proxy, MethodInfo method, object[] args)
        {
            return this.CallService(method, args);
        }

        #endregion
    }
}
