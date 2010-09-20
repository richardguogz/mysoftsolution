using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_Area : IEntity
    {
        [PrimaryKey]
        int ID { get; set; }
        string AreaID { get; }
        string AreaName { get; set; }
        string ParentID { get; set; }
        DateTime? AddTime { get; set; }
    }
}
