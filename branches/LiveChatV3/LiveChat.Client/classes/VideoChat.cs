using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using LiveChat.Entity;

namespace LiveChat.Client
{
    public class VideoChat
    {
        private Seat fromUser;
        private Seat toUser;
        private System.Drawing.Rectangle rect;
        private IntPtr loginPtr;
        private IntPtr parent;

        public VideoChat(Seat fromUser, Seat toUser, IntPtr parent, Rectangle rect)
        {
            this.fromUser = fromUser;
            this.toUser = toUser;
            this.parent = parent;
            this.rect = rect;
        }

        public IntPtr CreateClient()
        {
            this.loginPtr = NNVCreateClient(parent, 1);
            return loginPtr;
        }

        public void DestroyClient()
        {
            NNVDestroyClient();
        }

        public void LoginToServer()
        {
            NNVLoginToServer("www.enen6.com", 9558, fromUser.SeatID, fromUser.Password, 0);
            NNVSetVideoSize(0);
            MoveWindow(loginPtr, rect.X, rect.Y, rect.Width, rect.Height, 1);

            NNVAddUser(parent, toUser.SeatID, 1);
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

        //显示窗口
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        //设置窗口文本
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        //移动窗口
        [DllImport("user32.dll", EntryPoint = "MoveWindow")]
        public static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);
    }
}
