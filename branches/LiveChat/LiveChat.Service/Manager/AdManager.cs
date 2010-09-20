using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;
using LiveChat.Entity;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 广告管理
    /// </summary>
    public class AdManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly AdManager Instance = new AdManager();

        public AdManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region 广告管理

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public int AddAd(Ad ad)
        {
            lock (syncobj)
            {
                t_Ad msg = DataUtils.ConvertType<Ad, t_Ad>(ad);
                return dbSession.Save(msg);
            }
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public int UpdateAd(Ad ad)
        {
            lock (syncobj)
            {
                t_Ad msg = DataUtils.ConvertType<Ad, t_Ad>(ad);
                msg.Attach();
                return dbSession.Save(msg);
            }
        }

        /// <summary>
        /// 获取广告通过IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Ad GetAdFromIP(string companyID, string ip)
        {
            lock (syncobj)
            {
                string area = PHCZIP.Get(ip);
                WhereClip where = WhereClip.Bracket(t_Ad._.CompanyID == companyID || t_Ad._.IsCommon == 1);
                t_Ad ad = dbSession.From<t_Ad>().Where(where && new Field("replace(AdArea,'|','')") == area)
                    .OrderBy(t_Ad._.IsCommon.Asc && t_Ad._.AddTime.Desc).ToSingle();

                if (ad == null)
                {
                    where = WhereClip.Bracket(t_Ad._.CompanyID == companyID || t_Ad._.IsCommon == 1);
                    where &= t_Ad._.IsDefault == true;
                    ad = dbSession.From<t_Ad>().Where(where).OrderBy(t_Ad._.IsCommon.Asc && t_Ad._.AddTime.Desc).ToSingle();
                    if (ad == null) return null;
                }
                return ad.As<Ad>();
            }
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ad GetAd(int id)
        {
            lock (syncobj)
            {
                t_Ad ad = dbSession.Single<t_Ad>(t_Ad._.ID == id);
                if (ad == null) return null;

                return ad.As<Ad>();
            }
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAd(int id)
        {
            lock (syncobj)
            {
                return dbSession.Delete<t_Ad>(t_Ad._.ID == id) > 0;
            }
        }

        /// <summary>
        /// 分页获取广告信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IList<Ad> GetAds(string companyID, DateTime beginTime, DateTime endTime)
        {
            lock (syncobj)
            {
                WhereClip where = t_Ad._.CompanyID == companyID;
                where &= t_Ad._.AddTime.Between(beginTime, endTime);

                var list = dbSession.From<t_Ad>().Where(where).ToList();

                return list.ConvertTo<Ad>();
            }
        }

        #endregion

    }
}
