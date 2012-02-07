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
            reqMsg.CacheTime = -1;                                          //���û���ʱ��

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

                //�������
                if (config.Json) JsonInParameter(reqMsg);
            }

            #endregion

            //��ȡԼ����Ϣ
            var opContract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(methodInfo);
            int clientCacheTime = -1;
            if (opContract != null)
            {
                if (opContract.ServerCacheTime > 0) reqMsg.CacheTime = opContract.ServerCacheTime;
                if (opContract.ClientCacheTime > 0) clientCacheTime = opContract.ClientCacheTime;
            }

            try
            {
                string cacheKey = GetCacheKey(reqMsg, opContract);
                var resMsg = CacheHelper.Get<ResponseMessage>(cacheKey);

                //���÷���
                if (resMsg == null)
                {
                    //���ö��
                    var timesCount = config.Times;
                    if (timesCount < 1) timesCount = 1;
                    for (int times = 0; times < timesCount; times++)
                    {
                        resMsg = service.CallService(reqMsg);
                        if (resMsg != null) break;
                    }

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

        /// <summary>
        /// ��ȡ����Keyֵ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="opContract"></param>
        /// <returns></returns>
        private string GetCacheKey(RequestMessage reqMsg, OperationContractAttribute opContract)
        {
            if (opContract != null && !string.IsNullOrEmpty(opContract.CacheKey))
            {
                string cacheKey = opContract.CacheKey;
                foreach (var key in reqMsg.Parameters.Keys)
                {
                    string name = "{" + key + "}";
                    if (cacheKey.Contains(name))
                    {
                        var parameter = reqMsg.Parameters[key];
                        if (parameter != null)
                            cacheKey = cacheKey.Replace(name, parameter.ToString());
                    }
                }

                return string.Format("{0}_{1}", reqMsg.ServiceName, cacheKey);
            }

            return string.Format("ClientCache_{0}_{1}_{2}", reqMsg.ServiceName, reqMsg.MethodName, reqMsg.Parameters);
        }

        #endregion
    }
}
