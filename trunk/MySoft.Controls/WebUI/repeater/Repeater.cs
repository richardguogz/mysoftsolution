using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySoft.Core;

namespace MySoft.Web.UI.Controls
{
    /// <summary>
    /// 强类型Repeater (支持分页)
    /// </summary>
    [ControlBuilder(typeof(RepeaterControlBuilder))]
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        protected HtmlPager _htmlPager;
        private string dataItemType;

        /// <summary>
        /// DataItem数据类型
        /// </summary>
        public string DataItemType
        {
            get { return dataItemType; }
            set { dataItemType = value; }
        }

        /// <summary>
        /// 分页的HtmlPager(DataSource不用再次赋值)
        /// </summary>
        public HtmlPager DataPager
        {
            get { return _htmlPager; }
            set
            {
                _htmlPager = value;

                //给当前控件设置数据源
                if (_htmlPager.DataPage != null)
                    this.DataSource = _htmlPager.DataPage.DataSource;
            }
        }
    }

    /// <summary>
    /// 强类型Repeater (支持分页)
    /// </summary>
    /// <typeparam name="TDataItem"></typeparam>
    public class Repeater<TDataItem> : Repeater
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (_htmlPager != null && _htmlPager.DataPage != null)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(_htmlPager.ToString());
                writer.RenderEndTag();
            }
        }

        protected override RepeaterItem CreateItem(int itemIndex, ListItemType itemType)
        {
            return new RepeaterItem<TDataItem>(itemIndex, itemType);
        }
    }
}