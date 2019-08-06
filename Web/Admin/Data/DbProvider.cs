using System;
using System.Data.Common;

namespace Web.Admin.Data
{
    public class DbProvider
    {
        public static DbConnection CreateConnection(DataBaseType databasetype, string connectionString)
        {
            switch (databasetype)
            {
                case DataBaseType.MsSql:
                    return new System.Data.SqlClient.SqlConnection(connectionString);
                case DataBaseType.Access:
                    return new System.Data.OleDb.OleDbConnection(connectionString);
                default:
                    return new System.Data.SqlClient.SqlConnection(connectionString);
            }
        }

        public static DbDataAdapter CreateDataAdapter(DataBaseType databasetype)
        {
            switch (databasetype)
            {
                case DataBaseType.MsSql:
                    return new System.Data.SqlClient.SqlDataAdapter();
                case DataBaseType.Access:
                    return new System.Data.OleDb.OleDbDataAdapter();
                default:
                    return new System.Data.SqlClient.SqlDataAdapter();
            }
        }
    }
}
