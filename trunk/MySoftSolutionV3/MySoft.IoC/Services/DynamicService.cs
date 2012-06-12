using System;
using System.Collections;
using MySoft.IoC.Aspect;
using MySoft.IoC.Messages;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
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
            var resMsg = new ResponseMessage
            {
                TransactionId = reqMsg.TransactionId,
                ReturnType = reqMsg.ReturnType,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName
            };

            #region ��ȡ��Ӧ�ķ���

            var methodKey = string.Format("{0}${1}", reqMsg.ServiceName, reqMsg.MethodName);
            if (!hashtable.ContainsKey(methodKey))
            {
                var m = CoreHelper.GetMethodFromType(serviceType, reqMsg.MethodName);
                if (m == null)
                {
                    string message = string.Format("The server��{2}({3})��not find matching method. ({0},{1})."
                        , reqMsg.ServiceName, reqMsg.MethodName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                    resMsg.Error = new WarningException(message);
                    return resMsg;
                }

                hashtable[methodKey] = m;
            }

            #endregion

            //����ʵ������
            object instance = null;

            try
            {
                //����Method
                var method = hashtable[methodKey] as System.Reflection.MethodInfo;

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
                resMsg.Value = DynamicCalls.GetMethodInvoker(method).Invoke(service, parameters);

                //�����ز���
                IoCHelper.SetRefParameters(method, resMsg.Parameters, parameters);

                //���ؽ������
                if (reqMsg.InvokeMethod)
                {
                    resMsg.Value = new InvokeData
                    {
                        Value = SerializationManager.SerializeJson(resMsg.Value),
                        Count = resMsg.Count,
                        OutParameters = resMsg.Parameters.ToString()
                    };

                    //�����������
                    resMsg.Parameters.Clear();
                }
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

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            hashtable.Clear();
        }
    }
}
