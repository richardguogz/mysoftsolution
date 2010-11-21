using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using LiveChat.Entity;

namespace LiveChat.Web
{
    /// <summary>
    /// 客户端消息
    /// </summary>
    [Serializable]
    public class ClientMessage
    {
        private string _SenderID;
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderID
        {
            get { return _SenderID; }
            set { _SenderID = value; }
        }

        private string _SenderName;
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName
        {
            get { return _SenderName; }
            set { _SenderName = value; }
        }

        private MessageType _Type;
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private DateTime _SendTime;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get { return _SendTime; }
            set { _SendTime = value; }
        }

        private string _Content;
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
    }
}
