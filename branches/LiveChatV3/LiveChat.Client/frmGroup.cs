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
    public partial class frmGroup : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private SeatGroup group;
        private Company company;
        private Seat seat;
        private bool edit;

        public frmGroup(ISeatService service, Company company, Seat seat, SeatGroup group)
        {
            this.service = service;
            this.group = group;
            this.company = company;
            this.seat = seat;
            this.edit = false;

            if (group != null)
                this.edit = (group.CreateID == seat.SeatID || group.ManagerID == seat.SeatID);
            else
                this.edit = true;

            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string groupName = textBox1.Text.Trim();
            int maxCount = (int)numericUpDown1.Value;
            string createID = textBox4.Tag.ToString();
            string managerID = textBox5.Tag.ToString();
            string notification = textBox2.Text.Trim();
            string description = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(groupName))
            {
                ClientUtils.ShowMessage("群名称不能为空！");
                textBox1.Focus();
                return;
            }

            if (maxCount < 10 || maxCount > 100)
            {
                ClientUtils.ShowMessage("群人数应该在10-100人之间！");
                numericUpDown1.Focus();
                return;
            }

            //提示是否需要提交数据
            if (MessageBox.Show("确定提交吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;

            try
            {
                if (group == null)
                {
                    service.AddSeatGroup(groupName, maxCount, createID, managerID, notification, description);
                    if (Callback != null) Callback(null);
                }
                else
                {
                    service.UpdateSeatGroup(group.GroupID, groupName, maxCount, notification, description);
                    if (Callback != null) Callback(notification);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 24);//分别是宽和高
            listSeats.SmallImageList = imgList;   //这里设置listView的SmallImageList 

            if (group != null)
            {
                textBox1.Text = group.GroupName;
                numericUpDown1.Value = group.MaxPerson;

                if (!string.IsNullOrEmpty(group.CreateID))
                {
                    textBox4.Text = service.GetSeat(group.CreateID).SeatName;
                    textBox4.Tag = group.CreateID;
                }

                if (!string.IsNullOrEmpty(group.ManagerID))
                {
                    textBox5.Text = service.GetSeat(group.ManagerID).SeatName;
                    textBox5.Tag = group.ManagerID;
                }

                textBox2.Text = group.Notification;
                textBox3.Text = group.Description;

                btnSave.Text = "修改(&U)";

                LoadGroupSeats();

                if (group.CreateID == seat.SeatID)
                {
                    panel1.Visible = true;
                }
                else if (group.ManagerID == seat.SeatID)
                {
                    button3.Visible = false;
                    panel1.Visible = true;
                }
                else
                {
                    panel1.Visible = false;
                }
            }
            else
            {
                textBox4.Text = seat.SeatName;
                textBox5.Text = seat.SeatName;
                textBox4.Tag = seat.SeatID;
                textBox5.Tag = seat.SeatID;

                this.Text = "创建群";

                //如果是创建群，则移除第二个选项卡
                this.tabControl1.TabPages.RemoveAt(1);
            }

            if (!edit)
            {
                numericUpDown1.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;

                btnSave.Enabled = false;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (textBox1.Enabled)
            {
                textBox1.Focus();
            }
        }

        private void LoadGroupSeats()
        {
            //加载群成员
            IList<Seat> seats = service.GetGroupSeats(group.GroupID);
            listSeats.Items.Clear();
            if (seats != null)
            {
                int index = 1;
                foreach (Seat info in seats)
                {
                    ListViewItem item = new ListViewItem(new string[] { index.ToString(), info.SeatCode, info.SeatName, info.Telephone, info.MobileNumber, info.Email });
                    item.Tag = info;
                    listSeats.Items.Add(item);
                    index++;
                }
            }
        }

        //设为管理员
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listSeats.SelectedItems.Count == 0) return;
                Seat friend = listSeats.SelectedItems[0].Tag as Seat;

                if (group.CreateID == friend.SeatID || group.ManagerID == friend.SeatID)
                {
                    ClientUtils.ShowMessage(friend.SeatName + "已经是管理员。");
                    return;
                }

                if (MessageBox.Show("确定将【" + friend.SeatName + "】设置为管理员吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    service.SetSeatOnGroupManager(group.GroupID, friend.SeatID);
                    group.ManagerID = friend.SeatID;
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        //添加成员
        private void button1_Click(object sender, EventArgs e)
        {
            Singleton.Show(() =>
            {
                frmAddSeat frmadd = new frmAddSeat(service, company, seat);
                frmadd.Callback += new CallbackEventHandler(frmadd_Callback);
                return frmadd;
            });
        }

        //添加好友后返回
        void frmadd_Callback(object obj)
        {
            try
            {
                Seat friend = obj as Seat;
                service.AddSeatToGroup(group.GroupID, friend.SeatID);

                //重新加载客服
                LoadGroupSeats();
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        //删除成员
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listSeats.SelectedItems.Count == 0) return;
                Seat friend = listSeats.SelectedItems[0].Tag as Seat;

                if (group.ManagerID == friend.SeatID || group.CreateID == friend.SeatID)
                {
                    ClientUtils.ShowMessage("创建者与管理员不能被删除！");
                    return;
                }

                if (MessageBox.Show("确定将【" + friend.SeatName + "】删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    service.RemoveSeatFromGroup(group.GroupID, friend.SeatID);

                    //重新加载客服
                    LoadGroupSeats();
                }
            }
            catch (Exception ex)
            {
                ClientUtils.ShowError(ex);
            }
        }

        private void listSeats_DoubleClick(object sender, EventArgs e)
        {
            if (listSeats.SelectedItems.Count == 0) return;
            Seat friend = listSeats.SelectedItems[0].Tag as Seat;

            string key = string.Format("Config_{0}", friend.SeatID);
            SingletonMul.Show<frmSeatInfo>(key, () =>
            {
                frmSeatInfo frm = new frmSeatInfo(service, company, seat, friend);
                return frm;
            });
        }
    }
}
