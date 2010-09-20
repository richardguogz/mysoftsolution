using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 消息状态
    /// </summary>
    [Serializable]
    public enum MessageState
    {
        /// <summary>
        /// 正常的
        /// </summary>
        Normal,
        /// <summary>
        /// 异常的
        /// </summary>
        Abnormal
    }
}
