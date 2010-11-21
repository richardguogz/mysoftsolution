using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 消息对象
    /// </summary>
    [Serializable]
    public class GPMessage : Message
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
    }
}
