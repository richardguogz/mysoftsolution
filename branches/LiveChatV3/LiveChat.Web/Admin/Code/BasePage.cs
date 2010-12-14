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
using LiveChat.Service;
using LiveChat.Entity;
using System.Text;
using MySoft.Web;

namespace LiveChat.Web.Admin
{
    public class BasePage : ParentPage, IAjaxInitHandler
    {
        protected t_Seat user;
        public override void OnAjaxInit()
        {
            base.OnAjaxInit();

            var param = GetRequestParam<string>("param", null);
            user = GetSession<t_Seat>(param);
            if (user == null)
            {
                try
                {
                    if (param != null)
                    {
                        var d = Encoding.UTF8.GetString(Convert.FromBase64String(param));
                        var companyid = d.Split('|')[0];
                        var userid = d.Split('|')[1];
                        var where = t_Seat._.CompanyID == companyid && t_Seat._.SeatCode == userid;
                        user = DataAccess.DbChat.Single<t_Seat>(where);
                        SaveSession(param, user);
                    }
                }
                catch
                {
                    if (isPageLoad)
                    {
                        Response.Write("验证登录信息失败！");
                        Response.End();
                    }
                    else
                    {
                        throw new AjaxException("验证登录信息失败！");
                    }
                }
            }

            if (user == null)
            {
                if (isPageLoad)
                {
                    Response.Redirect("/admin/login.aspx");
                }
                else
                {
                    throw new AjaxException("当前会话已经过期！");
                }
            }
            else if (user.SeatType == SeatType.Normal)
            {
                if (isPageLoad)
                {
                    Response.Write("您没有操作权限！");
                    Response.End();
                }
                else
                {
                    throw new AjaxException("您没有操作权限！");
                }
            }
        }

        protected void JsWrite(string jsCode)
        {
            string scriptBlock = "<script type='text/javascript'>" + jsCode + "</script>";

            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), scriptBlock);
        }

        protected void JsAlert(string msg)
        {
            this.JsWrite("alert('" + msg + "');");
        }
    }
}
