using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_GroupUser : IEntity
    {
        [PrimaryKey]
        Guid GroupID { get; set; }
        [PrimaryKey]
        string UserID { get; set; }
        DateTime? AddTime { get; set; }
    }
}
