using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服Session
    /// </summary>
    [Serializable]
    public abstract class BaseInfo
    {
        #region 公共信息

        /// <summary>
        /// 返回最后获取的时间
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public DateTime this[string sessionID]
        {
            get
            {
                if (!_dictLastTime.ContainsKey(sessionID))
                {
                    _dictLastTime.Add(sessionID, DateTime.MinValue);
                }

                return _dictLastTime[sessionID];
            }
            set
            {
                _dictLastTime[sessionID] = value;
            }
        }

        private DateTime? _LoginTime;
        /// <summary>
        /// 登入时间
        /// </summary>
        public DateTime? LoginTime
        {
            get
            {
                return _LoginTime;
            }
            set
            {
                _LoginTime = value;
            }
        }

        private DateTime? _LogoutTime;
        /// <summary>
        /// 登出时间
        /// </summary>
        public DateTime? LogoutTime
        {
            get
            {
                return _LogoutTime;
            }
            set
            {
                _LogoutTime = value;
            }
        }

        private int _LoginCount;
        /// <summary>
        /// 登入次数
        /// </summary>
        public int LoginCount
        {
            get
            {
                return _LoginCount;
            }
            set
            {
                _LoginCount = value;
            }
        }

        protected OnlineState _State;
        /// <summary>
        /// 在线状态
        /// </summary>
        public OnlineState State
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

        /// <summary>
        /// 显示名称
        /// </summary>
        public abstract string ShowName { get; }

        #endregion

        /// <summary>
        /// 最后获取会话的时间
        /// </summary>
        [NonSerialized]
        private Dictionary<string, DateTime> _dictLastTime;

        [NonSerialized]
        private SessionList _Sessions;
        /// <summary>
        /// 会话列表
        /// </summary>
        public SessionList Sessions
        {
            get
            {
                return _Sessions;
            }
        }

        private int _SessionCount;
        /// <summary>
        /// 用户或客服总会话数
        /// </summary>
        public int SessionCount
        {
            get
            {
                return _SessionCount;
            }
        }

        /// <summary>
        /// 初始化会话
        /// </summary>
        public BaseInfo()
        {
            this._Sessions = new SessionList();
            this._dictLastTime = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// 添加会话到会话列表
        /// </summary>
        /// <param name="session"></param>
        public void AddSession(Session session)
        {
            if (!this._Sessions.Exists(session))
            {
                this._Sessions.Add(session);
                this._SessionCount = this.Sessions.Count;
            }

            AfterEvent();
        }

        /// <summary>
        /// 将会话从当前列表中移除
        /// </summary>
        /// <param name="session"></param>
        public void RemoveSession(Session session)
        {
            if (this._Sessions.Exists(session))
            {
                this.Sessions.Remove(session);
                this._SessionCount = this.Sessions.Count;
            }

            AfterEvent();
        }

        /// <summary>
        /// 之前的事件
        /// </summary>
        protected virtual void BeforeEvent() { }

        /// <summary>
        /// 之后的事件
        /// </summary>
        protected virtual void AfterEvent() { }
    }
}
