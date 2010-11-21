using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Client
{
    public class ClientSession
    {
        public string SessionID
        {
            get;
            set;
        }

        public string CreateID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string SeatName
        {
            get;
            set;
        }

        public string StartTime
        {
            get;
            set;
        }

        public string LastTime
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string From
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }
    }

    public class RequestSession : ClientSession
    {
        public string RequestSeat
        {
            get;
            set;
        }

        public string RequestTime
        {
            get;
            set;
        }
    }
}
