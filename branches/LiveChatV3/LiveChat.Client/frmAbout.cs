using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace LiveChat.Client
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(Application.ExecutablePath);
            label1.Text = string.Format("面料QQ v{0} Build {1}", Assembly.GetExecutingAssembly().GetName().Version,
                file.CreationTime.ToString("yyyyMMdd"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
