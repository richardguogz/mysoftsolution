using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using MySoft.Core;

namespace MySoft.Data
{
    /// <summary>
    /// ���ݷ�����
    /// </summary>
    public static class DataUtils
    {
        #region �ⲿ����

        /// <summary>
        /// �Ӷ���obj�л�ȡֵ������ǰʵ��,TOutput����Ϊclass��ӿ�
        /// TInput����Ϊclass��IRowReader��NameValueCollection
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

                    //�����ǰʵ��ΪEntity������ԴΪIRowReader�Ļ�������ͨ���ڲ�������ֵ
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

        #region �ж��Ƿ�Ϊnull���

        /// <summary>
        /// �ж�WhereClip�Ƿ�Ϊnull���
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
        /// �ж�OrderByClip�Ƿ�Ϊnull���
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
        /// �ж�GroupByClip�Ƿ�Ϊnull���
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

        #region �ڲ�����

        /// <summary>
        /// ��ʽ������Ϊ���ݿ�ͨ�ø�ʽ
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
                    //���������ֵ���ͣ������ϵ�л��洢
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
            //���ֵΪnull���򷵻ز�������
            if (values == null)
            {
                throw new MySoftException("��������ݲ���Ϊnull��");
            }

            //�������Ϊ0���򷵻ز�������
            if (values.Length == 0)
            {
                throw new MySoftException("��������ݸ�������Ϊ0��");
            }

            //����������Ͳ���object,��ǿ��ת��
            if (values.Length == 1 && values[0].GetType().IsArray)
            {
                try
                {
                    values = ArrayList.Adapter((Array)values[0]).ToArray();
                }
                catch
                {
                    throw new MySoftException("��������ݲ�����ȷ��������");
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
        /// ����һ��FieldValue�б�
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static List<FieldValue> CreateFieldValue(Field[] fields, object[] values, bool isInsert)
        {
            if (fields == null || values == null)
            {
                throw new MySoftException("�ֶμ�ֵ����Ϊnull��");
            }

            if (fields.Length != values.Length)
            {
                throw new MySoftException("�ֶμ�ֵ���Ȳ�һ�£�");
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
        /// ����Ƿ�Ϊ�ṹ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckStruct(Type type)
        {
            //������Ϊ�ṹʱ����ϵ�л�
            if (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsSerializable)
            {
                return true;
            }
            return false;
        }
    }
}