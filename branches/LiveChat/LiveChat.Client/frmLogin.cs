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
using LiveChat.Remoting;
using MySoft.Core;
using System.IO;
using MySoft.Core.Security;

namespace LiveChat.Client
{
    public partial class frmLogin : Form
    {
        private ISeatService service;
        private bool isExit = true;
        private bool isNewForm = false;
        private string path;
        private bool isRelogin;

        public frmLogin(bool isRelogin)
        {
            this.isRelogin = isRelogin;

            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.Left = 50;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height - 50) / 2;
            this.path = CoreUtils.GetFullPath("/user.dat");
            frmNav_Callback("office2007");

            if (service == null)
            {
                service = RemotingUtil.GetRemotingSeatService();
            }

            ReadUserInfoForFile();

            //如果是自动登录
            if (!isRelogin && checkBox2.Checked)
            {
                btnLogin_Click(sender, e);
            }
            else
            {
                this.Text = "重新登录";
            }

            //用户信息
            //this.txtCompanyID.Text = "1001";
            //this.txtClientID.Text = "super";
            //this.txtPassword.Text = "super";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            isNewForm = true;
            string companyID = txtCompanyID.Text.Trim();
            string userID = txtClientID.Text.Trim();
            string password = txtPassword.Text.Trim();

            Guid clientID = Guid.NewGuid();

            try
            {
                //IMResult result = service.LoginForCompanyName(clientID, companyName, userID, password, false);
                IMResult result = service.Login(clientID, companyID, userID, password, false);

                switch (result)
                {
                    case IMResult.InvalidUser:

                        MessageBox.Show("此公司用户ID不存在！", "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCompanyID.Focus();
                        break;

                    case IMResult.InvalidPassword:

                        MessageBox.Show("输入的密码不正确！", "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPassword.Focus();
                        break;

                    //case IMResult.NotManager:

                    //    MessageBox.Show("监控程序只允许客服经理及管理员登录！", "用户登录", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;

                    case IMResult.Successful:

                        if (checkBox1.Checked)
                        {
                            WriteUserInfoToFile(companyID, userID, password);
                        }
                        else
                        {
                            WriteUserInfoToFile(companyID, userID, null);
                        }

                        Company company = service.GetCompany(companyID);
                        Seat seat = service.GetSeat(companyID, userID);

                        Singleton.Show(() =>
                        {
                            Point point = new Point(this.Left, this.Top);
                            frmNavigate frmNav = new frmNavigate(service, company, seat, clientID, point);
                            frmNav.Callback += new CallbackEventHandler(frmNav_Callback);
                            return frmNav;
                        });

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

        void frmNav_Callback(object obj)
        {
            if (obj == null) return;

            string style = obj.ToString();
            this.skinEngine1.SkinFile = CoreUtils.GetFullPath(string.Format("/skin/{0}.ssk", style));
        }

        private void ReadUserInfoForFile()
        {
            if (!File.Exists(path)) return;
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //读取用户信息
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string value = sr.ReadLine();

                        try
                        {
                            if (value.Contains("[COMPANYID]"))
                            {
                                this.txtCompanyID.Text = Decode(value.Replace("[COMPANYID]", null));
                            }
                            else if (value.Contains("[USERID]"))
                            {
                                this.txtClientID.Text = Decode(value.Replace("[USERID]", null));
                            }
                            else if (value.Contains("[PASSWORD]"))
                            {
                                this.txtPassword.Text = Decode(value.Replace("[PASSWORD]", null));
                            }
                            else if (value.Contains("[CHKPASSWORD]"))
                            {
                                this.checkBox1.Checked = Convert.ToBoolean(Decode(value.Replace("[CHKPASSWORD]", null)));
                            }
                            else if (value.Contains("[CHKAUTOLOGIN]"))
                            {
                                this.checkBox2.Checked = Convert.ToBoolean(Decode(value.Replace("[CHKAUTOLOGIN]", null)));
                            }

                            //this.txtCompanyID.Text = "1001";
                            //this.txtClientID.Text = "super";
                            //this.txtPassword.Text = "super";
                        }
                        catch { }
                    }
                }
            }
        }

        private void WriteUserInfoToFile(string companyid, string userid, string password)
        {
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("[COMPANYID]{0}", Encode(companyid));
                    sw.WriteLine("[USERID]{0}", Encode(userid));
                    if (!string.IsNullOrEmpty(password))
                        sw.WriteLine("[PASSWORD]{0}", Encode(password));
                    sw.WriteLine("[CHKPASSWORD]{0}", Encode(checkBox1.Checked.ToString()));
                    sw.WriteLine("[CHKAUTOLOGIN]{0}", Encode(checkBox2.Checked));
                }
            }
        }

        private string Encode(object value)
        {
            return AESEncrypt.Encode(value.ToString(), "LiveChat");
        }

        private string Decode(object value)
        {
            return AESEncrypt.Decode(value.ToString(), "LiveChat");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isExit)
            {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        void frm_Callback(object obj)
        {
            service = RemotingUtil.GetRemotingSeatService();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isNewForm)
            {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        private void 网络设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNetwork frm = new frmNetwork();
            frm.Callback += new CallbackEventHandler(frm_Callback);
            frm.ShowDialog();
        }

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }

        private void StyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string style = item.ToString();
            this.skinEngine1.SkinFile = CoreUtils.GetFullPath(string.Format("/skin/{0}.ssk", style));
        }
    }
}