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
    public partial class frmConfirmFriend : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Company company;
        private Seat friend;
        private RequestInfo request;
        public frmConfirmFriend(ISeatService service, Company company, Seat friend, RequestInfo request)
        {
            this.service = service;
            this.company = company;
            this.friend = friend;
            this.request = request;

            InitializeComponent();
        }

        private void frmConfirmFriend_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = friend.SeatCode;
            lblSeatName.Text = friend.SeatName;
            lblTelephone.Text = friend.Telephone;
            lblMobileNumber.Text = friend.MobileNumber;
            lblEmail.Text = friend.Email;

            lblRequest.Text = "　　" + request.Request;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string refuse = textBox1.Text.Trim();
                AcceptType type = AcceptType.Accept;
                if (radioButton1.Checked) type = AcceptType.AcceptAdd;
                else if (radioButton2.Checked) type = AcceptType.Accept;
                else if (radioButton3.Checked) type = AcceptType.Refuse;

                service.ConfirmAddSeatFriend(request.ID, type, refuse);
                if (Callback != null) Callback(null);
                this.Close();
            }
            catch (LiveChatException ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
        }
    }
}
