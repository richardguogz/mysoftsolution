using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using LiveChat.Entity;
using System.Windows.Forms;

namespace LiveChat.Client
{
    public class VideoChat
    {
        private Seat fromUser;
        private Seat toUser;
        private System.Drawing.Rectangle rect;
        private IntPtr clientWnd;

        Int32 m_VideoCount = 2;
        IntPtr[] m_hVideoWnd = new IntPtr[2];	//6个视频窗口
        Int32[] m_bSend = new Int32[2];
        Int32[] m_bOpen = new Int32[2];
        Int32[] m_bOnline = new Int32[2];
        String[] m_strName = new String[2];
        bool isCreateForm = false;

        public VideoChat(Seat fromUser, Seat toUser, IntPtr clientWnd, Rectangle rect)
        {
            this.fromUser = fromUser;
            this.toUser = toUser;
            this.clientWnd = clientWnd;
            this.rect = rect;

            m_strName[0] = string.Format("{0}+{1}", fromUser.CompanyID, fromUser.SeatCode);
            m_strName[1] = string.Format("{0}+{1}", toUser.CompanyID, toUser.SeatCode);
        }

        /// <summary>
        /// 创建一个客户端，并登录
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="seat"></param>
        public static void CreateClientAndLogin(IntPtr hWnd, Seat seat)
        {
            NNVCreateClient(hWnd, 0);

            string strUser = string.Format("{0}+{1}", seat.CompanyID, seat.SeatCode);
            NNVLoginToServer(ClientConfig.VideoChatUrl, 9558, strUser, seat.Password, 0);
        }

        /// <summary>
        /// 销毁客户端
        /// </summary>
        public static void ExitClient()
        {
            NNVDestroyClient();
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        public void CreateClient()
        {
            if (m_hVideoWnd[0] == IntPtr.Zero)
            {
                this.m_hVideoWnd[0] = NNVCreateClient(clientWnd, 0);
                NNVSetVideoSize(0);
            }

            this.isCreateForm = true;
        }

        /// <summary>
        /// 销毁客户端
        /// </summary>
        public void DestroyClient()
        {
            NNVDestroyClient();
        }

        /// <summary>
        /// 登录到服务器
        /// </summary>
        public void LoginToServer()
        {
            NNVLoginToServer(ClientConfig.VideoChatUrl, 9558, m_strName[0], fromUser.Password, 0);
            SetWindowText(m_hVideoWnd[0], fromUser.SeatName);
        }

        /// <summary>
        /// 初始化请求
        /// </summary>
        /// <param name="send"></param>
        public void InitRequest(bool send)
        {
            if (m_hVideoWnd[0] != IntPtr.Zero)
            {
                ShowWindow(m_hVideoWnd[0], 5);
            }

            if (m_hVideoWnd[1] == IntPtr.Zero)
            {
                m_hVideoWnd[1] = NNVAddUser(clientWnd, m_strName[1], 1);
                SetWindowText(m_hVideoWnd[1], toUser.SeatName);
                ShowWindow(m_hVideoWnd[1], 5);
            }

            if (send)
            {
                //发送_ReqV表示请求
                NNVSendTextTo(m_strName[1], "_ReqV");
            }
        }

        /// <summary>
        /// 设置是否在线
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="online"></param>
        public void SetOnline(IntPtr hWnd, bool online)
        {
            for (int i = 0; i < m_VideoCount; i++)
            {
                if (m_hVideoWnd[i] == hWnd)
                {
                    m_bOnline[i] = online ? 1 : 0;
                }
            }
        }

        /// <summary>
        /// 打开视频会话
        /// </summary>
        /// <param name="hWnd"></param>
        public void OperateVideo(IntPtr hWnd)
        {
            for (int i = 0; i < m_VideoCount; i++)
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

                    break;
                }
            }
        }

        #region 操作用户

        /// <summary>
        /// 打开视频会话
        /// </summary>
        /// <param name="strUser"></param>
        public void OperateVideo(string strUser)
        {
            for (int i = 0; i < m_VideoCount; i++)
            {
                if (m_strName[i] == strUser)
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

                    break;
                }
            }
        }

        /// <summary>
        /// 给用户发送消息
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strText"></param>
        public void SendText(string strUser, string strText)
        {
            NNVSendTextTo(strUser, strText);
        }

        #endregion

        /// <summary>
        /// 退出视频
        /// </summary>
        public void ExitVideo()
        {
            if (m_hVideoWnd[0] != IntPtr.Zero)
            {
                NNVRemoveUser(m_strName[1]);

                ShowWindow(m_hVideoWnd[0], 0);
                ShowWindow(m_hVideoWnd[1], 0);

                m_hVideoWnd[1] = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 打开大视频
        /// </summary>
        /// <param name="hWnd"></param>
        public void OpenBigVideo(IntPtr hWnd)
        {
            for (int i = 0; i < m_VideoCount; i++)
            {
                if (m_hVideoWnd[i] == hWnd)
                {
                    if (m_bOpen[i] == 0)
                    {
                        NNVSendTextTo(m_strName[i], "_popup"); //_popup弹出窗口,_unpopup关闭弹出窗口
                        m_bOpen[i] = 1;
                    }
                    else
                    {
                        NNVSendTextTo(m_strName[i], "_unpopup"); //_popup弹出窗口,_unpopup关闭弹出窗口
                        m_bOpen[i] = 0;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 移动视频窗口
        /// </summary>
        /// <param name="rect"></param>
        public void MoveTo(Rectangle rect)
        {
            if (!isCreateForm) return;

            MoveWindow(m_hVideoWnd[0], rect.X, rect.Y + 60, rect.Width, rect.Height, 1);
            MoveWindow(m_hVideoWnd[1], rect.X, rect.Y + 65 + rect.Height, rect.Width, rect.Height, 1);
        }

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

        //发送文字。参数：用户ID，要发送的文字,不换行,最多100个字符。
        [DllImport("NCVideoChat.dll")]
        public static extern void NNVSendTextTo(String strUser, String strText);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("User32.dll ")]
        public static extern int GetWindowText(IntPtr handle, StringBuilder text, int MaxLen);

        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);
    }
}
