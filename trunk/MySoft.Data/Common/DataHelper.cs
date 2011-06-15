using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;

namespace MySoft.Data
{
    /// <summary>
    /// ���ݷ�����
    /// </summary>
    public static class DataHelper
    {
        #region ����ת��

        /// <summary>
        /// ��¡����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneObject<T>(T obj)
        {
            return ConvertType<T, T>(obj);
        }

        /// <summary>
        /// �Ӷ���obj�л�ȡֵ������ǰʵ��,TOutput����Ϊclass��ӿ�
        /// TInput����Ϊclass��NameValueCollection��IDictionary��IRowReader��DataRow
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TOutput ConvertType<TInput, TOutput>(TInput obj)
        {
            if (obj == null) return default(TOutput);

            if (obj is TOutput && typeof(TOutput).IsInterface)
            {
                return (TOutput)(obj as object);
            }
            else
            {
                TOutput t = default(TOutput);

                try
                {
                    //t = CoreHelper.CreateInstance<TOutput>();
                    if (typeof(TOutput) == typeof(TInput))
                    {
                        t = CoreHelper.CreateInstance<TOutput>(obj.GetType());
                    }
                    else
                    {
                        t = CoreHelper.CreateInstance<TOutput>();
                    }
                }
                catch (Exception ex)
                {
                    throw new DataException(string.Format("�������Ͷ���{0}���������ܲ����ڹ��캯����", typeof(TOutput).FullName), ex);
                }

                //�����ǰʵ��ΪEntity������ԴΪIRowReader�Ļ�������ͨ���ڲ�������ֵ
                if (t is Entity && obj is IRowReader)
                {
                    (t as Entity).SetDbValues(obj as IRowReader);
                }
                else
                {
                    foreach (PropertyInfo p in t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        object value = null;
                        if (obj is NameValueCollection)
                        {
                            NameValueCollection reader = obj as NameValueCollection;
                            if (reader[p.Name] == null) continue;
                            value = reader[p.Name];
                        }
                        else if (obj is IDictionary)
                        {
                            IDictionary reader = obj as IDictionary;
                            if (!reader.Contains(p.Name)) continue;
                            if (reader[p.Name] == null) continue;
                            value = reader[p.Name];
                        }
                        else if (obj is IRowReader)
                        {
                            IRowReader reader = obj as IRowReader;
                            if (reader.IsDBNull(p.Name)) continue;
                            value = reader[p.Name];
                        }
                        else if (obj is DataRow)
                        {
                            IRowReader reader = new SourceRow(obj as DataRow);
                            if (reader.IsDBNull(p.Name)) continue;
                            value = reader[p.Name];
                        }
                        else
                        {
                            value = CoreHelper.GetPropertyValue(obj, p.Name);
                        }

                        if (value == null) continue;
                        CoreHelper.SetPropertyValue(t, p, value);
                    }
                }

                //ͨ���˷�ʽ����Ķ����޸������
                if (t != null && t is Entity) (t as Entity).AttachSet();

                return t;
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
            else if (val is DbValue)
            {
                return ((DbValue)val).Value;
            }
            else if (type.IsEnum)
            {
                return Convert.ToInt32(val).ToString();
            }
            else if (type.IsValueType)
            {
                if (CoreHelper.CheckStructType(type))
                {
                    //���������ֵ���ͣ������ϵ�л��洢
                    return SerializationManager.SerializeJson(val);
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

            if (isAccess)
                sql = sql.Replace("__[", leftToken.ToString())
                        .Replace("]__", rightToken.ToString())
                        .Replace("__[[", '('.ToString())
                        .Replace("]]__", ')'.ToString());
            else
                sql = sql.Replace("__[", leftToken.ToString())
                        .Replace("]__", rightToken.ToString())
                        .Replace("__[[", ' '.ToString())
                        .Replace("]]__", ' '.ToString());

            //string str = sql.Replace(" . ", ".")
            //                .Replace(" , ", ",")
            //                .Replace(" ( ", " (")
            //                .Replace(" ) ", ") ");

            return CoreHelper.RemoveSurplusSpaces(sql);
        }

        internal static object[] CheckAndReturnValues(object[] values)
        {
            //���ֵΪnull���򷵻ز�������
            if (values == null)
            {
                throw new DataException("��������ݲ���Ϊnull��");
            }

            //�������Ϊ0���򷵻ز�������
            if (values.Length == 0)
            {
                throw new DataException("��������ݸ�������Ϊ0��");
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
                    throw new DataException("��������ݲ�����ȷ��������");
                }
            }

            return values;
        }

        internal static WhereClip GetPkWhere<T>(Table table, object[] pkValues)
            where T : Entity
        {
            WhereClip where = null;
            List<FieldValue> list = CoreHelper.CreateInstance<T>().GetFieldValues();
            pkValues = CheckAndReturnValues(pkValues);

            int pkCount = list.FindAll(p => p.IsPrimaryKey).Count;
            if (pkValues.Length != pkCount)
            {
                throw new DataException("����������������޷���Ӧ��Ӧ�ô��롾" + pkCount + "��������ֵ��");
            }

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
                throw new DataException("�ֶμ�ֵ����Ϊnull��");
            }

            if (fields.Length != values.Length)
            {
                throw new DataException("�ֶμ�ֵ���Ȳ�һ�£�");
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
    }
}