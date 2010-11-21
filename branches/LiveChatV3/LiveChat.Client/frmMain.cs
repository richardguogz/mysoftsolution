using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LiveChat.Entity;
using LiveChat.Interface;
using LiveChat.Utils;
using MySoft.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace LiveChat.Client
{
    public partial class frmMain : Form
    {
        //关闭窗口事件
        public event CallbackEventHandler CloseCallback;

        #region private member

        private Color currentColor;
        private Font currentFont;
        private Rectangle rect;

        private ISeatService service;
        private Company company;
        private Seat seat;
        private Guid clientID;
        private Timer chattimer;

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
                IList<P2CSession> list = service.GetP2CSessions(seat.SeatID, SortType.Vip);
                IList<RequestSession> requestSessions = new List<RequestSession>();
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
                    if (!string.IsNullOrEmpty(s.Message)) s.Message = new Regex("<img[^>]+>").Replace(s.Message, "[表情]");
                    s.From = session.FromAddress;
                    s.IP = session.FromIP;
                    s.RequestTime = session.StartTime.ToString();
                    requestSessions.Add(s);
                });

                #region 定位选择的行

                if (requestSessions.Count == 0)
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
                dgvTalks.DataSource = requestSessions;

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

        //显示提示信息
        private void ShowTip(TipInfo tip, CallbackEventHandler handler)
        {
            SingletonMul.Show<frmPopup>(tip.Key, () =>
            {
                frmPopup frm = new frmPopup(tip);
                frm.CallbackView += handler;
                return frm;
            });
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
            if (CloseCallback != null) CloseCallback(null);
            e.Cancel = true;
            this.Hide();
        }

        private void dgvTalks_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTalks.SelectedRows.Count == 0)
            {
                ClientUtils.ShowMessage("请先选中一个会话！");
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
                ClientUtils.ShowMessage("请先选中一个会话！");
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
                ClientUtils.ShowMessage("请先选中一个会话！");
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
                ClientUtils.ShowMessage("请先选中一个会话！");
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
            if (CloseCallback != null) CloseCallback(null);
            this.Hide();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (toolStripButton2.Checked)
            {
                panel2.Visible = false;
                splitContainer1.Visible = true;
                toolStripButton2.Checked = false;
            }
            else
            {
                panel2.Visible = true;
                splitContainer1.Visible = false;
                toolStripButton2.Checked = true;

                string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(seat.CompanyID + "|" + seat.SeatCode));
                string url = Path.Combine(company.ChatWebSite, "admin/analysis.aspx?param=" + param + "&" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                webBrowser1.Navigate(url);
            }
        }
    }
}