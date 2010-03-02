using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class MemoryFrom<T> : IMemoryFrom<T>
    {
        private MemoryQuery<T> query;

        #region ��ʼ��MemoryQuery

        /// <summary>
        /// ����һ��DataTable
        /// </summary>
        /// <param name="dt"></param>
        public MemoryFrom(DataTable dt)
        {
            this.query = new MemoryQuery<T>(dt);
        }

        /// <summary>
        /// ����һ��List
        /// </summary>
        /// <param name="list"></param>
        public MemoryFrom(IList<T> list)
        {
            DataTable dt = new SourceList<T>(list).ToTable() as DataTable;
            this.query = new MemoryQuery<T>(dt);
        }

        #endregion

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Single(WhereClip where)
        {
            return query.Where(where).ToSingle();
        }

        /// <summary>
        /// �ж��Ƿ����
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(WhereClip where)
        {
            return Single(where) != null;
        }

        /// <summary>
        /// ��ȡ��¼��
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Field field, WhereClip where)
        {
            return query.Where(where).Count(field);
        }

        #region IMemoryQuery<T> ��Ա

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> Where(WhereClip where)
        {
            return query.Where(where);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> OrderBy(OrderByClip order)
        {
            return query.OrderBy(order);
        }

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T ToSingle()
        {
            return query.ToSingle();
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList()
        {
            return query.ToList();
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int topSize)
        {
            return query.ToList(topSize);
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int startIndex, int endIndex)
        {
            return query.ToList(startIndex, endIndex);
        }

        /// <summary>
        /// ��ȡһ�����ݱ�
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            return query.ToTable();
        }

        /// <summary>
        /// ��ȡһ�����ݱ�
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int topSize)
        {
            return query.ToTable(topSize);
        }

        /// <summary>
        /// ��ȡһ�����ݱ�
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int startIndex, int endIndex)
        {
            return query.ToTable(startIndex, endIndex);
        }

        #region �������

        /// <summary>
        /// ��ȡ��¼��
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Field field)
        {
            return query.Count(field);
        }

        /// <summary>
        /// ����ƽ��ֵ
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg(Field field)
        {
            return query.Avg(field);
        }

        /// <summary>
        /// �������ֵ
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max(Field field)
        {
            return query.Max(field);
        }

        /// <summary>
        /// ������Сֵ
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min(Field field)
        {
            return query.Min(field);
        }

        /// <summary>
        /// �������ֵ
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum(Field field)
        {
            return query.Sum(field);
        }

        /// <summary>
        /// ����ƽ��ֵ
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<TResult>(Field field)
        {
            return query.Avg<TResult>(field);
        }

        /// <summary>
        /// �������ֵ
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<TResult>(Field field)
        {
            return query.Max<TResult>(field);
        }

        /// <summary>
        /// ������Сֵ
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<TResult>(Field field)
        {
            return query.Min<TResult>(field);
        }

        /// <summary>
        /// �������ֵ
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<TResult>(Field field)
        {
            return query.Sum<TResult>(field);
        }

        #endregion

        #region ����Object�б�

        /// <summary>
        ///  ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field)
        {
            return query.ToListResult(field);
        }

        /// <summary>
        ///  ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field, int topSize)
        {
            return query.ToListResult(field, topSize);
        }

        /// <summary>
        /// ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field, int startIndex, int endIndex)
        {
            return query.ToListResult(field, startIndex, endIndex);
        }

        /// <summary>
        ///  ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field)
        {
            return query.ToListResult<TResult>(field);
        }

        /// <summary>
        ///  ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field, int topSize)
        {
            return query.ToListResult<TResult>(field, topSize);
        }

        /// <summary>
        /// ����object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field, int startIndex, int endIndex)
        {
            return query.ToListResult<TResult>(field, startIndex, endIndex);
        }


        /// <summary>
        /// ���ط�ҳMemoryQuery
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MemoryPage<T> GetPage(Field field, int pageSize)
        {
            return query.GetPage(field, pageSize);
        }

        #endregion

        #region ���ط�ҳ��Ϣ

        /// <summary>
        /// ֱ�ӷ���DataPage��ҳ��Ϣ
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<IList<T>> ToListPage(Field field, int pageSize, int pageIndex)
        {
            return query.ToListPage(field, pageSize, pageIndex);
        }

        /// <summary>
        /// ֱ�ӷ���DataPage��ҳ��Ϣ
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<DataTable> ToTablePage(Field field, int pageSize, int pageIndex)
        {
            return query.ToTablePage(field, pageSize, pageIndex);
        }

        #endregion

        #endregion
    }
}
