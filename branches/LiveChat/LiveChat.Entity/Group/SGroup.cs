using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    [Serializable]
    public class SGroup
    {
        public Guid GroupID { get; set; }
        public string CompanyID { get; set; }
        public string GroupName { get; set; }
        public int? MaxPerson { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
