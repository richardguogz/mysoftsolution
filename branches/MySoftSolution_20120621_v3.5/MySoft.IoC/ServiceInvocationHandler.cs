using System;
using System.Collections.Generic;
using MySoft.Cache;
using MySoft.IoC.Configuration;
using MySoft.IoC.Logger;
using MySoft.IoC.Messages;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        private IDictionary<string, int> callTimes;
        private IServiceContainer container;
        private IService service;
        private Type serviceType;
        private ICacheStrategy cache;
        private IServiceLog logger;
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
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IServiceContainer container, IService service, Type serviceType, ICacheStrategy cache, IServiceLog logger)
        {
            this.config = config;
            this.container = container;
            this.serviceType = serviceType;
            this.service = service;
            this.cache = cache;
            this.logger = logger;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();

            this.cacheTimes = new Dictionary<string, int>();
            this.callTimes = new Dictionary<string, int>();

            var methods = CoreHelper.GetMethodsFromType(serviceType);
            foreach (var method in methods)
            {
                var contract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(method);
                if (contract != null)
                {
                    if (contract.CacheTime > 0)
                        cacheTimes[method.ToString()] = contract.CacheTime;

                    if (contract.Timeout > 0)
                        callTimes[method.ToString()] = contract.Timeout;
                }
            }
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
            var collection = IoCHelper.CreateParameters(method, parameters);
            string cacheKey = IoCHelper.GetCacheKey(serviceType, method, collection);
            var cacheValue = cache.GetCache<CacheObject>(cacheKey);

            //������ֵ
            if (cacheValue == null)
            {
                //���÷���
                var resMsg = InvokeMethod(method, collection);

                if (resMsg != null)
                {
                    returnValue = resMsg.Value;

                    //��������
                    IoCHelper.SetRefParameterValues(method, resMsg.Parameters, parameters);

                    //�����Ҫ���棬����뱾�ػ���
                    if (returnValue != null && cacheTimes.ContainsKey(method.ToString()))
                    {
                        int cacheTime = cacheTimes[method.ToString()];
                        cacheValue = new CacheObject
                        {
                            Value = resMsg.Value,
                            Parameters = resMsg.Parameters
                        };

                        cache.InsertCache(cacheKey, cacheValue, cacheTime);
                    }
                }
            }
            else
            {
                //��������ֵ
                returnValue = cacheValue.Value;

                //��������
                IoCHelper.SetRefParameterValues(method, cacheValue.Parameters, parameters);
            }

            //���ؽ��
            return returnValue;
        }

        /// <summary>
        /// ���÷�������
        /// </summary>
        /// <param name="method"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        private ResponseMessage InvokeMethod(System.Reflection.MethodInfo method, ParameterCollection collection)
        {
            #region ����������Ϣ

            int timeout = -1;

            //��ȡ��ʱʱ��
            if (callTimes.ContainsKey(method.ToString()))
            {
                timeout = callTimes[method.ToString()];
            }

            var reqMsg = new RequestMessage
            {
                AppName = config.AppName,                       //Ӧ������
                HostName = hostName,                            //�ͻ�������
                IPAddress = ipAddress,                          //�ͻ���IP��ַ
                ReturnType = method.ReturnType,                 //��������
                ServiceName = serviceType.FullName,             //��������
                MethodName = method.ToString(),                 //��������
                TransactionId = Guid.NewGuid(),                 //����ID��
                MethodInfo = method,                            //���õ��÷���
                Parameters = collection,                        //���ò���
                Timeout = timeout                               //��ʱʱ�䣨�룩
            };

            #endregion

            //���÷���
            return CallService(reqMsg);
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">Name of the sub service.</param>
        /// <returns>The result.</returns>
        protected virtual ResponseMessage CallService(RequestMessage reqMsg)
        {
            ResponseMessage resMsg = null;

            try
            {
                //д��־��ʼ
                logger.BeginRequest(reqMsg);

                //���÷���
                resMsg = service.CallService(reqMsg);

                //д��־����
                logger.EndRequest(reqMsg, resMsg, resMsg.ElapsedTime);

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

        #endregion
    }
}