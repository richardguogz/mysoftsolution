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
        /// ��ҳ��HtmlPager(DataSource�����ٴθ�ֵ)
        /// </summary>
        public HtmlPager DataPager
        {
            get { return _htmlPager; }
            set
            {
                _htmlPager = value;

                //����ǰ�ؼ���������Դ
                if (_htmlPager.DataPage != null)
                    this.DataSource = _htmlPager.DataPage.DataSource;
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