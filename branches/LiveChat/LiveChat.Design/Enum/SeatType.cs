using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服类型
    /// </summary>
    [Serializable]
    public enum SeatType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        Normal,
        /// <summary>
        /// 管理员
        /// </summary>
        Manager,
        /// <summary>
        /// 超级用户
        /// </summary>
        Super
    }
}
