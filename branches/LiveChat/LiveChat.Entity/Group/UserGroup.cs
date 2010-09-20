using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 用户群
    /// </summary>
    [Serializable]
    public class UserGroup : Group
    {
        [NonSerialized]
        private UserList _Users;
        /// <summary>
        /// 用户列表
        /// </summary>
        public UserList Users
        {
            get
            {
                return _Users;
            }
            set
            {
                _Users = value;
            }
        }

        public UserGroup(Company company, Guid groupID)
            : base(company, groupID)
        {
            this._Users = new UserList();
        }

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Exists(User user)
        {
            return this._Users.Exists(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if (!this._Users.Exists(user))
            {
                this[user.UserID] = DateTime.MinValue;
                this._Users.Add(user);
                this._PersonCount = this._Users.Count;
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(User user)
        {
            if (this._Users.Exists(user))
            {
                this._Users.Remove(user);
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        public override void AddMessage(Message msg)
        {
            msg.SessionID = string.Format("S_{0}", this.GroupID.ToString());
            base.AddMessage(msg);
        }
    }
}
