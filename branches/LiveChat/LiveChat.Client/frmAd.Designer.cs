namespace LiveChat.Client
{
    partial class frmAd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAd));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAdName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAdTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAdUrl = new System.Windows.Forms.TextBox();
            this.cboAdArea = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAdImgUrl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboProvince = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAdLogoUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAdLogoImgUrl = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAdText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAdTextUrl = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.chkCommon = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(299, 366);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "关闭(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Location = new System.Drawing.Point(209, 366);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "广告名称：";
            // 
            // txtAdName
            // 
            this.txtAdName.Location = new System.Drawing.Point(132, 38);
            this.txtAdName.Name = "txtAdName";
            this.txtAdName.Size = new System.Drawing.Size(396, 21);
            this.txtAdName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "广告标题：";
            // 
            // txtAdTitle
            // 
            this.txtAdTitle.Location = new System.Drawing.Point(132, 68);
            this.txtAdTitle.Name = "txtAdTitle";
            this.txtAdTitle.Size = new System.Drawing.Size(396, 21);
            this.txtAdTitle.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "投放区域：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "顶部广告URL：";
            // 
            // txtAdUrl
            // 
            this.txtAdUrl.Location = new System.Drawing.Point(133, 175);
            this.txtAdUrl.Name = "txtAdUrl";
            this.txtAdUrl.Size = new System.Drawing.Size(396, 21);
            this.txtAdUrl.TabIndex = 5;
            // 
            // cboAdArea
            // 
            this.cboAdArea.FormattingEnabled = true;
            this.cboAdArea.Location = new System.Drawing.Point(328, 98);
            this.cboAdArea.Name = "cboAdArea";
            this.cboAdArea.Size = new System.Drawing.Size(200, 20);
            this.cboAdArea.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "顶部广告图片：";
            // 
            // txtAdImgUrl
            // 
            this.txtAdImgUrl.Location = new System.Drawing.Point(132, 127);
            this.txtAdImgUrl.Name = "txtAdImgUrl";
            this.txtAdImgUrl.Size = new System.Drawing.Size(397, 21);
            this.txtAdImgUrl.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(130, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 12);
            this.label9.TabIndex = 11;
            this.label9.Text = "广告图片大小为 650 * 50";
            // 
            // cboProvince
            // 
            this.cboProvince.FormattingEnabled = true;
            this.cboProvince.Location = new System.Drawing.Point(133, 98);
            this.cboProvince.Name = "cboProvince";
            this.cboProvince.Size = new System.Drawing.Size(189, 20);
            this.cboProvince.TabIndex = 2;
            this.cboProvince.SelectedIndexChanged += new System.EventHandler(this.cboProvince_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "右边广告URL：";
            // 
            // txtAdLogoUrl
            // 
            this.txtAdLogoUrl.Location = new System.Drawing.Point(132, 255);
            this.txtAdLogoUrl.Name = "txtAdLogoUrl";
            this.txtAdLogoUrl.Size = new System.Drawing.Size(396, 21);
            this.txtAdLogoUrl.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "右边广告图片：";
            // 
            // txtAdLogoImgUrl
            // 
            this.txtAdLogoImgUrl.Location = new System.Drawing.Point(131, 205);
            this.txtAdLogoImgUrl.Name = "txtAdLogoImgUrl";
            this.txtAdLogoImgUrl.Size = new System.Drawing.Size(397, 21);
            this.txtAdLogoImgUrl.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(129, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "广告图片大小为 150 * 200";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(44, 287);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "底部广告文字：";
            // 
            // txtAdText
            // 
            this.txtAdText.Location = new System.Drawing.Point(132, 283);
            this.txtAdText.Name = "txtAdText";
            this.txtAdText.Size = new System.Drawing.Size(396, 21);
            this.txtAdText.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 316);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "底部广告URL：";
            // 
            // txtAdTextUrl
            // 
            this.txtAdTextUrl.Location = new System.Drawing.Point(132, 312);
            this.txtAdTextUrl.Name = "txtAdTextUrl";
            this.txtAdTextUrl.Size = new System.Drawing.Size(396, 21);
            this.txtAdTextUrl.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(130, 336);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(155, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "多个文字广告中间用 | 分隔";
            // 
            // chkDefault
            // 
            this.chkDefault.AutoSize = true;
            this.chkDefault.Location = new System.Drawing.Point(131, 13);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(144, 16);
            this.chkDefault.TabIndex = 12;
            this.chkDefault.Text = "将当前广告设置为默认";
            this.chkDefault.UseVisualStyleBackColor = true;
            // 
            // chkCommon
            // 
            this.chkCommon.AutoSize = true;
            this.chkCommon.Location = new System.Drawing.Point(314, 13);
            this.chkCommon.Name = "chkCommon";
            this.chkCommon.Size = new System.Drawing.Size(144, 16);
            this.chkCommon.TabIndex = 12;
            this.chkCommon.Text = "将当前广告设置为公共";
            this.chkCommon.UseVisualStyleBackColor = true;
            // 
            // frmAd
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(585, 414);
            this.Controls.Add(this.chkCommon);
            this.Controls.Add(this.chkDefault);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboProvince);
            this.Controls.Add(this.cboAdArea);
            this.Controls.Add(this.txtAdLogoImgUrl);
            this.Controls.Add(this.txtAdImgUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAdTextUrl);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtAdText);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAdLogoUrl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtAdUrl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAdTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAdName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(601, 452);
            this.MinimumSize = new System.Drawing.Size(601, 452);
            this.Name = "frmAd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "广告管理";
            this.Load += new System.EventHandler(this.frmAdManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAdName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAdTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAdUrl;
        private System.Windows.Forms.ComboBox cboAdArea;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAdImgUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboProvince;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAdLogoUrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAdLogoImgUrl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAdText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAdTextUrl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkDefault;
        private System.Windows.Forms.CheckBox chkCommon;
    }
}