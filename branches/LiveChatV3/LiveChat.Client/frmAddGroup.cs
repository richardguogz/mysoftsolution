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
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Seat seat;
        public frmAddGroup(ISeatService service, Seat seat)
        {
            this.service = service;
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

            if (MessageBox.Show("确定加入群【" + group.GroupName + "】吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                service.JoinGroup(seat.SeatID, group.GroupID);
                if (Callback != null) Callback(null);
                this.Close();
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
                frmGroup frm = new frmGroup(service, seat, group);
                return frm;
            });
        }
    }
}
