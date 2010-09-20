using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
    public interface t_Ad : IEntity
    {
        [PrimaryKey]
        int ID { get; }
        string CompanyID { get; set; }
        string AdName { get; set; }
        string AdTitle { get; set; }
        string AdArea { get; set; }
        string AdImgUrl { get; set; }
        string AdUrl { get; set; }
        string AdLogoImgUrl { get; set; }
        string AdLogoUrl { get; set; }
        string AdText { get; set; }
        string AdTextUrl { get; set; }
        bool IsDefault { get; set; }
        DateTime AddTime { get; set; }
        bool IsCommon { get; set; }
    }
}
