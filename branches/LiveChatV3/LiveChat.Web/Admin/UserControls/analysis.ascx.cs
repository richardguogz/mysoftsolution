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
using MySoft.Data;
using MySoft.Web.UI;
using LiveChat.Service;
using MySoft.Web;
using MySoft.Core;

namespace LiveChat.Web.Admin.UserControls
{
    public partial class analysis : BaseControl
    {
        protected DateTime startDate, endDate;
        public override void OnAjaxProcess(CallbackParams callbackParams)
        {
            WhereClip where = WhereClip.All;
            if (callbackParams.Contains("startDate"))
            {
                startDate = ConvertTo<DateTime>(callbackParams["startDate"].Value);
                where &= new WhereClip("a.StartTime >= $StartTime", new SQLParameter("$StartTime", startDate));
            }
            if (callbackParams.Contains("endDate"))
            {
                endDate = ConvertTo<DateTime>(callbackParams["endDate"].Value);
                where &= new WhereClip("a.StartTime <= $EndTime", new SQLParameter("$EndTime", endDate));
            }

            if (callbackParams.Contains("companyID"))
            {
                where &= new WhereClip("b.CompanyID = $CompanyID", new SQLParameter("$CompanyID", callbackParams["companyID"].Value));
            }
            if (callbackParams.Contains("seatCode"))
            {
                where &= new WhereClip("b.SeatCode = $SeatCode", new SQLParameter("$SeatCode", callbackParams["seatCode"].Value));
            }

            int pageIndex = 1;
            string sql = string.Empty;
            string countsql = @"select count(*) from t_P2SSession a left join t_Seat b
                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                        left join t_Company c on b.CompanyID = c.CompanyID
                        where a.SeatID is not null " + (where == WhereClip.All ? "" : " and " + DataAccess.DbChat.Serialization(where));

            if (callbackParams.Contains("pageIndex"))
            {
                pageIndex = callbackParams["pageIndex"].To<int>();
            }

            if (pageIndex > 1)
            {
                //                sql = @"select * from (select c.CompanyName,b.SeatName,a.*,row_number() over(order by a.SeatID, a.StartTime desc) as RowNumber from t_P2SSession a left join t_Seat b
                //                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                //                        left join t_Company c on b.CompanyID = c.CompanyID
                //                        where a.SeatID is not null and " + DataAccess.DbChat.Serialization(where) +
                //                        ") tmp where RowNumber between " + ((pageIndex - 1) * PageSize + 1) + " and " + (pageIndex * PageSize);

                sql = @"select c.CompanyName,b.SeatName,a.*,RowNumber=identity(int,1,1) into #tempTable from t_P2SSession a left join t_Seat b
                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                        left join t_Company c on b.CompanyID = c.CompanyID
                        where a.SeatID is not null " + (where == WhereClip.All ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                        @" order by a.StartTime desc;select * from #tempTable where RowNumber between " + ((pageIndex - 1) * PageSize + 1) + " and " + (pageIndex * PageSize);
            }
            else
            {
                sql = @"select top " + PageSize + @" c.CompanyName,b.SeatName,a.* from t_P2SSession a left join t_Seat b
                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                        left join t_Company c on b.CompanyID = c.CompanyID
                        where a.SeatID is not null " + (where == WhereClip.All ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                        @" order by  a.StartTime desc";
            }

            var totalInfo = DataAccess.DbChat.FromSql(sql).ToTable().OriginalData;
            var count = DataAccess.DbChat.FromSql(countsql).ToScalar<int>();
            rptData.DataSource = totalInfo;
            rptData.DataBind();

            var dataPage = new DataPage(PageSize);
            dataPage.CurrentPageIndex = pageIndex;
            dataPage.RowCount = count;

            HtmlPager pager = new HtmlPager(dataPage, string.Format("javascript:gotoPage($Page);", 10));
            pager.ShowBracket = false;
            pager.PrevPageTitle = "<<";
            pager.NextPageTitle = ">>";
            pager.LinkStyle = LinkStyle.Flickr;
            txtHtml.Text = pager.ToString();

            base.OnAjaxProcess(callbackParams);
        }
    }
}