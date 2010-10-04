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
    public partial class frmAddSeatConfirm : Form
    {
        private ISeatService service;
        private Company company;
        private Seat seat, friend;
        public frmAddSeatConfirm(ISeatService service, Company company, Seat seat, Seat friend)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.friend = friend;

            InitializeComponent();
        }

        private void frmAddSeatConfirm_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = friend.SeatCode;
            lblSeatName.Text = friend.SeatName;
            lblTelephone.Text = friend.Telephone;
            lblMobileNumber.Text = friend.MobileNumber;
            lblEmail.Text = friend.Email;

            if (friend.FaceImage != null)
            {
                MemoryStream ms = new MemoryStream(friend.FaceImage);
                Image img = BitmapManipulator.ResizeBitmap((Bitmap)Bitmap.FromStream(ms), 60, 60);
                pbSeatFace.Image = img;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string request = textBox1.Text.Trim();
                bool ret = service.AddSeatFriendRequest(seat.SeatID, friend.SeatID, request);
                if (ret)
                {
                    ClientUtils.ShowMessage("添加好友请求发送成功！");
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
