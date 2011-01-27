using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Web.UI;
using MySoft.Web;
using LiveChat.Interface;
using MySoft;

namespace LiveChat.Web.Admin
{
    public class ParentPage : AjaxPage, IAjaxInitHandler
    {
        protected static IUserService service;
        protected bool isPageLoad = false;
        public virtual void OnAjaxInit()
        {
            if (service == null)
                service = RemotingUtil.GetRemotingUserService();
        }

        protected override void OnInit(EventArgs e)
        {
            isPageLoad = true;
            OnAjaxInit();
            base.OnInit(e);
        }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SaveSession(string key, object value)
        {
            WebHelper.SaveSession(key, value);
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetSession<T>(string key)
        {
            return WebHelper.GetSession<T>(key);
        }
    }
}
