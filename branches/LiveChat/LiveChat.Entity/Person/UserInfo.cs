using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户相关信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        private int _CurrentSessionCount;
        /// <summary>
        /// 我当前会话个数
        /// </summary>
        public int CurrentSessionCount
        {
            get
            {
                return _CurrentSessionCount;
            }
            set
            {
                _CurrentSessionCount = value;
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
    }
}
