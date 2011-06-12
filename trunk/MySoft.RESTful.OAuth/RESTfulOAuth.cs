using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.RESTful.OAuth
{
    /// <summary>
    /// 实现RESTful的OAuth认证
    /// </summary>
    public class RESTfulOAuth : DefaultAuthentication
    {
        /// <summary>
        /// 实现OAuth认证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override bool AuthorizeForOAuth(AuthenticationToken token)
        {
            if (!string.IsNullOrEmpty(token.Parameters["username"]))
            {
                AuthenticationContext.Current.User = new AuthenticationUser
                {
                    AuthName = token.Parameters["username"]
                };
            }

            return base.AuthorizeForOAuth(token);
        }
    }
}
