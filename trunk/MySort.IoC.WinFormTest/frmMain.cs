using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MySoft.Remoting;
using MySoft.IoC;
using System.Diagnostics;
using MySoft.IoC.Dll;
using System.Runtime.Serialization;
using System.Reflection;
using System.Reflection.Emit;

namespace MySoft.IoC.WinFormTest
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
            //var item = new { Name = "maoyong", Age = 10, Children = new List<string>(new string[] { "a", "b" }) };
            //var json = MySoft.Core.SerializationManager.SerializeJson(item);

            //ConstructorInfo constructorInfo = item.GetType().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
            //var iii = constructorInfo.Invoke(new object[] { "a", 1 });

            //var objectType = CreateDynamicType(item.GetType());
            //var items = Activator.CreateInstance(objectType);

            //var item2 = MySoft.Core.SerializationManager.DeserializeJson(json, new { Name = string.Empty, Age = 0, Children = new List<string>() });

            //var a = item2.Name;
            //return;

            CastleFactory.Create().OnError += new ErrorLogEventHandler(frmMain_OnError);
            IUserService service = CastleFactory.Create().GetService<IUserService>("service");

            //var user = service.GetUserInfo("maoyong");

            //if (user != null)
            //{
            //    richTextBox1.AppendText(user.Description);
            //}

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
                    UserInfo info = service.GetUserInfo("maoyong_" + new Random().Next(10000000));

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
