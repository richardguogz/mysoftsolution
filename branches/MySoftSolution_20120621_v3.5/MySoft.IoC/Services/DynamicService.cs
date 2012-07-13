using System;
using System.Collections.Generic;
using System.Linq;
using MySoft.IoC.Aspect;
using MySoft.IoC.Messages;
using System.Threading;

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
            #region ��ȡ��Ӧ�ķ���

            //�жϷ����Ƿ����
            if (!methods.ContainsKey(reqMsg.MethodName))
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

                try
                {
                    //���ö�Ӧ�ķ���
                    resMsg.Value = DynamicCalls.GetMethodInvoker(method).Invoke(service, parameters);

                    //�����ز���
                    IoCHelper.SetRefParameters(method, resMsg.Parameters, parameters);
                }
                finally
                {
                    parameters = null;
                    service = null;
                }
            }
            catch (Exception ex)
            {
                resMsg.Value = null;

                //����ȫ�ִ���
                resMsg.Error = ex;

                if (ex is ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
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
