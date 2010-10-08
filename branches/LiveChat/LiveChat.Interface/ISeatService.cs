using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;
using LiveChat.Utils;

namespace LiveChat.Interface
{
    public interface ISeatService : ICommonService
    {
        /// <summary>
        /// 验证客户端
        /// </summary>
        /// <param name="id">要验证的使用者ID</param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        bool ValidateClient(string id, Guid clientID);

        #region 用户相信信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        User GetUser(string userID);

        /// <summary>
        /// 改变客服的状态
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="state"></param>
        void ChangeSeatState(string seatID, OnlineState state);

        #endregion

        /// <summary>
        /// 获取客服信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        SeatMessage GetSeatMessage(string seatID);

        /// <summary>
        /// 修改备注名称
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RenameFriend(string seatID, string friendID, string name);

        /// <summary>
        /// 结束客服所有会话
        /// </summary>
        /// <param name="seatID"></param>
        void CloseAllSession(string seatID);

        #region 登录相关

        /// <summary>
        /// 通过seatID获取一个客服
        /// </summary>
        /// <param name="seatID"></param>
        Seat GetSeat(string seatID);

        /// <summary>
        /// 客服登录
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="password"></param>
        /// <param name="isvalidateManager"></param>
        /// <returns></returns>
        IMResult Login(Guid clientID, string companyID, string seatCode, string password);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="seatID"></param>
        void Logout(string seatID);

        /// <summary>
        /// 修改客服密码
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        bool UpdatePassword(string seatID, string oldpassword, string newpassword);

        #endregion

        /// <summary>
        /// 获取客服所有会话
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<P2SSession> GetP2SSessions(string seatID);

        /// <summary>
        /// 获取请求的会话列表
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        IList<P2CSession> GetP2CSessions(string seatID, SortType sortType);

        /// <summary>
        /// 自动接入给我的会话(每次一个)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">自动接入会话最大数</param>
        /// <returns>返回我的会话列表</returns>
        P2SSession AutoAcceptSession(string seatID, int maxCount);

        /// <summary>
        /// 接受会话
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">最大允许接入会话数</param>
        /// <param name="sessionIDs"></param>
        void AcceptSessions(string seatID, int maxCount, params string[] sessionIDs);

        /// <summary>
        /// 接受会话
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="maxCount">最大允许接入会话数</param>
        /// <param name="sessionID"></param>
        P2SSession AcceptSession(string seatID, int maxCount, string sessionID);

        /// <summary>
        /// 发送消息到客服(指定发送者，用于帮助回答)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sessionID"></param>
        /// <param name="seatID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        void SendP2SMessage(MessageType type, string sessionID, string seatID, string senderIP, string content);

        /// <summary>
        /// 客服发送消息到群
        /// </summary>
        /// <param name="type"></param>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        void SendSGMessage(MessageType type, Guid groupID, string seatID, string senderIP, string content);

        /// <summary>
        /// 获取公司
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        Company GetCompany(string companyID);

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        IList<Company> GetCompanies();

        /// <summary>
        /// 获取公司(参数为公司名称)
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        Company GetCompanyForName(string companyName);

        /// <summary>
        /// 添加客服群
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="groupName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        bool AddSeatGroup(string companyID, string groupName, int maxCount);

        /// <summary>
        /// 修改客服群
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="groupName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        bool UpdateSeatGroup(Guid groupID, string groupName, int maxCount);

        /// <summary>
        /// 删除客服群
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        bool DeleteSeatGroup(Guid groupID);

        /// <summary>
        /// 获取公司下所有群
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IList<SGroup> GetSeatGroups(string companyID);

        /// <summary>
        /// 获取公司下所有客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IList<Seat> GetCompanySeats(string companyID);

        /// <summary>
        /// 获取客服群客服列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        IList<Seat> GetGroupSeats(Guid groupID);

        /// <summary>
        /// 获取客服群列表
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<SeatGroup> GetJoinGroups(string seatID);

        /// <summary>
        /// 获取未加入的客服群列表
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<SeatGroup> GetNoJoinGroups(string seatID);

        /// <summary>
        /// 添加到群中
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        void AddToGroup(string seatID, Guid groupID);

        /// <summary>
        /// 退出指定的群
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="groupID"></param>
        void ExitGroup(string seatID, Guid groupID);

        /// <summary>
        /// 获取会话中所有消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<Message> GetSGMessages(Guid groupID, string seatID);

        /// <summary>
        /// 获取会话中所有消息
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<Message> GetSGHistoryMessages(Guid groupID, string seatID);

        /// <summary>
        /// 获取会话中所有消息(指定时间后)
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="seatID"></param>
        /// <param name="lastGetTime"></param>
        /// <returns></returns>
        IList<Message> GetSGHistoryMessages(Guid groupID, string seatID, DateTime lastGetTime);

        #region 快捷回复

        /// <summary>
        /// 获取快捷回复信息列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IList<Reply> GetReplys(string companyID);

        /// <summary>
        /// 添加快捷回复信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        int AddReply(string companyID, string title, string content);

        /// <summary>
        /// 删除快捷回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <returns></returns>
        bool DeleteReply(int replyID);

        /// <summary>
        /// 删除快捷回复
        /// </summary>
        /// <param name="replyID"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        bool UpdateReply(int replyID, string title, string content);

        #endregion

        #region 留言信息

        /// <summary>
        /// 获取留言信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Leave GetLeave(int id);

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteLeave(int id);

        /// <summary>
        /// 分页获取留言信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        DataView<IList<Leave>> GetLeaves(string companyID, DateTime beginTime, DateTime endTime, int pIndex, int pSize);

        #endregion

        #region 公司信息

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyName"></param>
        /// <param name="webSite"></param>
        /// <param name="companyLogo"></param>
        /// <param name="chatWebSite"></param>
        /// <returns></returns>
        bool UpdateCompany(string companyID, string companyName, string webSite, string companyLogo, string chatWebSite);

        #endregion

        #region 快捷链接

        /// <summary>
        /// 获取快捷链接信息列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        IList<Link> GetLinks(string companyID);

        /// <summary>
        /// 添加快捷链接信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        int AddLink(string companyID, string title, string url);

        /// <summary>
        /// 删除快捷链接
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        bool DeleteLink(int linkID);

        /// <summary>
        /// 删除快捷链接
        /// </summary>
        /// <param name="linkID"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool UpdateLink(int linkID, string title, string url);

        #endregion

        #region 配置信息

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="seatCode"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <param name="mobilenumber"></param>
        /// <param name="sign"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool AddSeat(string companyID, string seatCode, string seatName, string password, string email, string telephone, string mobilenumber, string sign, string remark, SeatType seattype);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        bool UpdateSeatFace(string seatID, byte[] buffer);

        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="nickName"></param>
        /// <param name="sign"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        bool UpdateSeat(string seatID, string seatName, string email, string telephone, string mobilenumber, string sign, string introduction, SeatType seattype);

        /// <summary>
        /// 删除客服
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        bool DeleteSeat(string seatID);

        #endregion

        #region 广告信息

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IList<Ad> GetAds(string companyID, DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteAd(int id);

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="area"></param>
        /// <param name="imgurl"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool UpdateAd(int id, string name, string title, string area, string imgurl, string url, string adLogoImgUrl, string adLogoUrl, string adText, string adTextUrl, bool isdefault, bool iscommon);

        /// <summary>
        /// 新增广告
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="area"></param>
        /// <param name="imgurl"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        bool AddAd(string companyID, string name, string title, string area, string imgurl, string url, string adLogoImgUrl, string adLogoUrl, string adText, string adTextUrl, bool isdefault, bool iscommon);

        #endregion

        #region 获取公用信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        IList<User> GetUsers();

        /// <summary>
        /// 获取地区信息
        /// </summary>
        /// <returns></returns>
        IList<Area> GetAreas();

        #endregion

        /// <summary>
        /// 获取与指定用户的所有会话(从数据库中查询)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IList<P2SSession> GetP2SSessionsFromDB(Seat seat, string userID, DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 重新加载缓存
        /// </summary>
        /// <returns></returns>
        void ReloadCache();

        #region 客服对客服

        /// <summary>
        /// 发送消息到客服
        /// </summary>
        /// <param name="type"></param>
        /// <param name="seat1ID"></param>
        /// <param name="seat2ID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        void SendS2SMessage(MessageType type, string seat1ID, string seat2ID, string senderIP, string content);

        /// <summary>
        /// 获取会话中所有历史消息(从内存中查询)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="fromID"></param>
        /// <returns></returns>
        IList<Message> GetS2SHistoryMessages(string seatID, string fromID);

        /// <summary>
        /// 获取历史消息(指定时间后)
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="fromID"></param>
        /// <param name="lastGetTime"></param>
        /// <returns></returns>
        IList<Message> GetS2SHistoryMessages(string seatID, string fromID, DateTime lastGetTime);

        /// <summary>
        /// 获取会话中所有历史消息(从数据库中查询)
        /// </summary>
        /// <param name="sid">会话唯一标识</param>
        /// <param name="getDate"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        DataView<IList<S2SMessage>> GetS2SHistoryMessagesFromDB(string seatID, string friendID, DateTime fromDate, DateTime toDate, int pIndex, int pSize);

        #endregion

        #region 客服好友

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<SeatFriend> GetSeatFriends(string seatID);

        /// <summary>
        /// 获取客服的好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <returns></returns>
        IList<SeatFriend> GetSeatFriends(string seatID, out IList<SeatFriend> friends);

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        bool AddSeatFriendRequest(string seatID, string friendID, string request);

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="type"></param>
        /// <param name="refuse"></param>
        /// <returns></returns>
        bool ConfirmAddSeatFriend(int requestID, AcceptType type, string refuse);

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="seatID"></param>
        /// <param name="friendID"></param>
        /// <returns></returns>
        bool DeleteSeatFriend(string seatID, string friendID);

        #endregion
    }
}
