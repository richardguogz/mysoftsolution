using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Entity
{
    /// <summary>
    /// 分页信息
    /// </summary>
    [Serializable]
    public class DataView<T>
    {
        private T _DataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public T DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                _DataSource = value;
            }
        }

        private int _PageIndex;
        /// <summary>
        /// 当前面码
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }

        private int _PageSize;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }

        /// <summary>
        /// 记录页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(_RowCount * 1.0 / _PageSize));
            }
        }

        private int _RowCount;
        /// <summary>
        /// 记录条数
        /// </summary>
        public int RowCount
        {
            get
            {
                return _RowCount;
            }
            set
            {
                _RowCount = value;
            }
        }

        /// <summary>
        ///  获取一个值，该值指示当前页是否是首页
        /// </summary>
        public bool IsFirstPage
        {
            get
            {
                return _PageIndex <= 1;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前页是否是最后一页
        /// </summary>
        public bool IsLastPage
        {
            get
            {
                return _PageIndex >= PageCount;
            }
        }

        /// <summary>
        /// 初始化页信息
        /// </summary>
        public DataView(int pageSize)
        {
            this._PageSize = pageSize;
            this._PageIndex = 1;
        }
    }
}
