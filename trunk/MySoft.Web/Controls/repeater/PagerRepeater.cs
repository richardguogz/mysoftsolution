using System;
using System.Collections.Generic;
using System.Web;
using MySoft.Web.Controls;
using MySoft.Data;
using System.ComponentModel;
using System.Web.UI;

namespace MySoft.Web.Controls
{
    public class PagerRepeater<TDataItem> : Repeater<TDataItem>
    {
        private DataPage _dataPage;

        /// <summary>
        /// 分页数据源
        /// </summary>
        [Category("Behavior"), Browsable(false)]
        [Description("DataPage")]
        public DataPage DataPage
        {
            get
            {
                return _dataPage;
            }
            set
            {
                _dataPage = value;
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            this.DataSource = _dataPage.DataSource;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlPager pager = new HtmlPager(_dataPage, this.Page.Request.Url + "?page=$Page");
            pager.ShowBracket = false;
            writer.Write(pager.ToString());

            writer.RenderEndTag();
        }
    }
}
