using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_SeatFriend : IEntity
    {
        [PrimaryKey]
        string SeatID { get; set; }
        [PrimaryKey]
        string FriendID { get; set; }
        DateTime AddTime { get; set; }
    }
}
