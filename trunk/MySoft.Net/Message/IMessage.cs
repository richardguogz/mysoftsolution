namespace MySoft.Net.Message
{
    using System;

    public interface IMessage
    {
        void Load(byte[] bytes);
        byte[] Save();
    }
}

