using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;
using System.IO;

namespace LiveChat.Client
{
    public partial class frmMessage : Form
    {
        private ISeatService service;
        private P2SSession session;
        private Company company;
        private Seat seat;
        private Seat toSeat;

        public frmMessage(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;

            InitializeComponent();
        }

        public frmMessage(ISeatService service, Company company, Seat seat, P2SSession session)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.session = session;

            InitializeComponent();
        }

        public frmMessage(ISeatService service, Company company, Seat seat, Seat toSeat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.toSeat = toSeat;

            InitializeComponent();
        }

        private void frmMessage_Load(object sender, EventArgs e)
        {
            if (session == null)
            {
                tabControl1.TabPages.RemoveAt(0);
            }
            else
            {
                textBox1.Text = session.User.UserID;

                string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
                url = Path.Combine(url, "chat.htm");
                wbChatBox.Url = new Uri(url);
                wbChatBox.AllowNavigation = false;
                //wbChatBox.IsWebBrowserContextMenuEnabled = false;
                wbChatBox.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbChatBox_DocumentCompleted);
            }

            string chaturl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            chaturl = Path.Combine(chaturl, "chat.htm");
            wbChatHistory.Url = new Uri(chaturl);
            wbChatHistory.AllowNavigation = false;
            wbChatHistory.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbChatHistory_DocumentCompleted);
            //wbChatHistory.IsWebBrowserContextMenuEnabled = false;

            wbFriendChat.Url = new Uri(chaturl);
            wbFriendChat.AllowNavigation = false;
            //wbFriendChat.IsWebBrowserContextMenuEnabled = false;

            dgvSessions.AutoGenerateColumns = false;

            dtpStartTime.Value = DateTime.Today.AddDays(-7);
            dtpEndTime.Value = DateTime.Today.AddHours(23).AddMinutes(59);

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            BindSeatFriend();

            if (toSeat != null) tabControl1.SelectedTab = tabPage3;
        }

        void wbChatHistory_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (toSeat == null) return;

            var view = service.GetS2SHistoryMessagesFromDB(seat.SeatID, toSeat.SeatID, DateTime.Today, DateTime.Today.AddDays(1), 1, 1000);
            if (view.RowCount > 0)
            {
                var msgs = (view.DataSource as List<S2SMessage>).ConvertAll<Entity.Message>(p => (Entity.Message)p);
                LoadMessage(wbFriendChat, seat.SeatID, msgs);
            }
            else
            {
                LoadMessage(wbFriendChat, seat.SeatID, new List<Entity.Message>());
            }
        }

        void wbChatBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var list = service.GetP2SHistoryMessages(session.SessionID);
            LoadMessage(wbChatBox, session.CreateID, list);
        }

        private void BindSeatFriend()
        {
            TreeNode selectNode = null;
            IList<SeatFriend> friends;
            IList<SeatFriend> friendlist = service.GetSeatFriends(seat.SeatID, out friends);

            Dictionary<string, string> dictCompany = new Dictionary<string, string>();
            foreach (SeatFriend friend in friendlist)
            {
                if (!dictCompany.ContainsKey(friend.CompanyID))
                    dictCompany.Add(friend.CompanyID, friend.CompanyName);
            }

            tvLinkman.Nodes.Clear();
            foreach (KeyValuePair<string, string> kv in dictCompany)
            {
                var list = (friendlist as List<SeatFriend>).FindAll(p => p.CompanyID == kv.Key);
                TreeNode node = new TreeNode();
                node.SelectedImageIndex = 3;
                node.ImageIndex = 3;
                node.Text = string.Format("{0}({1})", kv.Value, list.Count);
                foreach (var friend in list)
                {
                    TreeNode tn = new TreeNode(string.Format("{0}", friend.ShowName));
                    if (!string.IsNullOrEmpty(friend.MemoName))
                        tn = new TreeNode(string.Format("{0}", friend.MemoName));

                    if (friend.State == OnlineState.Online)
                    {
                        tn.SelectedImageIndex = 0;
                        tn.ImageIndex = 0;
                    }
                    else if (friend.State == OnlineState.Busy)
                    {
                        tn.SelectedImageIndex = 6;
                        tn.ImageIndex = 6;
                    }
                    else if (friend.State == OnlineState.Leave)
                    {
                        tn.SelectedImageIndex = 7;
                        tn.ImageIndex = 7;
                    }
                    else
                    {
                        tn.SelectedImageIndex = 1;
                        tn.ImageIndex = 1;
                    }
                    tn.Tag = friend;

                    if (toSeat != null && toSeat.SeatID == friend.SeatID)
                    {
                        selectNode = tn;
                    }
                    node.Nodes.Add(tn);
                }

                //展开自己所在的公司
                if (kv.Key == company.CompanyID)
                {
                    node.Expand();
                }

                tvLinkman.Nodes.Add(node);
            }

            TreeNode tn1 = new TreeNode(string.Format("陌生人({0})", friends.Count));
            tn1.SelectedImageIndex = 3;
            tn1.ImageIndex = 3;

            tvLinkman.Nodes.Add(tn1);
            foreach (var friend in friends)
            {
                TreeNode tn = new TreeNode(string.Format("{0}", friend.ShowName));
                if (!string.IsNullOrEmpty(friend.MemoName))
                    tn = new TreeNode(string.Format("{0}", friend.MemoName));

                if (friend.State == OnlineState.Online)
                {
                    tn.SelectedImageIndex = 0;
                    tn.ImageIndex = 0;
                }
                else if (friend.State == OnlineState.Busy)
                {
                    tn.SelectedImageIndex = 6;
                    tn.ImageIndex = 6;
                }
                else if (friend.State == OnlineState.Leave)
                {
                    tn.SelectedImageIndex = 7;
                    tn.ImageIndex = 7;
                }
                else
                {
                    tn.SelectedImageIndex = 1;
                    tn.ImageIndex = 1;
                }

                tn.Tag = friend;

                if (toSeat != null && toSeat.SeatID == friend.SeatID)
                {
                    selectNode = tn;
                }
                tn1.Nodes.Add(tn);
            }

            if (selectNode != null)
            {
                tvLinkman.SelectedNode = selectNode;
            }
        }

        private void LoadMessage(WebBrowser wbChat, string createID, IList<Entity.Message> list)
        {
            try
            {
                HtmlElement element = wbChat.Document.GetElementById("historyBox");

                if (list.Count == 0)
                {
                    element.InnerHtml = string.Empty;

                    toolStripStatusLabel1.Text = "不存在聊天记录......";
                }
                else
                {
                    toolStripStatusLabel1.Text = string.Format("共有{0}条聊天信息......", list.Count);

                    element.InnerHtml = string.Empty;

                    foreach (Entity.Message msg in list)
                    {
                        HtmlElement p = element.Document.CreateElement("p");
                        StringBuilder sb = new StringBuilder();

                        if (msg.SenderID == createID)
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
                }
            }
            catch { }
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string userid = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(userid))
            {
                ClientUtils.ShowMessage("请输入要查询的用户ID号！");
                textBox1.Focus();
                return;
            }

            try
            {
                var list = service.GetP2SSessionsFromDB(seat, userid, dtpStartTime.Value, dtpEndTime.Value);
                dgvSessions.DataSource = (list as List<P2SSession>).ConvertAll<ClientSession>(p =>
                    {
                        ClientSession s = new ClientSession();
                        s.SessionID = p.SID.ToString();
                        s.CreateID = p.CreateID;
                        s.UserName = p.User.ShowName;
                        s.SeatName = p.Seat.ShowName;
                        s.StartTime = p.StartTime.ToString();
                        s.LastTime = p.LastReceiveTime.ToString();
                        s.From = p.FromAddress;
                        s.IP = p.FromIP;

                        return s;
                    });

                toolStripStatusLabel1.Text = string.Format("共有{0}次会话信息......", list.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败：" + ex.Message, "系统错误");
            }
        }

        private void dgvSessions_Click(object sender, EventArgs e)
        {
            if (dgvSessions.SelectedRows.Count == 0) return;

            try
            {
                ClientSession cs = dgvSessions.SelectedRows[0].DataBoundItem as ClientSession;
                var view = service.GetP2SHistoryMessagesFromDB(new Guid(cs.SessionID), 1, 1000);

                if (view.RowCount > 0)
                {
                    var list = (view.DataSource as List<P2SMessage>).ConvertAll<Entity.Message>(p => (Entity.Message)p);
                    LoadMessage(wbChatHistory, cs.CreateID, list);
                }
                else
                {
                    LoadMessage(wbChatHistory, cs.CreateID, new List<Entity.Message>());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败：" + ex.Message, "系统错误");
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (session == null) return;
            var list = service.GetP2SHistoryMessages(session.SessionID);
            LoadMessage(wbChatBox, session.CreateID, list);
        }

        private void tvLinkman_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;

            string friendID = ((SeatFriend)e.Node.Tag).SeatID;
            wbChatHistory.Tag = e.Node.Tag;

            try
            {
                var view = service.GetS2SHistoryMessagesFromDB(seat.SeatID, friendID, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1), 1, 1000);

                if (view.RowCount > 0)
                {
                    var list = (view.DataSource as List<S2SMessage>).ConvertAll<Entity.Message>(p => (Entity.Message)p);
                    LoadMessage(wbFriendChat, seat.SeatID, list);
                }
                else
                {
                    LoadMessage(wbFriendChat, seat.SeatID, new List<Entity.Message>());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败：" + ex.Message, "系统错误");
            }
        }

        private void tvLinkman_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = 3;
            e.Node.ImageIndex = 3;
        }

        private void tvLinkman_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = 4;
            e.Node.ImageIndex = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (wbChatHistory.Tag == null) return;
                string friendID = ((SeatFriend)wbChatHistory.Tag).SeatID;

                var view = service.GetS2SHistoryMessagesFromDB(seat.SeatID, friendID, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1), 1, 1000);

                if (view.RowCount > 0)
                {
                    var list = (view.DataSource as List<S2SMessage>).ConvertAll<Entity.Message>(p => (Entity.Message)p);
                    LoadMessage(wbFriendChat, seat.SeatID, list);
                }
                else
                {
                    LoadMessage(wbFriendChat, seat.SeatID, new List<Entity.Message>());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败：" + ex.Message, "系统错误");
            }
        }
    }
}