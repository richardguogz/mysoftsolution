using System;
using System.Threading;
using System.Web;

namespace MySoft.Mail
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class SmtpMail
    {
        public static readonly SmtpMail Instance = new SmtpMail();

        private string smtpServer;
        /// <summary>
        /// 邮件发送服务器
        /// </summary>
        public string SmtpServer
        {
            get { return smtpServer; }
            set { smtpServer = value; }
        }

        private string mailFrom;
        private string userName;
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string UserName
        {
            get
            {
                return mailFrom;
            }
            set
            {
                mailFrom = value;
                userName = value.Split('@')[0];
            }
        }

        private string password;
        /// <summary>
        /// 发件人密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string displayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private int smtpPort;
        /// <summary>
        /// 邮件端口
        /// </summary>
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        private bool isSystemMail;
        /// <summary>
        /// 是否系统邮件
        /// </summary>
        public bool IsSystemMail
        {
            get { return isSystemMail; }
            set { isSystemMail = value; }
        }

        public SmtpMail()
        {
            this.smtpServer = "smtp.163.com";
            this.UserName = "mysoft2011@163.com";
            this.password = "mysoft";
            this.displayName = "MySoft开发组";
            this.smtpPort = 25;
            this.isSystemMail = true;
        }

        public SmtpMail(string smtpServer, string userName, string password, string displayName)
        {
            this.smtpServer = smtpServer;
            this.UserName = userName;
            this.password = password;
            this.displayName = displayName;
            this.smtpPort = 25;
        }

        /// <summary>
        /// 发送邮件到指定邮箱
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult Send(string title, string body, string mailTo)
        {
            string[] mailto = new string[] { mailTo };
            return Send(title, body, mailto);
        }

        /// <summary>
        /// 发送邮件到指定邮箱
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendAsync(string title, string body, string mailTo)
        {
            string[] mailto = new string[] { mailTo };
            SendAsync(title, body, mailto);
        }

        /// <summary>
        /// 发送邮件到指定邮箱
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult Send(string title, string body, string[] mailTo)
        {
            if (isSystemMail) body += "<br><br>系统邮件，请勿直接回复！";
            SMTP smtp = new SMTP(this.mailFrom, mailTo, title, body, this.smtpServer, userName, password);
            smtp.SMTPPort = this.smtpPort;
            smtp.MailDisplyName = this.displayName;
            smtp.IsBodyHtml = true;

            return smtp.Send();
        }

        /// <summary>
        /// 发送邮件到指定邮箱
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendAsync(string title, string body, string[] mailTo)
        {
            if (isSystemMail) body += "<br><br>系统邮件，请勿直接回复！";
            SMTP smtp = new SMTP(this.mailFrom, mailTo, title, body, this.smtpServer, userName, password);
            smtp.SMTPPort = this.smtpPort;
            smtp.MailDisplyName = this.displayName;
            smtp.IsBodyHtml = true;

            //启用线程池来实现异步发送
            ThreadPool.QueueUserWorkItem(DoSend, smtp);
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        void DoSend(object sender)
        {
            if (sender == null) return;
            SMTP smtp = sender as SMTP;
            smtp.SendAsync(null);
        }

        #region 发送错误

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendException(Exception ex, string title, string mailTo)
        {
            string msg = ErrorHelper.GetHtmlError(ex);
            return Send(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendSampleException(Exception ex, string title, string mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            return Send(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="current"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendException(HttpContext current, string title, string mailTo)
        {
            HttpContext ctx = HttpContext.Current;
            Exception ex = ctx.Server.GetLastError();

            return SendException(ex, title, mailTo);
        }

        #endregion

        #region 发送错误(多发件人)

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendException(Exception ex, string title, string[] mailTo)
        {
            string msg = ErrorHelper.GetHtmlError(ex);
            return Send(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendSampleException(Exception ex, string title, string[] mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            return Send(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="current"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public ResponseResult SendException(HttpContext current, string title, string[] mailTo)
        {
            HttpContext ctx = HttpContext.Current;
            Exception ex = ctx.Server.GetLastError();

            return SendException(ex, title, mailTo);
        }

        #endregion

        #region 异步发送错误邮件

        #region 发送错误

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendExceptionAsync(Exception ex, string title, string mailTo)
        {
            string msg = ErrorHelper.GetHtmlError(ex);
            SendAsync(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendSampleExceptionAsync(Exception ex, string title, string mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            SendAsync(title, msg, mailTo);
        }

        #endregion

        #region 发送错误(多发件人)

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendExceptionAsync(Exception ex, string title, string[] mailTo)
        {
            string msg = ErrorHelper.GetHtmlError(ex);
            SendAsync(title, msg, mailTo);
        }

        /// <summary>
        /// 发送错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="mailTo"></param>
        /// <returns></returns>
        public void SendSampleExceptionAsync(Exception ex, string title, string[] mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            SendAsync(title, msg, mailTo);
        }

        #endregion

        #endregion
    }
}
