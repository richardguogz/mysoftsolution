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
    public partial class frmSeatChat : Form, ICallbackForm
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
        private bool isChating;
        private bool isClosing;

        private bool _IsReceiveRequest;
        private bool _IsVideoRequest;
        private IntPtr _MainFormParent;

        /// <summary>
        /// 是否启动就接受请求
        /// </summary>
        public bool IsReceiveRequest
        {
            get { return _IsReceiveRequest; }
            set { _IsReceiveRequest = value; }
        }

        /// <summary>
        /// 是否是视频请求
        /// </summary>
        public bool IsVideoRequest
        {
            get { return _IsVideoRequest; }
            set { _IsVideoRequest = value; }
        }

        /// <summary>
        /// 主窗口句柄
        /// </summary>
        public IntPtr MainFormParent
        {
            get { return _MainFormParent; }
            set { _MainFormParent = value; }
        }

        public frmSeatChat(ISeatService service, VideoChat chat, Company company, Seat fromSeat, Seat toSeat, string memoName, Font useFont, Color useColor)
        {
            this.service = service;
            this.chat = chat;

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

            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Panel2.Hide();
            splitContainer1.SplitterDistance = splitContainer1.Width;

            //获取点击的表情。
            emotionDropdown1.LoadImages(AppDomain.CurrentDomain.BaseDirectory);
            emotionDropdown1.EmotionContainer.ItemClick += new EmotionItemMouseEventHandler(EmotionContainer_ItemClick);

            InitBrowser();

            ReceiveRequest();
        }

        /// <summary>
        /// 接受请求
        /// </summary>
        private void ReceiveRequest()
        {
            //启动打开视频
            if (_IsReceiveRequest)
            {
                if (chat.IsConnected)
                {
                    ClientUtils.ShowMessage("当前你正在与【" + chat.GetChatName() + "】聊天中，语音及视频无法再次启动！");
                }
                else
                {
                    isChating = true;

                    if (splitContainer1.Width - 180 > 0)
                    {
                        if (splitContainer1.Panel2.Width < 80)
                        {
                            this.Width += 100;
                        }
                        splitContainer1.Panel2.Show();
                        splitContainer1.SplitterDistance = splitContainer1.Width - 180;
                    }

                    toolStripButton3.Enabled = false;
                    toolStripButton4.Enabled = false;

                    //接收并打开视频
                    chat.ReceiveRequest(_MainFormParent, this.Handle, toSeat);

                    frmSeatChat_SizeChanged(null, null);

                    button5.Visible = false;
                    panel6.Visible = true;
                }
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
                            VideoChat.FlashWindow(this.Handle, true);
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
                ClientUtils.ShowMessage(toSeat.SeatName + "不在线，不能进行语音会话！");
                return;
            }

            if (chat.IsConnected)
            {
                ClientUtils.ShowMessage("当前你正在与其它人进行对话，不能再次发送请求！");
                return;
            }

            //发送请求
            if (chat.SendRequest(_MainFormParent, this.Handle, toSeat, true))
            {
                if (splitContainer1.Panel2.Width < 80)
                {
                    this.Width += 100;
                }
                splitContainer1.Panel2.Show();
                splitContainer1.SplitterDistance = splitContainer1.Width - 180;

                toolStripButton3.Enabled = false;
                toolStripButton4.Enabled = false;

                isChating = true;

                button5.Visible = true;
                panel6.Visible = false;

                frmSeatChat_SizeChanged(sender, e);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (service.GetSeat(toSeat.SeatID).State == OnlineState.Offline)
            {
                ClientUtils.ShowMessage(toSeat.SeatName + "不在线，不能进行视频会话！");
                return;
            }

            if (chat.IsConnected)
            {
                ClientUtils.ShowMessage("当前你正在与其它人进行对话，不能再次发送请求！");
                return;
            }

            //发送请求
            if (chat.SendRequest(_MainFormParent, this.Handle, toSeat, false))
            {
                if (splitContainer1.Panel2.Width < 80)
                {
                    this.Width += 100;
                }
                splitContainer1.Panel2.Show();
                splitContainer1.SplitterDistance = splitContainer1.Width - 180;

                toolStripButton3.Enabled = false;
                toolStripButton4.Enabled = false;

                isChating = true;

                button5.Visible = true;
                panel6.Visible = false;

                frmSeatChat_SizeChanged(sender, e);
            }
        }

        private void frmSeatChat_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;
            if (isClosing) return;
            if (!isChating) return;

            var rect1 = panel3.ClientRectangle;
            rect1.X = splitContainer1.Width - rect1.Width;
            rect1.Y = 60;

            var rect2 = panel5.ClientRectangle;
            rect2.X = splitContainer1.Width - rect2.Width;
            rect2.Y = splitContainer1.Height - panel5.Height + 60;

            chat.MoveVideoTo(rect1, rect2);
            button5.Top = (panel1.Height - button5.Height) / 2 + 5;

            button4.Top = (panel6.Height - button4.Height) / 2 + 5;
            button6.Top = (panel6.Height - button6.Height) / 2 + 5;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //关闭视频
            chat.CloseVideo(this.Handle);

            //取消视频
            chat.ExitVideo(_MainFormParent, this.Handle);

            if (splitContainer1.Panel2.Width > 50)
            {
                this.Width -= 100;
            }

            toolStripButton3.Enabled = true;
            toolStripButton4.Enabled = true;

            splitContainer1.Panel2.Hide();
            splitContainer1.SplitterDistance = splitContainer1.Width;

            isChating = false;
            //chat.DestroyClient();
        }

        private void frmSeatChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isClosing = true;
        }

        #region ICallbackForm 成员

        /// <summary>
        /// 实现回调的方法
        /// </summary>
        /// <param name="args"></param>
        public void Run(params object[] args)
        {
            if (args.Length > 0)
            {
                if (Convert.ToBoolean(args[0]))
                {
                    button5.Visible = false;
                    panel6.Visible = true;

                    _IsReceiveRequest = true;
                    ReceiveRequest();

                    //闪屏处理
                    VideoChat.FlashWindow(this.Handle, true);
                }
                else
                {
                    chat.SetConnected(false);

                    CancelChat();
                }
            }
        }

        private void CancelChat()
        {
            //取消视频
            chat.ExitVideo(_MainFormParent, this.Handle);

            if (splitContainer1.Panel2.Width > 50)
            {
                this.Width -= 100;
            }

            toolStripButton3.Enabled = true;
            toolStripButton4.Enabled = true;

            splitContainer1.Panel2.Hide();
            splitContainer1.SplitterDistance = splitContainer1.Width;

            isChating = false;
        }

        #endregion

        private void frmSeatChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isChating)
            {
                chat.CloseVideo(this.Handle);
            }

            chat.ExitVideo(_MainFormParent, this.Handle);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string strUser = toSeat.SeatID.Replace('_', '+');

            if (chat.IsConnected)
            {
                ClientUtils.ShowMessage("当前你正在与其它人进行对话，不能再次接受请求！");

                //对方拒绝
                chat.SendText(strUser, "_ReqVFail");
                return;
            }

            button5.Visible = true;
            panel6.Visible = false;

            if (_IsVideoRequest)
            {
                //打开视频
                chat.OpenVideo(strUser);

                //对方接受
                chat.SendText(strUser, "_ReqVOK");
            }
            else
            {
                //关闭视频
                chat.CloseVideo();

                //打开视频
                chat.OpenVideo(strUser);

                //对方接受
                chat.SendText(strUser, "_ReqNVOK");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            string strUser = toSeat.SeatID.Replace('_', '+');

            //对方拒绝
            chat.SendText(strUser, "_ReqVFail");

            CancelChat();
        }
    }
}
