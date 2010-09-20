using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 消息对象
    /// </summary>
    [Serializable]
    public class P2SMessage : Message, IReceiver
    {
        private string _ReceiverID;
        /// <summary>
        /// 接收者ID
        /// </summary>
        public string ReceiverID
        {
            get
            {
                return _ReceiverID;
            }
            set
            {
                _ReceiverID = value;
            }
        }

        private string _ReceiverName;
        /// <summary>
        /// 接收者名称
        /// </summary>
        public string ReceiverName
        {
            get
            {
                return _ReceiverName;
            }
            set
            {
                _ReceiverName = value;
            }
        }

        //实例化消息
        public P2SMessage()
        {
            this._State = MessageState.Abnormal;
        }
    }
}