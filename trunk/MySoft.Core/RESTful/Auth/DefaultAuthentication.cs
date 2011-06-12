using System;
using System.Net;

namespace MySoft.RESTful
{
    /// <summary>
    /// 默认的认证
    /// </summary>
    public abstract class DefaultAuthentication : IAuthentication
    {
        /// <summary>
        /// 认证接口
        /// </summary>
        /// <returns></returns>
        public AuthenticationResult Authorize()
        {
            var token = AuthenticationContext.Current.Token;
            var result = new AuthenticationResult
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Status = HttpStatusCode.Unauthorized,
                Message = "认证失败！"
            };

            try
            {
                if (AuthorizeForOAuth(token) && AuthorizeForCookie(token))
                {
                    result = new AuthenticationResult
                    {
                        Status = HttpStatusCode.OK,
                        Message = "认证成功！",
                        Result = AuthenticationContext.Current.User
                    };
                }
            }
            catch (AuthenticationException ex)
            {
                result.Code = ex.Code;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.Code = (int)result.Status;
                result.Message = ErrorHelper.GetInnerException(ex).Message;
            }

            return result;
        }

        #region IAuthentication 成员

        /// <summary>
        /// OAuth认证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual bool AuthorizeForOAuth(AuthenticationToken token)
        {
            return true;
        }

        /// <summary>
        /// Cookie认证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual bool AuthorizeForCookie(AuthenticationToken token)
        {
            return true;
        }

        #endregion
    }
}
