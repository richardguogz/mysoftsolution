using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;
using LiveChat.Entity;

namespace LiveChat.Design
{
    public interface t_User : IEntity
    {
        [PrimaryKey]
        string UserID { get; set; }
        string UserName { get; set; }
        UserType UserType { get; set; }
        DateTime? LastChatTime { get; set; }
        int? ChatCount { get; set; }
    }
}
