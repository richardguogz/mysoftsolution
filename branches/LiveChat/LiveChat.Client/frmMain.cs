using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LiveChat.Entity;
using LiveChat.Interface;
using LiveChat.Utils;
using MySoft.Core;
using System.Text;

namespace LiveChat.Client
{
    public partial class frmMain : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        //关闭窗口事件
        public event CallbackEventHandler CallbackClose;

        //会话事件
        public event CallbackEventHandler CallbackSession;

        /// <summary>
        /// 显示消息
        /// </summary>
        public event ShowTipEventHandler CallbackShowTip;

        #region private member

        private Color currentColor;
        private Font currentFont;
        private Rectangle rect;

        private ISeatService service;
        private Company company;
        private Seat seat;
        private Guid clientID;
        private Timer chattimer, msgtimer;
        private List<RequestSession> reqSessions;

        #endregion

        public frmMain(ISeatService service, Company company, Seat seat, Guid clientID, Font useFont, Color useColor)
            : this(service, company, seat, clientID, useFont, useColor, Rectangle.Empty)
        { }

        public frmMain(ISeatService service, Company company, Seat seat, Guid clientID, Font useFont, Color useColor, Rectangle rect)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.clientID = clientID;
            this.currentFont = useFont;
            this.currentColor = useColor;
            this.rect = rect;

            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                if (rect != Rectangle.Empty)
                {
                    if (rect.X < 1024) this.Left = rect.X;
                    if (rect.Top < 768) this.Top = rect.Y;
                    if (rect.Width < 1024) this.Width = rect.Width;
                    if (rect.Height < 768) this.Height = rect.Height;
                }

                dgvTalks.AutoGenerateColumns = false;
                dgvClients.AutoGenerateColumns = false;
                cboTalkCount.SelectedIndex = 0;

                chattimer = new Timer();
                chattimer.Interval = 10000;
                chattimer.Tick += new EventHandler(chattimer_Tick);
                chattimer.Start();

                msgtimer = new Timer();
                msgtimer.Interval = 30000;
                msgtimer.Tick += new EventHandler(msgtimer_Tick);
                msgtimer.Start();

                int requestCount = BindRequest();
                int sessionCount = BindSession();

                tssiCurrentCompany.Text = company.CompanyName;
                tssiCurrentUser.Text = seat.SeatName;

                tssiInfo.Text = string.Format("有{0}个会话请求", requestCount);
                tssiInfo.Text += string.Format("，当前有{0}个会话", sessionCount);
            }
            catch (LiveChatException ex)
            {
                ClientUtils.ShowError(ex);
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError("系统初始化错误，请与开发商联系！", ex);
            }
        }

        //定时处理消息
        void msgtimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed)
            {
                msgtimer.Stop();
                msgtimer = null;
                return;
            }

            //如果选择不提示，则直接返回
            if (!checkBox2.Checked) return;

            msgtimer.Stop();

            try
            {
                var info = service.GetSeatMessage(seat.SeatID);

                StringBuilder sb = new StringBuilder();

                //会话消息
                foreach (KeyValuePair<P2SSession, int> kv in info.SessionMessages)
                {
                    if (kv.Key.Seat == null) continue;

                    if (kv.Value > 0)
                    {
                        string msgText = string.Format("访客【{0}】给您发送了【{1}】条新消息！", kv.Key.User.UserName, kv.Value);
                        sb.AppendLine(msgText);
                    }
                }

                //客服消息
                foreach (KeyValuePair<Seat, MessageInfo> kv in info.SeatMessages)
                {
                    if (kv.Value.Count > 0)
                    {
                        string msgText = string.Format("客服【{0}】给您发送了【{1}】条新消息！", kv.Key.SeatName, kv.Value.Count);
                        sb.AppendLine(msgText);
                    }
                }

                //群消息
                foreach (KeyValuePair<SeatGroup, int> kv in info.GroupMessages)
                {
                    if (kv.Value > 0)
                    {
                        string msgText = string.Format("群【{0}】发送了【{1}】条新消息！", kv.Key.GroupName, kv.Value);
                        sb.AppendLine(msgText);
                    }
                }

                if (sb.Length > 0)
                {
                    if (CallbackShowTip != null)
                    {
                        CallbackShowTip("您有新的消息", sb.ToString(), ToolTipIcon.Info, (p1, p2) =>
                        {
                            Singleton.Show<frmNavigate>();
                        });
                    }

                    FlashWindow(this.Handle, true);
                }

                //设置在线
                SetOnlineState(true);
            }
            catch (SocketException ex)
            {
                SetOnlineState(false);
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }

            msgtimer.Start();
        }

        void SetOnlineState(bool state)
        {
            if (state)
            {
                tssiClientState.Text = "联机";
                tssiClientState.BackColor = SystemColors.Control;
                tssiClientState.ForeColor = SystemColors.WindowText;
                splitContainer1.Enabled = true;
                toolStrip1.Enabled = true;
            }
            else
            {
                tssiClientState.Text = "脱机";
                tssiClientState.BackColor = Color.Red;
                tssiClientState.ForeColor = Color.White;
                splitContainer1.Enabled = false;
                toolStrip1.Enabled = false;
            }

            //设置在线状态
            ClientUtils.SoftwareIsOnline = state;
        }

        void chattimer_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed)
            {
                chattimer.Stop();
                chattimer = null;
                return;
            }

            //定时加载会话信息
            chattimer.Stop();

            try
            {
                #region 检测客服端

                bool isSuccess = service.ValidateClient(seat.SeatID, clientID);
                if (!isSuccess)
                {
                    if (msgtimer != null) msgtimer.Stop();
                    if (chattimer != null) chattimer.Stop();

                    ClientUtils.SoftwareOtherLogin = true;

                    MessageBox.Show("您当前的ID号在其它地方登录，将强制退出！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.ExitThread();
                    Application.Exit();

                    return;
                }

                #endregion

                if (chkAutoAccept.Checked)
                {
                    int count = Convert.ToInt32(cboTalkCount.Text);
                    var session = service.AutoAcceptSession(seat.SeatID, count);

                    if (session != null)
                    {
                        SingletonMul.Show(session.SessionID, () =>
                        {
                            frmChat chat = new frmChat(service, session, company, seat, currentFont, currentColor);
                            chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                            chat.Callback += new CallbackEventHandler(chat_Callback);
                            return chat;
                        });
                    }
                }

                int requestCount = BindRequest();
                int sessionCount = BindSession();

                tssiInfo.Text = string.Format("当前有{0}个会话请求", requestCount);
                tssiInfo.Text += string.Format("，有{0}个正在进行中的会话", sessionCount);
            }
            catch (SocketException ex)
            {
                SetOnlineState(false);
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }

            chattimer.Start();
        }

        private int BindRequest()
        {
            try
            {
                bool exists = false;
                IList<P2CSession> list = service.GetRequestSessions(seat.SeatID, SortType.Vip, 1, 100).DataSource;
                if (reqSessions == null)
                {
                    reqSessions = new List<RequestSession>();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (P2CSession s in list)
                    {
                        if (!reqSessions.Exists(p => p.SessionID == s.SessionID))
                        {
                            exists = true;

                            string msgText = string.Format("访客【{0}】发送请求，请及时处理！\r\n来自：{1}", s.User.UserName, s.FromAddress);
                            sb.AppendLine(msgText);
                        }
                    }

                    if (sb.Length > 0)
                    {
                        if (CallbackShowTip != null)
                        {
                            CallbackShowTip("您有新的请求", sb.ToString(), ToolTipIcon.Info, (p1, p2) =>
                            {
                                Singleton.Show<frmNavigate>();
                            });
                        }

                        //闪屏
                        FlashWindow(this.Handle, true);
                    }

                    reqSessions.Clear();
                }

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
                    reqSessions.Add(s);
                });

                if (exists)
                {
                    //回调事件
                    if (CallbackSession != null) CallbackSession(list);
                }

                #region 定位选择的行

                if (reqSessions.Count == 0)
                {
                    dgvTalks.DataSource = null;
                    return 0;
                }

                ClientSession currSession = null;
                if (dgvTalks.DataSource != null)
                {
                    foreach (DataGridViewRow row in dgvTalks.Rows)
                    {
                        if (row.Selected)
                        {
                            currSession = (ClientSession)row.DataBoundItem;
                            break;
                        }
                    }
                }

                dgvTalks.DataSource = null;
                dgvTalks.DataSource = reqSessions;

                if (dgvTalks.DataSource != null)
                {
                    if (currSession != null)
                    {
                        foreach (DataGridViewRow row in dgvTalks.Rows)
                        {
                            if (((ClientSession)row.DataBoundItem).SessionID == currSession.SessionID)
                            {
                                row.Selected = true;
                                tsbAcceptTalk.Enabled = true;
                                tsbEndTalk.Enabled = false;
                                break;
                            }
                        }
                    }
                }

                #endregion

                return list.Count;
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
                return 0;
            }
        }

        private int BindSession()
        {
            try
            {
                IList<P2SSession> list = service.GetP2SSessions(seat.SeatID);
                IList<ClientSession> talkSessions = new List<ClientSession>();
                (list as List<P2SSession>).ForEach(delegate(P2SSession session)
                {
                    ClientSession s = new ClientSession();
                    s.SessionID = session.SessionID;
                    s.CreateID = session.CreateID;
                    s.UserName = session.User.ShowName;
                    s.SeatName = session.Seat.ShowName;
                    s.StartTime = session.StartTime.ToString();
                    s.LastTime = session.LastReceiveTime.ToString();
                    s.Message = string.Format("【{0}】条未读", session.NoReadMessageCount);
                    s.From = session.FromAddress;
                    s.IP = session.FromIP;
                    talkSessions.Add(s);
                });

                //回调事件
                if (CallbackSession != null) CallbackSession(reqSessions);

                #region 定位选择的行

                if (talkSessions.Count == 0)
                {
                    dgvClients.DataSource = null;

                    return 0;
                }

                ClientSession currSession = null;
                if (dgvClients.DataSource != null)
                {
                    foreach (DataGridViewRow row in dgvClients.Rows)
                    {
                        if (row.Selected)
                        {
                            currSession = (ClientSession)row.DataBoundItem;
                            break;
                        }
                    }
                }

                dgvClients.DataSource = null;
                dgvClients.DataSource = talkSessions;

                if (dgvClients.DataSource != null)
                {
                    if (currSession != null)
                    {
                        foreach (DataGridViewRow row in dgvClients.Rows)
                        {
                            if (((ClientSession)row.DataBoundItem).SessionID == currSession.SessionID)
                            {
                                row.Selected = true;
                                tsbAcceptTalk.Enabled = false;
                                tsbEndTalk.Enabled = true;
                                break;
                            }
                        }
                    }
                }

                return list.Count;

                #endregion
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
                return 0;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (CallbackClose != null) CallbackClose(null);
            this.Hide();
        }

        private void dgvTalks_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTalks.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RequestSession session = (RequestSession)dgvTalks.SelectedRows[0].DataBoundItem;
            P2SSession s = service.GetSession(session.SessionID) as P2SSession;

            if (s != null)
            {
                SingletonMul.Show(s.SessionID, () =>
                {
                    frmChat chat = new frmChat(service, s, company, seat, currentFont, currentColor);
                    chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                    chat.Callback += new CallbackEventHandler(chat_Callback);
                    return chat;
                });
            }
        }

        void chat_CallbackFontColor(Font font, Color color)
        {
            this.currentFont = font;
            this.currentColor = color;
        }

        void chat_Callback(object obj)
        {
            try
            {
                if (obj != null && obj.GetType().IsArray)
                {
                    string[] ids = (string[])obj;

                    //将之前的窗口进行替换
                    SingletonMul.RenameKey(ids[0], ids[1]);
                }
                else if (obj != null && obj.GetType() == typeof(string))
                {
                    SingletonMul.RemoveKey(obj.ToString());
                }

                BindRequest();
                BindSession();
            }
            catch { }
        }

        private void dgvClients_DoubleClick(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClientSession session = (ClientSession)dgvClients.SelectedRows[0].DataBoundItem;
            P2SSession s = service.GetSession(session.SessionID) as P2SSession;

            if (s != null)
            {
                SingletonMul.Show(s.SessionID, () =>
                {
                    frmChat chat = new frmChat(service, s, company, seat, currentFont, currentColor);
                    chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                    chat.Callback += new CallbackEventHandler(chat_Callback);
                    return chat;
                });
            }
        }

        private void dgvTalks_Click(object sender, EventArgs e)
        {
            if (dgvTalks.SelectedRows.Count == 0)
            {
                tsbAcceptTalk.Enabled = false;
                tsbEndTalk.Enabled = false;
                return;
            }

            tsbAcceptTalk.Enabled = true;
            tsbEndTalk.Enabled = false;
        }

        private void dgvClients_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count == 0)
            {
                tsbAcceptTalk.Enabled = false;
                tsbEndTalk.Enabled = false;
                return;
            }

            tsbAcceptTalk.Enabled = false;
            tsbEndTalk.Enabled = true;
        }

        /// <summary>
        /// 查看消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbChatMessage_Click(object sender, EventArgs e)
        {
            ClientSession session = null;
            if (dgvTalks.SelectedRows.Count > 0)
            {
                session = (ClientSession)dgvTalks.SelectedRows[0].DataBoundItem;
            }
            else if (dgvClients.SelectedRows.Count > 0)
            {
                session = (ClientSession)dgvClients.SelectedRows[0].DataBoundItem;
            }

            if (session != null)
            {
                P2SSession s = service.GetSession(session.SessionID) as P2SSession;
                string key = "Message" + s.SessionID;
                SingletonMul.Show(key, () =>
                {
                    frmMessage frm = new frmMessage(service, company, seat, s);
                    return frm;
                });
            }
            else
            {
                Singleton.Show(() =>
                {
                    frmMessage frm = new frmMessage(service, company, seat);
                    return frm;
                });
            }
        }

        private void tsbAcceptTalk_Click(object sender, EventArgs e)
        {
            ClientSession session = null;
            if (dgvTalks.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            session = (ClientSession)dgvTalks.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show("确定接受当前选择的会话吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            try
            {
                //接入会话
                P2SSession p2session = service.AcceptSession(seat.SeatID, ClientUtils.MaxAcceptCount, session.SessionID);
                BindRequest();

                //弹出窗口
                SingletonMul.Show(p2session.SessionID, () =>
                {
                    frmChat chat = new frmChat(service, p2session, company, seat, currentFont, currentColor);
                    chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                    chat.Callback += new CallbackEventHandler(chat_Callback);
                    return chat;
                });
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void tsbEndTalk_Click(object sender, EventArgs e)
        {
            ClientSession session = null;
            if (dgvClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中一个会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            session = (ClientSession)dgvClients.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show("确定结束当前选择的会话吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            try
            {
                //接入会话
                service.CloseSession(session.SessionID);

                //关闭打开的窗口
                SingletonMul.Close<frmChat>(session.SessionID);

                BindSession();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void tsbLeave_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmLeaveManager frm = new frmLeaveManager(service, company.CompanyID);
                return frm;
            });
        }

        private void DisplayAllForm(bool visible)
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.Visible = visible;
            }

            System.Threading.Thread.Sleep(500);
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            if (CallbackClose != null) CallbackClose(null);
            this.Hide();
        }
    }
}