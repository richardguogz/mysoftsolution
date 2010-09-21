using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Interface;
using LiveChat.Entity;
using LiveChat.Utils;
using LiveChat.Service.Manager;

namespace LiveChat.Service
{
    public class UserService : MarshalByRefObject, IUserService
    {
        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            try
            {
                var connection = FundLiveChat.Default.CreateConnection();
                string connectionString = connection.ConnectionString;
                connection.Close();
                return connectionString;
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
                User user = UserManager.Instance.GetUser(id);
                if (user == null) return false;
                user.RefreshTime = DateTime.Now;
                return user.ClientID == clientID;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="id">要验证的使用者ID</param>
        /// <param name="password">要验证的使用者密码</param>
        /// <returns></returns>
        public IMResult ValidateUser(string id, string password)
        {
            try
            {
                //E.User u1 = M.UserManager.Instance.GetUser(id);
                //if (u1 == null)
                //{
                //    return IMResult.InvalidUser;
                //}
                //else if (Encrypt.MD5(u1.Password) != password)
                //{
                //    return IMResult.InvalidPassword;
                //}

                return IMResult.Successful;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region IUserService 成员

        /// <summary>
        /// 获取用户的会话列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<P2SSession> GetP2SSessions(string userID)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                IList<P2SSession> list = new List<P2SSession>();

                user.Sessions.RemoveAll(delegate(Session session)
                {
                    if (!SessionManager.Instance.ExistsSession(session.SessionID))
                    {
                        return true;
                    }
                    return false;
                });

                foreach (Session session in user.Sessions)
                {
                    if (session is P2SSession)
                    {
                        P2SSession ps = session as P2SSession;
                        DateTime getTime = user[ps.SessionID];
                        ps.NoReadMessageCount = (ps.Messages as List<Message>).FindAll(delegate(Message msg)
                        {
                            if ((msg.SenderID != userID || msg.Type == MessageType.Tip || msg.Type == MessageType.System) && msg.SendTime > getTime)
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

        #region 获取用户消息

        /// <summary>
        /// 发送消息到公司
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public string SendP2CMessage(MessageType type, string userID, string companyID, string seatCode, string senderIP, string content)
        {
            try
            {
                string sessionID = string.Format("S_{0}_{1}", userID, companyID);
                P2CSession session;
                if (!SessionManager.Instance.ExistsSession(sessionID))
                {
                    session = SessionManager.Instance.CreateP2CSession(userID, companyID, seatCode, senderIP);
                }
                else
                {
                    session = SessionManager.Instance.GetSession(sessionID) as P2CSession;
                }
                User user = UserManager.Instance.GetUser(userID);
                MessageManager.Instance.AddP2CMessage(session, userID, user.ShowName, senderIP, type, content);
                return session.SessionID;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 用户发送消息到群
        /// </summary>
        /// <param name="type"></param>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void SendUGMessage(MessageType type, Guid groupID, string userID, string senderIP, string content)
        {
            try
            {
                UserGroup group = GroupManager.Instance.GetUserGroup(groupID);
                User user = UserManager.Instance.GetUser(userID);
                MessageManager.Instance.AddUGMessage(group, userID, user.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

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
                User user = session.User;
                Seat seat = session.Seat;
                MessageManager.Instance.AddP2SMessage(session, user.UserID, user.ShowName, seat.SeatID, seat.ShowName, senderIP, type, content);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }


        /// <summary>
        /// 获取用户当前会话的消息
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
                User user = session.User;
                IList<Message> msgs = session.Messages;
                IList<Message> list = new List<Message>();
                DateTime getTime = user[session.SessionID];

                foreach (Message msg in msgs)
                {
                    //去除自己的消息
                    if ((msg.SenderID != user.UserID || msg.Type == MessageType.System) && msg.SendTime > getTime)
                    {
                        list.Add(msg);
                    }
                }

                user[session.SessionID] = DateTime.Now;

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 用户与公司是否已在会话中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public P2CSession GetP2CSession(string userID, string companyID)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                foreach (Session session in SessionManager.Instance.GetSessions())
                {
                    if (session is P2CSession)
                    {
                        P2CSession ps = session as P2CSession;
                        if (ps.User.UserID == userID && ps.Company.CompanyID == companyID)
                        {
                            return ps;
                        }
                    }
                    else if (session is P2SSession)
                    {
                        P2SSession ps = session as P2SSession;
                        if (ps.User.UserID == userID && ps.Seat.CompanyID == companyID)
                        {
                            Company company = CompanyManager.Instance.GetCompany(companyID);
                            P2CSession pc = new P2CSession(ps.User, company);
                            pc.Seat = ps.Seat;
                            return pc;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户未读的消息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<Message> GetNoReadMessages(string userID)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                IList<Message> list = new List<Message>(user.Messages);

                //读取消息后清除
                user.Messages.Clear();

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户当前会话的群消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<Message> GetUGMessages(Guid groupID, string userID)
        {
            try
            {
                UserGroup group = GroupManager.Instance.GetUserGroup(groupID);
                User user = UserManager.Instance.GetUser(userID);

                //如果群中不存在此用户
                if (!group.Exists(user))
                {
                    return new List<Message>();
                }

                IList<Message> msgs = group.Messages;
                IList<Message> list = new List<Message>();
                DateTime getTime = group[userID];

                foreach (Message msg in msgs)
                {
                    //去除自己发送的消息
                    if (msg.SenderID != userID && msg.SendTime > getTime)
                    {
                        list.Add(msg);
                    }
                }

                group[userID] = DateTime.Now;

                return list;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户当前会话的群消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<Message> GetUGHistoryMessages(Guid groupID, string userID)
        {
            try
            {
                UserGroup group = GroupManager.Instance.GetUserGroup(groupID);
                User user = UserManager.Instance.GetUser(userID);

                //如果群中不存在此用户
                if (!group.Exists(user))
                {
                    return new List<Message>();
                }

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
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<Message> GetUGHistoryMessages(Guid groupID, string userID, DateTime lastGetTime)
        {
            List<Message> list = GetUGHistoryMessages(groupID, userID) as List<Message>;
            return list.FindAll(p => p.SendTime >= lastGetTime);
        }

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

        #region IUserService 成员

        /// <summary>
        /// 通过用户ID获取一个用户
        /// </summary>
        /// <param name="userID"></param>
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

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="userType"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IMResult Login(Guid clientID, UserType userType, string userID, string password)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                if (user == null)
                {
                    switch (userType)
                    {
                        //匿名用户不需要密码
                        case UserType.TempUser:
                            //string key = CommonUtils.MakeUniqueKey(4, string.Empty);
                            //string userName = string.Format("{0}({1})", "匿名用户", key);
                            user = new User(userID);
                            user.UserName = user.UserID;
                            break;
                        //网站用户和基金宝客服端用户需要密码验证
                        //case UserType.FundUser:
                        //case UserType.WebUser:
                        //    E.User u1 = M.UserManager.Instance.GetUser(userID);
                        //    if (u1 == null)
                        //    {
                        //        return IMResult.InvalidUser;
                        //    }
                        //    else if (Encrypt.MD5(u1.Password) != password)
                        //    {
                        //        return IMResult.InvalidPassword;
                        //    }
                        //    user = new User(u1.Name);
                        //    user.IsVIP = u1.IsEndow;

                        //    UserExtend extend = new UserExtend();
                        //    extend.Email = u1.Email;
                        //    extend.MyAsset = u1.Money;
                        //    user.Extend = extend;
                        //    break;
                    }

                    //把用户添加到用户列表中
                    UserManager.Instance.AddUser(user);
                }
                //else if (userType != UserType.TempUser)
                //{
                //    //网站用户需要每次都验证
                //    E.User u = M.UserManager.Instance.GetUser(userID);
                //    if (u == null)
                //    {
                //        return IMResult.InvalidUser;
                //    }
                //    else if (Encrypt.MD5(u.Password) != password)
                //    {
                //        return IMResult.InvalidPassword;
                //    }

                //    user.IsVIP = u.IsEndow;

                //    UserExtend extend = user.Extend;
                //    extend.Email = u.Email;
                //    extend.MyAsset = u.Money;
                //}

                user.UserType = userType;
                user.LoginCount++;
                user.LoginTime = DateTime.Now;
                user.State = OnlineState.Online;
                user.ClientID = clientID;

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
        /// <param name="userID"></param>
        public void Logout(string userID)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                user.LogoutTime = DateTime.Now;
                user.State = OnlineState.Offline;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion

        #region IUserService 成员

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

        #endregion

        /// <summary>
        /// 获取用户相关信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(string userID)
        {
            try
            {
                User user = UserManager.Instance.GetUser(userID);
                UserInfo info = new UserInfo();
                info.CurrentSessionCount = user.Sessions.Count;
                foreach (Session session in user.Sessions)
                {
                    DateTime getTime = user[session.SessionID];
                    info.NoReadMessageCount += (session.Messages as List<Message>).FindAll(delegate(Message message)
                    {
                        if (message.SenderID != userID && message.SendTime > getTime)
                        {
                            return true;
                        }
                        return false;
                    }).Count;
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户群用户列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public IList<User> GetGroupUsers(Guid groupID)
        {
            try
            {
                return GroupManager.Instance.GetUserGroup(groupID).Users;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户群列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<UserGroup> GetJoinGroups(string userID)
        {
            try
            {
                return UserManager.Instance.GetGroups(userID, true);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户未加入的群列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<UserGroup> GetNoJoinGroups(string userID)
        {
            try
            {
                return UserManager.Instance.GetGroups(userID, false);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加到组中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        public void AddToGroup(string userID, Guid groupID)
        {
            try
            {
                UserGroup group = GroupManager.Instance.GetUserGroup(groupID);
                if (group.Users.Count < group.MaxPerson)
                {
                    User user = UserManager.Instance.GetUser(userID);

                    //将用户添加到群中
                    group.AddUser(user);
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
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        public void ExitGroup(string userID, Guid groupID)
        {
            try
            {
                UserGroup group = GroupManager.Instance.GetUserGroup(groupID);
                User user = UserManager.Instance.GetUser(userID);
                group.RemoveUser(user);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region ICommonService 成员

        /// <summary>
        /// 获取历史消息
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

        #endregion

        /// <summary>
        /// 获取公司客服是否在线
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool GetSeatOnline(string companyID, string seatCode)
        {
            try
            {
                Seat seat = SeatManager.Instance.GetSeat(companyID, seatCode);
                return seat.State != OnlineState.Offline;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取公司客服是否在线
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool GetSeatOnline(string companyID)
        {
            try
            {
                bool online = false;
                IList<Seat> seats = CompanyManager.Instance.GetCompany(companyID).Seats;
                foreach (Seat seat in seats)
                {
                    if (seat.State != OnlineState.Offline)
                    {
                        online = true;
                        break;
                    }
                }
                return online;
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

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
        /// 添加留言
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="postIP"></param>
        /// <returns></returns>
        public int AddLeave(string companyID, string name, string telephone, string email, string title, string body, string postIP)
        {
            try
            {
                Leave leave = new Leave()
                {
                    CompanyID = companyID,
                    Name = name,
                    Telephone = telephone,
                    Email = email,
                    Title = title,
                    Body = body,
                    PostIP = postIP,
                    AddTime = DateTime.Now
                };

                return LeaveManager.Instance.AddLeave(leave);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #region 广告信息

        /// <summary>
        /// 通过IP获取广告
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Ad GetAdFromIP(string companyID, string ip)
        {
            try
            {
                return AdManager.Instance.GetAdFromIP(companyID, ip);
            }
            catch (Exception ex)
            {
                throw new LiveChatException(ex.Message, ex);
            }
        }

        #endregion
    }
}
