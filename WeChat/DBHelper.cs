using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WeChat {
    public class DBHelper {
        //连接字符串
        //private const string DBConnectString = "Data Source=bds13396396.my3w.com;Initial Catalog=bds13396396_db;User ID=bds13396396;Password=icox2015";
        //private const string DBConnectString = "Data Source=182.254.134.51;Initial Catalog=bds13396396_db;User ID=MDB;Password=Main@JLF955icox";
        private const string DBConnectString = "Data Source=182.254.134.51;Initial Catalog=EJiaQin;User ID=MDB;Password=Main@JLF955icox";
        private static DBHelper mydb; //(自定义)通用数据库操作对象
        //实例化DBHelper
        public static DBHelper Instance() {
            if (mydb == null) {
                mydb = new DBHelper();
            }
            return mydb;
        }
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql) {
            SqlConnection conn = new SqlConnection(DBConnectString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        /// <summary>
        /// 执行(select)SQL语句返回数据表
        /// </summary>
        /// <param name="sql">select语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableBySql(string sql) {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(DBConnectString)) {
                try {
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dt);
                } catch (Exception e) {
                    return null;
                }
            }
            return dt;
        }
        /// <summary>
        /// 执行(select)SQL语句返回SqlDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader GetDataReaderBySql(string sql) {
            try {
                SqlConnection conn = new SqlConnection(DBConnectString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            } catch (Exception e) {
                return null;
            }
        }
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql">参数</param>
        /// <returns>resulf参数</returns>
        public bool ExecuteNonQuery(string sql) {
            SqlConnection conn = new SqlConnection(DBConnectString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int resulf = cmd.ExecuteNonQuery();
            conn.Close();
            return resulf > 0;
        }
        /// <summary>
        /// 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteNonQueryString(string sql) {
            SqlConnection conn = new SqlConnection(DBConnectString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return result > 0;
        }
        public static int ExecuteScalar(string sql) {
            SqlConnection conn = new SqlConnection(DBConnectString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int result =Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return result;
        }
    }
}
