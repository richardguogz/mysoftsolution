using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using MySoft.Web.Configuration;
using System.Threading;
using System.Collections;

namespace MySoft.Web
{
    /// <summary>
    /// ����htm��̬ҳ��
    /// </summary>
    public class ResponseFilter : AspNetFilter
    {
        private string filePath = string.Empty;
        private string validateString = string.Empty;
        private StringBuilder pageContent = new StringBuilder();
        private Encoding encoding;

        public ResponseFilter(Stream sink, string filePath, string validateString, bool replace, string extension)
            : base(sink, replace, extension)
        {
            this.filePath = filePath;
            this.validateString = validateString;

            //��ȡ��ǰ���ı���
            this.encoding = Encoding.GetEncoding(HttpContext.Current.Response.Charset);
        }

        public override void WriteContent(string content)
        {
            pageContent.Append(content);
        }

        public override void WriteComplete()
        {
            //�滻����
            string content = ReplaceContext(pageContent.ToString());
            string path = HttpContext.Current.Request.Url.PathAndQuery;

            //���������߳�
            ThreadPool.QueueUserWorkItem(WriteFile, new ArrayList { content, path });
        }

        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="state"></param>
        private void WriteFile(object state)
        {
            ArrayList arr = state as ArrayList;
            string content = arr[0].ToString();

            //���ҳ�������а���ָ������֤�ַ���������
            if (string.IsNullOrEmpty(validateString) || content.Contains(validateString))
            {
                //���ݽ��б��봦��
                string dynamicurl = arr[1].ToString();
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
                string fileName = Path.GetFileNameWithoutExtension(filePath) + new Random().Next(1000).ToString("000") + ".tmpfile";
                string tempFile = Path.Combine(Path.GetDirectoryName(filePath), fileName);

                try
                {
                    //���������ʱ�ļ����򷵻�
                    if (File.Exists(tempFile)) return;

                    //������д���ļ�
                    StaticPageManager.SaveFile(content, tempFile, encoding);

                    //ɾ��֮ǰ���ļ�
                    if (File.Exists(filePath)) File.Delete(filePath);

                    //���ļ��ƶ�����λ��
                    File.Move(tempFile, filePath);
                }
                catch
                {
                    //���������ʱ�ļ����򷵻�
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
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
