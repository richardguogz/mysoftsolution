using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 公共Session
    /// </summary>
    [Serializable]
    public abstract class Session
    {
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

        /// <summary>
        /// 会话ID
        /// </summary>
        public abstract string SessionID
        {
            get;
        }

        protected string _CreateID;
        /// <summary>
        /// 会话创建者ID
        /// </summary>
        public string CreateID
        {
            get
            {
                return _CreateID;
            }
            set
            {
                _CreateID = value;
            }
        }

        private SessionState _State;
        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionState State
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

        private int _MessageCount;
        /// <summary>
        /// 总消息数
        /// </summary>
        public int MessageCount
        {
            get
            {
                return _MessageCount;
            }
        }

        private int _NoReadMessageCount;
        /// <summary>
        /// 未读消息条数
        /// </summary>
        public int NoReadMessageCount
        {
            get
            {
                return _NoReadMessageCount;
            }
            set
            {
                _NoReadMessageCount = value;
            }
        }

        [NonSerialized]
        private IList<Message> _Messages;
        /// <summary>
        /// 消息列表
        /// </summary>
        public IList<Message> Messages
        {
            get
            {
                return _Messages;
            }
        }

        #region 消息相关信息

        private DateTime _LastReceiveTime;
        /// <summary>
        /// 最后接收消息时间
        /// </summary>
        public DateTime LastReceiveTime
        {
            get
            {
                return _LastReceiveTime;
            }
            set
            {
                _LastReceiveTime = value;
            }
        }

        #endregion

        /// <summary>
        /// 添加消息
        /// </summary>
        public virtual void AddMessage(Message msg)
        {
            msg.SID = this.SID;
            msg.SessionID = this.SessionID;
            this._Messages.Add(msg);
            this._MessageCount = this._Messages.Count;
            this._LastReceiveTime = msg.SendTime;
        }

        public Session()
        {
            this._SID = Guid.NewGuid();
            this._State = SessionState.WaitAccept;
            this._Messages = new List<Message>();
        }
    }
}
