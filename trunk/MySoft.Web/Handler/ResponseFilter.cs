using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace MySoft.Web
{
    /// <summary>
    /// 生成htm静态页面
    /// </summary>
    internal class ResponseFilter : Stream
    {
        private Stream m_sink;
        private long m_position;
        private string filePath = string.Empty;
        private string validateString = string.Empty;
        private StringBuilder pageContent = new StringBuilder();
        private Encoding enc;

        public ResponseFilter(Stream sink, string filePath, string validateString)
        {
            this.m_sink = sink;
            this.filePath = filePath;
            this.validateString = validateString;

            //获取当前流的编码
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

            //获取页面的内容
            string content = pageContent.ToString();

            //如果页面内容中包含指定的验证字符串则生成
            if (string.IsNullOrEmpty(validateString) || content.Contains(validateString))
            {
                //如果文件夹不存在，则创建
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                if (fs != null)
                {
                    fs.SetLength(0);

                    //能写入时才处理
                    if (fs.CanWrite)
                    {
                        //内容进行编码处理
                        enc = Encoding.GetEncoding(HttpContext.Current.Response.Charset);

                        string dynamicurl = HttpContext.Current.Request.Url.PathAndQuery;
                        string staticurl = filePath;

                        string extension = Path.GetExtension(staticurl);
                        if (extension != null && extension.ToLower() == ".js")
                        {
                            //加入静态页生成元素
                            content = string.Format("{3}\r\n\r\n//<!-- 生成方式：被动生成 -->\r\n//<!-- 更新时间：{0} -->\r\n//<!-- 动态URL：{1} -->\r\n//<!-- 静态URL：{2} -->",
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }
                        else
                        {
                            //加入静态页生成元素
                            content = string.Format("{3}\r\n\r\n<!-- 生成方式：被动生成 -->\r\n<!-- 更新时间：{0} -->\r\n<!-- 动态URL：{1} -->\r\n<!-- 静态URL：{2} -->",
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }

                        byte[] _buffer = enc.GetBytes(content);
                        int _count = enc.GetByteCount(content);

                        //将数据写入静态文件.
                        fs.Write(_buffer, 0, _count);
                    }

                    //关闭流
                    fs.Close();
                }
            }
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
            //首先判断有没有系统错误
            if (HttpContext.Current.Error == null)
            {
                //内容进行编码处理
                string content = enc.GetString(buffer, offset, count);

                pageContent.Append(content);
            }

            //Write out the response to the browser.
            this.m_sink.Write(buffer, offset, count);
        }

        /// <summary>
        /// 去除根目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveRootPath(string path)
        {
            return path.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/");
        }
    }
}
