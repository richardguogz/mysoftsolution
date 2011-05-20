using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using MySoft.Data;

namespace LiveChat.Service.Manager
{
    /// <summary>
    /// 公司管理类
    /// </summary>
    public class CompanyManager
    {
        private DbSession dbSession;
        private static Dictionary<string, Company> dictCompany = new Dictionary<string, Company>();
        public static readonly CompanyManager Instance = new CompanyManager();

        /// <summary>
        /// 初始化公司
        /// </summary>
        public CompanyManager()
        {
            this.dbSession = DataAccess.DbLiveChat;
        }

        /// <summary>
        /// 加载公司
        /// </summary>
        public void LoadCompany()
        {
            //清除
            dictCompany.Clear();

            List<t_Company> list = dbSession.From<t_Company>().ToList();
            List<t_Seat> list1 = dbSession.From<t_Seat>().ToList();
            list.ForEach(delegate(t_Company cp)
            {
                Company company = DataHelper.ConvertType<t_Company, Company>(cp);
                dictCompany.Add(company.CompanyID, company);

                List<t_Seat> l = list1.FindAll(delegate(t_Seat st)
                {
                    if (st.CompanyID == cp.CompanyID)
                    {
                        return true;
                    }
                    return false;
                });

                l.ForEach(delegate(t_Seat st)
                {
                    Seat seat = DataHelper.ConvertType<t_Seat, Seat>(st);
                    if (string.IsNullOrEmpty(seat.Introduction))
                    {
                        seat.Introduction = string.Format("您好，我是{0}客服，很高兴为您在线服务！", company.CompanyName);
                    }
                    company.AddSeat(seat);
                });
            });
        }

        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool AddCompany(Company company)
        {

            if (!dictCompany.ContainsKey(company.CompanyID))
            {
                t_Company c = new t_Company()
                {
                    CompanyID = company.CompanyID,
                    CompanyName = company.CompanyName,
                    CompanyLogo = company.CompanyLogo,
                    ChatWebSite = company.ChatWebSite,
                    IsHeadquarters = false,
                    WebSite = company.WebSite,
                    AddTime = DateTime.Now
                };

                int ret = dbSession.Save(c);
                if (ret > 0)
                {
                    if (company.Seats == null)
                    {
                        company.Seats = new SeatList();
                    }

                    dictCompany.Add(company.CompanyID, company);
                }
            }

            return true;

        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool DeleteCompany(string companyID)
        {

            if (dictCompany.ContainsKey(companyID))
            {
                using (var trans = dbSession.BeginTrans())
                {
                    try
                    {
                        int ret = trans.Delete<t_Seat>(t_Seat._.CompanyID == companyID);
                        ret += trans.Delete<t_Company>(t_Company._.CompanyID == companyID);

                        if (ret > 0)
                        {
                            dictCompany.Remove(companyID);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }

            return true;

        }

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompany(string companyID)
        {

            if (!dictCompany.ContainsKey(companyID))
            {
                return null;
            }
            return dictCompany[companyID];

        }

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompanyForName(string companyName)
        {

            foreach (Company company in dictCompany.Values)
            {
                if (company.CompanyName == companyName)
                {
                    return company;
                }
            }
            return null;

        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public IList<Company> GetCompanies()
        {

            return new List<Company>(dictCompany.Values);

        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool UpdateCompany(Company company)
        {

            t_Company c = dbSession.Single<t_Company>(t_Company._.CompanyID == company.CompanyID);
            c.CompanyName = company.CompanyName;
            c.CompanyLogo = company.CompanyLogo;
            c.WebSite = company.WebSite;
            c.ChatWebSite = company.ChatWebSite;
            c.Attach();

            bool ret = dbSession.Save(c) > 0;
            if (ret)
            {
                Company cc = dictCompany[c.CompanyID];
                cc.CompanyName = c.CompanyName;
                cc.CompanyLogo = c.CompanyLogo;
                cc.WebSite = c.WebSite;
                cc.ChatWebSite = c.ChatWebSite;
            }

            return ret;

        }
    }
}
