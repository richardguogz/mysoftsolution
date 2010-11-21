using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    [Serializable]
    public class SGroup
    {
        /// <summary>
        /// 群ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateID { get; set; }
        /// <summary>
        /// 管理者
        /// </summary>
        public string ManagerID { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 群人数
        /// </summary>
        public int MaxPerson { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 群描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 群公告
        /// </summary>
        public string Notification { get; set; }
    }
}
