using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;

namespace LiveChat.Service.Manager
{
    public class GroupManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        private static Dictionary<Guid, Group> dictGroup = new Dictionary<Guid, Group>();
        private static List<t_GroupUser> groupUsers = new List<t_GroupUser>();
        private static List<t_GroupSeat> groupSeats = new List<t_GroupSeat>();
        public static readonly GroupManager Instance = new GroupManager();

        /// <summary>
        /// 初始化群
        /// </summary>
        public GroupManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }


        /// <summary>
        /// 加载群
        /// </summary>
        public void LoadGroup()
        {
            //清除
            dictGroup.Clear();

            List<t_UGroup> ugroups = GetUserGroupsFromDB();
            List<t_SGroup> sgroups = GetSeatGroupsFromDB();
            groupUsers = GetGroupUsersFromDB();
            groupSeats = GetGroupSeatsFromDB();

            //加载用户群
            ugroups.ForEach(p =>
            {
                UserGroup ug = new UserGroup(p.GroupID)
                {
                    MaxPerson = p.MaxPerson,
                    GroupName = p.GroupName,
                    CreateID = p.CreateID,
                    ManagerID = p.ManagerID,
                    Notification = p.Notification,
                    Description = p.Description,
                    AddTime = p.AddTime
                };

                //加载用户到群里
                foreach (var guser in groupUsers.FindAll(g => g.GroupID == p.GroupID))
                {
                    var user = UserManager.Instance.GetUser(guser.UserID);
                    ug.Users.Add(user);
                }

                dictGroup.Add(ug.GroupID, ug);
            });


            //加载客服群
            sgroups.ForEach(p =>
            {
                SeatGroup sg = new SeatGroup(p.GroupID)
                {
                    MaxPerson = p.MaxPerson,
                    GroupName = p.GroupName,
                    CreateID = p.CreateID,
                    ManagerID = p.ManagerID,
                    Notification = p.Notification,
                    Description = p.Description,
                    AddTime = p.AddTime
                };

                //加载客服到群里
                foreach (var gseat in groupSeats.FindAll(g => g.GroupID == p.GroupID))
                {
                    var seat = SeatManager.Instance.GetSeat(gseat.SeatID);
                    sg.Seats.Add(seat);
                }

                dictGroup.Add(sg.GroupID, sg);
            });
        }

        #region IGroupManager 成员

        /// <summary>
        /// 获取用户未加入的群
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<UserGroup> GetUserNoJoinGroups(string userID)
        {
            lock (syncobj)
            {
                var ugroups = groupUsers.FindAll(p => p.UserID != userID);
                IList<Guid> guidList = new List<Guid>();
                foreach (var group in ugroups)
                {
                    if (!guidList.Contains(group.GroupID))
                    {
                        guidList.Add(group.GroupID);
                    }
                }

                IList<UserGroup> list = new List<UserGroup>();
                foreach (var guid in guidList)
                {
                    if (dictGroup.ContainsKey(guid))
                    {
                        var g = dictGroup[guid] as UserGroup;
                        list.Add(g);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 获取客服未加入的群
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<SeatGroup> GetSeatNoJoinGroups(string seatID)
        {
            lock (syncobj)
            {
                var sgroups = groupSeats.FindAll(p => p.SeatID != seatID);
                IList<Guid> guidList = new List<Guid>();
                foreach (var group in sgroups)
                {
                    if (!guidList.Contains(group.GroupID))
                    {
                        guidList.Add(group.GroupID);
                    }
                }

                IList<SeatGroup> list = new List<SeatGroup>();
                foreach (var guid in guidList)
                {
                    if (dictGroup.ContainsKey(guid))
                    {
                        var g = dictGroup[guid] as SeatGroup;
                        list.Add(g);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        public IList<UserGroup> GetUserGroups(string userID)
        {
            lock (syncobj)
            {
                var ugroups = groupUsers.FindAll(p => p.UserID == userID);
                IList<UserGroup> list = new List<UserGroup>();
                foreach (var group in ugroups)
                {
                    if (dictGroup.ContainsKey(group.GroupID))
                    {
                        var g = dictGroup[group.GroupID] as UserGroup;
                        g.PersonCount = g.Users.Count;
                        g.PersonOnlineCount = g.Users.FindAll(p => p.State != OnlineState.Offline).Count;
                        g.MemoName = group.MemoName;
                        list.Add(g);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 获取客服组列表
        /// </summary>
        /// <returns></returns>
        public IList<SeatGroup> GetSeatGroups(string seatID)
        {
            lock (syncobj)
            {
                var sgroups = groupSeats.FindAll(p => p.SeatID == seatID);
                IList<SeatGroup> list = new List<SeatGroup>();
                foreach (var group in sgroups)
                {
                    if (dictGroup.ContainsKey(group.GroupID))
                    {
                        var g = dictGroup[group.GroupID] as SeatGroup;
                        g.PersonCount = g.Seats.Count;
                        g.PersonOnlineCount = g.Seats.FindAll(p => p.State != OnlineState.Offline).Count;
                        g.MemoName = group.MemoName;
                        list.Add(g);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool UpdateSeatGroup(t_GroupSeat group)
        {
            lock (syncobj)
            {
                group.AttachSet(t_GroupSeat._.MemoName);
                bool ret = dbSession.Save(group) > 0;

                if (ret)
                {
                    var item = groupSeats.Find(p => p.SeatID == group.SeatID && p.GroupID == group.GroupID);
                    if (item != null) item.MemoName = group.MemoName;
                }

                return ret;
            }
        }

        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool UpdateUserGroup(t_GroupUser group)
        {
            lock (syncobj)
            {
                group.AttachSet(t_GroupUser._.MemoName);
                bool ret = dbSession.Save(group) > 0;

                if (ret)
                {
                    var item = groupUsers.Find(p => p.UserID == group.UserID && p.GroupID == group.GroupID);
                    if (item != null) item.MemoName = group.MemoName;
                }

                return ret;
            }
        }

        /// <summary>
        /// 获取我的用户群
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserGroup GetUserGroup(Guid groupID)
        {
            lock (syncobj)
            {
                if (!dictGroup.ContainsKey(groupID))
                {
                    return null;
                }
                return dictGroup[groupID] as UserGroup;
            }
        }

        /// <summary>
        /// 获取我的客服群
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public SeatGroup GetSeatGroup(Guid groupID)
        {
            lock (syncobj)
            {
                if (!dictGroup.ContainsKey(groupID))
                {
                    return null;
                }
                return dictGroup[groupID] as SeatGroup;
            }
        }

        /// <summary>
        /// 退出客服群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool ExitSeatGroup(string seatID, Guid groupID)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_GroupSeat>(t_GroupSeat._.SeatID == seatID
                    && t_GroupSeat._.GroupID == groupID) > 0;
            }
        }

        /// <summary>
        /// 退出用户群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool ExitUserGroup(string userID, Guid groupID)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_GroupUser>(t_GroupUser._.UserID == userID
                    && t_GroupUser._.GroupID == groupID) > 0;
            }
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        public void DismissSeatGroup(string seatID, Guid groupID)
        {
            lock (syncobj)
            {
                using (var trans = dbSession.BeginTrans())
                {
                    try
                    {
                        var group = GetSeatGroup(groupID);
                        if (group.CreateID == seatID)
                        {
                            dbSession.Delete<t_SGroup>(t_SGroup._.GroupID == groupID);
                            dbSession.Delete<t_GroupSeat>(t_GroupSeat._.GroupID == groupID);

                            trans.Commit();

                            if (dictGroup.ContainsKey(groupID))
                                dictGroup.Remove(groupID);
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        public void DismissUserGroup(string userID, Guid groupID)
        {
            lock (syncobj)
            {
                using (var trans = dbSession.BeginTrans())
                {
                    try
                    {
                        var group = GetUserGroup(groupID);
                        if (group.CreateID == userID)
                        {
                            dbSession.Delete<t_UGroup>(t_UGroup._.GroupID == groupID);
                            dbSession.Delete<t_GroupUser>(t_GroupUser._.GroupID == groupID);

                            trans.Commit();

                            if (dictGroup.ContainsKey(groupID))
                                dictGroup.Remove(groupID);
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// 加入客服群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool JoinSeatGroup(string seatID, Guid groupID)
        {
            lock (syncobj)
            {
                //把自己添加到群中
                t_GroupSeat gs = new t_GroupSeat()
                {
                    SeatID = seatID,
                    GroupID = groupID,
                    AddTime = DateTime.Now
                };

                bool ret = dbSession.Save(gs) > 0;
                if (ret)
                {
                    groupSeats.Add(gs);
                }

                return ret;
            }
        }

        /// <summary>
        /// 加入用户群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool JoinUserGroup(string userID, Guid groupID)
        {
            lock (syncobj)
            {
                //把自己添加到群中
                t_GroupUser gu = new t_GroupUser()
                {
                    UserID = userID,
                    GroupID = groupID,
                    AddTime = DateTime.Now
                };

                bool ret = dbSession.Save(gu) > 0;
                if (ret)
                {
                    groupUsers.Add(gu);
                }

                return ret;
            }
        }

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <returns></returns>
        private List<t_UGroup> GetUserGroupsFromDB()
        {
            lock (syncobj)
            {
                return dbSession.From<t_UGroup>().OrderBy(t_UGroup._.AddTime.Asc).ToList();
            }
        }

        /// <summary>
        /// 获取客服组
        /// </summary>
        /// <returns></returns>
        private List<t_SGroup> GetSeatGroupsFromDB()
        {
            lock (syncobj)
            {
                return dbSession.From<t_SGroup>().OrderBy(t_SGroup._.AddTime.Asc).ToList();
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        private List<t_GroupUser> GetGroupUsersFromDB()
        {
            lock (syncobj)
            {
                return dbSession.From<t_GroupUser>().OrderBy(t_GroupUser._.AddTime.Asc).ToList();
            }
        }

        /// <summary>
        /// 获取客服列表
        /// </summary>
        /// <returns></returns>
        private List<t_GroupSeat> GetGroupSeatsFromDB()
        {
            lock (syncobj)
            {
                return dbSession.From<t_GroupSeat>().OrderBy(t_GroupSeat._.AddTime.Asc).ToList();
            }
        }

        public int SaveSeatGroup(t_SGroup group, bool isUpdate)
        {
            lock (syncobj)
            {
                t_SGroup sg = null;
                if (!isUpdate)
                {
                    sg = new t_SGroup()
                    {
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        CreateID = group.CreateID,
                        ManagerID = group.ManagerID,
                        Description = group.Description,
                        Notification = group.Notification,
                        AddTime = group.AddTime
                    };

                    int ret = 0;
                    using (var trans = dbSession.BeginTrans())
                    {
                        try
                        {
                            ret = trans.Save(sg);

                            //把自己添加到群中
                            t_GroupSeat gs = new t_GroupSeat()
                            {
                                SeatID = group.CreateID,
                                GroupID = group.GroupID,
                                AddTime = DateTime.Now
                            };

                            ret += trans.Save(gs);

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    }

                    if (ret > 0)
                    {
                        SeatGroup ssg = new SeatGroup(sg.GroupID)
                        {
                            MaxPerson = sg.MaxPerson,
                            GroupName = sg.GroupName,
                            CreateID = sg.CreateID,
                            ManagerID = sg.ManagerID,
                            Notification = sg.Notification,
                            Description = sg.Description,
                            AddTime = sg.AddTime
                        };

                        var seat = SeatManager.Instance.GetSeat(ssg.CreateID);
                        ssg.Seats.Add(seat);

                        dictGroup.Add(ssg.GroupID, ssg);

                        //添加到客服列表中
                        t_GroupSeat ts = new t_GroupSeat()
                        {
                            SeatID = ssg.CreateID,
                            GroupID = ssg.GroupID,
                            AddTime = ssg.AddTime
                        };

                        if (!groupSeats.Exists(p => p.SeatID == ts.SeatID && p.GroupID == ts.GroupID))
                        {
                            groupSeats.Add(ts);
                        }
                    }

                    return ret;
                }
                else
                {
                    sg = new t_SGroup()
                    {
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        Description = group.Description,
                        Notification = group.Notification
                        //AddTime = group.AddTime
                    };
                    sg.Attach();

                    int ret = dbSession.Save(sg);

                    if (ret > 0)
                    {
                        var g = dictGroup[sg.GroupID];
                        g.GroupName = group.GroupName;
                        g.MaxPerson = group.MaxPerson;
                        g.Description = group.Description;
                        dictGroup[sg.GroupID].Notification = group.Notification;
                    }

                    return ret;
                }
            }
        }

        public int SaveUserGroup(t_UGroup group, bool isUpdate)
        {
            lock (syncobj)
            {
                t_UGroup ug = null;
                if (!isUpdate)
                {
                    ug = new t_UGroup()
                    {
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        CreateID = group.CreateID,
                        ManagerID = group.ManagerID,
                        Description = group.Description,
                        Notification = group.Notification,
                        AddTime = group.AddTime
                    };

                    int ret = 0;
                    using (var trans = dbSession.BeginTrans())
                    {
                        try
                        {
                            ret = trans.Save(ug);

                            //把自己添加到群中
                            t_GroupUser gu = new t_GroupUser()
                            {
                                UserID = group.CreateID,
                                GroupID = group.GroupID,
                                AddTime = DateTime.Now
                            };

                            ret += trans.Save(gu);

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    }

                    if (ret > 0)
                    {
                        UserGroup uug = new UserGroup(ug.GroupID)
                        {
                            MaxPerson = ug.MaxPerson,
                            GroupName = ug.GroupName,
                            CreateID = ug.CreateID,
                            ManagerID = ug.ManagerID,
                            Notification = ug.Notification,
                            Description = ug.Description,
                            AddTime = ug.AddTime
                        };

                        var user = UserManager.Instance.GetUser(uug.CreateID);
                        uug.Users.Add(user);

                        dictGroup.Add(uug.GroupID, uug);

                        //添加到用户列表中
                        t_GroupUser tu = new t_GroupUser()
                        {
                            UserID = uug.CreateID,
                            GroupID = uug.GroupID,
                            AddTime = uug.AddTime
                        };

                        if (!groupUsers.Exists(p => p.UserID == tu.UserID && p.GroupID == tu.GroupID))
                        {
                            groupUsers.Add(tu);
                        }
                    }

                    return ret;
                }
                else
                {
                    ug = new t_UGroup()
                    {
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        Description = group.Description,
                        Notification = group.Notification
                        //AddTime = group.AddTime
                    };
                    ug.Attach();

                    int ret = dbSession.Save(ug);

                    if (ret > 0)
                    {
                        var g = dictGroup[ug.GroupID];
                        g.GroupName = group.GroupName;
                        g.MaxPerson = group.MaxPerson;
                        g.Description = group.Description;
                        g.Notification = group.Notification;
                    }

                    return ret;
                }
            }
        }

        /// <summary>
        /// 删除客服群
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int DeleteSeatGroup(Guid groupID)
        {
            lock (syncobj)
            {
                int ret = 0;
                using (DbTrans trans = dbSession.BeginTrans())
                {
                    try
                    {
                        ret = trans.Delete<t_GroupSeat>(t_GroupSeat._.GroupID == groupID);
                        ret += trans.Delete<t_SGroup>(t_SGroup._.GroupID == groupID);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }

                    return ret;
                }
            }
        }

        /// <summary>
        /// 删除用户群
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int DeleteUserGroup(Guid groupID)
        {
            lock (syncobj)
            {
                int ret = 0;
                using (DbTrans trans = dbSession.BeginTrans())
                {
                    try
                    {
                        ret = trans.Delete<t_GroupUser>(t_GroupUser._.GroupID == groupID);
                        ret += trans.Delete<t_UGroup>(t_UGroup._.GroupID == groupID);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }

                    return ret;
                }
            }
        }

        #endregion
    }
}
