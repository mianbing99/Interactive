using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;

namespace Web.Admin.chartsTJ {
    public partial class Line2 : System.Web.UI.Page {
        protected string[] time;
        protected Int32[] time2;
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    getdata2();
                }
            }
        }
        public void getdata2() {
            string sql = "";
            string tsql = "", sdate = "2016-12-05", edate = DateTime.Now.ToString();
            tsql = string.Format(@"declare @sdate datetime 
            declare @edate datetime
            set @sdate = '{0}'
            set @edate = '{1}'
            select  dateadd(dd,num,@sdate) from 
                (select isnull((select count(1) from sysobjects where id <t.id),0) as num from sysobjects t) a 
            where dateadd(dd,num,@sdate) <=@edate", sdate, edate);
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(tsql);
            int c1 = ds.Tables[0].Rows.Count;
            time = new string[c1];
            for (int i = 0; i < c1; i++) {
                time[i] = ds.Tables[0].Rows[i]["Column1"].ToString();
                time[i] = time[i].Replace(" 0:00:00", "");
                sql += string.Format(@"select COUNT(distinct user_IP) from TempUserusejl  
                where clickTime<='{0} 23:59:59.999' union all ", time[i]);
            }
            sql = sql.Substring(0, sql.Length - 10);
            DataSet dss = new DataSet();
            dss = SqlFunction.Sql_DataAdapterToDS(sql);
            int c2 = dss.Tables[0].Rows.Count;
            time2 = new Int32[c2];
            for (int j = 0; j < c2; j++) {
                time2[j] = Convert.ToInt32(dss.Tables[0].Rows[j]["Column1"]);
            }

        }
    }
}