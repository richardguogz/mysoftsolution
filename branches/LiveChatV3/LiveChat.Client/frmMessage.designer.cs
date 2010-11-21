namespace LiveChat.Client
{
    partial class frmMessage
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
            if (wbChatBox != null) wbChatBox.Dispose();
            if (wbChatHistory != null) wbChatHistory.Dispose();

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
            System.Windows.Forms.ToolStripButton tsbExit;
            System.Windows.Forms.ToolStripButton tsbRefresh;
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("公司客服", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("我的好友", 0, 0);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessage));
            this.wbChatBox = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvSessions = new System.Windows.Forms.DataGridView();
            this.AddTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.wbChatHistory = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tvLinkman = new LiveChat.Client.DoubleBufferTreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.wbFriendChat = new System.Windows.Forms.WebBrowser();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSessions)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbExit
            // 
            tsbExit.Image = global::LiveChat.Client.Properties.Resources.endsession;
            tsbExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbExit.Name = "tsbExit";
            tsbExit.Size = new System.Drawing.Size(61, 28);
            tsbExit.Text = "关闭";
            tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // tsbRefresh
            // 
            tsbRefresh.Image = global::LiveChat.Client.Properties.Resources.refresh;
            tsbRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbRefresh.Name = "tsbRefresh";
            tsbRefresh.Size = new System.Drawing.Size(60, 28);
            tsbRefresh.Text = "刷新";
            tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // wbChatBox
            // 
            this.wbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatBox.Location = new System.Drawing.Point(0, 0);
            this.wbChatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatBox.Name = "wbChatBox";
            this.wbChatBox.Size = new System.Drawing.Size(784, 432);
            this.wbChatBox.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsbRefresh,
            tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 31);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(108, 24);
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(792, 464);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.wbChatBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(784, 432);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "当前访客聊天记录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(784, 432);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "历史访客聊天记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 42);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvSessions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(784, 390);
            this.splitContainer1.SplitterDistance = 402;
            this.splitContainer1.TabIndex = 1;
            // 
            // dgvSessions
            // 
            this.dgvSessions.AllowUserToAddRows = false;
            this.dgvSessions.AllowUserToDeleteRows = false;
            this.dgvSessions.AllowUserToResizeColumns = false;
            this.dgvSessions.AllowUserToResizeRows = false;
            this.dgvSessions.BackgroundColor = System.Drawing.Color.White;
            this.dgvSessions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSessions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AddTime,
            this.UserName1,
            this.RequestSeat,
            this.RequestTime});
            this.dgvSessions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSessions.GridColor = System.Drawing.SystemColors.Control;
            this.dgvSessions.Location = new System.Drawing.Point(0, 0);
            this.dgvSessions.MultiSelect = false;
            this.dgvSessions.Name = "dgvSessions";
            this.dgvSessions.RowHeadersVisible = false;
            this.dgvSessions.RowTemplate.Height = 23;
            this.dgvSessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSessions.Size = new System.Drawing.Size(402, 390);
            this.dgvSessions.TabIndex = 13;
            this.dgvSessions.Click += new System.EventHandler(this.dgvSessions_Click);
            // 
            // AddTime
            // 
            this.AddTime.DataPropertyName = "UserName";
            this.AddTime.HeaderText = "访客名称";
            this.AddTime.Name = "AddTime";
            this.AddTime.Width = 70;
            // 
            // UserName1
            // 
            this.UserName1.DataPropertyName = "SeatName";
            this.UserName1.HeaderText = "客服名称";
            this.UserName1.Name = "UserName1";
            this.UserName1.ReadOnly = true;
            // 
            // RequestSeat
            // 
            this.RequestSeat.DataPropertyName = "StartTime";
            this.RequestSeat.HeaderText = "开始时间";
            this.RequestSeat.Name = "RequestSeat";
            this.RequestSeat.ReadOnly = true;
            this.RequestSeat.Width = 120;
            // 
            // RequestTime
            // 
            this.RequestTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RequestTime.DataPropertyName = "LastTime";
            this.RequestTime.HeaderText = "结束时间";
            this.RequestTime.Name = "RequestTime";
            this.RequestTime.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.wbChatHistory);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(378, 390);
            this.panel2.TabIndex = 9;
            // 
            // wbChatHistory
            // 
            this.wbChatHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatHistory.Location = new System.Drawing.Point(0, 0);
            this.wbChatHistory.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatHistory.Name = "wbChatHistory";
            this.wbChatHistory.Size = new System.Drawing.Size(376, 388);
            this.wbChatHistory.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.dtpEndTime);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpStartTime);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 42);
            this.panel1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 11);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(110, 21);
            this.textBox1.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(682, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy年MM月dd日 HH:mm";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(492, 10);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(169, 21);
            this.dtpEndTime.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(433, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "开始时间：";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "yyyy年MM月dd日 HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(246, 10);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(169, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户ID：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始时间：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(784, 432);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "好友聊天记录";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tvLinkman);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.wbFriendChat);
            this.splitContainer2.Panel2.Controls.Add(this.panel3);
            this.splitContainer2.Size = new System.Drawing.Size(784, 432);
            this.splitContainer2.SplitterDistance = 220;
            this.splitContainer2.TabIndex = 0;
            // 
            // tvLinkman
            // 
            this.tvLinkman.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLinkman.HideSelection = false;
            this.tvLinkman.ImageIndex = 0;
            this.tvLinkman.ImageList = this.imgList;
            this.tvLinkman.ItemHeight = 24;
            this.tvLinkman.Location = new System.Drawing.Point(0, 0);
            this.tvLinkman.Name = "tvLinkman";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "节点2";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "公司客服";
            treeNode2.ImageIndex = 0;
            treeNode2.Name = "节点0";
            treeNode2.SelectedImageIndex = 0;
            treeNode2.Text = "我的好友";
            this.tvLinkman.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.tvLinkman.SelectedImageIndex = 0;
            this.tvLinkman.ShowLines = false;
            this.tvLinkman.ShowRootLines = false;
            this.tvLinkman.Size = new System.Drawing.Size(220, 432);
            this.tvLinkman.TabIndex = 1;
            this.tvLinkman.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvLinkman_AfterCollapse);
            this.tvLinkman.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvLinkman_NodeMouseClick);
            this.tvLinkman.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvLinkman_AfterExpand);
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
            this.imgList.Images.SetKeyName(5, "人.ico");
            this.imgList.Images.SetKeyName(6, "离开.ico");
            // 
            // wbFriendChat
            // 
            this.wbFriendChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbFriendChat.Location = new System.Drawing.Point(0, 42);
            this.wbFriendChat.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbFriendChat.Name = "wbFriendChat";
            this.wbFriendChat.Size = new System.Drawing.Size(560, 390);
            this.wbFriendChat.TabIndex = 9;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.dateTimePicker2);
            this.panel3.Controls.Add(this.dateTimePicker1);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(560, 42);
            this.panel3.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定(&O)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(245, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "-";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy年MM月dd日";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(260, 10);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(140, 21);
            this.dateTimePicker2.TabIndex = 2;
            this.dateTimePicker2.Value = new System.DateTime(2010, 9, 18, 15, 20, 10, 0);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy年MM月dd日";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(99, 10);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(140, 21);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.Value = new System.DateTime(2010, 9, 18, 15, 20, 10, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "请选择日期：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 495);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 10;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(74, 17);
            this.toolStripStatusLabel1.Text = "准备状态......";
            // 
            // frmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 517);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "消息记录";
            this.Load += new System.EventHandler(this.frmMessage_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSessions)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbChatBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvSessions;
        private System.Windows.Forms.WebBrowser wbChatHistory;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestSeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestTime;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DoubleBufferTreeView tvLinkman;
        private System.Windows.Forms.WebBrowser wbFriendChat;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
    }
}