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

            //ʵ�ֻ�ϵ�л���
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

            #region �ж����ݸ�ʽ

            if (container.Proxy != null)
            {
                //���ݴ�����ѹ����ʽ
                reqMsg.Encrypt = container.Proxy.Encrypt;

                //����ѹ����ʽ
                reqMsg.Compress = container.Proxy.Compress;

                //���ó�ʱʱ��
                reqMsg.Timeout = container.Proxy.Timeout;
            }

            //��ȡԼ����Ϣ
            var contract = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceInterfaceType);

            //�ж�Լ��
            if (contract != null && contract.Timeout > 0)
            {
                //���ó�ʱʱ��
                reqMsg.Timeout = contract.Timeout;
            }

            #endregion

            ParameterInfo[] pis = methodInfo.GetParameters();
            if ((pis.Length == 0 && paramValues != null && paramValues.Length > 0) || (paramValues != null && pis.Length != paramValues.Length))
            {
                //��������ȷֱ�ӷ����쳣
                throw new IoCException(string.Format("Invalid parameters ({0},{1}). ==> {2}", reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.Parameters));
            }

            if (pis.Length > 0)
            {
                for (int i = 0; i < paramValues.Length; i++)
                {
                    if (paramValues[i] != null)
                    {
                        //������ݵ������ã�������
                        if (!pis[i].ParameterType.IsByRef)
                        {
                            reqMsg.Parameters[pis[i].Name] = paramValues[i];
                        }
                    }
                }
            }

            //���÷���
            ResponseMessage resMsg = container.CallService(serviceInterfaceType, reqMsg);

            //�������Ϊnull,�򷵻�null
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
                    //��������ֵ
                    paramValues[index] = resMsg.Parameters[p.Name];
                }
                index++;
            }

            //��������
            #region �����ص�����

            //�����Ƿ����
            if (resMsg.Encrypt)
            {
                //������ʱ������
                resMsg.Data = XXTEA.Decrypt(resMsg.Data, resMsg.Keys);
            }

            //���巵�ص�ֵ
            object returnValue = null;

            //�����Ƿ�ѹ��
            if (resMsg.Compress)
            {
                using (MemoryStream ms = new MemoryStream(resMsg.Data))
                {
                    returnValue = formatter.Deserialize(ms);
                }
            }
            else
            {
                //����ѹ���ķ�ϵ�л�
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

        #region IInvocationHandler ��Ա

        /// <summary>
        /// ��Ӧί��
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
