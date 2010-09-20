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

namespace LiveChat.Web
{
    public partial class Leave : ParentPage, IAjaxProcessHandler
    {
        protected override bool EnableAjaxCallback
        {
            get
            {
                return true;
            }
        }

        #region IAjaxProcessHandler 成员

        public void OnAjaxProcess(CallbackParams callbackParams)
        {
            if (GetRequestParam<string>("action") == "add")
            {
                if (callbackParams["textImage"].Value != Session["ValidateCode"].ToString())
                {
                    throw new AjaxException("验证码输入不正确！");
                }

                Entity.Leave leave = new Entity.Leave()
                {
                    Name = callbackParams["name"].Value,
                    Body = callbackParams["feed"].Value,
                    CompanyID = GetRequestParam<string>("CompanyID"),
                    Email = callbackParams["email"].Value,
                    PostIP = Request.UserHostAddress,
                    Telephone = callbackParams["contact"].Value,
                    Title = callbackParams["subject"].Value
                };

                int ret = service.AddLeave(leave.CompanyID, leave.Name, leave.Telephone, leave.Email, leave.Title, leave.Body, leave.PostIP);
                Response.Write(ret > 0);
            }
        }

        #endregion
    }
}
