using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
//using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Web;
using MySoft.Web.UI;
using LiveChat.Entity;

namespace LiveChat.Web
{
    /// <summary>
    ///  所有控件的基页,用于Ajax调用
    /// </summary>
    public class BaseControl : UserControl, IAjaxProcessHandler, IAjaxInitHandler
    {
        protected int defaultPageSize;
        protected override void OnInit(EventArgs e)
        {
            OnAjaxInit();
            base.OnInit(e);
        }

        /// <summary>
        /// 提供Ajax的处理逻辑
        /// </summary>
        public virtual void OnAjaxProcess(CallbackParams callbackParams) { }

        /// <summary>
        /// Ajax初始化处理
        /// </summary>
        public void OnAjaxInit()
        {
            this.defaultPageSize = 15;
        }

        protected Seat GetAdmin()
        {
            if (WebUtils.ExistSession("Admin"))
            {
                return WebUtils.GetSession<Seat>("Admin");
            }

            return null;
        }

        /// <summary>
        /// 按照字符串的实际长度截取指定长度的字符串
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="Length">指定长度</param>
        /// <param name="cutText">附加的字符，如...</param>
        /// <returns></returns>
        public static string CutLen(string text, int length, string cutText)
        {
            if (text == null) return string.Empty;
            int i = 0, j = 0;
            foreach (char Char in text)
            {
                if ((int)Char > 127)
                    i += 2;
                else
                    i++;

                if (i > length)
                {
                    text = text.Substring(0, j);
                    text += cutText;
                    break;
                }
                j++;
            }
            return text;
        }
    }
}
