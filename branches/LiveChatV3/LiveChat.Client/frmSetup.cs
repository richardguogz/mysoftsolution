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
                ClientUtils.ShowMessage("请输入回复标题！");
                txtReplyTitle.Focus();

                return;
            }

            if (txtReplyBody.Text.Trim().Length == 0)
            {
                ClientUtils.ShowMessage("请输入回复内容！");
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
                    ClientUtils.ShowMessage("保存快速回复信息失败！");
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
                    ClientUtils.ShowMessage("保存快速回复信息失败！");
                }
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUrlTitle.Text.Trim().Length == 0)
                {
                    ClientUtils.ShowMessage("请输入链接标题！");
                    txtUrlTitle.Focus();

                    return;
                }

                if (txtUrlBody.Text.Trim().Length == 0)
                {
                    ClientUtils.ShowMessage("请输入链接地址！");
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
                        ClientUtils.ShowMessage("保存链接信息失败！");
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
                        ClientUtils.ShowMessage("保存链接信息失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
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
            listSeats.SmallImageList = imgList2;
            listURL.SmallImageList = imgList2;
            listCompany.SmallImageList = imgList2;

            LoadItems();

            SetCompanyInfo();
            LoadReplyInfo();
            LoadLinkInfo();

            if (seat.SeatType == SeatType.Super)
            {
                LoadCompanyInfo();
                LoadSeatInfo(null);
                LoadAdInfo(null);
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
                item = new ListViewItem(" 公司管理");
                item.Tag = "setup7";
                listView2.Items.Add(item);

                item = new ListViewItem(" 客服管理");
                item.Tag = "setup4";
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
                //case "setup5":
                //    tabControl1.SelectedIndex = 4;
                //    break;
                case "setup6":
                    tabControl1.SelectedIndex = 4;
                    break;
                case "setup7":
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

        private void LoadAdInfo(string companyID)
        {
            companyID = companyID ?? company.CompanyID;
            IList<Ad> list = service.GetAds(companyID, DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            listAds.Items.Clear();
            if (list == null) return;
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
            IList<Seat> list = service.GetCompanySeats(companyID);
            listSeats.Items.Clear();
            if (list == null) return;
            int index = 1;
            foreach (Seat info in list)
            {
                if (info.SeatType == SeatType.Super) continue;

                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.SeatCode, info.SeatName, info.Telephone, info.MobileNumber, info.Email });
                item.Tag = info;
                listSeats.Items.Add(item);
                index++;
            }
        }

        private void LoadCompanyInfo()
        {
            IList<Company> list = service.GetCompanies();
            listCompany.Items.Clear();
            if (list == null) return;

            //添加列表中的公司信息
            cboAdCompany.DisplayMember = "CompanyName";
            cboAdCompany.ValueMember = "CompanyID";
            cboAdCompany.DataSource = list;
            cboAdCompany.SelectedValue = company.CompanyID;

            cboSeatCompany.DisplayMember = "CompanyName";
            cboSeatCompany.ValueMember = "CompanyID";
            cboSeatCompany.DataSource = new List<Company>(list);
            cboSeatCompany.SelectedValue = company.CompanyID;

            int index = 1;
            foreach (Company info in list)
            {
                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.CompanyName, info.SeatCount.ToString(), info.AddTime.ToString() });
                item.Tag = info;
                listCompany.Items.Add(item);
                index++;
            }
        }

        private void LoadReplyInfo()
        {
            IList<Reply> list = service.GetReplys(company.CompanyID);
            listReply.Items.Clear();
            if (list == null) return;
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
            if (list == null) return;
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
                ClientUtils.ShowMessage("请输入客服站点！");
                txtChatWebSite.Focus();

                return;
            }

            if (txtCompanyName.Text.Trim().Length == 0)
            {
                ClientUtils.ShowMessage("请输入公司名称！");
                txtCompanyName.Focus();

                return;
            }

            if (txtWebSite.Text.Trim().Length == 0)
            {
                ClientUtils.ShowMessage("请输入公司网址！");
                txtWebSite.Focus();

                return;
            }

            if (txtLogo.Text.Trim().Length == 0)
            {
                ClientUtils.ShowMessage("请输入广告URL！");
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

            try
            {

                if (service.UpdateCompany(c.CompanyID, c.CompanyName, c.WebSite, c.CompanyLogo, c.ChatWebSite))
                {
                    this.company = service.GetCompany(c.CompanyID);
                    ClientUtils.ShowMessage("修改公司信息成功！");
                }
                else
                {
                    ClientUtils.ShowMessage("修改公司信息失败！");
                }
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
            if (listAds.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一条广告信息！");
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
            if (listAds.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一条广告信息！");
                return;
            }

            if (MessageBox.Show("确定删除当前选中的广告吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    //删除广告代码
                    Ad ad = listAds.SelectedItems[0].Tag as Ad;
                    if (service.DeleteAd(ad.ID))
                    {
                        LoadAdInfo(null);
                    }
                }
                catch (Exception ex)
                {
                    ClientUtils.ShowError(ex);
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
            if (listSeats.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一个客服！");
                return;
            }

            Seat seatConfig = listSeats.SelectedItems[0].Tag as Seat;
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
            if (listSeats.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一个客服！");
                return;
            }

            //删除客服代码
            Seat seatConfig = listSeats.SelectedItems[0].Tag as Seat;
            if (seatConfig.SeatID == seat.SeatID)
            {
                ClientUtils.ShowMessage("不能删除自己！");
                return;
            }

            if (MessageBox.Show("确定删除当前选中的客服吗？\r\n\r\n删除客服的操作最谨慎操作！", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (seatConfig.State == OnlineState.Online)
                {
                    ClientUtils.ShowMessage("此客服处于登录状态，不能删除！");
                    return;
                }
                else
                {
                    try
                    {
                        if (service.DeleteSeat(seatConfig.SeatID))
                        {
                            LoadSeatInfo(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientUtils.ShowError(ex);
                    }
                }
            }
        }

        private void listSeats_DoubleClick(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0) return;

            Seat seatConfig = listSeats.SelectedItems[0].Tag as Seat;
            Company c = cboSeatCompany.SelectedItem as Company;

            frmSeat frmSeat = new frmSeat(service, c, seatConfig);
            frmSeat.Callback += new CallbackEventHandler(frmSeat_Callback);
            frmSeat.ShowDialog();
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

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            frmCompany frmCompany = new frmCompany(service, null);
            frmCompany.Callback += new CallbackEventHandler(frmCompany_Callback);
            frmCompany.ShowDialog();
        }

        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (listCompany.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一个公司！");
                return;
            }

            Company company = listCompany.SelectedItems[0].Tag as Company;

            frmCompany frmCompany = new frmCompany(service, company);
            frmCompany.Callback += new CallbackEventHandler(frmCompany_Callback);
            frmCompany.ShowDialog();
        }

        void frmCompany_Callback(object obj)
        {
            LoadCompanyInfo();
        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (listCompany.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一个公司！");
                return;
            }

            Company company = listCompany.SelectedItems[0].Tag as Company;
            if (company.SeatCount > 0)
            {
                ClientUtils.ShowMessage("此公司存在使用中的客服，不允许删除！");
                return;
            }

            if (MessageBox.Show("确定删除当前选中的公司吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    service.DeleteCompany(company.CompanyID);
                    LoadCompanyInfo();
                }
                catch (Exception ex)
                {
                    ClientUtils.ShowError(ex);
                }
            }
        }

        private void listCompany_DoubleClick(object sender, EventArgs e)
        {
            if (listCompany.SelectedItems.Count == 0) return;

            Company company = listCompany.SelectedItems[0].Tag as Company;

            frmCompany frmCompany = new frmCompany(service, company);
            frmCompany.Callback += new CallbackEventHandler(frmCompany_Callback);
            frmCompany.ShowDialog();
        }
    }
}
