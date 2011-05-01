using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySoft.PlatformService.UserService;
using MySoft.IoC;

namespace MySoft.PlatformService.WebForm
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<UserInfo> list = new List<UserInfo>();

            for (int i = 0; i < 100; i++)
            {
                int userid;
                var user = CastleFactory.Create().GetService<IUserService>().GetUserInfo(Guid.NewGuid().ToString(), out userid);
                list.Add(user);
            }

            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }
    }
}