using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_GroupSeat : IEntity
    {
        [PrimaryKey]
        Guid GroupID { get; set; }
        [PrimaryKey]
        string SeatID { get; set; }
        DateTime AddTime { get; set; }
    }
}
