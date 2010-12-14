using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
//using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Web.UI;
using MySoft.Web;
using LiveChat.Interface;
using LiveChat.Service;

namespace LiveChat.Web.Admin
{
    public class BaseControl : UserControl, IAjaxProcessHandler, IAjaxInitHandler
    {
        protected const int PageSize = 15;
        protected static IUserService service;
        public void OnAjaxInit()
        {
            if (service == null)
                service = RemotingUtil.GetRemotingUserService();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callbackParams"></param>
        public virtual void OnAjaxProcess(CallbackParams callbackParams)
        { }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetSession<T>(string key)
        {
            return WebHelper.GetSession<T>(key);
        }


        /// <summary>
        /// 按照字符串的实际长度截取指定长度的字符串
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="Length">指定长度</param>
        /// <param name="cutText">附加的字符，如...</param>
        /// <returns></returns>
        protected string CutLen(object value, int length, string cutText)
        {
            if (value == null) return string.Empty;
            string text = value.ToString();

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
