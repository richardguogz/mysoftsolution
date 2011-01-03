namespace MySoft.Net
{
    using System;

    public class NetException : Exception
    {
        public NetException(string err)
            : base(err)
        {
        }
    }
}

