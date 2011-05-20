using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;
using LiveChat.Entity;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 留言管理
    /// </summary>
    public class LeaveManager
    {
        private DbSession dbSession;
        public static readonly LeaveManager Instance = new LeaveManager();

        public LeaveManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region LeaveManager 成员

        #region 留言信息

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        public int AddLeave(Leave leave)
        {

            t_Leave msg = DataHelper.ConvertType<Leave, t_Leave>(leave);
            return dbSession.Save(msg);

        }

        /// <summary>
        /// 获取留言信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Leave GetLeave(int id)
        {

            t_Leave leave = dbSession.Single<t_Leave>(t_Leave._.ID == id);
            if (leave == null) return null;

            return leave.As<Leave>();

        }

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLeave(int id)
        {
            return dbSession.Delete<t_Leave>(t_Leave._.ID == id) > 0;
        }

        /// <summary>
        /// 分页获取留言信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataView<IList<Leave>> GetLeaves(string companyID, DateTime beginTime, DateTime endTime, int pIndex, int pSize)
        {
            WhereClip where = t_Leave._.CompanyID == companyID;
            where &= t_Leave._.AddTime.Between(beginTime, endTime);

            OrderByClip order = t_Leave._.AddTime.Desc;

            var page = dbSession.From<t_Leave>().Where(where).OrderBy(order).GetPage(pSize);


            DataView<IList<Leave>> dataPage = new DataView<IList<Leave>>(pSize);
            dataPage.PageIndex = pIndex;
            dataPage.RowCount = page.RowCount;
            dataPage.DataSource = page.ToList(pIndex).ConvertTo<Leave>().OriginalData;

            foreach (Leave leave in dataPage.DataSource)
            {
                leave.PostArea = PHCZIP.Get(leave.PostIP);
            }

            return dataPage;
        }

        #endregion

        #endregion
    }
}
