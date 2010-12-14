using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 接受类型
    /// </summary>
    public enum AcceptType
    {
        /// <summary>
        /// 接受并添加
        /// </summary>
        AcceptAdd,
        /// <summary>
        /// 接受
        /// </summary>
        Accept,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }
}
