using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;

namespace LiveChat.Service
{
    public partial class t_Leave
    {
        public override ValidateResult Validation()
        {
            return new Validator<t_Leave>(this)
                    .Check(p => string.IsNullOrEmpty(p.Name), "姓名不能为空！")
                    .Check(p => string.IsNullOrEmpty(p.Email), "Email地址不能为空！")
                    .Check(p => string.IsNullOrEmpty(p.Body), "留言主题不能为空！").Result;
        }
    }
}
