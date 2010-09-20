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
using System.Net.Sockets;

namespace LiveChat.Monitoring
{
    public partial class frmMonitor : Form
    {
        private ISeatService service;
        private Company loginCompany;
        private Seat loginSeat;
        private Guid clientID;
        private Timer timer;
        private int MessageCount;
        private bool? isExit;

        public frmMonitor(ISeatService service, Company company, Seat seat, Guid clientID)
        {
            this.service = service;
            this.loginCompany = company;
            this.loginSeat = seat;
            this.clientID = clientID;
            InitializeComponent();
        }

        private void frmMonitor_Load(object sender, EventArgs e)
        {
            if (service == null)
            {
                service = RemotingUtil.GetRemotingSeatService();
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView3.AutoGenerateColumns = false;

            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            url = Path.Combine(url, "chat.htm");
            webBrowser1.Url = new Uri(url);
            webBrowser1.AllowNavigation = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;

            timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            this.Text += string.Format(" ({0})", loginCompany.CompanyName);

            BindSeat();
            BindRequest();

            if (loginSeat.SeatType != SeatType.Normal)
            {

                IList<Seat> list = service.GetCompanySeats(loginSeat.CompanyID);
                foreach (Seat seat in list)
                {
                    if (seat.SeatID == loginSeat.SeatID) continue;
                    string text = seat.ShowName;
                    if (seat.State == OnlineState.Online)
                    {
                        text += " 在线";
                    }
                    ToolStripMenuItem item = new ToolStripMenuItem(text);
                    item.Tag = seat;
                    item.Click += new EventHandler(item_Click);
                    分配此请求给ToolStripMenuItem.DropDownItems.Add(item);
                }
            }
            else
            {
                dataGridView1.Visible = false;
                splitContainer1.Panel1MinSize = 0;
                splitContainer1.SplitterDistance = 0;

                分配此请求给ToolStripMenuItem.Visible = false;
                toolStripSeparator1.Visible = false;
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个请求的会话！", "分配会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RequestSession session = (RequestSession)dataGridView3.SelectedRows[0].DataBoundItem;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Seat seat = (Seat)item.Tag;

            if (seat.State != OnlineState.Online)
            {
                MessageBox.Show(seat.ShowName + "不在线！", "分配会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                service.AcceptSessions(seat.SeatID, 20, session.SessionID);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            BindRequest();
            BindSeat();
        }

        private void 接受此请求ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选中一个请求的会话！", "分配会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                RequestSession session = (RequestSession)dataGridView3.SelectedRows[0].DataBoundItem;
                service.AcceptSessions(loginSeat.SeatID, 20, session.SessionID);

                IList<P2SSession> list = service.GetP2SSessions(loginSeat.SeatID);
                P2SSession sion = list[list.Count - 1];

                ClientSession s = new ClientSession();
                s.SessionID = sion.SessionID;
                s.CreateID = sion.CreateID;
                s.UserName = sion.User.ShowName;
                s.SeatName = sion.Seat.ShowName;
                s.StartTime = sion.StartTime.ToString();
                s.LastTime = sion.LastReceiveTime.ToString();
                s.Message = sion.RequestMessage;
                s.From = sion.FromAddress;
                s.IP = sion.FromIP;

                BindRequest();
                BindSeat();

                new frmReply(service, s, loginSeat).ShowDialog();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            try
            {
                if (!service.ValidateClient(loginSeat.SeatID, clientID))
                {
                    MessageBox.Show("此用户已经在其它地方登录！", "登录验证", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    isExit = true;
                    Application.Exit();
                }

                BindSeat();
                BindRequest();
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            timer.Start();
        }

        private void BindSeat()
        {
            try
            {
                IList<Seat> list = service.GetCompanySeats(loginSeat.CompanyID);
                IList<ClientSeat> seats = new List<ClientSeat>();
                (list as List<Seat>).ForEach(delegate(Seat seat)
                {
                    if (显示在线ToolStripMenuItem.Checked && seat.State != OnlineState.Online)
                    {
                        return;
                    }

                    ClientSeat s = new ClientSeat();
                    s.SeatID = seat.SeatID;
                    s.ShowName = seat.ShowName;
                    s.SeatName = string.Format("[{0}] {1}", seat.State == OnlineState.Online ? "在线" : "离线", seat.ShowName);
                    s.SessionCount = seat.SessionCount;
                    seats.Add(s);

                    foreach (ToolStripMenuItem item in 分配此请求给ToolStripMenuItem.DropDownItems)
                    {
                        if ((item.Tag as Seat).SeatID != seat.SeatID) continue;
                        string text = seat.ShowName;
                        if (seat.State == OnlineState.Online)
                        {
                            text += " 在线";
                        }
                        item.Text = text;
                    }
                });

                #region 定位客服

                ClientSeat currSeat = null;
                if (dataGridView1.DataSource != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Selected)
                        {
                            currSeat = (ClientSeat)row.DataBoundItem;
                            break;
                        }
                    }
                }

                if (seats.Count == 0) seats = null;
                dataGridView1.DataSource = seats;

                if (dataGridView1.DataSource != null)
                {
                    if (currSeat != null)
                    {
                        bool isexist = false;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (((ClientSeat)row.DataBoundItem).SeatID == currSeat.SeatID)
                            {
                                isexist = true;
                                row.Selected = true;
                                BindSession(currSeat);
                                break;
                            }
                        }

                        if (!isexist)
                        {
                            currSeat = (ClientSeat)dataGridView1.Rows[0].DataBoundItem;
                            BindSession(currSeat);
                        }
                    }
                    else
                    {
                        currSeat = (ClientSeat)dataGridView1.Rows[0].DataBoundItem;
                        BindSession(currSeat);
                    }
                }
                else
                {
                    BindSession(null);
                }

                #endregion

                int count = (list as List<Seat>).FindAll(delegate(Seat s)
                {
                    if (s.State == OnlineState.Online) return true;
                    return false;
                }).Count;

                toolStripStatusLabel1.Text = string.Format("当前信息：登录用户【{0}】", loginSeat.ShowName);
                toolStripStatusLabel1.Text += string.Format("，有{0}个客服在线", count);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindRequest()
        {
            try
            {
                IList<P2CSession> list = service.GetRequestSessions(loginSeat.SeatID, SortType.Vip, 1, 100).DataSource;
                IList<RequestSession> sessions = new List<RequestSession>();
                (list as List<P2CSession>).ForEach(delegate(P2CSession session)
                {
                    RequestSession s = new RequestSession();
                    s.SessionID = session.SessionID;
                    s.UserName = session.User.ShowName;
                    if (session.User.IsVIP)
                    {
                        s.UserName += "(VIP)";
                    }
                    s.RequestSeat = session.RequestCode;
                    s.Message = session.RequestMessage;
                    s.From = session.FromAddress;
                    s.IP = session.FromIP;
                    s.RequestTime = session.StartTime.ToString();
                    sessions.Add(s);
                });

                dataGridView3.DataSource = sessions;

                if (list.Count > 0)
                {
                    toolStripStatusLabel1.Text += string.Format("，有{0}个用户请求", list.Count);
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClientSeat seat = null;
            if (dataGridView1.SelectedRows.Count != 0)
            {
                seat = (ClientSeat)dataGridView1.SelectedRows[0].DataBoundItem;
            }
            MessageCount = -1;
            BindSession(seat);
        }

        private void BindSession(ClientSeat seat)
        {
            try
            {
                if (seat == null)
                {
                    dataGridView2.DataSource = null;
                    BindMessage(null);
                    return;
                }

                IList<P2SSession> list = service.GetP2SSessions(seat.SeatID);
                IList<ClientSession> sessions = new List<ClientSession>();
                (list as List<P2SSession>).ForEach(delegate(P2SSession session)
                {
                    ClientSession s = new ClientSession();
                    s.SessionID = session.SessionID;
                    s.CreateID = session.CreateID;
                    s.UserName = session.User.ShowName;
                    s.SeatName = session.Seat.ShowName;
                    s.StartTime = session.StartTime.ToString();
                    s.LastTime = session.LastReceiveTime.ToString();
                    s.Message = session.RequestMessage;
                    s.From = session.FromAddress;
                    s.IP = session.FromIP;
                    sessions.Add(s);
                });

                #region 定位客服

                ClientSession currSession = null;
                if (dataGridView2.DataSource != null)
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Selected)
                        {
                            currSession = (ClientSession)row.DataBoundItem;
                            break;
                        }
                    }
                }

                if (sessions.Count == 0) sessions = null;
                dataGridView2.DataSource = sessions;

                if (dataGridView2.DataSource != null)
                {
                    if (currSession != null)
                    {
                        bool isexist = false;
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (((ClientSession)row.DataBoundItem).SessionID == currSession.SessionID)
                            {
                                isexist = true;
                                row.Selected = true;
                                BindMessage(currSession);
                                break;
                            }
                        }

                        if (!isexist)
                        {
                            currSession = (ClientSession)dataGridView2.Rows[0].DataBoundItem;
                            BindMessage(currSession);
                        }

                    }
                    else
                    {
                        currSession = (ClientSession)dataGridView2.Rows[0].DataBoundItem;
                        BindMessage(currSession);
                    }
                }
                else
                {
                    BindMessage(null);
                }

                #endregion
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            ClientSession session = null;
            if (dataGridView2.SelectedRows.Count != 0)
            {
                session = (ClientSession)dataGridView2.SelectedRows[0].DataBoundItem;
            }
            MessageCount = -1;
            BindMessage(session);
        }

        private void BindMessage(ClientSession session)
        {
            try
            {
                if (webBrowser1.Document == null || webBrowser1.Document.Body == null)
                {
                    return;
                }

                if (session == null)
                {
                    HtmlElement ele = webBrowser1.Document.GetElementById("title");
                    ele.InnerHtml = "会话不存在";

                    ele = webBrowser1.Document.GetElementById("historyBox");
                    ele.InnerHtml = "选中会话来加载聊天记录！";

                    return;
                }

                string title = string.Format("{0}<font color='red'>{1}</font>与<font color='red'>{2}</font>的聊天记录",
                                 DateTime.Parse(session.StartTime).ToString("yyyy年MM月dd日"),
                                 session.UserName, session.SeatName);

                HtmlElement element = webBrowser1.Document.GetElementById("title");
                element.InnerHtml = title;

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

                        if (MessageCount > 0)
                        {
                            element.ScrollIntoView(false);
                        }

                        MessageCount = list.Count;
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 关闭所有会话ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个客服！", "关闭会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClientSeat seat = (ClientSeat)dataGridView1.SelectedRows[0].DataBoundItem;
            if (seat.SessionCount == 0)
            {
                MessageBox.Show("客服【" + seat.ShowName + "】没有接入会话！", "关闭会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("确定关闭客服【" + seat.ShowName + "】所有会话吗？", "关闭会话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    service.CloseAllSession(seat.SeatID);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                BindSeat();
            }
        }

        private void 显示在线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            显示在线ToolStripMenuItem.Checked = !显示在线ToolStripMenuItem.Checked;

            if (显示在线ToolStripMenuItem.Checked)
            {
                显示在线ToolStripMenuItem.Text = "显示所有";
            }
            else
            {
                显示在线ToolStripMenuItem.Text = "显示在线";
            }

            BindSeat();
        }

        private void 让此客服离线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个客服！", "客服离线", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClientSeat seat = (ClientSeat)dataGridView1.SelectedRows[0].DataBoundItem;
            if (seat.SeatID == loginSeat.SeatID)
            {
                MessageBox.Show("自己不能让自己离线！", "客服离线", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("确定让客服【" + seat.ShowName + "】离线吗？\r\n\r\n注：此功能一般用于客服非正常离线时使用！", "客服离线", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    service.Logout(seat.SeatID);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            BindSeat();
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;
                ClientSeat seat = (ClientSeat)dataGridView1.SelectedRows[0].DataBoundItem;
                Seat seat1 = service.GetSeat(seat.SeatID);
                if (seat1.State == OnlineState.Offline)
                {
                    让此客服离线ToolStripMenuItem.Enabled = false;
                }
                else
                {
                    让此客服离线ToolStripMenuItem.Enabled = true;
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 我来回答ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "回答问题", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClientSession session = (ClientSession)dataGridView2.SelectedRows[0].DataBoundItem;
            new frmReply(service, session, loginSeat).ShowDialog();
        }

        private void 关闭此会话ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "关闭会话", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClientSession session = (ClientSession)dataGridView2.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show("确定关闭与用户【" + session.UserName + "】的会话吗？", "关闭会话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    service.CloseSession(session.SessionID);
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                BindSeat();
            }
        }

        private void frmMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isExit.HasValue && isExit.Value)
            {
                e.Cancel = false;
            }
            else
            {
                if (MessageBox.Show("确定退出当前监控吗？", "退出监控", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        service.Logout(loginSeat.SeatID);
                    }
                    catch (SocketException ex)
                    {
                        MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    isExit = true;
                    Application.Exit();
                }
                else
                {
                    isExit = null;
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (dataGridView1.DataSource == null) return;
            if (e.Button == MouseButtons.Right)
            {
                int row = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                if (row < 0) return;
                dataGridView1.Rows[row].Selected = true;
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (dataGridView2.DataSource == null) return;
            if (e.Button == MouseButtons.Right)
            {
                int row = dataGridView2.HitTest(e.X, e.Y).RowIndex;
                if (row < 0) return;
                dataGridView2.Rows[row].Selected = true;

                ClientSession session = (ClientSession)dataGridView2.Rows[row].DataBoundItem;
                if (session.SeatName == loginSeat.ShowName)
                {
                    我来回答ToolStripMenuItem.Text = "我来回答";
                }
                else
                {
                    我来回答ToolStripMenuItem.Text = string.Format("帮【{0}】回答", session.SeatName);
                }
            }
        }

        private void dataGridView3_MouseDown(object sender, MouseEventArgs e)
        {
            if (dataGridView3.DataSource == null) return;
            if (e.Button == MouseButtons.Right)
            {
                int row = dataGridView3.HitTest(e.X, e.Y).RowIndex;
                if (row < 0) return;
                dataGridView3.Rows[row].Selected = true;
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            我来回答ToolStripMenuItem_Click(sender, e);
        }
    }
}
