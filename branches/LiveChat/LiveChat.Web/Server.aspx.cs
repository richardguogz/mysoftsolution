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
using MySoft.Web.UI;
using LiveChat.Entity;
using System.Collections.Generic;
using MySoft.Web;
using System.Text;

namespace LiveChat.Web
{
    public partial class Server : ValidatePage
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string CreateFile(string sessionID, string userID)
        {
            try
            {
                //打开聊天记录窗口
                var url = "/history.aspx";
                var query = "sessionID=" + sessionID;
                var path = Server.MapPath("history.aspx");
                var dir = System.IO.Path.GetDirectoryName(path);
                var fileName = string.Format("{0}.htm", userID);
                dir = System.IO.Path.Combine(dir, "history");
                var filePath = System.IO.Path.Combine(dir, fileName);

                if (StaticPageUtils.CreateLocalPage(url, query, filePath, "聊天记录", Encoding.UTF8, Encoding.UTF8))
                {
                    url = string.Format("/history/{0}", fileName);
                    return Server.UrlEncode(url);
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [AjaxMethod(true)]
        public string SendMessage(MessageType type, string sessionID, string seatCode, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    if (string.IsNullOrEmpty(seatCode))
                    {
                        seatCode = null;
                    }
                    sessionID = service.SendP2CMessage(type, GetUserID(), GetRequestParam<string>("CompanyID", null), seatCode, Request.UserHostAddress, text);
                }
                else
                {
                    service.SendP2SMessage(type, sessionID, Request.UserHostAddress, text);
                }

                return sessionID;
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 发送截图
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        [AjaxMethod(true)]
        public string[] SendImage(string base64String)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(base64String);
                return service.UploadImage(buffer, ".jpg");
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        [AjaxMethod(true)]
        public IList<ClientMessage> GetMessages(string sessionID)
        {
            try
            {
                if (service.GetSession(sessionID) == null)
                {
                    return null;
                }

                IList<ClientMessage> list = new List<ClientMessage>();
                if (!string.IsNullOrEmpty(sessionID))
                {
                    IList<Message> msgs = service.GetP2SMessages(sessionID);
                    foreach (Message msg in msgs)
                    {
                        ClientMessage cm = new ClientMessage();
                        cm.SenderID = msg.SenderID;
                        cm.SenderName = msg.SenderName;
                        cm.Type = msg.Type;
                        cm.SendTime = msg.SendTime;
                        cm.Content = Encode(msg.Content);
                        list.Add(cm);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <returns></returns>
        [AjaxMethod(true)]
        public string[] GetSessionInfo()
        {
            try
            {
                P2CSession p = service.GetP2CSession(GetUserID(), GetRequestParam<string>("CompanyID", null));
                Company company = service.GetCompany(GetRequestParam<string>("CompanyID", null));
                return p == null ? null : (p.Seat == null ? new string[] { p.SessionID } : new string[] { p.SessionID, company.CompanyName, p.Seat.ShowName, p.Seat.SeatCode, p.Seat.Telephone, p.Seat.MobileNumber, p.Seat.Email });
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 获取客服在线状态
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public bool GetSeatOnline()
        {
            try
            {
                return service.GetSeatOnline(GetRequestParam<string>("CompanyID", null));
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [AjaxMethod]
        public void EndRequest(string sessionID)
        {
            try
            {
                if (!string.IsNullOrEmpty(sessionID))
                {
                    //结束本次会话
                    service.CloseSession(sessionID);
                }

                service.Logout(GetUserID());
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message);
            }
        }

        /// <summary>
        /// 启用Ajax
        /// </summary>
        protected override bool EnableAjaxCallback
        {
            get
            {
                return true;
            }
        }
    }
}
