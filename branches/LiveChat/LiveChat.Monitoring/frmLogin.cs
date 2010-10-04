using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;
using LiveChat.Utils;

namespace LiveChat.Monitoring
{
    public partial class frmLogin : Form
    {
        private ISeatService service;
        private bool isExit = true;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (service == null)
            {
                service = RemotingUtil.GetRemotingSeatService();
            }

            //this.txtCompanyName.Text = "数米网";
            //this.txtUserID.Text = "admin";
            //this.txtPassword.Text = "admin";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string companyID = txtCompanyID.Text.Trim();
            string userID = txtUserID.Text.Trim();
            string password = txtPassword.Text.Trim();

            Guid clientID = Guid.NewGuid();

            try
            {
                IMResult result = service.Login(clientID, companyID, userID, password);

                switch (result)
                {
                    case IMResult.InvalidUser:

                        MessageBox.Show("此公司用户ID不存在！", "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case IMResult.InvalidPassword:

                        MessageBox.Show("输入的密码不正确！", "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case IMResult.Successful:

                        Company company = service.GetCompany(companyID);
                        Seat seat = service.GetSeat(companyID, userID);
                        new frmMonitor(service, company, seat, clientID).Show();
                        isExit = false;
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isExit)
            {
                Application.Exit();
            }
        }
    }
}
