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
        /// 保存方法
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> dictMethods = new Dictionary<string, MethodInfo>();

        private IServiceContainer container;
        private Type serviceInterfaceType;
        private PHPFormatter formatter;

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

            //实现化系列化器
            this.formatter = new PHPFormatter(Encoding.UTF8, AppDomain.CurrentDomain.GetAssemblies());
        }


        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage msg)
        {
            if (container == null || msg == null)
            {
                return null;
            }

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.TransactionId = msg.TransactionId;
            resMsg.RequestAddress = msg.RequestAddress;
            resMsg.Encrypt = msg.Encrypt;
            resMsg.Compress = msg.Compress;
            resMsg.Encrypt = msg.Encrypt;
            resMsg.Timeout = msg.Timeout;
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = msg.SubServiceName;
            resMsg.Parameters = msg.Parameters;
            resMsg.Expiration = msg.Expiration;

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
                object[] parms = new object[pis.Length];

                for (int i = 0; i < pis.Length; i++)
                {
                    Type type = pis[i].ParameterType;

                    if (!pis[i].ParameterType.IsByRef)
                    {
                        parms[i] = resMsg.Parameters[pis[i].Name];
                    }
                    else
                    {
                        parms[i] = this.GetType().GetMethod("DefaultValue", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(pis[i].ParameterType.GetElementType()).Invoke(this, null);
                    }
                }

                //返回拦截服务
                service = AspectManager.GetService(service);
                object returnValue = null;
                try
                {
                    returnValue = DynamicCalls.GetMethodInvoker(method).Invoke(service, parms);

                    //把返回值传递回去
                    int index = 0;
                    foreach (var p in pis)
                    {
                        if (p.ParameterType.IsByRef)
                        {
                            //给参数赋值
                            resMsg.Parameters[p.Name] = parms[index];
                        }
                        index++;
                    }
                }
                catch (Exception ex)
                {
                    resMsg.Exception = GetNewException(ex);
                }

                if (returnValue != null)
                {
                    //判断是否压缩
                    if (resMsg.Compress)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            formatter.Serialize(ms, returnValue);
                            resMsg.Data = ms.ToArray();
                        }
                    }
                    else
                    {
                        //处理不压缩的系列化
                        resMsg.Data = SerializationManager.SerializeBin(returnValue);
                    }

                    //判断是否加密
                    if (resMsg.Encrypt)
                    {
                        //长度能被16除，表示为有效的key
                        if (msg.KeyLength != 0 && msg.KeyLength % 16 == 0)
                        {
                            resMsg.KeyLength = msg.KeyLength;
                        }
                        else
                        {
                            resMsg.KeyLength = 128;
                        }

                        //获取加密的字符串
                        var encrypt = BigInteger.GenerateRandom(resMsg.KeyLength).ToString();
                        resMsg.Keys = MD5.Hash(Encoding.UTF8.GetBytes(encrypt));

                        //这里暂时不处理
                        resMsg.Data = XXTEA.Encrypt(resMsg.Data, resMsg.Keys);
                    }
                }
            }
            catch (Exception ex)
            {
                //捕获全局错误
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
        /// 获取新的Exception
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
