using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    public enum SessionState
    {
        /// <summary>
        /// 等待接受
        /// </summary>
        WaitAccept,
        /// <summary>
        /// 会话中
        /// </summary>
        Talking,
        /// <summary>
        /// 会话已结束
        /// </summary>
        Closed
    }
}
