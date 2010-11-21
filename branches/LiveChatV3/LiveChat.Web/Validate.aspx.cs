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
using System.Drawing;

namespace LiveChat.Web
{
    public partial class validate : Page
    {
        private static string CodeTable = "0123456789";
        //abcdefghijklmnopqrstuvwxyz

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = GetRandomCode(4);

            Session["ValidateCode"] = code;

            DrawCode(code);
        }

        private string GetRandomCode(int len)
        {
            Random r = new Random();

            string code = "";
            for (int i = 0; i < len; i++)
            {
                int num = r.Next() % CodeTable.Length;
                code += CodeTable[num];
            }
            return code;
        }

        private void DrawCode(string code)
        {
            Bitmap image = new Bitmap(code.Length * 15, 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random random = new Random();

                // 清空图片背景色
                g.Clear(Color.White);

                // 画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                // 画验证码
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                Font[] fonts = 
                {
                    new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic),
                    new Font("宋体", 14, FontStyle.Bold),
                    new Font("黑体", 14, FontStyle.Bold)
                };

                int x = 2;
                int y = 2;
                for (int i = 0; i < code.Length; i++)
                {
                    int fIndex = random.Next() % fonts.Length;
                    g.DrawString(code[i].ToString(), fonts[fIndex], brush, x, y);
                    x += 13;
                }

                // 画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    x = random.Next(image.Width);
                    y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                // 画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
