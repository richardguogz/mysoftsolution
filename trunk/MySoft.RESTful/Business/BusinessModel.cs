using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.RESTful.Business
{
    public class BusinessModel : BusinessStateModel
    {
        /// <summary>
        /// 业务实例元数据
        /// </summary>
        public IList<BusinessMetadata> Metadatas { get; set; }
    }
}
