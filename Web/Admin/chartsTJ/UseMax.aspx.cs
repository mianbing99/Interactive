using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;

namespace Web.Admin.chartsTJ {
    public partial class UseMax : System.Web.UI.Page {
        static int pageNum = 10;
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    GridView1.HeaderStyle.Wrap = true;
                    GridView2.HeaderStyle.Wrap = true;
                    GridView3.HeaderStyle.Wrap = true;
                    GridView4.HeaderStyle.Wrap = true;
                    databing();
                }
            }
        }
        public void databing() {
            gv1databing();
            gv2databing();
            gv3databing();
            gv4databing();
        }
        private void gv1databing() {
            string sql = @"select top 100  videoType as 类型播放最多TOP100,countNum as 点击次数 from UserAccessType where videoType!=''  order by 点击次数 desc";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql);
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            ds.Tables.Clear();
        }
        //未修改
        private void gv2databing() {
            string sql2 = @"select addr as 地域播放最多,count(*)as 点击次数 from UserAccessRecord group by addr order by 点击次数 desc";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql2);
            GridView2.DataSource = ds.Tables[0];
            GridView2.DataBind();
            ds.Tables.Clear();
        }
        private void gv3databing() {
            string sql3 = @"select  top 100 account as 账号TOP100,pwd as 密码,COUNT(account)as 点击次数  from UserAccessRecord group by account,pwd order by 点击次数 desc";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql3);
            GridView3.DataSource = ds.Tables[0];
            GridView3.DataBind();
            ds.Tables.Clear();
        }
        private void gv4databing() {
            string sql4 = @"
select top 100 (select title from Video where id = vid)as 视频播放次数TOP100 ,count(*) as 点击次数
from UserAccessRecord  group by vid order by 点击次数 desc";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql4);
            GridView4.DataSource = ds.Tables[0];
            GridView4.DataBind();
            ds.Tables.Clear();
        }


        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e) {

            e.Row.Cells[0].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word;");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
            //首先判断是否是数据行  
            if (e.Row.RowType == DataControlRowType.DataRow) {
                //当鼠标停留时更改背景色  
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='rgba(0,0,0,0.2)'");
                //当鼠标移开时还原背景色  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView1.PageIndex = 0;
            } else {
                GridView1.PageIndex = e.NewPageIndex;
            }
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView2.PageIndex = 0;
            } else {
                GridView2.PageIndex = e.NewPageIndex;
            }
        }
        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView3.PageIndex = 0;
            } else {
                GridView3.PageIndex = e.NewPageIndex;
            }
        }
        protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView4.PageIndex = 0;
            } else {
                GridView4.PageIndex = e.NewPageIndex;
            }
        }
        protected void GridView1_PageIndexChanged(object sender, EventArgs e) {
            gv1databing();
        }
        protected void GridView2_PageIndexChanged(object sender, EventArgs e) {
            gv2databing();
        }
        protected void GridView3_PageIndexChanged(object sender, EventArgs e) {
            gv3databing();
        }
        protected void GridView4_PageIndexChanged(object sender, EventArgs e) {
            gv4databing();
        }

    }
}