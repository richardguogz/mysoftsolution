using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MySoft.RESTful
{
    /// <summary>
    /// RESTful结果
    /// </summary>
    [Serializable]
    public class RESTfulResult
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message;
    }

    /// <summary>
    /// RESTfulCode
    /// </summary>
    public enum RESTfulCode
    {
        /// <summary>
        /// 正确返回数据
        /// </summary>
        OK = 20050,
        /// <summary>
        /// 认证失败
        /// </summary>
        AUTH_FAULT = 40150,
        /// <summary>
        /// 验证错误
        /// </summary>
        AUTH_ERROR = 40151,
        /// <summary>
        /// 业务错误
        /// </summary>
        BUSINESS_ERROR = 41750,
        /// <summary>
        /// 业务类型没找到
        /// </summary>
        BUSINESS_KIND_NOT_FOUND = 40050,
        /// <summary>
        /// 业务方法没找到
        /// </summary>
        BUSINESS_METHOD_NOT_FOUND = 40051,
        /// <summary>
        /// 业务类型不是激活状态
        /// </summary>
        BUSINESS_KIND_NO_ACTIVATED = 40052,
        /// <summary>
        /// 业务方法不是激活状态
        /// </summary>
        BUSINESS_METHOD_NO_ACTIVATED = 40053,
        /// <summary>
        /// 业务方法参数个数不匹配
        /// </summary>
        BUSINESS_METHOD_PARAMS_COUNT_NOT_MATCH = 40054,
        /// <summary>
        /// 业务方法参数类型不匹配
        /// </summary>
        BUSINESS_METHOD_PARAMS_TYPE_NOT_MATCH = 40055,
        /// <summary>
        /// 业务方法调用类型不匹配
        /// </summary>
        BUSINESS_METHOD_CALL_TYPE_NOT_MATCH = 40056
    }
}
