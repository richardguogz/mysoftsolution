namespace MySoft.Net.Message
{
    using MySoft.Net;
    using System;
    using System.Runtime.CompilerServices;

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

