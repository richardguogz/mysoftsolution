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
using MySoft.Web;

namespace LiveChat.Web
{
    public partial class Index : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var list = service.GetCompanies();
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }

        //private void CreateStaticPage()
        //{
        //    IUpdateDependency update = new SlidingUpdateTime(new TimeSpan(0, 1, 0));

        //    IStaticPageItem item1 = new SingleStaticPageItem("/default.aspx", Server.MapPath("/default.html"), "mysoft", update);
        //    IStaticPageItem item2 = new ParamStaticPageItem("/news.aspx", "sort=$sort&id=$id", Server.MapPath("/default_$sort_$id.html"), "mysoft", update,
        //        new StaticPageParamInfo("$sort", new object[] { "fund", "netvalue" }), new StaticPageParamInfo("$id", 1, 10));

        //    item1.Callback += new CallbackEventHandler(item1_Callback);
        //    item2.Callback += new CallbackEventHandler(item2_Callback);

        //    StaticPageUtils.AddItem(item1);
        //    StaticPageUtils.AddItem(item2);

        //    StaticPageUtils.Start(1000);
        //}

        //string item2_Callback(string content)
        //{
        //    content = content.Replace("a.aspx", "a.htm");
        //    return content;
        //}

        //string item1_Callback(string content)
        //{
        //    content = content.Replace("a.aspx", "a.htm");
        //    return content;
        //}
    }
}
