using System;
using System.Collections.Generic;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Cache;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        private IServiceContainer container;
        private IService service;
        private Type serviceType;
        private IServiceCache cache;
        private string hostName;
        private string ipAddress;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        /// <param name="service"></param>
        /// <param name="serviceType"></param>
        /// <param name="cache"></param>
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IServiceContainer container, IService service, Type serviceType, IServiceCache cache)
        {
            this.config = config;
            this.container = container;
            this.serviceType = serviceType;
            this.service = service;
            this.cache = cache;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();

            this.cacheTimes = new Dictionary<string, int>();
            var methods = CoreHelper.GetMethodsFromType(serviceType);
            foreach (var method in methods)
            {
                var contract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(method);
                if (contract != null && contract.CacheTime > 0)
                    cacheTimes[method.ToString()] = contract.CacheTime;
            }
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">Name of the sub service.</param>
        /// <returns>The result.</returns>
        private ResponseMessage CallService(RequestMessage reqMsg)
        {
            ResponseMessage resMsg = null;

            try
            {
                resMsg = service.CallService(reqMsg);

                //������쳣�������׳�
                if (resMsg.IsError) throw resMsg.Error;
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

            return resMsg;
        }

        #region IInvocationHandler ��Ա

        /// <summary>
        /// ��Ӧί��
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoke(object proxy, System.Reflection.MethodInfo method, object[] parameters)
        {
            object returnValue = null;

            #region ����������Ϣ

            var reqMsg = new RequestMessage
            {
                AppName = config.AppName,                       //Ӧ������
                HostName = hostName,                            //�ͻ�������
                IPAddress = ipAddress,                          //�ͻ���IP��ַ
                ReturnType = method.ReturnType,                 //��������
                ServiceName = serviceType.FullName,             //��������
                MethodName = method.ToString(),                 //��������
                TransactionId = Guid.NewGuid()                  //����ID��
            };

            //���õ��÷���
            (reqMsg as IInvoking).MethodInfo = method;

            //�����Ƿ񻺴�
            if (cacheTimes.ContainsKey(method.ToString()))
            {
                reqMsg.IsCaching = true;
            }

            #endregion

            reqMsg.Parameters = ServiceConfig.CreateParameters(method, parameters);
            string cacheKey = ServiceConfig.GetCacheKey(serviceType, method, reqMsg.Parameters);
            var cacheValue = cache.Get<CacheObject>(cacheKey);

            //������ֵ
            if (cacheValue == null)
            {
                //���÷���
                var resMsg = CallService(reqMsg);

                if (resMsg != null)
                {
                    returnValue = resMsg.Value;

                    //�������
                    ServiceConfig.SetParameterValue(method, parameters, resMsg.Parameters);

                    //�����Ҫ���棬����뱾�ػ���
                    if (returnValue != null && cacheTimes.ContainsKey(method.ToString()))
                    {
                        int cacheTime = cacheTimes[method.ToString()];
                        cacheValue = new CacheObject
                        {
                            Value = resMsg.Value,
                            Parameters = resMsg.Parameters
                        };

                        cache.Insert(cacheKey, cacheValue, cacheTime);
                    }
                }
            }
            else
            {
                //������ֵ
                returnValue = cacheValue.Value;

                //�������
                ServiceConfig.SetParameterValue(method, parameters, cacheValue.Parameters);
            }

            //���ؽ��
            return returnValue;
        }

        #endregion
    }
}
