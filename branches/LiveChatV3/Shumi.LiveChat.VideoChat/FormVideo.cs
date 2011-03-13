using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Shumi.LiveChat.VideoChat
{
    public partial class FormVideo : Form
    {
        //1.创建客户端,创建后可显示本地视频,可做相应窗口设置.参数:父窗口,是否启动单独的EXE运行;返回窗口句柄.
        //相当于初始化,必须先调用此接口,后面接口才能使用.
        [DllImport("NCVideoChat.dll")]
        public static extern IntPtr NNVCreateClient(IntPtr Int32Parent, Int32 bNewExe);

        //2.登陆服务器后可与其他人连接.参数:服务器IP,端口,用户名,密码.最后一个参数暂时不使用.
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVLoginToServer(String strIP, Int32 nPort, String strUser, String strPwd, Int32 dwData);

        //3.添加用户到本房间.某用户登陆本房间时,参数:父窗口,用户帐号,是否自动开启视频音频.返回视频窗口句柄.
        [DllImport("NCVideoChat.dll")]
        public static extern IntPtr NNVAddUser(IntPtr hParent, String strUser, Int32 bOpen);

        //4.打开视频给对方收看.参数:对方帐号
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVOpenVideoTo(String strUser);

        //5.关闭视频不给对方收看.参数:对方帐号.
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVCloseVideoTo(String strUser);

        //6.删除某用户.某用户退出本房间时
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVRemoveUser(String strUser);

        //7.销毁客户端.软件退出.
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVDestroyClient();
        //设置大视频还是小视频
        [DllImport("NCVideoChat.dll")]
        public static extern Int32 NNVSetVideoSize(Int32 nSize);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        Int32 m_nVideo;	//视频个数.

        IntPtr[] m_hVideoWnd = new IntPtr[6];	//6个视频窗口
        Int32[] m_bSend = new Int32[6];
        String[] m_strName = new String[6];

        public FormVideo()
        {
            InitializeComponent();

            //初始化
            m_nVideo = 0;
            for (int i = 0; i < 6; i++)
            {
                m_hVideoWnd[i] = IntPtr.Zero;
                m_bSend[i] = 0;
                m_strName[i] = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //创建客户端. 
            m_hVideoWnd[0] = NNVCreateClient(Handle, 0); //
            if (m_hVideoWnd[0] == IntPtr.Zero)
            {
                MessageBox.Show("创建失败!");
                return;
            }

            NNVLoginToServer("www.enen6.com", 9558, "1001", "222", 0);
            NNVSetVideoSize(0);

            MoveWindow(m_hVideoWnd[0], 10, 150, 174, 179, 1);
        }

        /// 重写窗体的消息处理函数
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x400 + 6668)//接收自定义消息
            {
                int a = (int)m.WParam;
                switch (a)
                {

                    case 100:  //第二个按钮被按下
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 1; i < m_nVideo; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    if (m_bSend[i] == 0)
                                    {
                                        NNVOpenVideoTo(m_strName[i]);
                                        m_bSend[i] = 1;
                                    }
                                    else
                                    {
                                        NNVCloseVideoTo(m_strName[i]);
                                        m_bSend[i] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 101: //第一个按钮
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 1; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    MessageBox.Show(m_strName[i]);
                                    break;
                                }
                            }
                        }
                        break;
                    case 102: //视频窗口被隐藏.
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    MessageBox.Show("用户关闭!此演示程序继续显示该窗口", m_strName[i]);
                                    ShowWindow(m_hVideoWnd[i], 5);

                                    break;
                                }
                            }
                        }
                        break;
                    case 103: //视频退出.
                        {
                            NNVDestroyClient();
                            for (int i = 0; i < 6; i++)
                            {
                                m_hVideoWnd[i] = IntPtr.Zero;
                                m_nVideo = 0;
                                button1.Enabled = true;
                            }
                        }
                        break;
                    case 104:	//用户名或密码出错.
                        {
                            NNVDestroyClient();
                            for (int i = 0; i < 6; i++)
                            {
                                m_hVideoWnd[i] = IntPtr.Zero;
                                m_nVideo = 0;
                                button1.Enabled = true;
                            }
                            MessageBox.Show("登陆失败！帐号有误");
                        }
                        break;
                    case 105:	//与服务器的连接掉线了。
                        {
                            //MessageBox.Show("msg:掉线了");
                        }
                        break;
                    case 106:	//登陆服务器成功。
                        {
                            //MessageBox.Show("msg:上线了\n");
                        }
                        break;
                    case 107:		//连接对方成功，第二个参数:窗口句柄.
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    //...
                                    break;
                                }
                            }
                        }
                        break;
                    case 108:	//断开与对方的连接。第二个参数:窗口句柄.
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    //...
                                    break;
                                }
                            }
                        }
                        break;

                    case 109:	//对方不在线，无法连接。第二个参数:窗口句柄.
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    MessageBox.Show("对方不在线，无法连接", m_strName[i]);
                                    break;
                                }
                            }
                        }
                        break;
                    case 110:		//对方没有添加你为用户，无法连接。第二个参数:窗口句柄.
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    MessageBox.Show("你没有被对方添加为用户,无法连接", m_strName[i]);
                                    break;
                                }
                            }
                        }
                        break;

                    case 111:	//连接对方超时，连接失败。
                        {
                            IntPtr hWnd = m.LParam;
                            for (int i = 0; i < 6; i++)
                            {
                                if (m_hVideoWnd[i] == hWnd)
                                {
                                    //...
                                    break;
                                }
                            }
                        }
                        break;
                    case 112:	//登陆服务器超时，登陆失败。
                        {
                            //MessageBox.Show ("msg:登陆服务器超时，登陆失败\n");
                        }
                        break;
                    case 113:	//其他原因(如：版本过低或人数已满等)登陆失败。
                        {
                            //MessageBox.Show ("msg:其他原因登陆失败\n");
                        }
                        break;
                    case 123: //是否子窗口还是弹出窗口
                        {
                            // if (checkBox3.Checked)
                            m.Result = (IntPtr)666;
                            //else
                            //    m.Result = (IntPtr)0;
                        }
                        break;
                }

            }
            else
            {
                base.DefWndProc(ref m);
            }
        }
    }
}
