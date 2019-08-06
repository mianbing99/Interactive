using System;
using System.Data;
using System.Data.Common;
using Web.Admin;

namespace Web.Admin.Data
{
    public enum DataBaseType
    {
        MsSql, Access, MySql
    }
    public class DbHelper
    {
        private DataBaseType dataBaseType = DataBaseType.MsSql;
        private DbConnection connection;
        private DbTransaction transaction;

        #region 构造方法
        public DbHelper()
        {
           // string connectionString = "Data Source=182.254.134.51;Initial Catalog=EJiaQin;User ID=MDB;Password=Main@JLF955icox";//Web.Admin.Data.AppConfig.Settings["connStr"];
            string connectionString = Web.Admin.Data.AppConfig.Settings["connStr"];
            connection = DbProvider.CreateConnection(this.dataBaseType, connectionString);
            connection.Open();
        }
        public DbHelper(string connectionString)
        {
            connection = DbProvider.CreateConnection(this.dataBaseType, connectionString);
            connection.Open();
        }
        public DbHelper(string connectionString, DataBaseType databasetype)
        {
            dataBaseType = databasetype;
            connection = DbProvider.CreateConnection(this.dataBaseType, connectionString);
            connection.Open();            
        }
        #endregion

        #region 事务处理
        public void BeginTrans()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            transaction = connection.BeginTransaction();            
        }
        public void CommitTrans()
        {
            transaction.Commit();
        }
        public void RollBackTrans()
        {
            transaction.Rollback();
        }
        #endregion

        #region 获得Command
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbCommand dbCommand;
            if (this.transaction != null)
            {
                dbCommand = this.transaction.Connection.CreateCommand();
                dbCommand.Transaction = this.transaction;
            }
            else
            {
                dbCommand = this.connection.CreateCommand();
            }

            if (dbCommand.Connection.State == ConnectionState.Closed) dbCommand.Connection.Open();
            dbCommand.CommandText = sqlQuery;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }
        public DbCommand GetStoredProcCommond(string storedProcedure)
        {
            DbCommand dbCommand;
            if (this.transaction != null)
            {
                dbCommand = this.transaction.Connection.CreateCommand();
                dbCommand.Transaction = this.transaction;
            }
            else
            {
                dbCommand = this.connection.CreateCommand();
            }

            if (dbCommand.Connection.State == ConnectionState.Closed) dbCommand.Connection.Open();
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        #endregion

        #region 增加参数
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }
        #endregion

        #region 执行
        public DataSet ExecuteDataSet(DbCommand cmd)
        {

            DbDataAdapter dbDataAdapter = DbProvider.CreateDataAdapter(this.dataBaseType);
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try
            {
                dbDataAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                throw (e);
            }
            CloseConnection();
            return ds;
        }
        public DataSet ExecuteDataSet(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            return ExecuteDataSet(cmd);
        }

        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DbDataAdapter dbDataAdapter = DbProvider.CreateDataAdapter(this.dataBaseType);
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            try
            {
                dbDataAdapter.Fill(dataTable);
            }
            catch { }
            CloseConnection();
            return dataTable;
        }
        public DataTable ExecuteDataTable(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            return ExecuteDataTable(cmd);
        }

        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            DbDataReader reader = null;
            try
            {
                OpenConnection();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch { }

            return reader;
        }
        public DbDataReader ExecuteReader(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            return ExecuteReader(cmd);
        }
        public int ExecuteNonQuery(DbCommand cmd)
        {
            int ret = -1;
            try
            {
                OpenConnection();
                ret = cmd.ExecuteNonQuery();
            }
            catch
            {
            }
            CloseConnection();
            return ret;
        }
        public int ExecuteNonQuery(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            return ExecuteNonQuery(cmd);
        }
        public object ExecuteScalar(DbCommand cmd)
        {
            object ret = null;
            try
            {
                OpenConnection();
                ret = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            CloseConnection();
            return ret;
        }
        public object ExecuteScalar(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            return ExecuteScalar(cmd);
        }
        #endregion

        #region 翻页查询(MSSQL专用)
        /// <summary>
        /// 查找所有记录
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="recordCount">传出参数，记录总数</param>
        /// <returns>得到的记录集</returns>
        public DataTable GetPageData(string sql, ref int recordCount)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            DataTable dt = ExecuteDataTable(cmd);
            recordCount = dt.Rows.Count;
            return dt;
        }

        /// <summary>
        /// 分页查询记录。对于SQL Server必须使用存储过程查找分页记录，
        /// 而对于其他数据库，如Oracle,DB2,MySQL,PostgreSQL 则可以
        /// 通过直接封装SQL语句分页
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns>得到的记录集</returns>
        public DataTable GetPageData(string sql, int pageSize,
            ref int pageIndex, ref int pageCount, ref int recordCount)
        {
            DbCommand cmd = GetStoredProcCommond("sp_pageQuery");
            AddInParameter(cmd, "@sqlstr", DbType.String, sql);
            AddInParameter(cmd, "@page_index", DbType.Int32, pageIndex);
            AddInParameter(cmd, "@page_size", DbType.Int32, pageSize);
            AddOutParameter(cmd, "@rec_count", DbType.Int32, 4);

            DataTable dt = ExecuteDataSet(cmd).Tables[1];
            recordCount = (int)GetParameter(cmd, "@rec_count").Value;
            pageCount = (int)Math.Ceiling(recordCount * 1.00 / pageSize);

            return dt;
        }
        #endregion

        public void CloseConnection()
        {
            try
            {
                if (transaction == null)
                {
                    if (this.connection.State == ConnectionState.Open)
                    {
                        this.connection.Close();
                    }
                }

            }
            catch
            {
            }

        }
        public void OpenConnection()
        {
            try
            {
                if (transaction == null)
                {
                    if (this.connection.State == ConnectionState.Closed)
                    {
                        this.connection.Open();
                    }
                }
            }
            catch
            {
            }

        }

    }
}
