﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.HTTP;

namespace MySoft.IoC.Http
{
    /// <summary>
    /// http解析工厂类
    /// </summary>
    public class HTTPRequestHandlerFactory : IHTTPRequestHandlerFactory
    {
        private HttpServiceCaller caller;

        #region IHTTPRequestHandlerFactory 成员

        /// <summary>
        /// 初始化CastleServiceHandler
        /// </summary>
        /// <param name="caller"></param>
        public HTTPRequestHandlerFactory(HttpServiceCaller caller)
        {
            this.caller = caller;
        }

        /// <summary>
        /// 返回请求句柄
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IHTTPRequestHandler CreateRequestHandler(HTTPServerRequest request)
        {
            //不是HttpGET方式，直接返回
            if (request.Method == HTTPServerRequest.HTTP_GET)
                return new HttpServiceHandler(caller);
            else
                return null;
        }

        #endregion
    }
}
