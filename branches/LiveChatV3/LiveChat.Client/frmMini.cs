using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveChat.Client
{
    public partial class frmMini : Form
    {
        private string url;
        public frmMini(string url)
        {
            this.url = url;

            InitializeComponent();
        }

        private void frmMini_Load(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.ScrollBarsEnabled = false;
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
                webBrowser1.Navigate(url, false);
            }
            catch { }
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document != null)
            {
                this.Text = webBrowser1.DocumentTitle;
            }
        }
    }
}
