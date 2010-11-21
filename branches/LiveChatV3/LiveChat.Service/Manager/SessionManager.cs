using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;
using System.Timers;
using System.Data.Common;
using LiveChat.Utils;
using MySoft.Core;

namespace LiveChat.Service.Manager
{
    public class SessionManager
    {
        private DbSession dbSession;
        private Timer timer, chatTimer;
        private static readonly object syncobj = new object();
        private static Dictionary<string, Session> dictSession = new Dictionary<string, Session>();
        private static Dictionary<string, Session> closeSession = new Dictionary<string, Session>();
        private static IList<string> timeoutSessions = new List<string>();
        private static List<Message> msgList = new List<Message>();
        public static readonly SessionManager Instance = new SessionManager();

        public SessionManager()
        {
            this.dbSession = DataAccess.DbLiveChat;

            this.timer = new Timer();
            this.timer.Interval = TimeSpan.FromMinutes(ServiceConfig.MessageSaveInterval).TotalMilliseconds;
            this.timer.AutoReset = true;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.Start();

            this.chatTimer = new Timer();
            this.chatTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            this.chatTimer.AutoReset = true;
            this.chatTimer.Elapsed += new ElapsedEventHandler(chatTimer_Elapsed);
            this.chatTimer.Start();
        }

        void chatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            chatTimer.Stop();

            try
            {
                foreach (Session session in dictSession.Values)
                {
                    if (session is P2SSession)
                    {
                        P2SSession s = session as P2SSession;
                        if (s.Seat == null) continue;

                        //检测会话超时(超时半小时未回话，自动关闭会话)
                        if (s.State == SessionState.Closed)
                        {
                            CloseSession(s.SessionID);
                        }
                        else if (s.State == SessionState.Talking && DateTime.Now.Subtract(s.RefreshTime).TotalMinutes > 30)
                        {
                            //超时的会话
                            if (!timeoutSessions.Contains(s.SessionID)) timeoutSessions.Add(s.SessionID);
                            CloseSession(s.SessionID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.Message);
            }
            finally
            {
                chatTimer.Start();
            }
        }

        /// <summary>
        /// 清除消息和会话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                SaveSessionAndMessageToDB();
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        #region 保存消息

        /// <summary>
        /// 保存p2s消息到数据库
        /// </summary>
        private void SaveSessionAndMessageToDB()
        {
            //调用清除消息的命令并取得消息
            foreach (Session session in dictSession.Values)
            {
                msgList.AddRange(session.Messages);
                closeSession[session.SessionID] = session;
            }

            //将关闭的会话消息全部保存
            foreach (Session session in closeSession.Values)
            {
                msgList.AddRange(session.Messages);
            }

            //将消息状态设置为正常
            msgList.ForEach(delegate(Message msg)
            {
                msg.State = MessageState.Normal;
            });

            //保存会话和消息
            IList<Session> saveSession = new List<Session>(closeSession.Values);
            bool ret = SaveSessionAndMessage(saveSession, msgList);

            if (ret)
            {
                //清除所有关闭的会话
                closeSession.Clear();

                //清除所有消息
                msgList.Clear();
            }
        }

        #endregion

        /// <summary>
        /// 启动时加载数据
        /// </summary>
        public void LoadSessionAndMessage()
        {
            try
            {
                //加载未结束的会话
                WhereClip where = t_P2SSession._.State == SessionState.Talking;
                IList<t_P2SSession> tlist = dbSession.From<t_P2SSession>().Where(where).ToList();
                IList<P2SSession> list = ConvertToP2SSessions(tlist);

                //一次加载所有的消息到内在查询
                WhereClip allWhere = t_P2SMessage._.State == MessageState.Abnormal;
                //加载存在于会话中的消息
                allWhere &= t_P2SMessage._.SessionID.In(dbSession.From<t_P2SSession>().Where(where).Select(t_P2SSession._.SessionID));
                //启用内存查询处理
                SourceList<t_P2SMessage> memory = dbSession.From<t_P2SMessage>().Where(allWhere).ToList();

                foreach (P2SSession session in list)
                {
                    //把异常的消息读取出来
                    IList<t_P2SMessage> msglist = memory.FindAll(p => p.SessionID == session.SessionID);
                    IList<P2SMessage> msgs = MessageManager.Instance.ConvertToP2SMessages(msglist);

                    //把消息附加在会话上
                    foreach (P2SMessage msg in msgs)
                    {
                        session.AddMessage(msg);
                    }

                    dictSession.Add(session.SessionID, session);

                    //把会话添加到各自的会话队列中
                    User user = session.User;
                    user[session.SessionID] = session.LastReceiveTime;
                    user.AddSession(session);

                    Seat seat = session.Seat;
                    seat[session.SessionID] = session.LastReceiveTime;
                    seat.AddSession(session);

                    //删除条件
                    WhereClip msgWhere = t_P2SMessage._.SessionID == session.SessionID;

                    //加载完后删除消息
                    dbSession.Delete<t_P2SMessage>(msgWhere);
                }

                //加载完后删除会话
                dbSession.Delete<t_P2SSession>(where);

                IList<t_S2SSession> slist = dbSession.From<t_S2SSession>().ToList();
                IList<S2SSession> pslist = ConvertToS2SSessions(slist);

                foreach (S2SSession session in pslist)
                {
                    Seat owner = session.Owner;
                    Seat friend = session.Friend;

                    if (owner == null || friend == null) continue;
                    if (dictSession.ContainsKey(session.SessionID)) continue;

                    //把会话添加到各自的会话队列中
                    dictSession.Add(session.SessionID, session);

                    owner[session.SessionID] = session.LastReceiveTime;
                    owner.AddSession(session);
                    friend[session.SessionID] = session.LastReceiveTime;
                    friend.AddSession(session);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 服务停止或服务出错时调用
        /// </summary>
        public void SaveSessionAndMessage()
        {
            try
            {
                List<Session> sessions = new List<Session>();
                List<Message> msgs = new List<Message>();

                foreach (Session session in dictSession.Values)
                {
                    sessions.Add(session);
                    msgs.AddRange(session.Messages);
                }

                //将关闭的会话消息全部保存
                foreach (Session session in closeSession.Values)
                {
                    sessions.Add(session);

                    //将消息状态设置为正常
                    (session.Messages as List<Message>).ForEach(delegate(Message msg)
                    {
                        msg.State = MessageState.Normal;
                    });

                    msgs.AddRange(session.Messages);
                }

                //保存会话与消息
                SaveSessionAndMessage(sessions, msgs);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 保存会话和消息
        /// </summary>
        private bool SaveSessionAndMessage(IList<Session> sessions, IList<Message> msgs)
        {
            using (DbTrans trans = dbSession.BeginTrans())
            {
                try
                {
                    if (sessions.Count > 0)
                    {
                        //获取已经关闭的会话中的消息
                        foreach (Session session in sessions)
                        {
                            //只有客服与用户的会话才保存消息
                            if (session is P2SSession)
                            {
                                P2SSession ps = session as P2SSession;

                                //客服与用户会话
                                t_P2SSession pss = DataUtils.ConvertType<P2SSession, t_P2SSession>(ps);
                                pss.UserID = ps.User.UserID;

                                if (ps.Seat != null)
                                    pss.SeatID = ps.Seat.SeatID;

                                trans.InsertOrUpdate(pss);
                            }
                            else if (session is S2SSession)
                            {
                                S2SSession ss = session as S2SSession;

                                //客服与客服会话
                                t_S2SSession sss = DataUtils.ConvertType<S2SSession, t_S2SSession>(ss);
                                sss.SeatID = ss.Owner.SeatID;
                                sss.FriendID = ss.Friend.SeatID;

                                trans.InsertOrUpdate(sss);
                            }
                        }
                    }

                    if (msgs.Count > 0)
                    {
                        //保存消息
                        foreach (Message msg in msgs)
                        {
                            if (msg is P2SMessage)
                            {
                                P2SMessage pmsg = msg as P2SMessage;

                                //客服与用户消息
                                t_P2SMessage tmsg = DataUtils.ConvertType<P2SMessage, t_P2SMessage>(pmsg);
                                trans.InsertOrUpdate(tmsg);
                            }
                            else if (msg is S2SMessage)
                            {
                                S2SMessage smsg = msg as S2SMessage;

                                //客服与用户消息
                                t_S2SMessage tmsg = DataUtils.ConvertType<S2SMessage, t_S2SMessage>(smsg);
                                trans.InsertOrUpdate(tmsg);
                            }
                        }
                    }

                    //提交事务
                    trans.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteLog(ex.Message);

                    //回滚事务
                    trans.Rollback();

                    return false;
                }
            }
        }

        #region ISessionManager 成员

        /// <summary>
        /// 创建用户与公司的会话
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public P2CSession CreateP2CSession(string userID, string companyID, string seatCode, string senderIP)
        {
            lock (syncobj)
            {
                User user = UserManager.Instance.GetUser(userID);
                Company company = CompanyManager.Instance.GetCompany(companyID);

                //实例化一个用户与公司的会话
                P2CSession session = new P2CSession(user, company);
                session.FromIP = senderIP;
                session.FromAddress = PHCZIP.Get(senderIP);
                session.RequestCode = seatCode;
                session.State = SessionState.WaitAccept;

                //添加到会话列表中
                dictSession.Add(session.SessionID, session);

                return session;
            }
        }

        /// <summary>
        /// 创建用户与用户的会话
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public P2PSession CreateP2PSession(string user1ID, string user2ID)
        {
            lock (syncobj)
            {
                User user1 = UserManager.Instance.GetUser(user1ID);
                User user2 = UserManager.Instance.GetUser(user2ID);

                P2PSession session = new P2PSession(user1, user2);
                session.State = SessionState.Talking;

                user1.AddSession(session);
                user2.AddSession(session);

                //添加到会话列表中
                dictSession.Add(session.SessionID, session);

                return session;
            }
        }

        /// <summary>
        /// 创建客服与客服的会话
        /// </summary>
        /// <param name="seat1"></param>
        /// <param name="seat2"></param>
        /// <returns></returns>
        public S2SSession CreateS2SSession(string seat1ID, string seat2ID)
        {
            lock (syncobj)
            {
                Seat seat1 = SeatManager.Instance.GetSeat(seat1ID);
                Seat seat2 = SeatManager.Instance.GetSeat(seat2ID);

                S2SSession session = new S2SSession(seat1, seat2);
                session.State = SessionState.Talking;

                seat1.AddSession(session);
                seat2.AddSession(session);

                //添加到会话列表中
                dictSession.Add(session.SessionID, session);

                return session;
            }
        }

        #region 会话操作

        /// <summary>
        /// 结束会话
        /// </summary>
        /// <param name="sessionID"></param>
        public void CloseSession(string sessionID)
        {
            lock (syncobj)
            {
                if (dictSession.ContainsKey(sessionID))
                {
                    //关闭后，把会话加入到关闭的会话列表中
                    Session session = dictSession[sessionID];
                    if (session != null)
                    {
                        if (session is P2SSession)
                        {
                            P2SSession ps = session as P2SSession;
                            ps.EndTime = DateTime.Now;
                            ps.State = SessionState.Closed;
                            closeSession[sessionID] = ps;

                            ps.Seat.RemoveSession(ps);
                            ps.User.RemoveSession(ps);
                        }
                        else if (session is S2SSession)
                        {
                            S2SSession ss = session as S2SSession;
                            ss.State = SessionState.Closed;
                            closeSession[sessionID] = ss;

                            ss.Owner.RemoveSession(ss);
                            ss.Friend.RemoveSession(ss);
                        }
                        else if (session is P2PSession)
                        {
                            P2PSession pps = session as P2PSession;
                            pps.State = SessionState.Closed;
                            closeSession[sessionID] = pps;

                            pps.Owner.RemoveSession(pps);
                            pps.Friend.RemoveSession(pps);
                        }
                    }

                    RemoveSession(sessionID);
                }
            }
        }

        /// <summary>
        /// 添加会话
        /// </summary>
        /// <param name="session"></param>
        public void AddSession(Session session)
        {
            lock (syncobj)
            {
                if (!dictSession.ContainsKey(session.SessionID))
                {
                    dictSession.Add(session.SessionID, session);
                }
            }
        }

        /// <summary>
        /// 移除会话
        /// </summary>
        /// <param name="sessionID"></param>
        public void RemoveSession(string sessionID)
        {
            lock (syncobj)
            {
                if (dictSession.ContainsKey(sessionID))
                {
                    dictSession.Remove(sessionID);
                }
            }
        }

        /// <summary>
        /// 会话是否存在
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public bool ExistsSession(string sessionID)
        {
            lock (syncobj)
            {
                return dictSession.ContainsKey(sessionID);
            }
        }

        #endregion

        /// <summary>
        /// 是否超时
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public bool IsTimeout(string sessionID)
        {
            lock (syncobj)
            {
                return timeoutSessions.Contains(sessionID);
            }
        }

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public Session GetSession(string sessionID)
        {
            lock (syncobj)
            {
                if (dictSession.ContainsKey(sessionID))
                {
                    return dictSession[sessionID];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取会话列表
        /// </summary>
        /// <returns></returns>
        public IList<Session> GetSessions()
        {
            lock (syncobj)
            {
                return new List<Session>(dictSession.Values);
            }
        }

        #endregion

        #region 获取历史会话

        /// <summary>
        /// 获取指定用户历史会话
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IList<P2SSession> GetUserSessionsFromDB(string seatID, string userID, DateTime beginTime, DateTime endTime)
        {
            IList<P2SSession> list = new List<P2SSession>();

            WhereClip where = WhereClip.All;
            where &= t_P2SSession._.SeatID == seatID;
            where &= !t_P2SSession._.SeatID.IsNull();
            where &= t_P2SSession._.UserID.Like(userID + "%");
            where &= t_P2SSession._.StartTime.Between(beginTime, endTime);
            var p2slist = dbSession.From<t_P2SSession>().Where(where).OrderBy(t_P2SSession._.StartTime.Desc).ToList();

            return ConvertToP2SSessions(p2slist);
        }

        /// <summary>
        /// 转换会话列表
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public IList<P2SSession> ConvertToP2SSessions(IList<t_P2SSession> sessions)
        {
            IList<P2SSession> list = new List<P2SSession>();
            foreach (t_P2SSession session in sessions)
            {
                P2SSession ps = DataUtils.ConvertType<t_P2SSession, P2SSession>(session);
                ps.User = UserManager.Instance.GetUser(session.UserID);

                if (!string.IsNullOrEmpty(session.SeatID))
                    ps.Seat = SeatManager.Instance.GetSeat(session.SeatID);

                list.Add(ps);
            }
            return list;
        }


        /// <summary>
        /// 转换会话列表
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public IList<S2SSession> ConvertToS2SSessions(IList<t_S2SSession> sessions)
        {
            IList<S2SSession> list = new List<S2SSession>();
            foreach (t_S2SSession session in sessions)
            {
                S2SSession ps = DataUtils.ConvertType<t_S2SSession, S2SSession>(session);
                ps.Owner = SeatManager.Instance.GetSeat(session.SeatID);
                ps.Friend = SeatManager.Instance.GetSeat(session.FriendID);

                list.Add(ps);
            }
            return list;
        }

        #endregion
    }
}
