using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// S2S会话
    /// </summary>
    [Serializable]
    public class S2SSession : Session
    {
        private Seat _Owner;
        /// <summary>
        /// 自己
        /// </summary>
        public Seat Owner
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

        private Seat _Friend;
        /// <summary>
        /// 朋友
        /// </summary>
        public Seat Friend
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
                return string.Format("S_{0}_{1}", _Owner.SeatID, _Friend.SeatID);
            }
        }

        public S2SSession(Seat seat1, Seat seat2)
            : this()
        {
            this._CreateID = seat1.SeatID;
            this._Owner = seat1;
            this._Friend = seat2;
        }

        public S2SSession()
            : base()
        { }
    }
}
