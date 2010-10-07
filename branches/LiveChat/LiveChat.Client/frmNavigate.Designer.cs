namespace LiveChat.Client
{
    partial class frmNavigate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (refreshtime != null)
            {
                refreshtime.Stop();
                refreshtime = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNavigate));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("会话请求", 3, 3);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("客服会话", 3, 3);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("公司客服", 3, 3);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("我的好友", 3, 3);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("客服群", 3, 3);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSign = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsUserName = new System.Windows.Forms.ToolStripDropDownButton();
            this.在线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.忙碌ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbSeatFace = new System.Windows.Forms.PictureBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.个人资料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改密码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.皮肤切换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.历史记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.留言ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.在线升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.版本信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.更换用户ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvSession = new LiveChat.Client.DoubleBufferTreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbSearchName = new System.Windows.Forms.TextBox();
            this.lvSearchName = new LiveChat.Client.DoubleBufferListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.tvLinkman = new LiveChat.Client.DoubleBufferTreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tvSeatGroup = new LiveChat.Client.DoubleBufferTreeView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmSysTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.隐藏窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.历史记录ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.留言信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.系统设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.关于我们ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.更换用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除好友ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改备注名称ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看资料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.查看资料ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.消息记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.OFFICE2007ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mP10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pAGEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wINXPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rOYALEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripMenuItem();
            this.消息记录ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem34 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip6 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看个人资料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改头像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSeatFace)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.cmSysTray.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            this.contextMenuStrip5.SuspendLayout();
            this.contextMenuStrip6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSign);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.pbSeatFace);
            this.panel1.Controls.Add(this.lblUserName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 78);
            this.panel1.TabIndex = 0;
            // 
            // lblSign
            // 
            this.lblSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSign.Location = new System.Drawing.Point(80, 37);
            this.lblSign.Name = "lblSign";
            this.lblSign.Size = new System.Drawing.Size(131, 32);
            this.lblSign.TabIndex = 2;
            this.lblSign.Text = "label1";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(183, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(5, 36);
            this.label2.TabIndex = 6;
            this.label2.Text = "     ";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Image = global::LiveChat.Client.Properties.Resources.visitor;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(188, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 27);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(158, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 6);
            this.label1.TabIndex = 5;
            this.label1.Text = "     ";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUserName});
            this.toolStrip1.Location = new System.Drawing.Point(167, 10);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(19, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsUserName
            // 
            this.tsUserName.AutoSize = false;
            this.tsUserName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tsUserName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsUserName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.在线ToolStripMenuItem,
            this.离开ToolStripMenuItem,
            this.忙碌ToolStripMenuItem});
            this.tsUserName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUserName.Margin = new System.Windows.Forms.Padding(0);
            this.tsUserName.Name = "tsUserName";
            this.tsUserName.Size = new System.Drawing.Size(16, 25);
            // 
            // 在线ToolStripMenuItem
            // 
            this.在线ToolStripMenuItem.Name = "在线ToolStripMenuItem";
            this.在线ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.在线ToolStripMenuItem.Text = "在线";
            this.在线ToolStripMenuItem.Click += new System.EventHandler(this.在线ToolStripMenuItem_Click);
            // 
            // 离开ToolStripMenuItem
            // 
            this.离开ToolStripMenuItem.Name = "离开ToolStripMenuItem";
            this.离开ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.离开ToolStripMenuItem.Text = "离开";
            this.离开ToolStripMenuItem.Click += new System.EventHandler(this.离开ToolStripMenuItem_Click);
            // 
            // 忙碌ToolStripMenuItem
            // 
            this.忙碌ToolStripMenuItem.Name = "忙碌ToolStripMenuItem";
            this.忙碌ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.忙碌ToolStripMenuItem.Text = "忙碌";
            this.忙碌ToolStripMenuItem.Click += new System.EventHandler(this.忙碌ToolStripMenuItem_Click);
            // 
            // pbSeatFace
            // 
            this.pbSeatFace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbSeatFace.ContextMenuStrip = this.contextMenuStrip6;
            this.pbSeatFace.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSeatFace.Image = ((System.Drawing.Image)(resources.GetObject("pbSeatFace.Image")));
            this.pbSeatFace.Location = new System.Drawing.Point(11, 10);
            this.pbSeatFace.Name = "pbSeatFace";
            this.pbSeatFace.Size = new System.Drawing.Size(60, 60);
            this.pbSeatFace.TabIndex = 0;
            this.pbSeatFace.TabStop = false;
            this.toolTip1.SetToolTip(this.pbSeatFace, "双击头像修改个人资料");
            this.pbSeatFace.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(79, 15);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(53, 12);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "客服名称";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 498);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(224, 64);
            this.panel2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Image = global::LiveChat.Client.Properties.Resources.add_f;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(100, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "添加好友";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = global::LiveChat.Client.Properties.Resources.history;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(11, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "消息记录";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统菜单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 39);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(224, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统菜单ToolStripMenuItem
            // 
            this.系统菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.个人资料ToolStripMenuItem,
            this.修改密码ToolStripMenuItem,
            this.皮肤切换ToolStripMenuItem,
            this.toolStripSeparator1,
            this.历史记录ToolStripMenuItem,
            this.留言ToolStripMenuItem,
            this.toolStripSeparator5,
            this.系统设置ToolStripMenuItem,
            this.在线升级ToolStripMenuItem,
            this.版本信息ToolStripMenuItem,
            this.toolStripSeparator2,
            this.更换用户ToolStripMenuItem1,
            this.退出系统ToolStripMenuItem});
            this.系统菜单ToolStripMenuItem.Name = "系统菜单ToolStripMenuItem";
            this.系统菜单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.系统菜单ToolStripMenuItem.Text = "系统菜单";
            // 
            // 个人资料ToolStripMenuItem
            // 
            this.个人资料ToolStripMenuItem.Name = "个人资料ToolStripMenuItem";
            this.个人资料ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.个人资料ToolStripMenuItem.Text = "个人资料";
            this.个人资料ToolStripMenuItem.Click += new System.EventHandler(this.个人资料ToolStripMenuItem_Click);
            // 
            // 修改密码ToolStripMenuItem
            // 
            this.修改密码ToolStripMenuItem.Name = "修改密码ToolStripMenuItem";
            this.修改密码ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.修改密码ToolStripMenuItem.Text = "修改密码";
            this.修改密码ToolStripMenuItem.Click += new System.EventHandler(this.修改密码ToolStripMenuItem_Click);
            // 
            // 皮肤切换ToolStripMenuItem
            // 
            this.皮肤切换ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem26,
            this.toolStripMenuItem27,
            this.toolStripMenuItem28,
            this.toolStripMenuItem29,
            this.toolStripMenuItem30,
            this.toolStripMenuItem31,
            this.toolStripMenuItem32});
            this.皮肤切换ToolStripMenuItem.Name = "皮肤切换ToolStripMenuItem";
            this.皮肤切换ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.皮肤切换ToolStripMenuItem.Text = "皮肤切换";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem26.Tag = "office2007";
            this.toolStripMenuItem26.Text = "OFFICE2007";
            this.toolStripMenuItem26.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem27.Tag = "mp10";
            this.toolStripMenuItem27.Text = "MP10";
            this.toolStripMenuItem27.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem28.Tag = "page";
            this.toolStripMenuItem28.Text = "PAGE";
            this.toolStripMenuItem28.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem29.Tag = "msn";
            this.toolStripMenuItem29.Text = "MSN";
            this.toolStripMenuItem29.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem30
            // 
            this.toolStripMenuItem30.Name = "toolStripMenuItem30";
            this.toolStripMenuItem30.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem30.Tag = "winxp";
            this.toolStripMenuItem30.Text = "WINXP";
            this.toolStripMenuItem30.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem31.Tag = "royale";
            this.toolStripMenuItem31.Text = "ROYALE";
            this.toolStripMenuItem31.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem32.Tag = "deep";
            this.toolStripMenuItem32.Text = "DEEP";
            this.toolStripMenuItem32.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // 历史记录ToolStripMenuItem
            // 
            this.历史记录ToolStripMenuItem.Name = "历史记录ToolStripMenuItem";
            this.历史记录ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.历史记录ToolStripMenuItem.Text = "消息记录";
            this.历史记录ToolStripMenuItem.Click += new System.EventHandler(this.历史记录ToolStripMenuItem_Click);
            // 
            // 留言ToolStripMenuItem
            // 
            this.留言ToolStripMenuItem.Name = "留言ToolStripMenuItem";
            this.留言ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.留言ToolStripMenuItem.Text = "留言管理";
            this.留言ToolStripMenuItem.Click += new System.EventHandler(this.留言ToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(121, 6);
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            this.系统设置ToolStripMenuItem.Click += new System.EventHandler(this.系统设置ToolStripMenuItem_Click);
            // 
            // 在线升级ToolStripMenuItem
            // 
            this.在线升级ToolStripMenuItem.Name = "在线升级ToolStripMenuItem";
            this.在线升级ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.在线升级ToolStripMenuItem.Text = "在线升级";
            this.在线升级ToolStripMenuItem.Click += new System.EventHandler(this.在线升级ToolStripMenuItem_Click);
            // 
            // 版本信息ToolStripMenuItem
            // 
            this.版本信息ToolStripMenuItem.Name = "版本信息ToolStripMenuItem";
            this.版本信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.版本信息ToolStripMenuItem.Text = "关于我们";
            this.版本信息ToolStripMenuItem.Click += new System.EventHandler(this.版本信息ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // 更换用户ToolStripMenuItem1
            // 
            this.更换用户ToolStripMenuItem1.Name = "更换用户ToolStripMenuItem1";
            this.更换用户ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.更换用户ToolStripMenuItem1.Text = "更换用户";
            this.更换用户ToolStripMenuItem1.Click += new System.EventHandler(this.更换用户ToolStripMenuItem_Click);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出系统ToolStripMenuItem.Text = "退出系统";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 78);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(224, 420);
            this.panel3.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(60, 24);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(224, 420);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvSession);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(216, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "访客";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvSession
            // 
            this.tvSession.ContextMenuStrip = this.contextMenuStrip1;
            this.tvSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSession.HideSelection = false;
            this.tvSession.ImageIndex = 0;
            this.tvSession.ImageList = this.imgList;
            this.tvSession.ItemHeight = 24;
            this.tvSession.Location = new System.Drawing.Point(0, 0);
            this.tvSession.Name = "tvSession";
            treeNode1.ImageIndex = 3;
            treeNode1.Name = "节点0";
            treeNode1.SelectedImageIndex = 3;
            treeNode1.Text = "会话请求";
            treeNode2.ImageIndex = 3;
            treeNode2.Name = "节点1";
            treeNode2.SelectedImageIndex = 3;
            treeNode2.Text = "客服会话";
            this.tvSession.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.tvSession.SelectedImageIndex = 0;
            this.tvSession.ShowLines = false;
            this.tvSession.ShowRootLines = false;
            this.tvSession.Size = new System.Drawing.Size(216, 388);
            this.tvSession.TabIndex = 0;
            this.tvSession.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSession_NodeMouseDoubleClick);
            this.tvSession.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvSession_AfterCollapse);
            this.tvSession.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvSession_AfterExpand);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新RToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 26);
            // 
            // 刷新RToolStripMenuItem
            // 
            this.刷新RToolStripMenuItem.Name = "刷新RToolStripMenuItem";
            this.刷新RToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.刷新RToolStripMenuItem.Text = "刷新(&R)";
            this.刷新RToolStripMenuItem.Click += new System.EventHandler(this.刷新RToolStripMenuItem_Click);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "人.png");
            this.imgList.Images.SetKeyName(1, "单人灰不在线.png");
            this.imgList.Images.SetKeyName(2, "群.png");
            this.imgList.Images.SetKeyName(3, "树第一级.png");
            this.imgList.Images.SetKeyName(4, "树打开.bmp");
            this.imgList.Images.SetKeyName(5, "人.png");
            this.imgList.Images.SetKeyName(6, "人.ico");
            this.imgList.Images.SetKeyName(7, "离开.ico");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(216, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "联系人";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbSearchName);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvSearchName);
            this.splitContainer1.Panel2.Controls.Add(this.tvLinkman);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(216, 388);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 3;
            // 
            // tbSearchName
            // 
            this.tbSearchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSearchName.Location = new System.Drawing.Point(0, 0);
            this.tbSearchName.Name = "tbSearchName";
            this.tbSearchName.Size = new System.Drawing.Size(216, 21);
            this.tbSearchName.TabIndex = 2;
            this.tbSearchName.TextChanged += new System.EventHandler(this.tbSearchName_TextChanged);
            // 
            // lvSearchName
            // 
            this.lvSearchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSearchName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvSearchName.FullRowSelect = true;
            this.lvSearchName.GridLines = true;
            this.lvSearchName.HideSelection = false;
            this.lvSearchName.Location = new System.Drawing.Point(1, 0);
            this.lvSearchName.MultiSelect = false;
            this.lvSearchName.Name = "lvSearchName";
            this.lvSearchName.Size = new System.Drawing.Size(214, 148);
            this.lvSearchName.TabIndex = 2;
            this.lvSearchName.UseCompatibleStateImageBehavior = false;
            this.lvSearchName.View = System.Windows.Forms.View.Details;
            this.lvSearchName.Visible = false;
            this.lvSearchName.DoubleClick += new System.EventHandler(this.lvSearchName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "客服信息";
            this.columnHeader1.Width = 190;
            // 
            // tvLinkman
            // 
            this.tvLinkman.ContextMenuStrip = this.contextMenuStrip1;
            this.tvLinkman.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLinkman.HideSelection = false;
            this.tvLinkman.ImageIndex = 0;
            this.tvLinkman.ImageList = this.imgList;
            this.tvLinkman.ItemHeight = 24;
            this.tvLinkman.Location = new System.Drawing.Point(0, 0);
            this.tvLinkman.Name = "tvLinkman";
            treeNode3.ImageIndex = 3;
            treeNode3.Name = "节点2";
            treeNode3.SelectedImageIndex = 3;
            treeNode3.Text = "公司客服";
            treeNode4.ImageIndex = 3;
            treeNode4.Name = "节点0";
            treeNode4.SelectedImageIndex = 3;
            treeNode4.Text = "我的好友";
            this.tvLinkman.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.tvLinkman.SelectedImageIndex = 0;
            this.tvLinkman.ShowLines = false;
            this.tvLinkman.ShowRootLines = false;
            this.tvLinkman.Size = new System.Drawing.Size(216, 359);
            this.tvLinkman.TabIndex = 0;
            this.tvLinkman.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvLinkman_NodeMouseDoubleClick);
            this.tvLinkman.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvLinkman_AfterCollapse);
            this.tvLinkman.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvLinkman_AfterExpand);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tvSeatGroup);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(216, 388);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "群";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tvSeatGroup
            // 
            this.tvSeatGroup.ContextMenuStrip = this.contextMenuStrip1;
            this.tvSeatGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSeatGroup.HideSelection = false;
            this.tvSeatGroup.ImageIndex = 0;
            this.tvSeatGroup.ImageList = this.imgList;
            this.tvSeatGroup.ItemHeight = 24;
            this.tvSeatGroup.Location = new System.Drawing.Point(0, 0);
            this.tvSeatGroup.Name = "tvSeatGroup";
            treeNode5.ImageIndex = 3;
            treeNode5.Name = "节点0";
            treeNode5.SelectedImageIndex = 3;
            treeNode5.Text = "客服群";
            this.tvSeatGroup.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.tvSeatGroup.SelectedImageIndex = 0;
            this.tvSeatGroup.ShowLines = false;
            this.tvSeatGroup.ShowRootLines = false;
            this.tvSeatGroup.Size = new System.Drawing.Size(216, 388);
            this.tvSeatGroup.TabIndex = 1;
            this.tvSeatGroup.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSeatGroup_NodeMouseDoubleClick);
            this.tvSeatGroup.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvSeatGroup_AfterCollapse);
            this.tvSeatGroup.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvSeatGroup_AfterExpand);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.cmSysTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "面料QQ";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // cmSysTray
            // 
            this.cmSysTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.隐藏窗口ToolStripMenuItem,
            this.tsmiShowForm,
            this.toolStripSeparator4,
            this.历史记录ToolStripMenuItem1,
            this.留言信息ToolStripMenuItem,
            this.toolStripSeparator6,
            this.系统设置ToolStripMenuItem1,
            this.关于我们ToolStripMenuItem,
            this.toolStripSeparator3,
            this.更换用户ToolStripMenuItem,
            this.tsmiExit});
            this.cmSysTray.Name = "cmSysTray";
            this.cmSysTray.Size = new System.Drawing.Size(125, 198);
            // 
            // 隐藏窗口ToolStripMenuItem
            // 
            this.隐藏窗口ToolStripMenuItem.Name = "隐藏窗口ToolStripMenuItem";
            this.隐藏窗口ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.隐藏窗口ToolStripMenuItem.Text = "隐藏窗口";
            this.隐藏窗口ToolStripMenuItem.Click += new System.EventHandler(this.隐藏窗口ToolStripMenuItem_Click);
            // 
            // tsmiShowForm
            // 
            this.tsmiShowForm.Name = "tsmiShowForm";
            this.tsmiShowForm.Size = new System.Drawing.Size(124, 22);
            this.tsmiShowForm.Text = "恢复窗口";
            this.tsmiShowForm.Visible = false;
            this.tsmiShowForm.Click += new System.EventHandler(this.tsmiShowForm_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(121, 6);
            // 
            // 历史记录ToolStripMenuItem1
            // 
            this.历史记录ToolStripMenuItem1.Name = "历史记录ToolStripMenuItem1";
            this.历史记录ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.历史记录ToolStripMenuItem1.Text = "消息记录";
            this.历史记录ToolStripMenuItem1.Click += new System.EventHandler(this.历史记录ToolStripMenuItem_Click);
            // 
            // 留言信息ToolStripMenuItem
            // 
            this.留言信息ToolStripMenuItem.Name = "留言信息ToolStripMenuItem";
            this.留言信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.留言信息ToolStripMenuItem.Text = "留言管理";
            this.留言信息ToolStripMenuItem.Click += new System.EventHandler(this.留言信息ToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(121, 6);
            // 
            // 系统设置ToolStripMenuItem1
            // 
            this.系统设置ToolStripMenuItem1.Name = "系统设置ToolStripMenuItem1";
            this.系统设置ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.系统设置ToolStripMenuItem1.Text = "系统设置";
            this.系统设置ToolStripMenuItem1.Click += new System.EventHandler(this.系统设置ToolStripMenuItem_Click);
            // 
            // 关于我们ToolStripMenuItem
            // 
            this.关于我们ToolStripMenuItem.Name = "关于我们ToolStripMenuItem";
            this.关于我们ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.关于我们ToolStripMenuItem.Text = "关于我们";
            this.关于我们ToolStripMenuItem.Click += new System.EventHandler(this.关于我们ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(121, 6);
            // 
            // 更换用户ToolStripMenuItem
            // 
            this.更换用户ToolStripMenuItem.Name = "更换用户ToolStripMenuItem";
            this.更换用户ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.更换用户ToolStripMenuItem.Text = "更换用户";
            this.更换用户ToolStripMenuItem.Click += new System.EventHandler(this.更换用户ToolStripMenuItem_Click);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(124, 22);
            this.tsmiExit.Text = "退出系统";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除好友ToolStripMenuItem,
            this.修改备注名称ToolStripMenuItem,
            this.查看资料ToolStripMenuItem,
            this.toolStripSeparator7,
            this.刷新ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(149, 98);
            // 
            // 删除好友ToolStripMenuItem
            // 
            this.删除好友ToolStripMenuItem.Name = "删除好友ToolStripMenuItem";
            this.删除好友ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除好友ToolStripMenuItem.Text = "删除好友";
            this.删除好友ToolStripMenuItem.Click += new System.EventHandler(this.删除好友ToolStripMenuItem_Click);
            // 
            // 修改备注名称ToolStripMenuItem
            // 
            this.修改备注名称ToolStripMenuItem.Name = "修改备注名称ToolStripMenuItem";
            this.修改备注名称ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.修改备注名称ToolStripMenuItem.Text = "修改备注名称";
            this.修改备注名称ToolStripMenuItem.Click += new System.EventHandler(this.修改备注名称ToolStripMenuItem_Click);
            // 
            // 查看资料ToolStripMenuItem
            // 
            this.查看资料ToolStripMenuItem.Name = "查看资料ToolStripMenuItem";
            this.查看资料ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.查看资料ToolStripMenuItem.Text = "查看资料";
            this.查看资料ToolStripMenuItem.Click += new System.EventHandler(this.查看资料ToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(145, 6);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新RToolStripMenuItem_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.查看资料ToolStripMenuItem1,
            this.toolStripSeparator8,
            this.toolStripMenuItem3});
            this.contextMenuStrip3.Name = "contextMenuStrip2";
            this.contextMenuStrip3.Size = new System.Drawing.Size(125, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "加为好友";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.加为好友ToolStripMenuItem_Click);
            // 
            // 查看资料ToolStripMenuItem1
            // 
            this.查看资料ToolStripMenuItem1.Name = "查看资料ToolStripMenuItem1";
            this.查看资料ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.查看资料ToolStripMenuItem1.Text = "查看资料";
            this.查看资料ToolStripMenuItem1.Click += new System.EventHandler(this.查看资料ToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem3.Text = "刷新";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.刷新RToolStripMenuItem_Click);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.消息记录ToolStripMenuItem,
            this.toolStripSeparator9,
            this.toolStripMenuItem4});
            this.contextMenuStrip4.Name = "contextMenuStrip2";
            this.contextMenuStrip4.Size = new System.Drawing.Size(125, 76);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "关闭会话";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 消息记录ToolStripMenuItem
            // 
            this.消息记录ToolStripMenuItem.Name = "消息记录ToolStripMenuItem";
            this.消息记录ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.消息记录ToolStripMenuItem.Text = "消息记录";
            this.消息记录ToolStripMenuItem.Click += new System.EventHandler(this.消息记录ToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem4.Text = "刷新";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.刷新RToolStripMenuItem_Click);
            // 
            // OFFICE2007ToolStripMenuItem
            // 
            this.OFFICE2007ToolStripMenuItem.Name = "OFFICE2007ToolStripMenuItem";
            this.OFFICE2007ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.OFFICE2007ToolStripMenuItem.Tag = "office2007";
            this.OFFICE2007ToolStripMenuItem.Text = "OFFICE2007";
            // 
            // mP10ToolStripMenuItem
            // 
            this.mP10ToolStripMenuItem.Name = "mP10ToolStripMenuItem";
            this.mP10ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.mP10ToolStripMenuItem.Tag = "mp10";
            this.mP10ToolStripMenuItem.Text = "MP10";
            // 
            // pAGEToolStripMenuItem
            // 
            this.pAGEToolStripMenuItem.Name = "pAGEToolStripMenuItem";
            this.pAGEToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.pAGEToolStripMenuItem.Tag = "page";
            this.pAGEToolStripMenuItem.Text = "PAGE";
            // 
            // mSNToolStripMenuItem
            // 
            this.mSNToolStripMenuItem.Name = "mSNToolStripMenuItem";
            this.mSNToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.mSNToolStripMenuItem.Tag = "msn";
            this.mSNToolStripMenuItem.Text = "MSN";
            // 
            // wINXPToolStripMenuItem
            // 
            this.wINXPToolStripMenuItem.Name = "wINXPToolStripMenuItem";
            this.wINXPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.wINXPToolStripMenuItem.Tag = "winxp";
            this.wINXPToolStripMenuItem.Text = "WINXP";
            // 
            // rOYALEToolStripMenuItem
            // 
            this.rOYALEToolStripMenuItem.Name = "rOYALEToolStripMenuItem";
            this.rOYALEToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.rOYALEToolStripMenuItem.Tag = "royale";
            this.rOYALEToolStripMenuItem.Text = "ROYALE";
            // 
            // dEEPToolStripMenuItem
            // 
            this.dEEPToolStripMenuItem.Name = "dEEPToolStripMenuItem";
            this.dEEPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.dEEPToolStripMenuItem.Tag = "deep";
            this.dEEPToolStripMenuItem.Text = "DEEP";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem5.Tag = "office2007";
            this.toolStripMenuItem5.Text = "OFFICE2007";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem6.Tag = "mp10";
            this.toolStripMenuItem6.Text = "MP10";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem7.Tag = "page";
            this.toolStripMenuItem7.Text = "PAGE";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem8.Tag = "msn";
            this.toolStripMenuItem8.Text = "MSN";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem9.Tag = "winxp";
            this.toolStripMenuItem9.Text = "WINXP";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem10.Tag = "royale";
            this.toolStripMenuItem10.Text = "ROYALE";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem11.Tag = "deep";
            this.toolStripMenuItem11.Text = "DEEP";
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem12.Tag = "office2007";
            this.toolStripMenuItem12.Text = "OFFICE2007";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem13.Tag = "mp10";
            this.toolStripMenuItem13.Text = "MP10";
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem14.Tag = "page";
            this.toolStripMenuItem14.Text = "PAGE";
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem15.Tag = "msn";
            this.toolStripMenuItem15.Text = "MSN";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem16.Tag = "winxp";
            this.toolStripMenuItem16.Text = "WINXP";
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem17.Tag = "royale";
            this.toolStripMenuItem17.Text = "ROYALE";
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem18.Tag = "deep";
            this.toolStripMenuItem18.Text = "DEEP";
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem19.Tag = "office2007";
            this.toolStripMenuItem19.Text = "OFFICE2007";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem20.Tag = "mp10";
            this.toolStripMenuItem20.Text = "MP10";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem21.Tag = "page";
            this.toolStripMenuItem21.Text = "PAGE";
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem22.Tag = "msn";
            this.toolStripMenuItem22.Text = "MSN";
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem23.Tag = "winxp";
            this.toolStripMenuItem23.Text = "WINXP";
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem24.Tag = "royale";
            this.toolStripMenuItem24.Text = "ROYALE";
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem25.Tag = "deep";
            this.toolStripMenuItem25.Text = "DEEP";
            // 
            // contextMenuStrip5
            // 
            this.contextMenuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem33,
            this.消息记录ToolStripMenuItem1,
            this.toolStripSeparator10,
            this.toolStripMenuItem34});
            this.contextMenuStrip5.Name = "contextMenuStrip2";
            this.contextMenuStrip5.Size = new System.Drawing.Size(125, 76);
            // 
            // toolStripMenuItem33
            // 
            this.toolStripMenuItem33.Name = "toolStripMenuItem33";
            this.toolStripMenuItem33.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem33.Text = "接受会话";
            this.toolStripMenuItem33.Click += new System.EventHandler(this.toolStripMenuItem33_Click);
            // 
            // 消息记录ToolStripMenuItem1
            // 
            this.消息记录ToolStripMenuItem1.Name = "消息记录ToolStripMenuItem1";
            this.消息记录ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.消息记录ToolStripMenuItem1.Text = "消息记录";
            this.消息记录ToolStripMenuItem1.Click += new System.EventHandler(this.消息记录ToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem34
            // 
            this.toolStripMenuItem34.Name = "toolStripMenuItem34";
            this.toolStripMenuItem34.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem34.Text = "刷新";
            // 
            // contextMenuStrip6
            // 
            this.contextMenuStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看个人资料ToolStripMenuItem,
            this.toolStripSeparator11,
            this.修改头像ToolStripMenuItem});
            this.contextMenuStrip6.Name = "contextMenuStrip6";
            this.contextMenuStrip6.Size = new System.Drawing.Size(149, 54);
            // 
            // 查看个人资料ToolStripMenuItem
            // 
            this.查看个人资料ToolStripMenuItem.Name = "查看个人资料ToolStripMenuItem";
            this.查看个人资料ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.查看个人资料ToolStripMenuItem.Text = "查看个人资料";
            this.查看个人资料ToolStripMenuItem.Click += new System.EventHandler(this.查看个人资料ToolStripMenuItem_Click);
            // 
            // 修改头像ToolStripMenuItem
            // 
            this.修改头像ToolStripMenuItem.Name = "修改头像ToolStripMenuItem";
            this.修改头像ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.修改头像ToolStripMenuItem.Text = "修改头像";
            this.修改头像ToolStripMenuItem.Click += new System.EventHandler(this.修改头像ToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(145, 6);
            // 
            // frmNavigate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 562);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmNavigate";
            this.Text = "面料QQ";
            this.Load += new System.EventHandler(this.frmNavigate_Load);
            this.SizeChanged += new System.EventHandler(this.frmNavigate_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNavigate_FormClosing);
            this.LocationChanged += new System.EventHandler(this.frmNavigate_LocationChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSeatFace)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.cmSysTray.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.contextMenuStrip4.ResumeLayout(false);
            this.contextMenuStrip5.ResumeLayout(false);
            this.contextMenuStrip6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblSign;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.PictureBox pbSeatFace;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 个人资料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 版本信息ToolStripMenuItem;
        private DoubleBufferTreeView tvSession;
        private System.Windows.Forms.ImageList imgList;
        private DoubleBufferTreeView tvLinkman;
        private System.Windows.Forms.TabPage tabPage3;
        private DoubleBufferTreeView tvSeatGroup;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip cmSysTray;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowForm;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem 修改密码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 留言ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于我们ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 留言信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 历史记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史记录ToolStripMenuItem1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刷新RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem 更换用户ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 更换用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton tsUserName;
        private System.Windows.Forms.ToolStripMenuItem 在线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 离开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 忙碌ToolStripMenuItem;
        private DoubleBufferListView lvSearchName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 删除好友ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tbSearchName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem OFFICE2007ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mP10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pAGEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wINXPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rOYALEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dEEPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem21;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem22;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem23;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem24;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem25;
        private System.Windows.Forms.ToolStripMenuItem 皮肤切换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem26;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem27;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem28;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem29;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem30;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem31;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem33;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem34;
        private System.Windows.Forms.ToolStripMenuItem 修改备注名称ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看资料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看资料ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 消息记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 消息记录ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 在线升级ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip6;
        private System.Windows.Forms.ToolStripMenuItem 查看个人资料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem 修改头像ToolStripMenuItem;
    }
}