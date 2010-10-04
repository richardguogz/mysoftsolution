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
    public partial class frmGroup : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private SGroup group;
        private Company company;
        public frmGroup(ISeatService service, Company company, SGroup group)
        {
            this.service = service;
            this.company = company;
            this.group = group;

            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string groupName = textBox1.Text.Trim();
            int maxCount = (int)numericUpDown1.Value;

            if (string.IsNullOrEmpty(groupName))
            {
                ClientUtils.ShowMessage("客服群名称不能为空！");
                textBox1.Focus();
                return;
            }

            if (maxCount < 10 || maxCount > 100)
            {
                ClientUtils.ShowMessage("客服群人数应该在10-100人之间！");
                numericUpDown1.Focus();
                return;
            }

            //提示是否需要提交数据
            if (MessageBox.Show("确定提交吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                if (group == null)
                {
                    service.AddSeatGroup(company.CompanyID, groupName, maxCount);
                    if (Callback != null) Callback(typeof(SGroup));
                }
                else
                {
                    service.UpdateSeatGroup(group.GroupID, groupName, maxCount);
                    if (Callback != null) Callback(company.CompanyID);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            if (group != null)
            {
                textBox1.Text = group.GroupName;

                if (group.MaxPerson.HasValue)
                    numericUpDown1.Value = group.MaxPerson.Value;

                btnSave.Text = "修改(&U)";
            }
        }
    }
}
