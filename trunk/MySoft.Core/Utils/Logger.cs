using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MySoft.Core
{
    /// <summary>
    /// 简单日志管理类(按日期生成文件)
    /// </summary>
    public class Logger
    {
        public static readonly Logger Instance = new Logger(AppDomain.CurrentDomain.BaseDirectory);
        private static readonly object syncobj = new object();

        private string dir;
        /// <summary>
        /// 实例化简单日志组件
        /// </summary>
        /// <param name="dir">日志存储根目录，下面会自动创建Log与ErrorLog文件夹</param>
        public Logger(string dir)
        {
            this.dir = dir;
        }

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

                    log = string.Format("{0} -> {1}{2}---------------------------------------------------------------------------------------------------------------{2}{2}",
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
                    string log = ErrorHelper.GetErrorWithoutHtml(ex);

                    string filePath = Path.Combine(dir, "ErrorLog");
                    string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    log = string.Format("{0} -> {1}{2}---------------------------------------------------------------------------------------------------------------{2}{2}",
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
        public void WriteLogWithSendMail(string log, params string[] mailTo)
        {
            lock (syncobj)
            {
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
        public void WriteLogWithSendMail(Exception ex, params string[] mailTo)
        {
            lock (syncobj)
            {
                try
                {
                    WriteLog(ex);

                    string log = ErrorHelper.GetHtmlError(ex);
                    string title = string.Format("异常日志邮件-由客户端【{0}({1})】发出，{2}", DnsHelper.GetHostName(), DnsHelper.GetIPAddress(), ex.Message);

                    MySoft.Mail.SmtpMail.Instance.SendAsync(title, log, mailTo);
                }
                catch { }
            }
        }
    }
}
