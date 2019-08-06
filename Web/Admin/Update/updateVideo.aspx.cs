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


namespace Web.Admin.Update {
    public partial class updateVideo : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    Button1.Enabled = false;
                    Button8.Enabled = true;
                    Layer(@"layer.open({
                                    type: 1,
                                    shadeClose: true, //点击遮罩关闭
                                    content: $('#tips')
                                  });", Page);

                }
            }
        }


        public void CheckText() {
            //开始检查数据
            string[] sArray, sTid;
            int count = GridView1.Rows.Count;
            int ccount = GridView1.Columns.Count;
            sArray = new string[count];
            sTid = new string[count];
            for (int i = 0; i < count; i++) {
                sArray[i] = GridView1.Rows[i].Cells[0].Text.ToString();
                sTid[i] = GridView1.Rows[i].Cells[2].Text.ToString();
            }
            string data = "", datatid = "";
            string sql = "";
            data = string.Join("','", sArray.ToArray());
            datatid = string.Join("','", sTid.ToArray());
            sql = "select title from video where title in('" + data + "')";
            string cftitle;//重复标题
            cftitle = SqlFunction.Sql_ReturnNumberES(sql);
            if (cftitle != "") {
                Layer(string.Format(@"layer.alert('数据库中已经存在表格中的标题,标题不能重复,错误标题:{0}', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", cftitle), Page);
            } else {
                string info = isdou(sArray);
                if (info == "") {
                    sql = "select top 1 id from VideoType where tid in('" + datatid + "')";
                    string tidbmq;//Tid不明确
                    tidbmq = SqlFunction.Sql_ReturnNumberES(sql);
                    if (tidbmq != "") {
                        string sql_1 = "select  tid from VideoType where id=" + tidbmq;
                        Layer(string.Format(@"layer.alert(输入类型Tid不明确,例如:您不能把一条视频归为老人专区,必须是老人专区-戏曲专区-等等....错误Tid:{0}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", SqlFunction.Sql_ReturnNumberES(sql_1)), Page);
                        return;
                    } else {
                        bool isnull = true;
                        for (int i = 0; i < sTid.Length; i++) {
                            sql = "select * from videoType where id =('" + sTid[i] + "')";
                            if (SqlFunction.Sql_ReturnNumberES(sql) != "") {
                                if (GridView1.Rows[i].Cells[3].Text != "" && GridView1.Rows[i].Cells[3].Text != "&nbsp;") {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 4].Text == "" || GridView1.Rows[i].Cells[m + 4].Text == "&nbsp;") {
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 4+ 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                } else {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 4].Text != "" && GridView1.Rows[i].Cells[m + 4].Text != "&nbsp;") {
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 4 + 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                }
                                if (GridView1.Rows[i].Cells[10].Text != "" && GridView1.Rows[i].Cells[10].Text != "&nbsp;") {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 10].Text == "" || GridView1.Rows[i].Cells[m + 10].Text == "&nbsp;") {
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 10 + 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                } else {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 10].Text != "" && GridView1.Rows[i].Cells[m + 10].Text != "&nbsp;") {
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 4 + 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                }
                                if (GridView1.Rows[i].Cells[17].Text != "" && GridView1.Rows[i].Cells[17].Text != "&nbsp;") {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 17].Text == "" || GridView1.Rows[i].Cells[m + 17].Text == "&nbsp;") {
                                            
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 17 + 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                } else {
                                    for (int m = 0; m < 6; m++) {
                                        if (GridView1.Rows[i].Cells[m + 17].Text != "" && GridView1.Rows[i].Cells[m + 17].Text != "&nbsp;") { 
                                            Layer(string.Format(@"layer.alert('信道中有多余值,第{0}行,第{1}', {{
                                              icon: 5,title: false,
                                              skin: 'layer-ext-moon' 
                                            }})", i + 1, m + 17 + 1), Page);
                                            isnull = false; return;
                                        }
                                    }
                                }
                            } else {
                                Layer(string.Format(@"layer.alert('输入的类型Tid在数据库中不存在,错误Tid:{0}', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", sTid[i]), Page);
                                isnull = false; return;
                            }
                        }
                        if (isnull) {
                            Button1.Enabled = true;
                            Layer(@"layer.alert('检查完毕,可以开始上传', {
                              icon: 1,title: false,
                              skin: 'layer-ext-moon' 
                            })", Page);
                        }
                    }
                } else {
                    Layer(string.Format(@"layer.alert('表中标题出现重复,错误信息:{0}', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", info), Page);
                }
            }
        }

        /// <summary>
        /// 开始更新数据库
        /// </summary>
        /// <returns></returns>
        protected string startUpdate() {
            int count = GridView1.Rows.Count;
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
                //video表增加
                sql = "insert into video (tid,title,cover,Describe,CreateDate,Sort,State,Definition,Advertisement)" +
                "values(" + "'" + GridView1.Rows[j].Cells[2].Text.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[0].Text.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[1].Text.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[xindaoIndex + 2].Text.ToString() + "',"//描述默认为信道1的原标题
                + "'" + dt.ToString() + "',"
                + "'" + GridView1.Rows[j].Cells[xindaoIndex + 1].Text.ToString() + "',"//优先级默认为信道1的优先级
                + "'" + GridView1.Rows[j].Cells[xindaoIndex + 4].Text.ToString() + "',"//状态默认为信道1的状态
                + "'',"//清晰度在video表中默认为空
                + "'')";//是否存在广告在video表中默认为空
                int Vnum = SqlFunction.Sql_UpdatePL(sql);
                if (Vnum > 0) {
                    //Alert("成功添加"+Vnum+"条数据",Page);
                } else {
                    Layer(string.Format(@"layer.alert('添加中出现问题:Vnum={0}', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", Vnum), Page);
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
                        DateTime dtU = DateTime.Now;
                        sql = "insert into videoUrl (Vid,Source,Path,TempPath,State,Sort,CreateDate,Definition,Advertisement)values("
                        + "'" + vid + "',"
                        + "'" + GridView1.Rows[j].Cells[uindex].Text.ToString() + "',"
                        + "'" + GridView1.Rows[j].Cells[uindex + 3].Text.ToString() + "',"
                        + "'',"//TempPath默认为空
                        + "'" + GridView1.Rows[j].Cells[uindex + 4].Text.ToString() + "',"
                        + "'" + GridView1.Rows[j].Cells[uindex + 1].Text.ToString() + "',"
                        + "'" + dtU.ToString() + "',"
                        + "'" + GridView1.Rows[j].Cells[uindex + 5].Text.ToString() + "',"
                        + "'" + GridView1.Rows[j].Cells[uindex + 6].Text.ToString() + "')";
                        int Unum = SqlFunction.Sql_UpdatePL(sql);
                        if (Unum > 0) {
                            Layer(string.Format(@"layer.alert('成功添加{0}条数据', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", Unum), Page);
                        } else {
                            Layer(string.Format(@"layer.alert('添加中出现问题:Vnum={0}', {{
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            }})", Vnum), Page);
                        }
                    }
                }
            }
            //videotype表增加
            return "";
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
                    if (a[i] == a[j]) {
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

        protected void Button1_Click(object sender, EventArgs e) {
            //UploadFile();
            startUpdate();
        }
        public static void Alert(string info, Page p) {
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='js/jquery-1.8.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public void UploadFile() {
            if (FileUpload1.HasFile) {

                FileUpload1.SaveAs(Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName);
                Layer(@"layer.alert('上传成功！', {
                                      icon: 6,title: false,
                                      skin: 'layer-ext-moon' 
                                    })", Page);
            }
        }

        protected void Button8_Click(object sender, EventArgs e) {
            string sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source=" + Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName +
            ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(sConnectionString);
                   
            if (FileUpload1.HasFile) {
                try {
                    Directory.CreateDirectory(Server.MapPath("~/UpdateExcel/"));
                    FileUpload1.SaveAs(Server.MapPath("~/UpdateExcel/") + FileUpload1.FileName);
                    OleDbDataAdapter objAdapter1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", objConn);
                    if (objConn.State == ConnectionState.Closed)
                        objConn.Open();

                    DataSet objDataset1 = new DataSet();
                    objAdapter1.Fill(objDataset1, "XLData");
                    try {
                        GridView1.Dispose();
                        GridView1.DataSource = objDataset1.Tables[0]; //测试代码,用来测试是否能读出EXCEL上面的数据
                        GridView1.DataBind();
                        createbutton.Enabled = true;
                        //CheckText();
                    } catch (Exception ee) {
                        createbutton.Enabled = false;
                        Layer(@"layer.alert('请关闭本页面,删除C:\\DownExcel\\路径中的xls,重新把您的xls复制进去再试,记得格式转化成97-2003哦', {
                              icon: 2,title: false,
                              skin: 'layer-ext-moon' 
                            })", Page);
                    }
                } catch {
                    createbutton.Enabled = false;
                    Layer(@"layer.alert('上传失败!未知错误', {
                              icon: 5,title: false,
                              skin: 'layer-ext-moon' 
                            })", Page);
                }
            } else {
                createbutton.Enabled = false;
                Layer(@"layer.alert('您没有选择文件', {
                              icon: 0,title: false,
                              skin: 'layer-ext-moon' 
                            })", Page);
            }
        }

        public void CreateControl() {
            ///批量创建100个按钮
            ///

            if (ViewState["CreateControl"] == null) return; //第一次的时候应该不要创建这些控件
            int count = GridView1.HeaderRow.Cells.Count;
                TableRow row = new TableRow();
                for (int y = 0; y < count; y++) {
                    TableCell cell = new TableCell();

                    CheckBox bt = new CheckBox();
                    bt.Text = GridView1.HeaderRow.Cells[y].Text;
                  
                    bt.CheckedChanged += new EventHandler(bt_Click);
                    cell.Controls.Add(bt);

                    row.Cells.Add(cell);
                }
                HolderTable.Rows.Add(row);
        }
        public void bt_Click(object sender, EventArgs e) {
            Trace.Write("控件动态事件");
            //((Button)sender).BackColor = System.Drawing.Color.Red;
            //Response.Write(string.Format("你点击了该按钮：{0}", ((Button)sender).Text));
        }
        protected override void OnLoad(EventArgs e) {
            CreateControl();
        }

        protected void createbutton_Click(object sender, EventArgs e) {
            if (ViewState["CreateControl"] == null) {
                ViewState["CreateControl"] = true;
                CreateControl();
            }
        }

        

    }
}