using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;
using System.Timers;

namespace LiveChat.Service.Manager
{
    public class MessageManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly MessageManager Instance = new MessageManager();

        public MessageManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region IMessageManager 成员

        /// <summary>
        /// 添加用户到客服的消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddP2CMessage(P2CSession session, string senderID, string senderName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                //组装信息
                P2SMessage msg = new P2SMessage();
                msg.SenderID = senderID;
                msg.SenderName = senderName;
                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                session.AddMessage(msg);
            }
        }

        /// <summary>
        /// 添加用户到客服的消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="receiverID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddP2SMessage(P2SSession session, string senderID, string senderName, string receiverID, string receiverName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                //组装信息
                P2SMessage msg = new P2SMessage();
                msg.SenderID = senderID;
                msg.SenderName = senderName;
                msg.ReceiverID = receiverID;
                msg.ReceiverName = receiverName;
                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                session.AddMessage(msg);
            }
        }

        /// <summary>
        /// 添加客服到群的消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddSGMessage(SeatGroup group, string senderID, string senderName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                GPMessage msg = new GPMessage();
                msg.GroupID = group.GroupID;
                msg.SenderID = senderID;
                msg.SenderName = senderName;
                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                group.AddMessage(msg);
            }
        }

        /// <summary>
        /// 添加用户到群的消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddUGMessage(UserGroup group, string senderID, string senderName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                GPMessage msg = new GPMessage();
                msg.GroupID = group.GroupID;
                msg.SenderID = senderID;
                msg.SenderName = senderName;
                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                group.AddMessage(msg);
            }
        }

        /// <summary>
        /// 添加用户到用户的消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddP2PMessage(P2PSession session, string senderID, string senderName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                //组装信息
                P2PMessage msg = new P2PMessage();
                msg.SenderID = senderID;
                msg.SenderName = senderName;

                if (session.Owner.UserID == senderID)
                {
                    msg.ReceiverID = session.Friend.UserID;
                    msg.ReceiverName = session.Friend.UserName;
                }
                else
                {
                    msg.ReceiverID = session.Owner.UserID;
                    msg.ReceiverName = session.Owner.UserName;
                }

                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                session.AddMessage(msg);
            }
        }

        /// <summary>
        /// 添加客服到客服的消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        public void AddS2SMessage(S2SSession session, string senderID, string senderName, string senderIP, MessageType msgType, string content)
        {
            lock (syncobj)
            {
                //组装信息
                S2SMessage msg = new S2SMessage();
                msg.SenderID = senderID;
                msg.SenderName = senderName;

                if (session.Owner.SeatID == senderID)
                {
                    msg.ReceiverID = session.Friend.SeatID;
                    msg.ReceiverName = session.Friend.SeatName;
                }
                else
                {
                    msg.ReceiverID = session.Owner.SeatID;
                    msg.ReceiverName = session.Owner.SeatName;
                }

                msg.SenderIP = senderIP;
                msg.Type = msgType;
                msg.Content = content;

                session.AddMessage(msg);
            }
        }

        #endregion

        #region IMessageManager 成员

        /// <summary>
        /// 获取历史消息
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public DataView<IList<P2SMessage>> GetP2SHistoryMessagesFromDB(Guid sid, int pIndex, int pSize)
        {
            WhereClip where = t_P2SMessage._.SID == sid;
            IList<P2SMessage> list = new List<P2SMessage>();
            PageSection<t_P2SMessage> page = dbSession.From<t_P2SMessage>().Where(where).OrderBy(t_P2SMessage._.SendTime.Asc).GetPage(pSize);

            DataView<IList<P2SMessage>> dataPage = new DataView<IList<P2SMessage>>(pSize);
            dataPage.PageIndex = pIndex;
            dataPage.RowCount = page.RowCount;
            dataPage.DataSource = ConvertToP2SMessages(page.ToList(pIndex));

            return dataPage;
        }

        #region 获取客服与用户会话

        /// <summary>
        /// 转换消息列表
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public IList<P2SMessage> ConvertToP2SMessages(IList<t_P2SMessage> msgs)
        {
            IList<P2SMessage> list = new List<P2SMessage>();
            foreach (t_P2SMessage msg in msgs)
            {
                P2SMessage pm = DataUtils.ConvertType<t_P2SMessage, P2SMessage>(msg);
                list.Add(pm);
            }
            return list;
        }

        #endregion

        #region 获取客服与客服会话

        /// <summary>
        /// 获取历史消息
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public DataView<IList<S2SMessage>> GetS2SHistoryMessagesFromDB(Guid sid, DateTime fromDate, DateTime toDate, int pIndex, int pSize)
        {
            WhereClip where = t_S2SMessage._.SID == sid;
            where &= t_S2SMessage._.SendTime > fromDate && t_S2SMessage._.SendTime < toDate;

            IList<S2SMessage> list = new List<S2SMessage>();
            PageSection<t_S2SMessage> page = dbSession.From<t_S2SMessage>().Where(where).OrderBy(t_S2SMessage._.SendTime.Asc).GetPage(pSize);

            DataView<IList<S2SMessage>> dataPage = new DataView<IList<S2SMessage>>(pSize);
            dataPage.PageIndex = pIndex;
            dataPage.RowCount = page.RowCount;
            dataPage.DataSource = ConvertToS2SMessages(page.ToList(pIndex));

            return dataPage;
        }

        /// <summary>
        /// 转换消息列表
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public IList<S2SMessage> ConvertToS2SMessages(IList<t_S2SMessage> msgs)
        {
            IList<S2SMessage> list = new List<S2SMessage>();
            foreach (t_S2SMessage msg in msgs)
            {
                S2SMessage pm = DataUtils.ConvertType<t_S2SMessage, S2SMessage>(msg);
                list.Add(pm);
            }
            return list;
        }

        #endregion

        #endregion
    }
}
