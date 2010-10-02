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
    public partial class frmSeatInfo : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Seat seat;
        private bool isSelf;
        public frmSeatInfo(ISeatService service, Seat seat, bool isSelf)
        {
            this.service = service;
            this.seat = seat;
            this.isSelf = isSelf;

            InitializeComponent();
        }

        private void frmSeatInfo_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = seat.SeatCode;
            txtSeatName.Text = seat.SeatName;
            txtTelephone.Text = seat.Telephone;
            txtMobileNumber.Text = seat.MobileNumber;
            txtEmail.Text = seat.Email;
            txtSign.Text = seat.Sign;
            txtRemark.Text = seat.Introduction;

            if (!isSelf)
            {
                this.Text = string.Format("【{0}】的个人资料", seat.SeatName);
                groupBox1.Enabled = false;
                button1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string seatName = txtSeatName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtTelephone.Text.Trim();
                string mobile = txtMobileNumber.Text.Trim();
                string sign = txtSign.Text.Trim();
                string remark = txtRemark.Text.Trim();

                if (string.IsNullOrEmpty(seatName))
                {
                    MessageBox.Show("客服名称不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSeatName.Focus();
                    return;
                }

                //提示是否需要提交数据
                if (MessageBox.Show("确定修改吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

                service.UpdateSeat(seat.SeatID, seatName, email, phone, mobile, sign, remark, seat.SeatType);
                seat.SeatName = seatName;
                seat.Sign = sign;
                seat.Introduction = remark;
                if (Callback != null) Callback(seat);
                this.Close();
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
