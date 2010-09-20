using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// P2S会话
    /// </summary>
    [Serializable]
    public class P2SSession : Session
    {
        protected User _User;
        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
            }
        }

        private Seat _Seat;
        /// <summary>
        /// 客服
        /// </summary>
        public Seat Seat
        {
            get
            {
                return _Seat;
            }
            set
            {
                _Seat = value;
            }
        }

        private string _FromIP;
        /// <summary>
        /// 请求者IP
        /// </summary>
        public string FromIP
        {
            get
            {
                return _FromIP;
            }
            set
            {
                _FromIP = value;
            }
        }

        private string _FromAddress;
        /// <summary>
        /// 请求者地址
        /// </summary>
        public string FromAddress
        {
            get
            {
                return _FromAddress;
            }
            set
            {
                _FromAddress = value;
            }
        }

        private string _RequestCode;
        /// <summary>
        /// 客服ID
        /// </summary>
        public string RequestCode
        {
            get
            {
                return _RequestCode;
            }
            set
            {
                _RequestCode = value;
            }
        }

        private string _RequestMessage;
        /// <summary>
        /// 请求的消息
        /// </summary>
        public string RequestMessage
        {
            get
            {
                return _RequestMessage;
            }
            set
            {
                _RequestMessage = value;
            }
        }

        private DateTime? _AcceptTime;
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime? AcceptTime
        {
            get
            {
                return _AcceptTime;
            }
            set
            {
                _AcceptTime = value;
            }
        }

        private DateTime _StartTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
            }
        }

        private DateTime? _EndTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
            }
        }

        public override string SessionID
        {
            get
            {
                return string.Format("S_{0}_{1}", _User.UserID, _Seat.SeatID);
            }
        }

        public override void AddMessage(Message msg)
        {
            if (string.IsNullOrEmpty(this._RequestMessage))
            {
                this._RequestMessage = msg.Content;
            }
            base.AddMessage(msg);
        }

        protected P2SSession(User user)
            : this()
        {
            this._CreateID = user.UserID;
            this._User = user;
        }

        public P2SSession()
            : base()
        {
            this._StartTime = DateTime.Now;
        }
    }
}
