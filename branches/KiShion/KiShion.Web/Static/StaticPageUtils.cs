using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Web.Hosting;
using System.Collections;

namespace KiShion.Web
{
    /// <summary>
    /// 写日志委托
    /// </summary>
    /// <param name="logMsg"></param>
    public delegate void LogHandler(string logMsg);

    /// <summary>
    /// 错误委托
    /// </summary>
    /// <param name="ex"></param>
    public delegate void StaticErrorHandler(Exception ex);

    /// <summary>
    /// 静态页处理类
    /// </summary>
    public abstract class StaticPageUtils
    {
        private const double Interval = 60000;

        public static event LogHandler OnLog;

        public static event StaticErrorHandler OnError;

        //静态生成计时器
        private static Timer StaticPageTimer;
        private static List<IStaticPageItem> staticPageItems = new List<IStaticPageItem>();
        private static Regex removeRemarkRegex = new Regex("<!--@[\\s\\S]+?-->");

        #region 启动静态页生成

        /// <summary>
        /// 启动静态管理类
        /// </summary>
        public static void Start()
        {
            Start(Interval);
        }

        /// <summary>
        /// 启动静态管理类
        /// </summary>
        /// <param name="interval">检测间隔时间(默认为一分钟)</param>
        public static void Start(double interval)
        {
            StaticPageTimer = new Timer(interval);
            StaticPageTimer.Elapsed += new ElapsedEventHandler(StaticPageTimer_Elapsed);
            StaticPageTimer.Start();
        }

        static void StaticPageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StaticPageTimer.Stop();

            lock (staticPageItems)
            {
                DateTime currentDate = DateTime.Now;
                foreach (IStaticPageItem sti in staticPageItems)
                {
                    //需要生成才启动线程
                    if (sti.NeedUpdate(currentDate))
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(obj =>
                        {
                            if (obj == null) return;

                            ArrayList arr = obj as ArrayList;
                            IStaticPageItem item = arr[0] as IStaticPageItem;
                            DateTime time = (DateTime)arr[1];
                            item.Update(time);
                        }, new ArrayList { sti, currentDate });
                    }
                }
            }

            StaticPageTimer.Start();
        }

        /// <summary>
        /// 成批添加静态页生成项
        /// </summary>
        /// <param name="items">静态生成项</param>
        public static void AddItem(params IStaticPageItem[] items)
        {
            lock (staticPageItems)
            {
                foreach (IStaticPageItem item in items)
                {
                    if (!staticPageItems.Contains(item))
                        staticPageItems.Add(item);
                }
            }
        }

        #endregion

        #region 生成页面

        /// <summary>
        /// 生成远程页面
        /// </summary>
        /// <param name="templatePath">模板文件路径，如:http://www.163.com</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="inEncoding">模板页面编码</param>
        /// <param name="outEncoding">文件保存页面编码</param>
        /// <param name="validateString">验证字符串</param>
        public static bool CreateRemotePage(string templatePath, string savePath, string validateString, Encoding inEncoding, Encoding outEncoding)
        {
            try
            {
                SaveFile(GetRemotePageString(templatePath, inEncoding, validateString), savePath, outEncoding);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(ex);
                return false;
            }
        }

        /// <summary>
        /// 生成远程页面
        /// </summary>
        /// <param name="templatePath">模板文件路径，如:http://www.163.com</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="validateString">验证字符串</param>
        public static bool CreateRemotePage(string templatePath, string savePath, string validateString)
        {
            try
            {
                SaveFile(GetRemotePageString(templatePath, new UTF8Encoding(), validateString), savePath, new UTF8Encoding());
                return true;
            }
            catch (Exception ex)
            {
                SaveError(ex);
                return false;
            }
        }

        /// <summary>
        /// 生成本地页面
        /// </summary>
        /// <param name="templatePath">模板文件路径，如:/Default.aspx</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="inEncoding">模板页面编码</param>
        /// <param name="outEncoding">文件保存页面编码</param>
        /// <param name="query">查询字符串</param>
        /// <param name="validateString">验证字符串</param>
        public static bool CreateLocalPage(string templatePath, string savePath, string validateString, string query, Encoding inEncoding, Encoding outEncoding)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, query, inEncoding, validateString), savePath, outEncoding);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(ex);
                return false;
            }
        }

        /// <summary>
        /// 生成本地页面
        /// </summary>
        /// <param name="templatePath">模板文件路径，如:/Default.aspx</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="query">查询字符串</param>
        /// <param name="validateString">验证字符串</param>
        public static bool CreateLocalPage(string templatePath, string savePath, string validateString, string query)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, query, new UTF8Encoding(), validateString), savePath, new UTF8Encoding());
                return true;
            }
            catch (Exception ex)
            {
                SaveError(ex);
                return false;
            }
        }

        /// <summary>
        /// 生成本地页面
        /// </summary>
        /// <param name="templatePath">模板文件路径，如:/Default.aspx</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="validateString">验证字符串</param>
        public static bool CreateLocalPage(string templatePath, string savePath, string validateString)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, null, new UTF8Encoding(), validateString), savePath, new UTF8Encoding());
                return true;
            }
            catch (Exception ex)
            {
                SaveError(ex);
                return false;
            }
        }

        #endregion

        #region Request处理

        /// <summary>
        /// 获取当前某文件绝对路径
        /// </summary>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            return AppDomain.CurrentDomain.BaseDirectory + path;
        }

        /// <summary>
        /// 内部处理IIS请求，获取结果
        /// </summary>
        /// <param name="page">页面路径。应用程序绝对路径。</param>
        /// <returns>解析结果</returns>
        public static string ProcessRequest(string page, string query)
        {
            using (StringWriter sw = new StringWriter())
            {
                HttpRuntime.ProcessRequest(new RequestEncoding(page.Replace("/", "\\").TrimStart('\\'), query, sw));
                return sw.ToString();
            }
        }

        public static string ProcessRequest(string page)
        {
            using (StringWriter sw = new StringWriter())
            {
                HttpRuntime.ProcessRequest(new RequestEncoding(page.Replace("/", "\\").TrimStart('\\'), null, sw));
                return sw.ToString();
            }
        }

        /// <summary>
        /// 内部处理IIS请求，获取结果
        /// </summary>
        /// <param name="page">页面路径。应用程序绝对路径。</param>
        /// <param name="encoding">编码，默认是UTF8</param>
        /// <returns>解析结果</returns>
        public static string ProcessRequest(string page, string query, Encoding encoding)
        {
            using (StringWriter sw = new StringWriter())
            {
                HttpRuntime.ProcessRequest(new RequestEncoding(page.Replace("/", "\\").TrimStart('\\'), query, sw, encoding));
                return sw.ToString();
            }
        }

        public static string ProcessRequest(string page, Encoding encoding)
        {
            using (StringWriter sw = new StringWriter())
            {
                HttpRuntime.ProcessRequest(new RequestEncoding(page.Replace("/", "\\").TrimStart('\\'), null, sw, encoding));
                return sw.ToString();
            }
        }

        #endregion

        #region 获取和保存页面内容

        /// <summary>
        /// 获取本地页面内容
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="query"></param>
        /// <param name="encoding"></param>
        /// <param name="validateString"></param>
        /// <returns></returns>
        internal static string GetLocalPageString(string templatePath, string query, Encoding encoding, string validateString)
        {
            string result;
            try
            {
                result = ProcessRequest(templatePath, query, encoding);
                result = removeRemarkRegex.Replace(result, "");
                if (!string.IsNullOrEmpty(validateString))
                {
                    if (result.IndexOf(validateString) >= 0)
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception("执行本地页面" + templatePath + (query == null ? "" : "?" + query) + "出错，页面内容和验证字符串匹配失败。");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取远程页面内容
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="encoding"></param>
        /// <param name="validateString"></param>
        /// <returns></returns>
        internal static string GetRemotePageString(string templatePath, Encoding encoding, string validateString)
        {
            WebClient wc = new WebClient();
            string result;
            try
            {
                using (Stream stream = wc.OpenRead(templatePath))
                {
                    using (StreamReader sr = new StreamReader(stream, encoding))
                    {
                        result = sr.ReadToEnd();
                        result = removeRemarkRegex.Replace(result, "");
                        if (string.IsNullOrEmpty(validateString))
                        {
                            throw new Exception("执行远程页面" + templatePath + "出错，验证字符串不能为空。");
                        }
                        else
                        {
                            if (result.IndexOf(validateString) >= 0)
                            {
                                return result;
                            }
                            else
                            {
                                throw new Exception("执行远程页面" + templatePath + "出错，页面内容和验证字符串匹配失败。");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  保存字符串到路径
        /// </summary>
        /// <param name="result">结果字符串</param>
        /// <param name="savePath">文件保存路径</param>
        /// <param name="outEncoding">文件保存页面编码</param>
        internal static void SaveFile(string result, string savePath, Encoding outEncoding)
        {
            StreamWriter writer = null;
            try
            {
                if (!File.Exists(savePath))
                {
                    FileInfo info = new FileInfo(savePath);
                    if (!info.Directory.Exists)
                    {
                        info.Directory.Create();
                    }
                    writer = new StreamWriter(info.Create(), outEncoding);
                }
                else
                {
                    writer = new StreamWriter(savePath, false, outEncoding);
                }

                writer.Write(result);

                //生成文件成功写日志
                SaveLog(string.Format("生成文件【{0}】成功！", savePath));
            }
            catch (Exception ex)
            {
                throw new IOException("文件保存出错！", ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="logMsg"></param>
        internal static void SaveLog(string logMsg)
        {
            if (OnLog != null) OnLog(logMsg);
        }

        /// <summary>
        /// 保存错误
        /// </summary>
        /// <param name="ex"></param>
        internal static void SaveError(Exception ex)
        {
            SaveLog(ex.Message);
            if (OnError != null) OnError(ex);
        }

        #endregion

    }

    /// <summary>
    /// 简单请求处理类
    /// </summary>
    internal class RequestEncoding : SimpleWorkerRequest
    {
        private TextWriter output;
        private Encoding encoding = Encoding.UTF8;

        public RequestEncoding(string page, string query, TextWriter output)
            : base(page, query, output)
        {
            this.output = output;
        }

        public RequestEncoding(string page, string query, TextWriter output, Encoding encoding)
            : base(page, query, output)
        {
            this.output = output;
            this.encoding = encoding;
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            output.Write(encoding.GetChars(data, 0, length));
        }
    }
}
