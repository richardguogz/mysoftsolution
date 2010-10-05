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

namespace LiveChat.Client
{
    public partial class frmNavigate : Form
    {
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        public event CallbackEventHandler Callback;

        #region private member

        private const int maxAcceptCount = 20;

        private bool isClose = false;           //是否关闭窗体标志
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
        private bool isMainShow = true;
        private IList<SeatFriend> myfriends;

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

                Singleton.Show(() =>
                {
                    int left = this.Left + this.Width;
                    if (this.Left > Screen.PrimaryScreen.Bounds.Width / 2)
                        left = this.Left - 800;

                    Rectangle rect = new Rectangle(left, this.Top, 10000, this.Height);
                    frmMain main = new frmMain(service, loginCompany, loginSeat, clientID, SystemFonts.DefaultFont, SystemColors.WindowText, rect);
                    main.CallbackClose += new CallbackEventHandler(main_Callback);
                    main.CallbackSession += new CallbackEventHandler(main_CallbackSession);
                    return main;
                });
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
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
            this.button3.ImageAlign = ContentAlignment.MiddleRight;

            int width = (tabControl1.Width - 5) / 3;
            tabControl1.ItemSize = new Size(width, 24);

            ImageList imgList2 = new ImageList();
            imgList2.ImageSize = new Size(1, 24);       //分别是宽和高
            lvSearchName.SmallImageList = imgList2;     //这里设置listView的SmallImageList 

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

                //刷新用户请求
                RefreshFriendRequest();

                //显示消息提示
                ShowMessage();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);

                //throw ex;

                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            refreshtime.Start();
        }

        private void ShowMessage()
        {
            IList<TipInfo> tiplist = new List<TipInfo>();

            //会话消息
            foreach (KeyValuePair<P2SSession, MessageInfo> kv in seatInfo.SessionMessages)
            {
                if (kv.Key.Seat == null) continue;

                if (kv.Value.Count > 0)
                {
                    string msgText = string.Format("访客【{0}】给您发送了【{1}】条新消息！", kv.Key.User.UserName, kv.Value.Count);

                    StringBuilder sbMsg = new StringBuilder(msgText);
                    sbMsg.AppendLine();
                    sbMsg.AppendLine();

                    sbMsg.AppendFormat("[{0}]" + kv.Value.Message.Content, kv.Value.Message.SendTime.ToLongTimeString());
                    sbMsg.AppendLine();

                    TipInfo tip = new TipInfo() { Title = "您有新的消息", Message = sbMsg.ToString() };
                    tip.Key = string.Format("UserMessage_{0}_{1}", loginSeat.SeatID, kv.Key.User.UserID);
                    tip.Target = kv.Key;

                    tiplist.Add(tip);
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
                    tip.Key = string.Format("SeatMessage_{0}_{1}", loginSeat.SeatID, kv.Key.SeatID);
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
                    tip.Key = string.Format("GroupMessage_{0}_{1}", loginSeat.SeatID, kv.Key.GroupID);
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
                            SingletonMul.Show(tip.Key.Replace("Message", "Chat"), () =>
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
                            SingletonMul.Show(tip.Key.Replace("Message", "Chat"), () =>
                            {
                                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, toSeat, toSeat.MemoName, currentFont, currentColor);
                                frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                                return frmSeatChat;
                            });
                        }
                        else if (tip.Target is SeatGroup)
                        {
                            SeatGroup group = tip.Target as SeatGroup;
                            SingletonMul.Show(tip.Key.Replace("Message", "Chat"), () =>
                            {
                                frmGroupChat frmGroupChat = new frmGroupChat(service, loginCompany, loginSeat, group, currentFont, currentColor);
                                frmGroupChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                                return frmGroupChat;
                            });
                        }
                    }, p2 =>
                    {
                        if (tip.Target is P2SSession)
                        {
                            P2SSession session = tip.Target as P2SSession;
                            service.GetP2SMessages(session.SessionID);
                        }
                        else if (tip.Target is Seat)
                        {
                            Seat toSeat = tip.Target as Seat;
                            service.GetS2SHistoryMessages(loginSeat.SeatID, toSeat.SeatID);
                        }
                        else if (tip.Target is Group)
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
            SingletonMul.Show<frmPopup>(tip.Key, () =>
            {
                frmPopup frm = new frmPopup(tip);
                frm.CallbackView += viewHandler;
                frm.CallbackCancel += cancelHandler;
                return frm;
            });
        }

        private void RefreshFriendRequest()
        {
            if (seatInfo == null) return;

            foreach (var request in seatInfo.RequestMessages)
            {
                string key = string.Format("Request_{0}", request.Key.SeatID);
                SingletonMul.Show<frmConfirmFriend>(key, () =>
                {
                    frmConfirmFriend frmFriend = new frmConfirmFriend(service, loginCompany, request.Key, request.Value);
                    frmFriend.Callback += new CallbackEventHandler(frmFriend_Callback);
                    return frmFriend;
                });
            }
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
            var list = service.GetRequestSessions(loginSeat.SeatID, SortType.Vip, 1, 100).DataSource;
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
                        tn.Text = string.Format("{0}({1})", kv.Key.GroupName, kv.Value.Count);
                    else
                        tn.Text = string.Format("{0}", kv.Key.GroupName);

                    tn.Tag = kv.Key;
                }
            }
        }

        private void BindSeatGroup()
        {
            IList<SeatGroup> list = service.GetJoinGroups(loginSeat.SeatID);
            TreeNode node = tvSeatGroup.Nodes[0];
            node.Nodes.Clear();
            node.Text = string.Format("客服群({0})", list.Count);
            foreach (var group in list)
            {
                TreeNode tn = new TreeNode(group.GroupName);
                tn.SelectedImageIndex = 2;
                tn.ImageIndex = 2;
                tn.Tag = group;
                node.Nodes.Add(tn);
            }
            node.Expand();
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

        private void BindSeatFriend()
        {
            IList<SeatFriend> friends;
            IList<SeatFriend> friendlist = service.GetSeatFriends(loginSeat.SeatID, out friends);
            myfriends = friendlist;
            lvSearchName.Items.Clear();

            Dictionary<string, string> dictCompany = new Dictionary<string, string>();
            foreach (SeatFriend friend in friendlist)
            {
                if (!dictCompany.ContainsKey(friend.CompanyID))
                    dictCompany.Add(friend.CompanyID, friend.CompanyName);

                ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
                if (!string.IsNullOrEmpty(friend.MemoName))
                    item = new ListViewItem(new string[] { friend.MemoName + string.Format("({0})", friend.SeatCode) });

                item.Tag = friend;
                lvSearchName.Items.Add(item);
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

                //展开自己所在的公司
                if (kv.Key == loginCompany.CompanyID)
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

            TreeNode tn1 = new TreeNode(string.Format("陌生人({0})", friends.Count));
            tn1.SelectedImageIndex = 3;
            tn1.ImageIndex = 3;

            tvLinkman.Nodes.Add(tn1);
            foreach (var friend in friends)
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
            if (!isClose)
            {
                e.Cancel = true;
                this.Hide();

                if (isMainShow) Singleton.Hide<frmMain>();
                //this.notifyIcon1.Visible = true;

                隐藏窗口ToolStripMenuItem.Visible = false;
                tsmiShowForm.Visible = true;
            }
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
                    main.CallbackClose += new CallbackEventHandler(main_Callback);
                    main.CallbackSession += new CallbackEventHandler(main_CallbackSession);
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

        void main_CallbackSession(object obj)
        {
            if (obj != null)
            {
                if (seatInfo != null)
                {
                    //判断当前会话中是否存在
                    IList<P2CSession> list = obj as List<P2CSession>;

                }
            }

            RefreshRequest();
            RefreshSession();
        }

        void main_Callback(object obj)
        {
            button3.ImageAlign = ContentAlignment.MiddleLeft;
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

        /// <summary>
        /// 应用程序退出
        /// </summary>
        private void AppExit()
        {
            try
            {
                if (ClientUtils.SoftwareOtherLogin)
                {
                    isClose = true;
                    this.Close();
                    ClientUtils.ExitApplication();

                    return;
                }

                //获取会话列表
                int talkCount = service.GetSeat(loginSeat.SeatID).UserSessionCount;
                string prefix = talkCount > 0 ? string.Format("当前有{0}个访客正在与您会话！\n", talkCount) : "";
                int requestCount = service.GetRequestSessions(loginSeat.SeatID, SortType.None, 1, 10).RowCount;
                prefix += requestCount > 0 ? string.Format("当前有{0}个访客正在请求会话！\n", requestCount) : "";

                if (MessageBox.Show(prefix + "确定退出客服系统吗？", "退出系统", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {
                    try
                    {
                        //先远程退出
                        service.Logout(loginSeat.SeatID);
                    }
                    catch { }

                    isClose = true;
                    this.Close();
                    ClientUtils.ExitApplication();
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError("退出系统时出现错误：", ex);
            }
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
            if (e.Node.Tag == null)
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
                return frmGroupChat;
            });
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

        private void 刷新RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //绑定请求
                BindRequest();

                //绑定会话
                BindSession();

                //绑定客服组
                BindSeatGroup();

                //绑定我的好友
                BindSeatFriend();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
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
            this.Close();
        }

        private void frmNavigate_SizeChanged(object sender, EventArgs e)
        {
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
            if (MessageBox.Show("确定退出当前系统更改另一用户登录吗？", "重新登录", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //先远程退出
                service.Logout(loginSeat.SeatID);

                //注销当前窗体
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

        private void frmNavigate_LocationChanged(object sender, EventArgs e)
        {
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
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag == null)
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
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag == null)
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

        private void 查看资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag == null)
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
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag == null)
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
    }
}
