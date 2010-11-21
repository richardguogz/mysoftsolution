using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_SeatFriendGroup : IEntity
    {
        [PrimaryKey]
        string SeatID { get; set; }
        [PrimaryKey]
        Guid GroupID { get; set; }
        string GroupName { get; set; }
        string Remark { get; set; }
        DateTime AddTime { get; set; }
    }
}
