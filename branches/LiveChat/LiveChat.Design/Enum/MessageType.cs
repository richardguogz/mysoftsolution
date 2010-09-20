using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [Serializable]
    public enum MessageType
    {
        /// <summary>
        /// 系统消息
        /// </summary>
        System,
        /// <summary>
        /// 提示
        /// </summary>
        Tip,
        /// <summary>
        /// 文本消息
        /// </summary>
        Text,
        /// <summary>
        /// 地址信息
        /// </summary>
        Url,
        /// <summary>
        /// 图片信息
        /// </summary>
        Picture,
        /// <summary>
        /// 传文件
        /// </summary>
        File
    }
}
