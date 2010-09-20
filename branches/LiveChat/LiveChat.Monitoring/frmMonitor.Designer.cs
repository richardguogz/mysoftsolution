namespace LiveChat.Monitoring
{
    partial class frmMonitor
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SeatName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SessionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭所有会话ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.显示在线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.让此客服离线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SeatName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.我来回答ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭此会话ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.UserName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTime1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.分配此请求给ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.接受此请求ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(705, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(705, 312);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeatName1,
            this.SessionCount});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(180, 312);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // SeatName1
            // 
            this.SeatName1.DataPropertyName = "SeatName";
            this.SeatName1.FillWeight = 120F;
            this.SeatName1.HeaderText = "客服名";
            this.SeatName1.Name = "SeatName1";
            this.SeatName1.ReadOnly = true;
            this.SeatName1.Width = 120;
            // 
            // SessionCount
            // 
            this.SessionCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SessionCount.DataPropertyName = "SessionCount";
            this.SessionCount.FillWeight = 50F;
            this.SessionCount.HeaderText = "会话数";
            this.SessionCount.Name = "SessionCount";
            this.SessionCount.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭所有会话ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.显示在线ToolStripMenuItem,
            this.让此客服离线ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 76);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // 关闭所有会话ToolStripMenuItem
            // 
            this.关闭所有会话ToolStripMenuItem.Name = "关闭所有会话ToolStripMenuItem";
            this.关闭所有会话ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭所有会话ToolStripMenuItem.Text = "关闭所有会话";
            this.关闭所有会话ToolStripMenuItem.Click += new System.EventHandler(this.关闭所有会话ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // 显示在线ToolStripMenuItem
            // 
            this.显示在线ToolStripMenuItem.Name = "显示在线ToolStripMenuItem";
            this.显示在线ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.显示在线ToolStripMenuItem.Text = "显示在线";
            this.显示在线ToolStripMenuItem.Click += new System.EventHandler(this.显示在线ToolStripMenuItem_Click);
            // 
            // 让此客服离线ToolStripMenuItem
            // 
            this.让此客服离线ToolStripMenuItem.Name = "让此客服离线ToolStripMenuItem";
            this.让此客服离线ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.让此客服离线ToolStripMenuItem.Text = "让此客服离线";
            this.让此客服离线ToolStripMenuItem.Click += new System.EventHandler(this.让此客服离线ToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer2.Size = new System.Drawing.Size(521, 312);
            this.splitContainer2.SplitterDistance = 120;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.SeatName2,
            this.StartTime,
            this.Message,
            this.LastTime,
            this.IP,
            this.From});
            this.dataGridView2.ContextMenuStrip = this.contextMenuStrip3;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(521, 120);
            this.dataGridView2.TabIndex = 2;
            this.dataGridView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDown);
            this.dataGridView2.DoubleClick += new System.EventHandler(this.dataGridView2_DoubleClick);
            this.dataGridView2.Click += new System.EventHandler(this.dataGridView2_Click);
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
            this.Message.HeaderText = "请求消息";
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
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.我来回答ToolStripMenuItem,
            this.toolStripSeparator2,
            this.关闭此会话ToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(137, 54);
            // 
            // 我来回答ToolStripMenuItem
            // 
            this.我来回答ToolStripMenuItem.Name = "我来回答ToolStripMenuItem";
            this.我来回答ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.我来回答ToolStripMenuItem.Text = "我来回答";
            this.我来回答ToolStripMenuItem.Click += new System.EventHandler(this.我来回答ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
            // 
            // 关闭此会话ToolStripMenuItem
            // 
            this.关闭此会话ToolStripMenuItem.Name = "关闭此会话ToolStripMenuItem";
            this.关闭此会话ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.关闭此会话ToolStripMenuItem.Text = "关闭此会话";
            this.关闭此会话ToolStripMenuItem.Click += new System.EventHandler(this.关闭此会话ToolStripMenuItem_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(705, 416);
            this.splitContainer3.SplitterDistance = 100;
            this.splitContainer3.TabIndex = 0;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AllowUserToResizeColumns = false;
            this.dataGridView3.AllowUserToResizeRows = false;
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView3.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName1,
            this.RequestSeat,
            this.RequestTime,
            this.Message1,
            this.LastTime1,
            this.IP1,
            this.From1});
            this.dataGridView3.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(0, 0);
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(705, 100);
            this.dataGridView3.TabIndex = 3;
            this.dataGridView3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView3_MouseDown);
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
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.分配此请求给ToolStripMenuItem,
            this.toolStripSeparator1,
            this.接受此请求ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(149, 54);
            // 
            // 分配此请求给ToolStripMenuItem
            // 
            this.分配此请求给ToolStripMenuItem.Name = "分配此请求给ToolStripMenuItem";
            this.分配此请求给ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.分配此请求给ToolStripMenuItem.Text = "分配此请求给";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // 接受此请求ToolStripMenuItem
            // 
            this.接受此请求ToolStripMenuItem.Name = "接受此请求ToolStripMenuItem";
            this.接受此请求ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.接受此请求ToolStripMenuItem.Text = "接受此请求";
            this.接受此请求ToolStripMenuItem.Click += new System.EventHandler(this.接受此请求ToolStripMenuItem_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(521, 188);
            this.webBrowser1.TabIndex = 0;
            // 
            // frmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 438);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客服会话";
            this.Load += new System.EventHandler(this.frmMonitor_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMonitor_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeatName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestSeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTime1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP1;
        private System.Windows.Forms.DataGridViewTextBoxColumn From1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeatName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SessionCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 关闭所有会话ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示在线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分配此请求给ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 让此客服离线ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 我来回答ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 接受此请求ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 关闭此会话ToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

