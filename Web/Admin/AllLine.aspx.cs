using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;

namespace Web.Admin {
    public partial class AllLine : System.Web.UI.Page {
        protected string[] daytime;//每日点击量
        protected string[] dayten;//x坐标

        protected string[] dayusernum;//每日客户点击人数
        //protected string[] usernumcount;//客户总人数
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    //getdata();
                }
            }

        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='Admin/js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }

        public void getdata() {
            string[] htime = new string[12];//时
            //string[] hdtime = new string[4];//时间段
            string[] weektime = new string[4];//周
            string[] mtime = new string[12];//月
            string sdate = "";
            string edate = "";
            string sql = "";
            string dqtime = DateTime.Now.ToShortDateString();
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            int d = DateTime.Now.Day;
            edate = y + "-" + m + "-" + d;
            sdate = "2016-12-5";
            sql = "declare @sdate datetime " +
" declare @edate datetime" +
" set @sdate = '" + sdate + "'" +
" set @edate = '" + edate + "'" +
" select  dateadd(dd,num,@sdate) from " +
" (select isnull((select count(1) from sysobjects where id <t.id),0) as num from sysobjects t) a " +
" where dateadd(dd,num,@sdate) <=@edate ";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql);
            string sqll = "";
            string aa = "";
            string usernumsql = "";
            string usercountsql = "";
            int c1 = ds.Tables[0].Rows.Count;
            dayten = new string[c1];
            for (int j = 0; j < c1 - 1; j++) {
                sqll += "select count(*) from TempUserusejl where clickTime>'" + ds.Tables[0].Rows[j][0] + "' and clickTime<'" + ds.Tables[0].Rows[j + 1][0] + "' union all ";
                aa = ds.Tables[0].Rows[j][0].ToString();
                dayten[j] = aa.Substring(5, aa.Length - 13);

                usernumsql += " select COUNT(distinct user_IP) from TempUserusejl where clickTime>='" + aa + "' and clickTime<'" + ds.Tables[0].Rows[j + 1][0] + "' union all ";
                //union all
                usercountsql += "select COUNT(distinct user_IP) from TempUserusejl  where clickTime<='" + aa + "' union all ";
            }
            sqll = sqll.Substring(0, sqll.Length - 10);
            usernumsql = usernumsql.Substring(0, usernumsql.Length - 10);
            usercountsql = usercountsql.Substring(0, usercountsql.Length - 10);
            DataSet dss = new DataSet();
            dss = SqlFunction.Sql_DataAdapterToDS(sqll);

            DataSet dsss = new DataSet();
            dsss = SqlFunction.Sql_DataAdapterToDS(usernumsql);

            DataSet dssss = new DataSet();
            dssss = SqlFunction.Sql_DataAdapterToDS(usercountsql);
            int c2 = dss.Tables[0].Rows.Count;
            daytime = new string[c2];
            dayusernum= new string[c2];
            //usernumcount = new string[c2];
            for (int a = 0; a < c2; a++) {
                daytime[a] = dss.Tables[0].Rows[a][0].ToString();
                dayusernum[a] = dsss.Tables[0].Rows[a][0].ToString();
                //usernumcount[a] = dssss.Tables[0].Rows[a][0].ToString();
            }
            ds.Clear(); dss.Clear(); dsss.Clear(); dssss.Clear();
            ds.Dispose(); dss.Dispose(); dsss.Dispose(); dssss.Dispose();
        }
    }
}