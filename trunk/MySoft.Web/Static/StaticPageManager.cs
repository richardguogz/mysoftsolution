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
using MySoft.Core;
using System.Threading;

namespace MySoft.Web
{
    /// <summary>
    /// ��̬ҳ������
    /// </summary>
    public abstract class StaticPageManager
    {
        private const int INTERVAL = 60000;

        public static event LogEventHandler OnLog;

        public static event ErrorLogEventHandler OnError;

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
        /// <param name="interval">�����ʱ��(Ĭ��Ϊһ����)</param>
        public static void Start(int interval)
        {
            Start(interval, false);
        }

        /// <summary>
        /// ������̬������
        /// </summary>
        /// <param name="interval">�����ʱ��</param>
        public static void Start(double interval, bool isStartUpdate)
        {
            if (isStartUpdate)
            {
                ThreadPool.QueueUserWorkItem(StartUpdate);
            }

            Thread thread = new Thread(DoWork);
            thread.Start(interval);
        }

        //��ʼ����
        static void StartUpdate(object value)
        {
            RunUpdate(DateTime.MaxValue);
        }

        static void RunUpdate(DateTime updateTime)
        {
            lock (staticPageItems)
            {
                foreach (IStaticPageItem sti in staticPageItems)
                {
                    try
                    {
                        //��Ҫ���ɲ������߳�
                        if (sti.NeedUpdate(updateTime))
                        {
                            System.Threading.ThreadPool.QueueUserWorkItem(obj =>
                            {
                                if (obj == null) return;

                                ArrayList arr = obj as ArrayList;
                                IStaticPageItem item = arr[0] as IStaticPageItem;
                                DateTime time = (DateTime)arr[1];
                                item.Update(time);
                            }, new ArrayList { sti, updateTime });
                        }
                    }
                    catch (Exception ex)
                    {
                        var exception = new WebException("ִ��ҳ�����ɳ����쳣��" + ex.Message, ex);
                        if (OnError != null) OnError(exception);
                    }
                }
            }
        }

        //ִ�������¼�
        static void DoWork(object value)
        {
            while (true)
            {
                RunUpdate(DateTime.Now);

                //���߼��
                Thread.Sleep(Convert.ToInt32(value));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
                SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", savePath));
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
            try
            {
                StringBuilder result = new StringBuilder();
                using (StringWriter sw = new StringWriter(result))
                {
                    string path = templatePath.TrimStart('/');
                    HttpRuntime.ProcessRequest(new EncodingWorkerRequest(path, query, sw, encoding));
                }

                string content = result.ToString();
                if (!string.IsNullOrEmpty(validateString))
                {
                    if (content.IndexOf(validateString) >= 0)
                    {
                        return content;
                    }
                    else
                    {
                        throw new Exception("ִ�б���ҳ��" + templatePath + (query == null ? "" : "?" + query) + "����ҳ�����ݺ���֤�ַ���ƥ��ʧ�ܡ�");
                    }
                }

                return content;
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
        /// <param name="log"></param>
        internal static void SaveLog(string log)
        {
            if (OnLog != null)
            {
                OnLog(log);
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="log"></param>
        /// <param name="ex"></param>
        internal static void SaveError(Exception ex, string log)
        {
            if (OnError != null)
            {
                var exception = new WebException(log, ex);
                OnError(exception);
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
        private MemoryStream stream;

        public EncodingWorkerRequest(string page, string query, TextWriter output)
            : base(page, query, output)
        {
            this.output = output;
            this.encoding = Encoding.UTF8;
            this.stream = new MemoryStream();
        }

        public EncodingWorkerRequest(string page, string query, TextWriter output, Encoding encoding)
            : base(page, query, output)
        {
            this.output = output;
            this.encoding = encoding;
            this.stream = new MemoryStream();
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            if (length > 0 && data != null)
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public override void FlushResponse(bool finalFlush)
        {
            if (finalFlush)
            {
                StreamReader sr = new StreamReader(stream, encoding);
                stream.Position = 0;
                stream.Flush();

                string content = sr.ReadToEnd();

                //������д���������
                output.Write(content);
            }
        }
    }
}
