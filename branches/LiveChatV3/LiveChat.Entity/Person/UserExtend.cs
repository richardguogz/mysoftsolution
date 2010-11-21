using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户扩展信息
    /// </summary>
    [Serializable]
    public class UserExtend
    {
        private SexType _Sex;
        /// <summary>
        /// 性别
        /// </summary>
        public SexType Sex
        {
            get
            {
                return _Sex;
            }
            set
            {
                _Sex = value;
            }
        }

        private int? _Age;
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age
        {
            get
            {
                return _Age;
            }
            set
            {
                _Age = value;
            }
        }

        private string _Preference;
        /// <summary>
        /// 偏好
        /// </summary>
        public string Preference
        {
            get
            {
                return _Preference;
            }
            set
            {
                _Preference = value;
            }
        }

        private Decimal? _MyAsset;
        /// <summary>
        /// 我的资产
        /// </summary>
        public Decimal? MyAsset
        {
            get
            {
                return _MyAsset;
            }
            set
            {
                _MyAsset = value;
            }
        }

        private string _Email;
        /// <summary>
        /// Email地址
        /// </summary>
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        private DateTime? _LastChatTime;
        /// <summary>
        /// 最后会话时间
        /// </summary>
        public DateTime? LastChatTime
        {
            get
            {
                return _LastChatTime;
            }
            set
            {
                _LastChatTime = value;
            }
        }

        private int? _ChatCount;
        /// <summary>
        /// 聊天次数
        /// </summary>
        public int? ChatCount
        {
            get
            {
                return _ChatCount;
            }
            set
            {
                _ChatCount = value;
            }
        }
    }
}
