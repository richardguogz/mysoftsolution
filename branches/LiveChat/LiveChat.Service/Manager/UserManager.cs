using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using System.Timers;
using MySoft.Data;
using LiveChat.Utils;
using MySoft.Core;

namespace LiveChat.Service.Manager
{
    public class UserManager
    {
        private IList<User> newUsers;
        private DbSession dbSession;
        private Timer timer;

        private static readonly object syncobj = new object();
        private static Dictionary<string, User> dictUser = new Dictionary<string, User>();
        public static readonly UserManager Instance = new UserManager();

        public UserManager()
        {
            this.newUsers = new List<User>();
            this.dbSession = DataAccess.DbLiveChat;

            this.timer = new Timer();
            this.timer.AutoReset = true;
            this.timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                //保存用户数据
                SaveUser();
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.Message);
            }
            timer.Start();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if (!dictUser.ContainsKey(user.UserID))
            {
                dictUser.Add(user.UserID, user);
                this.newUsers.Add(user);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUser(string userID)
        {
            lock (syncobj)
            {
                if (!dictUser.ContainsKey(userID))
                {
                    return null;
                }
                return dictUser[userID];
            }
        }

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUsers()
        {
            lock (syncobj)
            {
                return new List<User>(dictUser.Values);
            }
        }

        /// <summary>
        /// 修改聊天相关信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="LastChatTime"></param>
        /// <param name="ChatCount"></param>
        /// <returns></returns>
        public bool UpdateChatInfo(string userID, DateTime lastChatTime, int chatCount)
        {
            lock (syncobj)
            {
                if (!dictUser.ContainsKey(userID))
                {
                    return false;
                }

                User user = dictUser[userID];
                WhereClip where = t_User._.UserID == user.UserID;
                bool ret = dbSession.Update<t_User>(new Field[] { t_User._.LastChatTime, t_User._.ChatCount },
                    new object[] { lastChatTime, chatCount }, where) > 0;

                if (ret)
                {
                    if (user.Extend != null)
                    {
                        user.Extend.LastChatTime = lastChatTime;
                        user.Extend.ChatCount = chatCount;
                    }
                }
                return ret;
            }
        }

        #region IUserManager 成员

        /// <summary>
        /// 获取用户群列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public IList<UserGroup> GetGroups(string userID, bool isJoin)
        {
            lock (syncobj)
            {
                User user = GetUser(userID);
                IList<UserGroup> list = new List<UserGroup>();
                foreach (UserGroup group in GroupManager.Instance.GetUserGroups())
                {
                    if (isJoin)
                    {
                        if (group.Users.Exists(user))
                        {
                            list.Add(group);
                        }
                    }
                    else
                    {
                        if (!group.Users.Exists(user))
                        {
                            list.Add(group);
                        }
                    }
                }
                return list;
            }
        }

        #endregion

        #region 实现ITimerTask接口

        /// <summary>
        /// 加载用户
        /// </summary>
        public void LoadUser()
        {
            //清除
            dictUser.Clear();

            List<t_User> list = dbSession.From<t_User>().ToList();
            list.ForEach(delegate(t_User u)
            {
                User user = DataUtils.ConvertType<t_User, User>(u);

                //如果不是匿名用户，则加载扩展信息
                //if (user.UserType != UserType.TempUser)
                //{
                //    E.User u1 = M.UserManager.Instance.GetUser(u.UserID);
                //    user.IsVIP = u1.IsEndow;

                //    UserExtend extend = DataUtils.ConvertType<t_User, UserExtend>(u);
                //    extend.Email = u1.Email;
                //    extend.MyAsset = u1.Money;

                //    user.Extend = extend;
                //}

                dictUser.Add(user.UserID, user);
            });

            //把新用户加载入字典中
            (this.newUsers as List<User>).ForEach(delegate(User user)
            {
                dictUser.Add(user.UserID, user);
            });
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        public void SaveUser()
        {
            if (this.newUsers.Count > 0)
            {
                using (DbTrans trans = dbSession.BeginTrans())
                {
                    try
                    {
                        DbBatch batch = trans.BeginBatch(10);
                        foreach (User user in this.newUsers)
                        {
                            t_User u = DataUtils.ConvertType<User, t_User>(user);

                            //如果不是匿名用户，则还需要从扩展信息中获取
                            if (user.UserType != UserType.TempUser)
                            {
                                u.ChatCount = user.Extend.ChatCount;
                                u.LastChatTime = user.Extend.LastChatTime;
                            }

                            batch.Save<t_User>(u);
                        }

                        IList<MySoftException> exps;
                        batch.Execute(out exps);

                        if (exps.Count > 0)
                        {
                            //将错误写入日志中
                            foreach (Exception exp in exps)
                            {
                                Logger.Instance.WriteLog(exp.Message);
                            }

                            //回滚事务
                            trans.Rollback();
                        }
                        else
                        {
                            //提交事务
                            trans.Commit();

                            //保存成功，则清除新增用户
                            this.newUsers.Clear();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        #endregion
    }
}
