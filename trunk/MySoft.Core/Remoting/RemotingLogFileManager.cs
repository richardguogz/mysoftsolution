using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// Remoting Service Log File Manager
    /// </summary>
    public class RemotingLogFileManager : MarshalByRefObject
    {
        static string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

        /// <summary>
        /// 
        /// </summary>
        public RemotingLogFileManager()
        {
        }

        /// <summary>
        /// 获取所有日志文件名列表
        /// </summary>
        /// <returns></returns>
        public string[] GetAll()
        {
            string[] filepaths = Directory.GetFiles(logDir);
            string[] filenames = new string[filepaths.Length];

            for (int i = 0; i < filepaths.Length; i++)
            {
                filenames[i] = Path.GetFileName(filepaths[i]);
            }

            return filenames;
        }

        /// <summary>
        /// 获取日志文件内容
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string Get(string filename)
        {
            string filePath = Path.Combine(logDir, filename);
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// 删除日志文件
        /// </summary>
        /// <param name="filename"></param>
        public void Delete(string filename)
        {
            string filePath = Path.Combine(logDir, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
