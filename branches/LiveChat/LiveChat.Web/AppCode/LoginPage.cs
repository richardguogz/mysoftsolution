using System;
using System.Collections.Generic;
using System.Web;
using MySoft.Web.UI;

namespace LiveChat.Web
{
    public class LoginPage : BasePage, IAjaxInitHandler
    {
        public override void OnAjaxInit()
        {
            base.OnAjaxInit();

            if (GetAdmin() == null)
            {
                string url = "top.document.location = '/admin/login.aspx';";
                url = WrapScriptTag(url);
                Response.Write(url);
                Response.End();
            }
        }
    }
}
