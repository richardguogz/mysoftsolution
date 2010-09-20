using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 客服会话
    /// </summary>
    [Serializable]
    public class SeatSession
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

        private string _SeatID;
        /// <summary>
        /// 客服ID
        /// </summary>
        public string SeatID
        {
            get
            {
                return _SeatID;
            }
            set
            {
                _SeatID = value;
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

        private IList<P2SSession> _Sessions;
        /// <summary>
        /// 会话列表
        /// </summary>
        public IList<P2SSession> Sessions
        {
            get
            {
                return _Sessions;
            }
            set
            {
                _Sessions = value;
            }
        }
    }
}
