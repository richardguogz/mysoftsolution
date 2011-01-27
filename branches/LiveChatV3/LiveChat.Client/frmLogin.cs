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
using MySoft;
using System.IO;
using MySoft.Security;
using System.Net.Sockets;
using System.Diagnostics;

namespace LiveChat.Client
{
    public partial class frmLogin : Form
    {
        private ISeatService service;
        private string style;
        private Size size;
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
            this.path = CoreHelper.GetFullPath("/user.dat");

            if (service == null)
            {
                service = RemotingUtil.GetRemotingSeatService();
            }

            ReadUserInfoForFile();

            if (string.IsNullOrEmpty(style))
            {
                style = "office2007";
            }

            frmNav_Callback(style);

            //如果是自动登录
            if (!isRelogin && checkBox2.Checked)
            {
                btnLogin_Click(sender, e);
            }
            else if (isRelogin)
            {
                this.Text = "重新登录";
            }

            //用户信息
            //this.txtCompanyID.Text = "1001";
            //this.txtClientID.Text = "super";
            //this.txtPassword.Text = "super";
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            AppUpdater updater = new AppUpdater();

            string newVersion, oldVersion;
            bool isUpdate = updater.CheckForUpdate(out newVersion, out oldVersion);
            if (isUpdate)
            {
                if (MessageBox.Show("发现新版本【" + newVersion + "】的面料QQ软件，是否立即升级？\r\n目前使用的版本【" + oldVersion + "】", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    在线升级ToolStripMenuItem_Click(null, null);
                }
            }
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
                if (string.IsNullOrEmpty(companyID))
                {
                    ClientUtils.ShowMessage("请输入公司ID");
                    txtCompanyID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(userID))
                {
                    ClientUtils.ShowMessage("请输入用户ID");
                    txtClientID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    ClientUtils.ShowMessage("请输入密码");
                    txtPassword.Focus();
                    return;
                }

                IMResult result = service.Login(clientID, companyID, userID, password);

                switch (result)
                {
                    case IMResult.InvalidUser:

                        ClientUtils.ShowMessage("此公司用户ID不存在！");
                        txtCompanyID.Focus();
                        break;

                    case IMResult.InvalidPassword:

                        ClientUtils.ShowMessage("输入的密码不正确！");
                        txtPassword.Focus();
                        break;

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
                            Rectangle rect = new Rectangle(point, this.size);
                            frmNavigate frmNav = new frmNavigate(service, company, seat, clientID, rect);
                            frmNav.Callback += new CallbackEventHandler(frmNav_Callback);
                            frmNav.SizeChangedCallback += new CallbackEventHandler(frmNav_SizeChangedCallback);
                            return frmNav;
                        });

                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        void frmNav_SizeChangedCallback(object obj)
        {
            if (obj == null) return;
            Size size = (Size)obj;

            try
            {
                IniFiles ini = new IniFiles(path);
                string section = "userinfo";
                ini.WriteString(section, "formsize", Encode(string.Format("{0}*{1}", size.Width, size.Height)));
            }
            catch { }
        }

        void frmNav_Callback(object obj)
        {
            if (obj == null) return;

            string style = obj.ToString();

            try
            {
                IniFiles ini = new IniFiles(path);
                string section = "userinfo";
                ini.WriteString(section, "formstyle", Encode(style));
            }
            catch { }

            this.skinEngine1.SkinFile = CoreHelper.GetFullPath(string.Format("/skin/{0}.ssk", style));
        }

        private void ReadUserInfoForFile()
        {
            if (!File.Exists(path))
            {
                this.size = this.Size;
                return;
            }

            try
            {
                IniFiles ini = new IniFiles(path);
                string section = "userinfo";

                this.txtCompanyID.Text = Decode(ini.ReadString(section, "companyid", null));
                this.txtClientID.Text = Decode(ini.ReadString(section, "userid", null));
                this.txtPassword.Text = Decode(ini.ReadString(section, "password", null));
                this.checkBox1.Checked = Convert.ToBoolean(Decode(ini.ReadString(section, "chkpassword", Encode(false))));
                this.checkBox2.Checked = Convert.ToBoolean(Decode(ini.ReadString(section, "chkautologin", Encode(false))));
                this.style = Decode(ini.ReadString(section, "formstyle", null));

                string s = Decode(ini.ReadString(section, "formsize", null));
                if (string.IsNullOrEmpty(s))
                    this.size = this.Size;
                else
                    this.size = new Size(Convert.ToInt32(s.Split('*')[0]), Convert.ToInt32(s.Split('*')[1]));
            }
            catch { }

            //this.txtCompanyID.Text = "1001";
            //this.txtClientID.Text = "super";
            //this.txtPassword.Text = "super";

        }

        private void WriteUserInfoToFile(string companyid, string userid, string password)
        {
            try
            {
                IniFiles ini = new IniFiles(path);
                string section = "userinfo";

                ini.WriteString(section, "companyid", Encode(companyid));
                ini.WriteString(section, "userid", Encode(userid));
                ini.WriteString(section, "password", Encode(password));
                ini.WriteString(section, "chkpassword", Encode(checkBox1.Checked));
                ini.WriteString(section, "chkautologin", Encode(checkBox2.Checked));
                ini.WriteString(section, "formstyle", Encode(style));
            }
            catch { }
        }

        private string Encode(object value)
        {
            if (value == null) return null;
            return AESEncrypt.Encode(value.ToString(), "LiveChat");
        }

        private string Decode(object value)
        {
            if (value == null) return null;
            return AESEncrypt.Decode(value.ToString(), "LiveChat");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClientUtils.ExitApplication();
        }

        void frm_Callback(object obj)
        {
            service = RemotingUtil.GetRemotingSeatService();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isNewForm)
            {
                ClientUtils.ExitApplication();
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
            this.skinEngine1.SkinFile = CoreHelper.GetFullPath(string.Format("/skin/{0}.ssk", style));
        }

        private void 在线升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("在线升级时需要退出当前应用程序，确定退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ClientUtils.ExitApplication();

                ProcessStartInfo process = new ProcessStartInfo(CoreHelper.GetFullPath("AutoUpdate.exe"));
                Process p = Process.Start(process);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = linkLabel1.Tag.ToString();
                Process ps = new Process();
                ps.StartInfo.FileName = "iexplore.exe";
                ps.StartInfo.Arguments = url;
                ps.Start();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientUtils.ExitApplication();
        }

        private void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            txtClientID.Text = string.Empty;
        }

        private void txtClientID_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Text = string.Empty;
        }
    }
}