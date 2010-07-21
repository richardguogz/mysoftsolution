using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MySoft.Web.Controls
{
    [ControlBuilder(typeof(RepeaterControlBuilder))]
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        private string dataItemType;
        /// <summary>
        /// DataItem数据类型
        /// </summary>
        public string DataItemType
        {
            get { return dataItemType; }
            set { dataItemType = value; }
        }
    }

    public class Repeater<TDataItem> : Repeater
    {
        protected override RepeaterItem CreateItem(int itemIndex, ListItemType itemType)
        {
            return new RepeaterItem<TDataItem>(itemIndex, itemType);
        }
    }
}