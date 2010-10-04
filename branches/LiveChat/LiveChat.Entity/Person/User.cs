using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class User : BaseInfo
    {
        private string _UserID;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }

        private string _UserName;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        private bool _IsVIP;
        /// <summary>
        /// 是否是vip
        /// </summary>
        public bool IsVIP
        {
            get
            {
                return _IsVIP;
            }
            set
            {
                _IsVIP = value;
            }
        }

        private int? _ChatCount;
        /// <summary>
        /// 会话次数
        /// </summary>
        public int? ChatCount
        {
            get
            {
                return _ChatCount;
            }
            set
            {
                _ChatCount = value;
            }
        }

        private DateTime? _LastChatTime;
        /// <summary>
        /// 最后会话时间
        /// </summary>
        public DateTime? LastChatTime
        {
            get
            {
                return _LastChatTime;
            }
            set
            {
                _LastChatTime = value;
            }
        }

        /// <summary>
        /// 用户界面显示的用户名
        /// </summary>
        public override string ShowName
        {
            get
            {
                if (UserType == UserType.TempUser)
                {
                    return _UserName;
                }
                else
                {
                    if (string.IsNullOrEmpty(_UserName))
                    {
                        return _UserID;
                    }
                    return string.Format("{0}({1})", _UserName, _UserID);
                }
            }
        }

        private UserType _UserType;
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType UserType
        {
            get
            {
                return _UserType;
            }
            set
            {
                _UserType = value;
            }
        }

        public User()
        {
            this._State = OnlineState.Offline;
            this._UserType = UserType.TempUser;
            this._IsVIP = false;
        }

        public User(string userID)
        {
            this._UserID = userID;
            this._State = OnlineState.Offline;
            this._UserType = UserType.TempUser;
            this._IsVIP = false;
        }
    }
}
