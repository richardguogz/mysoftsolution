using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MySoft.IoC.Message;
using MySoft.Security;

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
        /// <summary>
        /// ���淽��
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> dictMethods = new Dictionary<string, MethodInfo>();

        private IServiceContainer container;
        private Type serviceInterfaceType;
        private object serviceInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        /// <param name="serviceInstance">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType, object serviceInstance)
            : base(container, serviceInterfaceType.FullName)
        {
            this.container = container;
            this.serviceInterfaceType = serviceInterfaceType;
            this.serviceInstance = serviceInstance;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            //�������������Ϊnull
            if (reqMsg == null) return null;

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = reqMsg.TransactionId;
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = reqMsg.SubServiceName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Compress = reqMsg.Compress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Expiration = reqMsg.Expiration;

            #region ��ȡ��Ӧ�ķ���

            MethodInfo method = null;

            string serviceKey = string.Format("{0}|{1}", reqMsg.ServiceName, reqMsg.SubServiceName);
            if (dictMethods.ContainsKey(serviceKey))
            {
                method = dictMethods[serviceKey];
            }
            else
            {
                method = CoreHelper.GetMethodFromType(serviceInterfaceType, reqMsg.SubServiceName);
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
                    lock (dictMethods)
                    {
                        dictMethods[serviceKey] = method;
                    }
                }
            }

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
            resMsg.ServiceName = serviceInstance.GetType().FullName;
            resMsg.SubServiceName = method.ToString();
            resMsg.ReturnType = method.ReturnType;

            //�������ط���
            var service = AspectManager.GetService(serviceInstance);

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