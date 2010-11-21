using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    [Serializable]
    public class SeatFriendGroup
    {
        private Guid _GroupID;
        /// <summary>
        /// 组ID
        /// </summary>
        public Guid GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }

        private string _GroupName;
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
    }

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

        private Guid? _GroupID;
        /// <summary>
        /// 组ID
        /// </summary>
        public Guid? GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }

        private string _MemoName;
        /// <summary>
        /// 备注名称
        /// </summary>
        public string MemoName
        {
            get { return _MemoName; }
            set { _MemoName = value; }
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
