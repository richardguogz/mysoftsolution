using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// µØÇø
    /// </summary>
    [Serializable]
    public class Area
    {
        public int ID { get; set; }
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public string ParentID { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
