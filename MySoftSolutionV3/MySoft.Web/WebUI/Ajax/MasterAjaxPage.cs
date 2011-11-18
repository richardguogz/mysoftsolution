using System;

namespace MySoft.Web.UI
{
    /// <summary>
    /// The MasterPage base class.
    /// </summary>
    public class MasterAjaxPage : MasterPage
    {
        #region ҳ�淽����д

        /// <summary>
        /// �Ƿ�����Ajax�ص�����
        /// </summary>
        protected virtual bool EnableAjaxCallback
        {
            get { return false; }
        }

        /// <summary>
        /// �Ƿ�����Ajaxģ�崦������
        /// </summary>
        protected virtual bool EnableAjaxTemplate
        {
            get { return false; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            AjaxRequestPage info = new AjaxRequestPage(this.Page);
            info.EnableAjaxCallback = EnableAjaxCallback;
            info.EnableAjaxTemplate = EnableAjaxTemplate;

            //ֻ������Ajax�Ŵ���
            if (info.EnableAjaxCallback)
            {
                AjaxRequest request = new AjaxRequest(info);
                request.SendRequest();
            }

            base.OnInit(e);
        }
    }
}