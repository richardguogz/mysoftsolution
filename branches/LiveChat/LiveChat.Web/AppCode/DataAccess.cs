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

namespace LiveChat.Web
{
    public class DataAccess
    {
        public readonly static DbSession DbChat;

        static DataAccess()
        {
            IUserService service = RemotingUtil.GetRemotingUserService();
            DbProvider provider = DbProviderFactory.CreateDbProvider(DbProviderType.SqlServer9, service.GetConnectionString());
            DbChat = new DbSession(provider);
        }
    }
}
