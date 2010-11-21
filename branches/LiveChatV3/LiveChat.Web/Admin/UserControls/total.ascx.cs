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

namespace LiveChat.Web.Admin.UserControls
{
    public partial class total : BaseControl
    {
        protected DateTime startDate, endDate;
        protected bool isMonth = false;
        public override void OnAjaxProcess(CallbackParams callbackParams)
        {
            WhereClip where = WhereClip.All;
            WhereClip whereTime = WhereClip.All;

            string showType = callbackParams["type"].Value;
            if (showType == "day")
            {
                if (callbackParams.Contains("startDate"))
                {
                    startDate = ConvertTo<DateTime>(callbackParams["startDate"].Value);
                    whereTime &= new WhereClip("StartTime >= $StartTime", new SQLParameter("$StartTime", startDate));
                }
                if (callbackParams.Contains("endDate"))
                {
                    endDate = ConvertTo<DateTime>(callbackParams["endDate"].Value);
                    whereTime &= new WhereClip("StartTime <= $EndTime", new SQLParameter("$EndTime", endDate));
                }
            }
            else
            {
                isMonth = true;
                if (callbackParams.Contains("startMonth"))
                {
                    startDate = ConvertTo<DateTime>(callbackParams["startMonth"].Value + "-01");
                    whereTime &= new WhereClip("StartTime >= $StartTime", new SQLParameter("$StartTime", startDate));
                }
                if (callbackParams.Contains("endMonth"))
                {
                    endDate = ConvertTo<DateTime>(callbackParams["endMonth"].Value + "-01");
                    endDate = endDate.AddMonths(1).AddDays(-1);
                    whereTime &= new WhereClip("StartTime <= $EndTime", new SQLParameter("$EndTime", endDate));
                }
            }

            if (callbackParams.Contains("companyID"))
            {
                where &= new WhereClip("b.CompanyID = $CompanyID", new SQLParameter("$CompanyID", callbackParams["companyID"].Value));
            }
            if (callbackParams.Contains("seatCode"))
            {
                where &= new WhereClip("b.SeatCode = $SeatCode", new SQLParameter("$SeatCode", callbackParams["seatCode"].Value));
            }

            string sql = string.Empty;
            if (showType == "day")
            {
                sql = @"select c.CompanyName,b.SeatName,a.SeatID,a.TotalDate,a.SessionCount from (
                        select SeatID,substring(CONVERT(varchar,StartTime,120),1,10) as TotalDate,
                        count(*) as SessionCount from t_P2SSession 
                        where " + DataAccess.DbChat.Serialization(whereTime) +
                        @" group by SeatID,substring(CONVERT(varchar,StartTime,120),1,10)) a left join t_Seat b
                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                        left join t_Company c on b.CompanyID = c.CompanyID
                        where a.SeatID is not null " + (where == WhereClip.All ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                        @" order by a.SeatID,a.TotalDate";

            }
            else if (showType == "month")
            {
                sql = @"select c.CompanyName,b.SeatName,a.SeatID,a.TotalDate,a.SessionCount from (
                        select SeatID,substring(CONVERT(varchar,StartTime,120),1,7) as TotalDate,
                        count(*) as SessionCount from t_P2SSession 
                         where " + DataAccess.DbChat.Serialization(whereTime) +
                        @" group by SeatID,substring(CONVERT(varchar,StartTime,120),1,7)) a left join t_Seat b
                        on a.SeatID = b.CompanyID + '_' + b.SeatCode
                        left join t_Company c on b.CompanyID = c.CompanyID
                        where a.SeatID is not null " + (where == WhereClip.All ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                        @" order by a.SeatID,a.TotalDate";
            }

            var totalInfo = DataAccess.DbChat.FromSql(sql).ToTable().OriginalData;
            Repeater1.DataSource = totalInfo;
            Repeater1.DataBind();

            base.OnAjaxProcess(callbackParams);
        }
    }
}