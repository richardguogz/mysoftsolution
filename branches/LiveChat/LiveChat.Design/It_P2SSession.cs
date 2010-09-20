using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;
using LiveChat.Entity;

namespace LiveChat.Design
{
    public interface t_P2SSession : IEntity
    {
        [PrimaryKey]
        Guid SID { get; set; }
        string SessionID { get; set; }
        string CreateID { get; set; }
        string UserID { get; set; }
        string SeatID { get; set; }
        string RequestCode { get; set; }
        string FromIP { get; set; }
        string FromAddress { get; set; }
        DateTime? LastReceiveTime { get; set; }
        string RequestMessage { get; set; }
        DateTime StartTime { get; set; }
        DateTime? EndTime { get; set; }
        SessionState State { get; set; }
    }
}
