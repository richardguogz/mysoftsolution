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
    public partial class frmSeat : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Seat seat;
        private Company company;

        public frmSeat(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.seat = seat;
            this.company = company;

            InitializeComponent();
        }

        private void frmSeat_Load(object sender, EventArgs e)
        {
            label10.Text = string.Empty;
            cboSeatType.Items.RemoveAt(2);

            if (seat != null)
            {
                txtSeatCode.Text = seat.SeatCode;
                txtSeatCode.Enabled = false;

                txtSeatName.Text = seat.SeatName;
                txtPassword.Text = seat.Password;
                txtPassword.Enabled = false;

                txtEmail.Text = seat.Email;
                txtTelephone.Text = seat.Telephone;
                txtMobileNumber.Text = seat.MobileNumber;

                switch (seat.SeatType)
                {
                    case SeatType.Normal:
                        cboSeatType.SelectedIndex = 0;
                        break;
                    case SeatType.Manager:
                        cboSeatType.SelectedIndex = 1;
                        break;
                    case SeatType.Super:
                        cboSeatType.SelectedIndex = 2;
                        break;
                }

                txtSign.Text = seat.Sign;
                txtRemark.Text = seat.Introduction;

                button1.Text = "修改(&U)";

                if (seat.State == OnlineState.Online)
                {
                    label10.Text = "此客服处于登录状态，不能修改！";
                    button1.Enabled = false;
                }
            }
            else
            {
                cboSeatType.SelectedIndex = 0;

                //设置默认的回复信息
                txtRemark.Text = string.Format("您好，我是{0}客服，很高兴为您在线服务！", company.CompanyName);
            }
        }

        /// <summary>
        /// 保存客服
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string seatCode = txtSeatCode.Text.Trim();
            string seatName = txtSeatName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtTelephone.Text.Trim();
            string mobile = txtMobileNumber.Text.Trim();
            string sign = txtSign.Text.Trim();
            string remark = txtRemark.Text.Trim();
            SeatType type = (SeatType)cboSeatType.SelectedIndex;

            if (string.IsNullOrEmpty(seatCode))
            {
                ClientUtils.ShowMessage("客服代码不能为空！");
                txtSeatCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(seatName))
            {
                ClientUtils.ShowMessage("客服名称不能为空！");
                txtSeatName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ClientUtils.ShowMessage("登录密码不能为空！");
                txtPassword.Focus();
                return;
            }

            //提示是否需要提交数据
            if (MessageBox.Show("确定提交吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                if (seat == null)
                {
                    service.AddSeat(company.CompanyID, seatCode, seatName, password, email, phone, mobile, sign, remark, type);
                }
                else
                {
                    service.UpdateSeat(seat.SeatID, seatName, email, phone, mobile, sign, remark, type);
                }
                if (Callback != null) Callback(company.CompanyID);
                this.Close();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
