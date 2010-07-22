using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

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
        private string validateString = string.Empty;
        private StringBuilder pageContent = new StringBuilder();
        private Encoding enc;

        public ResponseFilter(Stream sink, string filePath, string validateString)
        {
            this.m_sink = sink;
            this.filePath = filePath;
            this.validateString = validateString;

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

            //��ȡҳ�������
            string content = pageContent.ToString();

            //���ҳ�������а���ָ������֤�ַ���������
            if (content.Contains(validateString) && string.IsNullOrEmpty(validateString))
            {
                //����ļ��в����ڣ��򴴽�
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                if (fs != null)
                {
                    fs.SetLength(0);

                    //��д��ʱ�Ŵ���
                    if (fs.CanWrite)
                    {
                        //���ݽ��б��봦��
                        enc = Encoding.GetEncoding(HttpContext.Current.Response.Charset);

                        byte[] _buffer = enc.GetBytes(content);
                        int _count = enc.GetByteCount(content);

                        //������д�뾲̬�ļ�.
                        fs.Write(_buffer, 0, _count);
                    }

                    //�ر���
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
            if (HttpContext.Current.Response.ContentType == "text/html")
            {
                //�����ж���û��ϵͳ����
                if (HttpContext.Current.Error == null)
                {
                    //���ݽ��б��봦��
                    string content = enc.GetString(buffer, offset, count);

                    pageContent.Append(content);
                }
            }

            //Write out the response to the browser.
            this.m_sink.Write(buffer, offset, count);
        }
    }
}
