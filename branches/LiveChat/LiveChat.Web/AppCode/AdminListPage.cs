using System;
using System.Collections.Generic;
using System.Web;

namespace LiveChat.Web
{
    /// <summary>
    /// 列表页面
    /// </summary>
    public class AdminListPage : AdminPage
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
