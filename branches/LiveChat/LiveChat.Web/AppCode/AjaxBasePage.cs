using System;
using System.Collections.Generic;
using System.Web;

namespace LiveChat.Web
{
    /// <summary>
    /// Ajax基页
    /// </summary>
    public class AjaxBasePage : LoginPage
    {
        protected override bool EnableAjaxCallback
        {
            get
            {
                return true;
            }
        }
    }
}
