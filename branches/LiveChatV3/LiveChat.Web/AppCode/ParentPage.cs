using System;
using System.Collections.Generic;
using System.Web;
using MySoft.Web.UI;
using LiveChat.Entity;
using MySoft.Web;
using LiveChat.Interface;
using MySoft;

namespace LiveChat.Web
{
    public class ParentPage : AjaxPage, IAjaxInitHandler
    {
        protected static IUserService service;
        public virtual void OnAjaxInit()
        {
            if (service == null)
                service = RemotingUtil.GetRemotingUserService();
        }

        protected override void OnInit(EventArgs e)
        {
            OnAjaxInit();
            base.OnInit(e);
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        protected string GetUserID()
        {
            return GetIDValue("userid");
        }

        /// <summary>
        /// 获取客服ID
        /// </summary>
        /// <returns></returns>
        protected string GetSeatID()
        {
            return GetIDValue("seatid");
        }

        /// <summary>
        /// 从Session和Cookie中取出ID值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetIDValue(string key)
        {
            string id = WebHelper.GetSession<string>(key);
            if (string.IsNullOrEmpty(id))
            {
                HttpCookie cookie = WebHelper.GetCookie(key);
                if (cookie != null)
                {
                    id = cookie.Value;
                    if (!string.IsNullOrEmpty(id))
                    {
                        WebHelper.SaveSession(key, id);
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// 保存ID的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        protected void SaveIDValue(string key, string id)
        {
            WebHelper.SaveSession(key, id);

            //保存一个小时
            WebHelper.SaveCookie(key, id, CookieExpiresType.Hour, 1);
        }

        /// <summary>
        /// 对内容进行编码处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string Encode(string content)
        {
            if (content == null) return string.Empty;
            //content = content.Replace(">", "&gt;");
            //content = content.Replace("<", "&lt;");
            //content = content.Replace(" ", "&nbsp;");
            //content = content.Replace("\"", "&quot;");
            //content = content.Replace("\'", "&#39;");
            content = content.Replace("\r\n", "<br/>");
            content = content.Replace("\r", "");
            content = content.Replace("\n", "<br/>");
            return content;
        }
    }
}
