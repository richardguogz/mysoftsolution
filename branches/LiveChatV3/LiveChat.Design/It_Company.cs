using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_Company : IEntity
    {
        [PrimaryKey]
        int ID { get; }
        string CompanyID { get; set; }
        string CompanyName { get; set; }
        DateTime AddTime { get; set; }

        //ÐÂÔö×Ö¶Î
        string WebSite { get; set; }
        string ChatWebSite { get; set; }
        string CompanyLogo { get; set; }
        bool IsHeadquarters { get; set; }
    }
}
