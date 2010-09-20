﻿using System;
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
        private static readonly object syncobj = new object();
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
                Company company = DataUtils.ConvertType<t_Company, Company>(cp);
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
                    Seat seat = DataUtils.ConvertType<t_Seat, Seat>(st);
                    company.AddSeat(seat);
                });
            });
        }

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompany(string companyID)
        {
            lock (syncobj)
            {
                if (!dictCompany.ContainsKey(companyID))
                {
                    return null;
                }
                return dictCompany[companyID];
            }
        }

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public Company GetCompanyForName(string companyName)
        {
            lock (syncobj)
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
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public IList<Company> GetCompanies()
        {
            lock (syncobj)
            {
                return new List<Company>(dictCompany.Values);
            }
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool UpdateCompany(Company company)
        {
            lock (syncobj)
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
}
