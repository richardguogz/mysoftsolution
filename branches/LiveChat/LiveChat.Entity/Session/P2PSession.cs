using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// P2P会话
    /// </summary>
    [Serializable]
    public class P2PSession : Session
    {
        private User _Owner;
        /// <summary>
        /// 自己
        /// </summary>
        public User Owner
        {
            get
            {
                return _Owner;
            }
            set
            {
                _Owner = value;
            }
        }

        private User _Friend;
        /// <summary>
        /// 朋友
        /// </summary>
        public User Friend
        {
            get
            {
                return _Friend;
            }
            set
            {
                _Friend = value;
            }
        }

        /// <summary>
        /// 返回SessionID
        /// </summary>
        public override string SessionID
        {
            get
            {
                return string.Format("S_{0}_{1}", _Owner.UserID, _Friend.UserID);
            }
        }

        public P2PSession(User user1, User user2)
            : this()
        {
            this._CreateID = user1.UserID;
            this._Owner = user1;
            this._Friend = user2;
        }

        public P2PSession()
            : base()
        { }
    }
}
