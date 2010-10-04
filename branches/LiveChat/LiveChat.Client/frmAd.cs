using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Entity;
using LiveChat.Interface;

namespace LiveChat.Client
{
    public partial class frmAd : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private IList<Area> areaList;
        private Company company;
        private bool isLoaded = true;
        private Ad ad;
        public frmAd(ISeatService service, IList<Area> areaList, Company company, Ad ad)
        {
            this.service = service;
            this.areaList = areaList;
            this.company = company;
            this.ad = ad;

            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string adName = txtAdName.Text.Trim();
            string adTitle = txtAdTitle.Text.Trim();
            string adProvince = cboProvince.Text.Trim();
            string adArea = cboAdArea.Text.Trim();
            string adImgUrl = txtAdImgUrl.Text.Trim();
            string adUrl = txtAdUrl.Text.Trim();
            string adLogoImgUrl = txtAdLogoImgUrl.Text.Trim();
            string adLogoUrl = txtAdLogoUrl.Text.Trim();
            string adText = txtAdText.Text.Trim();
            string adTextUrl = txtAdTextUrl.Text.Trim();

            if (string.IsNullOrEmpty(adName))
            {
                ClientUtils.ShowMessage("广告名称不能为空！");
                txtAdName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(adTitle))
            {
                ClientUtils.ShowMessage("广告标题不能为空！");
                txtAdTitle.Focus();
                return;
            }

            if (string.IsNullOrEmpty(adArea))
            {
                ClientUtils.ShowMessage("投放地区不能为空！");
                cboAdArea.Focus();
                return;
            }

            if (string.IsNullOrEmpty(adImgUrl))
            {
                ClientUtils.ShowMessage("广告图片不能为空！");
                txtAdImgUrl.Focus();
                return;
            }

            if (string.IsNullOrEmpty(adUrl))
            {
                ClientUtils.ShowMessage("广告Url不能为空！");
                txtAdUrl.Focus();
                return;
            }

            //提示是否需要提交数据
            if (MessageBox.Show("确定提交吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                string area = string.Format("{0}|{1}", adProvince, adArea);
                if (ad == null)
                {
                    service.AddAd(company.CompanyID, adName, adTitle, area, adImgUrl, adUrl, adLogoImgUrl, adLogoUrl, adText, adTextUrl, chkDefault.Checked, chkCommon.Checked);
                }
                else
                {
                    service.UpdateAd(ad.ID, adName, adTitle, area, adImgUrl, adUrl, adLogoImgUrl, adLogoUrl, adText, adTextUrl, chkDefault.Checked, chkCommon.Checked);
                }

                if (Callback != null) Callback(company.CompanyID);
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

        private void frmAdManager_Load(object sender, EventArgs e)
        {
            LoadAreaInfo();

            if (company.IsHeadquarters)
            {
                chkCommon.Checked = true;
            }
            else
            {
                chkCommon.Visible = false;
                chkCommon.Checked = false;
            }

            //修改广告
            if (ad != null)
            {
                txtAdName.Text = ad.AdName;
                txtAdTitle.Text = ad.AdTitle;
                txtAdUrl.Text = ad.AdUrl;
                txtAdImgUrl.Text = ad.AdImgUrl;
                txtAdLogoImgUrl.Text = ad.AdLogoImgUrl;
                txtAdLogoUrl.Text = ad.AdLogoUrl;
                txtAdText.Text = ad.AdText;
                txtAdTextUrl.Text = ad.AdTextUrl;
                chkDefault.Checked = ad.IsDefault;
                chkCommon.Checked = ad.IsCommon;

                string[] area = ad.AdArea.Split('|');
                if (area.Length > 1)
                {
                    cboProvince.Text = area[0];
                    cboAdArea.Text = area[1];
                }
                else if (area.Length > 0)
                {
                    cboAdArea.Text = area[0];
                }

                btnSave.Text = "修改(&U)";
            }

            isLoaded = false;
        }

        private void LoadAreaInfo()
        {
            cboProvince.Text = string.Empty;
            IList<Area> list = (areaList as List<Area>).FindAll(p => string.IsNullOrEmpty(p.ParentID));
            cboProvince.DisplayMember = "AreaName";
            cboProvince.ValueMember = "AreaID";
            cboProvince.DataSource = list;

            if (list.Count > 0)
            {
                cboProvince.SelectedIndex = 0;
            }
        }

        private void cboProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded) return;

            cboProvince.Text = string.Empty;
            if (cboProvince.SelectedItem == null) return;
            string parentID = cboProvince.SelectedValue.ToString();

            IList<Area> list = (areaList as List<Area>).FindAll(p => p.ParentID == parentID);

            if (list.Count < 5)
            {
                cboAdArea.Visible = false;
            }
            else
            {
                cboAdArea.Visible = true;

                cboAdArea.DisplayMember = "AreaName";
                cboAdArea.ValueMember = "AreaID";
                cboAdArea.DataSource = list;

                if (list.Count > 0)
                {
                    cboAdArea.SelectedIndex = 0;
                }
            }
        }
    }
}
