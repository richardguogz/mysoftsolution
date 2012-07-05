﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MySoft.IoC.Communication
{
    /// <summary>
    /// Socket helper class
    /// </summary>
    internal static class TcpSocketHelper
    {
        /// <summary>
        /// 释放资源，释放资源，设置buffer为null，userToken为null，同时调用 dispose方法 ,同时将e设置为null
        /// </summary>
        /// <param name="e"></param>
        public static void Release(TcpSocketAsyncEventArgs e)
        {
            if (e == null) return;
            if (e.HasQueuing) return;

            try
            {
                //设置为null
                e.AcceptSocket = null;
                e.RemoteEndPoint = null;
                e.UserToken = null;
                e.SetBuffer(null, 0, 0);
                e.BufferList = null;
            }
            catch
            {
            }
            finally
            {
                //清理资源
                e.Pool.Push(e);
            }
        }

        /// <summary>
        /// 释放资源，释放资源，设置buffer为null，userToken为null，同时调用 dispose方法 ,同时将e设置为null
        /// </summary>
        /// <param name="e"></param>
        public static void Dispose(SocketAsyncEventArgs e)
        {
            if (e == null) return;

            try
            {
                //设置为null
                e.AcceptSocket = null;
                e.RemoteEndPoint = null;
                e.UserToken = null;
                e.SetBuffer(null, 0, 0);
                e.BufferList = null;
            }
            catch
            {
            }
            finally
            {
                //清理资源
                e.Dispose();
                e = null;
            }
        }
    }
}