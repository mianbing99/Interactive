using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Web.Admin.Data;


namespace Web.Admin.Class {
    public class SqlFunction {
        
        public static DataSet DataBind(string sql, int pageNum, GridView gv) {
            DbHelper db = new DbHelper();
            DataSet ds = new DataSet();
            ds = db.ExecuteDataSet(sql);
            gv.DataSource = ds;
            gv.PageIndex = 0;
            gv.DataBind();
            return ds;
        }
        public static int Sql_ReturnNumberENQ(string sql) {
            DbHelper db = new DbHelper();
            return db.ExecuteNonQuery(sql);
        }
        public static DataSet Sql_DataAdapterToDS(string sql) {
            DbHelper db = new DbHelper();
            DataSet ds = new DataSet();
            return ds = db.ExecuteDataSet(sql);
        }
        public static string Sql_ReturnNumberES(string sql) {
            DbHelper db = new DbHelper();
            string temp;
            if (db.ExecuteScalar(sql) == null) {
                temp = "";
            } else {
                temp = db.ExecuteScalar(sql).ToString();
            }
            return temp;
        }
        public static int Sql_UpdatePL(string sql) {
            DbHelper db = new DbHelper();
            return db.ExecuteNonQuery(sql);
        }
    }
}
