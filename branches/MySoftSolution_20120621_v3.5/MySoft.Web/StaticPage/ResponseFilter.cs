using System;
using System.IO;
using System.Text;
using System.Web;

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

            //���ҳ�������а���ָ������֤�ַ���������
            if (string.IsNullOrEmpty(validateString) || content.Contains(validateString))
            {
                //���ݽ��б��봦��
                string dynamicurl = path;
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

                //������д���ļ�
                StaticPageManager.SaveFile(content, filePath, encoding);
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
