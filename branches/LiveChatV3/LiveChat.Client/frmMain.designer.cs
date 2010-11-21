namespace LiveChat.Client
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。

        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。

        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (chattimer != null)
            {
                chattimer.Stop();
                chattimer = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。

        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripButton tsbExit;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAcceptTalk = new System.Windows.Forms.ToolStripButton();
            this.tsbEndTalk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChatMessage = new System.Windows.Forms.ToolStripButton();
            this.tsbLeave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.chkSelectTalk = new System.Windows.Forms.CheckBox();
            this.chkChangeTalk = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTalkCount = new System.Windows.Forms.ComboBox();
            this.chkAutoAccept = new System.Windows.Forms.CheckBox();
            this.dgvTalks = new System.Windows.Forms.DataGridView();
            this.UserName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTime1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeatName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssiCurrentCompany = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssiCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssiClientState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssiInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTalks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbExit
            // 
            tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            tsbExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbExit.Name = "tsbExit";
            tsbExit.Size = new System.Drawing.Size(60, 53);
            tsbExit.Text = "关闭窗口";
            tsbExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAcceptTalk,
            this.tsbEndTalk,
            this.toolStripSeparator4,
            this.tsbChatMessage,
            this.tsbLeave,
            this.toolStripSeparator2,
            this.toolStripButton2,
            this.toolStripSeparator1,
            tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 56);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAcceptTalk
            // 
            this.tsbAcceptTalk.Enabled = false;
            this.tsbAcceptTalk.Image = ((System.Drawing.Image)(resources.GetObject("tsbAcceptTalk.Image")));
            this.tsbAcceptTalk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAcceptTalk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAcceptTalk.Name = "tsbAcceptTalk";
            this.tsbAcceptTalk.Size = new System.Drawing.Size(60, 53);
            this.tsbAcceptTalk.Text = "接受会话";
            this.tsbAcceptTalk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAcceptTalk.Click += new System.EventHandler(this.tsbAcceptTalk_Click);
            // 
            // tsbEndTalk
            // 
            this.tsbEndTalk.Enabled = false;
            this.tsbEndTalk.Image = ((System.Drawing.Image)(resources.GetObject("tsbEndTalk.Image")));
            this.tsbEndTalk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEndTalk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEndTalk.Name = "tsbEndTalk";
            this.tsbEndTalk.Size = new System.Drawing.Size(60, 53);
            this.tsbEndTalk.Text = "结束会话";
            this.tsbEndTalk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEndTalk.Click += new System.EventHandler(this.tsbEndTalk_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 56);
            // 
            // tsbChatMessage
            // 
            this.tsbChatMessage.Image = global::LiveChat.Client.Properties.Resources.tool1;
            this.tsbChatMessage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbChatMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbChatMessage.Name = "tsbChatMessage";
            this.tsbChatMessage.Size = new System.Drawing.Size(60, 53);
            this.tsbChatMessage.Text = "消息记录";
            this.tsbChatMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbChatMessage.Click += new System.EventHandler(this.tsbChatMessage_Click);
            // 
            // tsbLeave
            // 
            this.tsbLeave.Image = global::LiveChat.Client.Properties.Resources.tool4;
            this.tsbLeave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbLeave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLeave.Name = "tsbLeave";
            this.tsbLeave.Size = new System.Drawing.Size(60, 53);
            this.tsbLeave.Text = "留言管理";
            this.tsbLeave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbLeave.Click += new System.EventHandler(this.tsbLeave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::LiveChat.Client.Properties.Resources.tool6;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(60, 53);
            this.toolStripButton2.Text = "后台管理";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 56);
            // 
            // chkSelectTalk
            // 
            this.chkSelectTalk.AutoSize = true;
            this.chkSelectTalk.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectTalk.Checked = true;
            this.chkSelectTalk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectTalk.Location = new System.Drawing.Point(12, 11);
            this.chkSelectTalk.Name = "chkSelectTalk";
            this.chkSelectTalk.Size = new System.Drawing.Size(168, 16);
            this.chkSelectTalk.TabIndex = 3;
            this.chkSelectTalk.Text = "只显示会话请求和我的会话";
            this.chkSelectTalk.UseVisualStyleBackColor = false;
            // 
            // chkChangeTalk
            // 
            this.chkChangeTalk.AutoSize = true;
            this.chkChangeTalk.BackColor = System.Drawing.Color.Transparent;
            this.chkChangeTalk.Checked = true;
            this.chkChangeTalk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeTalk.Location = new System.Drawing.Point(12, 33);
            this.chkChangeTalk.Name = "chkChangeTalk";
            this.chkChangeTalk.Size = new System.Drawing.Size(204, 16);
            this.chkChangeTalk.TabIndex = 5;
            this.chkChangeTalk.Text = "有新消息来时自动切换到当前会话";
            this.chkChangeTalk.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboTalkCount);
            this.panel1.Controls.Add(this.chkAutoAccept);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 526);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 36);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "个会话（目前允许客服最大接入数为20）";
            // 
            // cboTalkCount
            // 
            this.cboTalkCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTalkCount.FormattingEnabled = true;
            this.cboTalkCount.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15",
            "20"});
            this.cboTalkCount.Location = new System.Drawing.Point(85, 8);
            this.cboTalkCount.Name = "cboTalkCount";
            this.cboTalkCount.Size = new System.Drawing.Size(58, 20);
            this.cboTalkCount.TabIndex = 7;
            // 
            // chkAutoAccept
            // 
            this.chkAutoAccept.AutoSize = true;
            this.chkAutoAccept.Location = new System.Drawing.Point(12, 11);
            this.chkAutoAccept.Name = "chkAutoAccept";
            this.chkAutoAccept.Size = new System.Drawing.Size(72, 16);
            this.chkAutoAccept.TabIndex = 6;
            this.chkAutoAccept.Text = "自动接受";
            this.chkAutoAccept.UseVisualStyleBackColor = true;
            // 
            // dgvTalks
            // 
            this.dgvTalks.AllowUserToAddRows = false;
            this.dgvTalks.AllowUserToDeleteRows = false;
            this.dgvTalks.AllowUserToResizeColumns = false;
            this.dgvTalks.AllowUserToResizeRows = false;
            this.dgvTalks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTalks.BackgroundColor = System.Drawing.Color.White;
            this.dgvTalks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTalks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName1,
            this.RequestSeat,
            this.RequestTime,
            this.Message1,
            this.LastTime1,
            this.IP1,
            this.From1});
            this.dgvTalks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTalks.GridColor = System.Drawing.SystemColors.Control;
            this.dgvTalks.Location = new System.Drawing.Point(0, 0);
            this.dgvTalks.MultiSelect = false;
            this.dgvTalks.Name = "dgvTalks";
            this.dgvTalks.RowHeadersVisible = false;
            this.dgvTalks.RowTemplate.Height = 23;
            this.dgvTalks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTalks.Size = new System.Drawing.Size(784, 217);
            this.dgvTalks.TabIndex = 4;
            this.dgvTalks.DoubleClick += new System.EventHandler(this.dgvTalks_DoubleClick);
            this.dgvTalks.Click += new System.EventHandler(this.dgvTalks_Click);
            // 
            // UserName1
            // 
            this.UserName1.DataPropertyName = "UserName";
            this.UserName1.HeaderText = "用户名";
            this.UserName1.Name = "UserName1";
            this.UserName1.ReadOnly = true;
            this.UserName1.Width = 66;
            // 
            // RequestSeat
            // 
            this.RequestSeat.DataPropertyName = "RequestSeat";
            this.RequestSeat.HeaderText = "指定客服";
            this.RequestSeat.Name = "RequestSeat";
            this.RequestSeat.ReadOnly = true;
            this.RequestSeat.Width = 78;
            // 
            // RequestTime
            // 
            this.RequestTime.DataPropertyName = "RequestTime";
            this.RequestTime.HeaderText = "请求时间";
            this.RequestTime.Name = "RequestTime";
            this.RequestTime.ReadOnly = true;
            this.RequestTime.Width = 78;
            // 
            // Message1
            // 
            this.Message1.DataPropertyName = "Message";
            this.Message1.HeaderText = "请求消息";
            this.Message1.Name = "Message1";
            this.Message1.ReadOnly = true;
            this.Message1.Width = 78;
            // 
            // LastTime1
            // 
            this.LastTime1.DataPropertyName = "LastTime";
            this.LastTime1.HeaderText = "最后会话时间";
            this.LastTime1.Name = "LastTime1";
            this.LastTime1.ReadOnly = true;
            this.LastTime1.Width = 102;
            // 
            // IP1
            // 
            this.IP1.DataPropertyName = "IP";
            this.IP1.HeaderText = "IP";
            this.IP1.Name = "IP1";
            this.IP1.ReadOnly = true;
            this.IP1.Width = 42;
            // 
            // From1
            // 
            this.From1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.From1.DataPropertyName = "From";
            this.From1.HeaderText = "来自";
            this.From1.Name = "From1";
            this.From1.ReadOnly = true;
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvClients.BackgroundColor = System.Drawing.Color.White;
            this.dgvClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.SeatName2,
            this.StartTime,
            this.Message,
            this.LastTime,
            this.IP,
            this.From});
            this.dgvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClients.GridColor = System.Drawing.SystemColors.Control;
            this.dgvClients.Location = new System.Drawing.Point(0, 0);
            this.dgvClients.MultiSelect = false;
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.RowHeadersVisible = false;
            this.dgvClients.RowTemplate.Height = 23;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(784, 224);
            this.dgvClients.TabIndex = 3;
            this.dgvClients.DoubleClick += new System.EventHandler(this.dgvClients_DoubleClick);
            this.dgvClients.Click += new System.EventHandler(this.dgvClients_Click);
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "用户名";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Width = 66;
            // 
            // SeatName2
            // 
            this.SeatName2.DataPropertyName = "SeatName";
            this.SeatName2.HeaderText = "客服名";
            this.SeatName2.Name = "SeatName2";
            this.SeatName2.ReadOnly = true;
            this.SeatName2.Width = 66;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "开始时间";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.Width = 78;
            // 
            // Message
            // 
            this.Message.DataPropertyName = "Message";
            this.Message.HeaderText = "未读消息";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.Width = 78;
            // 
            // LastTime
            // 
            this.LastTime.DataPropertyName = "LastTime";
            this.LastTime.HeaderText = "最后会话时间";
            this.LastTime.Name = "LastTime";
            this.LastTime.ReadOnly = true;
            this.LastTime.Width = 102;
            // 
            // IP
            // 
            this.IP.DataPropertyName = "IP";
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            this.IP.Width = 42;
            // 
            // From
            // 
            this.From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.From.DataPropertyName = "From";
            this.From.HeaderText = "来自";
            this.From.Name = "From";
            this.From.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusStrip1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssiCurrentCompany,
            this.tssiCurrentUser,
            this.tssiClientState,
            this.tssiInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 501);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(784, 25);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "状态栏";
            // 
            // tssiCurrentCompany
            // 
            this.tssiCurrentCompany.AutoSize = false;
            this.tssiCurrentCompany.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssiCurrentCompany.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssiCurrentCompany.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssiCurrentCompany.Name = "tssiCurrentCompany";
            this.tssiCurrentCompany.RightToLeftAutoMirrorImage = true;
            this.tssiCurrentCompany.Size = new System.Drawing.Size(150, 20);
            this.tssiCurrentCompany.Text = "公司ID";
            this.tssiCurrentCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tssiCurrentCompany.ToolTipText = "当前公司ID";
            // 
            // tssiCurrentUser
            // 
            this.tssiCurrentUser.AutoSize = false;
            this.tssiCurrentUser.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssiCurrentUser.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssiCurrentUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssiCurrentUser.Name = "tssiCurrentUser";
            this.tssiCurrentUser.Size = new System.Drawing.Size(120, 20);
            this.tssiCurrentUser.Text = "客服ID";
            this.tssiCurrentUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tssiCurrentUser.ToolTipText = "当前客服ID";
            // 
            // tssiClientState
            // 
            this.tssiClientState.AutoSize = false;
            this.tssiClientState.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssiClientState.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssiClientState.Name = "tssiClientState";
            this.tssiClientState.Size = new System.Drawing.Size(50, 20);
            this.tssiClientState.Text = "联机";
            this.tssiClientState.ToolTipText = "客服在线状态";
            // 
            // tssiInfo
            // 
            this.tssiInfo.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssiInfo.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssiInfo.Name = "tssiInfo";
            this.tssiInfo.Size = new System.Drawing.Size(449, 20);
            this.tssiInfo.Spring = true;
            this.tssiInfo.Text = "各种状态用户信息";
            this.tssiInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tssiInfo.ToolTipText = "丰富的访客状态统计信息";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 25);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.系统ToolStripMenuItem.Text = "系统";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 56);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvTalks);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvClients);
            this.splitContainer1.Size = new System.Drawing.Size(784, 445);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.webBrowser1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 445);
            this.panel2.TabIndex = 5;
            this.panel2.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(784, 445);
            this.webBrowser1.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "会话列表";
            this.Load += new System.EventHandler(this.Main_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTalks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAcceptTalk;
        private System.Windows.Forms.ToolStripButton tsbEndTalk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbChatMessage;
        private System.Windows.Forms.CheckBox chkSelectTalk;
        private System.Windows.Forms.CheckBox chkChangeTalk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssiCurrentCompany;
        private System.Windows.Forms.ToolStripStatusLabel tssiCurrentUser;
        private System.Windows.Forms.ToolStripStatusLabel tssiClientState;
        private System.Windows.Forms.ToolStripStatusLabel tssiInfo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgvTalks;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestSeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTime1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP1;
        private System.Windows.Forms.DataGridViewTextBoxColumn From1;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.CheckBox chkAutoAccept;
        private System.Windows.Forms.ComboBox cboTalkCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton tsbLeave;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeatName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.WebBrowser webBrowser1;

    }
}