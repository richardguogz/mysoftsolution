using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace LiveChat.Web
{
    public partial class Download : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果出错则直接退出
            if (string.IsNullOrEmpty(Request["filePath"]))
            {
                Response.Write("传入的参数错误！");
                Response.End();
            }
            string filePath = Request["filePath"];
            //filePath = System.Text.Encoding.Default.GetString(Convert.FromBase64String(filePath));

            filePath = Server.MapPath(filePath);
            string fileName = Path.GetFileName(filePath);

            #region 判断文件是否存在

            //创建文件并写入文件流5
            if (!File.Exists(filePath))
            {
                Response.Write("找不到相应的文件，请重新生成！");
                Response.End();
            }

            FileInfo info = new FileInfo(filePath);

            Response.Clear(); //清除缓冲区流中的所有内容输出 
            Response.ClearHeaders(); //清除缓冲区流中的所有头 
            Response.Buffer = false; //设置缓冲输出为false 

            //设置输出流的 HTTP MIME 类型为application/octet-stream 
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.Default;

            //将 HTTP 头添加到输出流 
            if (Request.Browser.Browser.Contains("IE"))
            {
                string ext = fileName.Substring(fileName.LastIndexOf('.'));
                string name = fileName.Remove(fileName.Length - ext.Length);
                //name = name.Replace(".", "%2e").Replace("+", "%20");
                fileName = name + ext;
                Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + "\"");
            }
            else
            {
                Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + info.Name + "\"");
            }
            Response.AppendHeader("Content-Length", info.Length.ToString());

            //将指定的文件直接写入 HTTP 内容输出流。 
            Response.WriteFile(info.FullName);
            Response.Flush(); //向客户端发送当前所有缓冲的输出 
            Response.End(); //将当前所有缓冲的输出发送到客户端 

            #endregion
        }
    }
}
