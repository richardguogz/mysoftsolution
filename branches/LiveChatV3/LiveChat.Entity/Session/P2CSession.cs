using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// P2C会话
    /// </summary>
    [Serializable]
    public class P2CSession : P2SSession
    {
        #region 请求会话的客服信息

        private Company _Company;
        /// <summary>
        /// 公司
        /// </summary>
        public Company Company
        {
            get
            {
                return _Company;
            }
            set
            {
                _Company = value;
            }
        }

        #endregion

        public override string SessionID
        {
            get
            {
                if (Seat == null)
                {
                    return string.Format("S_{0}_{1}", User.UserID, _Company.CompanyID);
                }
                return base.SessionID;
            }
        }

        /// <summary>
        /// 临时会话
        /// </summary>
        /// <param name="user"></param>
        /// <param name="company"></param>
        public P2CSession(User user, Company company)
            : base(user)
        {
            this._CreateID = user.UserID;
            this._User = user;
            this._Company = company;
        }
    }
}
