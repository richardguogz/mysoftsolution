using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;

namespace LiveChat.Client
{
    public partial class frmPassword : Form
    {
        private ISeatService service;
        private string seatID;
        public frmPassword(ISeatService service, string seatID)
        {
            this.service = service;
            this.seatID = seatID;
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入旧密码！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOldPassword.Focus();

                return;
            }

            if (txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入新密码！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();

                return;
            }

            if (txtPassword2.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入确认密码！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword2.Focus();

                return;
            }

            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string newPassword2 = txtPassword2.Text.Trim();

            if (newPassword != newPassword2)
            {
                MessageBox.Show("两次输入的密码不一致，请重输！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();

                return;
            }

            if (service.UpdatePassword(seatID, oldPassword, newPassword))
            {
                MessageBox.Show("修改密码成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("修改密码失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
