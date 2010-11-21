using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;
using LiveChat.Entity;

namespace LiveChat.Design
{
    public interface t_P2SMessage : IEntity
    {
        [PrimaryKey]
        Guid ID { get; set; }
        Guid SID { get; set; }
        string SessionID { get; set; }
        string SenderID { get; set; }
        string SenderName { get; set; }
        string ReceiverID { get; set; }
        string ReceiverName { get; set; }
        string SenderIP { get; set; }
        DateTime SendTime { get; set; }
        MessageType Type { get; set; }
        string Content { get; set; }
        MessageState State { get; set; }
    }
}
