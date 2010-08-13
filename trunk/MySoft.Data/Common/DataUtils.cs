using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using MySoft.Core;

namespace MySoft.Data
{
    /// <summary>
    /// 数据服务类
    /// </summary>
    public static class DataUtils
    {
        #region 外部方法

        /// <summary>
        /// 从对象obj中获取值传给当前实体,TOutput必须为class或接口
        /// TInput可以为class、IRowReader、NameValueCollection
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TOutput ConvertType<TInput, TOutput>(TInput obj)
        {
            if (obj == null) return default(TOutput);

            if (obj is TOutput)
            {
                return (TOutput)(obj as object);
            }
            else
            {
                try
                {
                    TOutput t = CoreUtils.CreateInstance<TOutput>();

                    //如果当前实体为Entity，数据源为IRowReader的话，可以通过内部方法赋值
                    if (t is Entity && obj is IRowReader)
                    {
                        (t as Entity).SetAllValues(obj as IRowReader);
                    }
                    else
                    {
                        foreach (PropertyInfo p in typeof(TOutput).GetProperties())
                        {
                            object value = null;
                            if (obj is IRowReader)
                            {
                                IRowReader reader = obj as IRowReader;
                                if (reader.IsDBNull(p.Name)) continue;
                                value = reader[p.Name];
                            }
                            else if (obj is NameValueCollection)
                            {
                                NameValueCollection reader = obj as NameValueCollection;
                                if (reader[p.Name] == null) continue;
                                value = reader[p.Name];
                            }
                            else
                            {
                                value = CoreUtils.GetPropertyValue(obj, p.Name);
                            }
                            if (value == null) continue;
                            CoreUtils.SetPropertyValue(t, p, value);
                        }
                    }

                    return t;
                }
                catch (MySoftException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new MySoftException(ex.Message);
                }
            }
        }

        #endregion

        #region 判断是否为null或空

        /// <summary>
        /// 判断WhereClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(WhereClip where)
        {
            if ((object)where == null || string.IsNullOrEmpty(where.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断OrderByClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(OrderByClip order)
        {
            if ((object)order == null || string.IsNullOrEmpty(order.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断GroupByClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(GroupByClip group)
        {
            if ((object)group == null || string.IsNullOrEmpty(group.ToString()))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 格式化数据为数据库通用格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static string FormatValue(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return "null";
            }

            Type type = val.GetType();

            if (type == typeof(Guid))
            {
                return string.Format("'{0}'", val);
            }
            else if (type == typeof(DateTime))
            {
                return string.Format("'{0}'", ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (type == typeof(bool))
            {
                return ((bool)val) ? "1" : "0";
            }
            else if (val is Field)
            {
                return ((Field)val).Name;
            }
            else if (val is SysValue)
            {
                return ((SysValue)val).Value;
            }
            else if (type.IsEnum)
            {
                return Convert.ToInt32(val).ToString();
            }
            else if (type.IsValueType)
            {
                if (CheckStruct(type))
                {
                    //如果属性是值类型，则进行系列化存储
                    return SerializationManager.Serialize(val);
                }

                return val.ToString();
            }
            else
            {
                return string.Format("N'{0}'", val.ToString());
            }
        }

        internal static string FormatSQL(string sql, char leftToken, char rightToken, bool isAccess)
        {
            if (sql == null) return string.Empty;

            try
            {
                if (isAccess)
                    sql = string.Format(sql, leftToken, rightToken, '(', ')');
                else
                    sql = string.Format(sql, leftToken, rightToken, ' ', ' ');
            }
            catch
            {
                if (isAccess)
                    sql = sql.Replace("{0}", leftToken.ToString()).Replace("{1}", rightToken.ToString()).Replace("{2}", '('.ToString()).Replace("{3}", ')'.ToString());
                else
                    sql = sql.Replace("{0}", leftToken.ToString()).Replace("{1}", rightToken.ToString()).Replace("{2}", ' '.ToString()).Replace("{3}", ' '.ToString());
            }

            return sql.Trim().Replace(" . ", ".")
                            .Replace(" , ", ",")
                            .Replace(" ( ", " (")
                            .Replace(" ) ", ") ")
                            .Replace("   ", " ")
                            .Replace("  ", " ");
        }

        internal static object[] CheckAndReturnValues(object[] values)
        {
            //如果值为null，则返回不等条件
            if (values == null)
            {
                throw new MySoftException("传入的数据不能为null！");
            }

            //如果长度为0，则返回不等条件
            if (values.Length == 0)
            {
                throw new MySoftException("传入的数据个数不能为0！");
            }

            //如果传的类型不是object,则强制转换
            if (values.Length == 1 && values[0].GetType().IsArray)
            {
                try
                {
                    values = ArrayList.Adapter((Array)values[0]).ToArray();
                }
                catch
                {
                    throw new MySoftException("传入的数据不能正确被解析！");
                }
            }

            return values;
        }

        internal static WhereClip GetPkWhere<T>(Table table, object[] pkValues)
            where T : Entity
        {
            WhereClip where = null;
            List<FieldValue> list = CoreUtils.CreateInstance<T>().GetFieldValues();
            pkValues = CheckAndReturnValues(pkValues);

            list.ForEach(fv =>
            {
                int index = 0;
                if (fv.IsPrimaryKey)
                {
                    where &= fv.Field.At(table) == pkValues[index];
                    index++;
                }
            });

            return where;
        }

        internal static WhereClip GetPkWhere<T>(Table table, T entity)
            where T : Entity
        {
            WhereClip where = null;
            List<FieldValue> list = entity.GetFieldValues();

            list.ForEach(fv =>
            {
                if (fv.IsPrimaryKey)
                {
                    where &= fv.Field.At(table) == fv.Value;
                }
            });

            return where;
        }

        /// <summary>
        /// 创建一个FieldValue列表
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static List<FieldValue> CreateFieldValue(Field[] fields, object[] values, bool isInsert)
        {
            if (fields == null || values == null)
            {
                throw new MySoftException("字段及值不能为null！");
            }

            if (fields.Length != values.Length)
            {
                throw new MySoftException("字段及值长度不一致！");
            }

            int index = 0;
            List<FieldValue> fvlist = new List<FieldValue>();
            foreach (Field field in fields)
            {
                FieldValue fv = new FieldValue(field, values[index]);

                if (isInsert && values[index] is Field)
                {
                    fv.IsIdentity = true;
                }
                else if (!isInsert)
                {
                    fv.IsChanged = true;
                }

                fvlist.Add(fv);

                index++;
            }

            return fvlist;
        }

        #endregion


        /// <summary>
        /// 检测是否为结构数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckStruct(Type type)
        {
            //当属性为结构时进行系列化
            if (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsSerializable)
            {
                return true;
            }
            return false;
        }
    }
}