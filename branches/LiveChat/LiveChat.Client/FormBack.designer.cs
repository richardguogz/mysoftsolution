namespace LiveChat.Client
{
    partial class FormBack
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
            this.panel_Info = new System.Windows.Forms.Panel();
            this.label_CatcherHeight = new System.Windows.Forms.Label();
            this.label_CatcherWidth = new System.Windows.Forms.Label();
            this.label_CatcherLoc = new System.Windows.Forms.Label();
            this.label_Info = new System.Windows.Forms.Label();
            this.panel_Info.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Info
            // 
            this.panel_Info.BackgroundImage = global::LiveChat.Client.Properties.Resources.qq;
            this.panel_Info.Controls.Add(this.label_CatcherHeight);
            this.panel_Info.Controls.Add(this.label_CatcherWidth);
            this.panel_Info.Controls.Add(this.label_CatcherLoc);
            this.panel_Info.Controls.Add(this.label_Info);
            this.panel_Info.Location = new System.Drawing.Point(12, 12);
            this.panel_Info.Name = "panel_Info";
            this.panel_Info.Size = new System.Drawing.Size(174, 147);
            this.panel_Info.TabIndex = 1;
            this.panel_Info.MouseEnter += new System.EventHandler(this.panel_Info_MouseEnter);
            // 
            // label_CatcherHeight
            // 
            this.label_CatcherHeight.BackColor = System.Drawing.Color.Transparent;
            this.label_CatcherHeight.Location = new System.Drawing.Point(3, 121);
            this.label_CatcherHeight.Name = "label_CatcherHeight";
            this.label_CatcherHeight.Size = new System.Drawing.Size(168, 16);
            this.label_CatcherHeight.TabIndex = 1;
            this.label_CatcherHeight.Text = "高度     0";
            // 
            // label_CatcherWidth
            // 
            this.label_CatcherWidth.BackColor = System.Drawing.Color.Transparent;
            this.label_CatcherWidth.Location = new System.Drawing.Point(3, 105);
            this.label_CatcherWidth.Name = "label_CatcherWidth";
            this.label_CatcherWidth.Size = new System.Drawing.Size(168, 16);
            this.label_CatcherWidth.TabIndex = 1;
            this.label_CatcherWidth.Text = "宽度     0";
            // 
            // label_CatcherLoc
            // 
            this.label_CatcherLoc.BackColor = System.Drawing.Color.Transparent;
            this.label_CatcherLoc.Location = new System.Drawing.Point(3, 89);
            this.label_CatcherLoc.Name = "label_CatcherLoc";
            this.label_CatcherLoc.Size = new System.Drawing.Size(168, 16);
            this.label_CatcherLoc.TabIndex = 1;
            this.label_CatcherLoc.Text = "位置  X: 0, Y: 0";
            // 
            // label_Info
            // 
            this.label_Info.AutoSize = true;
            this.label_Info.BackColor = System.Drawing.Color.Transparent;
            this.label_Info.Location = new System.Drawing.Point(3, 32);
            this.label_Info.Name = "label_Info";
            this.label_Info.Size = new System.Drawing.Size(173, 48);
            this.label_Info.TabIndex = 0;
            this.label_Info.Text = "鼠标右键取消\r\n双击取景窗口发送截屏\r\n\r\n取景：";
            // 
            // FormBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 404);
            this.Controls.Add(this.panel_Info);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormBack";
            this.Text = "Form Back";
            this.TopMost = true;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FormBack_MouseClick);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormBack_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormBack_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormBack_MouseDown);
            this.Load += new System.EventHandler(this.FormBack_Load);
            this.panel_Info.ResumeLayout(false);
            this.panel_Info.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Info;
        private System.Windows.Forms.Label label_Info;
        private System.Windows.Forms.Label label_CatcherHeight;
        private System.Windows.Forms.Label label_CatcherWidth;
        private System.Windows.Forms.Label label_CatcherLoc;

    }
}

