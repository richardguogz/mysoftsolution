using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 接收人信息
    /// </summary>
    public interface IReceiver
    {
        /// <summary>
        /// 接收者ID
        /// </summary>
        string ReceiverID { get; set; }

        /// <summary>
        /// 接收者名称
        /// </summary>
        string ReceiverName { get; set; }
    }

    /// <summary>
    /// 消息对象
    /// </summary>
    [Serializable]
    public abstract class Message
    {
        private Guid _ID;
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private Guid _SID;
        /// <summary>
        /// 会话唯一标识
        /// </summary>
        public Guid SID
        {
            get
            {
                return _SID;
            }
            set
            {
                _SID = value;
            }
        }

        private string _SessionID;
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionID
        {
            get
            {
                return _SessionID;
            }
            set
            {
                _SessionID = value;
            }
        }

        private string _SenderID;
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderID
        {
            get
            {
                return _SenderID;
            }
            set
            {
                _SenderID = value;
            }
        }

        private string _SenderName;
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName
        {
            get
            {
                return _SenderName;
            }
            set
            {
                _SenderName = value;
            }
        }

        private string _SenderIP;
        /// <summary>
        /// 发送者IP
        /// </summary>
        public string SenderIP
        {
            get
            {
                return _SenderIP;
            }
            set
            {
                _SenderIP = value;
            }
        }

        private DateTime _SendTime;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get
            {
                return _SendTime;
            }
            set
            {
                _SendTime = value;
            }
        }

        private string _Content;
        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }

        private MessageType _Type;
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        protected MessageState _State;
        /// <summary>
        /// 消息状态
        /// </summary>
        public MessageState State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }

        public Message()
        {
            this._ID = Guid.NewGuid();
            this._SendTime = DateTime.Now;
            this._Type = MessageType.Text;
            this._State = MessageState.Normal;
        }
    }
}