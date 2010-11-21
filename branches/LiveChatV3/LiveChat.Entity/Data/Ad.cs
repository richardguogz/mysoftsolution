using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// ¹ã¸æ
    /// </summary>
    [Serializable]
    public class Ad
    {
        public int ID { get; set; }
        public string CompanyID { get; set; }
        public string AdName { get; set; }
        public string AdTitle { get; set; }
        public string AdArea { get; set; }
        public string AdImgUrl { get; set; }
        public string AdUrl { get; set; }
        public string AdLogoImgUrl { get; set; }
        public string AdLogoUrl { get; set; }
        public string AdText { get; set; }
        public string AdTextUrl { get; set; }
        public DateTime AddTime { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCommon { get; set; }
    }
}
