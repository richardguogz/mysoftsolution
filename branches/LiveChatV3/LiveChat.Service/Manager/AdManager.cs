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

            t_Ad msg = DataHelper.ConvertType<Ad, t_Ad>(ad);
            return dbSession.Save(msg);

        }

        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public int UpdateAd(Ad ad)
        {

            t_Ad msg = DataHelper.ConvertType<Ad, t_Ad>(ad);
            msg.Attach();
            return dbSession.Save(msg);

        }

        /// <summary>
        /// 获取广告通过IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Ad GetAdFromIP(string companyID, string ip)
        {
            string area = PHCZIP.Get(ip);
            WhereClip where = t_Ad._.CompanyID == companyID || t_Ad._.IsCommon == 1;
            IList<t_Ad> ads = dbSession.From<t_Ad>().Where(where && new Field("replace(AdArea,'|','')") == area)
                 .OrderBy(t_Ad._.IsCommon.Asc && t_Ad._.AddTime.Desc).ToList();

            if (ads.Count == 0)
            {
                where = t_Ad._.CompanyID == companyID || t_Ad._.IsCommon == 1;
                where &= t_Ad._.IsDefault == true;
                ads = dbSession.From<t_Ad>().Where(where).OrderBy(t_Ad._.IsCommon.Asc && t_Ad._.AddTime.Desc).ToList();
            }

            if (ads.Count == 0) return null;

            t_Ad ad = ads[new Random().Next(ads.Count)];
            return ad.As<Ad>();

        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ad GetAd(int id)
        {

            t_Ad ad = dbSession.Single<t_Ad>(t_Ad._.ID == id);
            if (ad == null) return null;

            return ad.As<Ad>();

        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAd(int id)
        {

            return dbSession.Delete<t_Ad>(t_Ad._.ID == id) > 0;

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

            WhereClip where = t_Ad._.CompanyID == companyID;
            where &= t_Ad._.AddTime.Between(beginTime, endTime);

            var list = dbSession.From<t_Ad>().Where(where).ToList();

            return list.ConvertTo<Ad>().OriginalData;

        }

        #endregion

    }
}
