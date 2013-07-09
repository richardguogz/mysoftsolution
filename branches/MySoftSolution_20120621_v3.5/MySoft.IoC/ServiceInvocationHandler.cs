using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using System;
using System.Collections.Generic;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    internal class ServiceInvocationHandler<T> : BaseServiceHandler, IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        private IDictionary<string, string> errors;
        private string hostName;
        private string ipAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        /// <param name="call"></param>
        /// <param name="service"></param>
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IServiceContainer container, IServiceCall call, IService service)
            : base(config, container, call, service)
        {
            this.config = config;
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
                AppVersion = ServiceConfig.CURRENT_FRAMEWORK_VERSION,       //�汾��
                AppName = config.AppName,                                   //Ӧ������
                AppPath = AppDomain.CurrentDomain.BaseDirectory,            //Ӧ��·��
                HostName = hostName,                                        //�ͻ�������
                IPAddress = ipAddress,                                      //�ͻ���IP��ַ
                ServiceName = typeof(T).FullName,                           //��������
                MethodName = method.ToString(),                             //��������
                EnableCache = config.EnableCache,                           //�Ƿ񻺴�
                TransactionId = Guid.NewGuid(),                             //����ID��
                MethodInfo = method,                                        //���õ��÷���
                Parameters = collection,                                    //���ò���
                RespType = ResponseType.Binary                              //��������
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
            var resMsg = InvokeRequest(reqMsg);

            if (resMsg != null)
            {
                returnValue = resMsg.Value;

                //�������
                IoCHelper.SetRefParameters(method, resMsg.Parameters, parameters);
            }
            else
            {
                //Ĭ��ֵ
                returnValue = CoreHelper.GetTypeDefaultValue(method.ReturnType);
            }

            //���ؽ��
            return returnValue;
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">Name of the sub service.</param>
        /// <returns>The result.</returns>
        private ResponseMessage InvokeRequest(RequestMessage reqMsg)
        {
            try
            {
                return CallService(reqMsg);
            }
            catch (Exception ex)
            {
                if (ex is BusinessException) throw;

                if (config.ThrowError)
                {
                    //�ж��Ƿ����Զ����쳣
                    if (errors.ContainsKey(reqMsg.MethodName))
                        throw new BusinessException(errors[reqMsg.MethodName]);
                    else
                        throw;
                }
            }

            return null;
        }

        #endregion
    }
}