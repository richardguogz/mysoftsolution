using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting;
using System.Net.Sockets;

namespace LiveChat.Client
{
    /// <summary>
    /// 提供回调的事件
    /// </summary>
    /// <param name="obj"></param>
    public delegate void CallbackEventHandler(object obj);

    /// <summary>
    /// 字体及颜色委托
    /// </summary>
    /// <param name="font"></param>
    /// <param name="color"></param>
    public delegate void CallbackFontColorEventHandler(Font font, Color color);

    /// <summary>
    /// 显示提示委托
    /// </summary>
    /// <param name="tip"></param>
    /// <param name="handler"></param>
    public delegate void ShowTipEventHandler(TipInfo tip, CallbackEventHandler handler);

    /// <summary>
    /// 提示信息
    /// </summary>
    public class TipInfo
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 消息KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public object Target { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClientUtils
    {
        /// <summary>
        /// 软件在线状态
        /// </summary>
        public static bool SoftwareIsOnline = true;

        /// <summary>
        /// 软件其它地方登录
        /// </summary>
        public static bool SoftwareOtherLogin = false;

        //最大会话数
        public const int MaxAcceptCount = 20;

        #region 显示消息

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowError(Exception ex)
        {
            if (ex is RemotingException)
            {
                MessageBox.Show("服务端产生异常，请检测服务状态或服务是否为最新版本！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ex is SocketException)
            {
                MessageBox.Show("链接服务器失败，请检测网络配置！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowError(string attachMessage, Exception ex)
        {
            MessageBox.Show(attachMessage + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 退出应用程序
        /// </summary>
        public static void ExitApplication()
        {
            Application.ExitThread();
            Application.Exit();
        }

        #endregion

        /// <summary>   
        /// get a certain rectangle part of a known graphic   
        /// </summary>   
        /// <param name="bitmapPathAndName">path and name of the source graphic</param>   
        /// <param name="width">width of the part graphic</param>   
        /// <param name="height">height of the part graphic</param>   
        /// <param name="offsetX">the width offset in the source graphic</param>   
        /// <param name="offsetY">the height offset in the source graphic</param>   
        /// <returns>wanted graphic</returns>   
        public static Bitmap GetPartOfImage(string bitmapPathAndName, int width, int height, int offsetX, int offsetY)
        {
            return GetPartOfImage(bitmapPathAndName, width, height, offsetX, offsetY, -1, -1);
        }

        /// <summary>   
        /// get a certain rectangle part of a known graphic   
        /// </summary>   
        /// <param name="bitmapPathAndName">path and name of the source graphic</param>   
        /// <param name="width">width of the part graphic</param>   
        /// <param name="height">height of the part graphic</param>   
        /// <param name="offsetX">the width offset in the source graphic</param>   
        /// <param name="offsetY">the height offset in the source graphic</param>   
        /// <returns>wanted graphic</returns>   
        public static Bitmap GetPartOfImage(string bitmapPathAndName, int width, int height, int offsetX, int offsetY, int picWidth, int picHeight)
        {
            Bitmap sourceBitmap = new Bitmap(bitmapPathAndName);
            Bitmap resultBitmap = null;
            if (picWidth > 0)
                resultBitmap = new Bitmap(picWidth, picHeight);
            else
                resultBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                Rectangle resultRectangle = new Rectangle();
                if (picWidth > 0)
                    resultRectangle = new Rectangle(0, 0, picWidth, picHeight);
                else
                    resultRectangle = new Rectangle(0, 0, width, height);

                Rectangle sourceRectangle = new Rectangle(0 + offsetX, 0 + offsetY, width, height);
                g.DrawImage(sourceBitmap, resultRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }
            return resultBitmap;
        }
    }
}
