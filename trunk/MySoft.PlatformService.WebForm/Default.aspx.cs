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
        protected SecondStatus timeStatus;
        protected IList<EndPoint> clients;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                status = CastleFactory.Create().GetStatusService().GetServerStatus();
                timeStatus = CastleFactory.Create().GetStatusService().GetLastSecondStatus();
                clients = CastleFactory.Create().GetStatusService().GetEndPoints();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}