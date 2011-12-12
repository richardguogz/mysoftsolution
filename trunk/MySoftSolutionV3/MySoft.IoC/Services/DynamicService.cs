using System;
using System.Reflection;
using MySoft.IoC.Aspect;
using MySoft.IoC.Messages;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        private ILog logger;
        private Type classType;
        private object instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="classType">Type of the service interface.</param>
        public DynamicService(ILog logger, Type classType, object instance)
            : base(logger, classType.FullName)
        {
            this.logger = logger;
            this.instance = instance;
            this.classType = classType;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = reqMsg.TransactionId;
            resMsg.ServiceName = reqMsg.ServiceName;
            resMsg.MethodName = reqMsg.MethodName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Expiration = reqMsg.Expiration;

            #region ��ȡ��Ӧ�ķ���

            string methodKey = string.Format("Method_{0}_{1}", reqMsg.ServiceName, reqMsg.MethodName);
            MethodInfo method = CacheHelper.Get<MethodInfo>(methodKey);
            if (method == null)
            {
                method = CoreHelper.GetMethodFromType(classType, reqMsg.MethodName);
                if (method == null)
                {
                    string title = string.Format("The server not find called method ({0},{1}).", reqMsg.ServiceName, reqMsg.MethodName);
                    var exception = new WarningException(title)
                    {
                        ApplicationName = reqMsg.AppName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };

                    resMsg.Error = exception;
                    return resMsg;
                }
                else
                {
                    CacheHelper.Insert(methodKey, method, 60);
                }
            }

            var pis = method.GetParameters();
            object[] paramValues = new object[pis.Length];

            for (int i = 0; i < pis.Length; i++)
            {
                if (!pis[i].ParameterType.IsByRef)
                {
                    paramValues[i] = resMsg.Parameters[pis[i].Name];
                }
                else if (!pis[i].IsOut)
                {
                    paramValues[i] = resMsg.Parameters[pis[i].Name];
                }
                else
                {
                    paramValues[i] = CoreHelper.GetTypeDefaultValue(pis[i].ParameterType);
                }
            }

            #endregion

            //��ȡ���񼰷�������
            resMsg.ReturnType = method.ReturnType;

            //�������ط���
            var service = AspectManager.GetService(instance);

            try
            {
                //���ö�Ӧ�ķ���
                object returnValue = DynamicCalls.GetMethodInvoker(method).Invoke(service, paramValues);

                //�ѷ���ֵ���ݻ�ȥ
                for (int i = 0; i < pis.Length; i++)
                {
                    if (pis[i].ParameterType.IsByRef)
                    {
                        //��������ֵ
                        resMsg.Parameters[pis[i].Name] = paramValues[i];
                    }
                }

                //���ؽ������
                resMsg.Value = returnValue;
            }
            catch (Exception ex)
            {
                //����ȫ�ִ���
                resMsg.Error = ex;
            }

            return resMsg;
        }
    }
}
