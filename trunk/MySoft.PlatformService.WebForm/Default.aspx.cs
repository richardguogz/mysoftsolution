using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySoft.PlatformService.UserService;
using MySoft.IoC;
using System.Net;

namespace MySoft.PlatformService.WebForm
{
    public partial class _Default : System.Web.UI.Page
    {
        protected ServerStatus status;
        protected TimeStatus timeStatus;
        protected HighestStatus highestStatus;
        protected IList<ConnectInfo> clients;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                status = CastleFactory.Create().StatusService.GetServerStatus();
                timeStatus = CastleFactory.Create().StatusService.GetLastTimeStatus();
                highestStatus = CastleFactory.Create().StatusService.GetHighestStatus();
                clients = CastleFactory.Create().StatusService.GetConnectInfoList();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}