using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;

namespace LiveChat.Client
{
    public partial class frmCompany : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Company company;
        public frmCompany(ISeatService service, Company company)
        {
            this.service = service;
            this.company = company;

            InitializeComponent();
        }

        private void btnSaveCompany_Click(object sender, EventArgs e)
        {
            string companyID = txtCompanyCode.Text.Trim();
            string companyName = txtCompanyName.Text.Trim();
            string companyWebSite = txtWebSite.Text.Trim();
            string companyLogo = txtLogo.Text.Trim();
            string chatWebSite = txtChatWebSite.Text.Trim();

            if (string.IsNullOrEmpty(companyID))
            {
                ClientUtils.ShowMessage("公司ID不能为空！");
                txtCompanyCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(companyName))
            {
                ClientUtils.ShowMessage("公司名称不能为空！");
                txtCompanyName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(chatWebSite))
            {
                ClientUtils.ShowMessage("客服站点不能为空！");
                txtChatWebSite.Focus();
                return;
            }

            //提示是否需要提交数据
            if (MessageBox.Show("确定提交吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                if (company == null)
                {
                    Company cp = new Company()
                    {
                        CompanyID = companyID,
                        CompanyName = companyName,
                        ChatWebSite = chatWebSite,
                        CompanyLogo = companyLogo,
                        WebSite = companyWebSite,
                        AddTime = DateTime.Now
                    };
                    service.AddCompany(cp);
                }
                else
                {
                    service.UpdateCompany(companyID, companyName, companyWebSite, companyLogo, chatWebSite);
                }

                if (Callback != null) Callback(null);
                this.Close();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void btnCancelCompany_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCompany_Load(object sender, EventArgs e)
        {
            if (company != null)
            {
                txtCompanyCode.Enabled = false;

                txtCompanyCode.Text = company.CompanyID;
                txtCompanyName.Text = company.CompanyName;
                txtWebSite.Text = company.WebSite;
                txtLogo.Text = company.CompanyLogo;
                txtChatWebSite.Text = company.ChatWebSite;

                btnSaveCompany.Text = "修改(&U)";
            }
        }
    }
}
