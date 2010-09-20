using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 网站用户
    /// </summary>
    [Serializable]
    public class WebUser : User
    {
        private string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        /// <summary>
        /// 实例化网站用户
        /// </summary>
        /// <param name="userID"></param>
        public WebUser(string userID)
            : base(userID)
        { }
    }

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

        [NonSerialized]
        private IList<Message> _Messages;
        /// <summary>
        /// 保存此用户未读的消息
        /// </summary>
        public IList<Message> Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
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

        private UserExtend _Extend;
        /// <summary>
        /// 用户扩展信息
        /// </summary>
        public UserExtend Extend
        {
            get
            {
                return _Extend;
            }
            set
            {
                _Extend = value;
            }
        }

        public User()
        {
            this._State = OnlineState.Offline;
            this._UserType = UserType.TempUser;
            this._IsVIP = false;
            this._Messages = new List<Message>();
        }

        public User(string userID)
        {
            this._UserID = userID;
            this._State = OnlineState.Offline;
            this._UserType = UserType.TempUser;
            this._IsVIP = false;
            this._Messages = new List<Message>();
        }
    }
}
