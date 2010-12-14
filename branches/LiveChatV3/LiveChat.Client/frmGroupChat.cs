using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSharpWin;
using System.IO;
using LiveChat.Entity;
using LiveChat.Interface;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using MySoft.Core;
using System.Drawing.Imaging;
using LiveChat.Utils;

namespace LiveChat.Client
{
    public partial class frmGroupChat : Form
    {
        public event CallbackFontColorEventHandler CallbackFontColor;

        public event CallbackEventHandler Callback;

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        private ISeatService service;
        private Company company;
        private Seat seat;
        private SeatGroup group;
        private Timer msgtimer;

        private Color currentColor;
        private Font currentFont;
        private DateTime currentTime;
        private int messageCount;

        public frmGroupChat(ISeatService service, Company company, Seat seat, SeatGroup group, Font useFont, Color useColor)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.group = group;
            this.currentFont = useFont;
            this.currentColor = useColor;
            this.currentTime = DateTime.Today;

            InitializeComponent();
        }

        private void frmGroupChat_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("【{0}】聊天中...", (group.MemoName ?? group.GroupName));
            textBox1.Text = group.Notification;

            if (seat.SeatID == group.CreateID)
            {
                toolStripButton5.Visible = true;
                toolStripButton3.Visible = false;
            }
            else
            {
                toolStripButton5.Visible = false;
                toolStripButton3.Visible = true;
            }

            IList<Seat> seats = new List<Seat>();
            try
            {
                seats = service.GetGroupSeats(group.GroupID);
                LoadSeatInfo(seats);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    this.splitContainer1.Enabled = false;
                    this.toolStrip1.Enabled = false;
                    this.Text += "【此群已经被解散】";
                }
            }

            int onlineCount = (seats as List<Seat>).FindAll(p => p.State == OnlineState.Online).Count;
            listSeats.Columns[0].Text = string.Format("群成员 ({0}/{1})", onlineCount, seats.Count);

            if (currentFont != null) txtMessage.Font = currentFont;
            if (currentColor != null) txtMessage.ForeColor = currentColor;

            //获取点击的表情。
            emotionDropdown1.LoadImages(AppDomain.CurrentDomain.BaseDirectory);
            emotionDropdown1.EmotionContainer.ItemClick += new EmotionItemMouseEventHandler(EmotionContainer_ItemClick);

            InitBrowser();
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

        void LoadSeatInfo(IList<Seat> seats)
        {
            (seats as List<Seat>).Sort(new SortComparer<Seat>(new SortProperty("State").Asc, new SortProperty("SeatCode").Asc));
            listSeats.Items.Clear();
            foreach (Seat info in seats)
            {
                //if (info.SeatType == SeatType.Super) continue;

                //ListViewItem item = new ListViewItem(new string[] { state, info.SeatCode, info.SeatName, info.Telephone, info.MobileNumber, info.Email });
                ListViewItem item = new ListViewItem(new string[] { string.Format("{0}({1})", info.SeatName, info.SeatCode) });
                switch (info.State)
                {
                    case OnlineState.Online:
                        item.ImageIndex = 0;
                        break;
                    case OnlineState.Leave:
                        item.ImageIndex = 6;
                        break;
                    case OnlineState.Busy:
                        item.ImageIndex = 7;
                        break;
                    case OnlineState.Offline:
                        item.ImageIndex = 1;
                        break;
                }
                item.Tag = info;
                listSeats.Items.Add(item);
            }
        }

        void wbChatBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!this.toolStrip1.Enabled) return;

            LoadMessage(false);

            msgtimer = new Timer();
            msgtimer.Interval = 5000;
            msgtimer.Tick += new EventHandler(msgtimer_Tick);
            msgtimer.Start();

            if (txtMessage.Enabled) txtMessage.Focus();
        }

        void EmotionContainer_ItemClick(object sender, EmotionItemMouseClickEventArgs e)
        {
            panel1.Visible = false;
            txtMessage.Text += "{" + string.Format("FACE#{0}#", e.Item.Text) + "}";
            txtMessage.Select(txtMessage.TextLength, 0);
            txtMessage.Focus();
        }

        void msgtimer_Tick(object sender, EventArgs e)
        {
            msgtimer.Stop();

            try
            {
                IList<Seat> seats = service.GetGroupSeats(group.GroupID);
                LoadSeatInfo(seats);

                int onlineCount = (seats as List<Seat>).FindAll(p => p.State == OnlineState.Online).Count;

                Regex reg = new Regex("([\\d]+)/([\\d]+)", RegexOptions.IgnoreCase);
                lblGroupInfo.Text = reg.Replace(lblGroupInfo.Text, string.Format("{0}/{1}", onlineCount, seats.Count));

                LoadMessage(true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ClientUtils.ShowMessage(ex.Message);
                    this.Close();
                    return;
                }
                else
                {
                    ClientUtils.ShowError(ex);
                }
            }

            msgtimer.Start();
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

            service.SendSGMessage(MessageType.Text, group.GroupID, seat.SeatID, null, message);

            LoadMessage(false);

            txtMessage.Text = string.Empty;
            txtMessage.Focus();
        }

        private void LoadMessage(bool isflash)
        {
            try
            {
                HtmlElement element = wbChatBox.Document.GetElementById("historyBox");

                IList<Entity.Message> list = service.GetSGHistoryMessages(group.GroupID, seat.SeatID, currentTime);
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

                            if (msg.SenderID != seat.SeatID)
                            {
                                p.SetAttribute("className", "visitor");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append(msg.SenderName + "向群发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
                                else
                                    sb.Append(msg.SenderName + "&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }
                            else
                            {
                                p.SetAttribute("className", "operator");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append("您向" + (group.MemoName ?? group.GroupName) + "发送了一个图片 <font style=\"font-weight: normal;\">" + msg.SendTime.ToString("yyyy/MM/dd HH:mm:ss") + "</font>:");
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
                        service.SendSGMessage(MessageType.Picture, group.GroupID, seat.SeatID, null, content);

                        LoadMessage(false);

                        txtMessage.Text = string.Empty;
                        txtMessage.Focus();
                    }
                    finally
                    {
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

        private void DisplayAllForm(bool visible)
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.Visible = visible;
            }

            System.Threading.Thread.Sleep(100);
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
                panel1.Visible = false;
            else
                panel1.Visible = true;
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

        private void txtMessage_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void listSeats_DoubleClick(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0) return;
            Seat friend = listSeats.SelectedItems[0].Tag as Seat;

            string key = string.Format("Config_{0}", friend.SeatID);
            SingletonMul.Show<frmSeatInfo>(key, () =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, company, seat, friend);
                return frm;
            });

            //string key = string.Format("SeatChat_{0}_{1}", seat.SeatID, friend.SeatID);
            //SingletonMul.Show(key, () =>
            //{
            //    frmSeatChat frmSeatChat = new frmSeatChat(service, company, seat, friend, friend.MemoName, currentFont, currentColor);
            //    frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
            //    return frmSeatChat;
            //});
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //群资料
            string key = string.Format("SeatGroup_{0}", group.GroupID);
            SingletonMul.Show(key, () =>
            {
                frmGroup frmGroup = new frmGroup(service, company, seat, group);
                frmGroup.Callback += new CallbackEventHandler(frmGroup_Callback);
                return frmGroup;
            });
        }

        void frmGroup_Callback(object obj)
        {
            if (obj == null) return;
            textBox1.Text = obj.ToString();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //退出该群
            if (MessageBox.Show("确定退出群【" + (group.MemoName ?? group.GroupName) + "】吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            service.ExitGroup(seat.SeatID, group.GroupID);
            if (Callback != null) Callback(null);
            this.Close();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //退出该群
            if (MessageBox.Show("确定解散群【" + (group.MemoName ?? group.GroupName) + "】吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            service.DismissGroup(seat.SeatID, group.GroupID);
            if (Callback != null) Callback(null);
            this.Close();
        }
    }
}
