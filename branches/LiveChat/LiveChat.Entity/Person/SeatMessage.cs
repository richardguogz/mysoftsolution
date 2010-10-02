using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服信息
    /// </summary>
    [Serializable]
    public class SeatMessage
    {
        /// <summary>
        /// 会话消息数
        /// </summary>
        public IDictionary<P2SSession, int> SessionMessages { get; set; }

        /// <summary>
        /// 客服消息数
        /// </summary>
        public IDictionary<Seat, MessageInfo> SeatMessages { get; set; }

        /// <summary>
        /// 群消息数
        /// </summary>
        public IDictionary<SeatGroup, int> GroupMessages { get; set; }

        /// <summary>
        /// 请求消息
        /// </summary>
        public IDictionary<Seat, RequestInfo> RequestMessages { get; set; }
    }
}
