using System;

namespace MySoft.IoC
{
    /// <summary>
    /// IoC异常
    /// </summary>
    [Serializable]
    public class IoCException : MySoftException
    {
        /// <summary>
        /// 错误头
        /// </summary>
        public string ExceptionHeader { get; set; }

        /// <summary>
        /// 普通异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        public IoCException(string message)
            : base(ExceptionType.RemotingException, message)
        { }

        /// <summary>
        /// 内嵌异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public IoCException(string message, Exception ex)
            : base(ExceptionType.RemotingException, message, ex)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">存储对象序列化和反序列化所需的全部数据</param>
        /// <param name="context">描述给定的序列化流的源和目标，并提供一个由调用方定义的附加上下文</param>
        protected IoCException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.ExceptionHeader = (string)info.GetValue("ExceptionHeader", typeof(string));
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("ExceptionHeader", this.ExceptionHeader);
            base.GetObjectData(info, context);
        }
    }
}
