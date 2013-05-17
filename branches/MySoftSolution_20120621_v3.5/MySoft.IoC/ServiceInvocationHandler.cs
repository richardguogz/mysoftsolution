using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySoft.IoC.Configuration;
using MySoft.IoC.Logger;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    internal class ServiceInvocationHandler<T> : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        private IDictionary<string, string> errors;
        private IContainer container;
        private IService service;
        private AsyncCaller caller;
        private IServiceLog logger;
        private string hostName;
        private string ipAddress;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        /// <param name="service"></param>
        /// <param name="caller"></param>
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IContainer container, IService service, AsyncCaller caller, IServiceLog logger)
        {
            this.config = config;
            this.container = container;
            this.service = service;
            this.logger = logger;
            this.caller = caller;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();

            this.cacheTimes = new Dictionary<string, int>();
            this.errors = new Dictionary<string, string>();

            var methods = CoreHelper.GetMethodsFromType(typeof(T));
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
                InvokeMethod = false,
                AppVersion = "v2.5",                                //�汾��
                AppName = config.AppName,                           //Ӧ������
                AppPath = AppDomain.CurrentDomain.BaseDirectory,    //Ӧ��·��
                HostName = hostName,                                //�ͻ�������
                IPAddress = ipAddress,                              //�ͻ���IP��ַ
                ServiceName = typeof(T).FullName,                   //��������
                MethodName = method.ToString(),                     //��������
                EnableCache = config.EnableCache,                   //�Ƿ񻺴�
                TransactionId = Guid.NewGuid(),                     //����ID��
                MethodInfo = method,                                //���õ��÷���
                Parameters = collection,                            //���ò���
                RespType = ResponseType.Binary                      //��������
            };

            //���û���ʱ��
            if (cacheTimes.ContainsKey(method.ToString()))
            {
                reqMsg.CacheTime = cacheTimes[method.ToString()];
            }

            #endregion

            //���巵��ֵ
            object returnValue = null;

            //���÷���
            var resMsg = CallService(reqMsg);

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

                //��ʼһ����ʱ��
                var watch = Stopwatch.StartNew();

                try
                {
                    //��ȡ������
                    using (var context = GetOperationContext(reqMsg))
                    {
                        //�첽���÷���
                        resMsg = caller.Run(service, context, reqMsg).Message;
                    }

                    //д��־����
                    logger.EndRequest(reqMsg, resMsg, watch.ElapsedMilliseconds);
                }
                finally
                {
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                }

                //������쳣�������׳�
                if (resMsg.IsError) throw resMsg.Error;
            }
            catch (BusinessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (config.ThrowError)
                {
                    //�ж��Ƿ����Զ����쳣
                    if (errors.ContainsKey(reqMsg.MethodName))
                        throw new BusinessException(errors[reqMsg.MethodName]);
                    else
                        throw;
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
                AppVersion = reqMsg.AppVersion,
                AppPath = reqMsg.AppPath,
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