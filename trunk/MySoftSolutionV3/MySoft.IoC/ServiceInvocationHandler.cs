using System;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        protected CastleFactoryConfiguration config;
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
            this.serviceType = serviceType;
            this.service = service;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="methodInfo">Name of the sub service.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>The result.</returns>
        private object CallService(System.Reflection.MethodInfo methodInfo, object[] paramValues)
        {
            #region ����������Ϣ

            RequestMessage reqMsg = new RequestMessage();
            reqMsg.AppName = config.AppName;                                //Ӧ������
            reqMsg.HostName = hostName;                                     //�ͻ�������
            reqMsg.IPAddress = ipAddress;                                   //�ͻ���IP��ַ
            reqMsg.ServiceName = serviceType.FullName;                      //��������
            reqMsg.MethodName = methodInfo.ToString();                      //��������
            reqMsg.ReturnType = methodInfo.ReturnType;                      //��������
            reqMsg.TransactionId = Guid.NewGuid();                          //����ID��

            #endregion

            #region �������

            var pis = methodInfo.GetParameters();
            if (paramValues != null && pis.Length != paramValues.Length)
            {
                //��������ȷֱ�ӷ����쳣
                string title = string.Format("Invalid parameters ({0},{1}).", reqMsg.ServiceName, reqMsg.MethodName);
                string body = string.Format("{0}\r\nParameters ==> {1}", title, reqMsg.Parameters);
                throw new WarningException(body)
                {
                    ApplicationName = reqMsg.AppName,
                    ServiceName = reqMsg.ServiceName,
                    ErrorHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    //����Ĭ��ֵ
                    reqMsg.Parameters[pis[i].Name] = paramValues[i] ?? CoreHelper.GetTypeDefaultValue(pis[i].ParameterType);
                }

                //�������
                if (config.Json) JsonInParameter(reqMsg);
            }

            #endregion

            //��ȡԼ����Ϣ
            var opContract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(methodInfo);
            int clientCacheTime = -1;
            if (opContract != null)
            {
                if (opContract.ClientCacheTime > 0) clientCacheTime = opContract.ClientCacheTime;
            }

            try
            {
                string cacheKey = ServiceConfig.GetCacheKey(reqMsg, opContract);
                var resMsg = CacheHelper.Get<ResponseMessage>(cacheKey);

                //���÷���
                if (resMsg == null)
                {
                    //���÷���
                    resMsg = service.CallService(reqMsg);

                    //�������Ϊnull,�򷵻�null
                    if (resMsg == null)
                    {
                        return CoreHelper.GetTypeDefaultValue(methodInfo.ReturnType);
                    }

                    //������쳣�������׳�
                    if (resMsg.IsError) throw resMsg.Error;

                    //�������
                    if (config.Json) JsonOutParameter(pis, resMsg);

                    //����ͻ��˻���ʱ�����0
                    if (clientCacheTime > 0)
                    {
                        //û���쳣���򻺴�����
                        CacheHelper.Insert(cacheKey, resMsg, clientCacheTime);
                    }
                }

                //�����õĲ�����ֵ
                for (int i = 0; i < pis.Length; i++)
                {
                    if (pis[i].ParameterType.IsByRef)
                    {
                        //��������ֵ
                        paramValues[i] = resMsg.Parameters[pis[i].Name];
                    }
                }

                //��������
                return resMsg.Value;
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

            //����Ĭ��ֵ
            return CoreHelper.GetTypeDefaultValue(methodInfo.ReturnType);
        }

        /// <summary>
        /// Json���봦��
        /// </summary>
        /// <param name="reqMsg"></param>
        protected virtual void JsonInParameter(RequestMessage reqMsg)
        {
            //Json�������
        }

        /// <summary>
        /// Json�������
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="resMsg"></param>
        protected virtual void JsonOutParameter(System.Reflection.ParameterInfo[] parameters, ResponseMessage resMsg)
        {
            //Json�������
        }

        #region IInvocationHandler ��Ա

        /// <summary>
        /// ��Ӧί��
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(object proxy, System.Reflection.MethodInfo method, object[] args)
        {
            return CallService(method, args);
        }

        #endregion
    }
}
