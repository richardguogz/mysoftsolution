using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MySoft.Mail;

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
        /// 写错误日志
        /// </summary>
        /// <param name="ex"></param>
        public void WriteLog(Exception ex)
        {
            string filePath = Path.Combine(dir, "ErrorLog");
            filePath = Path.Combine(filePath, ex.GetType().Name);
            string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

            WriteLog(logFileName, ex);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        public void WriteLog(string log)
        {
            string filePath = Path.Combine(dir, "Log");
            string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

            WriteLog(logFileName, log);
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
            WriteLog(log);
            SendMail(log, mailTo);
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
            WriteLog(ex);
            SendMail(ex, mailTo);
        }

        #endregion

        #region 传入文件信息

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ex"></param>
        public void WriteLog(string fileName, Exception ex)
        {
            string log = ErrorHelper.GetErrorWithoutHtml(ex);
            WriteLog(fileName, log);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        public void WriteLog(string fileName, string log)
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
            WriteLog(fileName, log);
            SendMail(log, mailTo);
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
            WriteLog(fileName, ex);
            SendMail(ex, mailTo);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="log"></param>
        /// <param name="to"></param>
        private void SendMail(string log, string[] to)
        {
            if (to == null || to.Length == 0)
            {
                throw new ArgumentException("请传入收件人地址信息参数！");
            }

            var body = CoreHelper.GetSubString(log, 100, "...");
            string title = string.Format("{2} - 普通邮件由【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress(), body);
            SmtpMail.Instance.SendAsync(title, log, to);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="to"></param>
        private void SendMail(Exception ex, string[] to)
        {
            if (to == null || to.Length == 0)
            {
                throw new ArgumentException("请传入收件人地址信息参数！");
            }

            var body = CoreHelper.GetSubString(ex.ToString(), 100, "...");
            string title = string.Format("({2}){3} - 异常邮件由【{0}({1})】发出", DnsHelper.GetHostName(), DnsHelper.GetIPAddress(), ex.GetType().Name, body);
            SmtpMail.Instance.SendExceptionAsync(ex, title, to);
        }

        #endregion
    }
}
