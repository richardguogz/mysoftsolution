﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MySoft.Web;
using System.Text;
using LiveChat.Entity;
using LiveChat.Utils;

namespace LiveChat.Web
{
    public partial class Transfer : ParentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = null;
            string password = null;
            UserType userType = UserType.TempUser;

            if (userid == null)
            {
                string tempuserid = GetIDValue("tempuserid");
                if (string.IsNullOrEmpty(tempuserid))
                {
                    tempuserid = MakeUniqueKey(10, "v");
                    SaveIDValue("tempuserid", tempuserid);
                }
                userid = tempuserid;
                password = userid;
                userType = UserType.TempUser;

                SaveIDValue("userid", userid);
            }

            string companyID = Request["CompanyID"];
            bool online = GetSeatOnline(companyID);
            if (!online)
            {
                //留言入口
                string url = "/Leave.aspx?CompanyID=" + companyID;
                Response.Redirect(url);
            }
            else
            {
                Guid clientID = Guid.NewGuid();
                IMResult result = service.Login(clientID, userType, userid, password);
                if (result == IMResult.Successful)
                {
                    string url = "/Chat.aspx?SkinID=" + Request["SkinID"] + "&CompanyID=" + companyID + "&ClientID=" + clientID;
                    Response.Redirect(url);
                }
                else
                {
                    Response.Write("关联网站用户信息失败！");
                }
            }
        }

        /// <summary>
        /// 获取客服在线状态
        /// </summary>
        /// <returns></returns>
        private bool GetSeatOnline(string companyID)
        {
            return service.GetSeatOnline(companyID);
        }

        public string MakeUniqueKey(int length, string prefix)
        {
            int prefixLength = prefix == null ? 0 : prefix.Length;
            //Check.Require(length >= 8, "length must be greater than 0!");
            //Check.Require(length > prefixLength, "length must be greater than prefix length!");

            string chars = "1234567890";//abcdefghijklmnopqrstuvwxyz
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            if (prefixLength > 0)
            {
                sb.Append(prefix);
            }
            int dupCount = 0;
            int preIndex = 0;
            for (int i = 0; i < length - prefixLength; ++i)
            {
                int index = rnd.Next(0, chars.Length);
                if (index == preIndex)
                {
                    ++dupCount;
                }
                sb.Append(chars[index]);
                preIndex = index;
            }
            if (dupCount >= length - prefixLength - 2)
            {
                rnd = new Random();
                return MakeUniqueKey(length, prefix);
            }

            return sb.ToString();
        }
    }
}
