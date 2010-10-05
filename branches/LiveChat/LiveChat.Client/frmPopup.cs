using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveChat.Client
{
    public partial class frmPopup : Form
    {
        //关闭窗口事件
        public event CallbackEventHandler Callback;

        private IntPtr HWND_TOPMOST = new IntPtr(-1);
        private TipInfo tip;

        public frmPopup(TipInfo tip)
        {
            this.tip = tip;

            InitializeComponent();
        }

        private void frmPopup_Load(object sender, EventArgs e)
        {
            this.Text = tip.Title;
            this.label1.Text = tip.Message;

            Win32.SetWindowPos(this.Handle, Win32.HWND_TOPMOST, Screen.PrimaryScreen.Bounds.Width - this.Width,
                Screen.PrimaryScreen.Bounds.Height - this.Height - 30, 50, 50, 1); //设置弹出位置

            Win32.AnimateWindow(this.Handle, 500, Win32.AW_VER_NEGATIVE); //设置弹出的动作
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Callback != null) Callback(tip);
            this.Close();
        }
    }
}
