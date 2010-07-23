using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 实体验证接口
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 根据实体状态来验证实体的有效性，返回一组错误信息
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        ValidateResult Validate(EntityState state);
    }

    /// <summary>
    /// 验证返回信息
    /// </summary>
    public class ValidateResult
    {
        private IList<string> _messages;

        /// <summary>
        /// 实例化ValidateResult
        /// </summary>
        /// <param name="messages"></param>
        public ValidateResult(IList<string> messages)
        {
            this._messages = messages;
        }

        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _messages.Count == 0;
            }
        }

        /// <summary>
        /// 消息列表
        /// </summary>
        public IList<string> Messages
        {
            get
            {
                return _messages;
            }
            private set
            {
                _messages = value;
            }
        }
    }
}
