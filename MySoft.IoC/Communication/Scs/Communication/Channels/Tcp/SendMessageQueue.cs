﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MySoft.IoC.Communication.Scs.Communication.Messages;

namespace MySoft.IoC.Communication.Scs.Communication.Channels.Tcp
{
    /// <summary>
    /// Send message queue.
    /// </summary>
    internal class SendMessageQueue : IDisposable
    {
        /// <summary>
        /// 用于完成异步操作的事件
        /// </summary>
        public event EventHandler<SocketAsyncEventArgs> Completed;
        public event EventHandler<SocketAsyncEventArgs> Disposed;

        private SocketAsyncEventArgs _sendEventArgs;
        private Socket _clientSocket;
        private Queue<BufferMessage> _msgQueue = new Queue<BufferMessage>();

        /// <summary>
        /// This object is just used for thread synchronizing (locking).
        /// </summary>
        private readonly object _syncLock;
        private bool _isCompleted;

        /// <summary>
        /// 实例化ScsMessageQueue
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="sendEventArgs"></param>
        public SendMessageQueue(Socket clientSocket, SocketAsyncEventArgs sendEventArgs)
        {
            this._clientSocket = clientSocket;
            this._sendEventArgs = sendEventArgs;
            this._syncLock = new object();
            this._isCompleted = true;
        }

        /// <summary>
        /// 发送数据服务
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageBytes"></param>
        public void Send(IScsMessage message, byte[] messageBytes)
        {
            //实例化BufferMessage
            var msg = new BufferMessage(message, messageBytes);

            lock (_syncLock)
            {
                if (_isCompleted)
                {
                    _isCompleted = false;
                    SendAsync(_sendEventArgs, msg);
                }
                else
                {
                    _msgQueue.Enqueue(msg);
                }
            }
        }

        /// <summary>
        /// 发送数据服务
        /// </summary>
        /// <param name="e"></param>
        public void Send(SocketAsyncEventArgs e)
        {
            lock (_syncLock)
            {
                if (_msgQueue.Count == 0)
                {
                    _isCompleted = true;
                }
                else
                {
                    var message = _msgQueue.Dequeue();
                    SendAsync(e, message);
                }
            }
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        private void SendAsync(SocketAsyncEventArgs e, BufferMessage message)
        {
            try
            {
                if (e == null) return;
                if (message == null) return;

                //设置缓冲区
                e.SetBuffer(message.Buffer, 0, message.Buffer.Length);

                e.UserToken = message.Message;

                //开始异步发送
                if (!_clientSocket.SendAsync(e))
                {
                    if (Completed != null)
                    {
                        Completed(_clientSocket, e);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Disposed != null)
                {
                    Disposed(_clientSocket, e);
                }

                throw;
            }
            finally
            {
                message.Dispose();
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// Dispose resource.
        /// </summary>
        public void Dispose()
        {
            if (Disposed != null)
            {
                Disposed(_clientSocket, _sendEventArgs);
            }

            try
            {
                while (_msgQueue.Count > 0)
                {
                    var message = _msgQueue.Dequeue();
                    message.Dispose();
                }
            }
            catch (Exception ex) { }
            finally
            {
                _msgQueue.Clear();

                _clientSocket = null;
                _sendEventArgs = null;
            }
        }

        #endregion
    }
}
