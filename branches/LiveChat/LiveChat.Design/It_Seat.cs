using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;
using LiveChat.Entity;

namespace LiveChat.Design
{
    public interface t_Seat : IEntity
    {
        [PrimaryKey]
        string CompanyID { get; set; }
        [PrimaryKey]
        string SeatCode { get; set; }
        string SeatName { get; set; }
        string Password { get; set; }
        string Sign { get; set; }
        string Introduction { get; set; }
        SeatType SeatType { get; set; }
        DateTime AddTime { get; set; }
        DateTime? LoginTime { get; set; }
        DateTime? LogoutTime { get; set; }
        int LoginCount { get; set; }

        //ÐÂÔö×Ö¶Î
        string Telephone { get; set; }
        string MobileNumber { get; set; }
        string Email { get; set; }
        byte[] FaceImage { get; set; }
    }
}
