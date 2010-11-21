using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using LiveChat.Entity;
using System.Collections.Generic;

namespace LiveChat.Web
{
    public partial class History : ParentPage
    {
        protected string skinID;
        protected string userID;
        protected string title;
        protected IList<ClientMessage> msgs;
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果出错则直接退出
            if (string.IsNullOrEmpty(GetRequestParam<string>("sessionID", null)))
            {
                Response.Write("传入的参数不正确！");
                Response.End();
            }

            skinID = GetRequestParam<string>("skinID", null);
            string sessionID = GetRequestParam<string>("sessionID", null);

            //如果会话已经结束，则查询出错
            if (service.GetSession(sessionID) == null)
            {
                Response.Write("无法查询到本次会话的聊天记录！");
                Response.End();
            }

            P2SSession session = service.GetSession(sessionID) as P2SSession;

            title = string.Format("{0}<font color='red'>{1}</font>与<font color='red'>{2}</font>的聊天记录",
                session.StartTime.ToString("yyyy年MM月dd日HH点mm分"),
                session.User.ShowName, session.Seat.ShowName);


            IList<Message> list = service.GetP2SHistoryMessages(sessionID);
            msgs = new List<ClientMessage>();

            //将消息转换成客户端消息
            foreach (Message msg in list)
            {
                ClientMessage cm = new ClientMessage();
                cm.SenderID = msg.SenderID;
                cm.SenderName = msg.SenderName;
                cm.Type = msg.Type;
                cm.SendTime = msg.SendTime;
                cm.Content = Encode(msg.Content);
                msgs.Add(cm);
            }
        }
    }
}
