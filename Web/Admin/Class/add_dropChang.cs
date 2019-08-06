using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Web.Admin.Class {
    public class add_dropChang {
        static public bool bingdDdl1( DropDownList ddl,string title) {
            string sql = "";
            sql = "select id from VideoType where title='" + title + "'";
            string id = SqlFunction.Sql_ReturnNumberES(sql);
            sql = "select title from VideoType where tid='" + id + "'";

            DataTable dt = new DataTable();
            dt = fillDs(sql);
            if (dt.Rows[0][0].ToString() == "f") {
                return false;
            } else {
                ddl.Items.Clear();
                ddl.Items.Add("全部");
                for (int i = 0; i < dt.Rows.Count; i++) {
                    //list_s.Add();
                    ddl.Items.Add(dt.Rows[i][0].ToString());
                }
                return true;
            }
            
            

           
        
        }

        static public DataTable fillDs(string sql) {
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql);
            if (ds.Tables[0].Rows.Count <= 0) { 
                ds.Tables[0].Rows.Add();
                ds.Tables[0].Rows[0][0] = "f";
            }
            return ds.Tables[0];
        }

    }
}