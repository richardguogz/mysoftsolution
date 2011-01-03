namespace MySoft.Net.Message
{

    public interface IMessage
    {
        void Load(byte[] bytes);
        byte[] Save();
    }
}

