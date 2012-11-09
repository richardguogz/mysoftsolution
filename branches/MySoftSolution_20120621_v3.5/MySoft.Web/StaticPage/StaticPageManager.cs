using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using MySoft.Logger;

namespace MySoft.Web
{
    /// <summary>
    /// ��̬ҳ������
    /// </summary>
    public abstract class StaticPageManager
    {
        private const int INTERVAL = 60;

        public static event LogEventHandler OnLog;

        public static event ErrorLogEventHandler OnError;

        private static bool isRunning = false;

        //��̬ҳ������
        private static List<IStaticPageItem> staticPageItems = new List<IStaticPageItem>();

        #region ������̬ҳ����

        /// <summary>
        /// ������̬������
        /// </summary>
        public static void Start()
        {
            Start(INTERVAL);
        }

        /// <summary>
        /// ������̬������
        /// </summary>
        public static void Start(bool isStartUpdate)
        {
            Start(INTERVAL, isStartUpdate);
        }

        /// <summary>
        /// ������̬������
        /// </summary>
        /// <param name="interval">�����ʱ��(Ĭ��Ϊһ����) ����λ���룩</param>
        public static void Start(int interval)
        {
            Start(interval, false);
        }

        /// <summary>
        /// ������̬������
        /// </summary>
        /// <param name="interval">�����ʱ�䣺��λ���룩</param>
        public static void Start(int interval, bool isStartUpdate)
        {
            if (isRunning) return;

            isRunning = true;

            if (isStartUpdate)
            {
                //����һ����ʱ�߳�����
                ThreadPool.QueueUserWorkItem(state => RunUpdate(DateTime.MaxValue));
            }

            //����һ��ѭ���߳�����
            Thread thread = new Thread(state =>
            {
                if (state == null) return;

                var timeSpan = TimeSpan.FromSeconds(Convert.ToInt32(state));

                while (true)
                {
                    try
                    {
                        RunUpdate(DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                    }

                    //���߼��
                    Thread.Sleep(timeSpan);
                }
            });

            //��λ����
            thread.Start(interval);
        }

        static void RunUpdate(DateTime updateTime)
        {
            lock (staticPageItems)
            {
                foreach (IStaticPageItem sti in staticPageItems)
                {
                    //��Ҫ���ɲ������߳�
                    if (sti.NeedUpdate(updateTime))
                    {
                        try
                        {
                            sti.Update(updateTime);
                        }
                        catch (Exception ex)
                        {
                            var exception = new StaticPageException("ִ��ҳ�����ɳ����쳣��" + ex.Message, ex);
                            if (OnError != null)
                            {
                                try
                                {
                                    OnError(exception);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
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

        /// <summary>
        /// ����ͳһԶ��������������Ϣ
        /// </summary>
        /// <param name="domainUri"></param>
        public static void SetRemote(string domainUri)
        {
            lock (staticPageItems)
            {
                foreach (IStaticPageItem item in staticPageItems)
                {
                    if (item.IsRemote) continue;

                    item.SetDomain(domainUri);
                }
            }
        }

        #endregion

        /// <summary>
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">��·������:/Default.aspx?code=123456</param>
        /// <param name="savePath">���浽·����ȫ·����</param>
        /// <param name="validateString">��֤�ַ���</param>
        internal static bool CreateStaticPage(String templatePath, String savePath, String validateString)
        {
            try
            {
                var content = string.Empty;

                //�ж��Ƿ���http://
                if (!templatePath.ToLower().StartsWith("http://"))
                {
                    var arr = templatePath.Split('?');
                    var url = arr.Length > 0 ? arr[0] : string.Empty;
                    var query = arr.Length > 1 ? arr[1] : string.Empty;

                    content = StaticPageManager.GetLocalPageString(url, query, Encoding.UTF8, validateString);
                }
                else
                {
                    content = StaticPageManager.GetRemotePageString(templatePath, Encoding.UTF8, validateString);
                }

                var dynamicurl = templatePath;
                var staticurl = savePath;

                //���뾲̬ҳ����Ԫ��
                content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ��Redirect��ʽ���� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());


                StaticPageManager.SaveFile(content, savePath, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException("ִ��ҳ��" + templatePath + "���ɳ����쳣��", ex));
                return false;
            }
        }

        /// <summary>
        /// ȥ����Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string RemoveRootPath(string path)
        {
            try
            {
                return path.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/").Replace("//", "/");
            }
            catch
            {
                return path;
            }
        }

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
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
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
                SaveFile(GetRemotePageString(templatePath, Encoding.UTF8, validateString), savePath, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
                return false;
            }
        }

        /// <summary>
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="query">��ѯ�ַ���</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="inEncoding">ģ��ҳ�����</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
        /// <param name="validateString">��֤�ַ���</param>
        public static bool CreateLocalPage(string templatePath, string query, string savePath, string validateString, Encoding inEncoding, Encoding outEncoding)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, query, inEncoding, validateString), savePath, outEncoding);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
                return false;
            }
        }

        /// <summary>
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="query">��ѯ�ַ���</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="validateString">��֤�ַ���</param>
        public static bool CreateLocalPage(string templatePath, string query, string savePath, string validateString)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, query, Encoding.UTF8, validateString), savePath, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
                return false;
            }
        }

        /// <summary>
        /// ���ɱ���ҳ��
        /// </summary>
        /// <param name="templatePath">ģ���ļ�·������:/Default.aspx</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="validateString">��֤�ַ���</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
        /// <param name="validateString">��֤�ַ���</param>
        public static bool CreateLocalPage(string templatePath, string savePath, string validateString, Encoding inEncoding, Encoding outEncoding)
        {
            try
            {
                SaveFile(GetLocalPageString(templatePath, null, inEncoding, validateString), savePath, outEncoding);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
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
                SaveFile(GetLocalPageString(templatePath, null, Encoding.UTF8, validateString), savePath, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                SaveError(new StaticPageException(string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath), ex));
                return false;
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
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                try
                {
                    var page = templatePath.Replace("/", "\\").TrimStart('\\');
                    HttpRuntime.ProcessRequest(new EncodingWorkerRequest(page, query, sw, encoding));
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
            }

            string content = sb.ToString();

            //��֤�ַ���
            if (string.IsNullOrEmpty(validateString))
            {
                throw new WebException("ִ�б���ҳ��" + templatePath + (query == null ? "" : "?" + query) + "������֤�ַ�������Ϊ�ա�");
            }
            else if (content.IndexOf(validateString) < 0)
            {
                throw new WebException("ִ�б���ҳ��" + templatePath + (query == null ? "" : "?" + query) + "����ҳ�����ݺ���֤�ַ���ƥ��ʧ�ܡ�");
            }

            return content;
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
            //�ж��Ƿ���http://
            if (!templatePath.ToLower().StartsWith("http://"))
            {
                templatePath = "http://" + templatePath;
            }

            //��������
            WebClient wc = new WebClient();
            wc.Encoding = encoding;
            string content = wc.DownloadString(templatePath);

            //��֤�ַ���
            if (string.IsNullOrEmpty(validateString))
            {
                throw new WebException("ִ��Զ��ҳ��" + templatePath + "������֤�ַ�������Ϊ�ա�");
            }
            else if (content.IndexOf(validateString) < 0)
            {
                throw new WebException("ִ��Զ��ҳ��" + templatePath + "����ҳ�����ݺ���֤�ַ���ƥ��ʧ�ܡ�");
            }

            return content;
        }

        /// <summary>
        ///  �����ַ�����·��
        /// </summary>
        /// <param name="content">����ַ���</param>
        /// <param name="savePath">�ļ�����·��</param>
        /// <param name="outEncoding">�ļ�����ҳ�����</param>
        internal static void SaveFile(string content, string savePath, Encoding outEncoding)
        {
            //������д���ļ�
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            //������д���ļ���
            var newSavePath = string.Format("{0}.tmp", savePath);
            File.WriteAllText(newSavePath, content, outEncoding);

            while (true)
            {
                if (File.Exists(newSavePath))
                {
                    try
                    {
                        using (var sr = new StreamReader(newSavePath, outEncoding))
                        using (var sw = new StreamWriter(savePath, false, outEncoding))
                        {
                            sw.Write(sr.ReadToEnd());
                            sw.Flush();
                        }

                        File.Delete(newSavePath);

                        break;
                    }
                    catch
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
                else
                {
                    break;
                }
            }

            //�����ļ��ɹ�д��־
            SaveLog(string.Format("�����ļ���{0}���ɹ���", savePath), LogType.Information);
        }

        /// <summary>
        /// ������־
        /// </summary>
        /// <param name="log"></param>
        internal static void SaveLog(string log, LogType type)
        {
            if (OnLog != null)
            {
                OnLog(log, type);
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="ex"></param>
        internal static void SaveError(StaticPageException ex)
        {
            if (OnError != null)
            {
                try
                {
                    OnError(ex);
                }
                catch (Exception e)
                {
                }
            }
        }

        #endregion
    }


    /// <summary>
    /// ����������
    /// </summary>
    internal class EncodingWorkerRequest : SimpleWorkerRequest
    {
        private TextWriter output;
        private Encoding encoding;

        public EncodingWorkerRequest(string page, string query, TextWriter output)
            : base(page, query, output)
        {
            this.output = output;
            this.encoding = Encoding.UTF8;
        }

        public EncodingWorkerRequest(string page, string query, TextWriter output, Encoding encoding)
            : base(page, query, output)
        {
            this.output = output;
            this.encoding = encoding;
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            output.Write(encoding.GetString(data));
        }
    }
}
