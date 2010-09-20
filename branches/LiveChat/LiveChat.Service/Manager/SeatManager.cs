using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;
using System.Timers;
using LiveChat.Utils;

namespace LiveChat.Service.Manager
{
    public class SeatManager
    {
        private DbSession dbSession;
        private Timer timer;
        private static readonly object syncobj = new object();
        public static readonly SeatManager Instance = new SeatManager();

        public SeatManager()
        {
            this.dbSession = DataAccess.DbLiveChat;

            this.timer = new Timer();
            this.timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            this.timer.AutoReset = true;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                lock (syncobj)
                {
                    //检测超时
                    foreach (Company company in CompanyManager.Instance.GetCompanies())
                    {
                        foreach (Seat seat in company.Seats)
                        {
                            if (seat.State == OnlineState.Offline) continue;

                            //如果超时，则置为离开状态
                            if (DateTime.Now.Subtract(seat.RefreshTime).TotalMinutes > ServiceConfig.TimeoutExitInterval)
                            {
                                seat.State = OnlineState.Offline;
                            }
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
                timer.Start();
            }
        }

        /// <summary>
        /// 获取客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <returns></returns>
        public Seat GetSeat(string companyID, string seatCode)
        {
            lock (syncobj)
            {
                //获取公司
                Company company = CompanyManager.Instance.GetCompany(companyID);
                if (company == null)
                {
                    return null;
                }

                //添加一个新客服
                foreach (Seat seat in company.Seats)
                {
                    if (seat.SeatCode == seatCode)
                    {
                        return seat;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取客服
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="seatCode"></param>
        /// <returns></returns>
        public Seat GetSeatForCompanyName(string companyName, string seatCode)
        {
            lock (syncobj)
            {
                //获取公司
                Company company = CompanyManager.Instance.GetCompanyForName(companyName);
                if (company == null)
                {
                    return null;
                }

                //添加一个新客服
                foreach (Seat seat in company.Seats)
                {
                    if (seat.SeatCode == seatCode)
                    {
                        return seat;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取客服
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        public Seat GetSeat(string seatID)
        {
            lock (syncobj)
            {
                string[] names = seatID.Split('_');
                return GetSeat(names[0], names[1]);
            }
        }

        #region ISeatManager 成员

        /// <summary>
        /// 获取客服群列表
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public IList<SeatGroup> GetGroups(string seatID, bool isJoin)
        {
            lock (syncobj)
            {
                Seat seat = GetSeat(seatID);
                IList<SeatGroup> list = new List<SeatGroup>();
                foreach (SeatGroup group in GroupManager.Instance.GetSeatGroups())
                {
                    //不是本公司的群，则继续
                    if (group.Company.CompanyID != seat.CompanyID)
                    {
                        continue;
                    }

                    if (isJoin)
                    {
                        if (group.Seats.Exists(seat))
                        {
                            list.Add(group);
                        }
                    }
                    else
                    {
                        if (!group.Seats.Exists(seat))
                        {
                            list.Add(group);
                        }
                    }
                }
                return list;
            }
        }

        #endregion
    }
}
