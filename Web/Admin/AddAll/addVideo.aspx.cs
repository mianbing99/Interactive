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
using Web.Admin.model;
using System.IO;


namespace Web.Admin.AddAll {
    public partial class addVideo : System.Web.UI.Page {
        
        string[] xdlm = {"id","视频标题","信道名","路径","优先级"};
        string[] splm = new string[5];
        static public user u = new user();
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    Button1.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                    Button1.Enabled = false;
                    Button8.CssClass = "layui-btn layui-btn-primary layui-btn-mini";
                    Button8.Enabled = true;
                }
            }
        }
        int error = 0;

        public  void CheckText() {
            //开始检查数据
            string[] sArray, sTid;
            int count = GridView1.Rows.Count;
            sArray = new string[count];
            sTid = new string[count];
            for (int i = 0; i < count; i++) {
                string b=GridView1.Rows[i].Cells[0].Text.ToString();
                if (b != null && b != "&nbsp;" && b != ""  && b != "null") {
                    if (error != 0) {
                        Label1.Text = "表中存在空值,不能上传";
                        Layer(@"layer.alert('表中存在空值,不能上传', {
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'   
                            })", Page);
                    } else {
                        sArray[i] = b;
                    }
                } else {
                    error++;
                    break;
                }
                
                string a = GridView1.Rows[i].Cells[2].Text.ToString();
                if (a != "&nbsp;" && a != "" && a != null && a != "null") {
                    if (error != 0) {
                        Label1.Text = "表中存在空值,不能上传";
                        Layer(@"layer.alert('表中存在空值,不能上传', {
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                    } else {
                        sTid[i] = a;
                    }
                } else {
                    error++;
                    break;
                }
            }
            string data = "", datatid = "";
            string sql = "";
            data = string.Join("','", sArray.ToArray());
            datatid = string.Join("','", sTid.ToArray());
            sql = "select title from video where title in('" + data + "')";
            string cftitle;//重复标题
            cftitle=SqlFunction.Sql_ReturnNumberES(sql);
            
            if (cftitle != "") {
                Label1.Text = "数据库中已经存在表格中的视频标题,标题不能重复,错误标题:" + cftitle;

                Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
            } else {
                string info = isdou(sArray);
                if (info == "") {
                    sql = "select top 1 id from VideoType where tid in('" + datatid + "')";
                    string tidbmq;//Tid不明确
                    tidbmq=SqlFunction.Sql_ReturnNumberES(sql);
                   
                    if (tidbmq != "") {
                        string sql_1 = "select  tid from VideoType where id=" + tidbmq;
                        string rtid;
                        rtid=SqlFunction.Sql_ReturnNumberES(sql_1);
                        Label1.Text = "输入类型Tid不明确,错误Tid:" + rtid;
                        Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                    } else {
                        bool isnull = true;
                        for (int i = 0; i < sTid.Length; i++) {
                            sql = "select * from videoType where id =('" + sTid[i] + "')";
                            string cmdid = "";
                            cmdid=SqlFunction.Sql_ReturnNumberES(sql);

                            if (cmdid != null && cmdid != "") {

                            } else {
                                Label1.Text = "输入的类型Tid在数据库中不存在,错误Tid:" + sTid[i];
                                Layer(string.Format(@"layer.alert('{0}', {{
                                                      icon: 2,title: false,
                                                      shadeClose: true,skin: 'layer-ext-moon'  
                                                    }})", Label1.Text), Page);
                                isnull = false; return;
                            }
                        }
                        if (isnull) {
                            Button1.CssClass = " layui-btn layui-btn-primary layui-btn-mini";
                            Button1.Enabled = true;
                            Button8.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                            Button8.Enabled = false;
                            Label1.Text = "检查成功,可以上传";
                        }
                    }
                } else {
                    Label1.Text = "表中标题出现重复,错误信息:" + info;
                    Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                }
            }
        }

        /// <summary>
        /// 开始更新数据库
        /// </summary>
        /// <returns></returns>
        protected void startUpdate() {
            int count = GridView1.Rows.Count;
            bool isok = false;
            int sourcenum = 0;
            int state0source = 0;
            string [] state0index=new string[3];
            int index=0;
            string error = "";
            for (int j = 0; j < count; j++) {
                int xindaoNum = 0, xindaoIndex = 0;
                string sort = "";
                for (int i = 17; i > 0; i -= 7) {
                    if (GridView1.Rows[j].Cells[i].Text == "" || GridView1.Rows[j].Cells[i].Text == "&nbsp;") {
                    } else {
                        xindaoNum++;
                        xindaoIndex = i;
                        switch (xindaoIndex) {
                            case 3: sort = "999"; break;
                            case 10: sort = "99"; break;
                            case 17: sort = "9"; break;
                        }
                    }
                }
                
                DateTime dt = DateTime.Now;
                string sql = "";
                string cover = "", Describe = "";
                cover = GridView1.Rows[j].Cells[1].Text.ToString();
                Describe = GridView1.Rows[j].Cells[xindaoIndex + 2].Text.ToString();
                if (cover == "&nbsp;")
                    cover = "";
                if (Describe == "&nbsp;")
                    Describe = "";
                //video表增加
                sql = "insert into video (tid,title,cover,Describe,CreateDate,Sort,State,Definition,Advertisement)" +
                "values(" + "'" + GridView1.Rows[j].Cells[2].Text.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[0].Text.ToString() + "',"
                +"'" + cover + "',"
                +"'" + Describe + "',"//描述默认为信道1的原标题
                + "'" + dt.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[xindaoIndex + 1].Text.ToString() + "',"//优先级默认为信道1的优先级
                + "'" + GridView1.Rows[j].Cells[xindaoIndex + 4].Text.ToString() + "',"//状态默认为信道1的状态
                + "'',"//清晰度在video表中默认为空
                + "'')";//是否存在广告在video表中默认为空
                int Vnum = SqlFunction.Sql_UpdatePL(sql);
                if (Vnum > 0) {
                    
                    //Alert("成功添加"+Vnum+"条数据",Page);
                } else {
                    Layer(@"layer.alert('给Video表添加数据失败,已停止', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                    return;
                }
                //获取VideoId添加信道
                string sql_id = "select id from Video where title='" + GridView1.Rows[j].Cells[0].Text.ToString() + "'";
                string vid = SqlFunction.Sql_ReturnNumberES(sql_id);


                //videourl表增加
                List<bool> bl = new List<bool>();
                bl.Add(false);
                bl.Add(false);
                bl.Add(false);
                if (GridView1.Rows[j].Cells[17].Text != "" && GridView1.Rows[j].Cells[17].Text != "&nbsp;")
                    bl[2] = true;//有信道1
                if (GridView1.Rows[j].Cells[10].Text != "" && GridView1.Rows[j].Cells[10].Text != "&nbsp;")
                    bl[1] = true;//有信道2
                if (GridView1.Rows[j].Cells[3].Text != "" && GridView1.Rows[j].Cells[3].Text != "&nbsp;")
                    bl[0] = true;//有信道3
                for (int i = 0; i < 3; i++) {
                    if (bl[i]) {
                        int uindex = 0;
                        switch (i) {
                            case 0: sort = "999"; uindex = 3; break;
                            case 1: sort = "99"; uindex = 10; break;
                            case 2: sort = "9"; uindex = 17; break;
                        }
                        string Definition = "", Advertisement = "";
                        Definition = GridView1.Rows[j].Cells[uindex + 5].Text.ToString();
                        Advertisement = GridView1.Rows[j].Cells[uindex + 6].Text.ToString();
                        
                        if (Definition == "&nbsp;")
                            Definition = "";
                        if (Advertisement == "&nbsp;")
                            Advertisement = "";
                        DateTime dtU = DateTime.Now;
                        if (GridView1.Rows[j].Cells[uindex].Text.ToString() == "youku") {
                            sql = "insert into videoUrl (Vid,Source,Path,State,Sort,CreateDate,Definition,Advertisement,Describe)values("
                            + "'" + vid + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex].Text.ToString() + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 3].Text.ToString() + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 4].Text.ToString() + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 1].Text.ToString() + "',"
                            + "'" + dtU.ToString() + "',"
                            + "'" + Definition + "',"
                            + "'" + Advertisement + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 2].Text.ToString() +"')";
                        } else {
                            string temppath = GridView1.Rows[i].Cells[uindex + 3].Text.ToString();
                            temppath = AES.Encrypt(temppath);
                            sql = "insert into videoUrl (Vid,Source,TempPath,State,Sort,CreateDate,Definition,Advertisement,Describe)values("
                            + "'" + vid + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex].Text.ToString() + "',"
                            + "'" + temppath + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 4].Text.ToString() + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 1].Text.ToString() + "',"
                            + "'" + dtU.ToString() + "',"
                            + "'" + Definition + "',"
                            + "'" + Advertisement + "',"
                            + "'" + GridView1.Rows[j].Cells[uindex + 2].Text.ToString() + "')";
                        }
                        int Unum = SqlFunction.Sql_UpdatePL(sql);
                        if (Unum > 0) {
                            isok = true;
                            sourcenum++;
                        }
                    }
                }
                if (isok) {
                    try {
                        if (SqlFunction.Sql_ReturnNumberENQ("update videoType set state=1 where id=" + GridView1.Rows[1].Cells[2].Text.ToString()) > 0) {
                            //sourcenum++;
                        } 
                    } catch (Exception e) {
                        state0source++;
                        error = e.Message;
                        state0index[index] = j.ToString(); index++;
                    }
                    
                    
                } else {
                    Label1.Text = "添加中出现问题:Vnum=" + Vnum;
                    Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                }
           }
            string clientIP = HttpContext.Current.Request.UserHostAddress;
            u = (user)Session["Adminu"];
            if (sourcenum > 0) {
                Buckup.Buckup_Add("添加视频", u.name, clientIP, count);
                Label1.Text = "添加成功," + sourcenum + "条信道" + count + "行数据";
                Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
            } else {
                if (state0source == 0)
                    Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", "全部行添加失败"), Page);
                else {
                    Buckup.Buckup_Add("添加视频", u.name, clientIP, count);
                    string errorinfo = "";
                    for (int m = 0; m < state0index.Length; m++) {
                        if (state0index[m] != null && state0index[m] != "") {
                            errorinfo += state0index[m]+",";
                        }
                    }
                    errorinfo = errorinfo.Substring(0,errorinfo.Length-1);
                    Label1.Text = ("添加成功," + count + 
                        "行数据,但是当前类型是状态为0,请修改为1就可以显示,发生在第"+errorinfo+"条信道" +
                        error).Replace("'", "〞");
                    Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                }
            }
        }

        /// <summary>
        /// 检查excel标题是否出现重复
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected string isdou(string []a) {
            bool flag = true;
            string info = "";
            for (int i = 0; i < a.Length - 1; i++) {
                for (int j = i + 1; j < a.Length; j++) {
                    if (a[i] == a[j] && a[i] != "&nbsp;" && a[i] != "" && a[i] != null && a[i] != "null") {
                        info = a[i] +"="+ a[j];
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

        protected void Button1_Click(object sender, EventArgs e) {
            //UploadFile();
            startUpdate();
            
            
        }
        public void Alert(string info, Page p) {
            //FileUpload1.FileContent.Dispose();
            //DeleteFolder(Server.MapPath("~/UpdateExcel/"));
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.8.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public void UploadFile() {
            if (FileUpload1.HasFile) {
               
                FileUpload1.SaveAs(Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName);
                Layer(@"layer.alert('上传成功', {
                              icon: 1,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }  
        }

        protected void Button8_Click(object sender, EventArgs e) {

            Directory.CreateDirectory(Server.MapPath("~/UpdateExcel/"));
            string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
             "Data Source=" + Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName +
             ";Extended Properties=Excel 8.0;";
             OleDbConnection objConn = new OleDbConnection(sConnectionString);
            if (FileUpload1.HasFile) {
                try {
                    FileUpload1.SaveAs(Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName);
                    //建立EXCEL的连接
                    if (objConn.State == ConnectionState.Closed)
                        objConn.Open();
                    OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet1$]", objConn);
                    OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
                    objAdapter1.SelectCommand = objCmdSelect;

                    DataSet objDataset1 = new DataSet();
                    try {
                        objAdapter1.Fill(objDataset1, "XLData");
                        string temp = "";
                        for (int i = 0; i < objDataset1.Tables[0].Rows.Count; i++) {//删除空行
                            temp = objDataset1.Tables[0].Rows[i][0].ToString();
                            if (temp == null || temp == "" || temp == "&nbsp;" || temp == "null") {
                                objDataset1.Tables[0].Rows[i].Delete();
                            }
                        }
                        GridView1.Dispose();
                        GridView1.DataSource = objDataset1.Tables[0].DefaultView; //测试代码,用来测试是否能读出EXCEL上面的数据
                        GridView1.DataBind();
                        CheckText();
                    } catch (Exception ee) {
                        try {
                            GridView1.Dispose();
                        } catch { }
                        Label1.Text = ("检查失败!可能是选错了表,或者表中信息错误,错误信息:" + ee.Message).Replace("'", "〞");
                        Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                        Button1.CssClass = " layui-btn-disabled touming";
                        Button1.Enabled = false;
                    }
                } catch (Exception ee) {
                    Label1.Text = ("检查失败!可能是不支持excel2007版本及以上,错误信息"+ee.Message).Replace("'", "〞");;
                    Layer(string.Format(@"layer.alert('{0}', {{
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", Label1.Text), Page);
                    Button1.CssClass = " layui-btn-disabled touming";
                    Button1.Enabled = false;
                } finally {
                    if (objConn.State == ConnectionState.Open)
                        objConn.Close();
                }
            } else {
                Label1.Text = "您没有选择文件";
                Layer(@"layer.alert('您没有选择文件', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }
        }
        /// <summary>
        /// 清除指定文件夹内的内容,不删除文件夹
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        public static void DeleteFolder(string dir) {
            
            foreach (string d in Directory.GetFileSystemEntries(dir)) {
                if (File.Exists(d)) {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                } else {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0) {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            //GridView1.DeleteRow(e.RowIndex);
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e) {

        }
    }
}