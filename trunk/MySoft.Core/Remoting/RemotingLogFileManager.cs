using System;
using System.IO;

namespace MySoft.Remoting
{
    /// <summary>
    /// �ļ�����Remoting����
    /// </summary>
    [ServiceContract]
    public interface IRemotingLogFileManager
    {
        /// <summary>
        /// ɾ����־�ļ�
        /// </summary>
        /// <param name="filename"></param>
        void Delete(string filename);

        /// <summary>
        /// ��ȡ��־�ļ�����
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string Get(string filename);

        /// <summary>
        /// ��ȡ������־�ļ����б�
        /// </summary>
        /// <returns></returns>
        string[] GetAll();
    }

    /// <summary>
    /// Remoting Service Log File Manager
    /// </summary>
    public class RemotingLogFileManager : MarshalByRefObject, MySoft.Remoting.IRemotingLogFileManager
    {
        static string logDir = CoreHelper.GetFullPath("log");

        /// <summary>
        /// 
        /// </summary>
        public RemotingLogFileManager() { }

        /// <summary>
        /// ��ȡ������־�ļ����б�
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
        /// ��ȡ��־�ļ�����
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string Get(string filename)
        {
            string filePath = Path.Combine(logDir, filename);
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// ɾ����־�ļ�
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
