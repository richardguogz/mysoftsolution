using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MySoft.Web.Controls
{
    public class RepeaterItem<TDataItem> : System.Web.UI.WebControls.RepeaterItem
    {
        public RepeaterItem(int itemIndex, ListItemType itemType)
            : base(itemIndex, itemType)
        {
        }

        public new TDataItem DataItem
        {
            get { return (TDataItem)base.DataItem; }
            set { base.DataItem = value; }
        }
    }
}