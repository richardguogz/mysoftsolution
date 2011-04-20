using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using LiveChat.Entity;
using System.Windows.Forms;
using System.Threading;

namespace LiveChat.Client
{
    /// <summary>
    /// 视频聊天类
    /// </summary>
    public class VideoChat
    {
        Int32 m_VideoCount = 2;
        Seat[] m_VideoUser = new Seat[2];
        IntPtr[] m_hVideoWnd = new IntPtr[2];	//6个视频窗口
        IntPtr m_hParentWnd = IntPtr.Zero;
        Int32[] m_bConnect = new Int32[2];
        Int32[] m_bOpen = new Int32[2];
        bool isOnline = false;
        bool isConnected = false;
        bool isCreateForm = false;
        bool isRequest = false;

        public VideoChat(Seat firstUser)
        {
            this.m_VideoUser[0] = firstUser;
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetUserName(Seat user)
        {
            if (user == null) return "0000";
            return string.Format("{0}+{1}", user.CompanyID, user.SeatCode);
        }

        /// <summary>
        /// 登录到服务器
        /// </summary>
        public void LoginToServer()
        {
            NNVLoginToServer(ClientConfig.VideoChatUrl, 9558, GetUserName(m_VideoUser[0]), m_VideoUser[0].Password, 0);
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="hParent"></param>
        /// <param name="url"></param>
        public void CreateClient(IntPtr hParent, string url)
        {
            if (m_hVideoWnd[0] == IntPtr.Zero)
            {
                this.m_hVideoWnd[0] = NNVCreateClient(hParent, 0);
                SetWindowText(m_hVideoWnd[0], m_VideoUser[0].SeatName);
                NNVSetVideoSize(0);

                SetWindowText(m_hVideoWnd[0], "/video.gif/" + url);
            }

            this.isCreateForm = true;
        }

        /// <summary>
        /// 退出视频
        /// </summary>
        public void ExitVideo(IntPtr hParent, IntPtr hWnd)
        {
            if (m_hParentWnd == hWnd)
            {
                isRequest = false;

                if (m_hVideoWnd[0] != IntPtr.Zero)
                {
                    SetParent(m_hVideoWnd[0], hParent);
                    ShowWindow(m_hVideoWnd[0], 0);
                }

                if (m_hVideoWnd[1] != IntPtr.Zero)
                {
                    NNVRemoveUser(GetUserName(m_VideoUser[1]));
                    ShowWindow(m_hVideoWnd[1], 0);

                    m_hVideoWnd[1] = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// 获取聊天对象
        /// </summary>
        /// <returns></returns>
        public string GetChatName()
        {
            if (isConnected)
            {
                return m_VideoUser[1].SeatName;
            }

            return null;
        }

        /// <summary>
        /// 关闭视频
        /// </summary>
        public void CloseVideo(IntPtr hWnd)
        {
            if (m_hParentWnd == hWnd)
            {
                SetConnected(false);

                SendText(GetUserName(m_VideoUser[1]), "_CloseVideo");
            }
        }

        /// <summary>
        /// 销毁客户端
        /// </summary>
        public void DestroyClient()
        {
            NNVDestroyClient();
        }

        /// <summary>
        /// 发送视频请求
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hParent"></param>
        /// <param name="user"></param>
        /// <param name="isVideo"></param>
        /// <returns></returns>
        public bool SendRequest(IntPtr hWnd, IntPtr hParent, Seat user, bool isVideo, string url)
        {
            if (!isOnline)
            {
                ClientUtils.ShowMessage("您当前视频服务不可用，不能与对方建立链接！");
                return false;
            }

            m_hParentWnd = hParent;

            if (m_hVideoWnd[0] != IntPtr.Zero)
            {
                SetParent(m_hVideoWnd[0], hParent);
                //ShowWindow(m_hVideoWnd[0], 5);
            }

            if (m_VideoUser[1] != null && m_hVideoWnd[1] != IntPtr.Zero)
            {
                SetParent(m_hVideoWnd[1], hParent);
                //ShowWindow(m_hVideoWnd[1], 5);
            }
            else
            {
                this.m_VideoUser[1] = user;

                m_hVideoWnd[1] = NNVAddUser(hWnd, GetUserName(m_VideoUser[1]), 1);
                SetWindowText(m_hVideoWnd[1], m_VideoUser[1].SeatName);

                //设置视频中间图像信息
                SetWindowText(m_hVideoWnd[1], "/video.gif/" + url);

                SetParent(m_hVideoWnd[1], hParent);
                //ShowWindow(m_hVideoWnd[1], 5);
            }

            if (isVideo)
            {
                //发送_ReqV表示请求视频
                NNVSendTextTo(GetUserName(m_VideoUser[1]), "_ReqV");
            }
            else
            {
                //发送_ReqNV表示请求语音
                NNVSendTextTo(GetUserName(m_VideoUser[1]), "_ReqNV");
            }

            isRequest = true;

            return true;
        }

        /// <summary>
        /// 接收请求并打开视频
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hParent"></param>
        /// <param name="user"></param>
        public void ReceiveRequest(IntPtr hWnd, IntPtr hParent, Seat user, string url)
        {
            m_hParentWnd = hParent;

            if (m_hVideoWnd[0] != IntPtr.Zero)
            {
                SetParent(m_hVideoWnd[0], hParent);
                //ShowWindow(m_hVideoWnd[0], 5);
            }

            if (m_VideoUser[1] != null && m_hVideoWnd[1] != IntPtr.Zero)
            {
                SetParent(m_hVideoWnd[1], hParent);
                //ShowWindow(m_hVideoWnd[1], 5);
            }
            else
            {
                this.m_VideoUser[1] = user;

                m_hVideoWnd[1] = NNVAddUser(hWnd, GetUserName(m_VideoUser[1]), 1);

                //设置视频中间图像信息
                SetWindowText(m_hVideoWnd[1], "/video.gif/" + url);

                SetWindowText(m_hVideoWnd[1], m_VideoUser[1].SeatName);

                SetParent(m_hVideoWnd[1], hParent);
                //ShowWindow(m_hVideoWnd[1], 5);
            }

            //打开视频给对方看
            //NNVOpenVideoTo(GetUserName(m_VideoUser[1]));
        }

        /// <summary>
        /// 设置是否在线
        /// </summary>
        /// <param name="online"></param>
        public void SetOnline(bool online)
        {
            this.isOnline = online;
        }

        /// <summary>
        /// 设置是否连接
        /// </summary>
        /// <param name="connected"></param>
        public void SetConnected(bool connected)
        {
            this.isConnected = connected;
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return isConnected || isRequest;
            }
        }

        /// <summary>
        /// 打开视频会话
        /// </summary>
        /// <param name="hWnd"></param>
        public void OpenVideo(IntPtr hWnd)
        {
            for (int i = 0; i < m_VideoCount; i++)
            {
                if (m_hVideoWnd[i] == hWnd)
                {
                    if (m_bConnect[i] == 0)
                    {
                        NNVOpenVideoTo(GetUserName(m_VideoUser[i]));
                        m_bConnect[i] = 1;
                    }
                    else
                    {
                        NNVCloseVideoTo(GetUserName(m_VideoUser[i]));
                        m_bConnect[i] = 0;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 打开视频会话
        /// </summary>
        /// <param name="strUser"></param>
        public void OpenVideoTo(string strUser)
        {
            try
            {
                NNVOpenVideoTo(strUser);
            }
            catch { }
        }

        private string strUser;
        private bool isVideo;

        /// <summary>
        /// 设置视频用户
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="isVideo"></param>
        public void SetVideoUser(string strUser, bool isVideo)
        {
            this.strUser = strUser;
            this.isVideo = isVideo;
        }

        /// <summary>
        /// 打开视频
        /// </summary>
        public void OpenVideo()
        {
            if (isConnected)
            {
                //如果不是视频，则关闭视频
                if (!isVideo) CloseVideo();

                OpenVideoTo(strUser);
            }
        }

        /// <summary>
        /// 关闭视频
        /// </summary>
        public void CloseVideo()
        {
            NNVSetVideoDevice(100);
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

        /// <summary>
        /// 打开大视频
        /// </summary>
        /// <param name="hWnd"></param>
        public void OpenLargeVideo(IntPtr hWnd)
        {
            for (int i = 0; i < m_VideoCount; i++)
            {
                if (m_hVideoWnd[i] == hWnd)
                {
                    NNVSendTextTo(GetUserName(m_VideoUser[i]), "_popup"); //_popup弹出窗口,_unpopup关闭弹出窗口
                }
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
                        NNVSendTextTo(GetUserName(m_VideoUser[i]), "_popup"); //_popup弹出窗口,_unpopup关闭弹出窗口
                        m_bOpen[i] = 1;
                    }
                    else
                    {
                        NNVSendTextTo(GetUserName(m_VideoUser[i]), "_unpopup"); //_popup弹出窗口,_unpopup关闭弹出窗口
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
        public void MoveVideoTo(Rectangle rect1, Rectangle rect2)
        {
            if (!isCreateForm) return;

            if (m_hVideoWnd[0] != IntPtr.Zero)
            {
                MoveWindow(m_hVideoWnd[0], rect1.X, rect1.Y, rect1.Width, rect1.Height, 1);
                ShowWindow(m_hVideoWnd[0], 5);
            }

            if (m_hVideoWnd[1] != IntPtr.Zero)
            {
                MoveWindow(m_hVideoWnd[1], rect2.X, rect2.Y, rect2.Width, rect2.Height, 1);
                ShowWindow(m_hVideoWnd[1], 5);
            }
        }

        //1.创建客户端,创建后可显示本地视频,可做相应窗口设置.参数:父窗口,是否启动单独的EXE运行;返回窗口句柄.
        //相当于初始化,必须先调用此接口,后面接口才能使用.
        [DllImport("NCVideoChat.dll")]
        private static extern IntPtr NNVCreateClient(IntPtr Int32Parent, Int32 bNewExe);

        //2.登陆服务器后可与其他人连接.参数:服务器IP,端口,用户名,密码.最后一个参数暂时不使用.
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVLoginToServer(String strIP, Int32 nPort, String strUser, String strPwd, Int32 dwData);

        //3.添加用户到本房间.某用户登陆本房间时,参数:父窗口,用户帐号,是否自动开启视频音频.返回视频窗口句柄.
        [DllImport("NCVideoChat.dll")]
        private static extern IntPtr NNVAddUser(IntPtr hParent, String strUser, Int32 bOpen);

        //4.打开视频给对方收看.参数:对方帐号
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVOpenVideoTo(String strUser);

        //5.关闭视频不给对方收看.参数:对方帐号.
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVCloseVideoTo(String strUser);

        //6.删除某用户.某用户退出本房间时
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVRemoveUser(String strUser);

        //7.销毁客户端.软件退出.
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVDestroyClient();

        //设置大视频还是小视频
        [DllImport("NCVideoChat.dll")]
        private static extern Int32 NNVSetVideoSize(Int32 nSize);

        //设置本地视频参数
        [DllImport("NCVideoChat.dll")]
        private static extern Int32 NNVSetVideoDevice(Int32 nSize);

        //发送文字。参数：用户ID，要发送的文字,不换行,最多100个字符。
        [DllImport("NCVideoChat.dll")]
        private static extern void NNVSendTextTo(String strUser, String strText);

        //显示窗口方法，0-隐藏窗口 5-显示窗口
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        //设置窗口标题方法
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        //获取窗口文本方法
        [DllImport("user32.dll ")]
        public static extern int GetWindowText(IntPtr handle, StringBuilder text, int MaxLen);

        //移动窗口方法
        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        //设置父窗口
        [DllImport("user32.dll ", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        //闪动屏幕
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);
    }
}
