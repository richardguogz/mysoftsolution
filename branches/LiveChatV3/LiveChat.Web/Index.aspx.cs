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
            try
            {
                var list = service.GetCompanies();
                Repeater1.DataSource = list;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("异常信息：" + ex.Message);
                Response.End();
            }
        }
    }
}
