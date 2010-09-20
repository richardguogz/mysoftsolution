using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Utils
{
    /// <summary>
    /// 客服返回值
    /// </summary>
    public enum IMResult
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Successful,
        /// <summary>
        /// 无效的用户名
        /// </summary>
        InvalidUser,
        /// <summary>
        /// 无效的密码
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// 不是客服经理
        /// </summary>
        NotManager
    }
}
