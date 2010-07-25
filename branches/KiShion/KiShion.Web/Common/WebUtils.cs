using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using KiShion.Web.Json;
using KiShion.Web.Converter;

namespace KiShion.Web
{
    /// <summary>
    /// ΪNoneʱ���cookieTimeΪ0ʹ��
    /// </summary>
    [Serializable]
    public enum CookieExpiresType
    {
        None,
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second
    }

    /// <summary>
    /// ͨ�÷���������
    /// </summary>
    [Serializable]
    public abstract class WebUtils
    {
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

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
                KiShion.Web.UI.AjaxNamespaceAttribute ajaxSpace = GetTypeAttribute<KiShion.Web.UI.AjaxNamespaceAttribute>(urlType);
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

            jslist.Add(page.ClientScript.GetWebResourceUrl(typeof(KiShion.Web.UI.AjaxPage), "KiShion.Web.Resources.request.js"));
            jslist.Add(page.ClientScript.GetWebResourceUrl(typeof(KiShion.Web.UI.AjaxPage), "KiShion.Web.Resources.ajax.js"));
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

        #region ת����ϵ�л����

        /// <summary>
        /// ��valueת���ɶ�Ӧ������ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(string value, T defaultValue)
        {
            bool isNullable = false;
            Type conversionType = typeof(T);
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
                isNullable = true;
            }

            bool success;
            if (ConverterFactory.Converters.ContainsKey(conversionType))
            {
                //���ת����ֵΪ�ղ��Ҷ����Ϊ��ʱ����Ĭ��ֵ
                if (string.IsNullOrEmpty(value) && isNullable) return defaultValue;

                object obj = ConverterFactory.Converters[conversionType].ConvertTo(value, out success);
                if (success) return (T)obj;
            }

            return defaultValue;
        }

        /// <summary>
        /// ������ϵ�л����ַ���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            Type type = obj.GetType();
            if (type.IsEnum)
            {
                return JavaScriptConvert.ToString((Enum)obj);
            }
            else if (type == typeof(string))
            {
                return JavaScriptConvert.ToString(obj.ToString());
            }
            else if (type == typeof(DateTime))
            {
                return JavaScriptConvert.ToString((DateTime)obj);
            }
            else if (type == typeof(bool))
            {
                return JavaScriptConvert.ToString((bool)obj);
            }
            else if (type.IsValueType)
            {
                return obj.ToString();
            }
            return JavaScriptConvert.SerializeObject(obj);
        }

        /// <summary>
        /// ���ַ�����ϵ�л��ɶ���
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="JSONString"></param>
        /// <returns></returns>
        public static object Deserialize(string jsonString, Type type)
        {
            if (type.IsArray && jsonString != null && !jsonString.StartsWith("["))
            {
                jsonString = "[" + jsonString + "]";
            }
            return JavaScriptConvert.DeserializeObject(jsonString, type);
        }

        /// <summary>
        /// ���ַ�����ϵ�л��ɶ���
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="JSONString"></param>
        /// <returns></returns>
        public static TObject Deserialize<TObject>(string jsonString)
        {
            return (TObject)Deserialize(jsonString, typeof(TObject));
        }

        /// <summary>
        /// ��ȡType��Ӧ���Զ�������
        /// </summary>
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AttributeType GetTypeAttribute<AttributeType>(Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeof(AttributeType), true);
            if (attrs != null && attrs.Length > 0)
            {
                return (AttributeType)attrs[0];
            }
            return default(AttributeType);
        }

        #endregion

        #region ���ܽ��ܴ���

        /// <summary>
        /// ���ַ�������MD5����
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string text)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(text, "MD5");
        }

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

