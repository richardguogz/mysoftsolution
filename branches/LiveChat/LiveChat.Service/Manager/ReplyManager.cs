using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 回复管理
    /// </summary>
    public class ReplyManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly ReplyManager Instance = new ReplyManager();

        public ReplyManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region IReplyManager 成员

        /// <summary>
        /// 获取一条回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        public Reply GetReply(string replyID)
        {
            lock (syncobj)
            {
                t_Reply reply = dbSession.Single<t_Reply>(t_Reply._.ReplyID == replyID);
                Reply rReply = DataUtils.ConvertType<t_Reply, Reply>(reply);
                return rReply;
            }
        }

        /// <summary>
        /// 获取所有回复
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Reply> GetReplys(string companyID)
        {
            lock (syncobj)
            {
                IList<t_Reply> list = dbSession.From<t_Reply>().Where(t_Reply._.CompanyID == companyID).ToList();
                IList<Reply> replies = new List<Reply>();
                foreach (t_Reply reply in list)
                {
                    Reply rReply = DataUtils.ConvertType<t_Reply, Reply>(reply);
                    replies.Add(rReply);
                }
                return replies;
            }
        }

        /// <summary>
        /// 添加快速回复信息
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public int AddReply(Reply rp)
        {
            lock (syncobj)
            {
                t_Reply reply = new t_Reply();
                reply.CompanyID = rp.CompanyID;
                reply.Title = rp.Title;
                reply.Content = rp.Content;
                reply.AddTime = rp.AddTime;
                dbSession.Save(reply);
                return reply.ReplyID;
            }
        }

        /// <summary>
        /// 删除快速回复信息
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        public bool DeleteReply(int replyID)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_Reply>(replyID) > 0;
            }
        }

        /// <summary>
        /// 修改快速回复信息
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public bool UpdateReply(Reply rp)
        {
            lock (syncobj)
            {
                t_Reply reply = new t_Reply();
                reply.ReplyID = rp.ReplyID;
                reply.Title = rp.Title;
                reply.Content = rp.Content;
                reply.Attach();

                return dbSession.Save(reply) > 0;
            }
        }

        #endregion
    }
}
