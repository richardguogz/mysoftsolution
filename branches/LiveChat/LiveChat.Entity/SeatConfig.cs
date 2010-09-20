using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [Serializable]
    public class SeatConfig : Seat
    {
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

        /// <summary>
        /// 实例化配置
        /// </summary>
        public SeatConfig() : base(null, null) { }

        /// <summary>
        /// 实例化配置
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        public SeatConfig(string companyID, string seatCode)
            : base(companyID, seatCode)
        { }
    }
}
