using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using System.IO;
using LiveChat.Entity;
using System.Runtime.InteropServices;

namespace LiveChat.Monitoring
{
    public partial class frmReply : Form
    {
        private ISeatService service;
        private ClientSession session;
        private Seat loginSeat;
        private Timer timer;
        private int MessageCount;

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        public frmReply(ISeatService service, ClientSession session, Seat loginSeat)
        {
            this.service = service;
            this.session = session;
            this.loginSeat = loginSeat;
            InitializeComponent();
        }

        private void frmReply_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("当前【{0}】与【{1}】正在聊天中...", session.SeatName, session.UserName);

            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            url = Path.Combine(url, "chat.htm");
            webBrowser1.Url = new Uri(url);
            webBrowser1.AllowNavigation = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

            BindFastReply();

            timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LoadMessage(true);
        }

        void BindFastReply()
        {
            IList<Reply> list = service.GetReplys(loginSeat.CompanyID);
            (list as List<Reply>).ForEach(delegate(Reply reply)
            {
                reply.Title = reply.Content;
            });

            Reply r = new Reply();
            r.Title = "--请选择--";
            list.Insert(0, r);

            comboBox1.DisplayMember = "Title";
            comboBox1.ValueMember = "Content";
            comboBox1.DataSource = list;
        }


        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LoadMessage(false);
        }

        private void LoadMessage(bool isflash)
        {
            try
            {
                HtmlElement element = webBrowser1.Document.GetElementById("title");
                element.InnerHtml = string.Empty;

                element = webBrowser1.Document.GetElementById("historyBox");

                IList<Entity.Message> list = service.GetP2SHistoryMessages(session.SessionID);
                if (list.Count == 0)
                {
                    element.InnerHtml = "不存在此会话的聊天记录！";
                }
                else
                {
                    if (MessageCount != list.Count)
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
                                    sb.Append(msg.SenderName + "向您发送了一个截屏 <font style=\"font-weight: normal;\">" + msg.SendTime.ToLongTimeString() + "</font>:");
                                else
                                    sb.Append(msg.SenderName + "&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToLongTimeString() + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }
                            else
                            {
                                p.SetAttribute("className", "operator");
                                if (msg.Type == MessageType.Picture)
                                    sb.Append("您向" + ((P2SMessage)msg).ReceiverName + "发送了一个截屏 <font style=\"font-weight: normal;\">" + msg.SendTime.ToLongTimeString() + "</font>:");
                                else
                                    sb.Append(msg.SenderName + "&nbsp;说 <font style=\"font-weight: normal;\">" + msg.SendTime.ToLongTimeString() + "</font>:");

                                sb.Append("<br />");
                                sb.Append("<span>" + msg.Content + "</span>");
                            }

                            p.InnerHtml = sb.ToString();
                            element.AppendChild(p);
                        }

                        element.ScrollIntoView(false);

                        MessageCount = list.Count;

                        if (isflash)
                        {
                            //闪屏处理
                            FlashWindow(this.Handle, true);
                        }
                    }
                }
            }
            catch { }

            //textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("发送的消息不能为空！", "我来回答", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
                return;
            }

            string message = textBox1.Text.Trim();
            message = message.Replace("\r\n", "<br>");
            message = message.Replace("\r", "");
            message = message.Replace("\n", "<br>");

            service.SendP2SMessage(MessageType.Text, session.SessionID, loginSeat.SeatID, null, message);

            LoadMessage(false);

            textBox1.Text = string.Empty;
            textBox1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;
            if (comboBox1.SelectedValue != null)
            {
                textBox1.Text = comboBox1.SelectedValue.ToString();
            }
        }
    }
}
