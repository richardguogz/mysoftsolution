using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using MySoft.Data.Design;

namespace MySoft.Data
{
    /// <summary>
    /// 数据源
    /// </summary>
    public class SourceTable : DataTable, ISourceTable
    {
        /// <summary>
        /// 实例化SourceTable
        /// </summary>
        /// <param name="dt"></param>
        public SourceTable(DataTable dt)
        {
            if (dt != null)
            {
                this.TableName = dt.TableName;
                foreach (DataColumn column in dt.Columns)
                {
                    this.Columns.Add(column.ColumnName, column.DataType);
                }
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.ImportRow(row);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        public int RowCount
        {
            get
            {
                return this.Rows.Count;
            }
        }

        /// <summary>
        /// 返回指定类型的List
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public ISourceList<TOutput> ConvertTo<TOutput>()
        {
            return this.ConvertAll<TOutput>(p => DataUtils.ConvertType<IRowReader, TOutput>(p));
        }

        /// <summary>
        /// 返回另一类型的列表(输入为类、输出为接口，用于实体的解耦)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="IOutput"></typeparam>
        /// <returns></returns>
        public ISourceList<IOutput> ConvertTo<TOutput, IOutput>()
        {
            if (!typeof(TOutput).IsClass)
            {
                throw new MySoftException("TOutput必须是Class类型！");
            }

            if (!typeof(IOutput).IsInterface)
            {
                throw new MySoftException("IOutput必须是Interface类型！");
            }

            //进行两次转换后返回
            return ConvertTo<TOutput>().ConvertTo<IOutput>();
        }

        /// <summary>
        /// 返回指定类型的List
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public ISourceList<TOutput> ConvertAll<TOutput>(Converter<IRowReader, TOutput> handler)
        {
            ISourceList<TOutput> list = new SourceList<TOutput>();
            for (int index = 0; index < this.RowCount; index++)
            {
                //读取数据到实体
                SourceRow row = new SourceRow(this.Rows[index]);
                list.Add(handler(row));
            }
            return list;
        }
    }
}
