using System;
using System.Collections.Generic;
using MySoft.Cache;
using MySoft.IoC.Configuration;
using MySoft.IoC.Logger;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;
using System.Diagnostics;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        private IDictionary<string, string> errors;
        private IServiceContainer container;
        private AsyncCaller asyncCaller;
        private IService service;
        private Type serviceType;
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
            this.logger = logger;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();

            this.cacheTimes = new Dictionary<string, int>();
            this.errors = new Dictionary<string, string>();

            var waitTime = TimeSpan.FromSeconds(ServiceConfig.DEFAULT_WAIT_TIMEOUT);

            //ʵ�����첽����
            if (config.EnableCache)
                this.asyncCaller = new AsyncCaller(container, service, waitTime, cache, false);
            else
                this.asyncCaller = new AsyncCaller(container, service, waitTime, false);

            var methods = CoreHelper.GetMethodsFromType(serviceType);
            foreach (var method in methods)
            {
                var contract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(method);
                if (contract != null)
                {
                    if (contract.CacheTime > 0)
                        cacheTimes[method.ToString()] = contract.CacheTime;

                    if (!string.IsNullOrEmpty(contract.ErrorMessage))
                        errors[method.ToString()] = contract.ErrorMessage;
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
            #region ����������Ϣ

            var collection = IoCHelper.CreateParameters(method, parameters);

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
                TransferType = TransferType.Binary              //��������
            };

            //���û���ʱ��
            if (cacheTimes.ContainsKey(method.ToString()))
            {
                reqMsg.CacheTime = cacheTimes[method.ToString()];
            }

            #endregion

            //���÷���
            var resMsg = CallService(reqMsg);

            //���巵��ֵ
            object returnValue = null;

            if (resMsg != null)
            {
                returnValue = resMsg.Value;

                //�������
                IoCHelper.SetRefParameters(method, resMsg.Parameters, parameters);
            }

            //���ؽ��
            return returnValue;
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

                //��ȡ������
                var context = GetOperationContext(reqMsg);

                //��ʼ��ʱ
                var watch = Stopwatch.StartNew();

                try
                {
                    //�첽���÷���
                    resMsg = asyncCaller.AsyncCall(context, reqMsg);
                }
                finally
                {
                    watch.Stop();
                }

                //д��־����
                logger.EndRequest(reqMsg, resMsg, watch.ElapsedMilliseconds);

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
                {
                    //�ж��Ƿ����Զ����쳣
                    if (errors.ContainsKey(reqMsg.MethodName))
                        throw new BusinessException(errors[reqMsg.MethodName]);
                    else
                        throw ex;
                }
            }

            return resMsg;
        }

        /// <summary>
        /// ��ȡ�����Ķ���
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private OperationContext GetOperationContext(RequestMessage reqMsg)
        {
            var caller = new AppCaller
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory,
                AppName = reqMsg.AppName,
                IPAddress = reqMsg.IPAddress,
                HostName = reqMsg.HostName,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName,
                Parameters = reqMsg.Parameters.ToString(),
                CallTime = DateTime.Now
            };

            return new OperationContext
            {
                Container = container,
                Caller = caller
            };
        }

        #endregion
    }
}
