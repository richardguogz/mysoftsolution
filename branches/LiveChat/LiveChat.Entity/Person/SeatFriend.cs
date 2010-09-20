using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服朋友信息
    /// </summary>
    [Serializable]
    public class SeatFriend : Seat
    {
        private Seat _Owner;
        /// <summary>
        /// 拥有者
        /// </summary>
        public Seat Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        private string _CompanyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        public SeatFriend() : base(null, null) { }

        public SeatFriend(Seat owner)
        {
            this._Owner = owner;
        }
    }
}
