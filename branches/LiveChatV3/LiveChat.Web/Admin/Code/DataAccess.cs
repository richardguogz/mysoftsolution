using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Data;
using LiveChat.Interface;

namespace LiveChat.Web.Admin
{
    public class DataAccess
    {
        public readonly static DbSession DbChat;

        static DataAccess()
        {
            IUserService service = RemotingUtil.GetRemotingUserService();
            DbProvider provider = DbProviderFactory.CreateDbProvider(DbProviderType.SqlServer9, service.GetConnectionString());
            DbChat = new DbSession(provider);
            //DbChat.RegisterSqlLogger(log =>
            //{
            //    System.IO.File.AppendAllText("d:\\sql.log", log + "\r\n");
            //});
        }

        public static string[] GetCities()
        {
            string[] citys = {
                                "北京" ,
                                "天津" ,
                                "河北" ,
                                "山西" ,
                                "内蒙古" ,
                                "辽宁" ,
                                "吉林" ,
                                "黑龙江" ,
                                "上海" ,
                                "江苏" ,
                                "浙江" ,
                                "安徽" ,
                                "福建" ,
                                "江西" ,
                                "山东" ,
                                "河南" ,
                                "湖北" ,
                                "湖南" ,
                                "广东" ,
                                "广西" ,
                                "海南" ,
                                "重庆" ,
                                "四川" ,
                                "贵州" ,
                                "云南" ,
                                "西藏" ,
                                "陕西" ,
                                "甘肃" ,
                                "青海" ,
                                "宁夏" ,
                                "新疆" ,
                                "其它"
                            };

            return citys;
        }

        public static string GetArea(string address)
        {
            foreach (string city in GetCities())
            {
                if (address.Contains(city)) return city;
            }

            return "其它";
        }

        #region 省份
        //北京 京  
        //天津 津  
        //河北 冀  
        //山西 晋  
        //内蒙古 内蒙古  
        //辽宁 辽  
        //吉林 吉  
        //黑龙江 黑  
        //上海 沪  
        //江苏 苏  
        //浙江 浙  
        //安徽 皖  
        //福建 闽  
        //江西 赣  
        //山东 鲁  
        //河南 豫  
        //湖北 鄂  
        //湖南 湘  
        //广东 粤  
        //广西 桂  
        //海南 琼  
        //重庆 渝  
        //四川 川  
        //贵州 黔  
        //云南 滇  
        //西藏 藏  
        //陕西 陕  
        //甘肃 甘  
        //青海 青  
        //宁夏 宁  
        //新疆 新  
        //香港 港  
        //澳门 澳  
        //台湾 台 
        #endregion
    }

    public class TotalInfo
    {
        public string SeatID { get; set; }

        public String TotalData { get; set; }

        public Int32 TotalCount { get; set; }

        public Double Percent { get; set; }

        public Double Rate { get; set; }
    }

}
