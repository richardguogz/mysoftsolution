using System;
using System.Web;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace LiveChat.Client
{
    /// <summary>
    /// updater 的摘要说明。
    /// </summary>
    public class AppUpdater
    {
        #region 成员与字段属性

        private string _updaterUrl;

        public string UpdaterUrl
        {
            set { _updaterUrl = value; }
            get { return this._updaterUrl; }
        }

        #endregion

        /// <summary>
        /// AppUpdater构造函数
        /// </summary>
        public AppUpdater() { }

        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <returns></returns>
        public bool CheckForUpdate(out string currVersion)
        {
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            string serverXmlFile = string.Empty;
            XmlFiles updaterXmlFiles = null;
            currVersion = null;

            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch
            {
                return false;
            }

            //获取服务器地址
            string updateUrl = updaterXmlFiles.GetNodeValue("//Url");

            this.UpdaterUrl = Path.Combine(updateUrl, "UpdateList.xml");

            string tempUpdatePath = null;
            //与服务器连接,下载更新配置文件
            try
            {
                tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + ".tmp" + "\\";
                DownAutoUpdateFile(tempUpdatePath);
            }
            catch
            {
                return false;
            }

            //获取更新文件列表
            Hashtable htUpdateFile = new Hashtable();

            serverXmlFile = Path.Combine(tempUpdatePath, "UpdateList.xml");
            if (!File.Exists(serverXmlFile))
            {
                return false;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");//5+1+a+s+p+x
            Hashtable updateFileList = new Hashtable();

            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();
                currVersion = newVer;

                ArrayList oldFileAl = new ArrayList();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);

                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }

            }
            return k > 0;
        }

        /// <summary>
        /// 返回下载更新文件的临时目录
        /// </summary>
        /// <returns></returns>
        private void DownAutoUpdateFile(string downpath)
        {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);
            string serverXmlFile = Path.Combine(downpath, "UpdateList.xml");

            try
            {
                WebRequest req = WebRequest.Create(this.UpdaterUrl);
                WebResponse res = req.GetResponse();
                if (res.ContentLength > 0)
                {
                    try
                    {
                        WebClient wClient = new WebClient();
                        wClient.DownloadFile(this.UpdaterUrl, serverXmlFile);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            catch
            {
                return;
            }
            //return tempPath;
        }
    }
}
