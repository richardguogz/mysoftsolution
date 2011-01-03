namespace MySoft.Net
{
    using System;
    using System.Collections;
    using System.Net.Sockets;

    public class Channel : IChannel
    {
        private Hashtable _TEMPDATA;
        private Exception mError;
        private byte[] mData;
        private string mID;
        private string mName;
        private int mReceiveSize;
        private bool mSureness;
        protected Socket mSocket;

        public event EventChannelClose Closed;

        public event EventChannelReceive Receive;

        public event EventRunError RunError;

        public Channel(System.Net.Sockets.Socket socket)
        {
            this.mReceiveSize = 0x400;
            this.mSocket = null;
            this.mName = "";
            this.mSureness = false;
            this.mID = Guid.NewGuid().ToString();
            this.mError = null;
            this._TEMPDATA = new Hashtable();
            this.mSocket = socket;
            this.OnReceive();
        }

        public Channel(System.Net.Sockets.Socket socket, int receivesize)
        {
            this.mReceiveSize = 0x400;
            this.mSocket = null;
            this.mName = "";
            this.mSureness = false;
            this.mID = Guid.NewGuid().ToString();
            this.mError = null;
            this._TEMPDATA = new Hashtable();
            this.mSocket = socket;
            this.ReceiveSize = receivesize;
            this.OnReceive();
        }

        public virtual void Close()
        {
            if (this.mSocket != null)
            {
                if (this.Closed != null)
                {
                    this.Closed(new EventChannelArgs(this));
                }
                this.Socket.Shutdown(SocketShutdown.Both);
                this.Socket.Close();
                this.mSocket = null;
            }
            this._TEMPDATA.Clear();
        }

        protected virtual void OnError(Exception e)
        {
            EventRunErrorArgs args = new EventRunErrorArgs(this);
            args.Error = e;
            if (this.RunError != null)
            {
                this.RunError(this, args);
            }
            this.mError = e;
            if (e is SocketException)
            {
                try
                {
                    this.Close();
                }
                catch
                {
                }
            }
        }

        private void OnReceive()
        {
            try
            {
                ReceiveData state = new ReceiveData(this.ReceiveSize, this.Socket);
                this.mSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCall), state);
            }
            catch (Exception exception)
            {
                this.OnError(exception);
            }
        }

        protected virtual void OnReceive(EventChannelReceiveArgs e)
        {
            if (this.Receive != null)
            {
                this.Receive(this, e);
            }
        }

        protected virtual void OnSend(byte[] bytes)
        {
            if ((bytes != null) && (bytes.Length != 0))
            {
                if (this.Socket == null)
                {
                    throw new NetException("连接断开或不存在!");
                }
                try
                {
                    this.Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.SendCall), this);
                }
                catch (Exception exception)
                {
                    this.OnError(exception);
                }
            }
        }

        protected void ReceiveBuffer(byte[] _reader)
        {
            EventChannelReceiveArgs e;
            if (DataPackage.IsDataPackage(_reader))
            {
                DataPackageCollection packages;
                DataPackage dp = DataPackage.GetDataPackage(_reader);
                if (this._TEMPDATA.Contains(dp.DataID))
                {
                    packages = (DataPackageCollection)this._TEMPDATA[dp.DataID];
                }
                else
                {
                    packages = new DataPackageCollection(dp.Count);
                    this._TEMPDATA.Add(dp.DataID, packages);
                }
                packages.Add(dp);
                if (packages.IsFull)
                {
                    this._TEMPDATA.Remove(packages.ID);
                    _reader = packages.GetData();
                    this.mData = _reader;
                    e = new EventChannelReceiveArgs(this);
                    e.Data = _reader;
                    this.OnReceive(e);
                }
            }
            else
            {
                e = new EventChannelReceiveArgs(this);
                e.Data = _reader;
                this.mData = _reader;
                this.OnReceive(e);
            }
        }

        private void ReceiveCall(IAsyncResult ar)
        {
            ReceiveData asyncState = (ReceiveData)ar.AsyncState;
            try
            {
                int num = asyncState.Socket.EndReceive(ar);
                if (num == 0)
                {
                    this.Close();
                }
                if (num > 0)
                {
                    byte[] buffer = new byte[num];
                    for (int i = 0; i < num; i++)
                    {
                        buffer[i] = asyncState.Buffer[i];
                    }
                    this.ReceiveBuffer(buffer);
                }
                ReceiveData state = new ReceiveData(this.ReceiveSize, asyncState.Socket);
                asyncState.Socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCall), state);
            }
            catch (Exception exception)
            {
                this.OnError(exception);
            }
        }

        public void Send(byte[] bytes)
        {
            if (bytes.Length > this.ReceiveSize)
            {
                DataPackage[] dataPackages = DataPackageCollection.GetDataPackages(bytes, this.ReceiveSize);
                foreach (DataPackage package in dataPackages)
                {
                    this.OnSend(DataPackage.GetData(package));
                }
            }
            else
            {
                this.OnSend(bytes);
            }
        }

        private void SendCall(IAsyncResult ar)
        {
            try
            {
                Channel asyncState = (Channel)ar.AsyncState;
                asyncState.Socket.EndSend(ar);
            }
            catch (Exception exception)
            {
                this.OnError(exception);
            }
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
        }

        public Exception Error
        {
            get
            {
                return this.mError;
            }
        }

        public string ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
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

        public System.Net.Sockets.Socket Socket
        {
            get
            {
                return this.mSocket;
            }
        }

        public bool Sureness
        {
            get
            {
                return this.mSureness;
            }
            set
            {
                this.mSureness = value;
            }
        }

        private class ReceiveData
        {
            private byte[] mBuffer;
            private System.Net.Sockets.Socket mSocket;

            public ReceiveData(int receivesize, System.Net.Sockets.Socket s)
            {
                this.mBuffer = new byte[receivesize];
                this.mSocket = s;
            }

            public byte[] Buffer
            {
                get
                {
                    return this.mBuffer;
                }
            }

            public System.Net.Sockets.Socket Socket
            {
                get
                {
                    return this.mSocket;
                }
            }
        }
    }
}

