﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.RESTful.Business
{
    /// <summary>
    /// 业务状态
    /// </summary>
    public abstract class BusinessStateModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        private BusinessState state = BusinessState.ACTIVATED;

        /// <summary>
        /// 业务接口状态
        /// </summary>
        public BusinessState State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
