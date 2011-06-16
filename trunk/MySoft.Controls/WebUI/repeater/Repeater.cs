using System.Web.UI;
using System.Web.UI.WebControls;

namespace MySoft.Web.UI.Controls
{
    /// <summary>
    /// ǿ����Repeater (֧�ַ�ҳ)
    /// </summary>
    [ControlBuilder(typeof(RepeaterControlBuilder))]
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        protected HtmlPager _htmlPager;
        private string dataItemType;

        /// <summary>
        /// DataItem��������
        /// </summary>
        public string DataItemType
        {
            get { return dataItemType; }
            set { dataItemType = value; }
        }

        /// <summary>
        /// DataSourceΪHtmlPagerʱ֧�ַ�ҳ
        /// </summary>
        public override object DataSource
        {
            get
            {
                if (_htmlPager != null)
                    return _htmlPager;
                else
                    return base.DataSource;
            }
            set
            {
                if (value is HtmlPager)
                {
                    _htmlPager = value as HtmlPager;

                    //����ǰ�ؼ���������Դ
                    if (_htmlPager.DataPage != null)
                    {
                        base.DataSource = _htmlPager.DataPage.DataSource;
                    }
                }
                else
                {
                    _htmlPager = null;
                    base.DataSource = value;
                }
            }
        }
    }

    /// <summary>
    /// ǿ����Repeater (֧�ַ�ҳ)
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