using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服信息
    /// </summary>
    [Serializable]
    public class Seat : BaseInfo
    {
        private Guid _ClientID;
        /// <summary>
        /// 用于验证的客服端ID
        /// </summary>
        public Guid ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                _ClientID = value;
            }
        }

        private DateTime _RefreshTime;
        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime RefreshTime
        {
            get
            {
                return _RefreshTime;
            }
            set
            {
                _RefreshTime = value;
            }
        }

        private string _CompanyID;
        /// <summary>
        /// 公司信息
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

        private string _SeatCode;
        /// <summary>
        /// 客服Code
        /// </summary>
        public string SeatCode
        {
            get
            {
                return _SeatCode;
            }
            set
            {
                _SeatCode = value;
            }
        }

        /// <summary>
        /// 客服ID
        /// </summary>
        public string SeatID
        {
            get
            {
                return string.Format("{0}_{1}", _CompanyID, _SeatCode);
            }
        }

        private string _SeatName;
        /// <summary>
        /// 客服名称
        /// </summary>
        public string SeatName
        {
            get
            {
                return _SeatName;
            }
            set
            {
                _SeatName = value;
            }
        }

        #region 增加字段

        private string _Telephone;
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone
        {
            get
            {
                return _Telephone;
            }
            set
            {
                _Telephone = value;
            }
        }

        private string _MobileNumber;
        /// <summary>
        /// 手机
        /// </summary>
        public string MobileNumber
        {
            get
            {
                return _MobileNumber;
            }
            set
            {
                _MobileNumber = value;
            }
        }

        private string _Email;
        /// <summary>
        /// Email地址
        /// </summary>
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        private string _Sign;
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Sign
        {
            get
            {
                return _Sign;
            }
            set
            {
                _Sign = value;
            }
        }

        private string _Introduction;
        /// <summary>
        /// 系统导语
        /// </summary>
        public string Introduction
        {
            get
            {
                return _Introduction;
            }
            set
            {
                _Introduction = value;
            }
        }

        private byte[] _FaceImage;
        /// <summary>
        /// 图像信息
        /// </summary>
        public byte[] FaceImage
        {
            get
            {
                return _FaceImage;
            }
            set
            {
                _FaceImage = value;
            }
        }

        #endregion

        private int _UserSessionCount;
        /// <summary>
        /// 用户会话数
        /// </summary>
        public int UserSessionCount
        {
            get
            {
                return _UserSessionCount;
            }
            set
            {
                _UserSessionCount = value;
            }
        }

        /// <summary>
        /// 用户界面显示的客服名
        /// </summary>
        public override string ShowName
        {
            get
            {
                if (string.IsNullOrEmpty(_SeatName))
                {
                    return _SeatCode;
                }
                return string.Format("{0}({1})", _SeatName, _SeatCode);
            }
        }

        private SeatType _SeatType;
        /// <summary>
        /// 客服类型
        /// </summary>
        public SeatType SeatType
        {
            get
            {
                return _SeatType;
            }
            set
            {
                _SeatType = value;
            }
        }

        protected string _Password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        public Seat()
        {
            this._State = OnlineState.Offline;
            this._SeatType = SeatType.Normal;
        }

        /// <summary>
        /// 实例化客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        public Seat(string companyID, string seatCode)
        {
            this._CompanyID = companyID;
            this._SeatCode = seatCode;
            this._State = OnlineState.Offline;
            this._SeatType = SeatType.Normal;
        }

        /// <summary>
        /// 之后的事件
        /// </summary>
        protected override void AfterEvent()
        {
            this._UserSessionCount = this.Sessions.FindAll(p => p is P2SSession).Count;
        }
    }
}
