using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;
using MySoft.Core;
using System.Runtime.InteropServices;
using System.IO;
using MySoft.Data;
using System.Diagnostics;
using System.Configuration;

namespace LiveChat.Client
{
    public partial class frmNavigate : Form
    {
        public event CallbackEventHandler Callback;

        #region private member

        private const int maxAcceptCount = 20;

        private bool isRelogin;
        private Color currentColor;
        private Font currentFont;
        private Point point;

        private ISeatService service;
        private Company loginCompany;
        private Seat loginSeat;
        private Guid clientID;
        private IList<Area> areaList;
        private Timer refreshtime;
        private SeatMessage seatInfo;
        private bool isMainShow = false;
        private IList<SeatFriend> myfriends;
        private Timer urlTimer;

        #endregion

        public frmNavigate(ISeatService service, Company company, Seat seat, Guid clientID, Point point)
        {
            this.service = service;
            this.loginCompany = company;
            this.loginSeat = seat;
            this.clientID = clientID;
            this.point = point;

            InitializeComponent();
        }

        private void frmNavigate_Load(object sender, EventArgs e)
        {
            try
            {
                InitSystemInfo();

                //绑定请求
                BindRequest();

                //绑定会话
                BindSession();

                //绑定客服组
                BindSeatGroup();

                //绑定我的好友
                BindSeatFriend();

                #region 判断是否管理员

                //只有系统管理员才能设置
                if (loginSeat.SeatType == SeatType.Normal)
                {
                    系统设置ToolStripMenuItem.Visible = false;
                    //tsbLeave.Visible = false;
                }

                #endregion

                urlTimer = new Timer();
                urlTimer.Interval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                urlTimer.Tick += new EventHandler(urlTimer_Tick);
                urlTimer.Start();

                isRelogin = false;
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        void urlTimer_Tick(object sender, EventArgs e)
        {
            Singleton.Show<frmMini>(() =>
            {
                urlTimer.Stop();
                string url = ConfigurationManager.AppSettings["DefaultPageUrl"];
                if (string.IsNullOrEmpty(url))
                {
                    url = "http://www.globalfabric.com/mini.asp";
                }
                string xy = ConfigurationManager.AppSettings["DefaultPageSize"];
                Size size = new Size(560, 580);
                if (!string.IsNullOrEmpty(xy))
                {
                    var arr = xy.Split('*');
                    size = new Size(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
                }
                frmMini frm = new frmMini(url);
                frm.MaximumSize = size;
                frm.MinimumSize = size;
                frm.Size = size;
                return frm;
            });
        }

        //设置提示信息
        //void main_CallbackShowTip(string title, string text, ToolTipIcon icon, EventHandler handler)
        //{
        //    notifyIcon1.BalloonTipTitle = title;
        //    notifyIcon1.BalloonTipText = text;
        //    notifyIcon1.BalloonTipIcon = icon;
        //    notifyIcon1.BalloonTipClicked += handler;
        //    notifyIcon1.ShowBalloonTip(1000);
        //}

        private void InitSystemInfo()
        {
            this.Left = point.X;
            this.Top = point.Y;
            this.button3.ImageAlign = ContentAlignment.BottomLeft;

            int width = (tabControl1.Width - 5) / 3;
            tabControl1.ItemSize = new Size(width, 24);

            ImageList imgList2 = new ImageList();
            imgList2.ImageSize = new Size(1, 24);       //分别是宽和高
            lvSearchName.SmallImageList = imgList2;     //这里设置listView的SmallImageList 
            lvSearchName.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            lblUserName.Text = string.Format("{0}(在线)", loginSeat.SeatName);
            tsUserName.Text = string.Empty;
            if (loginSeat.FaceImage != null)
            {
                MemoryStream ms = new MemoryStream(loginSeat.FaceImage);
                Image img = BitmapManipulator.ResizeBitmap((Bitmap)Bitmap.FromStream(ms), 60, 60);
                pbSeatFace.Image = img;
            }
            lblSign.Text = loginSeat.Sign;
            toolTip1.SetToolTip(lblSign, loginSeat.Sign);
            tsmiExit.Text = string.Format("退出系统【{0}】", loginSeat.ShowName);

            tvSession.HideSelection = false;
            tvSession.MouseDown += new MouseEventHandler(tvSession_MouseDown);
            tvLinkman.HideSelection = false;
            tvLinkman.MouseDown += new MouseEventHandler(tvLinkman_MouseDown);
            tvSeatGroup.HideSelection = false;
            tvSeatGroup.MouseDown += new MouseEventHandler(tvSeatGroup_MouseDown);

            //获取地区信息
            areaList = service.GetAreas();

            refreshtime = new Timer();
            refreshtime.Interval = 10000;
            refreshtime.Tick += new EventHandler(refreshtime_Tick);
            refreshtime.Start();
        }

        void tvSeatGroup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = tvSeatGroup.GetNodeAt(e.X, e.Y);
                if (node != null && node.Tag != null)
                {
                    tvSeatGroup.SelectedNode = node;
                }
            }
        }

        void tvLinkman_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = tvLinkman.GetNodeAt(e.X, e.Y);
                if (node != null && node.Tag != null)
                {
                    tvLinkman.SelectedNode = node;
                }
            }
        }

        void tvSession_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = tvSession.GetNodeAt(e.X, e.Y);
                if (node != null && node.Tag != null)
                {
                    tvSession.SelectedNode = node;
                }
            }
        }

        void refreshtime_Tick(object sender, EventArgs e)
        {
            refreshtime.Stop();

            try
            {
                #region 检测客服端

                bool isSuccess = service.ValidateClient(loginSeat.SeatID, clientID);
                if (!isSuccess)
                {
                    isRelogin = true;

                    this.Close();
                    ClientUtils.ShowMessage("您当前的ID号在其它地方登录，将强制退出！");
                    ClientUtils.ExitApplication();

                    return;
                }

                #endregion

                //处理定时刷新
                seatInfo = service.GetSeatMessage(loginSeat.SeatID);

                //绑定请求
                RefreshRequest();

                //绑定会话
                RefreshSession();

                //绑定客服组
                RefreshSeatGroup();

                //绑定我的好友
                RefreshSeatFriend();

                //显示消息提示
                ShowMessage();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);

                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            refreshtime.Start();
        }

        private void ShowMessage()
        {
            IList<TipInfo> tiplist = new List<TipInfo>();

            //请求信息
            foreach (KeyValuePair<Seat, RequestInfo> kv in seatInfo.RequestMessages)
            {
                if (kv.Value.ConfirmState == 0)
                {
                    string msgText = string.Format("客服【{0}】请求加您为好友！", kv.Key.ShowName);

                    StringBuilder sbMsg = new StringBuilder(msgText);
                    sbMsg.AppendLine();
                    sbMsg.AppendLine();

                    sbMsg.AppendFormat("[{0}]" + kv.Value.Request, kv.Value.AddTime.ToLongTimeString());
                    sbMsg.AppendLine();

                    TipInfo tip = new TipInfo() { Title = "请求添加好友", Message = sbMsg.ToString() };
                    tip.Id = string.Format("Message_{0}", kv.Value.ID);
                    tip.Key = string.Format("SeatRequest_{0}_{1}", loginSeat.SeatID, kv.Key.SeatID);
                    tip.Target = kv;

                    tiplist.Add(tip);
                }
            }

            //会话消息
            foreach (KeyValuePair<P2SSession, MessageInfo> kv in seatInfo.SessionMessages)
            {
                if (kv.Value.Count > 0)
                {
                    if (kv.Key.Seat == null)
                    {
                        string msgText = string.Format("访客【{0}】请求与您会话！\t来自：{1}", kv.Key.User.UserName, kv.Key.FromAddress);

                        StringBuilder sbMsg = new StringBuilder(msgText);
                        sbMsg.AppendLine();
                        sbMsg.AppendLine();

                        sbMsg.AppendFormat("[{0}]" + kv.Value.Message.Content, kv.Value.Message.SendTime.ToLongTimeString());
                        sbMsg.AppendLine();

                        TipInfo tip = new TipInfo() { Title = "您有新的请求", Message = sbMsg.ToString() };
                        tip.Id = string.Format("Message_{0}", kv.Key.SessionID);
                        tip.Key = string.Format("UserChat_{0}_{1}", loginSeat.SeatID, kv.Key.User.UserID);
                        tip.Target = kv.Key;

                        tiplist.Add(tip);
                    }
                    else
                    {
                        string msgText = string.Format("访客【{0}】给您发送了【{1}】条新消息！", kv.Key.User.UserName, kv.Value.Count);

                        StringBuilder sbMsg = new StringBuilder(msgText);
                        sbMsg.AppendLine();
                        sbMsg.AppendLine();

                        sbMsg.AppendFormat("[{0}]" + kv.Value.Message.Content, kv.Value.Message.SendTime.ToLongTimeString());
                        sbMsg.AppendLine();

                        TipInfo tip = new TipInfo() { Title = "您有新的消息", Message = sbMsg.ToString() };
                        tip.Id = string.Format("Message_{0}", kv.Value.Message.ID);
                        tip.Key = string.Format("UserChat_{0}_{1}", loginSeat.SeatID, kv.Key.User.UserID);
                        tip.Target = kv.Key;

                        tiplist.Add(tip);
                    }
                }
            }

            //客服消息
            foreach (KeyValuePair<SeatFriend, SeatInfo> kv in seatInfo.SeatMessages)
            {
                if (kv.Value.Count > 0)
                {
                    string msgText = string.Format("客服【{0}】给您发送了【{1}】条新消息！", kv.Key.SeatName, kv.Value.Count);

                    StringBuilder sbMsg = new StringBuilder(msgText);
                    sbMsg.AppendLine();
                    sbMsg.AppendLine();

                    sbMsg.AppendFormat("[{0}]" + kv.Value.Message.Content, kv.Value.Message.SendTime.ToLongTimeString());
                    sbMsg.AppendLine();

                    TipInfo tip = new TipInfo() { Title = "您有新的消息", Message = sbMsg.ToString() };
                    tip.Id = string.Format("Message_{0}", kv.Value.Message.ID);
                    tip.Key = string.Format("SeatChat_{0}_{1}", loginSeat.SeatID, kv.Key.SeatID);
                    tip.Target = kv.Key;

                    tiplist.Add(tip);
                }
            }

            //群消息
            foreach (KeyValuePair<SeatGroup, MessageInfo> kv in seatInfo.GroupMessages)
            {
                if (kv.Value.Count > 0)
                {
                    string msgText = string.Format("群【{0}】发送了【{1}】条新消息！", kv.Key.GroupName, kv.Value.Count);

                    StringBuilder sbMsg = new StringBuilder(msgText);
                    sbMsg.AppendLine();
                    sbMsg.AppendLine();

                    sbMsg.AppendFormat("[{0}]" + kv.Value.Message.Content, kv.Value.Message.SendTime.ToLongTimeString());
                    sbMsg.AppendLine();

                    TipInfo tip = new TipInfo() { Title = "您有新的消息", Message = sbMsg.ToString() };
                    tip.Id = string.Format("Message_{0}", kv.Value.Message.ID);
                    tip.Key = string.Format("GroupChat_{0}_{1}", loginSeat.SeatID, kv.Key.GroupID);
                    tip.Target = kv.Key;

                    tiplist.Add(tip);
                }
            }

            if (tiplist.Count > 0)
            {
                foreach (var tip in tiplist)
                {
                    ShowTip(tip, p1 =>
                    {
                        if (tip.Target is P2SSession)
                        {
                            P2SSession session = tip.Target as P2SSession;

                            //会话不存了就返回
                            if (service.GetSession(session.SessionID) == null)
                            {
                                ClientUtils.ShowMessage("当前会话已经被客服接受或会话已失效！");
                                return;
                            }

                            SingletonMul.Show(tip.Key, () =>
                            {
                                frmChat chat = new frmChat(service, session, loginCompany, loginSeat, currentFont, currentColor);
                                chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                                chat.Callback += new CallbackEventHandler(chat_Callback);
                                return chat;
                            });
                        }
                        else if (tip.Target is SeatFriend)
                        {
                            SeatFriend toSeat = tip.Target as SeatFriend;
                            SingletonMul.Show(tip.Key, () =>
                            {
                                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, toSeat, toSeat.MemoName, currentFont, currentColor);
                                frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                                return frmSeatChat;
                            });
                        }
                        else if (tip.Target is SeatGroup)
                        {
                            SeatGroup group = tip.Target as SeatGroup;
                            SingletonMul.Show(tip.Key, () =>
                            {
                                frmGroupChat frmGroupChat = new frmGroupChat(service, loginCompany, loginSeat, group, currentFont, currentColor);
                                frmGroupChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                                frmGroupChat.Callback += new CallbackEventHandler(frmGroupChat_Callback);
                                return frmGroupChat;
                            });
                        }
                        else if (tip.Target is KeyValuePair<Seat, RequestInfo>)
                        {
                            KeyValuePair<Seat, RequestInfo> request = (KeyValuePair<Seat, RequestInfo>)tip.Target;
                            SingletonMul.Show<frmConfirmFriend>(tip.Key, () =>
                            {
                                frmConfirmFriend frmFriend = new frmConfirmFriend(service, loginCompany, request.Key, request.Value);
                                frmFriend.Callback += new CallbackEventHandler(frmFriend_Callback);
                                return frmFriend;
                            });
                        }
                    }, p2 =>
                    {
                        if (tip.Target is P2SSession)
                        {
                            P2SSession session = tip.Target as P2SSession;
                            if (session.Seat != null)
                                service.GetP2SMessages(session.SessionID);
                        }
                        else if (tip.Target is SeatFriend)
                        {
                            SeatFriend toSeat = tip.Target as SeatFriend;
                            service.GetS2SHistoryMessages(loginSeat.SeatID, toSeat.SeatID);
                        }
                        else if (tip.Target is SeatGroup)
                        {
                            SeatGroup group = tip.Target as SeatGroup;
                            service.GetSGHistoryMessages(group.GroupID, loginSeat.SeatID);
                        }
                    });
                }
            }
        }

        //显示提示信息
        private void ShowTip(TipInfo tip, CallbackEventHandler viewHandler, CallbackEventHandler cancelHandler)
        {
            //如果窗口已经打开，则返回
            if (SingletonMul.ExistForm(tip.Key)) return;

            SingletonMul.Show<frmPopup>(tip.Id, () =>
            {
                frmPopup frm = new frmPopup(tip);
                frm.CallbackView += viewHandler;
                frm.CallbackCancel += cancelHandler;
                return frm;
            });
        }

        void frmFriend_Callback(object obj)
        {
            //重新绑定好友
            BindSeatFriend();

            //处理定时刷新
            seatInfo = service.GetSeatMessage(loginSeat.SeatID);
        }

        private void RefreshRequest()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.SessionMessages)
            {
                if (kv.Key.Seat != null) continue;

                TreeNode tn = FindTreeNode<P2SSession>(tvSession.Nodes[0], kv.Key, "SessionID");
                if (tn != null)
                {
                    if (kv.Value.Count > 0)
                        tn.Text = string.Format("[{1}]{0}({2})【{3}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Value.Count, kv.Key.FromAddress);
                    else
                        tn.Text = string.Format("[{1}]{0}【{2}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Key.FromAddress);

                    tn.Tag = kv.Key;
                }
                else
                {
                    tn = new TreeNode(string.Format("[{1}]{0}({2})【{3}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Value.Count, kv.Key.FromAddress));
                    tn.SelectedImageIndex = 5;
                    tn.ImageIndex = 5;
                    tn.Tag = kv.Key;
                    tn.ContextMenuStrip = contextMenuStrip5;
                    tvSession.Nodes[0].Nodes.Insert(0, tn);
                }
            }

            if (tvSession.Nodes[0].Nodes.Count > 0)
            {
                tvSession.Nodes[0].Text = string.Format("会话请求({0})", tvSession.Nodes[0].Nodes.Count);
                tvSession.Nodes[0].Expand();
            }
            else
            {
                tvSession.Nodes[0].Text = "会话请求(0)";
            }
        }

        private void BindRequest()
        {
            var list = service.GetP2CSessions(loginSeat.SeatID, SortType.Vip);
            TreeNode node = tvSession.Nodes[0];
            node.Nodes.Clear();
            node.Text = string.Format("会话请求({0})", list.Count);
            foreach (var info in list)
            {
                TreeNode tn = new TreeNode(string.Format("[{1}]{0}({2})【{3}】", info.User.UserID, info.LastReceiveTime.ToString("HH:mm"), info.NoReadMessageCount, info.FromAddress));
                tn.Text = tn.Text.Replace("(0)", "");
                tn.SelectedImageIndex = 5;
                tn.ImageIndex = 5;
                tn.Tag = info;
                tn.ContextMenuStrip = contextMenuStrip5;
                node.Nodes.Add(tn);
            }

            if (list.Count > 0) node.Expand();
        }

        private void RefreshSession()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.SessionMessages)
            {
                if (kv.Key.Seat == null) continue;

                TreeNode tn = FindTreeNode<P2SSession>(tvSession.Nodes[1], kv.Key, "SessionID");
                if (tn != null)
                {
                    if (kv.Value.Count > 0)
                        tn.Text = string.Format("[{1}]{0}({2})【{3}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Value.Count, kv.Key.FromAddress);
                    else
                        tn.Text = string.Format("[{1}]{0}【{2}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Key.FromAddress);

                    tn.Tag = kv.Key;
                }
                else
                {
                    tn = new TreeNode(string.Format("[{1}]{0}({2})【{3}】", kv.Key.User.UserID, kv.Key.LastReceiveTime.ToString("HH:mm"), kv.Value.Count, kv.Key.FromAddress));
                    tn.SelectedImageIndex = 5;
                    tn.ImageIndex = 5;
                    tn.Tag = kv.Key;
                    tn.ContextMenuStrip = contextMenuStrip4;
                    tvSession.Nodes[1].Nodes.Add(tn);
                }
            }

            if (tvSession.Nodes[1].Nodes.Count > 0)
            {
                tvSession.Nodes[1].Text = string.Format("客服会话({0})", tvSession.Nodes[1].Nodes.Count);
                tvSession.Nodes[1].Expand();
            }
            else
            {
                tvSession.Nodes[1].Text = "客服会话(0)";
            }
        }

        private void BindSession()
        {
            var list = service.GetP2SSessions(loginSeat.SeatID);
            TreeNode node = tvSession.Nodes[1];
            node.Nodes.Clear();
            node.Text = string.Format("客服会话({0})", list.Count);
            foreach (var info in list)
            {
                TreeNode tn = new TreeNode(string.Format("[{1}]{0}({2})【{3}】", info.User.UserID, info.LastReceiveTime.ToString("HH:mm"), info.NoReadMessageCount, info.FromAddress));
                tn.Text = tn.Text.Replace("(0)", "");
                tn.SelectedImageIndex = 5;
                tn.ImageIndex = 5;
                tn.Tag = info;
                tn.ContextMenuStrip = contextMenuStrip4;
                node.Nodes.Add(tn);
            }

            if (list.Count > 0) node.Expand();
        }

        private void RefreshSeatGroup()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.GroupMessages)
            {
                TreeNode tn = FindTreeNode<SeatGroup>(tvSeatGroup, kv.Key, "GroupID");
                if (tn != null)
                {
                    if (kv.Value.Count > 0)
                        tn.Text = string.Format("{0} [{1}/{2}]({3})", kv.Key.MemoName ?? kv.Key.GroupName,
                            kv.Key.PersonOnlineCount, kv.Key.PersonCount, kv.Value.Count);
                    else
                        tn.Text = string.Format("{0} [{1}/{2}]", kv.Key.MemoName ?? kv.Key.GroupName,
                            kv.Key.PersonOnlineCount, kv.Key.PersonCount);

                    tn.Tag = kv.Key;
                }
            }
        }

        private void BindSeatGroup()
        {
            tvSeatGroup.ContextMenuStrip = contextMenuStrip9;
            tvSeatGroup.Nodes.Clear();

            IList<SeatGroup> list = service.GetSeatGroups(loginSeat.SeatID);
            foreach (var group in list)
            {
                TreeNode tn = new TreeNode(string.Format("{0} [{1}/{2}]", group.MemoName ?? group.GroupName,
                    group.PersonOnlineCount, group.PersonCount));

                tn.SelectedImageIndex = 2;
                tn.ImageIndex = 2;
                tn.ContextMenuStrip = contextMenuStrip8;

                if (group.CreateID == loginSeat.SeatID)
                {
                    退出群QToolStripMenuItem.Visible = false;
                    解散群JToolStripMenuItem.Visible = true;
                }
                else
                {
                    退出群QToolStripMenuItem.Visible = true;
                    解散群JToolStripMenuItem.Visible = false;
                }

                tn.Tag = group;

                tvSeatGroup.Nodes.Add(tn);
            }
        }

        private void RefreshSeatFriend()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.SeatMessages)
            {
                TreeNode tn = FindTreeNode<SeatFriend>(tvLinkman, kv.Key, "SeatID");
                if (tn != null)
                {
                    if (!string.IsNullOrEmpty(kv.Value.MemoName))
                    {
                        if (kv.Value.Count > 0)
                            tn.Text = string.Format("{0}({1}) {2}", kv.Value.MemoName, kv.Value.Count, kv.Key.Sign);
                        else
                            tn.Text = string.Format("{0} {1}", kv.Value.MemoName, kv.Key.Sign);
                    }
                    else
                    {
                        if (kv.Value.Count > 0)
                            tn.Text = string.Format("{0}({1}) {2}", kv.Key.ShowName, kv.Value.Count, kv.Key.Sign);
                        else
                            tn.Text = string.Format("{0} {1}", kv.Key.ShowName, kv.Key.Sign);
                    }

                    if (kv.Key.State != (tn.Tag as Seat).State)
                    {
                        if (kv.Key.State == OnlineState.Online)
                        {
                            tn.SelectedImageIndex = 0;
                            tn.ImageIndex = 0;
                        }
                        else if (kv.Key.State == OnlineState.Busy)
                        {
                            tn.SelectedImageIndex = 6;
                            tn.ImageIndex = 6;
                        }
                        else if (kv.Key.State == OnlineState.Leave)
                        {
                            tn.SelectedImageIndex = 7;
                            tn.ImageIndex = 7;
                        }
                        else
                        {
                            tn.SelectedImageIndex = 1;
                            tn.ImageIndex = 1;
                        }

                        tn.Tag = kv.Key;
                    }
                }
            }
        }

        void tssitem_Click(object sender, EventArgs e)
        {
            SeatFriend friend = tvLinkman.SelectedNode.Tag as SeatFriend;

            if (((ToolStripMenuItem)sender).Tag == null)
            {
                bool success = service.MoveFriendToGroup(loginSeat.SeatID, friend.SeatID, Guid.Empty);
                if (success) BindSeatFriend();
            }
            else
            {
                SeatFriendGroup group = ((ToolStripMenuItem)sender).Tag as SeatFriendGroup;
                if (friend.GroupID.HasValue)
                {
                    if (friend.GroupID.Value == group.GroupID)
                    {
                        return;
                    }
                }

                //ClientUtils.ShowMessage(friend.ShowName + "_" + group.GroupName);

                bool success = service.MoveFriendToGroup(loginSeat.SeatID, friend.SeatID, group.GroupID);
                if (success) BindSeatFriend();
            }
        }

        private void BindSeatFriend()
        {
            IList<SeatFriend> friends;
            IList<SeatFriend> friendlist = service.GetSeatFriends(loginSeat.SeatID, out friends);
            myfriends = friendlist;
            lvSearchName.Items.Clear();

            //Dictionary<string, string> dictCompany = new Dictionary<string, string>();
            //foreach (SeatFriend friend in friendlist)
            //{
            //    if (!dictCompany.ContainsKey(friend.CompanyID))
            //        dictCompany.Add(friend.CompanyID, friend.CompanyName);

            //    ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
            //    if (!string.IsNullOrEmpty(friend.MemoName))
            //        item = new ListViewItem(new string[] { friend.MemoName + string.Format("({0})", friend.SeatCode) });

            //    item.Tag = friend;
            //    lvSearchName.Items.Add(item);
            //}

            var grouplist = service.GetSeatFriendGroup(loginSeat.SeatID);
            SeatFriendGroup friendGroup = new SeatFriendGroup()
            {
                GroupID = Guid.Empty,
                GroupName = "我的好友"
            };
            grouplist.Insert(0, friendGroup);

            移动到ToolStripMenuItem.DropDownItems.Clear();
            Dictionary<Guid, SeatFriendGroup> dictGroup = new Dictionary<Guid, SeatFriendGroup>();
            foreach (var group in grouplist)
            {
                dictGroup.Add(group.GroupID, group);

                ToolStripMenuItem tssitem = new ToolStripMenuItem(group.GroupName);
                tssitem.Click += new EventHandler(tssitem_Click);
                tssitem.Tag = group;
                移动到ToolStripMenuItem.DropDownItems.Add(tssitem);
            }

            foreach (SeatFriend friend in friendlist)
            {
                ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
                if (!string.IsNullOrEmpty(friend.MemoName))
                    item = new ListViewItem(new string[] { friend.MemoName + string.Format("({0})", friend.SeatCode) });

                item.Tag = friend;
                lvSearchName.Items.Add(item);
            }

            tvLinkman.Nodes.Clear();
            foreach (KeyValuePair<Guid, SeatFriendGroup> kv in dictGroup)
            {
                var list = new List<SeatFriend>();
                if (kv.Key == Guid.Empty)
                    list = (friendlist as List<SeatFriend>).FindAll(p => p.GroupID == null);
                else
                    list = (friendlist as List<SeatFriend>).FindAll(p => p.GroupID != null && p.GroupID == kv.Key);

                TreeNode node = new TreeNode();
                node.SelectedImageIndex = 3;
                node.ImageIndex = 3;
                node.Tag = kv.Value;
                node.Text = string.Format("{0} [{1}/{2}]", kv.Value.GroupName,
                    list.FindAll(p => p.State != OnlineState.Offline).Count, list.Count);

                if (kv.Key != Guid.Empty)
                    node.ContextMenuStrip = contextMenuStrip7;

                var tempFriends = new List<SeatFriend>(list);
                tempFriends.Sort((p1, p2) => p1.State.CompareTo(p2.State));
                foreach (var friend in tempFriends)
                {
                    TreeNode tn = new TreeNode(string.Format("{0} {1}", friend.ShowName, friend.Sign));
                    if (!string.IsNullOrEmpty(friend.MemoName))
                        tn = new TreeNode(string.Format("{0} {1}", friend.MemoName, friend.Sign));

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
                    tn.ContextMenuStrip = contextMenuStrip2;
                    node.Nodes.Add(tn);
                }

                //展开我的好友
                if (kv.Key == Guid.Empty)
                {
                    node.Expand();
                }

                tvLinkman.Nodes.Add(node);
            }

            //默生人
            foreach (var friend in friends)
            {
                myfriends.Add(friend);
                ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
                if (!string.IsNullOrEmpty(friend.MemoName))
                    item = new ListViewItem(new string[] { friend.MemoName });

                item.Tag = friend;
                lvSearchName.Items.Add(item);
            }

            TreeNode tn1 = new TreeNode(string.Format("陌生人 [{1}/{0}]", friends.Count, (friends as List<SeatFriend>).FindAll(p => p.State != OnlineState.Offline).Count));
            tn1.SelectedImageIndex = 3;
            tn1.ImageIndex = 3;

            tvLinkman.Nodes.Add(tn1);
            {
                var tempFriends = new List<SeatFriend>(friends);
                tempFriends.Sort((p1, p2) => p1.State.CompareTo(p2.State));

                foreach (var friend in tempFriends)
                {
                    TreeNode tn = new TreeNode(string.Format("{0} {1}", friend.ShowName, friend.Sign));
                    if (!string.IsNullOrEmpty(friend.MemoName))
                        tn = new TreeNode(string.Format("{0} {1}", friend.MemoName, friend.Sign));

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

                    tn.ContextMenuStrip = contextMenuStrip3;
                    tn.Tag = friend;
                    tn1.Nodes.Add(tn);
                }
            }
        }

        //查找节点
        private TreeNode FindTreeNode<T>(TreeView treeView, T entity, string propertyName)
        {
            TreeNode findNode = null;
            foreach (TreeNode node in treeView.Nodes)
            {
                findNode = FindTreeNode<T>(node, entity, propertyName);
                if (findNode != null) break;
            }

            return findNode;
        }

        //查找节点
        private TreeNode FindTreeNode<T>(TreeNode parent, T entity, string propertyName)
        {
            foreach (TreeNode tn in parent.Nodes)
            {
                if (tn.Nodes.Count > 0)
                {
                    return FindTreeNode<T>(tn, entity, propertyName);
                }

                if (tn.Tag == null) continue;
                T target = (T)tn.Tag;
                object value1 = CoreUtils.GetPropertyValue(target, propertyName);
                object value2 = CoreUtils.GetPropertyValue(entity, propertyName);
                int ret = CoreUtils.Compare<object>(value1, value2);
                if (ret == 0) return tn;
            }

            return null;
        }

        private void frmNavigate_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppExit(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmMessage frm = new frmMessage(service, loginCompany, loginSeat);
                return frm;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmAddSeat frmadd = new frmAddSeat(service, loginCompany, loginSeat);
                return frmadd;
            });
        }

        private void 个人资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, loginCompany, loginSeat, loginSeat);
                frm.Callback += new CallbackEventHandler(frm_Callback);
                return frm;
            });
        }

        void frm_Callback(object obj)
        {
            if (obj == null) return;
            if (obj is Seat)
            {
                Seat seat = obj as Seat;
                lblUserName.Text = seat.ShowName;
                lblSign.Text = seat.Sign;
            }
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSetup setup = new frmSetup(service, areaList, loginCompany, loginSeat);
            setup.ShowDialog();
        }

        private void 版本信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.ImageAlign == ContentAlignment.MiddleLeft)
            {
                button3.ImageAlign = ContentAlignment.MiddleRight;
                Rectangle rect = new Rectangle(this.Left + this.Width, this.Top, 10000, this.Height);
                Singleton.Show(() =>
                {
                    frmMain main = new frmMain(service, loginCompany, loginSeat, clientID, currentFont, currentColor);
                    main.CloseCallback += new CallbackEventHandler(main_Callback);
                    return main;
                }, rect);

                isMainShow = true;
            }
            else
            {
                button3.ImageAlign = ContentAlignment.MiddleLeft;
                Singleton.Hide<frmMain>();

                isMainShow = false;
            }
        }

        void main_Callback(object obj)
        {
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            isMainShow = false;
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppExit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //notifyIcon1.Visible = false;
            this.Show();
            this.Activate();
            if (isMainShow) Singleton.Show<frmMain>();
            this.WindowState = FormWindowState.Normal;
            隐藏窗口ToolStripMenuItem.Visible = true;
            tsmiShowForm.Visible = false;
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPassword frm = new frmPassword(service, loginSeat.SeatID);
            frm.ShowDialog();
        }

        private void 留言ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmLeaveManager frm = new frmLeaveManager(service, loginCompany.CompanyID);
                return frm;
            });
        }

        private void AppExit(FormClosingEventArgs e)
        {
            if (isRelogin)
            {
                //先远程退出
                service.Logout(loginSeat.SeatID);
                return;
            }

            try
            {
                string prefix = string.Empty;
                if (seatInfo != null)
                {
                    List<P2SSession> list = new List<P2SSession>(seatInfo.SessionMessages.Keys);

                    //获取会话列表
                    int talkCount = list.FindAll(p => p.Seat != null).Count;
                    prefix = talkCount > 0 ? string.Format("当前有{0}个访客正在与您会话！\n", talkCount) : "";

                    int requestCount = list.FindAll(p => p.Seat == null).Count;
                    prefix += requestCount > 0 ? string.Format("当前有{0}个访客正在请求会话！\n", requestCount) : "";
                }

                if (MessageBox.Show(prefix + "确定退出面料QQ吗？", "退出系统", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    try
                    {
                        //先远程退出
                        service.Logout(loginSeat.SeatID);
                    }
                    catch { }

                    ClientUtils.ExitApplication();
                }
                else
                {
                    if (e != null) e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError("退出系统时出现错误：", ex);
            }
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        private void AppExit()
        {
            AppExit(null);
        }

        void chat_CallbackFontColor(Font font, Color color)
        {
            this.currentFont = font;
            this.currentColor = color;
        }

        #region Timer事件

        //Timer事件，DoWork

        #endregion

        private void tvLinkman_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null || e.Node.Tag is SeatFriendGroup)
            {
                foreach (TreeNode node in tvLinkman.Nodes)
                {
                    if (node != e.Node)
                        node.Collapse();
                }
                return;
            }

            SeatFriend toSeat = (SeatFriend)e.Node.Tag;
            if (toSeat.SeatID == loginSeat.SeatID)
            {
                ClientUtils.ShowMessage("不能自己与自己聊天！");
                return;
            }

            string key = string.Format("SeatChat_{0}_{1}", loginSeat.SeatID, toSeat.SeatID);
            SingletonMul.Show(key, () =>
            {
                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, toSeat, toSeat.MemoName, currentFont, currentColor);
                frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                return frmSeatChat;
            });
        }

        private void tvSeatGroup_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;

            SeatGroup group = ((SeatGroup)e.Node.Tag);

            string key = string.Format("GroupChat_{0}_{1}", loginSeat.SeatID, group.GroupID);
            SingletonMul.Show(key, () =>
            {
                frmGroupChat frmGroupChat = new frmGroupChat(service, loginCompany, loginSeat, group, currentFont, currentColor);
                frmGroupChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                frmGroupChat.Callback += new CallbackEventHandler(frmGroupChat_Callback);
                return frmGroupChat;
            });
        }

        void frmGroupChat_Callback(object obj)
        {
            BindSeatGroup();
        }

        private void tvSession_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;
            P2SSession session = ((P2SSession)e.Node.Tag);
            if (service.GetSession(session.SessionID) == null)
            {
                tvSession.Nodes.Remove(e.Node);
                return;
            }

            string key = string.Format("UserChat_{0}_{1}", loginSeat.SeatID, session.User.UserID);
            SingletonMul.Show(key, () =>
            {
                frmChat chat = new frmChat(service, session, loginCompany, loginSeat, currentFont, currentColor);
                chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                chat.Callback += new CallbackEventHandler(chat_Callback);
                return chat;
            });
        }

        void chat_Callback(object obj)
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

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void 留言信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmLeaveManager frm = new frmLeaveManager(service, loginCompany.CompanyID);
                return frm;
            });
        }

        private void 历史记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmMessage frm = new frmMessage(service, loginCompany, loginSeat);
                return frm;
            });
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            AppExit();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, loginCompany, loginSeat, loginSeat);
                frm.Callback += new CallbackEventHandler(frm_Callback);
                return frm;
            });
        }

        //恢复窗口
        private void tsmiShowForm_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            if (isMainShow) Singleton.Show<frmMain>();
            this.WindowState = FormWindowState.Normal;
            隐藏窗口ToolStripMenuItem.Visible = true;
            tsmiShowForm.Visible = false;
        }

        private void 隐藏窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            隐藏窗口ToolStripMenuItem.Visible = false;
            tsmiShowForm.Visible = true;
        }

        private void frmNavigate_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                隐藏窗口ToolStripMenuItem.Visible = false;
                tsmiShowForm.Visible = true;

                this.Hide();
                if (isMainShow) Singleton.Hide<frmMain>();
                //this.notifyIcon1.Visible = true;
            }

            if (isMainShow)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    Singleton.Hide<frmMain>();
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    Singleton.Show<frmMain>();
                }
            }
        }

        private void tvSeatGroup_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 4;
            e.Node.SelectedImageIndex = 4;
        }

        private void tvLinkman_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 4;
            e.Node.SelectedImageIndex = 4;
        }

        private void tvSession_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 4;
            e.Node.SelectedImageIndex = 4;
        }

        private void tvSession_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 3;
            e.Node.SelectedImageIndex = 3;
        }

        private void tvLinkman_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 3;
            e.Node.SelectedImageIndex = 3;
        }

        private void tvSeatGroup_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 3;
            e.Node.SelectedImageIndex = 3;
        }

        private void 更换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出当前用户更改另一用户登录吗？", "更换用户", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                isRelogin = true;
                urlTimer.Stop();
                urlTimer.Dispose();

                //先远程退出
                service.Logout(loginSeat.SeatID);

                //注销当前窗体
                CloseAllForm();

                //关闭主窗口
                Singleton.Close<frmMain>();
                Singleton.Close<frmNavigate>();

                //显示登录窗体
                Singleton.Show<frmLogin>(() =>
                {
                    frmLogin frm = new frmLogin(true);
                    return frm;
                });
            }
        }

        private void CloseAllForm()
        {
            for (int i = Application.OpenForms.Count - 1; i > 0; i--)
            {
                var frm = Application.OpenForms[i];
                frm.Close();
                frm.Dispose();
            }

            System.Threading.Thread.Sleep(100);
        }

        private void frmNavigate_LocationChanged(object sender, EventArgs e)
        {
            if (!isMainShow) return;

            frmMain main = Singleton.GetForm<frmMain>();
            if (main == null) return;
            main.Top = this.Top;

            if (this.Left > Screen.PrimaryScreen.Bounds.Width / 2)
                main.Left = this.Left - main.Width;
            else
                main.Left = this.Left + this.Width;
            main.Activate();
        }

        private void 在线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Online);
                lblUserName.Text = string.Format("{0}(在线)", loginSeat.SeatName);
            }
            catch { }
        }

        private void 离开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Leave);
                lblUserName.Text = string.Format("{0}(离开)", loginSeat.SeatName);
            }
            catch { }
        }

        private void 忙碌ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Busy);
                lblUserName.Text = string.Format("{0}(忙碌)", loginSeat.SeatName);
            }
            catch { }
        }

        private void tbSearchName_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchName.Text.Trim() == string.Empty)
            {
                lvSearchName.Visible = false;
            }
            else
            {
                lvSearchName.Visible = true;

                if (myfriends != null)
                {
                    var friends = (myfriends as List<SeatFriend>).FindAll(p =>
                        {
                            string value = tbSearchName.Text.Trim();
                            return p.ShowName.Contains(value) || (!string.IsNullOrEmpty(p.MemoName) && p.MemoName.Contains(value));
                        });

                    lvSearchName.Items.Clear();
                    foreach (SeatFriend friend in friends)
                    {
                        ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
                        if (!string.IsNullOrEmpty(friend.MemoName))
                            item = new ListViewItem(new string[] { friend.MemoName + string.Format("({0})", friend.SeatCode) });

                        item.Tag = friend;
                        lvSearchName.Items.Add(item);
                    }

                    if (lvSearchName.Items.Count > 0)
                    {
                        lvSearchName.Items[0].Selected = true;
                    }
                }
            }
        }

        private void 加为好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriendGroup)
            {
                ClientUtils.ShowMessage("请选中要操作的好友！");
                return;
            }

            Seat friend = (Seat)tvLinkman.SelectedNode.Tag;
            //发送请求加对方为好友
            Singleton.Show<frmAddSeatConfirm>(() =>
            {
                frmAddSeatConfirm frm = new frmAddSeatConfirm(service, loginCompany, loginSeat, friend);
                return frm;
            });
        }

        private void 消息记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvSession.SelectedNode == null || tvSession.SelectedNode.Tag == null)
            {
                ClientUtils.ShowMessage("请选中要操作的会话！");
                return;
            }

            P2SSession session = tvSession.SelectedNode.Tag as P2SSession;

            Singleton.Show(() =>
            {
                frmMessage frm = new frmMessage(service, loginCompany, loginSeat, session);
                return frm;
            });
        }

        private void 删除好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriendGroup)
            {
                ClientUtils.ShowMessage("请选中要操作的好友！");
                return;
            }

            Seat friend = (Seat)tvLinkman.SelectedNode.Tag;
            if (MessageBox.Show(string.Format("确定删除【{0}】吗？", friend.SeatName), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (service.DeleteSeatFriend(loginSeat.SeatID, friend.SeatID))
                {
                    BindSeatFriend();
                }
            }
        }

        private void lvSearchName_DoubleClick(object sender, EventArgs e)
        {
            if (lvSearchName.SelectedItems.Count == 0) return;
            lvSearchName.Visible = false;
            tvLinkman.Refresh();

            SeatFriend friend = (SeatFriend)lvSearchName.SelectedItems[0].Tag;
            if (friend.SeatID == loginSeat.SeatID)
            {
                ClientUtils.ShowMessage("不能自己与自己聊天！");
                return;
            }

            string key = string.Format("CHAT_{0}_{1}", loginSeat.SeatID, friend.SeatID);
            SingletonMul.Show(key, () =>
            {
                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, friend, friend.MemoName, currentFont, currentColor);
                frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                return frmSeatChat;
            });
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                tbSearchName.Focus();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tvSession.SelectedNode == null || tvSession.SelectedNode.Tag == null)
            {
                ClientUtils.ShowMessage("请选中要操作的会话！");
                return;
            }

            P2SSession session = tvSession.SelectedNode.Tag as P2SSession;
            if (MessageBox.Show(string.Format("确定结束与【{0}】的会话吗？", session.User.UserID), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    service.CloseSession(session.SessionID);
                    BindSession();
                }
                catch (Exception ex)
                {
                    ClientUtils.ShowError(ex);
                }
            }
        }

        private void StyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string style = item.ToString();
            if (Callback != null) Callback(style);
            //this.skinEngine1.SkinFile = CoreUtils.GetFullPath(string.Format("/skin/{0}.ssk", style));
        }

        private void toolStripMenuItem33_Click(object sender, EventArgs e)
        {
            if (tvSession.SelectedNode == null || tvSession.SelectedNode.Tag == null)
            {
                ClientUtils.ShowMessage("请选中要操作的会话！");
                return;
            }

            P2SSession session = tvSession.SelectedNode.Tag as P2SSession;

            if (MessageBox.Show(string.Format("确定接受与【{0}】的会话吗？", session.User.UserID), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    //接入会话
                    P2SSession p2session = service.AcceptSession(loginSeat.SeatID, ClientUtils.MaxAcceptCount, session.SessionID);
                    BindRequest();
                    BindSession();

                    //弹出窗口
                    SingletonMul.Show(p2session.SessionID, () =>
                    {
                        frmChat chat = new frmChat(service, p2session, loginCompany, loginSeat, currentFont, currentColor);
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
        }

        private void 在线升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("在线升级时需要退出当前应用程序，确定退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ClientUtils.ExitApplication();

                ProcessStartInfo process = new ProcessStartInfo(CoreUtils.GetFullPath("AutoUpdate.exe"));
                Process p = Process.Start(process);
            }
        }

        private void 查看资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriendGroup)
            {
                ClientUtils.ShowMessage("请选中要操作的好友！");
                return;
            }

            Seat friend = tvLinkman.SelectedNode.Tag as Seat;
            Singleton.Show<frmSeatInfo>(() =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, loginCompany, loginSeat, friend);
                return frm;
            });
        }

        private void 修改备注名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriendGroup)
            {
                ClientUtils.ShowMessage("请选中要操作的好友！");
                return;
            }

            SeatFriend friend = tvLinkman.SelectedNode.Tag as SeatFriend;

            //实现备注名称的修改
            Singleton.Show<frmSeatRename>(() =>
            {
                frmSeatRename frmRename = new frmSeatRename(service, friend);
                frmRename.Callback += new CallbackEventHandler(frmRename_Callback);
                return frmRename;
            });
        }

        void frmRename_Callback(object obj)
        {
            //刷新好友
            BindSeatFriend();
        }

        private void 查看个人资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, loginCompany, loginSeat, loginSeat);
                frm.Callback += new CallbackEventHandler(frm_Callback);
                return frm;
            });
        }

        private void 修改头像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmSeatFace frmFace = new frmSeatFace(service, loginCompany, loginSeat);
                frmFace.Callback += new CallbackEventHandler(frmFace_Callback);
                return frmFace;
            });
        }

        void frmFace_Callback(object obj)
        {
            if (obj != null)
            {
                Image img = obj as Image;
                pbSeatFace.Image = img;
            }
        }

        private void 新建群ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmFriendGroup frmFriendGroup = new frmFriendGroup(service, loginSeat, null);
                frmFriendGroup.Callback += new CallbackEventHandler(frmFriendGroup_Callback);
                return frmFriendGroup;
            });
        }

        private void 修改群名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriend)
            {
                ClientUtils.ShowMessage("请选中要操作的组！");
                return;
            }

            SeatFriendGroup group = tvLinkman.SelectedNode.Tag as SeatFriendGroup;
            Singleton.Show(() =>
            {
                frmFriendGroup frmFriendGroup = new frmFriendGroup(service, loginSeat, group);
                frmFriendGroup.Callback += new CallbackEventHandler(frmFriendGroup_Callback);
                return frmFriendGroup;
            });
        }

        void frmFriendGroup_Callback(object obj)
        {
            BindSeatFriend();
        }

        private void 删除群DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriend)
            {
                ClientUtils.ShowMessage("请选中要操作的组！");
                return;
            }

            SeatFriendGroup group = tvLinkman.SelectedNode.Tag as SeatFriendGroup;
            if (MessageBox.Show(string.Format("确定删除组【{0}】吗？", group.GroupName), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                bool success = service.DeleteFriendGroup(loginSeat.SeatID, group.GroupID);
                if (success) BindSeatFriend();
            }
        }

        private void 删除好友并从对方移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag is SeatFriendGroup)
            {
                ClientUtils.ShowMessage("请选中要操作的好友！");
                return;
            }

            Seat friend = (Seat)tvLinkman.SelectedNode.Tag;
            if (MessageBox.Show(string.Format("确定删除【{0}】并从对方移除自己吗？", friend.SeatName), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (service.DeleteSeatFriendAndRemoveOwner(loginSeat.SeatID, friend.SeatID))
                {
                    BindSeatFriend();
                }
            }
        }

        private void 迷你首页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show<frmMini>(() =>
            {
                urlTimer.Stop();
                string url = ConfigurationManager.AppSettings["DefaultPageUrl"];
                if (string.IsNullOrEmpty(url))
                {
                    url = "http://www.globalfabric.com/mini.asp";
                }
                string xy = ConfigurationManager.AppSettings["DefaultPageSize"];
                Size size = new Size(560, 580);
                if (!string.IsNullOrEmpty(xy))
                {
                    var arr = xy.Split('*');
                    size = new Size(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
                }
                frmMini frm = new frmMini(url);
                frm.MaximumSize = size;
                frm.MinimumSize = size;
                frm.Size = size;
                return frm;
            });
        }

        private void toolStripMenuItem39_Click(object sender, EventArgs e)
        {
            try
            {
                //绑定请求
                BindRequest();

                //绑定会话
                BindSession();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void toolStripMenuItem38_Click(object sender, EventArgs e)
        {
            try
            {
                //绑定客服组
                BindSeatGroup();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void 刷新RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //绑定好友
                BindSeatFriend();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void 查找添加群SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmAddGroup frmAddGroup = new frmAddGroup(service, loginSeat);
                frmAddGroup.Callback += new CallbackEventHandler(frmAddGroup_Callback);
                return frmAddGroup;
            });
        }

        void frmAddGroup_Callback(object obj)
        {
            BindSeatGroup();
        }

        private void toolStripMenuItem35_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmGroup frmGroup = new frmGroup(service, loginSeat, null);
                frmGroup.Callback += new CallbackEventHandler(frmGroup_Callback);
                return frmGroup;
            });
        }

        void frmGroup_Callback(object obj)
        {
            BindSeatGroup();
        }

        private void 修改群信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvSeatGroup.SelectedNode == null)
            {
                ClientUtils.ShowMessage("请选中要操作的群！");
                return;
            }

            SeatGroup group = (SeatGroup)tvSeatGroup.SelectedNode.Tag;
            string key = string.Format("SeatGroup_{0}", group.GroupID);
            SingletonMul.Show(key, () =>
            {
                frmGroup frmGroup = new frmGroup(service, loginSeat, group);
                frmGroup.Callback += new CallbackEventHandler(frmGroup_Callback);
                return frmGroup;
            });
        }

        private void 修改备注名称MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvSeatGroup.SelectedNode == null)
            {
                ClientUtils.ShowMessage("请选中要操作的群！");
                return;
            }

            SeatGroup group = (SeatGroup)tvSeatGroup.SelectedNode.Tag;
            Singleton.Show(() =>
            {
                frmGroupRename frmGroupRename = new frmGroupRename(service, loginSeat, group);
                frmGroupRename.Callback += new CallbackEventHandler(frmGroupRename_Callback);
                return frmGroupRename;
            });
        }

        void frmGroupRename_Callback(object obj)
        {
            BindSeatGroup();
        }

        private void 退出群QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvSeatGroup.SelectedNode == null)
            {
                ClientUtils.ShowMessage("请选中要操作的群！");
                return;
            }

            SeatGroup group = (SeatGroup)tvSeatGroup.SelectedNode.Tag;
            //退出该群
            if (MessageBox.Show("确定退出群【" + (group.MemoName ?? group.GroupName) + "】吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            service.ExitGroup(loginSeat.SeatID, group.GroupID);
            BindSeatGroup();
        }

        private void 解散群JToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvSeatGroup.SelectedNode == null)
            {
                ClientUtils.ShowMessage("请选中要操作的群！");
                return;
            }

            SeatGroup group = (SeatGroup)tvSeatGroup.SelectedNode.Tag;
            //退出该群
            if (MessageBox.Show("确定解散群【" + (group.MemoName ?? group.GroupName) + "】吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            service.DismissGroup(loginSeat.SeatID, group.GroupID);
            BindSeatGroup();
        }
    }
}
