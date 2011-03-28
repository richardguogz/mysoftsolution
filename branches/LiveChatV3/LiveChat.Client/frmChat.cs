using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using LiveChat.Entity;
using LiveChat.Interface;
using LiveChat.Client;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using CSharpWin;

namespace LiveChat.Client
{
    public partial class frmChat : Form
    {
        public event CallbackEventHandler Callback;

        public event CallbackFontColorEventHandler CallbackFontColor;

        private const int maxAcceptCount = 20;

        private Color currentColor;
        private Font currentFont;

        private ISeatService service;
        private P2SSession session;
        private Company company;
        private Seat seat;
        private Timer msgtimer;
        private int messageCount;

        public frmChat(ISeatService service, P2SSession session, Company company, Seat seat, Font useFont, Color useColor)
        {
            this.service = service;
            this.session = session;
            this.seat = seat;
            this.company = company;
            this.currentFont = useFont;
            this.currentColor = useColor;

            InitializeComponent();
        }

        private void frmChat_Load(object sender, EventArgs e)
        {
            if (session.Seat == null)
            {
                this.Text = string.Format("【{0}】请求会话中...", session.User.UserName);
                tsbAcceptTalk.Enabled = true;
                tsbEndTalk.Enabled = false;
                plChat.Enabled = false;
            }
            else
            {
                this.Text = string.Format("您与【{0}】聊天中...", session.User.UserName);
                tsbAcceptTalk.Enabled = false;
                tsbEndTalk.Enabled = true;
            }

            if (currentFont != null) txtMessage.Font = currentFont;
            if (currentColor != null) txtMessage.ForeColor = currentColor;

            //获取点击的表情。
            emotionDropdown1.LoadImages(AppDomain.CurrentDomain.BaseDirectory);
            emotionDropdown1.EmotionContainer.ItemClick += new EmotionItemMouseEventHandler(EmotionContainer_ItemClick);

            InitBrowser();

            BindFastReply();
            BindFastLink();
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

        void timer_Tick(object sender, EventArgs e)
        {
            msgtimer.Stop();

            try
            {
                if (session.Seat == null)
                {
                    var s = service.GetSession(session.SessionID);
                    if (s == null) //表示会话被其它人接受
                    {
                        msgtimer.Stop();

                        ClientUtils.ShowMessage("当前会话已经被其它客服接受，将强制关闭本窗口！");
                        if (Callback != null) Callback(session.SessionID);
                        this.Close();
                        return;
                    }
                }
                else if (session.Seat != null)
                {
                    var s = service.GetSession(session.SessionID);
                    if (s == null)  //表示会话已经被关闭
                    {
                        msgtimer.Stop();

                        ClientUtils.ShowMessage("当前会话已经被用户结束，将强制关闭本窗口！");
                        if (Callback != null) Callback(session.SessionID);
                        this.Close();
                        return;
                    }
                }
            }
            catch
            {
                ClientUtils.SoftwareIsOnline = false;
            }

            LoadMessage(true);

            msgtimer.Start();
        }

        void wbChatBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LoadMessage(false);

            msgtimer = new Timer();
            msgtimer.Interval = 5000;
            msgtimer.Tick += new EventHandler(timer_Tick);
            msgtimer.Start();

            if (txtMessage.Enabled) txtMessage.Focus();
        }

        private void LoadMessage(bool isflash)
        {
            try
            {
                HtmlElement element = wbChatBox.Document.GetElementById("historyBox");

                IList<Entity.Message> list = service.GetP2SHistoryMessages(session.SessionID);
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

                            if (msg.SenderID == session.CreateID)
                            {
                                p.SetAttribute("className", "visitor");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append(msg.SenderName + "向您发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                else if (msg.Type == MessageType.File)
                                {
                                    sb.Append(msg.SenderName + "向您传送了一个文件 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                    msg.Content = "点击下载:" + msg.Content;
                                }
                                else
                                    sb.Append(msg.SenderName + "&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }
                            else
                            {
                                p.SetAttribute("className", "operator");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append("您向" + ((IReceiver)msg).ReceiverName + "发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                else if (msg.Type == MessageType.File)
                                {
                                    sb.Append("您向" + ((IReceiver)msg).ReceiverName + "传送了一个文件 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
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

        void BindFastReply()
        {
            IList<Reply> list = service.GetReplys(seat.CompanyID);
            tsReply.DropDownItems.Clear();
            int index = 1;
            foreach (Reply info in list)
            {
                ToolStripButton item = new ToolStripButton(info.Title);
                item.AutoSize = false;
                item.Width = 360;
                item.Tag = info;
                item.ToolTipText = info.Content;
                item.Click += new EventHandler(reply_Click);
                tsReply.DropDownItems.Add(item);
                index++;
            }
        }

        private void BindFastLink()
        {
            IList<Link> list = service.GetLinks(seat.CompanyID);
            tsUrl.DropDownItems.Clear();
            int index = 1;
            foreach (Link info in list)
            {
                ToolStripButton item = new ToolStripButton(info.Title);
                item.AutoSize = false;
                item.Width = 240;
                item.Tag = info;
                item.ToolTipText = info.Url;
                item.Click += new EventHandler(link_Click);
                tsUrl.DropDownItems.Add(item);
                index++;
            }
        }

        void reply_Click(object sender, EventArgs e)
        {
            if (session.Seat == null)
            {
                ClientUtils.ShowMessage("请先接受会话！");
                return;
            }

            Reply reply = (sender as ToolStripButton).Tag as Reply;
            string content = reply.Content;
            service.SendP2SMessage(MessageType.Text, session.SessionID, seat.SeatID, null, content);
            LoadMessage(false);
        }

        void link_Click(object sender, EventArgs e)
        {
            if (session.Seat == null)
            {
                ClientUtils.ShowMessage("请先接受会话！");
                return;
            }

            Link link = (sender as ToolStripButton).Tag as Link;
            string url = link.Url;
            url = string.Format("<a href='{0}' target='_blank'>{0}</a>", url);

            service.SendP2SMessage(MessageType.Url, session.SessionID, seat.SeatID, null, url);
            LoadMessage(false);
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
                        service.SendP2SMessage(MessageType.Picture, session.SessionID, seat.SeatID, null, content);

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

            service.SendP2SMessage(MessageType.Text, session.SessionID, seat.SeatID, null, message);
            LoadMessage(false);

            txtMessage.Text = string.Empty;
            txtMessage.Focus();
        }

        /// <summary>
        /// 退出当前窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbAcceptTalk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定接受当前会话吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            try
            {
                if (session.Seat == null)
                {
                    string oldID = session.SessionID;

                    //接入会话
                    session = service.AcceptSession(seat.SeatID, maxAcceptCount, session.SessionID);
                    if (Callback != null) Callback(new string[] { oldID, session.SessionID });

                    LoadMessage(false);

                    this.Text = string.Format("您与【{0}】聊天中...", session.User.UserName);
                    tsbAcceptTalk.Enabled = false;
                    tsbEndTalk.Enabled = true;
                    plChat.Enabled = true;
                }

                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void tsbEndTalk_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定结束当前会话吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                //接入会话
                service.CloseSession(session.SessionID);
                if (Callback != null) Callback(session.SessionID);
                this.Close();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
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
            panel4.Visible = false;
            txtMessage.Text += "{" + string.Format("FACE#{0}#", e.Item.Text) + "}";
            txtMessage.Select(txtMessage.TextLength, 0);
            txtMessage.Focus();
        }

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
            service.SendP2SMessage(MessageType.Text, session.SessionID, seat.SeatID, null, message);
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
            }

            //发送消息
            service.SendP2SMessage(msgType, session.SessionID, seat.SeatID, null, content);

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

        private void tsbChatMessage_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmMessage frm = new frmMessage(service, company, seat, session);
                return frm;
            });
        }

        private void txtMessage_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel2.Visible = false;
        }
    }
}
