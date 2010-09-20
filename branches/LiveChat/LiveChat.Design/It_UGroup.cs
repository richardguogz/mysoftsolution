using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_UGroup : IEntity
    {
        [PrimaryKey]
        Guid GroupID { get; set; }
        string CompanyID { get; set; }
        string GroupName { get; set; }
        int? MaxPerson { get; set; }
        DateTime? AddTime { get; set; }
    }
}
