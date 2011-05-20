﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.Mail
{
    /// <summary>
    /// 邮件发送结果
    /// </summary>
    [Serializable]
    public class SendResult : ResponseResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        public SendResult()
            : base()
        {
            this.Success = true;
        }
    }
}
