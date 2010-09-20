using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using System.Collections;
using System.Reflection;

namespace LiveChat.Service
{
    /// <summary>
    /// 对P2CSession进行排序
    /// </summary>
    public class SortVipP2CSession : IComparer<P2CSession>
    {
        /// <summary>
        /// Sort by Vip and StartTime
        /// </summary>
        /// <param name="session1"></param>
        /// <param name="session2"></param>
        /// <returns></returns>
        public int Compare(P2CSession session1, P2CSession session2)
        {
            if (session1 != null && session2 != null)
            {
                if (session1.User.IsVIP == session2.User.IsVIP)
                {
                    return DateTime.Compare(session2.StartTime, session1.StartTime);
                }
                else
                {
                    return session2.User.IsVIP.CompareTo(session1.User.IsVIP);
                }
            }

            return -1;
        }
    }

    /// <summary>
    /// 对P2CSession进行排序
    /// </summary>
    public class SortTimeP2CSession : IComparer<P2CSession>
    {
        /// <summary>
        /// Sort by StartTime
        /// </summary>
        /// <param name="session1"></param>
        /// <param name="session2"></param>
        /// <returns></returns>
        public int Compare(P2CSession session1, P2CSession session2)
        {
            if (session1 != null && session2 != null)
            {
                return DateTime.Compare(session2.StartTime, session1.StartTime);
            }

            return -1;
        }
    }
}

