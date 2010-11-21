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
using LiveChat.Entity;

namespace LiveChat.Web.Admin.UserControls
{
    public partial class analysisdetail : BaseControl
    {
        protected IList<P2SMessage> msglist;
        protected string createID;
        public override void OnAjaxProcess(CallbackParams callbackParams)
        {
            Guid sid = callbackParams["sid"].To<Guid>();
            var session = DataAccess.DbChat.Single<t_P2SSession>(t_P2SSession._.SID == sid);
            createID = session.CreateID;

            msglist = service.GetP2SHistoryMessagesFromDB(sid, 1, 1000).DataSource;

            base.OnAjaxProcess(callbackParams);
        }
    }
}