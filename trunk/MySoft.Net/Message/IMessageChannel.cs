namespace MySoft.Net.Message
{
    using System;
    using MySoft.Net;

    public interface IMessageChannel : IDisposable
    {
        event EventMessageReceive Receive;

        event EventRunError RunError;

        void Close();
        void Send(IMessage message);

        IChannel BaseChannel { get; }

        Exception Error { get; }

        IMessage Message { get; }
    }
}

