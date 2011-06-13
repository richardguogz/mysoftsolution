﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.RESTful.Configuration;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace MySoft.RESTful
{
    /// <summary>
    /// 认证工厂
    /// </summary>
    public class AuthenticationUtils
    {
        /// <summary>
        /// 初始化上下文
        /// </summary>
        private static void InitializeContext()
        {
            var request = WebOperationContext.Current.IncomingRequest;

            //初始化AuthenticationContext
            AuthenticationToken authToken = new AuthenticationToken(request.UriTemplateMatch.RequestUri, request.UriTemplateMatch.QueryParameters, request.Method);
            AuthenticationContext.Current = new AuthenticationContext(authToken);

            if (HttpContext.Current != null)
            {
                AuthenticationContext.Current.Token.Cookies = HttpContext.Current.Request.Cookies;
            }
            else
            {
                string cookie = request.Headers[HttpRequestHeader.Cookie];
                SetCookie(cookie);
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        private static void SetCookie(string cookie)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                HttpCookieCollection collection = new HttpCookieCollection();
                string[] cookies = cookie.Split(';');
                HttpCookie cook = null;
                foreach (string e in cookies)
                {
                    if (!string.IsNullOrEmpty(e))
                    {
                        string[] values = e.Split(new char[] { '=' }, 2);
                        if (values.Length == 2)
                        {
                            cook = new HttpCookie(values[0], values[1]);
                        }
                        collection.Add(cook);
                    }
                }

                AuthenticationContext.Current.Token.Cookies = collection;
            }
        }

        /// <summary>
        /// 进行认证
        /// </summary>
        /// <returns></returns>
        public static RESTfulResult Authorize()
        {
            //初始化上下文
            InitializeContext();

            var response = WebOperationContext.Current.OutgoingResponse;

            //进行认证处理
            var result = new RESTfulResult
            {
                Code = (int)RESTfulCode.AUTH_FAULT_CODE,
                Message = "Authentication fault!"
            };
            response.StatusCode = HttpStatusCode.Unauthorized;

            try
            {
                var config = RESTfulConfiguration.GetConfig();
                if (config == null || config.Auths == null || config.Auths.Count == 0)
                {
                    result.Code = (int)RESTfulCode.AUTH_ERROR_CODE;
                    result.Message = "No any authentication!";
                    return result;
                }

                bool isAuth = false;

                //如果配置了服务
                foreach (Authentication auth in config.Auths)
                {
                    var type = Type.GetType(auth.Type);
                    if (type == null) continue;

                    var service = (IAuthentication)Activator.CreateInstance(type);
                    isAuth = service.Authorize();

                    if (isAuth)
                    {
                        //检测是否设置了Current.User
                        if (AuthenticationContext.Current.User == null)
                        {
                            result.Code = (int)RESTfulCode.AUTH_FAULT_CODE;
                            result.Message = "Not set authentication user!";
                        }
                        else
                        {
                            result.Code = (int)RESTfulCode.OK;
                            result.Message = "Authentication success!";
                        }

                        break;
                    }
                }
            }
            catch (AuthenticationException ex)
            {
                result.Code = ex.Code;
                result.Message = ex.Message;
                response.StatusCode = ex.StatusCode;
            }
            catch (Exception ex)
            {
                result.Code = (int)RESTfulCode.AUTH_ERROR_CODE;
                result.Message = ErrorHelper.GetInnerException(ex).Message;
            }

            return result;
        }
    }
}
