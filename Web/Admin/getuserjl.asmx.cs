using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Web.Admin.Class;
using LitJson;

namespace Web.Admin {
    /// <summary>
    /// getuserjl 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class getuserjl : System.Web.Services.WebService {
        protected string[] time;
        protected Int32[] time2;
        [WebMethod]
        public void Getdate() {
            string sql = "";
            string tsql="",sdate="2016-12-03",edate=DateTime.Now.ToString();
            tsql =string.Format( @"declare @sdate datetime 
            declare @edate datetime
            set @sdate = '{0}'
            set @edate = '{1}'
            select  dateadd(dd,num,@sdate) from 
                (select isnull((select count(1) from sysobjects where id <t.id),0) as num from sysobjects t) a 
            where dateadd(dd,num,@sdate) <=@edate",sdate,edate) ;
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(tsql);
                int c1 = ds.Tables[0].Rows.Count;
                time = new string[c1];
                for (int i=0;i<c1 ;i++ ) {
                    time[i] = ds.Tables[0].Rows[i]["Column1"].ToString();
                    time[i] = time[i].Replace(" 0:00:00", "");
                    sql += string.Format(@" select COUNT(distinct user_IP) from TempUserusejl 
 where clickTime>='{0} 00:00:00.000' and clickTime<='{0} 23:59:59.999'
 union all", time[i]);
                }
                sql = sql.Substring(0, sql.Length - 10);
                DataSet dss = new DataSet();
                dss = SqlFunction.Sql_DataAdapterToDS(sql);
                int c2 = dss.Tables[0].Rows.Count;
                time2 = new Int32[c2];
                for (int j = 0; j < c2; j++) {
                    time2[j] =Convert.ToInt32(dss.Tables[0].Rows[j]["Column1"]);
                }
                Dictionary<string, Int32> dataa = new Dictionary<string, Int32>();
        int y = 2016, m = 12, d = 3;
        for (int i = 0; i < time2.Length-1; i++)
        {
            dataa.Add("[Date.UTC(" + y + "," + m + "," + d + "),", time2[i]);
            d++;
            if (m != 2) {
                if (m != 4 && m != 6 && m != 9 && m != 11)//小月30天
                {
                    if (d > 31) {
                        d = 1; m++;
                        if (m > 12)
                        {
                            y++; m = 1;d=30;
                        }
                    }
                } else {//大月
                    if (d > 30) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 30;
                        }
                    }
                }
            } else {//2月
                if ((y % 4 == 0 && y % 100 != 0) || (y % 400 == 0)) {//29天
                    if (d > 29) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 30;
                        }
                    }
                } else {//28天
                    if (d > 28) {
                        d = 1; m++;
                        if (m > 12) {
                            y++; m = 1; d = 30;
                        }
                    }
                }
            }
        }
        string data = JsonMapper.ToJson(dataa);
        Write(data);
        }
        private void Write(string a) {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            a=a.Replace("{\"", "[");
            a = a.Replace("}", "]];");
            a = a.Replace("\":", "");
            a = a.Replace(",\"", "],");
            Context.Response.Write(a);
        }
        
    }
}
