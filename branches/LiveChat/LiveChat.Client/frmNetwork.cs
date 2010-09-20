using System;
using System.Configuration;
using System.Windows.Forms;
using LiveChat.Remoting;

namespace LiveChat.Client
{
    public partial class frmNetwork : Form
    {
        public event CallbackEventHandler Callback;

        private string oldurl = string.Empty;
        public frmNetwork()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("IP地址不允许为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                MessageBox.Show("端口不允许为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox2.Focus();
                return;
            }

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                var client = RemotingClientConfiguration.GetConfig();
                if (client.RemotingHosts.ContainsKey("host1"))
                {
                    var host = client.RemotingHosts["host1"];
                    host.DefaultServer = "s1";
                    if (host.Servers.ContainsKey("s1"))
                    {
                        var server = host.Servers["s1"];
                        var url = string.Format("tcp://{0}:{1}", textBox1.Text.Trim(), textBox2.Text.Trim());

                        server.ServerUrl = url;

                        string xml = System.IO.File.ReadAllText(config.FilePath);
                        xml = xml.Replace(oldurl, url);
                        System.IO.File.WriteAllText(config.FilePath, xml);

                        AppDomain.CurrentDomain.SetData(host.Name, null);

                        config.Save(ConfigurationSaveMode.Modified);
                        config = null;

                        //回调
                        if (Callback != null) Callback(null);

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmNetwork_Load(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                var client = RemotingClientConfiguration.GetConfig();
                if (client.RemotingHosts.ContainsKey("host1"))
                {
                    var host = client.RemotingHosts["host1"];
                    host.DefaultServer = "s1";
                    if (host.Servers.ContainsKey("s1"))
                    {
                        var server = host.Servers["s1"];

                        oldurl = server.ServerUrl;
                        var url = oldurl.Remove(server.ServerUrl.IndexOf("tcp://"), 6);

                        textBox1.Text = url.Split(':')[0];
                        textBox2.Text = url.Split(':')[1];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
