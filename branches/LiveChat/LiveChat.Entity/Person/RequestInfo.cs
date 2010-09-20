using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 请求信息
    /// </summary>
    [Serializable]
    public class RequestInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 请求消息
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 拒绝消息
        /// </summary>
        public string Refuse { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public int ConfirmState { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
