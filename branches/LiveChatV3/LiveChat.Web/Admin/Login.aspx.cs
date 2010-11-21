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
using MySoft.Web;
using LiveChat.Service;
using System.Text;

namespace LiveChat.Web.Admin
{
    public partial class login : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Response.Redirect("analysis.aspx");
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AjaxMethod]
        public bool ValidateCode(string code)
        {
            if (Session["ValidateCode"] == null) return false;
            return Session["ValidateCode"].ToString() == code;
        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AjaxMethod]
        public bool ValidateUser(string companyid, string userid, string password)
        {
            var where = t_Seat._.CompanyID == companyid && t_Seat._.SeatCode == userid;
            t_Seat seat = DataAccess.DbChat.Single<t_Seat>(where);
            if (seat == null) return false;
            bool ret = seat.Password == password;
            if (ret)
            {
                if (seat.SeatType == LiveChat.Entity.SeatType.Normal)
                {
                    throw new AjaxException("必须是管理员才能登录！");
                }

                string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(seat.CompanyID + "|" + seat.SeatCode));
                SaveSession(param, seat);
            }

            return ret;
        }

        protected override bool EnableAjaxCallback
        {
            get
            {
                return true;
            }
        }
    }
}
