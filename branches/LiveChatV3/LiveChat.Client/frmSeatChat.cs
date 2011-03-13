using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using System.Runtime.InteropServices;
using LiveChat.Entity;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using CSharpWin;

namespace LiveChat.Client
{
    public partial class frmSeatChat : Form
    {
        public event CallbackFontColorEventHandler CallbackFontColor;

        private const int maxAcceptCount = 20;

        private Color currentColor;
        private Font currentFont;

        private ISeatService service;
        private Company company;
        private Seat fromSeat, toSeat;
        private Timer msgtimer;
        private int messageCount;
        private string memoName;
        private VideoChat chat;

        private string _OpenUser;
        /// <summary>
        /// 打开的用户
        /// </summary>
        public string OpenUser
        {
            get { return _OpenUser; }
            set { _OpenUser = value; }
        }

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        public frmSeatChat(ISeatService service, Company company, Seat fromSeat, Seat toSeat, string memoName, Font useFont, Color useColor)
        {
            this.service = service;
            this.company = company;
            this.fromSeat = fromSeat;
            this.toSeat = toSeat;
            this.memoName = memoName;
            this.currentFont = useFont;
            this.currentColor = useColor;

            InitializeComponent();
        }

        private void frmSeatChat_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(memoName))
                this.Text = string.Format("您与【{0}】聊天中...", toSeat.SeatName);
            else
                this.Text = string.Format("您与【{0}】聊天中...", memoName);

            if (currentFont != null) txtMessage.Font = currentFont;
            if (currentColor != null) txtMessage.ForeColor = currentColor;

            button5.Visible = false;

            var rect = splitContainer2.Panel1.ClientRectangle;
            rect.X = splitContainer1.Width - splitContainer2.Width;

            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Panel2.Hide();
            splitContainer1.SplitterDistance = splitContainer1.Width;

            //获取点击的表情。
            emotionDropdown1.LoadImages(AppDomain.CurrentDomain.BaseDirectory);
            emotionDropdown1.EmotionContainer.ItemClick += new EmotionItemMouseEventHandler(EmotionContainer_ItemClick);

            InitBrowser();

            this.chat = new VideoChat(fromSeat, toSeat, this.Handle, rect);
            this.chat.CreateClient();
            this.chat.LoginToServer();

            //打开视频
            if (!string.IsNullOrEmpty(OpenUser))
            {
                OpenVideo(OpenUser);
            }
        }

        /// <summary>
        /// 初始化浏览器
        /// </summary>
        void InitBrowser()
        {
            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            url = Path.Combine(url, "chat.htm");
            wbChatBox.Url = new Uri(url);
            wbChatBox.AllowNavigation = false;
            //wbChatBox.IsWebBrowserContextMenuEnabled = false;
            wbChatBox.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbChatBox_DocumentCompleted);
        }

        void msgtimer_Tick(object sender, EventArgs e)
        {
            msgtimer.Stop();

            LoadMessage(true);

            msgtimer.Start();
        }

        void wbChatBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LoadMessage(false);

            msgtimer = new Timer();
            msgtimer.Interval = 5000;
            msgtimer.Tick += new EventHandler(msgtimer_Tick);
            msgtimer.Start();

            if (txtMessage.Enabled) txtMessage.Focus();
        }

        private void LoadMessage(bool isflash)
        {
            try
            {
                HtmlElement element = wbChatBox.Document.GetElementById("historyBox");

                IList<Entity.Message> list = service.GetS2SHistoryMessages(fromSeat.SeatID, toSeat.SeatID);
                if (list.Count == 0)
                {
                    element.InnerHtml = string.Empty;
                }
                else
                {
                    if (messageCount != list.Count)
                    {
                        element.InnerHtml = string.Empty;

                        foreach (Entity.Message msg in list)
                        {
                            HtmlElement p = element.Document.CreateElement("p");
                            StringBuilder sb = new StringBuilder();

                            if (msg.SenderID == toSeat.SeatID)
                            {
                                p.SetAttribute("className", "visitor");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append(GetReceiverName(msg.SenderName) + "向您发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                else if (msg.Type == MessageType.File)
                                {
                                    sb.Append(GetReceiverName(msg.SenderName) + "向您传送了一个文件 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                    msg.Content = "点击下载:" + msg.Content;
                                }
                                else
                                    sb.Append(GetReceiverName(msg.SenderName) + "&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }
                            else
                            {
                                p.SetAttribute("className", "operator");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append("您向" + GetReceiverName(((IReceiver)msg).ReceiverName) + "发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                else if (msg.Type == MessageType.File)
                                {
                                    sb.Append("您向" + GetReceiverName(((IReceiver)msg).ReceiverName) + "传送了一个文件 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                    msg.Content = "点击下载:" + msg.Content;
                                }
                                else
                                    sb.Append("您&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }

                            p.InnerHtml = sb.ToString();
                            element.AppendChild(p);
                        }

                        element.ScrollIntoView(false);

                        messageCount = list.Count;

                        if (isflash)
                        {
                            //闪屏处理
                            FlashWindow(this.Handle, true);
                        }
                    }
                }
            }
            catch (SocketException) { }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private string GetReceiverName(string receiverName)
        {
            if (!string.IsNullOrEmpty(memoName))
                return memoName;
            else
                return receiverName;
        }

        //文本格式设置
        private void tsbFont_Click(object sender, EventArgs e)
        {
            Font oldFont = currentFont ?? txtMessage.Font;
            Font newFont = new Font(oldFont.FontFamily, oldFont.Size, oldFont.Style, GraphicsUnit.Point);
            fontDialog1.Font = newFont;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font font = fontDialog1.Font;
                currentFont = new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Pixel);
                txtMessage.Font = currentFont;

                //将当前设置到主窗口
                if (CallbackFontColor != null) CallbackFontColor(currentFont, currentColor);
            }
        }

        //文本颜色设置
        private void tsbColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = txtMessage.ForeColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                txtMessage.ForeColor = color;
                currentColor = color;

                //将当前设置到主窗口
                if (CallbackFontColor != null) CallbackFontColor(currentFont, currentColor);
            }
        }

        private void tsbScreenCaptrue_Click(object sender, EventArgs e)
        {

            if (tsbHideScreen.Checked)
            {
                DisplayAllForm(false);
            }

            Singleton.Show<FormBack>(() =>
            {
                FormBack frm = new FormBack();
                frm.Callback += new CallbackEventHandler(frm_Callback);
                return frm;
            });
        }

        void frm_Callback(object obj)
        {
            try
            {
                if (obj != null)
                {
                    try
                    {
                        //string base64String = iu.GetImageInBase64((int)SL.ImgType.IMAGE_JPEG);
                        //byte[] buffer = Convert.FromBase64String(base64String);
                        Image img = obj as Image;
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, ImageFormat.Jpeg);
                        byte[] buffer = ms.ToArray();
                        string[] urls = service.UploadImage(buffer, ".jpg");

                        string content = "<a href='" + urls[1] + "' target='_blank'><img border='0px' alt='查看大图' src='" + urls[0] + "' /></a>";
                        service.SendS2SMessage(MessageType.Picture, fromSeat.SeatID, toSeat.SeatID, null, content);

                        LoadMessage(false);

                        txtMessage.Text = string.Empty;
                        txtMessage.Focus();
                    }
                    finally
                    {
                        this.Refresh();

                        if (tsbHideScreen.Checked)
                        {
                            DisplayAllForm(true);
                        }
                    }
                }
                else
                {
                    if (tsbHideScreen.Checked)
                    {
                        DisplayAllForm(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("截图失败：" + ex.Message, "系统错误");
            }
        }

        private void tsbHideScreen_Click(object sender, EventArgs e)
        {
            tsbHideScreen.Checked = !tsbHideScreen.Checked;
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (按Control键发送消息ToolStripMenuItem.Checked)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSend_Click(sender, new EventArgs());
                }
            }
            else if (按ControlEnter键发送消息ToolStripMenuItem.Checked)
            {
                if (e.Control && e.KeyCode == Keys.Enter)
                {
                    btnSend_Click(sender, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text.Trim() == string.Empty)
            {
                ClientUtils.ShowMessage("发送的消息不能为空！");
                txtMessage.Focus();
                return;
            }

            string message = txtMessage.Text.Trim();
            message = message.Replace("\r\n", "<br>");
            message = message.Replace("\r", "");
            message = message.Replace("\n", "<br>");
            message = new Regex("{FACE#([\\d]+)#}", RegexOptions.IgnoreCase).Replace(message, "<img border=\"0\" src=\"" + company.ChatWebSite + "/images/face/$1.gif\" />");

            Font font = txtMessage.Font;
            Color color = txtMessage.ForeColor;

            if (!font.IsSystemFont)
            {
                //加入字体
                message = string.Format("<label style=\"font-size: {0}px; font-family: {1}; font-style: {2}; \">{3}</label>",
                    font.Size, font.FontFamily.Name, font.Style, message);

                //文字加特效
                if (font.Bold) message = string.Format("<b>{0}</b>", message);
                if (font.Strikeout) message = string.Format("<s>{0}</s>", message);
                if (font.Underline) message = string.Format("<u>{0}</u>", message);
                if (font.Italic) message = string.Format("<i>{0}</i>", message);
            }

            if (!color.IsSystemColor)
            {
                //设置颜色
                message = string.Format("<font color='{0}'>{1}</font>", color.IsNamedColor ? color.Name : "#" + color.Name.Remove(0, 2), message);
            }

            service.SendS2SMessage(MessageType.Text, fromSeat.SeatID, toSeat.SeatID, null, message);
            LoadMessage(false);

            txtMessage.Text = string.Empty;
            txtMessage.Focus();
        }

        private void DisplayAllForm(bool visible)
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.Visible = visible;
            }

            System.Threading.Thread.Sleep(100);
        }

        void EmotionContainer_ItemClick(object sender, EmotionItemMouseClickEventArgs e)
        {
            //throw new NotImplementedException();
            panel4.Visible = false;
            txtMessage.Text += "{" + string.Format("FACE#{0}#", e.Item.Text) + "}";
            txtMessage.Select(txtMessage.TextLength, 0);
            txtMessage.Focus();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (panel4.Visible)
                panel4.Visible = false;
            else
                panel4.Visible = true;
        }

        private void 按Control键发送消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            按Control键发送消息ToolStripMenuItem.Checked = true;
            按ControlEnter键发送消息ToolStripMenuItem.Checked = false;
        }

        private void 按ControlEnter键发送消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            按Control键发送消息ToolStripMenuItem.Checked = false;
            按ControlEnter键发送消息ToolStripMenuItem.Checked = true;
        }

        #region 文件上传

        /// <summary>
        /// 文件上传功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (panel2.Visible)
            {
                panel2.Visible = false;
            }
            else
            {
                panel2.Visible = true;
                txtFile.Text = string.Empty;
                button3.Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtFile.Text.Trim() == string.Empty)
            {
                ClientUtils.ShowMessage("请先选择要传送的文件！");
                button3.Focus();
                return;
            }

            string fileName = txtFile.Text.Trim();
            FileInfo file = new FileInfo(fileName);
            if (file.Length > 1024 * 1024 * 4)
            {
                ClientUtils.ShowMessage("当前选择的文件大于4M，不能进行传送！");
                return;
            }

            string message = string.Format("正在传送文件{0}，请稍候......", fileName);
            service.SendS2SMessage(MessageType.Text, fromSeat.SeatID, toSeat.SeatID, null, message);
            LoadMessage(false);

            System.Threading.ThreadPool.QueueUserWorkItem(UploadFile, fileName);
        }

        private void UploadFile(object value)
        {
            if (value == null) return;
            string content = null;
            var msgType = MessageType.Text;

            string fileName = value.ToString();
            string extension = Path.GetExtension(fileName).ToLower();

            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);

            //如果是图片则用上传图片的方法
            if (extension == ".jpg" || extension == ".gif" || extension == ".bmp" || extension == ".png")
            {
                string[] imageUrl = service.UploadImage(buffer, extension);
                msgType = MessageType.Picture;
                content = "<a href='" + imageUrl[1] + "' target='_blank'><img border='0px' alt='查看大图' src='" + imageUrl[0] + "' /></a>";
            }
            else
            {
                string fileURL = service.UploadFile(buffer, extension);
                msgType = MessageType.File;
                content = "<a href='" + fileURL + "' target='_blank'>" + Path.GetFileName(fileName) + "</a>";
                //message = "文件" + fileURL + "传送成功！";
            }

            //发送消息
            service.SendS2SMessage(msgType, fromSeat.SeatID, toSeat.SeatID, null, content);

            if (this.InvokeRequired)
                this.Invoke(new CallbackEventHandler(RefreshMessage), false);
            else
                RefreshMessage(false);
        }

        private void RefreshMessage(object args)
        {
            panel2.Visible = false;
            LoadMessage(Convert.ToBoolean(args));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        #endregion

        private void tsbChatMessage_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmMessage frm = new frmMessage(service, company, fromSeat, toSeat);
                return frm;
            });
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbAcceptTalk_Click(object sender, EventArgs e)
        {
            Singleton.Show<frmSeatInfo>(() =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, company, fromSeat, toSeat);
                return frm;
            });
        }

        private void txtMessage_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel2.Visible = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (service.GetSeat(toSeat.SeatID).State == OnlineState.Offline)
            {
                ClientUtils.ShowMessage(toSeat.SeatName + "不在线，不能进行视频会话！");
                return;
            }

            if (splitContainer1.Panel2.Width < 50)
            {
                this.Width += 80;
            }
            splitContainer1.Panel2.Show();
            splitContainer1.SplitterDistance = splitContainer1.Width - 160;
            toolStripButton3.Enabled = false;
            button5.Visible = true;

            chat.CreateClient();
            chat.InitRequest(true);

            frmSeatChat_SizeChanged(sender, e);
        }

        #region 消息管理

        /// 重写窗体的消息处理函数
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x400 + 6668)//接收自定义消息
            {
                int a = (int)m.WParam;
                switch (a)
                {
                    case 100:  //第二个按钮被按下
                        {
                            chat.OpenVideo(m.LParam);
                        }
                        break;
                    case 101: //第一个按钮
                        {
                            chat.OpenBigVideo(m.LParam);
                        }
                        break;
                    case 102: //视频窗口被隐藏.
                        {

                        }
                        break;
                    case 103: //视频退出.
                        {
                            chat.DestroyClient();
                        }
                        break;
                    case 104:	//用户名或密码出错.
                        {
                            chat.DestroyClient();
                            ClientUtils.ShowMessage("登陆失败，帐号有误！");
                        }
                        break;
                    case 105:	//与服务器的连接掉线了。
                        {
                            chat.SetOnline(m.LParam, false);
                            button5.Visible = true;
                        }
                        break;
                    case 106:	//登陆服务器成功。
                        {
                            chat.SetOnline(m.LParam, true);
                        }
                        break;
                    case 107:		//连接对方成功，第二个参数:窗口句柄.
                        {
                            //打开自己的视频
                            chat.OpenSelfVideo();
                        }
                        break;
                    case 108:	//断开与对方的连接。第二个参数:窗口句柄.
                        {
                            chat.CloseSelfVideo();
                        }
                        break;

                    case 109:	//对方不在线，无法连接。第二个参数:窗口句柄.
                        {

                        }
                        break;
                    case 110:		//对方没有添加你为用户，无法连接。第二个参数:窗口句柄.
                        {

                        }
                        break;

                    case 111:	//连接对方超时，连接失败。
                        {

                        }
                        break;
                    case 112:	//登陆服务器超时，登陆失败。
                        {
                            //MessageBox.Show ("msg:登陆服务器超时，登陆失败\n");
                        }
                        break;
                    case 113:	//其他原因(如：版本过低或人数已满等)登陆失败。
                        {
                            //MessageBox.Show ("msg:其他原因登陆失败\n");
                        }
                        break;
                    case 114: //收到用户请求,格式:用户名换行文字.
                        {
                            IntPtr hWnd = m.LParam;
                            StringBuilder text = new StringBuilder(1024);
                            int i = VideoChat.GetWindowText(hWnd, text, 1000);
                            if (i > 0)
                            {
                                String str = text.ToString();
                                int pos = str.IndexOf('\n');
                                if (pos > 0)
                                {
                                    String strUser = str.Substring(0, pos);
                                    String strText = str.Substring(pos + 1);
                                    String showUser = strUser;
                                    if (strText == "_ReqV") //发送的请求
                                    {
                                        if (fromSeat.SeatID == strUser.Replace('+', '_'))
                                            showUser = fromSeat.SeatName;
                                        else
                                            showUser = toSeat.SeatName;

                                        if (MessageBox.Show("用户【" + showUser + "】请求通话,是否确定与他通话？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                        {
                                            OpenVideo(strUser);
                                        }
                                        else
                                        {
                                            chat.SendText(strUser, "_CancelV");
                                        }
                                    }
                                    else if (strText == "_CancelV")
                                    {
                                        ClientUtils.ShowMessage("对方取消了你的请求！");
                                    }
                                    else
                                    {
                                        //MessageBox.Show(strText, strUser);
                                    }
                                }
                            }
                        }
                        break;
                    case 123: //是否子窗口还是弹出窗口
                        {
                            m.Result = (IntPtr)666;
                        }
                        break;
                }

            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        private void OpenVideo(String strUser)
        {
            if (splitContainer1.Panel2.Width < 50)
            {
                this.Width += 80;
            }

            splitContainer1.Panel2.Show();
            splitContainer1.SplitterDistance = splitContainer1.Width - 160;
            toolStripButton3.Enabled = false;
            button5.Visible = true;

            //首先添加.
            chat.CreateClient();
            chat.InitRequest(false);

            frmSeatChat_SizeChanged(null, null);

            chat.OpenVideo(strUser);
        }

        #endregion

        private void frmSeatChat_ResizeEnd(object sender, EventArgs e)
        {
            var rect = splitContainer2.Panel1.ClientRectangle;
            rect.X = splitContainer1.Width - splitContainer2.Width;
            chat.MoveTo(rect);
        }

        private void frmSeatChat_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;

            var rect = splitContainer2.Panel1.ClientRectangle;
            rect.X = splitContainer1.Width - splitContainer2.Width;
            chat.MoveTo(rect);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //取消视频
            chat.ExitVideo();

            if (splitContainer1.Panel2.Width > 50)
            {
                this.Width -= 80;
            }

            toolStripButton3.Enabled = true;
            splitContainer1.Panel2.Hide();
            splitContainer1.SplitterDistance = splitContainer1.Width;

            //chat.DestroyClient();
        }

        private void frmSeatChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            chat.DestroyClient();
        }
    }
}
