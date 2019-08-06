using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web {
    public partial class Test : System.Web.UI.Page {
        string hostcs = Util.GetConfig("CourseHost");
        private DataTable dt_data = new DataTable();
        string str_filename = @"C:\Users\ICOX\Desktop\111.xls";
        protected void Page_Load(object sender, EventArgs e) {
            
        }
        protected void Button1_Click(object sender, EventArgs e) {
            String strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                             "Data Source=" + this.str_filename + ";" +
                             "Extended Properties='Excel 8.0;'";
            OleDbConnection objConn = new OleDbConnection(strConn);
            OleDbCommand objCmd = new OleDbCommand("SELECT * FROM [Sheet1$]", objConn);
            OleDbDataAdapter objDA = new OleDbDataAdapter(objCmd);
            objDA.Fill(dt_data);
            dt_data.Columns.Add("prod_ids", typeof(string));
            dt_data.Columns.Add("publish_ids", typeof(string));
            dt_data.Columns.Add("file_size", typeof(string));
            dt_data.Columns.Add("course_ware_no", typeof(string));
            for (int i = 0; i < dt_data.Rows.Count; i++) {
                DataRow dr = dt_data.Rows[i];
                string ISBN = dr["ISBN"].ToString().Trim();
                string GrName = dr["年级"].ToString().Trim();
                string PrName = dr["出版社"].ToString().Trim();
                string CaName = dr["单元目录"].ToString().Trim();
                string Text = dr["课文"].ToString().Trim();
                string ViName = dr["视频名称"].ToString().Trim();
                string SuName = dr["科目"].ToString().Trim();
                string RouteAress = dr["视频路径"].ToString().Trim();
                string Sestr = dr["学期"].ToString().Trim();
                string Ststr = dr["学段"].ToString().Trim();
                string Page = "0";//dr["页码"].ToString().Trim();
                string Img = dr["图片路径"].ToString().Trim();
                string CaNum = dr["目录序号"].ToString().Trim();
                string Ystr = dr["年份版本"].ToString().Trim();
                if (string.IsNullOrEmpty(ISBN)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行ISBN为空 \");</script>");
                    return;
                }
                if (string.IsNullOrEmpty(GrName)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行年级为空 \");</script>");
                    return;
                }
                if (string.IsNullOrEmpty(PrName)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行出版社为空 \");</script>");
                    return;
                }
                if (string.IsNullOrEmpty(CaName)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行目录为空 \");</script>");
                    return;
                }
                if (string.IsNullOrEmpty(Text)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行课文为空 \");</script>");
                    return;
                }
                if (string.IsNullOrEmpty(ViName)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行视频名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(RouteAress)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行视频路径名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(Sestr)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行学期名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(Ststr)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行学段名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(Page)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行页码名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(Img)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行图片路径名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(CaNum)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行目录序号名称为空 \");</script>");
                    return;
                } if (string.IsNullOrEmpty(Ystr)) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "行年份版本名称为空 \");</script>");
                    return;
                }
                int Semester;
                int Study;
                int Year = Convert.ToInt32(Ystr);
                if (Sestr == "上" || Sestr == "上学期" || Sestr == "上册") {
                    Semester = 1;
                } else if (Sestr == "下" || Sestr == "下学期" || Sestr == "下册") {
                    Semester = 1;
                } else {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请检查第" + i + "行学期输入是否规范！ \");</script>");
                    return;
                }
                if (Ststr == "小学") {
                    Study = 1;
                } else if (Ststr == "初中") {
                    Study = 2;
                } else if (Ststr == "高中") {
                    Study = 3;
                } else {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请检查第" + i + "行学段输入是否规范！ \");</script>");
                    return;
                }
                //Response.Write(" <script type=\"text/javascript\"> alert(\"123 " + dr["科目"].ToString().Trim() + "  \");</script>");
                string strsql = "EXEC proc_add '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'";
                string sql = string.Format(strsql, ISBN, GrName, PrName, CaName, Text, ViName, SuName, RouteAress, Semester, Study, Page, Img, CaNum, Year);
                //string sql = "delete [dbo].[Video]";
                string connstr = "server=182.254.134.51;database=Teaching; user id=MDB;password=Main@JLF955icox;";
                try {
                    SqlConnection conn = new SqlConnection(connstr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int jg = cmd.ExecuteNonQuery();
                    conn.Close();
                } catch (Exception ex) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"第" + ex + "次 \");</script>");
                }

                Response.Write(" <script type=\"text/javascript\"> alert(\"第" + i + "次插入成功 \");</script>");
            }

        }
    }
}