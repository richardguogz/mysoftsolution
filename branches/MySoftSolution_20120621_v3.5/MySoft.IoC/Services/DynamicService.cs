using System;
using System.Collections.Generic;
using System.Linq;
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
        private IDictionary<string, System.Reflection.MethodInfo> methods;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceType)
            : base(serviceType)
        {
            this.container = container;
            this.serviceType = serviceType;

            //Get method
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

            var callMethod = methods[reqMsg.MethodName];

            //��������
            ResolveParameters(callMethod, reqMsg);

            //���÷���
            var resMsg = InvokeMethod(callMethod, reqMsg);

            //������Ϣ
            HandleMessage(reqMsg, resMsg);

            return resMsg;
        }

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="callMethod"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private ResponseMessage InvokeMethod(System.Reflection.MethodInfo callMethod, RequestMessage reqMsg)
        {
            var resMsg = new ResponseMessage
            {
                TransactionId = reqMsg.TransactionId,
                ReturnType = reqMsg.ReturnType,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName
            };

            //��������
            var instance = container.Resolve(serviceType);

            try
            {
                //�������ط���
                var service = AspectFactory.CreateProxy(serviceType, instance);

                //������ֵ
                object[] parameters = IoCHelper.CreateParameters(callMethod, reqMsg.Parameters);

                //���ö�Ӧ�ķ���
                resMsg.Value = callMethod.FastInvoke(service, parameters);

                //�����ز���
                IoCHelper.SetRefParameters(callMethod, parameters, resMsg.Parameters);
            }
            finally
            {
                //�ͷ���Դ
                container.Release(instance);
            }

            return resMsg;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="callMethod"></param>
        /// <param name="reqMsg"></param>
        private void ResolveParameters(System.Reflection.MethodInfo callMethod, RequestMessage reqMsg)
        {
            if (reqMsg.InvokeMethod)
            {
                var objValue = reqMsg.Parameters["InvokeParameter"];

                if (objValue != null)
                {
                    //��������
                    reqMsg.Parameters = IoCHelper.CreateParameters(callMethod, objValue.ToString());
                }
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="resMsg"></param>
        private void HandleMessage(RequestMessage reqMsg, ResponseMessage resMsg)
        {
            //���ؽ������
            if (reqMsg.InvokeMethod)
            {
                resMsg.Value = new InvokeData
                {
                    Value = SerializationManager.SerializeJson(resMsg.Value),
                    Count = resMsg.Count,
                    ElapsedTime = resMsg.ElapsedTime,
                    OutParameters = resMsg.Parameters.ToString()
                };

                //�����������
                resMsg.Parameters.Clear();
            }
        }
    }
}
