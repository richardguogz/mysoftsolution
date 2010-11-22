using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Interface;
using LiveChat.Entity;
using MySoft.Data;
using LiveChat.Utils;
using LiveChat.Service.Manager;

namespace LiveChat.Service
{
    public class SeatService : MarshalByRefObject, ISeatService
    {
        /// <summary>
        /// 添加好友群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool AddFriendGroup(string seatID, SeatFriendGroup group)
        {
            try
            {
                return SeatFriendManager.Instance.AddFriendGroup(seatID, group);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
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
            try
            {
                return SeatFriendManager.Instance.UpdateFriendGroupName(seatID, groupID, groupName);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除好友组
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool DeleteFriendGroup(string seatID, Guid groupID)
        {
            try
            {
                return SeatFriendManager.Instance.DeleteFriendGroup(seatID, groupID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 移动好友到组
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool MoveFriendToGroup(string seatID, string friendID, Guid groupID)
        {
            try
            {
                return SeatFriendManager.Instance.UpdateGroupID(seatID, friendID, groupID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool AddCompany(Company company)
        {
            try
            {
                return CompanyManager.Instance.AddCompany(company);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool DeleteCompany(string companyID)
        {
            try
            {
                return CompanyManager.Instance.DeleteCompany(companyID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取好友群
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatFriendGroup> GetSeatFriendGroup(string seatID)
        {
            try
            {
                return SeatFriendManager.Instance.GetSeatFriendGroup(seatID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 验证客户端
        /// </summary>
        /// <param name="id">要验证的使用者ID</param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public bool ValidateClient(string id, Guid clientID)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(id);
                if (seat == null) return false;
                seat.RefreshTime = DateTime.Now;
                return seat.ClientID == clientID;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 改变客服的状态
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="state"></param>
        public void ChangeSeatState(string seatID, OnlineState state)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                seat.State = state;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region ISeatService 成员

        /// <summary>
        /// 获取客服的会话列表
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<P2SSession> GetP2SSessions(string seatID)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                IList<P2SSession> list = new List<P2SSession>();

                seat.Sessions.RemoveAll(delegate(Session session)
                {
                    if (!SessionManager.Instance.ExistsSession(session.SessionID))
                    {
                        return true;
                    }
                    return false;
                });

                foreach (Session session in seat.Sessions)
                {
                    if (session is P2SSession)
                    {
                        P2SSession ps = session as P2SSession;
                        DateTime getTime = seat[ps.SessionID];
                        ps.NoReadMessageCount = (ps.Messages as List<Message>).FindAll(delegate(Message msg)
                        {
                            if ((msg.SenderID != seatID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > getTime)
                            {
                                return true;
                            }
                            return false;
                        }).Count;

                        list.Add(ps);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region 获取客服信息

        /// <summary>
        /// 发送消息到用户
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sessionID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void SendP2SMessage(MessageType type, string sessionID, string senderIP, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    throw new LiveChatException("会话ID不能为空！");
                }

                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    throw new LiveChatException(string.Format("会话({0})不存在！", sessionID));
                }
                P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;
                Seat seat = session.Seat;
                User user = session.User;
                MessageManager.Instance.AddP2SMessage(session, seat.SeatID, seat.ShowName, user.UserID, user.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 发送消息到客服(指定发送者，用于帮助回答)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sessionID"></param>
        /// <param name="seatID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void SendP2SMessage(MessageType type, string sessionID, string seatID, string senderIP, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    throw new LiveChatException("会话ID不能为空！");
                }

                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    throw new LiveChatException(string.Format("会话({0})不存在！", sessionID));
                }
                P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                User user = session.User;
                MessageManager.Instance.AddP2SMessage(session, seat.SeatID, seat.ShowName, user.UserID, user.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服当前会话的消息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public IList<Message> GetP2SMessages(string sessionID)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    throw new LiveChatException("会话ID不能为空！");
                }

                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    return new List<Message>();
                }

                P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;
                Seat seat = session.Seat;
                IList<Message> msgs = session.Messages;
                IList<Message> list = new List<Message>();
                DateTime getTime = seat[session.SessionID];

                foreach (Message msg in msgs)
                {
                    //去除自己发送的消息
                    if ((msg.SenderID != seat.SeatID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > getTime)
                    {
                        list.Add(msg);
                    }
                }

                seat[session.SessionID] = DateTime.Now;

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #endregion

        #region ICommonService 成员

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public Session GetSession(string sessionID)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    throw new LiveChatException("会话ID不能为空！");
                }

                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    return null;
                }
                return SessionManager.Instance.GetSession(sessionID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 自动接入给我的会话(每次一个)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">自动接入会话最大数</param>
        /// <returns>返回我的会话列表</returns>
        public P2SSession AutoAcceptSession(string seatID, int maxCount)
        {
            try
            {
                //先获取一下我的会话数
                IList<P2SSession> sessions = GetP2SSessions(seatID);

                //小于最大会话数则自动接入
                if (sessions.Count < maxCount)
                {
                    //获取客服
                    Seat seat = SeatManager.Instance.GetSeat(seatID);

                    //获取客服会话
                    IList<P2CSession> list = GetP2CSessions(seatID, SortType.Vip);
                    (list as List<P2CSession>).RemoveAll(delegate(P2CSession session)
                    {
                        if (!string.IsNullOrEmpty(session.RequestCode) && session.RequestCode != seat.SeatCode) return true;
                        return false;
                    });

                    //接入后返回接受的会话
                    if (list.Count > 0)
                    {
                        return AcceptSession(seatID, maxCount, list[0].SessionID);
                    }

                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取请求的会话信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<P2CSession> GetP2CSessions(string seatID, SortType sortType)
        {
            List<P2CSession> list1 = new List<P2CSession>();
            List<P2CSession> list2 = new List<P2CSession>();
            Seat seat = SeatManager.Instance.GetSeat(seatID);
            foreach (Session session in SessionManager.Instance.GetSessions())
            {
                if (session is P2CSession)
                {
                    P2CSession psession = session as P2CSession;
                    if (psession.Company.CompanyID == seat.CompanyID && psession.State == SessionState.WaitAccept)
                    {
                        if (psession.RequestCode == seat.SeatCode)
                        {
                            //添加给自己的到list1
                            list1.Add(psession);
                        }
                        else
                        {
                            //添加别人的到list2
                            list2.Add(psession);
                        }
                    }
                }
            }

            //将会话合并
            List<P2CSession> list = new List<P2CSession>();

            if (sortType == SortType.Code)
            {
                //对自己的会话进行时间排序
                list1.Sort(new SortTimeP2CSession());

                //对其它的会话进行时间排序
                list2.Sort(new SortTimeP2CSession());

                list.AddRange(list1);
                list.AddRange(list2);
            }
            else if (sortType == SortType.Vip)
            {
                list.AddRange(list1);
                list.AddRange(list2);

                //对会话进行VIP排序
                list.Sort(new SortVipP2CSession());
            }
            else
            {
                list.AddRange(list1);
                list.AddRange(list2);

                //对所有会话进行时间排序
                list.Sort(new SortTimeP2CSession());
            }

            //移除不是给自己的会话
            list.RemoveAll(p => !string.IsNullOrEmpty(p.RequestCode) && p.RequestCode != seat.SeatCode);

            return list;
        }

        /// <summary>
        /// 接受会话
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">最大允许接入会话数</param>
        /// <param name="sessionID"></param>
        public P2SSession AcceptSession(string seatID, int maxCount, string sessionID)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);

                //获取客服会话数
                IList<P2SSession> list = GetP2SSessions(seatID);

                //如果客服会话数大于最大会话数,直接退出
                if (list.Count >= maxCount)
                {
                    return null;
                }

                P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;
                session.Seat = seat;
                session.AcceptTime = DateTime.Now;
                session.State = SessionState.Talking;

                //把消息接收对象改为当前客服,会话ID改为新的会话ID
                foreach (Message msg in session.Messages)
                {
                    P2SMessage pmsg = msg as P2SMessage;
                    pmsg.SessionID = session.SessionID;
                    pmsg.ReceiverID = seat.SeatID;
                    pmsg.ReceiverName = seat.SeatName;
                }

                //移除老的Session,添加新的Session
                SessionManager.Instance.RemoveSession(sessionID);
                SessionManager.Instance.AddSession(session);

                seat.AddSession(session);

                User user = session.User;
                user.AddSession(session);

                if (user.UserType != UserType.TempUser)
                {
                    int chatCount = 1;
                    if (user.ChatCount.HasValue)
                    {
                        chatCount = user.ChatCount.Value + 1;
                    }
                    UserManager.Instance.UpdateChatInfo(user.UserID, DateTime.Now, chatCount);
                }

                //自动发送系统导语
                MessageManager.Instance.AddP2SMessage(session, seat.SeatID, seat.ShowName, user.UserID, user.ShowName,
                    null, MessageType.Tip, seat.Introduction);

                return session;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 接受会话
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">最大允许接入会话数</param>
        /// <param name="sessionIDs"></param>
        public void AcceptSessions(string seatID, int maxCount, params string[] sessionIDs)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);

                foreach (string sessionID in sessionIDs)
                {
                    if (!SessionManager.Instance.ExistsSession(sessionID))
                    {
                        continue;
                    }

                    //获取客服会话数
                    IList<P2SSession> list = GetP2SSessions(seatID);

                    //如果客服会话数大于最大会话数,直接退出
                    if (list.Count >= maxCount)
                    {
                        break;
                    }

                    P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;
                    session.Seat = seat;
                    session.AcceptTime = DateTime.Now;
                    session.State = SessionState.Talking;

                    //把消息接收对象改为当前客服,会话ID改为新的会话ID
                    foreach (Message msg in session.Messages)
                    {
                        P2SMessage pmsg = msg as P2SMessage;
                        pmsg.SessionID = session.SessionID;
                        pmsg.ReceiverID = seat.SeatID;
                        pmsg.ReceiverName = seat.SeatName;
                    }

                    //移除老的Session,添加新的Session
                    SessionManager.Instance.RemoveSession(sessionID);
                    SessionManager.Instance.AddSession(session);

                    seat.AddSession(session);

                    User user = session.User;
                    user.AddSession(session);

                    if (user.UserType != UserType.TempUser)
                    {
                        int chatCount = 1;
                        if (user.ChatCount.HasValue)
                        {
                            chatCount = user.ChatCount.Value + 1;
                        }
                        UserManager.Instance.UpdateChatInfo(user.UserID, DateTime.Now, chatCount);
                    }

                    //自动发送系统导语
                    MessageManager.Instance.AddP2SMessage(session, seat.SeatID, seat.ShowName, user.UserID, user.ShowName,
                        null, MessageType.Tip, seat.Introduction);
                }
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 结束客服所有会话
        /// </summary>
        /// <param name="seatID"></param>
        public void CloseAllSession(string seatID)
        {
            try
            {
                foreach (P2SSession session in GetP2SSessions(seatID))
                {
                    CloseSession(session.SessionID);
                }
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 结束会话
        /// </summary>
        /// <param name="sessionID"></param>
        public void CloseSession(string sessionID)
        {
            try
            {
                SessionManager.Instance.CloseSession(sessionID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region ISeatService 成员


        /// <summary>
        /// 通过seatID获取一个客服
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public Seat GetSeat(string seatID)
        {
            try
            {
                return SeatManager.Instance.GetSeat(seatID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public SeatMessage GetSeatMessage(string seatID)
        {
            try
            {
                var info = new SeatMessage();
                info.GroupMessages = new Dictionary<SeatGroup, MessageInfo>();
                info.SeatMessages = new Dictionary<SeatFriend, SeatInfo>();
                info.SessionMessages = new Dictionary<P2SSession, MessageInfo>();
                info.RequestMessages = new Dictionary<Seat, RequestInfo>();

                //添加组消息
                var groups = GroupManager.Instance.GetSeatGroups(seatID);
                foreach (var group in groups)
                {
                    var list = (group.Messages as List<Message>).FindAll(msg =>
                    {
                        //去除自己发送的消息
                        if ((msg.SenderID != seatID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > group[seatID])
                            return true;
                        else
                            return false;
                    });

                    info.GroupMessages[group] = new MessageInfo { Count = list.Count, Message = GetMessage(list) };
                }

                //添加会话消息
                Seat seat = GetSeat(seatID);

                //会话请求列表
                List<P2CSession> reqSessions = GetP2CSessions(seatID, SortType.Vip) as List<P2CSession>;
                List<Session> sessions = reqSessions.ConvertAll<Session>(p => (Session)p);
                sessions.AddRange(seat.Sessions);

                foreach (var session in sessions)
                {
                    if (session is P2SSession)
                    {
                        var list = (session.Messages as List<Message>).FindAll(msg =>
                        {
                            //去除自己发送的消息
                            if ((msg.SenderID != seatID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > seat[session.SessionID])
                                return true;
                            else
                                return false;
                        });

                        //处理未读消息数
                        session.NoReadMessageCount = list.Count;

                        P2SSession p2s = session as P2SSession;
                        info.SessionMessages[p2s] = new MessageInfo { Count = list.Count, Message = GetMessage(list) };
                    }
                }

                IList<SeatFriend> friends;
                var friendlist = GetSeatFriends(seatID, out friends);
                (friendlist as List<SeatFriend>).AddRange(friends);

                foreach (var friend in friendlist)
                {
                    var seatInfo = new SeatInfo() { MemoName = friend.MemoName };

                    foreach (var session in sessions)
                    {
                        if (session is S2SSession)
                        {
                            S2SSession s2s = session as S2SSession;

                            if (s2s.Owner.SeatID == friend.SeatID || s2s.Friend.SeatID == friend.SeatID)
                            {
                                var list = (session.Messages as List<Message>).FindAll(msg =>
                                {
                                    //去除自己发送的消息
                                    if ((msg.SenderID != seatID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > seat[session.SessionID])
                                        return true;
                                    else
                                        return false;
                                });

                                //处理未读消息数
                                session.NoReadMessageCount = list.Count;

                                seatInfo.Count = list.Count;
                                seatInfo.Message = GetMessage(list);
                                info.SeatMessages[friend] = seatInfo;

                                break;
                            }
                        }
                    }
                }

                try
                {
                    //请求信息
                    info.RequestMessages = SeatFriendManager.Instance.GetRequestFriends(seatID);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteLog(ex.ToString());
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private Message GetMessage(IList<Message> list)
        {
            if (list.Count == 0) return null;
            (list as List<Message>).Sort(new SortTimeMessage());
            return list[0];
        }

        /// <summary>
        /// 通过公司ID和Code获取一个客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <returns></returns>
        public Seat GetSeat(string companyID, string seatCode)
        {
            try
            {
                return SeatManager.Instance.GetSeat(companyID, seatCode);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 客服登录
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="password"></param>
        /// <param name="isvalidateManager"></param>
        /// <returns></returns>
        public IMResult Login(Guid clientID, string companyID, string seatCode, string password)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(companyID, seatCode);
                if (seat == null)
                {
                    return IMResult.InvalidUser;
                }
                else if (Encrypt.MD5(seat.Password) != password)
                {
                    return IMResult.InvalidPassword;
                }

                seat.LoginCount++;
                seat.LoginTime = DateTime.Now;
                seat.RefreshTime = DateTime.Now;
                seat.State = OnlineState.Online;
                seat.ClientID = clientID;

                SeatManager.Instance.UpdateSeat(seat.SeatID,
                                new Field[] { t_Seat._.LoginCount, t_Seat._.LoginTime },
                                new object[] { seat.LoginCount, seat.LoginTime }
                            );

                return IMResult.Successful;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="seatID"></param>
        public void Logout(string seatID)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                seat.LogoutTime = DateTime.Now;
                seat.State = OnlineState.Offline;

                SeatManager.Instance.UpdateSeat(seat.SeatID,
                                new Field[] { t_Seat._.LogoutTime },
                                new object[] { seat.LogoutTime }
                            );
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改客服密码
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public bool UpdatePassword(string seatID, string oldpassword, string newpassword)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                if (seat.Password != oldpassword)
                {
                    throw new LiveChatException("原始密码验证不成功！");
                }

                return SeatManager.Instance.UpdatePassword(seatID, newpassword);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyName"></param>
        /// <param name="webSite"></param>
        /// <param name="companyLogo"></param>
        /// <param name="chatWebSite"></param>
        /// <returns></returns>
        public bool UpdateCompany(string companyID, string companyName, string webSite, string companyLogo, string chatWebSite)
        {
            try
            {
                Company company = new Company();
                company.CompanyID = companyID;
                company.CompanyName = companyName;
                company.WebSite = webSite;
                company.CompanyLogo = companyLogo;
                company.ChatWebSite = chatWebSite;

                return CompanyManager.Instance.UpdateCompany(company);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region ISeatService 成员

        /// <summary>
        /// 客服发送消息到群
        /// </summary>
        /// <param name="type"></param>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void SendSGMessage(MessageType type, Guid groupID, string seatID, string senderIP, string content)
        {
            try
            {
                SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                MessageManager.Instance.AddSGMessage(group, seatID, seat.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服当前会话的群消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<Message> GetSGMessages(Guid groupID, string seatID)
        {
            try
            {
                SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                Seat seat = SeatManager.Instance.GetSeat(seatID);

                //如果群中不存在此客服
                if (!group.Exists(seat))
                {
                    return new List<Message>();
                }

                IList<Message> msgs = group.Messages;
                IList<Message> list = new List<Message>();
                DateTime getTime = group[seatID];

                foreach (Message msg in msgs)
                {
                    //去除自己发送的消息
                    if (msg.SenderID != seatID && msg.SendTime > getTime)
                    {
                        list.Add(msg);
                    }
                }

                group[seatID] = DateTime.Now;

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服当前会话的群消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<Message> GetSGHistoryMessages(Guid groupID, string seatID)
        {
            try
            {
                SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                Seat seat = SeatManager.Instance.GetSeat(seatID);

                //如果群中不存在此客服
                if (!group.Exists(seat))
                {
                    return new List<Message>();
                }

                //记录已经读过的时间
                group[seatID] = DateTime.Now;

                return group.Messages;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取会话中所有消息(指定时间后)
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <param name="lastGetTime"></param>
        /// <returns></returns>
        public IList<Message> GetSGHistoryMessages(Guid groupID, string seatID, DateTime lastGetTime)
        {
            List<Message> list = GetSGHistoryMessages(groupID, seatID) as List<Message>;
            return list.FindAll(p => p.SendTime >= lastGetTime);
        }

        #endregion

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompany(string companyID)
        {
            try
            {
                return CompanyManager.Instance.GetCompany(companyID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取所有公司
        /// </summary>
        /// <returns></returns>
        public IList<Company> GetCompanies()
        {
            try
            {
                return CompanyManager.Instance.GetCompanies();
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取公司(参数为公司名称)
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public Company GetCompanyForName(string companyName)
        {
            try
            {
                return CompanyManager.Instance.GetCompanyForName(companyName);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region 客服群管理

        /// <summary>
        /// 添加客服群
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public bool AddSeatGroup(string groupName, int maxCount, string createID, string managerID, string notification, string description)
        {
            try
            {
                t_SGroup group = new t_SGroup()
                {
                    GroupID = Guid.NewGuid(),
                    GroupName = groupName,
                    MaxPerson = maxCount,
                    CreateID = createID,
                    ManagerID = managerID,
                    Description = description,
                    Notification = notification,
                    AddTime = DateTime.Now
                };

                return GroupManager.Instance.SaveSeatGroup(group, false) > 0;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改客服群
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="groupName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public bool UpdateSeatGroup(Guid groupID, string groupName, int maxCount, string notification, string description)
        {
            try
            {
                t_SGroup group = new t_SGroup()
                {
                    GroupID = groupID,
                    GroupName = groupName,
                    MaxPerson = maxCount,
                    Description = description,
                    Notification = notification
                };
                return GroupManager.Instance.SaveSeatGroup(group, true) > 0;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除客服群
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool DeleteSeatGroup(Guid groupID)
        {
            try
            {
                return GroupManager.Instance.DeleteSeatGroup(groupID) > 0;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取某个客服的所有群
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatGroup> GetSeatGroups(string seatID)
        {
            try
            {
                return GroupManager.Instance.GetSeatGroups(seatID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取某个客服的所有群
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatGroup> GetSeatNoJoinGroups(string seatID)
        {
            try
            {
                return GroupManager.Instance.GetSeatNoJoinGroups(seatID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改群名称
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool UpdateSeatGroupName(string seatID, Guid groupID, string groupName)
        {
            try
            {
                t_GroupSeat gs = new t_GroupSeat()
                {
                    SeatID = seatID,
                    GroupID = groupID,
                    MemoName = groupName
                };

                return GroupManager.Instance.UpdateSeatGroup(gs);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 获取公司下所有客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Seat> GetCompanySeats(string companyID)
        {
            try
            {
                Company company = CompanyManager.Instance.GetCompany(companyID);
                return company.Seats;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服来自ID或名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<Seat> GetSeatsFromIDOrName(string id, string name, string cname)
        {
            try
            {
                var companies = CompanyManager.Instance.GetCompanies();
                IList<Seat> list = new List<Seat>();
                foreach (var company in companies)
                {
                    foreach (var seat in company.Seats)
                    {
                        if ((!string.IsNullOrEmpty(id) && seat.SeatCode == id) || (!string.IsNullOrEmpty(name) && seat.SeatName.Contains(name))
                            || (!string.IsNullOrEmpty(cname) && company.CompanyName.Contains(cname)))
                        {
                            list.Add(seat);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服群客服列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public IList<Seat> GetGroupSeats(Guid groupID)
        {
            try
            {
                return GroupManager.Instance.GetSeatGroup(groupID).Seats;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加到指定的群中
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        public void JoinGroup(string seatID, Guid groupID)
        {
            try
            {
                SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                if (group.Seats.Count < group.MaxPerson)
                {
                    bool success = GroupManager.Instance.JoinSeatGroup(seatID, groupID);
                    if (success)
                    {
                        Seat seat = SeatManager.Instance.GetSeat(seatID);

                        //将客服添加到群中
                        group.AddSeat(seat);
                    }
                }
                else
                {
                    throw new LiveChatException(string.Format("当前群({0})达到了最大人员限制！", group.GroupName));
                }
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 退出指定的群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        public void ExitGroup(string seatID, Guid groupID)
        {
            try
            {
                bool success = GroupManager.Instance.ExitSeatGroup(seatID, groupID);
                if (success)
                {
                    SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                    Seat seat = SeatManager.Instance.GetSeat(seatID);
                    group.RemoveSeat(seat);
                }
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 解散群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        public void DismissGroup(string seatID, Guid groupID)
        {
            try
            {
                //bool success = GroupManager.Instance.ExitSeatGroup(seatID, groupID);
                //if (success)
                //{
                //    SeatGroup group = GroupManager.Instance.GetSeatGroup(groupID);
                //    Seat seat = SeatManager.Instance.GetSeat(seatID);
                //    group.RemoveSeat(seat);
                //}
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region ISeatService 成员

        #region 回复信息

        /// <summary>
        /// 按公司获取所有的回复信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Reply> GetReplys(string companyID)
        {
            try
            {
                return ReplyManager.Instance.GetReplys(companyID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加快捷回复信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int AddReply(string companyID, string title, string content)
        {
            try
            {
                Reply reply = new Reply();
                reply.CompanyID = companyID;
                reply.Title = title;
                reply.Content = content;
                reply.AddTime = DateTime.Now;
                return ReplyManager.Instance.AddReply(reply);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除快捷回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        public bool DeleteReply(int replyID)
        {
            try
            {
                return ReplyManager.Instance.DeleteReply(replyID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改快捷回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool UpdateReply(int replyID, string title, string content)
        {
            try
            {
                Reply reply = new Reply();
                reply.ReplyID = replyID;
                reply.Title = title;
                reply.Content = content;

                return ReplyManager.Instance.UpdateReply(reply);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region 链接信息

        /// <summary>
        /// 按公司获取所有的链接信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Link> GetLinks(string companyID)
        {
            try
            {
                return LinkManager.Instance.GetLinks(companyID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加快捷链接信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public int AddLink(string companyID, string title, string url)
        {
            try
            {
                Link reply = new Link();
                reply.CompanyID = companyID;
                reply.Title = title;
                reply.Url = url;
                reply.AddTime = DateTime.Now;
                return LinkManager.Instance.AddLink(reply);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除快捷链接
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        public bool DeleteLink(int linkID)
        {
            try
            {
                return LinkManager.Instance.DeleteLink(linkID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改快捷链接
        /// </summary>
        /// <param name="linkID"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool UpdateLink(int linkID, string title, string url)
        {
            try
            {
                Link reply = new Link();
                reply.LinkID = linkID;
                reply.Title = title;
                reply.Url = url;
                return LinkManager.Instance.UpdateLink(reply);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <param name="mobilenumber"></param>
        /// <param name="sign"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddSeat(string companyID, string seatCode, string seatName, string password, string email, string telephone, string mobilenumber, string sign, string remark, SeatType seattype)
        {
            try
            {
                Seat seat = new Seat()
                {
                    CompanyID = companyID,
                    SeatCode = seatCode,
                    SeatName = seatName,
                    Password = password,
                    Email = email,
                    Telephone = telephone,
                    MobileNumber = mobilenumber,
                    Sign = sign,
                    Introduction = remark,
                    SeatType = seattype,
                };

                return SeatManager.Instance.AddSeat(seat);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="nickName"></param>
        /// <param name="sign"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        public bool UpdateSeat(string seatID, string seatName, string email, string telephone, string mobilenumber, string sign, string introduction, SeatType seattype)
        {
            try
            {
                Seat seat = GetSeat(seatID);
                if (seat == null) return false;

                seat.SeatType = seattype;
                seat.Telephone = telephone;
                seat.MobileNumber = mobilenumber;
                seat.Email = email;
                seat.SeatName = seatName;
                seat.Sign = sign;
                seat.Introduction = introduction;

                return SeatManager.Instance.UpdateSeat(seat);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool UpdateSeatFace(string seatID, byte[] buffer)
        {
            try
            {
                Seat seat = GetSeat(seatID);
                if (seat == null) return false;

                seat.FaceImage = buffer;
                return SeatManager.Instance.UpdateSeatFace(seat);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除客服
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public bool DeleteSeat(string seatID)
        {
            try
            {
                return SeatManager.Instance.DeleteSeat(seatID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region ICommonService 成员

        /// <summary>
        /// 获取会话中所有历史消息(从内存中查询)
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public IList<Message> GetP2SHistoryMessages(string sessionID)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionID))
                {
                    throw new LiveChatException("会话ID不能为空！");
                }

                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    return new List<Message>();
                }

                P2SSession session = SessionManager.Instance.GetSession(sessionID) as P2SSession;

                if (session.Seat != null)
                {
                    //记录已经读过的时间
                    Seat seat = session.Seat;
                    seat[session.SessionID] = DateTime.Now;
                }

                return session.Messages;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取历史消息(指定时间后)
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public IList<Message> GetP2SHistoryMessages(string sessionID, DateTime lastGetTime)
        {
            List<Message> list = GetP2SHistoryMessages(sessionID) as List<Message>;
            return list.FindAll(p => p.SendTime >= lastGetTime);
        }

        /// <summary>
        /// 获取与指定用户的所有会话(从数据库中查询)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IList<P2SSession> GetP2SSessionsFromDB(string seatID, string userID, DateTime beginTime, DateTime endTime)
        {
            try
            {
                return SessionManager.Instance.GetUserSessionsFromDB(seatID, userID, beginTime, endTime);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取会话中所有历史消息(从数据库中查询)
        /// </summary>
        /// <param name="sid">会话唯一标识</param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public DataView<IList<P2SMessage>> GetP2SHistoryMessagesFromDB(Guid sid, int pIndex, int pSize)
        {
            try
            {
                return MessageManager.Instance.GetP2SHistoryMessagesFromDB(sid, pIndex, pSize);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取会话中所有历史消息(从数据库中查询)
        /// </summary>
        /// <param name="sid">会话唯一标识</param>
        /// <param name="getDate"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public DataView<IList<S2SMessage>> GetS2SHistoryMessagesFromDB(string seatID, string friendID, DateTime fromDate, DateTime toDate, int pIndex, int pSize)
        {
            try
            {
                string sessionID = string.Format("S_{0}_{1}", seatID, friendID);
                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    //调换一下ID，以判断是否存在会话
                    sessionID = string.Format("S_{1}_{0}", seatID, friendID);
                    if (!SessionManager.Instance.ExistsSession(sessionID))
                    {
                        return new DataView<IList<S2SMessage>>(10);
                    }
                }
                S2SSession session = SessionManager.Instance.GetSession(sessionID) as S2SSession;

                return MessageManager.Instance.GetS2SHistoryMessagesFromDB(session.SID, fromDate, toDate, pIndex, pSize);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region ISeatService 成员

        /// <summary>
        /// 获取用户相关信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUser(string userID)
        {
            try
            {
                return UserManager.Instance.GetUser(userID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region ICommonService 成员

        /// <summary>
        /// 上传图像
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string[] UploadImage(byte[] buffer, string extension)
        {
            try
            {
                return CommonUtils.UploadData(UploadType.Seat, FileType.Image, buffer, extension);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string UploadFile(byte[] buffer, string extension)
        {
            try
            {
                return CommonUtils.UploadData(UploadType.User, FileType.File, buffer, extension)[0];
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 重新加载缓存
        /// </summary>
        /// <returns></returns>
        public void ReloadCache()
        {
            try
            {
                CompanyManager.Instance.LoadCompany();
                GroupManager.Instance.LoadGroup();
                UserManager.Instance.LoadUser();
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region ISeatService 成员


        /// <summary>
        /// 获取留言信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Leave GetLeave(int id)
        {
            try
            {
                return LeaveManager.Instance.GetLeave(id);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLeave(int id)
        {
            try
            {
                return LeaveManager.Instance.DeleteLeave(id);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取留言信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataView<IList<Leave>> GetLeaves(string companyID, DateTime beginTime, DateTime endTime, int pIndex, int pSize)
        {
            try
            {
                return LeaveManager.Instance.GetLeaves(companyID, beginTime, endTime, pIndex, pSize);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region 广告管理

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IList<Ad> GetAds(string companyID, DateTime beginTime, DateTime endTime)
        {
            try
            {
                return AdManager.Instance.GetAds(companyID, beginTime, endTime);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAd(int id)
        {
            try
            {
                return AdManager.Instance.DeleteAd(id);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="area"></param>
        /// <param name="imgurl"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool UpdateAd(int id, string name, string title, string area, string imgurl, string url, string adLogoImgUrl, string adLogoUrl, string adText, string adTextUrl, bool isdefault, bool isCommon)
        {
            try
            {
                Ad ad = new Ad()
                {
                    ID = id,
                    AdName = name,
                    AdTitle = title,
                    AdArea = area,
                    AdImgUrl = imgurl,
                    AdUrl = url,
                    AdLogoImgUrl = adLogoImgUrl,
                    AdLogoUrl = adLogoUrl,
                    AdText = adText,
                    AdTextUrl = adTextUrl,
                    AddTime = DateTime.Now,
                    IsDefault = isdefault,
                    IsCommon = isCommon
                };
                return AdManager.Instance.UpdateAd(ad) > 0;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 新增广告
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="area"></param>
        /// <param name="imgurl"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool AddAd(string companyID, string name, string title, string area, string imgurl, string url, string adLogoImgUrl, string adLogoUrl, string adText, string adTextUrl, bool isdefault, bool isCommon)
        {
            try
            {
                Ad ad = new Ad()
                {
                    CompanyID = companyID,
                    AdName = name,
                    AdTitle = title,
                    AdArea = area,
                    AdImgUrl = imgurl,
                    AdUrl = url,
                    AdLogoImgUrl = adLogoImgUrl,
                    AdLogoUrl = adLogoUrl,
                    AdText = adText,
                    AdTextUrl = adTextUrl,
                    AddTime = DateTime.Now,
                    IsDefault = isdefault,
                    IsCommon = isCommon
                };
                return AdManager.Instance.AddAd(ad) > 0;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region 获取地区

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUsers()
        {
            try
            {
                return UserManager.Instance.GetUsers();
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取地区信息
        /// </summary>
        /// <returns></returns>
        public IList<Area> GetAreas()
        {
            try
            {
                return AreaManager.Instance.GetAreas();
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region 客服与客服会话

        /// <summary>
        /// 发送消息到客服
        /// </summary>
        /// <param name="type"></param>
        /// <param name="seat1ID"></param>
        /// <param name="seat2ID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void SendS2SMessage(MessageType type, string seat1ID, string seat2ID, string senderIP, string content)
        {
            try
            {
                string sessionID = string.Format("S_{0}_{1}", seat1ID, seat2ID);
                S2SSession session;
                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    //调换一下ID，以判断是否存在会话
                    sessionID = string.Format("S_{1}_{0}", seat1ID, seat2ID);
                    if (!SessionManager.Instance.ExistsSession(sessionID))
                    {
                        session = SessionManager.Instance.CreateS2SSession(seat1ID, seat2ID);
                    }
                    else
                    {
                        session = SessionManager.Instance.GetSession(sessionID) as S2SSession;
                    }
                }
                else
                {
                    session = SessionManager.Instance.GetSession(sessionID) as S2SSession;
                }

                //获取创建者
                Seat seat = SeatManager.Instance.GetSeat(seat1ID);

                MessageManager.Instance.AddS2SMessage(session, seat.SeatID, seat.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取会话中所有历史消息(从内存中查询)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="fromID"></param>
        /// <returns></returns>
        public IList<Message> GetS2SHistoryMessages(string seatID, string fromID)
        {
            try
            {
                string sessionID = string.Format("S_{0}_{1}", seatID, fromID);
                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    //调换一下ID，以判断是否存在会话
                    sessionID = string.Format("S_{1}_{0}", seatID, fromID);
                    if (!SessionManager.Instance.ExistsSession(sessionID))
                    {
                        return new List<Message>();
                    }
                }

                S2SSession session = SessionManager.Instance.GetSession(sessionID) as S2SSession;

                //记录已经读过的时间
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                seat[session.SessionID] = DateTime.Now;

                return session.Messages;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取历史消息(指定时间后)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="fromID"></param>
        /// <param name="lastGetTime"></param>
        /// <returns></returns>
        public IList<Message> GetS2SHistoryMessages(string seatID, string fromID, DateTime lastGetTime)
        {
            List<Message> list = GetS2SHistoryMessages(seatID, fromID) as List<Message>;
            return list.FindAll(p => p.SendTime >= lastGetTime);
        }

        #endregion

        #region 好友功能

        /// <summary>
        /// 修改备注名称
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RenameFriend(string seatID, string friendID, string name)
        {
            try
            {
                return SeatFriendManager.Instance.UpdateMemoName(seatID, friendID, name);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatFriend> GetSeatFriends(string seatID)
        {
            try
            {
                return SeatFriendManager.Instance.GetFriends(seatID, true);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public IList<SeatFriend> GetSeatFriends(string seatID, out IList<SeatFriend> friends)
        {
            try
            {
                var list = SeatFriendManager.Instance.GetFriends(seatID, true);
                friends = SeatFriendManager.Instance.GetFriends(seatID, false);

                (friends as List<SeatFriend>).RemoveAll(p =>
                {
                    return (list as List<SeatFriend>).Exists(s => s.SeatID == p.SeatID);
                });

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool AddSeatFriendRequest(string seatID, string friendID, string reqeust)
        {
            try
            {
                return SeatFriendManager.Instance.AddFriendRequest(seatID, friendID, reqeust);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public bool ConfirmAddSeatFriend(int requestID, AcceptType type, string refuse)
        {
            try
            {
                return SeatFriendManager.Instance.AcceptFriend(requestID, type, refuse);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool DeleteSeatFriend(string seatID, string friendID)
        {
            try
            {
                return SeatFriendManager.Instance.DeleteFriend(seatID, friendID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除好友并移除自己
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        public bool DeleteSeatFriendAndRemoveOwner(string seatID, string friendID)
        {
            try
            {
                return SeatFriendManager.Instance.DeleteFriendAndRemoveOwner(seatID, friendID);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion
    }
}
