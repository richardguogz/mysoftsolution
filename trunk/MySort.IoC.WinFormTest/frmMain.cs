using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MySoft.Core.Remoting;
using MySoft.IoC.Service;
using System.Diagnostics;
using MySoft.IoC.Dll;

namespace MySort.IoC.WinFormTest
{
    public delegate void UpdateMessage(string msg);

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = (int)numericUpDown1.Value;

            CastleFactory.Create().OnLog += new MySoft.Core.LogEventHandler(frmMain_OnLog);
            IUserService service = CastleFactory.Create().GetService<IUserService>("service");

            richTextBox1.AppendText("测试：" + service.GetUserInfo("test") + "\r\n");

            for (int i = 0; i < count; i++)
            {
                Thread thread = new Thread(DoWork);
                thread.Name = string.Format("Thread-->{0}", i);
                thread.IsBackground = true;
                thread.Start(service);
            }
        }

        void frmMain_OnLog(string log)
        {

        }

        private void DoWork(object value)
        {
            IUserService service = value as IUserService;
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                try
                {
                    UserInfo info = service.GetUserInfo("maoyong");

                    string msg = string.Format("线程：{0} 耗时：{1} ms 数据：{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, info.Description);
                    WriteMessage(msg);
                }
                catch (Exception ex)
                {
                    string msg = string.Format("线程：{0} 耗时：{1} ms 异常：{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, ex.Message);
                    WriteMessage(msg);
                }
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
