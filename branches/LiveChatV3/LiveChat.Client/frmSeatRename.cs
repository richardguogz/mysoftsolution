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
    public partial class frmSeatRename : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        private SeatFriend friend;

        public frmSeatRename(ISeatService service, SeatFriend friend)
        {
            this.service = service;
            this.friend = friend;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
                name = null;

            try
            {
                bool success = service.RenameFriend(friend.Owner.SeatID, friend.SeatID, name);
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

        private void frmSeatRename_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = friend.MemoName;
        }
    }
}
