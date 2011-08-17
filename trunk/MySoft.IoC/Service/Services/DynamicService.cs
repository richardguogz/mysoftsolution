using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MySoft.IoC.Message;
using MySoft.Security;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// �첽����ί��
    /// </summary>
    /// <param name="reqMsg"></param>
    /// <returns></returns>
    public delegate ResponseMessage AsyncMethodCaller(RequestMessage reqMsg);

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
            resMsg.SubServiceName = reqMsg.SubServiceName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Compress = reqMsg.Compress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Expiration = reqMsg.Expiration;

            #region ��ȡ��Ӧ�ķ���

            string methodKey = string.Format("Method_{0}_{1}", reqMsg.ServiceName, reqMsg.SubServiceName);
            MethodInfo method = CacheHelper.Get<MethodInfo>(methodKey);
            if (method == null)
            {
                method = CoreHelper.GetMethodFromType(classType, reqMsg.SubServiceName);
                if (method == null)
                {
                    string title = string.Format("The server not find called method ({0},{1}).", reqMsg.ServiceName, reqMsg.SubServiceName);
                    var exception = new WarningException(title)
                    {
                        ApplicationName = reqMsg.AppName,
                        ExceptionHeader = string.Format("Application��{0}��occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };

                    resMsg.Exception = exception;
                    return resMsg;
                }
                else
                {
                    CacheHelper.Insert(methodKey, method, 60);
                }
            }

            //���÷��񼰷�������
            resMsg.ServiceName = instance.GetType().FullName;
            resMsg.SubServiceName = method.ToString();

            ParameterInfo[] pis = method.GetParameters();
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
            resMsg.ServiceName = instance.GetType().FullName;
            resMsg.SubServiceName = method.ToString();
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

                if (returnValue != null)
                {
                    byte[] keys = null;

                    //�ж��Ƿ����
                    if (reqMsg.Encrypt)
                    {
                        int keyLength = 128;

                        //��ȡ���ܵ��ַ���
                        var encrypt = BigInteger.GenerateRandom(keyLength).ToString();
                        keys = MD5.Hash(Encoding.UTF8.GetBytes(encrypt));
                    }

                    //���ؽ������
                    resMsg.Data = new ResponseData(reqMsg, keys, returnValue);
                }
            }
            catch (BusinessException ex)
            {
                if (ex.InnerException == null)
                    resMsg.Exception = new BusinessException(ex.Code, ex.Message);
                else
                    resMsg.Exception = new BusinessException(ex.Code, ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                //����ȫ�ִ���
                resMsg.Exception = ex;
            }

            return resMsg;
        }
    }
}
