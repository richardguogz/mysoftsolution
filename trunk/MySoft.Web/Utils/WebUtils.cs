using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Security.Cryptography;
using MySoft.Core;
using System.IO;

namespace MySoft.Web
{
    /// <summary>
    /// ͨ�÷���������
    /// </summary>
    [Serializable]
    public abstract class WebUtils
    {
        #region ע���ļ���ҳ��

        /// <summary>
        /// ע��Ajax����js��ҳ��
        /// </summary>
        /// <param name="type"></param>
        public static void RegisterPageForAjax(Page page, string url)
        {
            RegisterPageForAjax(page, page.GetType(), url);
        }

        /// <summary>
        /// ע��Ajax����js��ҳ��
        /// </summary>
        /// <param name="type"></param>
        public static void RegisterPageForAjax(Page page, Type urlType, string url)
        {
            #region ���ɵ�ǰpath����Դ�ļ�

            Type type = page.GetType();
            string ajaxKey = "AjaxProcess";
            if (url.IndexOf("?") >= 0) url = url.Remove(url.IndexOf("?"));
            if (page.Request.QueryString.Count > 0) url += page.Request.Url.Query;

            string query = Encrypt(url, ajaxKey);
            if (urlType != null)
            {
                UI.AjaxNamespaceAttribute ajaxSpace = CoreUtils.GetTypeAttribute<UI.AjaxNamespaceAttribute>(urlType);
                if (ajaxSpace != null) query += ";" + Encrypt(ajaxSpace.Name ?? urlType.Name, ajaxKey);
            }

            string urlResource = page.Request.ApplicationPath + (page.Request.ApplicationPath.EndsWith("/") ? "" : "/") + "Ajax/" + type.FullName + ".ashx?" + query;

            #endregion

            RegisterForAjax(page, urlResource);
        }

        /// <summary>
        /// ע��Ajax����js��ҳ��
        /// </summary>
        /// <param name="type"></param>
        private static void RegisterForAjax(Page page, string urlResource)
        {
            List<string> jslist = new List<string>();

            jslist.Add(page.ClientScript.GetWebResourceUrl(typeof(UI.AjaxPage), "MySoft.Web.Resources.request.js"));
            jslist.Add(page.ClientScript.GetWebResourceUrl(typeof(UI.AjaxPage), "MySoft.Web.Resources.ajax.js"));
            jslist.Add(urlResource);

            //��jsע�ᵽҳ��
            RegisterPageJsFile(page, jslist.ToArray());
        }

        /// <summary>
        /// ��ҳ���м���js�ļ�
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsurls"></param>
        public static void RegisterPageJsFile(Page page, params string[] jsfiles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string jsfile in jsfiles)
            {
                sb.Append("<script src=\"" + jsfile + "\" type=\"text/javascript\"></script>\r\n");
            }

            string cskey = "key" + Guid.NewGuid().ToString();
            ClientScriptManager cs = page.ClientScript;
            Type type = page.GetType();
            if (!cs.IsClientScriptBlockRegistered(type, cskey))
            {
                cs.RegisterClientScriptBlock(type, cskey, sb.ToString(), false);
            }
        }

        /// <summary>
        /// ���Javascript�ű���ҳ��
        /// </summary>
        /// <param name="scriptString"></param>
        public static void RegisterPageScript(Page page, params string[] scripts)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string script in scripts)
            {
                sb.Append(script);
            }

            string cskey = "key" + Guid.NewGuid().ToString();
            ClientScriptManager cs = page.ClientScript;
            Type type = page.GetType();
            if (!cs.IsClientScriptBlockRegistered(type, cskey))
            {
                cs.RegisterClientScriptBlock(type, cskey, sb.ToString(), true);
            }
        }


        /// <summary>
        /// ��ҳ���м���css�ļ�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cssurls"></param>
        public static void RegisterPageCssFile(Page page, params string[] cssfiles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string cssfile in cssfiles)
            {
                sb.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + cssfile + "\" />\r\n");
            }

            string cskey = "key" + Guid.NewGuid().ToString();
            ClientScriptManager cs = page.ClientScript;
            Type type = page.GetType();
            if (!cs.IsClientScriptBlockRegistered(type, cskey))
            {
                cs.RegisterClientScriptBlock(type, cskey, sb.ToString(), false);
            }
        }

        #endregion

        #region ���ͱ���Cookie

        public static void SaveCookie(string cookieName, string value)
        {
            SaveCookie(cookieName, value, null);
        }

        public static void SaveCookie(string cookieName, string value, CookieExpiresType expiresType, int cookieTime)
        {
            SaveCookie(cookieName, value, null, expiresType, cookieTime);
        }

        public static void SaveCookie(string cookieName, string[] paramNames, string[] paramValues)
        {
            SaveCookie(cookieName, paramNames, paramValues, null);
        }

        public static void SaveCookie(string cookieName, string value, string cookieDomain)
        {
            SaveCookie(cookieName, new string[] { cookieName }, new string[] { value }, cookieDomain, CookieExpiresType.None, 0);
        }

        public static void SaveCookie(string cookieName, string value, string cookieDomain, CookieExpiresType expiresType, int cookieTime)
        {
            SaveCookie(cookieName, new string[] { cookieName }, new string[] { value }, cookieDomain, expiresType, cookieTime);
        }

        public static void SaveCookie(string cookieName, string[] paramNames, string[] paramValues, string cookieDomain)
        {
            SaveCookie(cookieName, paramNames, paramValues, cookieDomain, CookieExpiresType.None, 0);
        }

        /// <summary>
        /// ����һ��Cookie,0Ϊ��������
        /// </summary>
        /// <param name="cookieName">Cookie����</param>
        /// <param name="CookieValue">Cookieֵ</param>
        /// <param name="CookieTime">Cookie����ʱ��(����),0Ϊ�ر�ҳ��ʧЧ</param>
        public static void SaveCookie(string cookieName, string[] paramNames, string[] paramValues, string cookieDomain, CookieExpiresType expiresType, int cookieTime)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;

            if (paramNames.Length == 1 && paramNames[0] == cookieName)
            {
                myCookie.Value = paramValues[0];
            }
            else
            {
                for (int index = 0; index < paramNames.Length; index++)
                {
                    myCookie.Values[paramNames[index]] = paramValues[index];
                }
            }

            if (cookieDomain != string.Empty && cookieDomain != null)
            {
                myCookie.Domain = cookieDomain;
            }

            //���ù���ʱ��
            switch (expiresType)
            {
                case CookieExpiresType.None:
                    //���ó���������
                    myCookie.Expires = new DateTime(9999, 12, 31);
                    break;
                case CookieExpiresType.Year:
                    myCookie.Expires = now.AddYears(cookieTime);
                    break;
                case CookieExpiresType.Month:
                    myCookie.Expires = now.AddMonths(cookieTime);
                    break;
                case CookieExpiresType.Day:
                    myCookie.Expires = now.AddDays(cookieTime);
                    break;
                case CookieExpiresType.Hour:
                    myCookie.Expires = now.AddHours(cookieTime);
                    break;
                case CookieExpiresType.Minute:
                    myCookie.Expires = now.AddMinutes(cookieTime);
                    break;
                case CookieExpiresType.Second:
                    myCookie.Expires = now.AddSeconds(cookieTime);
                    break;
            }

            if (HttpContext.Current.Response.Cookies[cookieName] != null)
            {
                HttpContext.Current.Response.Cookies.Remove(cookieName);
            }

            HttpContext.Current.Response.AppendCookie(myCookie);
        }

        /// <summary>
        /// ���cookid���Ƿ����ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistCookie(string cookieName)
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (myCookie == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ȡ��CookieValue
        /// </summary>
        /// <param name="cookieName">Cookie����</param>
        /// <returns>Cookie��ֵ</returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie = HttpContext.Current.Request.Cookies[cookieName];

            if (myCookie != null)
                return myCookie;
            else
                return null;
        }

        /// <summary>
        /// ���CookieValue
        /// </summary>
        /// <param name="cookieName">Cookie����</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;

            myCookie.Expires = now.AddYears(-1);

            HttpContext.Current.Response.SetCookie(myCookie);
        }

        #endregion

        #region ���ͱ���Session
        /// <summary>
        /// ����Sessionֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// ��ҳ�����Ƴ�Sessionֵ
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        public static void RemoveSession(string key)
        {
            try
            {
                HttpContext.Current.Session.Remove(key);
            }
            catch { }
        }

        /// <summary>
        /// ���Session���Ƿ����ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistSession(string key)
        {
            try
            {
                if (HttpContext.Current.Session[key] == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��Session�л�ȡ����
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TObject GetSession<TObject>(string key)
        {
            try
            {
                return (TObject)HttpContext.Current.Session[key];
            }
            catch
            {
                return default(TObject);
            }
        }
        #endregion

        #region ���ַ����ļ���/����

        /// <summary>
        /// ���ַ���������Ӧ ServU �� MD5 ����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ServUEncrypt(string str)
        {
            string strResult = "";
            strResult = RandomSTR(2);
            str = strResult + str;
            str = NoneEncrypt(str, 1);
            str = strResult + str;

            return str;
        }

        /// <summary>
        /// ��ȡһ����26��Сд��ĸ��ɵ�ָ�����ȵ��漴�ַ���
        /// </summary>
        /// <param name="intLong">ָ������</param>
        /// <returns></returns>
        private static string RandomSTR(int intLong)
        {
            string strResult = "";
            string[] array = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            Random r = new Random();

            for (int i = 0; i < intLong; i++)
            {
                strResult += array[r.Next(26)];
            }

            return strResult;
        }

        /// <summary>
        /// ���ַ���������ͨmd5����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            str = NoneEncrypt(str, 1);
            return str;
        }

        /// <summary>
        /// ���ַ������м��ܣ������棩
        /// </summary>
        /// <param name="Password">Ҫ���ܵ��ַ���</param>
        /// <param name="Format">���ܷ�ʽ,0 is SHA1,1 is MD5</param>
        /// <returns></returns>
        public static string NoneEncrypt(string Password, int Format)
        {
            string strResult = "";
            switch (Format)
            {
                case 0:
                    strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
                    break;
                case 1:
                    strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
                    break;
                default:
                    strResult = Password;
                    break;
            }

            return strResult;
        }


        /// <summary>
        /// ���ַ������м���
        /// </summary>
        /// <param name="Passowrd">�����ܵ��ַ���</param>
        /// <returns>string</returns>
        public static string Encrypt(string Passowrd)
        {
            string strResult = "";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Passowrd, true, 2);
            strResult = FormsAuthentication.Encrypt(ticket).ToString();

            return strResult;
        }

        /// <summary>
        /// ���ַ������н���
        /// </summary>
        /// <param name="Passowrd">�Ѽ��ܵ��ַ���</param>
        /// <returns></returns>
        public static string Decrypt(string Passowrd)
        {
            string strResult = "";

            strResult = FormsAuthentication.Decrypt(Passowrd).Name.ToString();

            return strResult;
        }

        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        /// <summary>
        /// ���ַ������м���
        /// </summary>
        /// <param name="text">�����ܵ��ַ���</param>
        /// <returns>string</returns>
        public static string Encrypt(string text, string key)
        {
            try
            {
                key = key.PadRight(32, ' ');
                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(key);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

                byte[] inputData = Encoding.UTF8.GetBytes(text);
                byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Convert.ToBase64String(encryptedData);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// ���ַ������н���
        /// </summary>
        /// <param name="text">�Ѽ��ܵ��ַ���</param>
        /// <returns></returns>
        public static string Decrypt(string text, string key)
        {
            try
            {
                key = key.PadRight(32, ' ');
                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(key);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(text);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
