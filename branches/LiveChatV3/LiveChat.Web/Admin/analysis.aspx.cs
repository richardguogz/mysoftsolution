using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using MySoft.Web.UI;
using MySoft.Data;
using LiveChat.Entity;
using LiveChat.Service;

namespace LiveChat.Web.Admin
{
    public partial class analysis : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dpCompany.DataTextField = "CompanyName";
            dpCompany.DataValueField = "CompanyID";

            var list = service.GetCompanies();
            dpCompany.DataSource = list;
            dpCompany.DataBind();

            var item = new ListItem("全部", "-1");
            dpCompany.Items.Insert(0, item);
            dpCompany.SelectedIndex = 0;

            if (user.SeatType != SeatType.Super)
            {
                dpCompany.SelectedValue = user.CompanyID;
                dpCompany.Enabled = false;

                var seats = service.GetCompanySeats(user.CompanyID);
                dpSeat.DataTextField = "ShowName";
                dpSeat.DataValueField = "SeatCode";
                dpSeat.DataSource = seats;
                dpSeat.DataBind();
            }

            dpSeat.Items.Insert(0, item);
        }

        /// <summary>
        /// 获取指定公司客服
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        [AjaxMethod]
        public IList<Seat> GetSeats(string companyid)
        {
            return service.GetCompanySeats(companyid);
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
