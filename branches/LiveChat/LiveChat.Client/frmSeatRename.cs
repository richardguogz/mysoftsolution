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
        private Seat seat, friend;

        public frmSeatRename(ISeatService service, Seat seat, Seat friend)
        {
            this.service = service;
            this.seat = seat;
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
                bool success = service.RenameFriend(seat.SeatID, friend.SeatID, name);
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
    }
}
