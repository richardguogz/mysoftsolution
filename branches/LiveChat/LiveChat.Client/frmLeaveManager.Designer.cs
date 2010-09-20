namespace LiveChat.Client
{
    partial class frmLeaveManager
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
            System.Windows.Forms.ToolStripButton tsbExit;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLeaveManager));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnQuery = new System.Windows.Forms.Button();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLeaves = new System.Windows.Forms.DataGridView();
            this.AddTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastTime1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaves)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbExit
            // 
            tsbExit.Image = global::LiveChat.Client.Properties.Resources.endsession;
            tsbExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbExit.Name = "tsbExit";
            tsbExit.Size = new System.Drawing.Size(61, 27);
            tsbExit.Text = "关闭";
            tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripSeparator1,
            tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 30);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::LiveChat.Client.Properties.Resources.last;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(64, 27);
            this.toolStripButton2.Text = "上一页";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = global::LiveChat.Client.Properties.Resources.next;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(64, 27);
            this.toolStripButton4.Text = "下一页";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 502);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 9;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(74, 17);
            this.toolStripStatusLabel1.Text = "准备状态......";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.dtpEndTime);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpStartTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 42);
            this.panel1.TabIndex = 11;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(716, 9);
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
            this.dtpEndTime.Location = new System.Drawing.Point(377, 9);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(200, 21);
            this.dtpEndTime.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "开始时间：";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "yyyy年MM月dd日 HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(75, 9);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(200, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始时间：";
            // 
            // dgvLeaves
            // 
            this.dgvLeaves.AllowUserToAddRows = false;
            this.dgvLeaves.AllowUserToDeleteRows = false;
            this.dgvLeaves.AllowUserToResizeColumns = false;
            this.dgvLeaves.AllowUserToResizeRows = false;
            this.dgvLeaves.BackgroundColor = System.Drawing.Color.White;
            this.dgvLeaves.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLeaves.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AddTime,
            this.UserName1,
            this.RequestSeat,
            this.RequestTime,
            this.Message1,
            this.LastTime1});
            this.dgvLeaves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLeaves.GridColor = System.Drawing.SystemColors.Control;
            this.dgvLeaves.Location = new System.Drawing.Point(0, 0);
            this.dgvLeaves.MultiSelect = false;
            this.dgvLeaves.Name = "dgvLeaves";
            this.dgvLeaves.RowHeadersVisible = false;
            this.dgvLeaves.RowTemplate.Height = 23;
            this.dgvLeaves.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLeaves.Size = new System.Drawing.Size(520, 430);
            this.dgvLeaves.TabIndex = 12;
            this.dgvLeaves.Click += new System.EventHandler(this.dgvLeaves_Click);
            // 
            // AddTime
            // 
            this.AddTime.DataPropertyName = "AddTime";
            this.AddTime.HeaderText = "留言时间";
            this.AddTime.Name = "AddTime";
            this.AddTime.ReadOnly = true;
            // 
            // UserName1
            // 
            this.UserName1.DataPropertyName = "Name";
            this.UserName1.HeaderText = "姓名";
            this.UserName1.Name = "UserName1";
            this.UserName1.ReadOnly = true;
            this.UserName1.Width = 80;
            // 
            // RequestSeat
            // 
            this.RequestSeat.DataPropertyName = "Title";
            this.RequestSeat.HeaderText = "留言主题";
            this.RequestSeat.Name = "RequestSeat";
            this.RequestSeat.ReadOnly = true;
            // 
            // RequestTime
            // 
            this.RequestTime.DataPropertyName = "Telephone";
            this.RequestTime.HeaderText = "电话";
            this.RequestTime.Name = "RequestTime";
            this.RequestTime.ReadOnly = true;
            this.RequestTime.Width = 80;
            // 
            // Message1
            // 
            this.Message1.DataPropertyName = "Email";
            this.Message1.HeaderText = "邮箱";
            this.Message1.Name = "Message1";
            this.Message1.ReadOnly = true;
            // 
            // LastTime1
            // 
            this.LastTime1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LastTime1.DataPropertyName = "PostArea";
            this.LastTime1.HeaderText = "地址";
            this.LastTime1.Name = "LastTime1";
            this.LastTime1.ReadOnly = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 72);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvLeaves);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(818, 430);
            this.splitContainer1.SplitterDistance = 520;
            this.splitContainer1.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(294, 406);
            this.textBox1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightYellow;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(294, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "留言内容：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmLeaveManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 524);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLeaveManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "留言管理";
            this.Load += new System.EventHandler(this.frmLeaveManager_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLeaveManager_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeaves)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridView dgvLeaves;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestSeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastTime1;
    }
}