﻿using System;
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
        private SeatGroup group;

        public frmGroupRename(ISeatService service, Seat seat, SeatGroup group)
        {
            this.service = service;
            this.group = group;
            this.seat = seat;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
                name = null;

            try
            {
                bool success = service.UpdateSeatGroupName(seat.SeatID, group.GroupID, name);
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
            this.textBox1.Text = group.MemoName;
        }
    }
}
