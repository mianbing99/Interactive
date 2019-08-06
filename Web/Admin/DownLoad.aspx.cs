using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;
using Web.Admin.model;

namespace Web.Admin {
    public partial class DownLoad : System.Web.UI.Page {
        static public int state;
        static public int select1, add, expore;
        static DataSet ds = new DataSet();
        static string sql = "";
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                user u = new user();
                u = (user)Session["Adminu"];
                state = u.state;
                select1 = u.select;
                add = u.add;
                expore = u.export;
                GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
        protected void btn_dlexcel_dc_Click(object sender, EventArgs e) {
            string s_utj = s_date.Value;
            string e_utj = e_date.Value;
            string where = "";
            if (s_utj == null)
                s_utj = "";
            if (e_utj == null)
                e_utj = "";
            where = " where time>='" + s_utj + "' and time<='" + e_utj + "'";
            if (where == " where time>='' and time<=''")
                where = "";

            string temp = sql;
            sql = sql + where + " order by time desc";
            DataSet ds2 = new DataSet();
            ds2 = SqlFunction.Sql_DataAdapterToDS(sql);
            sql = temp;
            if (!ToExcel.CreateExcel(ds2, "导出数据记录", 1, null, null, null)) {
                LayerA("当前时间段没有信息,无法导出", 5, Page);
            }
        }
        public static void LayerA(string info, int icon, Page p) {
            string script = "<script type='text/javascript' src='js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>layer.alert('";
            script = script + info + "!', {title: false,icon:" + icon + ",skin: 'layer-ext-moon' })" + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e) {
            e.Row.Cells[0].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word;");
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e) {
            GridView1.DataSource = ds;
            DataTable dtt = new DataTable();
            dtt = ds.Tables[0];
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView1.PageIndex = 0;
            } else {
                GridView1.PageIndex = e.NewPageIndex;
            }
        }


        protected void Button1_Click(object sender, EventArgs e) {

            //导出记录绑定
            sql = "select admin as 操作员,ip,type as 格式,time as 导出时间 from Backup_downloadExcel ";
            lab.InnerText = "导出记录";
            ds.Tables.Clear();
            ds = SqlFunction.Sql_DataAdapterToDS(sql+" order by time desc");
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            dc_btn.Attributes.Add("style", "display:block");
        }

        protected void Button2_Click(object sender, EventArgs e) {
            //修改表操作
            sql = "select tb as 表,columns as 被修改的列,update_id as 被修改的Id,beforeInfo as 修改前,afterInfo as 修改后,time as 时间,operator as 管理员,ip from Backup_update_tb";
            lab.InnerText = "修改表记录";
            ds.Tables.Clear();
            ds = SqlFunction.Sql_DataAdapterToDS(sql + " order by time desc");
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            dc_btn.Attributes.Add("style", "display:block");
        }

        protected void Button3_Click(object sender, EventArgs e) {
            //增加和删除表操作
            sql = @"select tb as 表,time as 时间,operator as 管理员,ip,
                        column_value1 as 字段1,column_value2 as 字段2,column_value3 as 字段3,
                        column_value4 as 字段4,column_value5 as 字段5,column_value6 as 字段6,
                        column_value7 as 字段7,column_value8 as 字段8,column_value9 as 字段9,
                        column_value10 as 字段10 from Backup_delete_tb";
            lab.InnerText = "增加和删除表记录";
            ds.Tables.Clear();
            ds = SqlFunction.Sql_DataAdapterToDS(sql + " order by time desc");
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            dc_btn.Attributes.Add("style", "display:block");
        }

        protected void Button4_Click(object sender, EventArgs e) {
            //账号操作绑定
            sql = "select operation as 格式,admin as 操作员,ip,time as 时间,username as 被影响的账号,info as 被影响的内容 from Backup_AccountOperation";
            lab.InnerText = "账号注册修改记录";
            ds.Tables.Clear();
            ds = SqlFunction.Sql_DataAdapterToDS(sql + " order by time desc");
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            dc_btn.Attributes.Add("style", "display:block");
        }

        protected void Button5_Click(object sender, EventArgs e) {
            //账号操作绑定
            sql = "select tb as 增加的表,time as 时间,admin as 操作员,ip,addnum as 增加的个数 from Backup_add_tb ";
            lab.InnerText = "增加记录";
            ds.Tables.Clear();
            ds = SqlFunction.Sql_DataAdapterToDS(sql + " order by time desc");
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            dc_btn.Attributes.Add("style", "display:block");
        }


    }
}