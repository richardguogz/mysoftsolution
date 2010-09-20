using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_Link : IEntity
    {
        [PrimaryKey]
        int LinkID { get; }
        string CompanyID { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        DateTime AddTime { get; set; }
    }
}
