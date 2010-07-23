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
        IEnumerable<string> Validate(EntityState state);
    }
}
