using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 客服配置管理类
    /// </summary>
    public class SeatConfigManager
    {
        private DbSession dbSession;
        private static readonly object syncobj = new object();
        private static Dictionary<string, SeatConfig> dictSeatConfig = new Dictionary<string, SeatConfig>();
        public static readonly SeatConfigManager Instance = new SeatConfigManager();

        /// <summary>
        /// 初始化公司
        /// </summary>
        public SeatConfigManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
            LoadSeatConfig();
        }

        /// <summary>
        /// 创建配置
        /// </summary>
        private void LoadSeatConfig()
        {
            dictSeatConfig.Clear();

            IList<Company> companys = CompanyManager.Instance.GetCompanies();
            List<t_Seat> list = dbSession.From<t_Seat>().OrderBy(t_Seat._.SeatCode.Asc).ToList();
            foreach (Company company in companys)
            {
                List<t_Seat> slist = list.FindAll(delegate(t_Seat s)
                 {
                     if (s.CompanyID == company.CompanyID)
                     {
                         return true;
                     }
                     return false;
                 });

                slist.ForEach(p =>
                {
                    SeatConfig config = DataUtils.ConvertType<t_Seat, SeatConfig>(p);
                    if (string.IsNullOrEmpty(config.Introduction))
                    {
                        config.Introduction = string.Format("您好，我是{0}客服，很高兴为您在线服务！", company.CompanyName);
                    }
                    dictSeatConfig.Add(config.SeatID, config);

                    Seat seat = DataUtils.ConvertType<t_Seat, Seat>(p);
                    if (!company.Seats.Exists(seat))
                    {
                        company.AddSeat(seat);
                    }
                });
            }
        }

        /// <summary>
        /// 获取客服配置
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public SeatConfig GetSeatConfig(string seatID)
        {
            lock (syncobj)
            {
                if (!dictSeatConfig.ContainsKey(seatID))
                {
                    return null;
                }
                return dictSeatConfig[seatID];
            }
        }

        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        public IList<SeatConfig> GetSeatConfigs()
        {
            lock (syncobj)
            {
                return new List<SeatConfig>(dictSeatConfig.Values);
            }
        }

        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateSeatConfig(SeatConfig cf)
        {
            lock (syncobj)
            {
                Seat seat = SeatManager.Instance.GetSeat(cf.SeatID);
                seat.SeatName = cf.SeatName;

                WhereClip where = t_Seat._.CompanyID == seat.CompanyID && t_Seat._.SeatCode == seat.SeatCode;
                bool ret = dbSession.Update<t_Seat>
                    (new Field[] { t_Seat._.SeatName, t_Seat._.Email, t_Seat._.Telephone, t_Seat._.MobileNumber, t_Seat._.Sign, t_Seat._.Introduction, t_Seat._.SeatType },
                    new object[] { cf.SeatName, cf.Email, cf.Telephone, cf.MobileNumber, cf.Sign, cf.Introduction, cf.SeatType }, where) > 0;

                if (ret)
                {
                    dictSeatConfig[cf.SeatID] = cf;
                }

                return ret;
            }
        }

        /// <summary>
        /// 修改客服密码
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool UpdatePassword(string seatID, string password)
        {
            lock (syncobj)
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                WhereClip where = t_Seat._.CompanyID == seat.CompanyID && t_Seat._.SeatCode == seat.SeatCode;
                bool ret = dbSession.Update<t_Seat>(t_Seat._.Password, password, where) > 0;
                if (ret)
                {
                    seat.Password = password;
                }
                return ret;
            }
        }

        /// <summary>
        /// 删除客服
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public bool DeleteSeat(string seatID)
        {
            lock (syncobj)
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                WhereClip where = t_Seat._.CompanyID == seat.CompanyID && t_Seat._.SeatCode == seat.SeatCode;
                bool ret = dbSession.Delete<t_Seat>(where) > 0;
                if (ret)
                {
                    dictSeatConfig.Remove(seatID);
                    Company company = CompanyManager.Instance.GetCompany(seat.CompanyID);
                    if (company.Seats.Exists(seat))
                    {
                        company.Seats.RemoveAll(p => p.SeatID == seat.SeatID);
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <param name="seat"></param>
        /// <returns></returns>
        public bool AddSeat(SeatConfig seat)
        {
            lock (syncobj)
            {
                t_Seat s = DataUtils.ConvertType<SeatConfig, t_Seat>(seat);
                s.AddTime = DateTime.Now;

                bool ret = dbSession.Save(s) > 0;
                if (ret)
                {
                    LoadSeatConfig();
                }

                return ret;
            }
        }

        public IList<SeatConfig> GetSeatConfigs(string companyID)
        {
            lock (syncobj)
            {
                var slist = new List<SeatConfig>(dictSeatConfig.Values);
                var list = slist.FindAll(p => p.CompanyID == companyID);
                foreach (SeatConfig sc in list)
                {
                    Seat seat = SeatManager.Instance.GetSeat(sc.SeatID);
                    sc.State = seat.State;
                }
                return list;
            }
        }

        /// <summary>
        /// 保存实体到数据库
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        public void UpdateSeat(string seatID, Field[] fields, object[] values)
        {
            lock (syncobj)
            {
                Seat seat = SeatManager.Instance.GetSeat(seatID);
                dbSession.Update<t_Seat>(fields, values, t_Seat._.CompanyID == seat.CompanyID && t_Seat._.SeatCode == seat.SeatCode);
            }
        }
    }
}
