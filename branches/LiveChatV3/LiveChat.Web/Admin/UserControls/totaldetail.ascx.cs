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
using LiveChat.Entity;

namespace LiveChat.Web.Admin.UserControls
{
    public partial class totaldetail : BaseControl
    {
        protected Dictionary<string, TotalInfo> totalDictInfo1;
        protected Dictionary<string, TotalInfo> totalDictInfo2;
        protected string totalType;
        protected string totalDate;
        protected int totalCount;

        public override void OnAjaxProcess(CallbackParams callbackParams)
        {
            WhereClip where = new WhereClip("SeatID = $SeatID", new SQLParameter("$SeatID", callbackParams["seatID"].Value));

            if (callbackParams.Contains("totalDate"))
            {
                totalDate = callbackParams["totalDate"].Value;
                if (totalDate.Length <= 8)
                {
                    totalType = "月份";
                    DateTime date = ConvertTo<DateTime>(totalDate + "-01");
                    where &= new WhereClip("StartTime >= $StartTime", new SQLParameter("$StartTime", date.ToString("yyyy-MM-dd 00:00:00")));
                    where &= new WhereClip("StartTime <= $EndTime", new SQLParameter("$EndTime", date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59")));
                }
                else
                {
                    totalType = "日期";
                    DateTime date = ConvertTo<DateTime>(totalDate);
                    where &= new WhereClip("StartTime >= $StartTime", new SQLParameter("$StartTime", date.ToString("yyyy-MM-dd 00:00:00")));
                    where &= new WhereClip("StartTime <= $EndTime", new SQLParameter("$EndTime", date.ToString("yyyy-MM-dd 23:59:59")));
                }
            }
            if (callbackParams.Contains("totalCount"))
            {
                totalCount = callbackParams["totalCount"].To<int>();
            }

            GetTimeInfo(where);
            GetAreaInfo(where);

            base.OnAjaxProcess(callbackParams);
        }

        private void GetTimeInfo(WhereClip where)
        {
            string sql = @"select SeatID, substring(convert(varchar(13), StartTime,120),12,2) as totalData,count(*) as totalCount from t_P2SSession where SeatID is not null"
                            + (where == WhereClip.None ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                            @" group by SeatID, substring(convert(varchar(13), StartTime,120),12,2)
                            order by SeatID, substring(convert(varchar(13), StartTime,120),12,2)";

            IList<TotalInfo> totalInfo = DataAccess.DbChat.FromSql(sql).ToTable().ConvertTo<TotalInfo>();

            int maxCount = 0;
            int totalCount = 0;
            foreach (TotalInfo mail in totalInfo)
            {
                if (mail.TotalCount > maxCount) maxCount = mail.TotalCount;
                totalCount += mail.TotalCount;
            }
            foreach (TotalInfo mail in totalInfo)
            {
                mail.Percent = Math.Round(mail.TotalCount / (maxCount * 1.0), 4);
                mail.Rate = Math.Round(mail.TotalCount / (totalCount * 1.0), 4);
            }

            totalDictInfo1 = new Dictionary<string, TotalInfo>();
            if (totalInfo.Count > 0)
            {
                for (int timer = 0; timer < 24; timer++)
                {
                    totalDictInfo1.Add(timer.ToString("00"), GetMailInfo(totalInfo, timer.ToString("00")));
                }
            }
        }

        private void GetAreaInfo(WhereClip where)
        {
            string whereSql = DataAccess.DbChat.Serialization(where);
            string sql = @"select SeatID, FromAddress as totalData,count(*) as totalCount from t_P2SSession where SeatID is not null"
                         + (where == WhereClip.None ? "" : " and " + DataAccess.DbChat.Serialization(where)) +
                         @" group by SeatID, FromAddress
                            order by SeatID, FromAddress";

            IList<TotalInfo> totalInfo = DataAccess.DbChat.FromSql(sql).ToTable().ConvertTo<TotalInfo>();
            int maxCount = 0;
            int totalCount = 0;
            foreach (TotalInfo mail in totalInfo)
            {
                if (mail.TotalCount > maxCount) maxCount = mail.TotalCount;
                totalCount += mail.TotalCount;
            }
            foreach (TotalInfo mail in totalInfo)
            {
                mail.Percent = Math.Round(mail.TotalCount / (maxCount * 1.0), 4);
                mail.Rate = Math.Round(mail.TotalCount / (totalCount * 1.0), 4);
            }

            totalDictInfo2 = new Dictionary<string, TotalInfo>();
            foreach (TotalInfo info in totalInfo)
            {
                if (string.IsNullOrEmpty(info.TotalData))
                {
                    continue;
                }

                info.TotalData = DataAccess.GetArea(info.TotalData);
                if (!totalDictInfo2.ContainsKey(info.SeatID + info.TotalData))
                {
                    totalDictInfo2.Add(info.SeatID + info.TotalData, info);
                }
                else
                {
                    TotalInfo mail = totalDictInfo2[info.SeatID + info.TotalData];
                    mail.TotalCount += info.TotalCount;
                }
            }

            if (totalDictInfo2.Count > 0)
            {
                IList<string> citys = DataAccess.GetCities();
                IList<TotalInfo> list = new List<TotalInfo>(totalDictInfo2.Values);
                totalDictInfo2.Clear();
                foreach (string city in citys)
                {
                    totalDictInfo2.Add(city, GetMailInfo(list, city));
                }
            }
        }

        private TotalInfo GetMailInfo(IList<TotalInfo> infos, string key)
        {
            foreach (TotalInfo info in infos)
            {
                if (info.TotalData == key) return info;
            }

            TotalInfo info1 = new TotalInfo();
            info1.TotalData = key;
            return info1;
        }
    }
}