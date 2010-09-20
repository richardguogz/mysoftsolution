using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 公司信息类
    /// </summary>
    [Serializable]
    public class Company
    {
        private string _CompanyID;
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID
        {
            get
            {
                return _CompanyID;
            }
            set
            {
                _CompanyID = value;
            }
        }

        private string _CompanyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                _CompanyName = value;
            }
        }

        #region 增加字段

        private string _WebSite;
        /// <summary>
        /// 公司网址
        /// </summary>
        public string WebSite
        {
            get
            {
                return _WebSite;
            }
            set
            {
                _WebSite = value;
            }
        }

        private string _ChatWebSite;
        /// <summary>
        /// 会话网址
        /// </summary>
        public string ChatWebSite
        {
            get
            {
                return _ChatWebSite;
            }
            set
            {
                _ChatWebSite = value;
            }
        }

        private string _CompanyLogo;
        /// <summary>
        /// 公司Logo
        /// </summary>
        public string CompanyLogo
        {
            get
            {
                return _CompanyLogo;
            }
            set
            {
                _CompanyLogo = value;
            }
        }

        private bool _IsHeadquarters;
        /// <summary>
        /// 是否总部
        /// </summary>
        public bool IsHeadquarters
        {
            get
            {
                return _IsHeadquarters;
            }
            set
            {
                _IsHeadquarters = value;
            }
        }

        #endregion

        private int _SeatCount;
        /// <summary>
        /// 客服人数
        /// </summary>
        public int SeatCount
        {
            get
            {
                return _SeatCount;
            }
        }

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

        public Company()
        {
            this._Seats = new SeatList();
        }

        public Company(string companyID)
        {
            this._CompanyID = companyID;
            this._Seats = new SeatList();
        }

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <param name="seat"></param>
        public void AddSeat(Seat seat)
        {
            if (!this._Seats.Exists(seat))
            {
                this._Seats.Add(seat);
                this._SeatCount = this._Seats.Count;
            }
        }
    }
}
