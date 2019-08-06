using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;

namespace Web.Admin.chartsTJ {
    public partial class Line3 : System.Web.UI.Page {
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
            sql = @"select COUNT(a.date)as 每日在线人数 from 
(select substring(time,1,10)as date,account,pwd from UserAccessRecord group by substring(time,1,10),account,pwd) a
group by a.date order by date";
            DataSet dss = new DataSet();
            dss = SqlFunction.Sql_DataAdapterToDS(sql);
            int c2 = dss.Tables[0].Rows.Count;
            time2 = new Int32[c2 + 160];
            //旧数据 2016-12-3~2017-05-11 共160天
            int[] oldinfo = new int[160]{1403,1865,1601,1387,1469,2497,2235,2481,2589,1769,
                2022,1765,1967,1945,2330,2424,1807,2246,2234,1942,1935,2561,2398,1701,1873,
                1895,1987,2895,2698,2425,2870,2242,2246,2500,2349,2985,2702,2218,2480,2425,
                2399,2577,2365,2826,2844,2759,2908,2851,2646,2890,2563,2623,2737,2495,2314,
                2731,2123,2668,2332,2136,2857,2236,2347,2837,2176,2504,2546,2135,2341,2456,
                2316,2193,1731,1753,1783,2027,2084,2055,2043,1842,2642,1914,1856,1814,2478,
                2346,1690,1632,1806,1572,1569,2205,2188,1622,1491,1353,1435,1725,2612,2401,
                1791,1561,1638,1502,1842,2241,2178,1528,1530,1568,1644,1649,2041,2159,1262,
                1467,1309,1518,1405,1306,1674,1635,2160,1795,1251,1282,2050,2172,1571,1369,
                1271,1456,1335,1827,1828,1221,1201,1286,1751,1617,1951,1623,977,1243,1162,
                1309,1340,1794,1679,1616,1220,1330,1314,1454,1705,1656,1242,1019,1246,1297};
            //2017-05-12开始的数据
            for (int j = 160; j < c2 + 160; j++) {
                time2[j] = Convert.ToInt32(dss.Tables[0].Rows[j - 160]["每日在线人数"]);
            }
            Array.Copy(oldinfo, 0, time2, 0, oldinfo.Length);
        }
    }
}