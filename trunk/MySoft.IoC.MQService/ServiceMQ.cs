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

namespace MySoft.IoC.MQService
{
    public partial class ServiceMQ : ServiceBase
    {
        private static string[] mailTo;
        public ServiceMQ()
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


            SimpleLog.Instance.WriteLog("正在启动消息队列服务......");

            SimpleLog.Instance.WriteLog("Service MQ Server started...");
            try
            {
                CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();

                MemoryServiceMQ mq = new MemoryServiceMQ();
                //mq.OnLog += new LogHandler(mq_OnLog);
                mq.OnError += new ErrorLogEventHandler(mq_OnError);

                CastleConfigService rh = new CastleConfigService(config);
                rh.OnLog += new LogEventHandler(rh_OnLog);
                rh.PublishWellKnownServiceInstance(mq);

                SimpleLog.Instance.WriteLog("Logger Status: On");
                SimpleLog.Instance.WriteLog("Service MQ Server start success!");
            }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLog("Service MQ Server start error: " + ex.Message);
            }
        }

        void rh_OnLog(string logMsg)
        {
            SimpleLog.Instance.WriteLog(logMsg);
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
