using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LiveChat.Utils
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class Logger
    {
        public static readonly Logger Instance = new Logger();
        private static readonly object syncobj = new object();

        /// <summary>
        /// 写入到日志
        /// </summary>
        public void WriteLog(string log)
        {
            lock (syncobj)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                string logFileName = Path.Combine(filePath, string.Format("{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                log = string.Format("{0} -> {1}{2}", DateTime.Now.ToLongTimeString(), log, Environment.NewLine);
                File.AppendAllText(logFileName, log);
            }
        }
    }
}
