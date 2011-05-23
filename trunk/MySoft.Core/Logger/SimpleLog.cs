using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MySoft.Logger
{
    /// <summary>
    /// 简单日志管理类(按日期生成文件)
    /// </summary>
    public class SimpleLog
    {
        /// <summary>
        /// 简单日志的单例
        /// </summary>
        public static readonly SimpleLog Instance = new SimpleLog(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly object syncobj = new object();

        private string dir;
        /// <summary>
        /// 实例化简单日志组件
        /// </summary>
        /// <param name="dir">日志存储根目录，下面会自动创建Log与ErrorLog文件夹</param>
        public SimpleLog(string dir)
        {
            this.dir = dir;
        }

        #region 自动创建文件

        /// <summary>
        /// 写入日志
        /// </summary>
        public void WriteLog(string log)
        {
            lock (syncobj)
            {
                try
                {
                    string filePath = Path.Combine(dir, "Log");
                    string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    log = string.Format("{0} -> {1}{2}==============================================================================================================={2}{2}",
                       DateTime.Now.ToLongTimeString(), log, Environment.NewLine);

                    File.AppendAllText(logFileName, log);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="ex"></param>
        public void WriteLog(Exception ex)
        {
            lock (syncobj)
            {
                try
                {
                    string filePath = Path.Combine(dir, "ErrorLog");
                    string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string log = ErrorHelper.GetErrorWithoutHtml(ex);

                    log = string.Format("{0} -> {1}{2}==============================================================================================================={2}{2}",
                        DateTime.Now.ToLongTimeString(), log, Environment.NewLine);

                    File.AppendAllText(logFileName, log);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写日志并发送邮件
        /// </summary>
        /// <param name="log"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string log, string mailTo)
        {
            WriteLogWithSendMail(log, new string[] { mailTo });
        }

        /// <summary>
        /// 写日志并发送邮件
        /// </summary>
        /// <param name="log"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string log, string[] mailTo)
        {
            lock (syncobj)
            {
                if (mailTo == null || mailTo.Length == 0)
                {
                    throw new Exception("请传入收件人地址信息参数！");
                }

                try
                {
                    WriteLog(log);

                    string title = string.Format("普通日志邮件-由客户端【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress());
                    MySoft.Mail.SmtpMail.Instance.SendAsync(title, log, mailTo);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写错误日志并发送邮件
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(Exception ex, string mailTo)
        {
            WriteLogWithSendMail(ex, new string[] { mailTo });
        }

        /// <summary>
        /// 写错误日志并发送邮件
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(Exception ex, string[] mailTo)
        {
            lock (syncobj)
            {
                if (mailTo == null || mailTo.Length == 0)
                {
                    throw new Exception("请传入收件人地址信息参数！");
                }

                try
                {
                    WriteLog(ex);

                    string title = string.Format("异常日志邮件-由客户端【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress());
                    MySoft.Mail.SmtpMail.Instance.SendExceptionAsync(ex, title, mailTo);
                }
                catch { }
            }
        }

        #endregion

        #region 传入文件信息

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        public void WriteLog(string fileName, string log)
        {
            lock (syncobj)
            {
                try
                {
                    string logFileName = Path.Combine(dir, fileName);
                    //string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                    if (!Directory.Exists(Path.GetDirectoryName(logFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
                    }

                    log = string.Format("{0} -> {1}{2}==============================================================================================================={2}{2}",
                       DateTime.Now.ToLongTimeString(), log, Environment.NewLine);

                    File.AppendAllText(logFileName, log);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ex"></param>
        public void WriteLog(string fileName, Exception ex)
        {
            lock (syncobj)
            {
                try
                {
                    string logFileName = Path.Combine(dir, fileName);
                    //string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                    if (!Directory.Exists(Path.GetDirectoryName(logFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
                    }

                    string log = ErrorHelper.GetErrorWithoutHtml(ex);

                    log = string.Format("{0} -> {1}{2}==================================================================================================================================={2}{2}",
                        DateTime.Now.ToLongTimeString(), log, Environment.NewLine);

                    File.AppendAllText(logFileName, log);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写日志并发送邮件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string fileName, string log, string mailTo)
        {
            WriteLogWithSendMail(fileName, log, new string[] { mailTo });
        }

        /// <summary>
        /// 写日志并发送邮件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string fileName, string log, string[] mailTo)
        {
            lock (syncobj)
            {
                if (mailTo == null || mailTo.Length == 0)
                {
                    throw new Exception("请传入收件人地址信息参数！");
                }

                try
                {
                    WriteLog(fileName, log);

                    string title = string.Format("普通日志邮件-由客户端【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress());
                    MySoft.Mail.SmtpMail.Instance.SendAsync(title, log, mailTo);
                }
                catch { }
            }
        }

        /// <summary>
        /// 写错误日志并发送邮件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ex"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string fileName, Exception ex, string mailTo)
        {
            WriteLogWithSendMail(fileName, ex, new string[] { mailTo });
        }

        /// <summary>
        /// 写错误日志并发送邮件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ex"></param>
        /// <param name="mailTo"></param>
        public void WriteLogWithSendMail(string fileName, Exception ex, string[] mailTo)
        {
            lock (syncobj)
            {
                if (mailTo == null || mailTo.Length == 0)
                {
                    throw new Exception("请传入收件人地址信息参数！");
                }

                try
                {
                    WriteLog(fileName, ex);

                    string title = string.Format("异常日志邮件-由客户端【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress());
                    MySoft.Mail.SmtpMail.Instance.SendExceptionAsync(ex, title, mailTo);
                }
                catch { }
            }
        }

        #endregion
    }
}
