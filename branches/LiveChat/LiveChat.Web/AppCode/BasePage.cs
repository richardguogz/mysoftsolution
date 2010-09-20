using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Web.UI;
using System.Text;
using MySoft.Web;
using System.IO;
using MySoft.Core.Converter;
using LiveChat.Entity;
using LiveChat.Interface;

namespace LiveChat.Web
{
    /// <summary>
    /// 所有页面的基页,用于Ajax调用
    /// </summary>
    public class BasePage : AjaxPage, IAjaxInitHandler
    {
        protected static ISeatService service;
        public virtual void OnAjaxInit()
        {
            if (service == null)
                service = RemotingUtil.GetRemotingSeatService();
        }

        /// <summary>
        /// 将Request中的值转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nvc"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected T ConvertEntity<T>(NameValueCollection nvc, string prefix)
            where T : class, new()
        {
            ObjectBuilder<T> objBuilder = new ObjectBuilder<T>(prefix);
            return objBuilder.Bind(nvc);
        }

        protected Seat GetAdmin()
        {
            if (WebUtils.ExistSession("Admin"))
            {
                return WebUtils.GetSession<Seat>("Admin");
            }
            else
            {
                if (WebUtils.ExistCookie("Admin"))
                {
                    Seat admin = service.GetSeat(WebUtils.GetCookie("Admin").Value);
                    WebUtils.SaveSession("Admin", admin);

                    return admin;
                }
            }

            return null;
        }

        protected void SaveAdmin(Seat admin, bool isSaveLogin)
        {
            if (isSaveLogin)
                WebUtils.SaveCookie("Admin", admin.SeatID.ToString(), CookieExpiresType.Day, 1);
            else
                WebUtils.ClearCookie("Admin");

            WebUtils.SaveSession("Admin", admin);
        }

        protected void ClearAdmin()
        {
            WebUtils.ClearCookie("Admin");
            WebUtils.RemoveSession("Admin");
        }

        protected void JsWrite(string jsCode)
        {
            string scriptBlock = WrapScriptTag(ConvertScript(jsCode));
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), scriptBlock);
        }

        protected void JsAlert(string msg)
        {
            this.JsWrite("alert('" + ConvertScript(msg) + "');");
        }

        /// <summary>
        /// Wraps the script tag.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        /// <returns>The script.</returns>
        protected string WrapScriptTag(params string[] scripts)
        {
            if (scripts != null && scripts.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\r\n<script language=\"javascript\" type=\"text/javascript\">\r\n<!--\r\n");

                foreach (string script in scripts)
                {
                    string str = ConvertScript(script);
                    sb.Append(str.EndsWith(";") || str.EndsWith("}") ? str : str + ";");
                }

                sb.Append("\r\n-->\r\n</script>\r\n");
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 对脚本程序进行处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ConvertScript(string str)
        {
            string strResult = "";
            if (str != "")
            {
                StringReader sr = new StringReader(str);
                string rl;
                do
                {
                    strResult += sr.ReadLine();
                } while ((rl = sr.ReadLine()) != null);
            }

            strResult = strResult.Replace("\"", "&quot;");

            return strResult;
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
