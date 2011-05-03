using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;
using Castle.Core.Interceptor;
using System.IO;

namespace MySoft.IoC.Services
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType)
            : base(serviceInterfaceType.FullName)
        {
            this.container = container;
            this.OnLog += container.WriteLog;
            this.OnError += container.WriteError;
            this.serviceInterfaceType = serviceInterfaceType;
        }


        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            if (container == null || reqMsg == null)
            {
                return null;
            }

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = reqMsg.TransactionId;
            resMsg.RequestAddress = reqMsg.RequestAddress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Compress = reqMsg.Compress;
            resMsg.Encrypt = reqMsg.Encrypt;
            resMsg.Timeout = reqMsg.Timeout;
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = reqMsg.SubServiceName;
            resMsg.Parameters = reqMsg.Parameters;
            resMsg.Expiration = reqMsg.Expiration;

            object service = null;
            try
            {
                service = container[serviceInterfaceType];
            }
            catch { }
            if (service == null)
            {
                resMsg.Exception = new IoCException(string.Format("The server not find matching service ({0}).", resMsg.ServiceName));
                return resMsg;
            }

            try
            {
                #region ��ȡ��Ӧ�ķ���

                MethodInfo method = null;
                if (dictMethods.ContainsKey(resMsg.SubServiceName))
                {
                    method = dictMethods[resMsg.SubServiceName];
                }
                else
                {
                    method = serviceInterfaceType.GetMethods()
                               .Where(p => p.ToString() == resMsg.SubServiceName)
                               .FirstOrDefault();

                    if (method == null)
                    {
                        foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                        {
                            method = inheritedInterface.GetMethods()
                                    .Where(p => p.ToString() == resMsg.SubServiceName)
                                    .FirstOrDefault();

                            if (method != null) break;
                        }
                    }

                    if (method == null)
                    {
                        resMsg.Exception = new IoCException(string.Format("The server not find called method ({0},{1}).", resMsg.ServiceName, resMsg.SubServiceName));
                        return resMsg;
                    }
                    else
                    {
                        dictMethods[resMsg.SubServiceName] = method;
                    }
                }

                ParameterInfo[] pis = method.GetParameters();
                object[] paramValues = new object[pis.Length];

                for (int i = 0; i < pis.Length; i++)
                {
                    Type type = pis[i].ParameterType;
                    if (!type.IsByRef)
                    {
                        paramValues[i] = resMsg.Parameters[pis[i].Name];
                    }
                    else
                    {
                        paramValues[i] = this.GetType().GetMethod("DefaultValue", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(type.GetElementType()).Invoke(this, null);
                    }
                }

                #endregion

                //�������ط���
                service = AspectManager.GetService(service);

                //���ö�Ӧ�ķ���
                object returnValue = DynamicCalls.GetMethodInvoker(method).Invoke(service, paramValues);

                //�ѷ���ֵ���ݻ�ȥ
                for (int i = 0; i < pis.Length; i++)
                {
                    Type type = pis[i].ParameterType;
                    if (type.IsByRef)
                    {
                        //��������ֵ
                        resMsg.Parameters[type.Name] = paramValues[i];
                    }
                }

                if (returnValue != null)
                {
                    #region ��������

                    //������ϵ�л���byte����
                    resMsg.Data = SerializationManager.SerializeBin(returnValue);

                    //�ж��Ƿ�ѹ��
                    if (resMsg.Compress)
                    {
                        resMsg.Data = CompressionManager.CompressSharpZip(resMsg.Data);
                    }

                    //�ж��Ƿ����
                    if (resMsg.Encrypt)
                    {
                        //�����ܱ�16������ʾΪ��Ч��key
                        if (reqMsg.KeyLength != 0 && reqMsg.KeyLength % 16 == 0)
                        {
                            resMsg.KeyLength = reqMsg.KeyLength;
                        }
                        else
                        {
                            resMsg.KeyLength = 128;
                        }

                        //��ȡ���ܵ��ַ���
                        var encrypt = BigInteger.GenerateRandom(resMsg.KeyLength).ToString();
                        resMsg.Keys = MD5.Hash(Encoding.UTF8.GetBytes(encrypt));

                        //������ʱ������
                        resMsg.Data = XXTEA.Encrypt(resMsg.Data, resMsg.Keys);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //����ȫ�ִ���
                resMsg.Exception = GetNewException(ex);
            }

            return resMsg;
        }

        /// <summary>
        /// Defaults the value.
        /// </summary>
        /// <returns></returns>
        protected object DefaultValue<MemberType>()
        {
            return default(MemberType);
        }

        /// <summary>
        /// ��ȡ�µ�Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Exception GetNewException(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            else
                return new IoCException(ex.Message, ErrorHelper.GetInnerException(ex.InnerException));
        }
    }
}
