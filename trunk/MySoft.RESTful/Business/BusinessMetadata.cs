using System;
using System.Reflection;

namespace MySoft.RESTful.Business
{
    /// <summary>
    /// 方法元数据定义
    /// </summary>
    [Serializable]
    public class BusinessMetadata : BusinessStateModel
    {
        /// <summary>
        /// 方法调用类型
        /// </summary>
        public MethodType Type { get; set; }
        /// <summary>
        /// 业务执行对象
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// 执行的业务实例方法
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// 业务示例方法参数
        /// </summary>
        public ParameterInfo[] Parameters { get; set; }
        /// <summary>
        /// 业务实例方法参数个数
        /// </summary>
        public int ParametersCount { get; set; }
    }
}
