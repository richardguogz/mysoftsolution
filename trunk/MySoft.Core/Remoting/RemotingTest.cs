using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// Remoting�����࣬��������Remoting�������Ƿ���������
    /// </summary>
    public class RemotingTest : MarshalByRefObject
    {
        /// <summary>
        /// ���Է�������ȡRemoting������ʱ��
        /// </summary>
        /// <returns></returns>
        public string GetDate()
        {
            return DateTime.Now.ToString();
        }
    }
}
