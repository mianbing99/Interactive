using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;

namespace Web.Admin.chartsTJ {
    public partial class Line1 : System.Web.UI.Page {
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
//            string tsql = "", sdate = "2016-12-05", edate = DateTime.Now.ToString();
//            tsql = string.Format(@"declare @sdate datetime 
//            declare @edate datetime
//            set @sdate = '{0}'
//            set @edate = '{1}'
//            select  dateadd(dd,num,@sdate) from 
//                (select isnull((select count(1) from sysobjects where id <t.id),0) as num from sysobjects t) a 
//            where dateadd(dd,num,@sdate) <=@edate", sdate, edate);
//            DataSet ds = new DataSet();
//            ds = SqlFunction.Sql_DataAdapterToDS(tsql);
//            int c1 = ds.Tables[0].Rows.Count;
//            time = new string[c1];
//            for (int i = 0; i < c1; i++) {
//                time[i] = ds.Tables[0].Rows[i]["Column1"].ToString();
//                time[i] = time[i].Replace(" 0:00:00", "");
//                time[i] = time[i].Replace(" 00:00:00", "");
//                sql += string.Format(@"select count(*) from UserAccessTj where clickTime>'{0} 00:00:00.000'
//                and clickTime<'{0} 23:59:59.999' union all ", time[i]);
//            }
            //sql = sql.Substring(0, sql.Length - 10);
            sql = "select Count(substring(time,1,10))as 每日点击次数 from UserAccessRecord group by substring(time,1,10)  order by 每日点击次数";
            DataSet dss = new DataSet();
            dss = SqlFunction.Sql_DataAdapterToDS(sql);
            int c2 = dss.Tables[0].Rows.Count;
            time2 = new Int32[c2+160];
            //旧数据 2016-12-3~2017-05-11 共160天
            int []oldinfo=new int[160]{14083,18665,16051,13807,14609,24967,22345,24811,25849,17669,
                20292,17685,19667,19475,23340,24254,18007,22466,22314,19452,19395,25601,23948,17061,18713,
                18395,19987,20895,26698,24225,22870,21242,22246,21500,23049,26985,25702,22818,24080,24625,
                24399,25577,28365,28626,28944,27549,29084,28513,26496,28907,25623,26203,27397,24925,23414,
                22731,23123,22668,23332,21936,21857,23236,23457,25837,23176,25094,25462,27135,23841,24756,
                23316,21193,17431,17853,17813,20276,20840,26055,24043,19842,21642,19164,18576,18714,25478,
                23416,16950,16532,18006,15752,15690,22505,21388,16322,14591,13536,14305,17425,23612,24901,
                17591,15961,16638,15062,18452,22481,21378,15228,15340,15680,16044,16459,20451,21509,12562,
                14467,13096,15178,14085,13086,16745,16365,21670,17935,12151,12582,20520,21772,15761,13699,
                12731,14546,13135,18237,18298,12251,12011,12886,17581,16817,19651,16123,9747,12493,11962,
                13089,13406,17934,16759,16106,12206,13630,13014,14854,17056,16576,12421,10819,12466,12497};
            //2017-05-12开始的数据
            for (int j = 160; j < c2+160; j++) {
                time2[j] = Convert.ToInt32(dss.Tables[0].Rows[j-160]["每日点击次数"]);
            }
            Array.Copy(oldinfo, 0, time2, 0, oldinfo.Length);
        }
    }
}