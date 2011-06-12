﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.RESTful
{
    /// <summary>
    /// 认证用户
    /// </summary>
    [Serializable]
    public class AuthenticationUser
    {
        /// <summary>
        /// 认证用户ID
        /// </summary>
        public int AuthID { get; set; }

        /// <summary>
        /// 认证用户名称
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 认证用户邮箱
        /// </summary>
        public string AuthEmail { get; set; }
    }
}
