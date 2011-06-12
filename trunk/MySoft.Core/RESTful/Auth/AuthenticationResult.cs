using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MySoft.RESTful
{
    /// <summary>
    /// 验证的结果
    /// </summary>
    [Serializable]
    public class AuthenticationResult : ResponseResult<AuthenticationUser>
    {
        /// <summary>
        /// Http状态码
        /// </summary>
        public HttpStatusCode Status { get; set; }
    }
}
