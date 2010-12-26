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
    public partial class frmAddGroup : Form
    {
        private ISeatService service;
        private Company company;
        private Seat seat;
        public frmAddGroup(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;

            InitializeComponent();
        }

        private void frmAddGroup_Load(object sender, EventArgs e)
        {
            var groups = service.GetSeatNoJoinGroups(seat.SeatID);
            listGroups.Items.Clear();
            foreach (SeatGroup info in groups)
            {
                ListViewItem item = new ListViewItem(new string[] { info.GroupName, info.MaxPerson.ToString(), info.AddTime.ToString("yyyy-MM-dd"), info.Description });
                item.Tag = info;
                item.ImageIndex = 2;
                listGroups.Items.Add(item);
            }
        }

        //确定
        private void button4_Click(object sender, EventArgs e)
        {
            if (listGroups.SelectedItems.Count == 0) return;
            SeatGroup group = listGroups.SelectedItems[0].Tag as SeatGroup;

            try
            {
                //service.JoinGroup(seat.SeatID, group.GroupID);
                //if (Callback != null) Callback(null);
                //this.Close();

                string key = string.Format("AddToGroup_{0}", group.GroupID);
                SingletonMul.Show<frmAddSGroupConfirm>(key, () =>
                {
                    frmAddSGroupConfirm frm = new frmAddSGroupConfirm(service, company, seat, group);
                    return frm;
                });
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listGroups_DoubleClick(object sender, EventArgs e)
        {
            if (listGroups.SelectedItems.Count == 0) return;
            SeatGroup group = listGroups.SelectedItems[0].Tag as SeatGroup;

            string key = string.Format("SeatGroup_{0}", group.GroupID);
            SingletonMul.Show<frmGroup>(key, () =>
            {
                frmGroup frm = new frmGroup(service, company, seat, group);
                return frm;
            });
        }
    }
}
