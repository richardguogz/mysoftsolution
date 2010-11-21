using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户类型
    /// </summary>
    [Serializable]
    public enum UserType
    {
        /// <summary>
        /// 临时用户(匿名)
        /// </summary>
        TempUser,
        /// <summary>
        /// 网站用户
        /// </summary>
        WebUser
    }
}
