using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 上传类型
    /// </summary>
    public enum UploadType
    {
        /// <summary>
        /// 用户端
        /// </summary>
        User,
        /// <summary>
        /// 客服端
        /// </summary>
        Seat
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 文件
        /// </summary>
        File
    }
}
