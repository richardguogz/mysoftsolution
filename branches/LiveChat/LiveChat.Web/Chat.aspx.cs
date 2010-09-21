using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LiveChat.Entity;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Web
{
    public partial class Chat : ParentPage
    {
        protected string userID;
        protected string seatID;
        protected string seatCode;
        protected string skinID;
        protected string sessionID;
        protected string companyID;
        protected Company company;
        protected Seat seat;
        protected void Page_Load(object sender, EventArgs e)
        {
            userID = GetUserID();
            if (string.IsNullOrEmpty(userID))
            {
                Response.Write("��ҳ���Ѿ�ʧЧ�������´򿪻Ựҳ�棡");
                Response.End();
            }

            string loadSessionID = null;
            skinID = GetRequestParam<string>("SkinID", null);
            companyID = GetRequestParam<string>("CompanyID", null);
            seatCode = GetRequestParam<string>("SeatCode", null);

            string[] arr = GetSessionInfo(userID, companyID);
            if (arr == null || arr.Length == 1)
            {
                bool online = GetSeatOnline(companyID);
                string title = string.Empty;
                if (!online)
                {
                    title = "��û�пͷ�����Ϊ������";
                }
                else
                {
                    title = "������Ϣ����ͷ������Ự...";
                }

                this.Title = title;
                if (arr != null)
                {
                    loadSessionID = arr[0];
                }
            }
            else if (arr.Length > 1)
            {
                sessionID = arr[0];
                seatID = arr[3];
                seat = new Seat() { SeatName = arr[2], SeatCode = arr[3], Telephone = arr[4], MobileNumber = arr[5], Email = arr[6] };
                this.Title = "��������" + arr[1] + "�ͷ���" + arr[2] + "���Ự... ";

                loadSessionID = sessionID;
            }

            //���ö�ٵ�ҳ��
            AddEnumToPage(typeof(MessageType));

            //�ѷ���ҳ��ע�ᵽserver.aspx
            RegisterPageForAjax("server.aspx");

            //��ȡ��˾����
            company = service.GetCompany(companyID);

            Ad ad = service.GetAdFromIP(companyID, Request.UserHostAddress);
            if (ad == null)
            {
                //���ع��
                headerBox.InnerHtml = "û���ҵ��˵�����Ӧ�Ĺ�棡";

                adLogo.InnerHtml = string.Format("<a href=\"{0}\" title=\"{1}\" target='_blank'><img src=\"{2}\" width=\"150\" height=\"120\" border=\"0px\" /></a>", company.WebSite, company.CompanyName, company.CompanyLogo);
                adText.InnerHtml = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", company.WebSite, company.CompanyName);
            }
            else
            {
                headerBox.InnerHtml = this.Title;
                adBox.InnerHtml = string.Format("<a href='{0}' target='_blank' title='{1}'><img src='{2}' width='650px' height='50px' border='0px' alt='{1}' /></a>", ad.AdUrl, ad.AdTitle, ad.AdImgUrl);

                if (!string.IsNullOrEmpty(ad.AdLogoUrl) && !string.IsNullOrEmpty(ad.AdLogoImgUrl))
                {
                    adLogo.InnerHtml = string.Format("<a href='{0}' title='{1}' target='_blank'><img src='{2}' width='150px' height='120px' border='0px' alt='{1}' /></a>", ad.AdLogoUrl, ad.AdTitle, ad.AdLogoImgUrl);
                }
                else
                {
                    adLogo.InnerHtml = string.Format("<a href=\"{0}\" title=\"{1}\" target='_blank'><img src=\"{2}\" width=\"150\" height=\"120\" border=\"0px\" /></a>", company.WebSite, company.CompanyName, company.CompanyLogo);
                }

                if (!string.IsNullOrEmpty(ad.AdText) && !string.IsNullOrEmpty(ad.AdTextUrl))
                {
                    string[] adtext = ad.AdText.Split('|');
                    string[] adurl = ad.AdTextUrl.Split('|');
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < adtext.Length; i++)
                    {
                        sb.AppendFormat("<a href='{0}' target='_blank' title='{1}'>{2}</a>&nbsp;&nbsp;", adurl[i], ad.AdTitle, adtext[i]);
                    }
                    adText.InnerHtml = sb.ToString();
                }
                else
                {
                    adText.InnerHtml = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", company.WebSite, company.CompanyName);
                }
            }
        }

        /// <summary>
        /// ��ȡ�ͷ�����״̬
        /// </summary>
        /// <returns></returns>
        private bool GetSeatOnline(string companyID)
        {
            return service.GetSeatOnline(companyID);
        }

        /// <summary>
        /// ��ȡ�Ự��Ϣ
        /// </summary>
        /// <returns></returns>
        private string[] GetSessionInfo(string userID, string companyID)
        {
            P2CSession p = service.GetP2CSession(userID, companyID);
            Company company = service.GetCompany(companyID);
            return p == null ? null : (p.Seat == null ? new string[] { p.SessionID } : new string[] { p.SessionID, company.CompanyName, p.Seat.ShowName, p.Seat.SeatCode, p.Seat.Telephone, p.Seat.MobileNumber, p.Seat.Email });
        }
    }
}
