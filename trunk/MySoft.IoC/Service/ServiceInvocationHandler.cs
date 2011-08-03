using System;
using System.Reflection;
using MySoft.IoC.Configuration;
using MySoft.IoC.Message;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IServiceContainer container;
        private IService service;
        private Type serviceType;
        private string hostName;
        private string ipAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="container">config.</param>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IServiceContainer container, IService service, Type serviceType)
        {
            this.config = config;
            this.container = container;
            this.service = service;
            this.serviceType = serviceType;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="methodInfo">Name of the sub service.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>The result.</returns>
        private object CallService(MethodInfo methodInfo, object[] paramValues)
        {
            #region ����������Ϣ

            RequestMessage reqMsg = new RequestMessage();
            reqMsg.AppName = config.AppName;                                //Ӧ������
            reqMsg.HostName = hostName;                                     //�ͻ�������
            reqMsg.IPAddress = ipAddress;                                   //�ͻ���IP��ַ
            reqMsg.ServiceName = serviceType.FullName;                      //��������
            reqMsg.SubServiceName = methodInfo.ToString();                  //��������
            reqMsg.ReturnType = methodInfo.ReturnType;                      //��������
            reqMsg.TransactionId = Guid.NewGuid();                          //����ID��
            //reqMsg.Encrypt = config.Encrypt;                                                              //���ݴ�����ѹ����ʽ
            //reqMsg.Compress = config.Compress;                                                       //����ѹ����ʽ
            reqMsg.Timeout = config.Timeout;                                //���ó�ʱʱ��
            reqMsg.Expiration = DateTime.Now.AddSeconds(config.Timeout);    //���ù���ʱ��

            #endregion

            #region �������

            ParameterInfo[] pis = methodInfo.GetParameters();
            if ((pis.Length == 0 && paramValues != null && paramValues.Length > 0) || (paramValues != null && pis.Length != paramValues.Length))
            {
                //��������ȷֱ�ӷ����쳣
                string title = string.Format("Invalid parameters ({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName);
                string body = string.Format("{0}\r\nParameters ==> {1}", title, reqMsg.Parameters.SerializedData);
                throw new WarningException(body)
                {
                    ApplicationName = reqMsg.AppName,
                    ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    if (paramValues[i] != null)
                    {
                        if (!pis[i].ParameterType.IsByRef)
                        {
                            //������ݵ������ã�������
                            reqMsg.Parameters[pis[i].Name] = paramValues[i];
                        }
                        else if (!pis[i].IsOut)
                        {
                            //������ݵ������ã�������
                            reqMsg.Parameters[pis[i].Name] = paramValues[i];
                        }
                    }
                    else
                    {
                        //���ݲ���ֵΪnull
                        reqMsg.Parameters[pis[i].Name] = null;
                    }
                }
            }

            #endregion

            #region ������

            //����cacheKey��Ϣ
            string cacheKey = string.Format("IoC_Cache_{0}_{1}", reqMsg.SubServiceName, reqMsg.Parameters);

            bool isAllowCache = false;
            double cacheTime = config.CacheTime; //Ĭ�ϻ���ʱ����ϵͳ���õ�ʱ��һ��

            #region ��ȡԼ����Ϣ

            //��ȡԼ����Ϣ
            var serviceContract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceType);

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
                if (isAllowCache) isAllowCache = operationContract.AllowCache;
                if (operationContract.CacheTime > 0) cacheTime = operationContract.CacheTime;
                if (operationContract.Timeout > 0) reqMsg.Timeout = operationContract.Timeout;

                //ѹ������
                reqMsg.Compress = config.Compress && operationContract.Compress;
                reqMsg.Encrypt = config.Encrypt && operationContract.Encrypt;
            }
            else
            {
                //Ĭ�Ϸ��������л���
                isAllowCache = false;
            }

            //���ù���ʱ��
            reqMsg.Expiration = DateTime.Now.AddSeconds(reqMsg.Timeout);

            #endregion

            //���巵�ص�ֵ
            object returnValue = null;
            ParameterCollection parameters = null;

            //�������
            ServiceCache cacheValue = null;

            //����Ĵ���
            if (isAllowCache && cacheTime > 0 && container.Cache != null)
            {
                //�ӻ����ȡ����
                cacheValue = container.Cache.GetCache(serviceType, cacheKey) as ServiceCache;
            }

            //������治Ϊnull;
            if (cacheValue != null)
            {
                parameters = cacheValue.Parameters;
                returnValue = cacheValue.CacheObject;
            }
            else
            {
                ResponseMessage resMsg = null;
                try
                {
                    //���÷���
                    resMsg = service.CallService(reqMsg, config.LogTime);

                    //������쳣�������׳�
                    if (resMsg != null && resMsg.Exception != null)
                    {
                        throw resMsg.Exception;
                    }
                }
                catch (BusinessException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (config.ThrowError)
                        throw ex;
                    else
                        container.WriteError(ex);
                }

                //����
                parameters = resMsg.Parameters;

                //�������Ϊnull,�򷵻�null
                if (resMsg == null || resMsg.Data == null || resMsg.Data.Value == null)
                {
                    //�����õĲ�����ֵ
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].ParameterType.IsByRef)
                        {
                            //��������ֵ
                            paramValues[i] = parameters[pis[i].Name];
                        }
                    }

                    return CoreHelper.GetTypeDefaultValue(reqMsg.ReturnType);
                }

                //�ӷ��ؽ����ȡֵ
                returnValue = resMsg.Data.Value;

                if (returnValue != null)
                {
                    //����Ĵ���
                    if (isAllowCache && cacheTime > 0 && container.Cache != null)
                    {
                        cacheValue = new ServiceCache { CacheObject = returnValue, Parameters = resMsg.Parameters };

                        //��ֵ��ӵ�������
                        container.Cache.AddCache(serviceType, cacheKey, cacheValue, cacheTime);
                    }
                }
            }

            #endregion

            //�����õĲ�����ֵ
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                {
                    //��������ֵ
                    paramValues[i] = parameters[pis[i].Name];
                }
            }

            //��������
            return returnValue;
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
            return CallService(method, args);
        }

        #endregion
    }
}
