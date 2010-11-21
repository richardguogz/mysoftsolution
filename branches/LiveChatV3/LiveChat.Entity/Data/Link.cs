using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 快捷链接
    /// </summary>
    [Serializable]
    public class Link
    {
        private int _LinkID;
        /// <summary>
        /// 回复ID
        /// </summary>
        public int LinkID
        {
            get
            {
                return _LinkID;
            }
            set
            {
                _LinkID = value;
            }
        }

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

        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        private string _Url;
        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        private DateTime _AddTime;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get
            {
                return _AddTime;
            }
            set
            {
                _AddTime = value;
            }
        }
    }
}
