using System;
using System.Collections;
using System.Data;

namespace MySoft.Common
{
    /// <summary>
    /// PostUserInfo : ���촫���û����ݡ�
    /// </summary>
    public class PostUserInfo
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public PostUserInfo()
        {

        }


        // Private Field
        private string _UID;
        private string _UserID;
        private string _UserPwd;
        private string _UserMail;
        private string _OldUserPwd;
        private ServiceMethod _Method;

        #region �û���������

        /// <summary>
        /// �û����
        /// </summary>
        public string UID
        {
            get { return _UID; }
            set { _UID = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        /// <summary>
        /// �û�����
        /// </summary>
        public string UserPwd
        {
            get { return _UserPwd; }
            set { _UserPwd = value; }
        }

        /// <summary>
        /// �û��ʼ���ַ
        /// </summary>
        public string UserMail
        {
            get { return _UserMail; }
            set { _UserMail = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string OldUserPwd
        {
            get { return _OldUserPwd; }
            set { _OldUserPwd = value; }
        }

        /// <summary>
        /// �Զ������Ĳ�����ʽ
        /// </summary>
        public ServiceMethod Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        #endregion
    }
}
