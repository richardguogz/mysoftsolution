namespace MySoft.Net
{
    using System;
    using System.Net;

    public interface IAddressFilter
    {
        bool Filter(EndPoint client);
    }
}

