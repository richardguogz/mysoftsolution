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

namespace LiveChat.Web
{
    public partial class Online : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string online = GetRequestParam<string>("online", "<font color=\"red\">在线</font>");
            string offline = GetRequestParam<string>("offline", "离线");
            string companyID = GetRequestParam<string>("companyID", null);
            string type = GetRequestParam<string>("type", "jpg");

            if (string.IsNullOrEmpty(companyID))
            {
                Response.Write("传入的公司ID不正确！");
                Response.End();
            }

            var ret = service.GetSeatOnline(companyID);
            var seatid = GetRequestParam<string>("seatCode", null);

            if (seatid != null)
            {
                ret = service.GetSeatOnline(companyID, seatid);
            }

            if (type == "jpg")
            {
                if (ret) //在线
                {
                    Response.Redirect(online + "?" + DateTime.Now.ToString("yyyyMMddHHmmss"), true);
                }
                else //离线
                {
                    Response.Redirect(offline + "?" + DateTime.Now.ToString("yyyyMMddHHmmss"), true);
                }
            }
            else if (type == "text")
            {
                if (ret) //在线
                {
                    Response.Write("document.write('" + online + "');");
                }
                else //离线
                {
                    Response.Write("document.write('" + offline + "');");
                }
                Response.End();
            }

            Response.Write("参数错误！");
            Response.End();
        }
    }
}
