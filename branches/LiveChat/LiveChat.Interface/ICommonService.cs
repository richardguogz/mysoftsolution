using System;
using System.Collections.Generic;
using System.Text;
using LiveChat.Entity;

namespace LiveChat.Interface
{
    public interface ICommonService
    {
        /// <summary>
        /// 验证客户端
        /// </summary>
        /// <param name="id">要验证的使用者ID</param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        bool ValidateClient(string id, Guid clientID);

        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        Session GetSession(string sessionID);

        /// <summary>
        /// 发送消息到客服
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sessionID"></param>
        /// <param name="senderIP"></param>
        /// <param name="content"></param>
        void SendP2SMessage(MessageType type, string sessionID, string senderIP, string content);

        /// <summary>
        /// 获取会话中所有消息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        IList<Message> GetP2SMessages(string sessionID);

        /// <summary>
        /// 结束会话
        /// </summary>
        /// <param name="sessionID"></param>
        void CloseSession(string sessionID);

        #region 历史消息

        /// <summary>
        /// 获取会话中所有历史消息(从内存中查询)
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        IList<Message> GetP2SHistoryMessages(string sessionID);

        /// <summary>
        /// 获取会话中所有历史消息(从内存中查询)
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        IList<Message> GetP2SHistoryMessages(string sessionID, DateTime lastGetTime);

        /// <summary>
        /// 获取会话中所有历史消息(从数据库中查询)
        /// </summary>
        /// <param name="sid">会话唯一标识</param>
        /// <param name="getDate"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        DataView<IList<P2SMessage>> GetP2SHistoryMessagesFromDB(Guid sid, int pIndex, int pSize);

        #endregion

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        string[] UploadImage(byte[] buffer, string extension);

        /// <summary>
        /// 上传普通文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        string UploadFile(byte[] buffer, string extension);
    }
}
