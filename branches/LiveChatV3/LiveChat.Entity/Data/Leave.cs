using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 留言信息
    /// </summary>
    [Serializable]
    public class Leave
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 提交IP
        /// </summary>
        public string PostIP { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string PostArea { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
