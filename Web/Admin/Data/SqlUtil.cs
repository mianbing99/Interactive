using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Reflection;

namespace IFrameWork.Data
{
    public class SqlUtil
    {
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 特殊字符处理及Uncode字符的函数
        /// 在直接拼凑SQL语句中的=语句中使用。
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// 目的是防sql注入的攻击，例如：
        /// string sql = "select * from Customers where CustomerID = '" + temp + "'";
        /// 如果，给temp赋值为 5' or 1=1 --  
        /// 那么你拼接起来的语句为 select * from Customers where CustomerID = '5' or 1=1 -- '
        /// 如果替换了单引号就变成
        /// select * from Customers where CustomerID = '5'' or 1=1 -- '
        /// 就不会有问题了
        /// 
        /// 注意：
        ///    不论什么类型的参数，参数的两边都必须加上单引号，例如：
        /// string sql = "select * from Customers where CustomerID = " + temp;
        /// temp两边没有单引号， 如果给temp赋值为 5 or 1=1 --  
        /// 那么你拼接起来的语句为 select * from Customers where CustomerID = 5 or 1=1 --
        /// 没有单引号，这样还是有问题
        /// </remarks>
        public static string SCCEqual(string strValue)
        {
            strValue = strValue.Replace("'", "''");
            return strValue;
        }

        /// <summary>
        /// 特殊字符处理及Uncode字符的函数
        /// 在直接拼凑SQL语句中的Like语句中使用。
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string SCCLike(string strValue)
        {
            strValue = strValue.Replace("'", "''");
            strValue = strValue.Replace("[", "[[]");
            strValue = strValue.Replace("_", "[_]");
            strValue = strValue.Replace("%", "[%]");
            
            return strValue;
        }

        public static object Nullable(object o)
        {
            object obj = (o == null ? System.DBNull.Value : o);
            return obj;
        }

        /// <summary>
        /// 将DbDataReader的一行转变成一个对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Object ReaderToObject(DbDataReader reader, Type entityType)
        {
            Object entity = Activator.CreateInstance(entityType, true);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo propertyInfo = entityType.GetProperty(reader.GetName(i));
                if (propertyInfo != null)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(entity, Enum.ToObject(propertyInfo.PropertyType, reader.GetValue(i)), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(entity, reader.GetValue(i), null);
                        }
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 将DataTable的一行转变成一个对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static Object DataRowToObject(DataRow row, Type entityType)
        {
            Object entity = Activator.CreateInstance(entityType, true);
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = entityType.GetProperty(row.Table.Columns[i].ColumnName);
                if (propertyInfo != null)
                {
                    if (row[i] != DBNull.Value)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(entity, Enum.ToObject(propertyInfo.PropertyType, row[i]), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(entity, row[i], null);
                        }
                    }
                }
            }
            return entity;
        }

        #region 转换相关

        public static int ToInt(object strValue)
        {
            return ToInt(strValue, 0);
        }
        public static int ToInt(object strValue, int defaultValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty) || (strValue.ToString().Length > 10))
            {
                return defaultValue;
            }
            int intValue = defaultValue;

            bool IsInt = new Regex(@"^([-]|[0-9])[0-9]*$").IsMatch(strValue.ToString());
            if (IsInt)
            {
                intValue = Convert.ToInt32(strValue);
            }
            
            return intValue;
        }

        public static float ToFloat(object strValue)
        {
            return ToFloat(strValue, 0);
        }
        public static float ToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            
            bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
            if (IsFloat)
            {
                intValue = Convert.ToSingle(strValue);
            }
         
            return intValue;
        }
        public static int TimeToInt(DateTime time)
        {
            TimeSpan timeSpan = (TimeSpan)(time - new DateTime(1970, 1, 1));
            return ((int)timeSpan.TotalSeconds);
        }
        #endregion
    }
}
