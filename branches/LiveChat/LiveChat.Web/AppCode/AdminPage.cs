using System;
using System.Collections.Generic;
using System.Web;

namespace LiveChat.Web
{
    /// <summary>
    /// 超级管理员页面
    /// </summary>
    public class AdminPage : LoginPage
    {
        public override void OnAjaxInit()
        {
            base.OnAjaxInit();

            if (GetAdmin().SeatType == LiveChat.Entity.SeatType.Normal)
            {
                string url = "top.document.location = '/admin/login.aspx';";
                url = WrapScriptTag(url);
                Response.Write(url);
                Response.End();
            }
        }
    }
}
