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
        private SeatConfig config;
        private Guid clientID;
        private IList<Area> areaList;
        private Timer refreshtime;
        private SeatInfo seatInfo;
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
                    Rectangle rect = new Rectangle(this.Left + this.Width, this.Top, 10000, this.Height);
                    frmMain main = new frmMain(service, loginCompany, loginSeat, clientID, SystemFonts.DefaultFont, SystemColors.WindowText, rect);
                    main.CallbackClose += new CallbackEventHandler(main_Callback);
                    main.CallbackSession += new CallbackEventHandler(main_CallbackSession);
                    main.CallbackShowTip += new ShowTipEventHandler(main_CallbackShowTip);
                    return main;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //设置提示信息
        void main_CallbackShowTip(string title, string text, ToolTipIcon icon, EventHandler handler)
        {
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipIcon = icon;
            notifyIcon1.BalloonTipClicked += handler;
            notifyIcon1.ShowBalloonTip(1000);
        }

        private void InitSystemInfo()
        {
            this.Left = point.X;
            this.Top = point.Y;
            this.button3.ImageAlign = ContentAlignment.MiddleRight;

            int width = (tabControl1.Width - 5) / 3;
            tabControl1.ItemSize = new Size(width, 24);

            ImageList imgList2 = new ImageList();
            imgList2.ImageSize = new Size(1, 24);//分别是宽和高
            lvSearchName.SmallImageList = imgList2;   //这里设置listView的SmallImageList 

            config = service.GetSeatConfig(loginSeat.SeatID);
            lblUserName.Text = string.Format("{0}(在线)", config.SeatName);
            tsUserName.Text = string.Empty;
            lblSign.Text = config.Sign;
            toolTip1.SetToolTip(lblSign, config.Sign);
            tsmiExit.Text = string.Format("退出系统【{0}】", loginSeat.SeatCode);

            tvSession.HideSelection = false;
            tvLinkman.HideSelection = false;
            tvSeatGroup.HideSelection = false;

            //获取地区信息
            areaList = service.GetAreas();

            refreshtime = new Timer();
            refreshtime.Interval = 10000;
            refreshtime.Tick += new EventHandler(refreshtime_Tick);
            refreshtime.Start();
        }

        void refreshtime_Tick(object sender, EventArgs e)
        {
            refreshtime.Stop();

            try
            {
                //处理定时刷新
                seatInfo = service.GetSeatInfo(loginSeat.SeatID);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //throw ex;

                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            refreshtime.Start();
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
            seatInfo = service.GetSeatInfo(loginSeat.SeatID);
        }

        private void RefreshRequest()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.SessionMessages)
            {
                if (kv.Key.Seat != null) return;

                TreeNode tn = FindTreeNode<P2SSession>(tvSession.Nodes[0], kv.Key, "SessionID");
                if (tn != null)
                {
                    if (kv.Value > 0)
                        tn.Text = string.Format("{0}({1})", kv.Key.User.UserID, kv.Value);
                    else
                        tn.Text = string.Format("{0}", kv.Key.User.UserID);

                    tn.Tag = kv.Key;
                }
                else
                {
                    tn = new TreeNode(string.Format("{0}({1})", kv.Key.User.UserID, kv.Value));
                    tn.SelectedImageIndex = 5;
                    tn.ImageIndex = 5;
                    tn.Tag = kv.Key;
                    tvSession.Nodes[0].Nodes.Add(tn);
                }
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
                TreeNode tn = new TreeNode(string.Format("{0}({1})", info.User.UserID, info.NoReadMessageCount));
                tn.Text = tn.Text.Replace("(0)", "");
                tn.SelectedImageIndex = 5;
                tn.ImageIndex = 5;
                tn.Tag = info;
                tn.ContextMenuStrip = contextMenuStrip5;
                node.Nodes.Add(tn);
            }
            node.Expand();
        }

        private void RefreshSession()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.SessionMessages)
            {
                if (kv.Key.Seat == null) return;

                TreeNode tn = FindTreeNode<P2SSession>(tvSession.Nodes[1], kv.Key, "SessionID");
                if (tn != null)
                {
                    if (kv.Value > 0)
                        tn.Text = string.Format("{0}({1})", kv.Key.User.UserID, kv.Value);
                    else
                        tn.Text = string.Format("{0}", kv.Key.User.UserID);

                    tn.Tag = kv.Key;
                }
                else
                {
                    tn = new TreeNode(string.Format("{0}({1})", kv.Key.User.UserID, kv.Value));
                    tn.SelectedImageIndex = 5;
                    tn.ImageIndex = 5;
                    tn.Tag = kv.Key;
                    tvSession.Nodes[1].Nodes.Add(tn);
                }
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
                TreeNode tn = new TreeNode(string.Format("{0}({1})", info.User.UserID, info.NoReadMessageCount));
                tn.Text = tn.Text.Replace("(0)", "");
                tn.SelectedImageIndex = 5;
                tn.ImageIndex = 5;
                tn.Tag = info;
                tn.ContextMenuStrip = contextMenuStrip4;
                node.Nodes.Add(tn);
            }
            node.Expand();
        }

        private void RefreshSeatGroup()
        {
            if (seatInfo == null) return;

            foreach (var kv in seatInfo.GroupMessages)
            {
                TreeNode tn = FindTreeNode<Group>(tvSeatGroup, kv.Key, "GroupID");
                if (tn != null)
                {
                    if (kv.Value > 0)
                        tn.Text = string.Format("{0}({1})", kv.Key.GroupName, kv.Value);
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
                TreeNode tn = FindTreeNode<Seat>(tvLinkman, kv.Key, "SeatID");
                if (tn != null)
                {
                    if (kv.Value > 0)
                        tn.Text = string.Format("{0}({1})", kv.Key.ShowName, kv.Value);
                    else
                        tn.Text = string.Format("{0}", kv.Key.ShowName);

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
                    TreeNode tn = new TreeNode(string.Format("{0}", friend.ShowName));
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
                item.Tag = friend;
                lvSearchName.Items.Add(item);
            }

            TreeNode tn1 = new TreeNode(string.Format("陌生人({0})", friends.Count));
            tn1.SelectedImageIndex = 3;
            tn1.ImageIndex = 3;

            tvLinkman.Nodes.Add(tn1);
            foreach (var friend in friends)
            {
                TreeNode tn = new TreeNode(string.Format("{0}", friend.ShowName));
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
                SeatConfig seat = service.GetSeatConfig(loginSeat.SeatID);
                frmSeatInfo frm = new frmSeatInfo(service, seat, true);
                frm.Callback += new CallbackEventHandler(frm_Callback);
                return frm;
            });
        }

        void frm_Callback(object obj)
        {
            if (obj == null) return;
            if (obj is SeatConfig)
            {
                SeatConfig seat = obj as SeatConfig;
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
                    main.CallbackShowTip += new ShowTipEventHandler(main_CallbackShowTip);
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
                    Application.Exit();
                    Environment.Exit(0);
                    this.Close();

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
                    Application.Exit();
                    Environment.Exit(0);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("退出系统时出现错误：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string seatID = ((SeatFriend)e.Node.Tag).SeatID;
            if (seatID == loginSeat.SeatID)
            {
                MessageBox.Show("不能自己与自己聊天！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string key = string.Format("CHAT_{0}_{1}", loginSeat.SeatID, seatID);

            SingletonMul.Show(key, () =>
            {
                Seat toSeat = service.GetSeat(seatID);
                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, toSeat, currentFont, currentColor);
                frmSeatChat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                return frmSeatChat;
            });
        }

        private void tvSeatGroup_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;

            SeatGroup group = ((SeatGroup)e.Node.Tag);
            string key = string.Format("CHAT_{0}_{1}", loginSeat.SeatID, group.GroupID);

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

            SingletonMul.Show(session.SessionID, () =>
            {
                frmChat chat = new frmChat(service, session, loginCompany, loginSeat, currentFont, currentColor);
                chat.CallbackFontColor += new CallbackFontColorEventHandler(chat_CallbackFontColor);
                chat.Callback += new CallbackEventHandler(chat_Callback);
                return chat;
            });
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
                SeatConfig seat = service.GetSeatConfig(loginSeat.SeatID);
                frmSeatInfo frm = new frmSeatInfo(service, seat, true);
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
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            main.Left = this.Left + this.Width;
            main.Activate();
        }

        private void 在线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Online);
                lblUserName.Text = string.Format("{0}(在线)", config.SeatName);
            }
            catch { }
        }

        private void 离开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Leave);
                lblUserName.Text = string.Format("{0}(离开)", config.SeatName);
            }
            catch { }
        }

        private void 忙碌ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                service.ChangeSeatState(loginSeat.SeatID, OnlineState.Busy);
                lblUserName.Text = string.Format("{0}(忙碌)", config.SeatName);
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
                            return p.ShowName.Contains(value);
                        });

                    lvSearchName.Items.Clear();
                    foreach (SeatFriend friend in friends)
                    {
                        ListViewItem item = new ListViewItem(new string[] { friend.ShowName });
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
                MessageBox.Show("请选中要操作的好友！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SeatFriend friend = tvLinkman.SelectedNode.Tag as SeatFriend;

            if (MessageBox.Show(string.Format("确定添加【{0}】为好友吗？", friend.SeatName), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (service.AddSeatFriend(loginSeat.SeatID, friend.SeatID))
                {
                    BindSeatFriend();
                }
            }
        }

        private void 删除好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvLinkman.SelectedNode == null || tvLinkman.SelectedNode.Tag == null)
            {
                MessageBox.Show("请选中要操作的好友！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SeatFriend friend = tvLinkman.SelectedNode.Tag as SeatFriend;
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

            string seatID = ((SeatFriend)lvSearchName.SelectedItems[0].Tag).SeatID;
            if (seatID == loginSeat.SeatID)
            {
                MessageBox.Show("不能自己与自己聊天！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string key = string.Format("CHAT_{0}_{1}", loginSeat.SeatID, seatID);

            SingletonMul.Show(key, () =>
            {
                Seat toSeat = service.GetSeat(seatID);
                frmSeatChat frmSeatChat = new frmSeatChat(service, loginCompany, loginSeat, toSeat, currentFont, currentColor);
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
                MessageBox.Show("请选中要操作的会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("请选中要操作的会话！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
