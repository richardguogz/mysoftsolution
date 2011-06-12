using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.RESTful
{
    /// <summary>
    /// 系列化接口
    /// </summary>
    public interface ISerializer
    {
        string Serialize(object data);
    }
}
