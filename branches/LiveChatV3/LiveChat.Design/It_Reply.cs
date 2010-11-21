using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_Reply : IEntity
    {
        [PrimaryKey]
        int ReplyID { get; }
        string CompanyID { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        DateTime AddTime { get; set; }
    }
}
