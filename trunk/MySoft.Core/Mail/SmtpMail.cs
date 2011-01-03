using System;
using System.Collections.Generic;
using System.Text;
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
        public string SmtpServer
        {
            get { return smtpServer; }
            set { smtpServer = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string mailFrom;
        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }

        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private int smtpPort;
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        public SmtpMail()
        {
            this.smtpServer = "mail.51shumi.com";
            this.userName = "alicc";
            this.password = "fund123";
            this.mailFrom = "alicc@51shumi.com";
            this.displayName = "数米网";
            this.smtpPort = 25;
        }

        public SmtpMail(string smtpServer, string mailFrom, string userName, string password, string displayName)
        {
            this.smtpServer = smtpServer;
            this.userName = userName;
            this.password = password;
            this.mailFrom = mailFrom;
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
        public bool Send(string title, string body, string mailTo)
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
        public bool Send(string title, string body, string[] mailTo)
        {
            body += "<br><br>系统邮件，请勿直接回复！";
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
            body += "<br><br>系统邮件，请勿直接回复！";
            SMTP smtp = new SMTP(this.mailFrom, mailTo, title, body, this.smtpServer, userName, password);
            smtp.SMTPPort = this.smtpPort;
            smtp.MailDisplyName = this.displayName;
            smtp.IsBodyHtml = true;

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
        public bool SendException(Exception ex, string title, string mailTo)
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
        public bool SendSampleException(Exception ex, string title, string mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            return Send(title, msg, mailTo);
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
        public bool SendException(Exception ex, string title, string[] mailTo)
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
        public bool SendSampleException(Exception ex, string title, string[] mailTo)
        {
            string msg = ErrorHelper.GetErrorWithoutHtml(ex);
            return Send(title, msg, mailTo);
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
