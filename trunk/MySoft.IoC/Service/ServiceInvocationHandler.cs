using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using System.IO;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IInvocationHandler
    {
        private IServiceContainer container;
        private Type serviceInterfaceType;
        private PHPFormatter formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceInterfaceImpl"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public ServiceInvocationHandler(IServiceContainer container, Type serviceInterfaceType)
        {
            this.container = container;
            this.serviceInterfaceType = serviceInterfaceType;

            //实现化系列化器
            this.formatter = new PHPFormatter(Encoding.UTF8, AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="methodInfo">Name of the sub service.</param>
        /// <param name="paramValues">The param values.</param>
        /// <returns>The result.</returns>
        private object CallService(MethodInfo methodInfo, params object[] paramValues)
        {
            RequestMessage reqMsg = new RequestMessage();
            reqMsg.ServiceName = serviceInterfaceType.FullName;
            reqMsg.SubServiceName = methodInfo.ToString();
            reqMsg.TransactionId = Guid.NewGuid();

            #region 判断数据格式

            if (container.Proxy != null)
            {
                //传递传输与压缩格式
                reqMsg.Encrypt = container.Proxy.Encrypt;

                //设置压缩格式
                reqMsg.Compress = container.Proxy.Compress;

                //设置超时时间
                reqMsg.Timeout = container.Proxy.Timeout;
            }

            //获取约束信息
            var contract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);

            //判断约束
            if (contract != null && contract.Timeout > 0)
            {
                //设置超时时间
                reqMsg.Timeout = contract.Timeout;
            }

            #endregion

            ParameterInfo[] pis = methodInfo.GetParameters();
            if ((pis.Length == 0 && paramValues != null && paramValues.Length > 0) || (paramValues != null && pis.Length != paramValues.Length))
            {
                //参数不正确直接返回异常
                throw new IoCException(string.Format("Invalid parameters ({0},{1}). ==> {2}", reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.Parameters));
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    if (paramValues[i] != null)
                    {
                        //如果传递的是引用，则跳过
                        if (!pis[i].ParameterType.IsByRef)
                        {
                            reqMsg.Parameters[pis[i].Name] = paramValues[i];
                        }
                    }
                }
            }

            //调用服务
            ResponseMessage resMsg = container.CallService(serviceInterfaceType, reqMsg);

            //如果数据为null,则返回null
            if (resMsg == null || resMsg.Data == null)
            {
                return this.GetType().GetMethod("DefaultValue", BindingFlags.Instance | BindingFlags.NonPublic)
                            .MakeGenericMethod(methodInfo.ReturnType).Invoke(this, null);
            }

            int index = 0;
            foreach (var p in pis)
            {
                if (p.ParameterType.IsByRef)
                {
                    //给参数赋值
                    paramValues[index] = resMsg.Parameters[p.Name];
                }
                index++;
            }

            //处理数据
            #region 处理返回的数据

            //处理是否解密
            if (resMsg.Encrypt)
            {
                //这里暂时不处理
                resMsg.Data = XXTEA.Decrypt(resMsg.Data, resMsg.Keys);
            }

            //定义返回的值
            object returnValue = null;

            //处理是否压缩
            if (resMsg.Compress)
            {
                using (MemoryStream ms = new MemoryStream(resMsg.Data))
                {
                    returnValue = formatter.Deserialize(ms);
                }
            }
            else
            {
                //处理不压缩的反系列化
                returnValue = SerializationManager.DeserializeBin(resMsg.Data);
            }

            return returnValue;

            #endregion
        }

        /// <summary>
        /// Defaults the value.
        /// </summary>
        /// <returns></returns>
        protected object DefaultValue<MemberType>()
        {
            return default(MemberType);
        }

        #region IInvocationHandler 成员

        /// <summary>
        /// 响应委托
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(object proxy, MethodInfo method, object[] args)
        {
            return this.CallService(method, args);
        }

        #endregion
    }
}
