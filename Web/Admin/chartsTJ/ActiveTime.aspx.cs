using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Admin.Class;
using System.Data;

namespace Web.Admin.chartsTJ {
    public partial class ActiveTime : System.Web.UI.Page {
        static public string[] time;
        static public int c;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                string sql = "";
                string ytime = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
                string thistime = DateTime.Now.ToShortDateString();
                sql = string.Format(@"select  substring(CONVERT(varchar(100), time, 21),12,2)as time2,COUNT(substring(CONVERT(varchar(100), time, 21),12,2))as c1 from UserAccessRecord
where substring(CONVERT(varchar(100), time, 21),0,11)='{0}' group by substring(CONVERT(varchar(100), time, 21),12,2)", ytime);
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(sql);

                time = new string[ds.Tables[0].Rows.Count];
                c = 0;
                //string[] time24 = new string[24] { "00", "01", "02", "03", "04", "05", "06","07",
                //    "08", "09", "10", "11", "12", "13", "14","15",
                //    "16", "17", "18", "19", "20", "21", "22","23" };
                //string[] stime = new string[time.Length];
                if (time.Length != 24) {
                    //for (int j = 0; j < time.Length; j++) {
                    //    stime[j] = ds.Tables[0].Rows[j]["time2"].ToString();
                    //}
                    for (int j = 0; j < time.Length; j++) {
                        time[j] = ds.Tables[0].Rows[j]["c1"].ToString();
                        c += Convert.ToInt32(time[j]);
                    }

                } else {
                    for (int j = 0; j < time.Length; j++) {
                        time[j] = ds.Tables[0].Rows[j]["c1"].ToString();
                        c += Convert.ToInt32(time[j]);
                    }
                }
                
            }
        }
    }
}