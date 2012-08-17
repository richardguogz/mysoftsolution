using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MySoft.IoC.Aspect;
using MySoft.IoC.Messages;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        private IServiceContainer container;
        private Type serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceType)
            : base(container, serviceType)
        {
            this.container = container;
            this.serviceType = serviceType;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage reqMsg)
        {
            //��ȡMethod
            var callMethod = CoreHelper.GetMethodFromType(serviceType, reqMsg.MethodName);

            #region ��ȡ��Ӧ�ķ���

            //�жϷ����Ƿ����
            if (callMethod == null)
            {
                string message = string.Format("The server��{2}({3})��not find matching method. ({0},{1})."
                    , reqMsg.ServiceName, reqMsg.MethodName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                throw new WarningException(message);
            }

            #endregion

            //����ʵ������
            object instance = null;

            var resMsg = new ResponseMessage
            {
                TransactionId = reqMsg.TransactionId,
                ReturnType = reqMsg.ReturnType,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName
            };

            try
            {
                //��������
                instance = container.Resolve(serviceType);

                //�������ط���
                var service = AspectFactory.CreateProxyService(serviceType, instance);

                if (reqMsg.InvokeMethod)
                {
                    var objValue = reqMsg.Parameters["InvokeParameter"];
                    var jsonString = (objValue == null ? string.Empty : objValue.ToString());

                    //��������
                    reqMsg.Parameters = IoCHelper.CreateParameters(callMethod, jsonString);
                }

                //������ֵ
                object[] parameters = IoCHelper.CreateParameterValues(callMethod, reqMsg.Parameters);

                //���ö�Ӧ�ķ���
                var invoke = DynamicCalls.GetMethodInvoker(callMethod);
                resMsg.Value = invoke(service, parameters);

                //�����ز���
                IoCHelper.SetRefParameters(callMethod, resMsg.Parameters, parameters);
            }
            catch (Exception ex)
            {
                //����ȫ�ִ���
                resMsg.Error = ex;
            }
            finally
            {
                //�ͷ���Դ
                container.Release(instance);

                instance = null;
            }

            return resMsg;
        }
    }
}
