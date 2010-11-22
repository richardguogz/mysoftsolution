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
            if (wbChatBox != null) wbChatBox.Dispose();
            if (msgtimer != null) msgtimer.Dispose();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupChat));
            System.Windows.Forms.ToolStripButton toolStripButton4;
            this.lblGroupInfo = new System.Windows.Forms.Label();
            this.plChat = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnSendMessage = new System.Windows.Forms.Button();
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listSeats = new LiveChat.Client.DoubleBufferListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.tsbAcceptTalk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChatMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            tsbExit = new System.Windows.Forms.ToolStripButton();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.plChat.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tsChatTools.SuspendLayout();
            this.panel3.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            // 
            // toolStripButton4
            // 
            toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            toolStripButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(60, 53);
            toolStripButton4.Text = "关闭窗口";
            toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
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
            this.lblGroupInfo.Text = "群聊天（1/10）";
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.emotionDropdown1);
            this.panel1.Location = new System.Drawing.Point(1, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 256);
            this.panel1.TabIndex = 13;
            this.panel1.Visible = false;
            // 
            // emotionDropdown1
            // 
            this.emotionDropdown1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emotionDropdown1.Location = new System.Drawing.Point(0, 0);
            this.emotionDropdown1.MaximumSize = new System.Drawing.Size(401, 291);
            this.emotionDropdown1.MinimumSize = new System.Drawing.Size(401, 291);
            this.emotionDropdown1.Name = "emotionDropdown1";
            this.emotionDropdown1.Size = new System.Drawing.Size(401, 291);
            this.emotionDropdown1.TabIndex = 0;
            // 
            // wbChatBox
            // 
            this.wbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbChatBox.Location = new System.Drawing.Point(0, 0);
            this.wbChatBox.Margin = new System.Windows.Forms.Padding(5);
            this.wbChatBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChatBox.Name = "wbChatBox";
            this.wbChatBox.Size = new System.Drawing.Size(466, 266);
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
            this.panel2.Location = new System.Drawing.Point(0, 266);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(466, 100);
            this.panel2.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(378, 33);
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
            this.txtMessage.Size = new System.Drawing.Size(364, 66);
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
            this.tsChatTools.Size = new System.Drawing.Size(464, 25);
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
            this.toolStripButton2.Image = global::LiveChat.Client.Properties.Resources._0;
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 56);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.wbChatBox);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(644, 366);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listSeats);
            this.splitContainer2.Size = new System.Drawing.Size(174, 366);
            this.splitContainer2.SplitterDistance = 123;
            this.splitContainer2.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(0, 23);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 100);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "群动态";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listSeats
            // 
            this.listSeats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7});
            this.listSeats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSeats.FullRowSelect = true;
            this.listSeats.GridLines = true;
            this.listSeats.HideSelection = false;
            this.listSeats.Location = new System.Drawing.Point(0, 0);
            this.listSeats.MultiSelect = false;
            this.listSeats.Name = "listSeats";
            this.listSeats.Size = new System.Drawing.Size(174, 239);
            this.listSeats.SmallImageList = this.imgList;
            this.listSeats.TabIndex = 5;
            this.listSeats.UseCompatibleStateImageBehavior = false;
            this.listSeats.View = System.Windows.Forms.View.Details;
            this.listSeats.DoubleClick += new System.EventHandler(this.listSeats_DoubleClick);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "群信息 (1/1)";
            this.columnHeader7.Width = 150;
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
            // tsbAcceptTalk
            // 
            this.tsbAcceptTalk.Image = global::LiveChat.Client.Properties.Resources.tool2;
            this.tsbAcceptTalk.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAcceptTalk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAcceptTalk.Name = "tsbAcceptTalk";
            this.tsbAcceptTalk.Size = new System.Drawing.Size(60, 53);
            this.tsbAcceptTalk.Text = "好友资料";
            this.tsbAcceptTalk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
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
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton5,
            this.toolStripSeparator3,
            toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(644, 56);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::LiveChat.Client.Properties.Resources.tool2;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(48, 53);
            this.toolStripButton1.Text = "群资料";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 56);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::LiveChat.Client.Properties.Resources.tool6;
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(60, 53);
            this.toolStripButton3.Text = "退出该群";
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::LiveChat.Client.Properties.Resources.tool6;
            this.toolStripButton5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(60, 53);
            this.toolStripButton5.Text = "解散该群";
            this.toolStripButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 56);
            // 
            // frmGroupChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 422);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGroupChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "群会话";
            this.Load += new System.EventHandler(this.frmGroupChat_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGroupChat_FormClosing);
            this.plChat.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tsChatTools.ResumeLayout(false);
            this.tsChatTools.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGroupInfo;
        private System.Windows.Forms.Panel plChat;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSendMessage;
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
        private DoubleBufferListView listSeats;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripButton tsbAcceptTalk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbChatMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ImageList imgList;
    }
}