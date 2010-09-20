using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace LiveChat.Web
{
    public partial class Upload : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files.Count == 0) Response.End();
            HttpPostedFile file = Request.Files[0];
            int fileLength = file.ContentLength;
            byte[] buffer = new byte[fileLength];
            file.InputStream.Read(buffer, 0, fileLength);

            string fileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(fileName).ToLower();

            if (buffer.Length > 4 * 1024 * 1024)
            {
                Response.Write(ClientScriptFactory.WrapScriptTag("parent.parent.writeError('传送的文件大小不能超过4M！');"));
            }
            else
            {
                //如果是图片则用上传图片的方法
                if (extension == ".jpg" || extension == ".gif" || extension == ".bmp" || extension == ".png")
                {
                    string[] imageUrl = service.UploadImage(buffer, extension);

                    Response.Write(ClientScriptFactory.WrapScriptTag("parent.parent.writeFile('" + imageUrl[0] + "','" + imageUrl[1] + "');"));
                }
                else
                {
                    string fileURL = service.UploadFile(buffer, extension);

                    Response.Write(ClientScriptFactory.WrapScriptTag("parent.parent.writeFile('" + fileName.Replace("\\", "\\\\") + "','" + fileURL + "',true);"));
                }
            }

            Response.End();
        }
    }
}
