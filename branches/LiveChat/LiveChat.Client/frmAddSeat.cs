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
        private bool isLoaded = false;

        public frmAddSeat(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;

            InitializeComponent();
        }

        private void frmAddSeat_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 24);//分别是宽和高
            listSeats.SmallImageList = imgList;   //这里设置listView的SmallImageList 

            IList<Company> list = service.GetCompanies();
            comboBox1.DisplayMember = "CompanyName";
            comboBox1.ValueMember = "CompanyID";
            comboBox1.DataSource = list;

            //定位公司
            comboBox1.SelectedValue = company.CompanyID;

            LoadSeatInfo(company.CompanyID);

            isLoaded = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (comboBox1.SelectedValue == null) return;

            LoadSeatInfo(comboBox1.SelectedValue.ToString());
        }

        private void LoadSeatInfo(string companyID)
        {
            companyID = companyID ?? company.CompanyID;

            IList<Seat> list = service.GetCompanySeats(companyID);
            listSeats.Items.Clear();
            int index = 1;
            foreach (Seat info in list)
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
            if (listSeats.SelectedItems.Count == 0) return;
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
    }
}
