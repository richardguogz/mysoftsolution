namespace LiveChat.Client
{
    partial class frmSeatChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeatChat));
            this.wbChatBox = new System.Windows.Forms.WebBrowser();
            this.plChat = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tsChatTools = new System.Windows.Forms.ToolStrip();
            this.tsbFont = new System.Windows.Forms.ToolStripButton();
            this.tsbColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCaptrue = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbScreenCaptrue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHideScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.按Control键发送消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按ControlEnter键发送消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.emotionDropdown1 = new LiveChat.Client.EmotionDropdown();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAcceptTalk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChatMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            this.plChat.SuspendLayout();
            this.tsChatTools.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            // wbChatBox
            // 
            this.wbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatBox.Location = new System.Drawing.Point(0, 56);
            this.wbChatBox.Margin = new System.Windows.Forms.Padding(5);
            this.wbChatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatBox.Name = "wbChatBox";
            this.wbChatBox.Size = new System.Drawing.Size(584, 256);
            this.wbChatBox.TabIndex = 13;
            // 
            // plChat
            // 
            this.plChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plChat.Controls.Add(this.btnSend);
            this.plChat.Controls.Add(this.txtMessage);
            this.plChat.Controls.Add(this.tsChatTools);
            this.plChat.Controls.Add(this.panel7);
            this.plChat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plChat.Location = new System.Drawing.Point(0, 312);
            this.plChat.Name = "plChat";
            this.plChat.Size = new System.Drawing.Size(584, 100);
            this.plChat.TabIndex = 12;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(496, 33);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(77, 59);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送(&S)";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(4, 28);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(482, 66);
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
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripButton1,
            this.toolStripSeparator14,
            this.tsbCaptrue,
            this.toolStripDropDownButton1});
            this.tsChatTools.Location = new System.Drawing.Point(0, 0);
            this.tsChatTools.Name = "tsChatTools";
            this.tsChatTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsChatTools.Size = new System.Drawing.Size(582, 25);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::LiveChat.Client.Properties.Resources.export;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(79, 22);
            this.toolStripButton1.Text = "上传文件";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
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
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.emotionDropdown1);
            this.panel4.Location = new System.Drawing.Point(1, 224);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(226, 87);
            this.panel4.TabIndex = 15;
            this.panel4.Visible = false;
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
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.txtFile);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(1, 241);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 70);
            this.panel2.TabIndex = 16;
            this.panel2.Visible = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(321, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(59, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "选择";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.BackColor = System.Drawing.Color.White;
            this.txtFile.Enabled = false;
            this.txtFile.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFile.Location = new System.Drawing.Point(22, 34);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(293, 21);
            this.txtFile.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(386, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(46, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "传送";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(438, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件大小限制在4M以内！";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAcceptTalk,
            this.toolStripSeparator2,
            this.tsbChatMessage,
            this.toolStripSeparator5,
            tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(584, 56);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAcceptTalk
            // 
            this.tsbAcceptTalk.Image = global::LiveChat.Client.Properties.Resources.tool2;
            this.tsbAcceptTalk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAcceptTalk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAcceptTalk.Name = "tsbAcceptTalk";
            this.tsbAcceptTalk.Size = new System.Drawing.Size(60, 53);
            this.tsbAcceptTalk.Text = "好友资料";
            this.tsbAcceptTalk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAcceptTalk.Click += new System.EventHandler(this.tsbAcceptTalk_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 56);
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
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 56);
            // 
            // frmSeatChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.wbChatBox);
            this.Controls.Add(this.plChat);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSeatChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客服与客服会话";
            this.Load += new System.EventHandler(this.frmSeatChat_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSeatChat_FormClosing);
            this.plChat.ResumeLayout(false);
            this.plChat.PerformLayout();
            this.tsChatTools.ResumeLayout(false);
            this.tsChatTools.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbChatBox;
        private System.Windows.Forms.Panel plChat;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ToolStrip tsChatTools;
        private System.Windows.Forms.ToolStripButton tsbFont;
        private System.Windows.Forms.ToolStripButton tsbColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripDropDownButton tsbCaptrue;
        private System.Windows.Forms.ToolStripMenuItem tsbScreenCaptrue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsbHideScreen;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 按Control键发送消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按ControlEnter键发送消息ToolStripMenuItem;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Panel panel4;
        private EmotionDropdown emotionDropdown1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbChatMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbAcceptTalk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}