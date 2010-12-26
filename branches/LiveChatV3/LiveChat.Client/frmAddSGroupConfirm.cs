using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Entity;
using LiveChat.Interface;
using LiveChat.Utils;
using System.IO;

namespace LiveChat.Client
{
    public partial class frmAddSGroupConfirm : Form
    {
        private ISeatService service;
        private Company company;
        private Seat seat;
        private SeatGroup group;
        public frmAddSGroupConfirm(ISeatService service, Company company, Seat seat, SeatGroup group)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.group = group;

            InitializeComponent();
        }

        private void frmAddSGroupConfirm_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = group.GroupName;
            lblSeatName.Text = string.Format("{0}人", group.PersonCount);

            Seat seat1 = service.GetSeat(group.CreateID);
            Seat seat2 = service.GetSeat(group.ManagerID);
            lblTelephone.Text = seat1.SeatName;
            lblMobileNumber.Text = seat2.SeatName;
            lblEmail.Text = group.Description;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string request = textBox1.Text.Trim();
                bool ret = service.AddSeatGroupRequest(seat.SeatID, group.GroupID, request);
                if (ret)
                {
                    ClientUtils.ShowMessage("群请求信息发送成功！");
                }
                this.Close();
            }
            catch (LiveChatException ex)
            {
                //显示异常信息
                ClientUtils.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
