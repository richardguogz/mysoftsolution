using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using System.Configuration;

namespace MySoft.IoC.Service
{
    /// <summary>
    /// Windows服务
    /// </summary>
    public class WindowsService : IServiceRun
    {
        private readonly object syncobj = new object();
        private CastleServiceConfiguration config;
        private StartMode startMode = StartMode.Service;
        private CastleService server;
        private string[] mailTo;

        public WindowsService()
        {
            this.config = CastleServiceConfiguration.GetConfig();
            this.server = new CastleService(config);

            this.server.OnLog += new LogEventHandler(server_OnLog);
            this.server.OnError += new ErrorLogEventHandler(server_OnError);

            //处理邮件地址
            string address = ConfigurationManager.AppSettings["SendMailAddress"];
            if (string.IsNullOrEmpty(address)) mailTo = new string[] { "maoyong@fund123.cn" };
            else mailTo = address.Split(',', ';', '|');
        }

        #region IServiceRun 成员

        /// <summary>
        /// 设置运行类型
        /// </summary>
        public StartMode StartMode
        {
            get
            {
                return this.startMode;
            }
            set
            {
                this.startMode = value;
            }
        }

        public void Start()
        {
            if (startMode == StartMode.Console)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] => Service ready started...", DateTime.Now);

                server.Start();

                Console.WriteLine("[{0}] => Server host: {1}", DateTime.Now, server.ServerUrl);
                Console.WriteLine("[{0}] => Max connect: {1} -> Max Buffer: {2} bytes", DateTime.Now, server.MaxConnect, server.MaxBuffer);
                Console.WriteLine("[{0}] => Logger status: On  -> Log time: {1} ms", DateTime.Now, config.LogTime);
                Console.WriteLine("[{0}] => Press enter to exit and stop service...", DateTime.Now);
            }
            else
            {
                server.Start();
            }
        }

        public void Stop()
        {
            if (startMode == StartMode.Console)
            {
                server.Stop();
            }
            else
            {
                server.Stop();
            }
        }

        #endregion

        void server_OnLog(string log, LogType type)
        {
            lock (syncobj)
            {
                if (startMode == StartMode.Console)
                {
                    string message = string.Format("[{0}] => ({1}) {2}", DateTime.Now, type, log);
                    if (type == LogType.Error)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (type == LogType.Warning)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                }
                else
                {
                    if (type == LogType.Warning)
                    {
                        string message = string.Format("({0}) {1}", type, log);
                        SimpleLog.Instance.WriteLogWithSendMail(message, mailTo);
                    }
                }
            }
        }

        void server_OnError(Exception exception)
        {
            lock (syncobj)
            {
                if (startMode == StartMode.Console)
                {
                    string message = string.Format("[{0}] => {1}", DateTime.Now, exception.ToString());
                    if (exception.InnerException != null)
                    {
                        message += "\r\n" + exception.InnerException.ToString();
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                }
                else
                {
                    SimpleLog.Instance.WriteLogWithSendMail(exception, mailTo);
                }
            }
        }
    }
}
