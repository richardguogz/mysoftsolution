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

    /// <summary>
    /// 群信息
    /// </summary>
    [Serializable]
    public class GroupInfo : RequestInfo
    {
        public Seat Seat { get; set; }
    }

    /// <summary>
    /// 客服信息
    /// </summary>
    [Serializable]
    public class SeatInfo : MessageInfo
    {
        private string _MemoName;
        /// <summary>
        /// 备注名称
        /// </summary>
        public string MemoName
        {
            get { return _MemoName; }
            set { _MemoName = value; }
        }
    }

    /// <summary>
    /// 消息信息
    /// </summary>
    [Serializable]
    public class MessageInfo
    {
        private int _Count;
        /// <summary>
        /// 消息数
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        private Message _Message;
        /// <summary>
        /// 最后消息
        /// </summary>
        public Message Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
}
