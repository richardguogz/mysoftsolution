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
    public partial class frmSetup : Form
    {
        private ISeatService service;
        private IList<Area> areaList;
        private Company company;
        private Seat seat;
        private bool isLoaded = true;
        public frmSetup(ISeatService service, IList<Area> areaList, Company company, Seat seat)
        {
            this.service = service;
            this.areaList = areaList;
            this.company = company;
            this.seat = seat;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtReplyTitle.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入回复标题！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtReplyTitle.Focus();

                return;
            }

            if (txtReplyBody.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入回复内容！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtReplyBody.Focus();

                return;
            }

            Reply info = new Reply();
            info.Title = txtReplyTitle.Text;
            info.Content = txtReplyBody.Text;
            info.AddTime = DateTime.Now;
            info.CompanyID = company.CompanyID;
            if (txtReplyTitle.Tag != null)
            {
                btnSave.Text = "添加(&A)";
                info.ReplyID = (int)txtReplyTitle.Tag;

                if (service.UpdateReply(info.ReplyID, info.Title, info.Content))
                {
                    txtReplyTitle.Tag = null;
                    txtReplyTitle.Text = string.Empty;
                    txtReplyBody.Text = string.Empty;

                    LoadReplyInfo();
                }
                else
                {
                    MessageBox.Show("保存快速回复信息失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (service.AddReply(company.CompanyID, info.Title, info.Content) > 0)
                {
                    txtReplyTitle.Tag = null;
                    txtReplyTitle.Text = string.Empty;
                    txtReplyBody.Text = string.Empty;

                    LoadReplyInfo();
                }
                else
                {
                    MessageBox.Show("保存快速回复信息失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (txtUrlTitle.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入链接标题！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUrlTitle.Focus();

                return;
            }

            if (txtUrlBody.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入链接地址！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUrlBody.Focus();

                return;
            }

            Link info = new Link();
            info.Title = txtUrlTitle.Text;
            info.Url = txtUrlBody.Text;
            if (txtUrlTitle.Tag != null)
            {
                btnSave2.Text = "添加(&A)";
                info.LinkID = (int)txtUrlTitle.Tag;
                if (service.UpdateLink(info.LinkID, info.Title, info.Url))
                {
                    txtUrlTitle.Tag = null;
                    txtUrlTitle.Text = string.Empty;
                    txtUrlBody.Text = string.Empty;
                    LoadLinkInfo();
                }
                else
                {
                    MessageBox.Show("保存链接信息失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (service.AddLink(company.CompanyID, info.Title, info.Url) > 0)
                {
                    txtUrlTitle.Tag = null;
                    txtUrlTitle.Text = string.Empty;
                    txtUrlBody.Text = string.Empty;
                    LoadLinkInfo();
                }
                else
                {
                    MessageBox.Show("保存链接信息失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (txtReplyTitle.Tag != null)
            {
                txtReplyTitle.Tag = null;
                txtReplyTitle.Text = string.Empty;
                txtReplyBody.Text = string.Empty;
                btnSave.Text = "添加(&A)";
                btnCancel.Text = "退出(&E)";
            }
            else
            {
                this.Close();
            }
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            if (txtUrlTitle.Tag != null)
            {
                txtUrlTitle.Tag = null;
                txtUrlTitle.Text = string.Empty;
                txtUrlBody.Text = string.Empty;
                btnSave2.Text = "添加(&A)";
                btnCancel2.Text = "退出(&E)";
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 加载回复信息和链接信息

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSetup_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 30);//分别是宽和高
            listView2.SmallImageList = imgList;   //这里设置listView的SmallImageList 

            ImageList imgList2 = new ImageList();
            imgList2.ImageSize = new Size(1, 24);//分别是宽和高
            listAds.SmallImageList = imgList2;   //这里设置listView的SmallImageList 
            listReply.SmallImageList = imgList2;
            listGroup.SmallImageList = imgList2;
            listSeats.SmallImageList = imgList2;
            listURL.SmallImageList = imgList2;

            LoadItems();

            SetCompanyInfo();
            LoadReplyInfo();
            LoadLinkInfo();

            if (seat.SeatType == SeatType.Super)
            {
                LoadCompanyInfo();
                LoadSeatInfo(null);
                LoadAdInfo(null);
                LoadGroupInfo(null);
            }

            tabControl1.Focus();

            isLoaded = false;
        }

        private void LoadItems()
        {
            ListViewItem item = new ListViewItem(" 基本设置");
            item.Tag = "setup1";
            item.Selected = true;
            listView2.Items.Add(item);

            item = new ListViewItem(" 快速回复");
            item.Tag = "setup2";
            listView2.Items.Add(item);

            item = new ListViewItem(" 推送链接");
            item.Tag = "setup3";
            listView2.Items.Add(item);

            if (seat.SeatType == SeatType.Super)
            {
                item = new ListViewItem(" 客服管理");
                item.Tag = "setup4";
                listView2.Items.Add(item);

                item = new ListViewItem(" 群管理");
                item.Tag = "setup5";
                listView2.Items.Add(item);

                item = new ListViewItem(" 广告管理");
                item.Tag = "setup6";
                listView2.Items.Add(item);
            }

            listView2.Click += new EventHandler(listView2_Click);
            tabControl1.Top = -22;
        }

        void listView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0) return;
            ListViewItem item = listView2.SelectedItems[0];
            switch (item.Tag.ToString())
            {
                case "setup1":
                    tabControl1.SelectedIndex = 0;
                    break;
                case "setup2":
                    tabControl1.SelectedIndex = 1;
                    break;
                case "setup3":
                    tabControl1.SelectedIndex = 2;
                    break;
                case "setup4":
                    tabControl1.SelectedIndex = 3;
                    break;
                case "setup5":
                    tabControl1.SelectedIndex = 4;
                    break;
                case "setup6":
                    tabControl1.SelectedIndex = 5;
                    break;
            }

            listView2.Focus();
        }

        private void SetCompanyInfo()
        {
            this.txtChatWebSite.Text = company.ChatWebSite;
            this.txtCompanyName.Text = company.CompanyName;
            this.txtWebSite.Text = company.WebSite;
            this.txtLogo.Text = company.CompanyLogo;
        }

        private void LoadCompanyInfo()
        {
            IList<Company> list = service.GetCompanies();
            cboAdCompany.DisplayMember = "CompanyName";
            cboAdCompany.ValueMember = "CompanyID";
            cboAdCompany.DataSource = list;
            cboAdCompany.SelectedValue = company.CompanyID;

            cboSeatCompany.DisplayMember = "CompanyName";
            cboSeatCompany.ValueMember = "CompanyID";
            cboSeatCompany.DataSource = new List<Company>(list);
            cboSeatCompany.SelectedValue = company.CompanyID;

            cboGroupCompany.DisplayMember = "CompanyName";
            cboGroupCompany.ValueMember = "CompanyID";
            cboGroupCompany.DataSource = new List<Company>(list);
            cboGroupCompany.SelectedValue = company.CompanyID;
        }

        private void LoadAdInfo(string companyID)
        {
            companyID = companyID ?? company.CompanyID;
            IList<Ad> list = service.GetAds(companyID, DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            listAds.Items.Clear();
            int index = 1;
            foreach (Ad info in list)
            {
                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.AdName, info.AdTitle, info.AdArea, info.AdUrl, info.AddTime.ToString() });
                item.Tag = info;
                listAds.Items.Add(item);
                index++;
            }
        }

        private void LoadSeatInfo(string companyID)
        {
            companyID = companyID ?? company.CompanyID;
            IList<SeatConfig> list = service.GetSeatConfigs(companyID);
            listSeats.Items.Clear();
            int index = 1;
            foreach (SeatConfig info in list)
            {
                if (info.SeatType == SeatType.Super) continue;

                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.SeatCode, info.SeatName, info.Telephone, info.MobileNumber, info.Email });
                item.Tag = info;
                listSeats.Items.Add(item);
                index++;
            }
        }

        private void LoadGroupInfo(string companyID)
        {
            companyID = companyID ?? company.CompanyID;
            IList<SGroup> list = service.GetSeatGroups(companyID);
            listGroup.Items.Clear();
            int index = 1;
            foreach (SGroup info in list)
            {
                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.GroupName, info.MaxPerson.ToString(), info.AddTime.ToString() });
                item.Tag = info;
                listGroup.Items.Add(item);
                index++;
            }
        }

        private void LoadReplyInfo()
        {
            IList<Reply> list = service.GetReplys(company.CompanyID);
            listReply.Items.Clear();
            int index = 1;
            foreach (Reply info in list)
            {
                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.Title, info.Content });
                item.Tag = info;
                listReply.Items.Add(item);
                index++;
            }
        }

        private void LoadLinkInfo()
        {
            IList<Link> list = service.GetLinks(company.CompanyID);
            listURL.Items.Clear();
            int index = 1;
            foreach (Link info in list)
            {
                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.Title, info.Url });
                item.Tag = info;
                listURL.Items.Add(item);
                index++;
            }
        }

        private void listReply_DoubleClick(object sender, EventArgs e)
        {
            if (listReply.SelectedItems.Count == 0) return;
            Reply info = (Reply)listReply.SelectedItems[0].Tag;
            txtReplyTitle.Text = info.Title;
            txtReplyBody.Text = info.Content;
            txtReplyTitle.Tag = info.ReplyID;
            btnSave.Text = "修改(&M)";
            btnCancel.Text = "取消(&C";
        }

        private void listURL_DoubleClick(object sender, EventArgs e)
        {
            if (listURL.SelectedItems.Count == 0) return;
            Link info = (Link)listURL.SelectedItems[0].Tag;
            txtUrlTitle.Text = info.Title;
            txtUrlBody.Text = info.Url;
            txtUrlTitle.Tag = info.LinkID;
            btnSave2.Text = "修改(&M)";
            btnCancel2.Text = "取消(&C";
        }

        private void btnSaveCompany_Click(object sender, EventArgs e)
        {
            if (txtChatWebSite.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入客服站点！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtChatWebSite.Focus();

                return;
            }

            if (txtCompanyName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入公司名称！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCompanyName.Focus();

                return;
            }

            if (txtWebSite.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入公司网址！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWebSite.Focus();

                return;
            }

            if (txtLogo.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入广告URL！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLogo.Focus();

                return;
            }

            Company c = new Company()
            {
                CompanyID = company.CompanyID,
                CompanyName = txtCompanyName.Text.Trim(),
                ChatWebSite = txtChatWebSite.Text.Trim(),
                CompanyLogo = txtLogo.Text.Trim(),
                WebSite = txtWebSite.Text.Trim()
            };

            if (service.UpdateCompany(c.CompanyID, c.CompanyName, c.WebSite, c.CompanyLogo, c.ChatWebSite))
            {
                this.company = service.GetCompany(c.CompanyID);
                MessageBox.Show("修改公司信息成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("保存链接信息失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancelCompany_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSetup_SizeChanged(object sender, EventArgs e)
        {
            tabControl1.Refresh();
        }

        #region 广告管理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            Company c = cboAdCompany.SelectedItem as Company;

            frmAd frm = new frmAd(service, areaList, c, null);
            frm.Callback += new CallbackEventHandler(frm_Callback);
            frm.ShowDialog();
        }

        void frm_Callback(object obj)
        {
            if (obj != null && obj.GetType() == typeof(string))
                LoadAdInfo(obj.ToString());
            else
                LoadAdInfo(null);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            if (listAds.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一条广告信息！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Ad ad = listAds.SelectedItems[0].Tag as Ad;
            Company c = cboAdCompany.SelectedItem as Company;

            frmAd frm = new frmAd(service, areaList, c, ad);
            frm.Callback += new CallbackEventHandler(frm_Callback);
            frm.ShowDialog();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            if (listAds.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一条广告信息！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("确定删除当前选中的广告吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //删除广告代码
                Ad ad = listAds.SelectedItems[0].Tag as Ad;
                if (service.DeleteAd(ad.ID))
                {
                    LoadAdInfo(null);
                }
            }
        }

        private void listAds_DoubleClick(object sender, EventArgs e)
        {
            if (listAds.SelectedItems.Count == 0) return;

            Ad ad = listAds.SelectedItems[0].Tag as Ad;
            Company c = cboAdCompany.SelectedItem as Company;

            frmAd frm = new frmAd(service, areaList, c, ad);
            frm.Callback += new CallbackEventHandler(frm_Callback);
            frm.ShowDialog();
        }

        #endregion

        #region 客服管理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            Company c = cboSeatCompany.SelectedItem as Company;

            frmSeat frmSeat = new frmSeat(service, c, null);
            frmSeat.Callback += new CallbackEventHandler(frmSeat_Callback);
            frmSeat.ShowDialog();
        }

        void frmSeat_Callback(object obj)
        {
            if (obj != null && obj.GetType() == typeof(string))
                LoadSeatInfo(obj.ToString());
            else
                LoadSeatInfo(null);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            if (listSeats.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一个客服！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SeatConfig seatConfig = listSeats.SelectedItems[0].Tag as SeatConfig;
            Company c = cboSeatCompany.SelectedItem as Company;

            frmSeat frmSeat = new frmSeat(service, c, seatConfig);
            frmSeat.Callback += new CallbackEventHandler(frmSeat_Callback);
            frmSeat.ShowDialog();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");

            if (listSeats.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一个客服！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //删除客服代码
            SeatConfig seatConfig = listSeats.SelectedItems[0].Tag as SeatConfig;
            if (seatConfig.SeatID == seat.SeatID)
            {
                MessageBox.Show("不能删除自己！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("确定删除当前选中的客服吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (seatConfig.State == OnlineState.Online)
                {
                    MessageBox.Show("此客服处于登录状态，不能删除！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (service.DeleteSeat(seatConfig.SeatID))
                    {
                        LoadSeatInfo(null);
                    }
                }
            }
        }

        private void listSeats_DoubleClick(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0) return;

            SeatConfig seatConfig = listSeats.SelectedItems[0].Tag as SeatConfig;
            Company c = cboSeatCompany.SelectedItem as Company;

            frmSeat frmSeat = new frmSeat(service, c, seatConfig);
            frmSeat.Callback += new CallbackEventHandler(frmSeat_Callback);
            frmSeat.ShowDialog();
        }

        #endregion

        #region 客服群管理

        /// <summary>
        /// 添加群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            if (listGroup.Items.Count > 0)
            {
                MessageBox.Show("目前只支持一个客服群！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Company c = cboGroupCompany.SelectedItem as Company;

            frmGroup frm1 = new frmGroup(service, c, null);
            frm1.Callback += new CallbackEventHandler(frm1_Callback);
            frm1.ShowDialog();
        }

        void frm1_Callback(object obj)
        {
            if (obj != null && obj.GetType() == typeof(string))
                LoadGroupInfo(obj.ToString());
            else
                LoadGroupInfo(null);
        }

        /// <summary>
        /// 修改群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");
            if (listGroup.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择客服群！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SGroup group = listGroup.SelectedItems[0].Tag as SGroup;
            Company c = cboGroupCompany.SelectedItem as Company;

            frmGroup frm2 = new frmGroup(service, c, group);
            frm2.Callback += new CallbackEventHandler(frm2_Callback);
            frm2.ShowDialog();
        }

        /// <summary>
        /// 删除群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未实现！");

            if (listGroup.Items.Count <= 1)
            {
                MessageBox.Show("目前只支持一个客服群，暂不能进行删除！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (listGroup.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择客服群！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SGroup group = listGroup.SelectedItems[0].Tag as SGroup;
            try
            {
                service.DeleteSeatGroup(group.GroupID);
                LoadGroupInfo(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listGroup_DoubleClick(object sender, EventArgs e)
        {
            if (listGroup.SelectedItems.Count == 0) return;

            SGroup group = listGroup.SelectedItems[0].Tag as SGroup;
            Company c = cboGroupCompany.SelectedItem as Company;

            frmGroup frm2 = new frmGroup(service, c, group);
            frm2.Callback += new CallbackEventHandler(frm2_Callback);
            frm2.ShowDialog();
        }

        void frm2_Callback(object obj)
        {
            LoadGroupInfo(null);
        }

        #endregion

        private void cboAdCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded) return;
            if (cboAdCompany.SelectedValue == null) return;

            LoadAdInfo(cboAdCompany.SelectedValue.ToString());
        }

        private void cboSeatCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded) return;
            if (cboSeatCompany.SelectedValue == null) return;

            LoadSeatInfo(cboSeatCompany.SelectedValue.ToString());
        }

        private void cboGroupCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded) return;
            if (cboGroupCompany.SelectedValue == null) return;

            LoadGroupInfo(cboGroupCompany.SelectedValue.ToString());
        }
    }
}
