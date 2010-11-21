using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 群信息
    /// </summary>
    [Serializable]
    public abstract class Group
    {
        private Guid _GroupID;
        /// <summary>
        /// 群ID
        /// </summary>
        public Guid GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                _GroupID = value;
            }
        }

        private string _GroupName;
        /// <summary>
        /// 群名称
        /// </summary>
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
            }
        }

        private string _MemoName;
        /// <summary>
        /// 群备注名称
        /// </summary>
        public string MemoName
        {
            get
            {
                return _MemoName;
            }
            set
            {
                _MemoName = value;
            }
        }

        private DateTime _AddTime;
        /// <summary>
        /// 群创建时间
        /// </summary>
        public DateTime AddTime
        {
            get
            {
                return _AddTime;
            }
            set
            {
                _AddTime = value;
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

        private int _MaxPerson;
        /// <summary>
        /// 最高人数
        /// </summary>
        public int MaxPerson
        {
            get
            {
                return _MaxPerson;
            }
            set
            {
                _MaxPerson = value;
            }
        }

        protected int _PersonCount;
        /// <summary>
        /// 群人数
        /// </summary>
        public int PersonCount
        {
            get
            {
                return _PersonCount;
            }
        }

        private string _CreateID;
        /// <summary>
        /// 创建者ID
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

        private string _ManagerID;
        /// <summary>
        /// 管理者ID
        /// </summary>
        public string ManagerID
        {
            get
            {
                return _ManagerID;
            }
            set
            {
                _ManagerID = value;
            }
        }

        private string _Description;
        /// <summary>
        /// 群描述
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        private string _Notification;
        /// <summary>
        /// 群通知
        /// </summary>
        public string Notification
        {
            get
            {
                return _Notification;
            }
            set
            {
                _Notification = value;
            }
        }

        /// <summary>
        /// 返回最后获取的时间
        /// </summary>
        /// <param name="senderID"></param>
        /// <returns></returns>
        public DateTime this[string senderID]
        {
            get
            {
                if (!_dictLastTime.ContainsKey(senderID))
                {
                    _dictLastTime.Add(senderID, DateTime.MinValue);
                }
                return _dictLastTime[senderID];
            }
            set
            {
                _dictLastTime[senderID] = value;
            }
        }

        //最后接收时间
        private DateTime _LastReceiveTime;

        /// <summary>
        /// 最后获取会话的时间
        /// </summary>
        [NonSerialized]
        private Dictionary<string, DateTime> _dictLastTime;

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

        /// <summary>
        /// 添加消息
        /// </summary>
        public virtual void AddMessage(Message msg)
        {
            this._Messages.Add(msg);
            this._MessageCount = this._Messages.Count;
            this._LastReceiveTime = msg.SendTime;
        }

        /// <summary>
        /// 初始化群
        /// </summary>
        public Group(Guid groupID)
        {
            this._Messages = new List<Message>();
            this._LastReceiveTime = DateTime.Now;
            this._dictLastTime = new Dictionary<string, DateTime>();
            this._GroupID = groupID;
            this._AddTime = DateTime.Now;
            this._MaxPerson = 10;
        }
    }
}
