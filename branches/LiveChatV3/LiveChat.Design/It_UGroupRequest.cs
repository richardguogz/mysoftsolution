using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_UGroupRequest : IEntity
    {
        [PrimaryKey]
        int RequestID { get; }
        string UserID { get; set; }
        Guid GroupID { get; set; }
        string Request { get; set; }
        string Refuse { get; set; }
        int ConfirmState { get; set; }
        DateTime AddTime { get; set; }
    }
}
