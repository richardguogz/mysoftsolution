using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;
using LiveChat.Entity;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 地区管理
    /// </summary>
    public class AreaManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        public static readonly AreaManager Instance = new AreaManager();

        public AreaManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        #region 地区信息

        /// <summary>
        /// 获取地区信息
        /// </summary>
        /// <returns></returns>
        public IList<Area> GetAreas()
        {
            lock (syncobj)
            {
                return dbSession.From<t_Area>().OrderBy(t_Area._.AreaID.Asc)
                    .ToList().ConvertTo<Area>();
            }
        }

        #endregion
    }
}
