using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 在线状态
    /// </summary>
    [Serializable]
    public enum OnlineState
    {
        /// <summary>
        /// 在线
        /// </summary>
        Online,
        /// <summary>
        /// 忙碌
        /// </summary>
        Busy,
        /// <summary>
        /// 离开
        /// </summary>
        Leave,
        /// <summary>
        /// 离线
        /// </summary>
        Offline
    }
}
