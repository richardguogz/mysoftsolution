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
    /// д��־ί��
    /// </summary>
    /// <param name="logMsg"></param>
    public delegate void LogHandler(string logMsg);

    /// <summary>
    /// ����ί��
    /// </summary>
    /// <param name="ex"></param>
    public delegate void StaticErrorHandler(Exception ex);

    /// <summary>
    /// ��̬ҳ������
    /// </summary>
    public abstract class StaticPageUtils
    {
        private const double Interval = 60000;

        public static event LogHandler OnLog;

        public static event StaticErrorHandler OnError;

        //��̬���ɼ�ʱ��
        private static Timer StaticPageTimer;
        private static List<IStaticPageItem> staticPageItems = new List<IStaticPageItem>();
        private static Regex removeRemarkRegex = new Regex("<!--@[\\s\\S]+?-->");

        #region ������̬ҳ����

        /// <summary>
        /// ������̬������
        /// </summary>
        public static void Start()
        {
            Start(Interval);
        }

        /// <summary>
        /// ������̬������
        /// </summary>
        /// <param name="interval">�����ʱ��(Ĭ��Ϊһ����)</param>
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
                    //��Ҫ���ɲ������߳�
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
        /// ������Ӿ�̬ҳ������
        /// </summary>
        /// <param name="items">��̬������</param>
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

        #region ����ҳ��

        /// <summary>
        /// ����Զ��ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:http://www.163.com</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="inEncoding">ģ��ҳ�����</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
        /// <param name="validateString">��֤�ַ���</param>
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
        /// ����Զ��ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:http://www.163.com</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="validateString">��֤�ַ���</param>
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
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="inEncoding">ģ��ҳ�����</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
        /// <param name="query">��ѯ�ַ���</param>
        /// <param name="validateString">��֤�ַ���</param>
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
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="query">��ѯ�ַ���</param>
        /// <param name="validateString">��֤�ַ���</param>
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
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="validateString">��֤�ַ���</param>
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

        #region Request����

        /// <summary>
        /// ��ȡ��ǰĳ�ļ�����·��
        /// </summary>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            return AppDomain.CurrentDomain.BaseDirectory + path;
        }

        /// <summary>
        /// �ڲ�����IIS���󣬻�ȡ���
        /// </summary>
        /// <param name="page">ҳ��·����Ӧ�ó������·����</param>
        /// <returns>�������</returns>
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
        /// �ڲ�����IIS���󣬻�ȡ���
        /// </summary>
        /// <param name="page">ҳ��·����Ӧ�ó������·����</param>
        /// <param name="encoding">���룬Ĭ����UTF8</param>
        /// <returns>�������</returns>
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

        #region ��ȡ�ͱ���ҳ������

        /// <summary>
        /// ��ȡ����ҳ������
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
                        throw new Exception("ִ�б���ҳ��" + templatePath + (query == null ? "" : "?" + query) + "����ҳ�����ݺ���֤�ַ���ƥ��ʧ�ܡ�");
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
        /// ��ȡԶ��ҳ������
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
                            throw new Exception("ִ��Զ��ҳ��" + templatePath + "������֤�ַ�������Ϊ�ա�");
                        }
                        else
                        {
                            if (result.IndexOf(validateString) >= 0)
                            {
                                return result;
                            }
                            else
                            {
                                throw new Exception("ִ��Զ��ҳ��" + templatePath + "����ҳ�����ݺ���֤�ַ���ƥ��ʧ�ܡ�");
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
        ///  �����ַ�����·��
        /// </summary>
        /// <param name="result">����ַ���</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
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

                //�����ļ��ɹ�д��־
                SaveLog(string.Format("�����ļ���{0}���ɹ���", savePath));
            }
            catch (Exception ex)
            {
                throw new IOException("�ļ��������", ex);
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
        /// ������־
        /// </summary>
        /// <param name="logMsg"></param>
        internal static void SaveLog(string logMsg)
        {
            if (OnLog != null) OnLog(logMsg);
        }

        /// <summary>
        /// �������
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
    /// ����������
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
