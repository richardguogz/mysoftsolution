using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LiveChat.Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Singleton.Show<FormBack>(() =>
            //{
            //    FormBack frm = new FormBack();
            //    //frm.Callback += new CallbackEventHandler(frm_Callback);
            //    return frm;
            //});

            Singleton.Show<frmLogin>(() =>
            {
                frmLogin frm = new frmLogin(false);
                return frm;
            });
            Application.Run();
        }
    }
}