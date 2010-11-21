using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 快捷回复
    /// </summary>
    [Serializable]
    public class Reply
    {
        private int _ReplyID;
        /// <summary>
        /// 回复ID
        /// </summary>
        public int ReplyID
        {
            get
            {
                return _ReplyID;
            }
            set
            {
                _ReplyID = value;
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

        private string _Content;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
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
