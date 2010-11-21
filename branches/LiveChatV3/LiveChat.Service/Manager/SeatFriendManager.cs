using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;
using LiveChat.Entity;
using LiveChat.Utils;

namespace LiveChat.Service.Manager
{
    public class SeatFriendManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly SeatFriendManager Instance = new SeatFriendManager();

        /// <summary>
        /// 初始化管理
        /// </summary>
        public SeatFriendManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IDictionary<Seat, RequestInfo> GetRequestFriends(string seatID)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriendRequest._.FriendID == seatID && t_SeatFriendRequest._.ConfirmState == 0;
                var list = dbSession.From<t_SeatFriendRequest>().Where(where).ToList();

                var friendlist = list.ConvertAll<KeyValuePair<Seat, RequestInfo>>(p =>
                {
                    var seat = SeatManager.Instance.GetSeat(p.SeatID);

                    if (seat == null) return new KeyValuePair<Seat, RequestInfo>();

                    var request = new RequestInfo()
                    {
                        ID = p.RequestID,
                        ConfirmState = p.ConfirmState,
                        Request = p.Request,
                        Refuse = p.Refuse,
                        AddTime = p.AddTime
                    };

                    return new KeyValuePair<Seat, RequestInfo>(seat, request);
                });

                IDictionary<Seat, RequestInfo> items = new Dictionary<Seat, RequestInfo>();
                foreach (var item in friendlist)
                {
                    items.Add(item);
                }

                return items;
            }
        }

        /// <summary>
        /// 修改备注名称
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool UpdateMemoName(string seatID, string friendID, string name)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.FriendID == friendID;
                return dbSession.Update<t_SeatFriend>(t_SeatFriend._.MemoName, name, where) > 0;
            }
        }

        /// <summary>
        /// 删除好友群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool DeleteFriendGroup(string seatID, Guid groupID)
        {
            lock (syncobj)
            {
                using (DbTrans trans = dbSession.BeginTrans())
                {
                    try
                    {
                        int ret = 0;
                        WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.GroupID == groupID;
                        ret = trans.Update<t_SeatFriend>(t_SeatFriend._.GroupID, null, where);

                        where = t_SeatFriendGroup._.SeatID == seatID && t_SeatFriendGroup._.GroupID == groupID;
                        ret += trans.Delete<t_SeatFriendGroup>(where);

                        trans.Commit();

                        return ret > 0;
                    }
                    catch
                    {
                        trans.Rollback();

                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 添加好友群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool AddFriendGroup(string seatID, SeatFriendGroup group)
        {
            lock (syncobj)
            {
                t_SeatFriendGroup g = DataUtils.ConvertType<SeatFriendGroup, t_SeatFriendGroup>(group);
                g.SeatID = seatID;
                g.AddTime = DateTime.Now;

                return dbSession.Save(g) > 0;
            }
        }

        /// <summary>
        /// 修改好友群名称
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool UpdateFriendGroupName(string seatID, Guid groupID, string groupName)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriendGroup._.SeatID == seatID && t_SeatFriendGroup._.GroupID == groupID;
                return dbSession.Update<t_SeatFriendGroup>(t_SeatFriendGroup._.GroupName, groupName, where) > 0;
            }
        }

        /// <summary>
        /// 更改组信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool UpdateGroupID(string seatID, string friendID, Guid groupID)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.FriendID == friendID;

                if (groupID == Guid.Empty)
                    return dbSession.Update<t_SeatFriend>(t_SeatFriend._.GroupID, null, where) > 0;
                else
                    return dbSession.Update<t_SeatFriend>(t_SeatFriend._.GroupID, groupID, where) > 0;
            }
        }

        /// <summary>
        /// 获取好友分组
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatFriendGroup> GetSeatFriendGroup(string seatID)
        {
            var grouplist = dbSession.From<t_SeatFriendGroup>().Where(t_SeatFriendGroup._.SeatID == seatID).ToList();
            var list = grouplist.ConvertTo<SeatFriendGroup>();

            return list;
        }

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatFriend> GetFriends(string seatID, bool isFriend)
        {
            lock (syncobj)
            {
                WhereClip where = WhereClip.All;
                if (isFriend) where = t_SeatFriend._.SeatID == seatID;
                else where = t_SeatFriend._.FriendID == seatID;

                var list = dbSession.From<t_SeatFriend>().Where(where).ToList();

                var friendlist = list.ConvertAll<SeatFriend>(p =>
                    {
                        var seat1 = SeatManager.Instance.GetSeat(p.SeatID);
                        var seat2 = SeatManager.Instance.GetSeat(p.FriendID);

                        if (!isFriend)
                        {
                            seat2 = SeatManager.Instance.GetSeat(p.SeatID);
                            seat1 = SeatManager.Instance.GetSeat(p.FriendID);
                        }

                        if (seat1 == null || seat2 == null) return null;
                        var company = CompanyManager.Instance.GetCompany(seat2.CompanyID);

                        var friend = new SeatFriend()
                        {
                            Owner = seat1,
                            ClientID = seat2.ClientID,
                            CompanyID = seat2.CompanyID,
                            CompanyName = company.CompanyName,
                            Email = seat2.Email,
                            LoginCount = seat2.LoginCount,
                            LoginTime = seat2.LoginTime,
                            LogoutTime = seat2.LogoutTime,
                            MobileNumber = seat2.MobileNumber,
                            Password = seat2.Password,
                            SeatCode = seat2.SeatCode,
                            SeatName = seat2.SeatName,
                            SeatType = seat2.SeatType,
                            State = seat2.State,
                            Telephone = seat2.Telephone,
                            FaceImage = seat2.FaceImage,
                            Sign = seat2.Sign,
                            Introduction = seat2.Introduction,
                            MemoName = p.MemoName,
                            GroupID = p.GroupID
                        };

                        //如果是陌生人，则显示原来的名字
                        if (!isFriend) friend.MemoName = null;

                        return friend;
                    });

                //移除为null的数据
                friendlist.RemoveAll(p => p == null);

                return friendlist;
            }
        }

        /// <summary>
        /// 确定添加
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool AcceptFriend(int requestID, AcceptType type, string refuse)
        {
            lock (syncobj)
            {
                //获取请求对象
                t_SeatFriendRequest request = dbSession.Single<t_SeatFriendRequest>(t_SeatFriendRequest._.RequestID == requestID);

                if (type == AcceptType.Accept)
                {
                    int ret = 0;

                    using (DbTrans trans = dbSession.BeginTrans())
                    {
                        try
                        {
                            //处理接受
                            WhereClip where = t_SeatFriend._.SeatID == request.SeatID && t_SeatFriend._.FriendID == request.FriendID;

                            if (!trans.Exists<t_SeatFriend>(where))
                            {
                                //把对方加为好友
                                t_SeatFriend friend = new t_SeatFriend()
                                {
                                    SeatID = request.SeatID,
                                    FriendID = request.FriendID,
                                    AddTime = DateTime.Now
                                };

                                ret = trans.Save(friend);
                            }

                            WhereClip where1 = t_SeatFriendRequest._.RequestID == requestID;
                            ret = trans.Update<t_SeatFriendRequest>(t_SeatFriendRequest._.ConfirmState, 1, where1);

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    }

                    return ret > 0;
                }
                else if (type == AcceptType.AcceptAdd)
                {
                    int ret = 0;

                    using (DbTrans trans = dbSession.BeginTrans())
                    {
                        try
                        {
                            //处理接受并添加
                            //处理接受
                            WhereClip where = t_SeatFriend._.SeatID == request.SeatID && t_SeatFriend._.FriendID == request.FriendID;

                            if (!dbSession.Exists<t_SeatFriend>(where))
                            {
                                //把对方加为好友
                                t_SeatFriend friend = new t_SeatFriend()
                                {
                                    SeatID = request.SeatID,
                                    FriendID = request.FriendID,
                                    AddTime = DateTime.Now
                                };

                                ret = dbSession.Save(friend);
                            }

                            where = t_SeatFriend._.SeatID == request.FriendID && t_SeatFriend._.FriendID == request.SeatID;
                            if (!dbSession.Exists<t_SeatFriend>(where))
                            {
                                //把自己加为对方好友请求
                                //t_SeatFriend friend = new t_SeatFriend()
                                //{
                                //    SeatID = request.FriendID,
                                //    FriendID = request.SeatID,
                                //    AddTime = DateTime.Now
                                //};

                                //ret = dbSession.Save(friend);

                                //把对方加为好友
                                t_SeatFriendRequest friend = new t_SeatFriendRequest()
                                {
                                    SeatID = request.FriendID,
                                    FriendID = request.SeatID,
                                    Request = "对方同意并请求加你为好友！",
                                    ConfirmState = 0,
                                    AddTime = DateTime.Now
                                };

                                ret += dbSession.Save(friend);
                            }

                            WhereClip where1 = t_SeatFriendRequest._.RequestID == requestID;
                            ret = dbSession.Update<t_SeatFriendRequest>(t_SeatFriendRequest._.ConfirmState, 2, where1);

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    }

                    return ret > 0;
                }
                else if (type == AcceptType.Refuse)
                {
                    //处理拒绝
                    WhereClip where = t_SeatFriendRequest._.RequestID == requestID;

                    return dbSession.Update<t_SeatFriendRequest>(
                        new Field[] { t_SeatFriendRequest._.Refuse, t_SeatFriendRequest._.ConfirmState },
                       new object[] { refuse, -1 }, where) > 0;
                }

                return false;
            }
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool AddFriendRequest(string seatID, string friendID, string request)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.FriendID == friendID;
                if (dbSession.Exists<t_SeatFriend>(where))
                {
                    throw new LiveChatException("此客服已经是您的好友，不能重复添加！");
                }

                Seat seat = SeatManager.Instance.GetSeat(friendID);

                //把对方加为好友
                t_SeatFriendRequest friend = new t_SeatFriendRequest()
                {
                    SeatID = seatID,
                    FriendID = friendID,
                    Request = request,
                    ConfirmState = 0,
                    AddTime = DateTime.Now
                };

                return dbSession.Save(friend) > 0;
            }
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool DeleteFriend(string seatID, string friendID)
        {
            lock (syncobj)
            {
                WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.FriendID == friendID;
                return dbSession.Delete<t_SeatFriend>(where) > 0;
            }
        }

        /// <summary>
        /// 删除好友并移除自己
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool DeleteFriendAndRemoveOwner(string seatID, string friendID)
        {
            lock (syncobj)
            {
                using (DbTrans trans = dbSession.BeginTrans())
                {
                    try
                    {
                        int ret = 0;

                        WhereClip where = t_SeatFriend._.SeatID == seatID && t_SeatFriend._.FriendID == friendID;
                        ret = dbSession.Delete<t_SeatFriend>(where);

                        where = t_SeatFriend._.SeatID == friendID && t_SeatFriend._.FriendID == seatID;
                        ret += dbSession.Delete<t_SeatFriend>(where);

                        trans.Commit();

                        return ret > 0;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
