using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using MySoft.Web.Configuration;

namespace MySoft.Web
{
    /// <summary>
    /// ����htm��̬ҳ��
    /// </summary>
    internal class ResponseFilter : Stream
    {
        private Stream m_sink;
        private long m_position;
        private string filePath = string.Empty;
        private StaticPageRule rule;
        private UpdateRuleCollection updates;
        private StringBuilder pageContent = new StringBuilder();
        private Encoding enc;

        public ResponseFilter(Stream sink, string filePath, StaticPageRule rule, UpdateRuleCollection updates)
        {
            this.m_sink = sink;
            this.filePath = filePath;
            this.rule = rule;
            this.updates = updates;

            //��ȡ��ǰ���ı���
            enc = Encoding.GetEncoding(HttpContext.Current.Response.Charset);
        }

        // The following members of Stream must be overriden.
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return 0;
        }

        public override void SetLength(long length)
        {
            this.m_sink.SetLength(length);
        }

        public override void Close()
        {
            this.m_sink.Close();

            //д�ļ�
            WriteFile(pageContent.ToString());
        }

        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="content"></param>
        private void WriteFile(string content)
        {
            //���ҳ�������а���ָ������֤�ַ���������
            if (string.IsNullOrEmpty(rule.ValidateString) || content.Contains(rule.ValidateString))
            {
                //�滻����
                content = ReplaceContext(content);

                //���ݽ��б��봦��
                string dynamicurl = HttpContext.Current.Request.Url.PathAndQuery;
                string staticurl = filePath;

                string extension = Path.GetExtension(staticurl);
                if (extension != null && extension.ToLower() == ".js")
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n//<!-- ���ɷ�ʽ���������� -->\r\n//<!-- ����ʱ�䣺{0} -->\r\n//<!-- ��̬URL��{1} -->\r\n//<!-- ��̬URL��{2} -->",
                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }
                else
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ���������� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }

                //������ʱ�ļ�
                string tempFile = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".tmpfile");

                //������д���ļ�
                StaticPageManager.SaveFile(content, tempFile, enc);

                try
                {
                    //ɾ��֮ǰ���ļ�
                    if (File.Exists(filePath)) File.Delete(filePath);

                    //���ļ��ƶ�����λ��
                    File.Move(tempFile, filePath);
                }
                catch { }
            }
        }

        private string ReplaceContext(string content)
        {
            List<UpdateRule> list = new List<UpdateRule>();
            if (updates != null && updates.Count > 0)
            {
                foreach (UpdateRule r in updates) list.Add(r);
            }

            if (rule.Updates != null && rule.Updates.Length > 0)
            {
                //�����ɵ����ݽ����滻
                foreach (UpdateRule r in rule.Updates)
                {
                    //�Ե�ǰҳ������Ϊ׼
                    var item = list.Find(p => string.Compare(p.SearchFor, r.SearchFor) == 0);
                    if (item != null) list.Remove(item);
                    list.Add(r);
                }
            }

            //�����ɵ����ݽ����滻
            foreach (UpdateRule r in list)
            {
                string searchFor = RewriterUtils.ResolveUrl(HttpContext.Current.Request.ApplicationPath, r.SearchFor);
                Regex reg = new Regex(searchFor, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                content = reg.Replace(content, r.ReplaceTo);
            }

            return content;
        }

        public override void Flush()
        {
            this.m_sink.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_sink.Read(buffer, offset, count);
        }

        // Override the Write method to filter Response to a file.
        public override void Write(byte[] buffer, int offset, int count)
        {
            //�����ж���û��ϵͳ����
            if (HttpContext.Current.Error == null)
            {
                //���ݽ��б��봦��
                string content = enc.GetString(buffer, offset, count);

                pageContent.Append(content);
            }

            //Write out the response to the browser.
            this.m_sink.Write(buffer, offset, count);
        }

        /// <summary>
        /// ȥ����Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveRootPath(string path)
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
    }
}
