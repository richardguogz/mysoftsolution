using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_SGroup : IEntity
    {
        [PrimaryKey]
        Guid GroupID { get; set; }
        string CreateID { get; set; }
        string ManagerID { get; set; }
        string GroupName { get; set; }
        int MaxPerson { get; set; }
        DateTime AddTime { get; set; }
        string Description { get; set; }
        string Notification { get; set; }
    }
}
