using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;
using System.Timers;

namespace LiveChat.Service.Manager
{
    public class GroupManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        private static Dictionary<Guid, Group> dictGroup = new Dictionary<Guid, Group>();
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

            IList<Company> companies = CompanyManager.Instance.GetCompanies();
            List<t_UGroup> ugroups = GetUserGroupsFromDB();
            List<t_SGroup> sgroups = GetSeatGroupsFromDB();

            foreach (Company company in companies)
            {
                //加载用户群
                ugroups.FindAll(p => p.CompanyID == company.CompanyID).ForEach(p =>
                {
                    UserGroup ug = new UserGroup(company, p.GroupID);
                    ug.MaxPerson = p.MaxPerson.Value;
                    ug.GroupName = p.GroupName;
                    ug.AddTime = p.AddTime.Value;

                    dictGroup.Add(ug.GroupID, ug);
                });


                //加载客服群
                sgroups.FindAll(p => p.CompanyID == company.CompanyID).ForEach(p =>
                {
                    SeatGroup sg = new SeatGroup(company, p.GroupID);
                    sg.MaxPerson = p.MaxPerson.Value;
                    sg.GroupName = p.GroupName;
                    sg.AddTime = p.AddTime.Value;

                    //加载座席到群里
                    foreach (Seat seat in company.Seats)
                    {
                        sg.Seats.Add(seat);
                    }

                    dictGroup.Add(sg.GroupID, sg);
                });
            }
        }

        /// <summary>
        /// 获取用户群
        /// </summary>
        /// <param name="groupID"></param>
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
        /// 获取客服群
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

        #region IGroupManager 成员

        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <returns></returns>
        public IList<UserGroup> GetUserGroups()
        {
            lock (syncobj)
            {
                IList<UserGroup> list = new List<UserGroup>();
                foreach (Group group in dictGroup.Values)
                {
                    if (group is UserGroup)
                    {
                        list.Add(group as UserGroup);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 获取客服组列表
        /// </summary>
        /// <returns></returns>
        public IList<SeatGroup> GetSeatGroups()
        {
            lock (syncobj)
            {
                IList<SeatGroup> list = new List<SeatGroup>();
                foreach (Group group in dictGroup.Values)
                {
                    if (group is SeatGroup)
                    {
                        list.Add(group as SeatGroup);
                    }
                }

                return list;
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
        /// 获取客服群按公司ID
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<SGroup> GetSeatGroupByCompanyID(string companyID)
        {
            lock (syncobj)
            {
                return dbSession.From<t_SGroup>().Where(t_SGroup._.CompanyID == companyID)
                    .OrderBy(t_SGroup._.AddTime.Asc).ToList()
                    .ConvertTo<SGroup>();
            }
        }


        public int SaveSeatGroup(SGroup group, bool isUpdate)
        {
            lock (syncobj)
            {
                t_SGroup sg = null;
                if (!isUpdate)
                {
                    sg = new t_SGroup()
                    {
                        CompanyID = group.CompanyID,
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        AddTime = group.AddTime
                    };
                }
                else
                {
                    sg = new t_SGroup()
                    {
                        //CompanyID = group.CompanyID,
                        GroupID = group.GroupID,
                        GroupName = group.GroupName,
                        MaxPerson = group.MaxPerson,
                        //AddTime = group.AddTime
                    };
                    sg.Attach();
                }

                int ret = dbSession.Save(sg);
                if (ret > 0)
                {
                    LoadGroup();
                }
                return ret;
            }
        }

        public int DeleteSeatGroup(Guid groupID)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_SGroup>(t_SGroup._.GroupID == groupID);
            }
        }

        #endregion
    }
}
