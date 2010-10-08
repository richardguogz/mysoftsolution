using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LiveChat.Client
{
    public partial class frmPopup : Form
    {
        //查看事件
        public event CallbackEventHandler CallbackView;

        //取消事件
        public event CallbackEventHandler CallbackCancel;

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

            tip.Message = new Regex("<img[^>]+>").Replace(tip.Message, "[表情]");
            this.label1.Text = tip.Message;

            Win32.SetWindowPos(this.Handle, Win32.HWND_TOPMOST, Screen.PrimaryScreen.Bounds.Width - this.Width,
                Screen.PrimaryScreen.Bounds.Height - this.Height - 30, 50, 50, 1); //设置弹出位置

            Win32.AnimateWindow(this.Handle, 500, Win32.AW_VER_NEGATIVE); //设置弹出的动作
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CallbackView != null) CallbackView(tip);
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CallbackCancel != null) CallbackCancel(tip);
            this.Close();
        }
    }
}
