using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using MySoft.IoC;
using MySoft.Logger;
using System.Configuration;

namespace MySoft.IoC.HostService
{
    public partial class ServiceHost : ServiceBase
    {
        private static string[] mailTo;
        public ServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string address = ConfigurationManager.AppSettings["SendMailAddress"];
            if (string.IsNullOrEmpty(address))
            {
                mailTo = new string[] { "maoyong@fund123.cn" };
            }
            else
            {
                mailTo = address.Split(',', ';', '|');
            }

            SimpleLog.Instance.WriteLog("正在启动宿主服务程序......");

            SimpleLog.Instance.WriteLog("Service host started...");
            try
            {
                CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();

                //CastleFactory.Create().OnLog += new LogHandler(mq_OnLog);
                CastleFactory.Create().OnError += new ErrorLogEventHandler(mq_OnError);

                SimpleLog.Instance.WriteLog("Logger Status: On");
                SimpleLog.Instance.WriteLog("Service host start success!");
            }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLog(new Exception("Service host start error: " + ex.Message, ex));
            }
        }

        static void mq_OnError(Exception exception)
        {
            SimpleLog.Instance.WriteLog(exception);

            if (exception is IoCException)
            {
                var ex = exception as IoCException;

                if (string.IsNullOrEmpty(ex.ExceptionHeader))
                    SendMail(ex, "调用IoC服务产生异常，请查证！");
                else
                    SendMail(ex, "调用IoC服务产生异常，请查证！" + ex.ExceptionHeader);
            }
        }

        //static void mq_OnLog(string logMsg)
        //{
        //    SimpleLog.Instance.Write(logMsg);
        //}

        static void SendMail(Exception ex, string title)
        {
            try
            {
                MySoft.Mail.SmtpMail.Instance.SendExceptionAsync(ex, title, mailTo);
            }
            catch { }
        }
    }
}
