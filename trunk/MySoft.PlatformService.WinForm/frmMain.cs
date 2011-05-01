using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using MySoft.Net.Sockets;
using MySoft.IoC;
using MySoft.PlatformService.UserService;
using MySoft.Logger;

namespace MySoft.PlatformService.WinForm
{
    public partial class frmMain : Form
    {
        public delegate void UpdateMessage(string msg);
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CastleFactory.Create().OnError += new ErrorLogEventHandler(frmMain_OnError);
            IUserService service = CastleFactory.Create().GetService<IUserService>();

            int count = (int)numericUpDown1.Value;

            for (int i = 0; i < count; i++)
            {
                Thread thread = new Thread(DoWork);
                thread.Name = string.Format("Thread-->{0}", i);
                thread.IsBackground = true;
                thread.Start(service);
            }
        }

        void frmMain_OnError(Exception exception)
        {
            WriteMessage(exception.Message);
        }

        private void DoWork(object value)
        {
            IUserService service = value as IUserService;
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                try
                {
                    int userid;
                    UserInfo info = service.GetUserInfo("maoyong_" + new Random().Next(10000000), out userid);

                    string msg = string.Format("线程：{0} 耗时：{1} ms 数据：{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, info.Description);
                    WriteMessage(msg);
                }
                catch (Exception ex)
                {
                    string msg = string.Format("线程：{0} 耗时：{1} ms 异常：{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, ex.Message);
                    WriteMessage(msg);
                }

                Thread.Sleep(10);
            }
        }

        private void WriteMessage(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateMessage(p =>
                {
                    richTextBox1.AppendText(p + "\r\n");
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    richTextBox1.ScrollToCaret();
                }), msg);
            }
            else
            {
                richTextBox1.AppendText(msg + "\r\n");
                richTextBox1.Select(richTextBox1.TextLength, 0);
                richTextBox1.ScrollToCaret();
            }
        }
    }
}
