namespace LiveChat.Client
{
    partial class frmGroupChat
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
            wbChatBox.Dispose();
            msgtimer.Dispose();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupChat));
            this.lblGroupInfo = new System.Windows.Forms.Label();
            this.plChat = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.emotionDropdown1 = new LiveChat.Client.EmotionDropdown();
            this.wbChatBox = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tsChatTools = new System.Windows.Forms.ToolStrip();
            this.tsbFont = new System.Windows.Forms.ToolStripButton();
            this.tsbColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbCaptrue = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbScreenCaptrue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHideScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.按Control键发送消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按ControlEnter键发送消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listSeats = new LiveChat.Client.DoubleBufferListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.plChat.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tsChatTools.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblGroupInfo
            // 
            this.lblGroupInfo.BackColor = System.Drawing.Color.LightYellow;
            this.lblGroupInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGroupInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGroupInfo.Location = new System.Drawing.Point(3, 3);
            this.lblGroupInfo.Name = "lblGroupInfo";
            this.lblGroupInfo.Size = new System.Drawing.Size(435, 24);
            this.lblGroupInfo.TabIndex = 10;
            this.lblGroupInfo.Text = "客服群聊天（1/10）";
            this.lblGroupInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // plChat
            // 
            this.plChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plChat.Controls.Add(this.btnSend);
            this.plChat.Controls.Add(this.panel7);
            this.plChat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plChat.Location = new System.Drawing.Point(3, 288);
            this.plChat.Name = "plChat";
            this.plChat.Size = new System.Drawing.Size(435, 124);
            this.plChat.TabIndex = 8;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(347, 33);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(77, 83);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnSendMessage);
            this.panel7.Location = new System.Drawing.Point(215, 307);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(88, 122);
            this.panel7.TabIndex = 8;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMessage.Enabled = false;
            this.btnSendMessage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendMessage.Location = new System.Drawing.Point(5, 0);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(78, 122);
            this.btnSendMessage.TabIndex = 0;
            this.btnSendMessage.Text = "发送";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(120, 24);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 412);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.wbChatBox);
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(576, 380);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "客服群聊天（1/10）";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.emotionDropdown1);
            this.panel1.Location = new System.Drawing.Point(2, 189);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 87);
            this.panel1.TabIndex = 13;
            this.panel1.Visible = false;
            // 
            // emotionDropdown1
            // 
            this.emotionDropdown1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emotionDropdown1.Location = new System.Drawing.Point(0, 0);
            this.emotionDropdown1.MaximumSize = new System.Drawing.Size(226, 87);
            this.emotionDropdown1.MinimumSize = new System.Drawing.Size(226, 87);
            this.emotionDropdown1.Name = "emotionDropdown1";
            this.emotionDropdown1.Size = new System.Drawing.Size(226, 87);
            this.emotionDropdown1.TabIndex = 0;
            // 
            // wbChatBox
            // 
            this.wbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatBox.Location = new System.Drawing.Point(3, 3);
            this.wbChatBox.Margin = new System.Windows.Forms.Padding(5);
            this.wbChatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatBox.Name = "wbChatBox";
            this.wbChatBox.Size = new System.Drawing.Size(570, 274);
            this.wbChatBox.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.txtMessage);
            this.panel2.Controls.Add(this.tsChatTools);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 277);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(570, 100);
            this.panel2.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(482, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 59);
            this.button1.TabIndex = 1;
            this.button1.Text = "发送(&S)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(4, 28);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(468, 66);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Click += new System.EventHandler(this.txtMessage_Click);
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // tsChatTools
            // 
            this.tsChatTools.BackColor = System.Drawing.Color.White;
            this.tsChatTools.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsChatTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsChatTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFont,
            this.tsbColor,
            this.toolStripSeparator14,
            this.toolStripButton2,
            this.tsbCaptrue,
            this.toolStripDropDownButton1});
            this.tsChatTools.Location = new System.Drawing.Point(0, 0);
            this.tsChatTools.Name = "tsChatTools";
            this.tsChatTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsChatTools.Size = new System.Drawing.Size(568, 25);
            this.tsChatTools.TabIndex = 9;
            this.tsChatTools.Text = "聊天工具箱";
            // 
            // tsbFont
            // 
            this.tsbFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFont.Image = global::LiveChat.Client.Properties.Resources.字体格式;
            this.tsbFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFont.Name = "tsbFont";
            this.tsbFont.Size = new System.Drawing.Size(23, 22);
            this.tsbFont.Text = "文本格式";
            this.tsbFont.Click += new System.EventHandler(this.tsbFont_Click);
            // 
            // tsbColor
            // 
            this.tsbColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbColor.Image = global::LiveChat.Client.Properties.Resources.字体颜色;
            this.tsbColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbColor.Name = "tsbColor";
            this.tsbColor.Size = new System.Drawing.Size(23, 22);
            this.tsbColor.Text = "字体颜色";
            this.tsbColor.Click += new System.EventHandler(this.tsbColor_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::LiveChat.Client.Properties.Resources.face1;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton2.Text = "表情";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // tsbCaptrue
            // 
            this.tsbCaptrue.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbScreenCaptrue,
            this.toolStripSeparator9,
            this.tsbHideScreen});
            this.tsbCaptrue.Image = global::LiveChat.Client.Properties.Resources.截屏;
            this.tsbCaptrue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCaptrue.Name = "tsbCaptrue";
            this.tsbCaptrue.Size = new System.Drawing.Size(62, 22);
            this.tsbCaptrue.Text = "截屏";
            // 
            // tsbScreenCaptrue
            // 
            this.tsbScreenCaptrue.Name = "tsbScreenCaptrue";
            this.tsbScreenCaptrue.Size = new System.Drawing.Size(191, 22);
            this.tsbScreenCaptrue.Text = "屏幕截图";
            this.tsbScreenCaptrue.Click += new System.EventHandler(this.tsbScreenCaptrue_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(188, 6);
            // 
            // tsbHideScreen
            // 
            this.tsbHideScreen.Checked = true;
            this.tsbHideScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbHideScreen.Name = "tsbHideScreen";
            this.tsbHideScreen.Size = new System.Drawing.Size(191, 22);
            this.tsbHideScreen.Text = "截图时隐藏当前窗口";
            this.tsbHideScreen.Click += new System.EventHandler(this.tsbHideScreen_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.按Control键发送消息ToolStripMenuItem,
            this.按ControlEnter键发送消息ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::LiveChat.Client.Properties.Resources.quickrep;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(114, 22);
            this.toolStripDropDownButton1.Text = "消息发送方式";
            // 
            // 按Control键发送消息ToolStripMenuItem
            // 
            this.按Control键发送消息ToolStripMenuItem.Checked = true;
            this.按Control键发送消息ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.按Control键发送消息ToolStripMenuItem.Name = "按Control键发送消息ToolStripMenuItem";
            this.按Control键发送消息ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.按Control键发送消息ToolStripMenuItem.Text = "按 Control 键发送消息";
            this.按Control键发送消息ToolStripMenuItem.Click += new System.EventHandler(this.按Control键发送消息ToolStripMenuItem_Click);
            // 
            // 按ControlEnter键发送消息ToolStripMenuItem
            // 
            this.按ControlEnter键发送消息ToolStripMenuItem.Name = "按ControlEnter键发送消息ToolStripMenuItem";
            this.按ControlEnter键发送消息ToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.按ControlEnter键发送消息ToolStripMenuItem.Text = "按 Control+Enter 键发送消息";
            this.按ControlEnter键发送消息ToolStripMenuItem.Click += new System.EventHandler(this.按ControlEnter键发送消息ToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button2);
            this.panel3.Location = new System.Drawing.Point(215, 307);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(88, 122);
            this.panel3.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(5, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 122);
            this.button2.TabIndex = 0;
            this.button2.Text = "发送";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listSeats);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(576, 380);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "群成员";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listSeats
            // 
            this.listSeats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.listSeats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSeats.FullRowSelect = true;
            this.listSeats.GridLines = true;
            this.listSeats.HideSelection = false;
            this.listSeats.Location = new System.Drawing.Point(0, 0);
            this.listSeats.MultiSelect = false;
            this.listSeats.Name = "listSeats";
            this.listSeats.Size = new System.Drawing.Size(576, 380);
            this.listSeats.TabIndex = 5;
            this.listSeats.UseCompatibleStateImageBehavior = false;
            this.listSeats.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "状态";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "客服代码";
            this.columnHeader8.Width = 80;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "客服名称";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "电话";
            this.columnHeader10.Width = 120;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "手机";
            this.columnHeader11.Width = 100;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Email";
            this.columnHeader12.Width = 150;
            // 
            // frmGroupChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGroupChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客服群会话";
            this.Load += new System.EventHandler(this.frmGroupChat_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGroupChat_FormClosing);
            this.plChat.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tsChatTools.ResumeLayout(false);
            this.tsChatTools.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblGroupInfo;
        private System.Windows.Forms.Panel plChat;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private EmotionDropdown emotionDropdown1;
        private System.Windows.Forms.WebBrowser wbChatBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ToolStrip tsChatTools;
        private System.Windows.Forms.ToolStripButton tsbFont;
        private System.Windows.Forms.ToolStripButton tsbColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton tsbCaptrue;
        private System.Windows.Forms.ToolStripMenuItem tsbScreenCaptrue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsbHideScreen;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 按Control键发送消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按ControlEnter键发送消息ToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TabPage tabPage1;
        private DoubleBufferListView listSeats;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
    }
}