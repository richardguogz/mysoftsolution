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
        private FileStream fs;
        private string filePath = string.Empty;

        public ResponseFilter(Stream sink, string filePath)
        {
            this.m_sink = sink;
            this.filePath = filePath;
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
            this.fs.Close();
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
            if (HttpContext.Current.Response.ContentType == "text/html")
            {
                //首先判断有没有系统错误
                if (HttpContext.Current.Error == null)
                {
                    try
                    {
                        if (fs == null)
                        {
                            //如果文件夹不存在，则创建
                            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                            this.fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                            this.fs.SetLength(0);
                        }

                        //内容进行编码处理
                        Encoding enc = Encoding.GetEncoding(HttpContext.Current.Response.Charset);
                        string pageStr = enc.GetString(buffer, offset, count);
                        byte[] _buffer = enc.GetBytes(pageStr);
                        int _count = enc.GetByteCount(pageStr);

                        //将数据写入静态文件.
                        this.fs.Write(_buffer, 0, _count);
                        this.m_sink.Write(_buffer, 0, _count);
                        return;
                    }
                    catch
                    {
                        if (fs != null)
                        {
                            //关闭流
                            this.fs.Close();

                            //删除静态页面
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                                return;
                            }
                        }
                    }
                }
            }

            //Write out the response to the browser.
            this.m_sink.Write(buffer, 0, count);
        }
    }
}
