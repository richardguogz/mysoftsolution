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
    public partial class frmSeatFace : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Company company;
        private Seat seat;

        public frmSeatFace(ISeatService service, Company company, Seat seat)
        {
            this.service = service;
            this.company = company;
            this.seat = seat;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFile.Text.Trim() == string.Empty)
            {
                ClientUtils.ShowMessage("请先选择要传送的文件！");
                button3.Focus();
                return;
            }

            try
            {
                string fileName = txtFile.Text.Trim();
                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fs);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);

                service.UpdateSeatFace(seat.SeatID, buffer);
                if (Callback != null) Callback(pbSeatFace.Image);
                this.Close();

                ClientUtils.ShowMessage("头像修改成功！");
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
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(openFileDialog1.FileName);
                if (file.Length > 1024 * 100)
                {
                    ClientUtils.ShowMessage("当前选择的文件大于100K，不能做为用户头像！");
                    return;
                }

                string extension = file.Extension;

                //如果是图片则用上传图片的方法
                if (!(extension == ".jpg" || extension == ".gif" || extension == ".bmp" || extension == ".png"))
                {
                    ClientUtils.ShowMessage("头像格式不正确，必须是.jpg、.gif、.bmp、.png的文件！");
                    return;
                }

                txtFile.Text = openFileDialog1.FileName;

                try
                {
                    Bitmap bitmap = (Bitmap)Image.FromFile(txtFile.Text);
                    Image img = BitmapManipulator.ResizeBitmap(bitmap, 60, 60);
                    pbSeatFace.Image = img;
                }
                catch { }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定将您的头像重置为默认头像吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                pbSeatFace.Image = pbDefault.Image;

                try
                {
                    service.UpdateSeatFace(seat.SeatID, null);
                    if (Callback != null) Callback(pbSeatFace.Image);
                }
                catch (Exception ex)
                {
                    ClientUtils.ShowError(ex);
                }
            }
        }

        private void frmSeatFace_Load(object sender, EventArgs e)
        {
            if (seat.FaceImage != null)
            {
                MemoryStream ms = new MemoryStream(seat.FaceImage);
                Image img = BitmapManipulator.ResizeBitmap((Bitmap)Bitmap.FromStream(ms), 60, 60);
                pbSeatFace.Image = img;
            }
        }
    }
}
