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

namespace LiveChat.Client
{
    public partial class frmAddSeat : Form
    {
        private ISeatService service;
        private Company company;
        private Seat seat;

        public frmAddSeat(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;

            InitializeComponent();
        }

        private void frmAddSeat_Load(object sender, EventArgs e)
        {
            //上一步
            firstPanel.Visible = true;
            nextPanel.Visible = false;
            companyPanel.Visible = false;
            friendPanel.Visible = true;

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 24);//分别是宽和高
            listSeats.SmallImageList = imgList;   //这里设置listView的SmallImageList 

            IList<Company> list = service.GetCompanies();
            comboBox1.DisplayMember = "CompanyName";
            comboBox1.ValueMember = "CompanyID";
            comboBox1.DataSource = list;

            //定位公司
            comboBox1.SelectedValue = company.CompanyID;
        }

        private void LoadSeatInfo(IList<Seat> seats)
        {
            listSeats.Items.Clear();
            int index = 1;
            foreach (Seat info in seats)
            {
                //去除自己
                if (info.SeatID == seat.SeatID) continue;

                ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.SeatCode, info.SeatName, info.Telephone, info.MobileNumber, info.Email });
                item.Tag = info;
                listSeats.Items.Add(item);
                index++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0)
            {
                ClientUtils.ShowMessage("请选择一个好友！");
                return;
            }

            Seat config = listSeats.SelectedItems[0].Tag as Seat;

            Singleton.Show<frmAddSeatConfirm>(() =>
            {
                frmAddSeatConfirm frm = new frmAddSeatConfirm(service, company, seat, config);
                return frm;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listSeats_DoubleClick(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0) return;
            Seat friend = listSeats.SelectedItems[0].Tag as Seat;

            string key = string.Format("Config_{0}", friend.SeatID);
            SingletonMul.Show<frmSeatInfo>(key, () =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, company, seat, friend);
                return frm;
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //上一步
            firstPanel.Visible = true;
            nextPanel.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string id = textBox1.Text.Trim();
                string name = textBox2.Text.Trim();
                string cname = textBox3.Text.Trim();
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(cname))
                {
                    ClientUtils.ShowMessage("请输入客服工号、客服名称或公司名称！");
                    textBox1.Focus();
                    return;
                }

                IList<Seat> list = service.GetSeatsFromIDOrName(id, name, cname);
                LoadSeatInfo(list);
            }
            else if (radioButton2.Checked)
            {
                string companyID = comboBox1.SelectedValue.ToString();
                IList<Seat> list = service.GetCompanySeats(companyID);
                LoadSeatInfo(list);
            }

            //下一步
            firstPanel.Visible = false;
            nextPanel.Visible = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            companyPanel.Visible = false;
            friendPanel.Visible = true;
            textBox1.Focus();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            companyPanel.Visible = true;
            friendPanel.Visible = false;
            comboBox1.Focus();
        }
    }
}
