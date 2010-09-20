using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using LiveChat.Utils;

namespace LiveChat.Interface
{
    public interface IUserService : ICommonService
    {
        #region 登录相关

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="id">要验证的使用者ID</param>
        /// <param name="password">要验证的使用者密码</param>
        /// <returns></returns>
        IMResult ValidateUser(string id, string password);

        /// <summary>
        /// 通过用户ID获取一个用户
        /// </summary>
        /// <param name="userID"></param>
        User GetUser(string userID);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="userType"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IMResult Login(Guid clientID, UserType userType, string userID, string password);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userID"></param>
        void Logout(string userID);

        #endregion

        /// <summary>
        /// 获取用户所有会话
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<P2SSession> GetP2SSessions(string userID);

        /// <summary>
        /// 获取用户与公司的会话
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        P2CSession GetP2CSession(string userID, string companyID);

        /// <summary>
        /// 发送消息到公司
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        string SendP2CMessage(MessageType type, string userID, string companyID, string seatCode, string senderIP, string content);

        /// <summary>
        /// 客服发送消息到群
        /// </summary>
        /// <param name="type"></param>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        void SendUGMessage(MessageType type, Guid groupID, string userID, string senderIP, string content);

        /// <summary>
        /// 获取用户相关信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        UserInfo GetUserInfo(string userID);

        /// <summary>
        /// 获取用户群用户列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        IList<User> GetGroupUsers(Guid groupID);

        /// <summary>
        /// 获取用户群列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<UserGroup> GetJoinGroups(string userID);

        /// <summary>
        /// 获取用户未加入的群列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<UserGroup> GetNoJoinGroups(string userID);

        /// <summary>
        /// 添加到组中
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        void AddToGroup(string userID, Guid groupID);

        /// <summary>
        /// 退出指定的群
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        void ExitGroup(string userID, Guid groupID);

        /// <summary>
        /// 获取用户未读的消息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<Message> GetNoReadMessages(string userID);

        /// <summary>
        /// 获取会话中所有消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<Message> GetUGMessages(Guid groupID, string userID);

        /// <summary>
        /// 获取会话中所有消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<Message> GetUGHistoryMessages(Guid groupID, string userID);

        /// <summary>
        /// 获取会话中所有消息(指定时间后)
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<Message> GetUGHistoryMessages(Guid groupID, string userID, DateTime lastGetTime);

        #region 获取公司相关信息

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        Company GetCompany(string companyID);

        /// <summary>
        /// 获取公司下所有客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IList<Seat> GetCompanySeats(string companyID);

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        IList<Company> GetCompanies();

        #endregion

        /// <summary>
        /// 获取公司客服是否在线
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        bool GetSeatOnline(string companyID);

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="postIP"></param>
        /// <returns></returns>
        int AddLeave(string companyID, string name, string telephone, string email, string title, string body, string postIP);

        #region 广告信息

        /// <summary>
        /// 通过IP获取广告
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        Ad GetAdFromIP(string companyID, string ip);

        #endregion
    }
}
