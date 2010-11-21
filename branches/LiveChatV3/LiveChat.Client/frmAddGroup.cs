using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveChat.Interface;

namespace LiveChat.Client
{
    public partial class frmAddGroup : Form
    {
        public event CallbackEventHandler Callback;

        private ISeatService service;
        public frmAddGroup(ISeatService service)
        {
            this.service = service;

            InitializeComponent();
        }

        private void frmAddGroup_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
