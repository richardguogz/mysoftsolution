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
                Logger.Instance.WriteLog(ex.ToString());
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

        #region 修改客服信息

        public bool UpdateSeatFace(string seatID, t_Seat cf)
        {
            lock (syncobj)
            {
                string companyID = seatID.Split('_')[0];
                string seatCode = seatID.Split('_')[1];

                WhereClip where = t_Seat._.CompanyID == companyID && t_Seat._.SeatCode == seatCode;
                bool ret = dbSession.Update<t_Seat>(t_Seat._.FaceImage, cf.FaceImage, where) > 0;
                if (ret)
                {
                    Seat seat = SeatManager.Instance.GetSeat(seatID);
                    seat.FaceImage = cf.FaceImage;
                }

                return ret;
            }
        }

        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateSeat(string seatID, t_Seat cf)
        {
            lock (syncobj)
            {
                string companyID = seatID.Split('_')[0];
                string seatCode = seatID.Split('_')[1];

                WhereClip where = t_Seat._.CompanyID == companyID && t_Seat._.SeatCode == seatCode;
                bool ret = dbSession.Update<t_Seat>
                    (new Field[] { t_Seat._.SeatName, t_Seat._.Email, t_Seat._.Telephone, t_Seat._.MobileNumber, t_Seat._.Sign, t_Seat._.Introduction, t_Seat._.SeatType },
                    new object[] { cf.SeatName, cf.Email, cf.Telephone, cf.MobileNumber, cf.Sign, cf.Introduction, cf.SeatType }, where) > 0;

                if (ret)
                {
                    Seat seat = SeatManager.Instance.GetSeat(seatID);
                    seat.SeatName = cf.SeatName;
                    seat.Email = cf.Email;
                    seat.Telephone = cf.Telephone;
                    seat.MobileNumber = cf.MobileNumber;
                    seat.Sign = cf.Sign;
                    seat.Introduction = cf.Introduction;
                    seat.SeatType = cf.SeatType;
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
        public bool AddSeat(Seat seat)
        {
            lock (syncobj)
            {
                t_Seat s = DataHelper.ConvertType<Seat, t_Seat>(seat);
                s.AddTime = DateTime.Now;

                bool ret = dbSession.Save(s) > 0;
                if (ret)
                {
                    Company company = CompanyManager.Instance.GetCompany(seat.CompanyID);
                    company.AddSeat(seat);
                }

                return ret;
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

        #endregion

    }
}
