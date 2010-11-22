namespace LiveChat.Client
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtCompanyID = new System.Windows.Forms.TextBox();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.皮肤切换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OFFICE2007ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mP10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pAGEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wINXPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rOYALEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.网络设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.在线升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于我们ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "公司ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "客服ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "密  码";
            // 
            // txtClientID
            // 
            this.txtClientID.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtClientID.Location = new System.Drawing.Point(75, 145);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(134, 21);
            this.txtClientID.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtPassword.Location = new System.Drawing.Point(75, 179);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtPassword.Size = new System.Drawing.Size(134, 21);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(121, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "退出(&E)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.Location = new System.Drawing.Point(40, 286);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登录(&L)";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtCompanyID
            // 
            this.txtCompanyID.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtCompanyID.Location = new System.Drawing.Point(75, 111);
            this.txtCompanyID.Name = "txtCompanyID";
            this.txtCompanyID.Size = new System.Drawing.Size(134, 21);
            this.txtCompanyID.TabIndex = 0;
            // 
            // skinEngine1
            // 
            this.skinEngine1.MenuFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = "";
            this.skinEngine1.TitleFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(75, 216);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "记住密码";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(75, 243);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "自动登录";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(29, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 62);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统菜单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 547);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(239, 25);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统菜单ToolStripMenuItem
            // 
            this.系统菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.皮肤切换ToolStripMenuItem,
            this.网络设置ToolStripMenuItem,
            this.toolStripSeparator1,
            this.在线升级ToolStripMenuItem,
            this.关于我们ToolStripMenuItem,
            this.toolStripSeparator2,
            this.退出系统ToolStripMenuItem});
            this.系统菜单ToolStripMenuItem.Name = "系统菜单ToolStripMenuItem";
            this.系统菜单ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.系统菜单ToolStripMenuItem.Text = "系统菜单";
            // 
            // 皮肤切换ToolStripMenuItem
            // 
            this.皮肤切换ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OFFICE2007ToolStripMenuItem,
            this.mP10ToolStripMenuItem,
            this.pAGEToolStripMenuItem,
            this.mSNToolStripMenuItem,
            this.wINXPToolStripMenuItem,
            this.rOYALEToolStripMenuItem,
            this.dEEPToolStripMenuItem});
            this.皮肤切换ToolStripMenuItem.Name = "皮肤切换ToolStripMenuItem";
            this.皮肤切换ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.皮肤切换ToolStripMenuItem.Text = "皮肤切换";
            // 
            // OFFICE2007ToolStripMenuItem
            // 
            this.OFFICE2007ToolStripMenuItem.Name = "OFFICE2007ToolStripMenuItem";
            this.OFFICE2007ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.OFFICE2007ToolStripMenuItem.Tag = "office2007";
            this.OFFICE2007ToolStripMenuItem.Text = "OFFICE2007";
            this.OFFICE2007ToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // mP10ToolStripMenuItem
            // 
            this.mP10ToolStripMenuItem.Name = "mP10ToolStripMenuItem";
            this.mP10ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.mP10ToolStripMenuItem.Tag = "mp10";
            this.mP10ToolStripMenuItem.Text = "MP10";
            this.mP10ToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // pAGEToolStripMenuItem
            // 
            this.pAGEToolStripMenuItem.Name = "pAGEToolStripMenuItem";
            this.pAGEToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.pAGEToolStripMenuItem.Tag = "page";
            this.pAGEToolStripMenuItem.Text = "PAGE";
            this.pAGEToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // mSNToolStripMenuItem
            // 
            this.mSNToolStripMenuItem.Name = "mSNToolStripMenuItem";
            this.mSNToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.mSNToolStripMenuItem.Tag = "msn";
            this.mSNToolStripMenuItem.Text = "MSN";
            this.mSNToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // wINXPToolStripMenuItem
            // 
            this.wINXPToolStripMenuItem.Name = "wINXPToolStripMenuItem";
            this.wINXPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.wINXPToolStripMenuItem.Tag = "winxp";
            this.wINXPToolStripMenuItem.Text = "WINXP";
            this.wINXPToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // rOYALEToolStripMenuItem
            // 
            this.rOYALEToolStripMenuItem.Name = "rOYALEToolStripMenuItem";
            this.rOYALEToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.rOYALEToolStripMenuItem.Tag = "royale";
            this.rOYALEToolStripMenuItem.Text = "ROYALE";
            this.rOYALEToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // dEEPToolStripMenuItem
            // 
            this.dEEPToolStripMenuItem.Name = "dEEPToolStripMenuItem";
            this.dEEPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.dEEPToolStripMenuItem.Tag = "deep";
            this.dEEPToolStripMenuItem.Text = "DEEP";
            this.dEEPToolStripMenuItem.Click += new System.EventHandler(this.StyleToolStripMenuItem_Click);
            // 
            // 网络设置ToolStripMenuItem
            // 
            this.网络设置ToolStripMenuItem.Name = "网络设置ToolStripMenuItem";
            this.网络设置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.网络设置ToolStripMenuItem.Text = "网络设置";
            this.网络设置ToolStripMenuItem.Click += new System.EventHandler(this.网络设置ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // 在线升级ToolStripMenuItem
            // 
            this.在线升级ToolStripMenuItem.Name = "在线升级ToolStripMenuItem";
            this.在线升级ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.在线升级ToolStripMenuItem.Text = "在线升级";
            this.在线升级ToolStripMenuItem.Click += new System.EventHandler(this.在线升级ToolStripMenuItem_Click);
            // 
            // 关于我们ToolStripMenuItem
            // 
            this.关于我们ToolStripMenuItem.Name = "关于我们ToolStripMenuItem";
            this.关于我们ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.关于我们ToolStripMenuItem.Text = "关于我们";
            this.关于我们ToolStripMenuItem.Click += new System.EventHandler(this.关于我们ToolStripMenuItem_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(36, 482);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(173, 12);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "http://www.globalfabric.com/reg.asp";
            this.linkLabel1.Text = "www.globalfabric.com/reg.asp";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 460);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "企业注册";
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.退出系统ToolStripMenuItem.Text = "退出系统";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(239, 572);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txtCompanyID);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtClientID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(245, 600);
            this.MinimumSize = new System.Drawing.Size(245, 600);
            this.Name = "frmLogin";
            this.Text = "登录";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClientID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtCompanyID;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 网络设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于我们ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 皮肤切换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem OFFICE2007ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mP10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pAGEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wINXPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rOYALEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dEEPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 在线升级ToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
    }
}