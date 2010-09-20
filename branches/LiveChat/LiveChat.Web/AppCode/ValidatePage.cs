using System;
using System.Collections.Generic;
using System.Web;
using LiveChat.Interface;
using LiveChat.Entity;
using MySoft.Web;
using MySoft.Web.UI;

namespace LiveChat.Web
{
    public class ValidatePage : ParentPage
    {
        public override void OnAjaxInit()
        {
            base.OnAjaxInit();

            Guid clientID = new Guid(Request["ClientID"]);
            string userid = GetUserID();
            if (string.IsNullOrEmpty(userid))
            {
                throw new AjaxException("当前会话已经过期，请重新联系客服！");
            }
            else
            {
                if (!service.ValidateClient(userid, clientID))
                {
                    throw new AjaxException("此用户已在其它地方登录！");
                }
            }
        }
    }
}
