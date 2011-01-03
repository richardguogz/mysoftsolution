namespace MySoft.Net
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class NetListener
    {
        private TcpListener _Listener;
        private Thread _ListenerThread;
        private bool IsStart;
        private ClientPool mPool;
        private int mReceiveSize;
        private static ManualResetEvent re = new ManualResetEvent(false);

        public event EventClientPoolChange Accepted;

        public event EventChannelReceive Receive;

        public event EventClientPoolChange Released;

        public event EventRunError RunError;

        public NetListener(IPEndPoint point)
        {
            this.mReceiveSize = 0x400;
            this._ListenerThread = null;
            this.mPool = new ClientPool();
            this.IsStart = false;
            this._Listener = new TcpListener(point);
        }

        public NetListener(IPEndPoint point, int receivesize)
        {
            this.mReceiveSize = 0x400;
            this._ListenerThread = null;
            this.mPool = new ClientPool();
            this.IsStart = false;
            this._Listener = new TcpListener(point);
            this.ReceiveSize = receivesize;
        }

        private void ClientClose(EventChannelArgs e)
        {
            this.Pool.Remove(e.Channel);
            if (this.Released != null)
            {
                this.Released(this, new EventChannelArgs(e.Channel));
            }
        }

        private static void ConnectCall(IAsyncResult ar)
        {
            ((Socket)ar.AsyncState).EndConnect(ar);
            re.Set();
        }

        public static IChannel CreateChannel(IPEndPoint ip)
        {
            return CreateChannel(ip, 0x400);
        }

        public static IChannel CreateChannel(IPEndPoint ip, int receivesize)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip);
            if (socket.Connected)
            {
                return new Channel(socket, receivesize);
            }
            return null;
        }

        private void OnExecuteError(object source, EventRunErrorArgs e)
        {
            if (this.RunError != null)
            {
                this.RunError(this, e);
            }
        }

        private void OnReceive(object source, EventChannelReceiveArgs e)
        {
            if (this.Receive != null)
            {
                this.Receive(source, e);
            }
        }

        private void OnStart()
        {
            this._Listener.Start();
            Exception exception = null;
            IChannel msg = null;
            while (this.IsStart)
            {
                try
                {
                    msg = new Channel(this._Listener.AcceptSocket(), this.ReceiveSize);
                    this.Pool.Add(msg);
                    msg.Closed += new EventChannelClose(this.ClientClose);
                    msg.Receive += new EventChannelReceive(this.OnReceive);
                    msg.RunError += new EventRunError(this.OnExecuteError);
                    if (this.Accepted != null)
                    {
                        this.Accepted(this, new EventChannelArgs(msg));
                    }
                }
                catch (Exception exception2)
                {
                    if (this.IsStart)
                    {
                        exception = exception2;
                    }
                }
                if (exception != null)
                {
                    EventRunErrorArgs e = new EventRunErrorArgs(msg);
                    e.Error = exception;
                    this.OnExecuteError(this, e);
                }
            }
        }

        public void Start()
        {
            this.IsStart = true;
            this._ListenerThread = new Thread(new ThreadStart(this.OnStart));
            this._ListenerThread.Start();
        }

        public void Stop()
        {
            this.IsStart = false;
            this._Listener.Stop();
            this._ListenerThread.Abort();
            this.Pool.Clear();
        }

        public ClientPool Pool
        {
            get
            {
                return this.mPool;
            }
        }

        public int ReceiveSize
        {
            get
            {
                return this.mReceiveSize;
            }
            set
            {
                this.mReceiveSize = value;
            }
        }
    }
}

