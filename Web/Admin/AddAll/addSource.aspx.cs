using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using Web.Admin.Class;
using System.IO;
using Web.Admin.model;

namespace Web.Admin.AddAll {
    public partial class addSource : System.Web.UI.Page {
        private DataSet dsExcel;
        string[] xdlm = { "id", "视频标题", "信道名", "路径", "优先级" };
        static public user u = new user();
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                Button3.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                Button3.Enabled = false;
                Button2.CssClass = " layui-btn layui-btn-primary layui-btn-mini ";
                Button2.Enabled = true;
            }
        }
        protected void Button2_Click(object sender, EventArgs e) {
            if (FileUpload2.HasFile) {
                ShowExcelContent();  //导入进gridview
            } else {
                Label2.Text = "您没有选择文件";
                Layer(@"layer.alert('您没有选择文件', {
                              icon: 0,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }
        }
        public void ToSqlServer() {
            //string strCon = @"Data Source=bds13396396.my3w.com;initial catalog=bds13396396_db;user id=bds13396396;password=icox2015;multipleactiveresultsets=True;";
            //data source=bds13396396.my3w.com;initial catalog=bds13396396_db;user id=bds13396396;password=icox2015;multipleactiveresultsets=True;
            try { 
            int yes = 0;
            int no = 0;
                int c1 = GridView2.Rows.Count;
                //MessageBox.Show("表格行数=" + GridView2.Rows.Count, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                for (int i = 0; i < c1; i++) {
                    if (GridView2.Rows[i].Cells[2].Text == "youku") {
                        string temppath = GridView2.Rows[i].Cells[3].Text.ToString();
                        string sql = "insert into dbo.VideoUrl(Vid,Source,Path,sort,Describe) values(" + GridView2.Rows[i].Cells[0].Text + ",'"
                       + GridView2.Rows[i].Cells[2].Text.ToString() + "','" + GridView2.Rows[i].Cells[3].Text.ToString() + "','" 
                       + GridView2.Rows[i].Cells[4].Text.ToString()+"','" + GridView2.Rows[i].Cells[5].Text.ToString() +"') ";
                        if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0)
                            yes++;
                        else
                            no++;
                    } else {
                        
                        string temppath = GridView2.Rows[i].Cells[3].Text.ToString();
                        temppath = AES.Encrypt(temppath);
                        string sql = "insert into dbo.VideoUrl(Vid,Source,TempPath,sort,Describe) values(" + GridView2.Rows[i].Cells[0].Text + ",'"
                       + GridView2.Rows[i].Cells[2].Text.ToString() + "','" + temppath + "','"
                       + GridView2.Rows[i].Cells[4].Text.ToString()+"','" + GridView2.Rows[i].Cells[5].Text.ToString() + "') ";
                        if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0)
                            yes++;
                        else
                            no++;
                    }
                }
                string clientIP = HttpContext.Current.Request.UserHostAddress;
                u = (user)Session["Adminu"];
                Buckup.Buckup_Add("添加信道", u.name, clientIP, yes);
                //MessageBox.Show("导入SqlServer成功！成功数:" + yes + "失败数：" + no, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Label2.Text = "导入成功:" + yes + "; 失败:" + no;
            } catch (Exception ex) {
                Label2.Text = ("导入过程中出错,请联系开发人员:"+ex.Message).Replace("'", "〞");
                Layer(string.Format(@"layer.alert('{0}', {
                              icon: 5,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Label2.Text), Page);

            } finally {
            }
        }
        public void ShowExcelContent() {
            string strCon = @"Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + Server.MapPath("~/UpdateExcel/") + FileUpload2.FileName + ";Extended Properties=Excel 8.0";
            string strSql = "select * from [Sheet1$]";
            OleDbConnection oleDbCon = new OleDbConnection(strCon);
            try {
                Directory.CreateDirectory(Server.MapPath("~/UpdateExcel/"));//创建文件夹
                FileUpload2.SaveAs(Server.MapPath("~/UpdateExcel/") + FileUpload2.FileName);//把excel上传至文件夹
                OleDbDataAdapter oleDbDa = new OleDbDataAdapter(strSql, oleDbCon);
                dsExcel = new DataSet();
                if (oleDbCon.State == ConnectionState.Closed)
                    oleDbCon.Open();
                oleDbDa.Fill(dsExcel, "Info");
               
                string temp = "";
                for (int i = 0; i < dsExcel.Tables[0].Rows.Count; i++) {//删除空行
                    temp = dsExcel.Tables[0].Rows[i][0].ToString();
                    if (temp == null || temp == "" || temp == "&nbsp;" || temp == "null") {
                        dsExcel.Tables[0].Rows[i].Delete();
                    }
                }
                GridView2.Dispose();
                GridView2.DataSource = dsExcel.Tables[0];
                GridView2.DataBind();
                checkXD();//开始检查
                //Label2.Text = "检查成功,共有"+GridView2.Rows.Count+"条";
            } catch (Exception e) {
                try {
                    GridView2.Dispose();
                } catch { }
                Label2.Text = ("检查失败!可能是选错了表,或者是表中列的顺序有问题"+e.Message).Replace("'", "〞");
                Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label2.Text), Page);
                Button3.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                Button3.Enabled = false;
            } finally { 
                if (oleDbCon.State == ConnectionState.Open)
                    oleDbCon.Close();
            }
        }
        public void checkXD() {
            int count2 = GridView2.Rows.Count;
            for (int i = 0; i < count2 ; i++) {
                for (int j = 0; j < 4; j++) {
                    string b = GridView2.Rows[i].Cells[j].Text.ToString();
                    if (b == null || b == "&nbsp;" || b == "" || b == "null") {
                        Label2.Text = "表中存在空值,不能上传";
                        Layer(@"layer.alert('表中存在空值,不能上传', {
                              icon: 2,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'   
                            })", Page);
                        return;
                    }
                }
            }

            string rinfo = "";
   
            string[] sArray;
            int count = GridView2.Rows.Count;
            sArray = new string[count];
                for (int i = 0; i < count; i++) {
                    sArray[i] = GridView2.Rows[i].Cells[0].Text;
                }
                string info = isdou(sArray);
                if (info == "") {
                    string data = string.Join(",", sArray.ToArray());
                    string sql = "select count(*) from video where id in(" + data + ")";
                    int gc = GridView2.Rows.Count;
                    int sc = Convert.ToInt32(SqlFunction.Sql_ReturnNumberES(sql));
                    if (gc == sc) {
                        Label2.Text = "检查成功,可以上传,共" + gc + "条";
                        rinfo = Label2.Text;
                        Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 6,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label2.Text), Page);
                        Button2.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                        Button2.Enabled = false;
                        Button3.CssClass = " layui-btn layui-btn-primary layui-btn-mini";
                        Button3.Enabled = true;
                    } else {
                        rinfo = Label2.Text = "表中部分titl列值,在数据库中不存在,共有" + (gc - sc) + "条";
                        Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label2.Text), Page);
                        Button2.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                        Button2.Enabled = false;
                    }
                } else {
                    rinfo = Label2.Text = "表中id列值出现重复";
                    Layer(@"layer.alert('表中id列值出现重复', {
                              icon: 2,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                    Button2.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                    Button2.Enabled = false;
                }
            

        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.Header) {
                for (int i = 0; i < e.Row.Cells.Count; i++) {
                    if (e.Row.Cells[i].Text != xdlm[i]) {
                        Label2.Text = "表中列名错误应为:id,title,source,url,sort";
                        Layer(@"layer.alert('表中列名错误应为:id,title,source,url,sort', {
                              icon: 2,title: false,shade:false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                        Button2.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                        Button2.Enabled = false;
                    }
                }
            }
        }
        protected void Button3_Click(object sender, EventArgs e) {
            ToSqlServer();

        }
        public void Alert(string info, Page p) {
            //FileUpload1.FileContent.Dispose();
            //DeleteFolder(Server.MapPath("~/UpdateExcel/"));
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        /// <summary>
        /// 检查excel标题是否出现重复
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected string isdou(string[] a) {
            bool flag = true;
            string info = "";
            for (int i = 0; i < a.Length - 1; i++) {
                for (int j = i + 1; j < a.Length; j++) {
                    if (a[i] == a[j] && a[i] != "&nbsp;" && a[i] != "" && a[i] != null && a[i] != "null") {
                        info = a[i] + "=" + a[j];
                        flag = false;
                        break;
                    }
                }
            }
            if (flag) {
                return "";
            } else {
                return info;
            }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e) {

        }

    }
}