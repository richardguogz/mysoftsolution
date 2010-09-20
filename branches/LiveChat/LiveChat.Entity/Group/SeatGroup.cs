using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服群
    /// </summary>
    [Serializable]
    public class SeatGroup : Group
    {
        [NonSerialized]
        private SeatList _Seats;
        /// <summary>
        /// 客服列表
        /// </summary>
        public SeatList Seats
        {
            get
            {
                return _Seats;
            }
            set
            {
                _Seats = value;
            }
        }

        public SeatGroup(Company company, Guid groupID)
            : base(company, groupID)
        {
            this._Seats = new SeatList();
        }

        /// <summary>
        /// 是否存在客服
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public bool Exists(Seat seat)
        {
            return this._Seats.Exists(seat);
        }

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <param name="seat"></param>
        public void AddSeat(Seat seat)
        {
            if (!this._Seats.Exists(seat))
            {
                this[seat.SeatID] = DateTime.MinValue;
                this._Seats.Add(seat);
                this._PersonCount = this._Seats.Count;
            }
        }

        /// <summary>
        /// 移除客服
        /// </summary>
        /// <param name="seat"></param>
        public void RemoveSeat(Seat seat)
        {
            if (this._Seats.Exists(seat))
            {
                this._Seats.Remove(seat);
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        public override void AddMessage(Message msg)
        {
            msg.SessionID = string.Format("S_{0}_{1}", this.Company.CompanyID, this.GroupID.ToString());
            base.AddMessage(msg);
        }
    }
}
