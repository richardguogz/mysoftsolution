using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;

namespace MySoft.Web.UI
{
    /// <summary>
    /// The MasterPage base class.
    /// </summary>
    public class MasterAjaxPage : MasterPage
    {
        #region 页面方法重写

        /// <summary>
        /// 是否启用Ajax回调功能
        /// </summary>
        protected virtual bool EnableAjaxCallback
        {
            get { return false; }
        }

        /// <summary>
        /// 是否启用Ajax模板处理功能
        /// </summary>
        protected virtual bool EnableAjaxTemplate
        {
            get { return false; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            AjaxInfo info = new AjaxInfo(this.Page);
            info.EnableAjaxCallback = EnableAjaxCallback;
            info.EnableAjaxTemplate = EnableAjaxTemplate;

            AjaxRequest request = new AjaxRequest(info);
            request.SendRequest();

            base.OnInit(e);
        }
    }
}
