namespace LiveChat.Client
{
    partial class frmChat
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
            System.Windows.Forms.ToolStripButton tsbExit;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChat));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.emotionDropdown1 = new LiveChat.Client.EmotionDropdown();
            this.wbChatBox = new System.Windows.Forms.WebBrowser();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAcceptTalk = new System.Windows.Forms.ToolStripButton();
            this.tsbEndTalk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChatMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.splitter1 = new System.Windows.Forms.Splitter();
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
            this.tsReply = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsUrl = new System.Windows.Forms.ToolStripDropDownButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.plChat.SuspendLayout();
            this.tsChatTools.SuspendLayout();
            this.panel7.SuspendLayout();
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
            // panel1
            // 
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 412);
            this.panel1.TabIndex = 3;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.panel12);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(584, 412);
            this.panel9.TabIndex = 2;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel3);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(582, 410);
            this.panel12.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.wbChatBox);
            this.panel3.Controls.Add(this.toolStrip1);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.plChat);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(582, 410);
            this.panel3.TabIndex = 0;
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
            this.panel2.Location = new System.Drawing.Point(2, 235);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 70);
            this.panel2.TabIndex = 11;
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
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.emotionDropdown1);
            this.panel4.Location = new System.Drawing.Point(1, 218);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(226, 87);
            this.panel4.TabIndex = 12;
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
            // wbChatBox
            // 
            this.wbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatBox.Location = new System.Drawing.Point(0, 56);
            this.wbChatBox.Margin = new System.Windows.Forms.Padding(5);
            this.wbChatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatBox.Name = "wbChatBox";
            this.wbChatBox.Size = new System.Drawing.Size(582, 251);
            this.wbChatBox.TabIndex = 7;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAcceptTalk,
            this.tsbEndTalk,
            this.toolStripSeparator2,
            this.tsbChatMessage,
            this.toolStripSeparator5,
            tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(582, 56);
            this.toolStrip1.TabIndex = 10;
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
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 307);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(582, 3);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // plChat
            // 
            this.plChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plChat.Controls.Add(this.btnSend);
            this.plChat.Controls.Add(this.txtMessage);
            this.plChat.Controls.Add(this.tsChatTools);
            this.plChat.Controls.Add(this.panel7);
            this.plChat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plChat.Location = new System.Drawing.Point(0, 310);
            this.plChat.Name = "plChat";
            this.plChat.Size = new System.Drawing.Size(582, 100);
            this.plChat.TabIndex = 7;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(494, 33);
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
            this.txtMessage.Size = new System.Drawing.Size(480, 66);
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
            this.toolStripDropDownButton1,
            this.tsReply,
            this.tsUrl});
            this.tsChatTools.Location = new System.Drawing.Point(0, 0);
            this.tsChatTools.Name = "tsChatTools";
            this.tsChatTools.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsChatTools.Size = new System.Drawing.Size(580, 25);
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
            // tsReply
            // 
            this.tsReply.Image = global::LiveChat.Client.Properties.Resources.status;
            this.tsReply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsReply.Name = "tsReply";
            this.tsReply.Size = new System.Drawing.Size(88, 22);
            this.tsReply.Text = "快速回复";
            // 
            // tsUrl
            // 
            this.tsUrl.Image = global::LiveChat.Client.Properties.Resources.status;
            this.tsUrl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUrl.Name = "tsUrl";
            this.tsUrl.Size = new System.Drawing.Size(88, 22);
            this.tsUrl.Text = "快速推送";
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正在与 xxx 聊天中...";
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmChat_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.plChat.ResumeLayout(false);
            this.plChat.PerformLayout();
            this.tsChatTools.ResumeLayout(false);
            this.tsChatTools.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel plChat;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.WebBrowser wbChatBox;
        private System.Windows.Forms.ToolStrip tsChatTools;
        private System.Windows.Forms.ToolStripButton tsbFont;
        private System.Windows.Forms.ToolStripButton tsbColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripDropDownButton tsbCaptrue;
        private System.Windows.Forms.ToolStripMenuItem tsbScreenCaptrue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsbHideScreen;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAcceptTalk;
        private System.Windows.Forms.ToolStripButton tsbEndTalk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel panel4;
        private EmotionDropdown emotionDropdown1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 按Control键发送消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按ControlEnter键发送消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbChatMessage;
        private System.Windows.Forms.ToolStripDropDownButton tsReply;
        private System.Windows.Forms.ToolStripDropDownButton tsUrl;
    }
}