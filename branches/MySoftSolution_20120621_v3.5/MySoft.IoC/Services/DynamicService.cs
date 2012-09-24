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
            #region 获取相应的方法

            //判断方法是否存在
            if (!methods.ContainsKey(reqMsg.MethodName))
            {
                string message = string.Format("The server【{2}({3})】not find matching method. ({0},{1})."
                    , reqMsg.ServiceName, reqMsg.MethodName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                throw new WarningException(message);
            }

            #endregion

            //容器实例对象
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
                var callMethod = methods[reqMsg.MethodName];

                if (reqMsg.InvokeMethod)
                {
                    var objValue = reqMsg.Parameters["InvokeParameter"];

                    if (objValue != null)
                    {
                        //解析参数
                        reqMsg.Parameters = IoCHelper.CreateParameters(callMethod, objValue.ToString());
                    }
                }

                //解析服务
                instance = container.Resolve(serviceType);

                //返回拦截服务
                var service = AspectFactory.CreateProxy(serviceType, instance);

                //参数赋值
                object[] parameters = IoCHelper.CreateParameters(callMethod, reqMsg.Parameters);

                //调用对应的服务
                resMsg.Value = callMethod.FastInvoke(service, parameters);

                //处理返回参数
                IoCHelper.SetRefParameters(callMethod, parameters, resMsg.Parameters);
            }
            catch (Exception ex)
            {
                //捕获全局错误
                resMsg.Error = ex;
            }
            finally
            {
                //释放资源
                container.Release(instance);
            }

            return resMsg;
        }
    }
}
