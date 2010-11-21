﻿using System;
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
            List<t_GroupUser> gusers = GetGroupUsersFromDB();
            List<t_GroupSeat> gseats = GetGroupSeatsFromDB();
            groupUsers = gusers;
            groupSeats = gseats;

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
                foreach (var guser in gusers.FindAll(g => g.GroupID == p.GroupID))
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
                foreach (var gseat in gseats.FindAll(g => g.GroupID == p.GroupID))
                {
                    var seat = SeatManager.Instance.GetSeat(gseat.SeatID);
                    sg.Seats.Add(seat);
                }

                dictGroup.Add(sg.GroupID, sg);
            });
        }

        #region IGroupManager 成员

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
                return dbSession.Save(group) > 0;
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

                        groupSeats = GetGroupSeatsFromDB();
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

                    return dbSession.Save(sg);
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

                        groupUsers = GetGroupUsersFromDB();
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
                }

                return dbSession.Save(ug);
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
