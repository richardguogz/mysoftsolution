using System;
using MySoft.IoC.Messages;
using MySoft.Logger;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The base class of services.
    /// </summary>
    [Serializable]
    public abstract class BaseService : IService
    {
        /// <summary>
        ///  The service logger
        /// </summary>
        private IServiceContainer container;

        /// <summary>
        /// The service name.
        /// </summary>
        protected string serviceName;

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public BaseService(IServiceContainer container, string serviceName)
        {
            this.container = container;
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected abstract ResponseMessage Run(RequestMessage reqMsg);

        #region IService Members

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">The MSG.</param>
        /// <returns>The msg.</returns>
        public ResponseMessage CallService(RequestMessage reqMsg)
        {
            //从缓存获取值
            string cacheKey = GetCacheKey(reqMsg);
            ResponseMessage resMsg = null;

            if (reqMsg.CacheTime > 0)
            {
                if (container.Cache == null)
                    resMsg = CacheHelper.Get<ResponseMessage>(cacheKey);
                else
                    resMsg = container.Cache.GetCache<ResponseMessage>(cacheKey);
            }

            //运行请求获得结果
            if (resMsg == null)
            {
                resMsg = Run(reqMsg);

                if (resMsg.IsError)
                {
                    //如果是业务异常，则不抛出错误
                    if (!resMsg.IsBusinessError)
                    {
                        var ex = resMsg.Error;
                        string body = string.Format("【{5}】Dynamic ({0}) service ({1},{2}) error. \r\nMessage ==> {4}\r\nParameters ==> {3}", reqMsg.Message, reqMsg.ServiceName, reqMsg.MethodName, reqMsg.Parameters, resMsg.Message, resMsg.TransactionId);
                        var exception = new IoCException(body, ex)
                        {
                            ApplicationName = reqMsg.AppName,
                            ServiceName = reqMsg.ServiceName,
                            ErrorHeader = string.Format("Application【{0}】occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                        };
                        container.WriteError(exception);
                    }
                }
                else if (reqMsg.CacheTime > 0) //判断是否需要缓存
                {
                    //加入缓存
                    if (container.Cache == null)
                        CacheHelper.Insert(cacheKey, resMsg, reqMsg.CacheTime);
                    else
                        container.Cache.AddCache(cacheKey, resMsg, reqMsg.CacheTime);
                }
            }
            else
            {
                resMsg.TransactionId = reqMsg.TransactionId;
            }

            return resMsg;
        }

        /// <summary>
        /// 获取缓存Key值
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private string GetCacheKey(RequestMessage reqMsg)
        {
            return string.Format("ServerCache_{0}_{1}_{2}", reqMsg.ServiceName, reqMsg.MethodName, reqMsg.Parameters);
        }

        #endregion
    }
}
