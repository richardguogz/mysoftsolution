namespace MySoft.Net
{
    using System;

    public interface IChannel
    {
        event EventChannelClose Closed;

        event EventChannelReceive Receive;

        event EventRunError RunError;

        void Close();
        void Send(byte[] bytes);

        byte[] Data { get; }

        Exception Error { get; }

        string ID { get; set; }

        string Name { get; set; }

        int ReceiveSize { get; set; }

        System.Net.Sockets.Socket Socket { get; }

        bool Sureness { get; set; }
    }
}

