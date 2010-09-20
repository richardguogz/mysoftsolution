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

namespace LiveChat.Client
{
    public partial class frmAddSeatConfirm : Form
    {
        private ISeatService service;
        private Company company;
        private Seat seat;
        private SeatConfig config;
        public frmAddSeatConfirm(ISeatService service, Company company, Seat seat, SeatConfig config)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;
            this.config = config;

            InitializeComponent();
        }

        private void frmAddSeatConfirm_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = config.SeatCode;
            lblSeatName.Text = config.SeatName;
            lblTelephone.Text = config.Telephone;
            lblMobileNumber.Text = config.MobileNumber;
            lblEmail.Text = config.Email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚未完成！");
            if (MessageBox.Show(string.Format("确定添加【{0}】为您的好友吗？", config.SeatName), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    string request = textBox1.Text.Trim();
                    bool ret = service.AddSeatFriendRequest(seat.SeatID, config.SeatID, request);
                    if (ret)
                    {
                        MessageBox.Show("添加好友请求发送成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (LiveChatException ex)
                {
                    MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
