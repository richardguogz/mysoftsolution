using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;
using LiveChat.Entity;
using System.IO;

namespace LiveChat.Client
{
    public partial class frmSeatInfo : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Company company;
        private Seat owner, friend;

        public frmSeatInfo(ISeatService service, Company company, Seat owner, Seat friend)
        {
            this.service = service;
            this.company = company;
            this.owner = owner;
            this.friend = friend;

            InitializeComponent();
        }

        private void frmSeatInfo_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = friend.SeatCode;
            txtSeatName.Text = friend.SeatName;
            txtTelephone.Text = friend.Telephone;
            txtMobileNumber.Text = friend.MobileNumber;
            txtEmail.Text = friend.Email;
            txtSign.Text = friend.Sign;
            txtRemark.Text = friend.Introduction;
            lblCompanyName.Text = service.GetCompany(friend.CompanyID).CompanyName;

            if (friend.FaceImage != null)
            {
                MemoryStream ms = new MemoryStream(friend.FaceImage);
                Image img = BitmapManipulator.ResizeBitmap((Bitmap)Bitmap.FromStream(ms), 60, 60);
                pbSeatFace.Image = img;
            }

            if (owner.SeatID != friend.SeatID)
            {
                this.Text = string.Format("【{0}】的个人资料", friend.SeatName);
                groupBox1.Enabled = false;
                button1.Visible = false;
                button3.Visible = true;
            }
            else
            {
                button3.Visible = false;
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
                    ClientUtils.ShowMessage("客服名称不能为空！");
                    txtSeatName.Focus();
                    return;
                }

                //提示是否需要提交数据
                if (MessageBox.Show("确定修改吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

                service.UpdateSeat(friend.SeatID, seatName, email, phone, mobile, sign, remark, friend.SeatType);
                friend.SeatName = seatName;
                friend.Sign = sign;
                friend.Introduction = remark;
                if (Callback != null) Callback(friend);
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

        private void button3_Click(object sender, EventArgs e)
        {
            //发送请求加对方为好友
            Singleton.Show<frmAddSeatConfirm>(() =>
            {
                frmAddSeatConfirm frm = new frmAddSeatConfirm(service, company, owner, friend);
                return frm;
            });
            this.Close();
        }
    }
}
