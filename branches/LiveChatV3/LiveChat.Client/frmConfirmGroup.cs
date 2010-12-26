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
using System.IO;

namespace LiveChat.Client
{
    public partial class frmConfirmGroup : Form
    {
        private ISeatService service;
        private Company company;
        private SeatGroup group;
        private GroupInfo request;
        public frmConfirmGroup(ISeatService service, Company company, SeatGroup group, GroupInfo request)
        {
            this.service = service;
            this.company = company;
            this.group = group;
            this.request = request;

            InitializeComponent();
        }

        private void frmConfirmGroup_Load(object sender, EventArgs e)
        {
            lblSeatCode.Text = request.Seat.SeatCode;
            lblSeatName.Text = request.Seat.SeatName;
            lblTelephone.Text = request.Seat.Telephone;
            lblMobileNumber.Text = request.Seat.MobileNumber;
            lblEmail.Text = request.Seat.Email;

            lblRequest.Text = "　　" + request.Request;

            label2.Text = string.Format("请求加入群【{0}】：", group.GroupName);

            if (request.Seat.FaceImage != null)
            {
                MemoryStream ms = new MemoryStream(request.Seat.FaceImage);
                Image img = BitmapManipulator.ResizeBitmap((Bitmap)Bitmap.FromStream(ms), 60, 60);
                pbSeatFace.Image = img;
            }
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
                if (radioButton2.Checked) type = AcceptType.Accept;
                else if (radioButton3.Checked) type = AcceptType.Refuse;

                service.ConfirmAddSeatGroup(request.ID, type, refuse);
                this.Close();
            }
            catch (LiveChatException ex)
            {
                ClientUtils.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
        }
    }
}
