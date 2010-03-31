namespace MySoft.Net.Message
{
    using MySoft.Net;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class MessageChannel : IMessageChannel, IDisposable
    {
        private IChannel mBaseChannel;

        public event EventMessageReceive Receive;

        public event EventRunError RunError;

        public MessageChannel(IChannel channel)
        {
            this.mBaseChannel = channel;
            this.mBaseChannel.Receive += new EventChannelReceive(this.DataReceive);
            this.mBaseChannel.RunError += new EventRunError(this.OnExecuteError);
        }

        public void Close()
        {
            this.BaseChannel.Close();
        }

        private void DataReceive(object source, EventChannelReceiveArgs e)
        {
            EventMessageReceiveArgs args = new EventMessageReceiveArgs(GetMessage(e.Data), this);
            if (this.Receive != null)
            {
                this.OnReceive(args);
            }
        }

        public void Dispose()
        {
            this.BaseChannel.Receive -= new EventChannelReceive(this.DataReceive);
            this.mBaseChannel.RunError -= new EventRunError(this.OnExecuteError);
        }

        public static IMessage GetMessage(byte[] data)
        {
            int index;
            if ((data == null) || (data.Length == 0))
            {
                return null;
            }
            if (Encoding.ASCII.GetString(data, 0, 0x24) != "8DD25FFF-FFD8-42a9-9640-96F5E7FAA796")
            {
                return null;
            }
            int num = BitConverter.ToInt32(data, 0x24);
            byte[] bytes = new byte[num];
            for (index = 40; index < (num + 40); index++)
            {
                bytes[index - 40] = data[index];
            }
            int num3 = 40 + num;
            byte[] buffer2 = new byte[data.Length - num3];
            for (index = num3; index < data.Length; index++)
            {
                buffer2[index - num3] = data[index];
            }
            IMessage message = (IMessage) Activator.CreateInstance(Type.GetType(Encoding.ASCII.GetString(bytes)));
            message.Load(buffer2);
            return message;
        }

        private void OnExecuteError(object source, EventRunErrorArgs e)
        {
            if (this.RunError != null)
            {
                this.RunError(this, e);
            }
        }

        protected virtual void OnReceive(EventMessageReceiveArgs e)
        {
            if (this.Receive != null)
            {
                this.Receive(this, e);
            }
        }

        private void OnSend(byte[] data)
        {
            this.BaseChannel.Send(data);
        }

        public void Send(IMessage message)
        {
            int length = message.GetType().AssemblyQualifiedName.Length;
            byte[] bytes = Encoding.ASCII.GetBytes(message.GetType().AssemblyQualifiedName);
            byte[] buffer2 = message.Save();
            byte[] array = new byte[(40 + length) + buffer2.Length];
            Encoding.ASCII.GetBytes("8DD25FFF-FFD8-42a9-9640-96F5E7FAA796").CopyTo(array, 0);
            BitConverter.GetBytes(length).CopyTo(array, 0x24);
            bytes.CopyTo(array, 40);
            buffer2.CopyTo(array, (int) (40 + length));
            this.BaseChannel.Send(array);
        }

        public IChannel BaseChannel
        {
            get
            {
                return this.mBaseChannel;
            }
        }

        public Exception Error
        {
            get
            {
                return this.BaseChannel.Error;
            }
        }

        public IMessage Message
        {
            get
            {
                return GetMessage(this.BaseChannel.Data);
            }
        }
    }
}

