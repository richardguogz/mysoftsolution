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
    public partial class frmGroupRename : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private Seat seat;
        private SeatFriendGroup group;

        public frmGroupRename(ISeatService service, Seat seat, SeatFriendGroup group)
        {
            this.service = service;
            this.seat = seat;
            this.group = group;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
                name = null;

            try
            {
                bool success = false;
                if (group == null)
                {
                    group = new SeatFriendGroup()
                    {
                        GroupID = Guid.NewGuid(),
                        GroupName = name
                    };
                    success = service.AddFriendGroup(seat.SeatID, group);
                }
                else
                {
                    success = service.UpdateFriendGroupName(seat.SeatID, group.GroupID, name);
                }
                if (success) Callback(name);
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

        private void frmGroupRename_Load(object sender, EventArgs e)
        {
            if (group == null)
            {
                this.Text = "新建好友组名";
            }
            else
            {
                this.textBox1.Text = group.GroupName;
            }
        }
    }
}
