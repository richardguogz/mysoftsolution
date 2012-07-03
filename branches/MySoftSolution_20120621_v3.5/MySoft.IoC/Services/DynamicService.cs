using System;
using System.Collections;
using MySoft.IoC.Aspect;
using MySoft.IoC.Messages;
using System.Collections.Generic;
using System.Linq;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        private IServiceContainer container;
        private Type serviceType;
        private IDictionary<string, System.Reflection.MethodInfo> methods;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceType)
            : base(container, serviceType)
        {
            this.container = container;
            this.serviceType = serviceType;
            this.methods = CoreHelper.GetMethodsFromType(serviceType).ToDictionary(p => p.ToString());
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            var resMsg = IoCHelper.GetResponse(reqMsg);

            #region ��ȡ��Ӧ�ķ���

            //�жϷ����Ƿ����
            if (!methods.ContainsKey(reqMsg.MethodName))
            {
                string message = string.Format("The server��{2}({3})��not find matching method. ({0},{1})."
                    , reqMsg.ServiceName, reqMsg.MethodName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                resMsg.Error = new WarningException(message);
                return resMsg;
            }

            #endregion

            //����ʵ������
            object instance = null;

            try
            {
                //����Method
                var method = methods[reqMsg.MethodName];

                //��������
                instance = container.Resolve(serviceType);

                //�������ط���
                var service = AspectFactory.CreateProxyService(serviceType, instance);

                if (reqMsg.InvokeMethod)
                {
                    var objValue = reqMsg.Parameters["InvokeParameter"];
                    var jsonString = (objValue == null ? string.Empty : objValue.ToString());

                    //��������
                    reqMsg.Parameters = IoCHelper.CreateParameters(method, jsonString);
                }

                //������ֵ
                object[] parameters = IoCHelper.CreateParameterValues(method, reqMsg.Parameters);

                //���ö�Ӧ�ķ���
                var value = DynamicCalls.GetMethodInvoker(method).Invoke(service, parameters);

                //�����ز���
                IoCHelper.SetRefParameters(method, resMsg.Parameters, parameters);

                //���ؽ������
                if (reqMsg.InvokeMethod)
                {
                    resMsg.Value = new InvokeData
                    {
                        Value = SerializationManager.SerializeJson(value),
                        Count = resMsg.Count,
                        OutParameters = resMsg.Parameters.ToString()
                    };

                    //�����������
                    resMsg.Parameters.Clear();
                }
                else
                {
                    resMsg.Value = value;
                }
            }
            catch (Exception ex)
            {
                resMsg.Value = null;

                //����ȫ�ִ���
                resMsg.Error = ex;
            }
            finally
            {
                //�ͷ���Դ
                container.Release(instance);
            }

            return resMsg;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            methods.Clear();
        }
    }
}
