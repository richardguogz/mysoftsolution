using System;

namespace MySoft.Remoting
{
    /// <summary>
    /// Remoting������ӿ�
    /// </summary>
    [ServiceContract]
    public interface IRemotingTest
    {
        /// <summary>
        /// ���Է�������ȡRemoting������ʱ��
        /// </summary>
        /// <returns></returns>
        string GetDate();
    }

    /// <summary>
    /// Remoting�����࣬��������Remoting�������Ƿ���������
    /// </summary>
    public class RemotingTest : MarshalByRefObject, IRemotingTest
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
