using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;

namespace LiveChat.Service.Manager
{
    public class LinkManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly LinkManager Instance = new LinkManager();

        public LinkManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region ILinkManager 成员

        /// <summary>
        /// 获取一条链接
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        public Link GetLink(string replyID)
        {
            lock (syncobj)
            {
                t_Link reply = dbSession.Single<t_Link>(t_Link._.LinkID == replyID);
                Link rLink = DataUtils.ConvertType<t_Link, Link>(reply);
                return rLink;
            }
        }

        /// <summary>
        /// 获取所有链接
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Link> GetLinks(string companyID)
        {
            lock (syncobj)
            {
                IList<t_Link> list = dbSession.From<t_Link>().Where(t_Link._.CompanyID == companyID).ToList();
                IList<Link> replies = new List<Link>();
                foreach (t_Link reply in list)
                {
                    Link rLink = DataUtils.ConvertType<t_Link, Link>(reply);
                    replies.Add(rLink);
                }
                return replies;
            }
        }

        /// <summary>
        /// 添加快速链接信息
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public int AddLink(Link link)
        {
            lock (syncobj)
            {
                t_Link reply = new t_Link();
                reply.CompanyID = link.CompanyID;
                reply.Title = link.Title;
                reply.Url = link.Url;
                reply.AddTime = link.AddTime;
                dbSession.Save(reply);
                return reply.LinkID;
            }
        }

        /// <summary>
        /// 删除快速链接信息
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        public bool DeleteLink(int replyID)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_Link>(replyID) > 0;
            }
        }

        /// <summary>
        /// 修改快速链接信息
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public bool UpdateLink(Link rp)
        {
            lock (syncobj)
            {
                t_Link reply = new t_Link();
                reply.LinkID = rp.LinkID;
                reply.Title = rp.Title;
                reply.Url = rp.Url;
                reply.Attach();

                return dbSession.Save(reply) > 0;
            }
        }

        #endregion
    }
}
