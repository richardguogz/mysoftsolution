using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户列表
    /// </summary>
    [Serializable]
    public class UserList : List<User>
    {
        public bool Exists(User user)
        {
            return this.Exists(delegate(User u)
            {
                if (u.UserID == user.UserID) return true;
                return false;
            });
        }
    }

    /// <summary>
    /// 客服列表
    /// </summary>
    [Serializable]
    public class SeatList : List<Seat>
    {
        public bool Exists(Seat seat)
        {
            return this.Exists(delegate(Seat s)
            {
                if (s.SeatID == seat.SeatID) return true;
                return false;
            });
        }
    }

    /// <summary>
    /// 会话列表
    /// </summary>
    [Serializable]
    public class SessionList : List<Session>
    {
        public bool Exists(Session session)
        {
            return this.Exists(delegate(Session s)
            {
                if (s.SessionID == session.SessionID) return true;
                return false;
            });
        }
    }
}
