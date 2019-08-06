using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Web.Admin.Class;
using Web.Admin.model;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Parsing;
using Web.Common;


namespace Web.Admin {
    public partial class index : System.Web.UI.Page {
        public int yingxiangHang = 0;
        public int yingxiangNum = 0;
        static List<string> list = new List<string>();//修改表操作的列名的集合数组
        public static string currentTable = "";//当前加载了哪张表
        static int addCloumns = 2;
        static string sqlQ = "";
        static int pageNum = 10;
        static DataSet dsQ = new DataSet();
        public static List<int> list_int = new List<int>();//将选中的checkboxID赋值给list
        static DataTable dt_CkbQ = new DataTable();
        static int o = 0;
        public static Int32 tb_count = 0;
        public static int state,export,add,delete,update ;
        public user u = new user();
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Write(@"<script type='text/javascript' src='../js/jquery-1.8.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>
                    layer.alert('当前页面已失效,请重新登录', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})
                    </script>");
                Response.Write("<script>window.close();</script>");
                Response.Redirect("/Admin/login.aspx");
            } else {
                ddl_cloumnInit();
                ddl_manysInit();
                if (!IsPostBack) {
                    zsgc_img.Attributes.Add("style", "display:none");
                    type2.Attributes.Add("style", "display:none");
                    type3.Attributes.Add("style", "display:none");
                    type4.Attributes.Add("style", "display:none");
                    type5.Attributes.Add("style", "display:none");
                    GridView1.Font.Size = 12;
                    GridView1.HeaderStyle.Wrap = false;
                    chuxian_panduan.Visible = false;
                    GridView1.PagerSettings.Mode = PagerButtons.NumericFirstLast;
                    GridView1.PagerSettings.PageButtonCount = 4;
                    // 页数居中显示
                    GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
                    try {
                        if (o == 0) {
                            dt_CkbQ.Columns.Add("id");
                            dt_CkbQ.Columns.Add("page");
                            dt_CkbQ.Columns.Add("rowNum");
                            dt_CkbQ.Columns.Add("ckd");
                            dt_CkbQ.PrimaryKey = new DataColumn[] { dt_CkbQ.Columns["id"] };
                        }
                        o++;
                    } catch { }
                    //加载video
                    currentTable = "Video";
                    BindData(currentTable, pageNum);
                    table_ddl.SelectedIndex = 0;
                    u = (user)Session["Adminu"];
                    state = u.state;
                    export = u.export;
                    add = u.add;
                    delete = u.delete;
                    update = u.update;
                }
            }
        }
        private void ddl_manysInit() {
            List<string> manyselect = new List<string>();
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS("select distinct Source from videourl");//信道列表
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                ddl_manys_xdm.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            } ds.Dispose();
        }
        Dictionary<string, string> video_columns = new Dictionary<string, string>();
        Dictionary<string, string> videou_columns = new Dictionary<string, string>();
        Dictionary<string, string> videot_columns = new Dictionary<string, string>();
        private void ddl_cloumnInit() {

            video_columns.Add("Id", "id");
            video_columns.Add("Tid", "对应类型表id");
            video_columns.Add("Title", "标题");
            video_columns.Add("Cover", "图片路径");
            video_columns.Add("Describe", "描述");
            video_columns.Add("CreateDate", "建立时间");
            video_columns.Add("Sort", "优先级");
            video_columns.Add("State", "状态");
            video_columns.Add("Definition", "清晰度");
            video_columns.Add("Advertisement", "是否有广告");

            videou_columns.Add("Id", "id");
            videou_columns.Add("Vid", "对应名称表id");
            videou_columns.Add("Source", "信道");
            videou_columns.Add("Path", "路径");
            videou_columns.Add("TempPath", "解析路径");
            videou_columns.Add("State", "状态");
            videou_columns.Add("Sort", "优先级");
            videou_columns.Add("CreateDate", "建立时间");
            videou_columns.Add("Definition", "清晰度");
            videou_columns.Add("Advertisement", "是否有广告");
            videou_columns.Add("Describe", "原标题");

            videot_columns.Add("Id", "id");
            videot_columns.Add("Tid", "对应类型表id");
            videot_columns.Add("Title", "标题");
            videot_columns.Add("Cover", "图片路径");
            videot_columns.Add("Sort", "优先级");
            videot_columns.Add("State", "状态");
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="select_table">当前表</param>
        /// <param name="pageNum">当前显示多少页</param>
        /// <param name="ds">数据源</param>
        private void BindData(string select_table, int pageNum) {
            GridView1.PageSize = pageNum;
            String sql = "";
            switch (currentTable) {
                case "Video": sql = "select Id ,Tid as 类型,Title as 标题,State as 状态 from " + select_table; break;
                case "VideoUrl": sql = "select Id,Vid,Source,Path,State,Sort,CreateDate,describe from " + select_table; break;
                case "VideoType": sql = "select * from " + select_table; break;
                case "Table_Test": sql = "select * from Table_Test"; break;

            }
            //获取列名集合


            DataTable dt = new DataTable();
            DataSet dss = new DataSet();
            dss = SqlFunction.Sql_DataAdapterToDS("select name from syscolumns where id=object_id('" + currentTable + "')");
            dt = dss.Tables[0];
            list.Clear();
            for (int i = 0; i < dt.Rows.Count; i++) {
                list.Add(dt.Rows[i][0].ToString());
            }
            dsQ.Tables.Clear();
            dsQ = SqlFunction.Sql_DataAdapterToDS(sql);
            GridView1.DataSource = dsQ.Tables[0];
            GridView1.DataKeyNames = new string[] { "Id" };
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView1.DataBind();
            zsgc_img.Attributes.Add("style", "display:");
            columns_ddl.Items.Clear();
            Select1.Items.Clear();
            DropDownList1.Items.Clear();

            for (int i = 0; i < list.Count; i++) {
                //columns_ddl.Items.Add(list[i]);
                switch (currentTable) {
                    case "Video": columns_ddl.Items.Add(new ListItem(video_columns[list[i]], list[i]));
                        Label4.Text = "名称表";
                        Select1.Items.Add(new ListItem(video_columns[list[i]], list[i]));
                        break;
                    case "VideoType": columns_ddl.Items.Add(new ListItem(videot_columns[list[i]], list[i]));
                        Label4.Text = "类型表"; Select1.Items.Add(new ListItem(videot_columns[list[i]], list[i]));
                        break;
                    case "VideoUrl": columns_ddl.Items.Add(new ListItem(videou_columns[list[i]], list[i]));
                        Label4.Text = "信道表"; Select1.Items.Add(new ListItem(videou_columns[list[i]], list[i]));
                        break;
                }

                //columns_ddl.it.Add(video_columns[list[i]]);
            }
            for (int i = 1; i < list.Count; i++) {
                switch (currentTable) {
                    case "Video": DropDownList1.Items.Add(new ListItem(video_columns[list[i]], list[i])); break;
                    case "VideoType": DropDownList1.Items.Add(new ListItem(videot_columns[list[i]], list[i])); break;
                    case "VideoUrl": DropDownList1.Items.Add(new ListItem(videou_columns[list[i]], list[i])); break;
                }
            }
            list_int.Clear();
            string isint2 = "";
            isint2 = SqlFunction.Sql_ReturnNumberES("select data_type from information_schema.columns where table_name='" + currentTable + "' and COLUMN_NAME='" + list[0] + "'");
            if (isint2 == "int") {//判断SQL表中字段是否为int类型
                chuxian_panduan.Visible = true;
                TextBox1.Width = 54;
            } else {
                chuxian_panduan.Visible = false;
                TextBox1.Width = 100;
            }
        }
        /// <summary>
        /// 点击gridview自带的编辑发生的编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e) {
            GridView1.EditIndex = e.NewEditIndex;/*编辑操作，利用e.NewEditIndex获取当前编辑行索引*/
            //BindData(currentTable, pageNum,dsQ);/*再次绑定显示编辑行的原数据,不进行绑定要点2次编辑才能跳到编辑状态*/
            GridView1.DataSource = dsQ;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView1.DataBind();
        }
        /// <summary>
        /// 当gridview行绑定发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);/*获取主键，需要设置 DataKeyNames，这里设为 id */

            string sql_1 = "update " + currentTable + " set ";
            string sql_2 = " where " + list[0] + "='" + id + "'";
            string sql_3 = "";
            string sql_3_1 = "";
            yingxiangNum = 0;
            string changBefore = "";
            //获取被影响的个数
            switch (currentTable) {
                case "Video":
                    for (int i = 3; i < 9; i++) {
                        changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i - addCloumns] + " from " + currentTable + " where " + list[0] + "='" + id + "'");
                        sql_3 += list[i - addCloumns] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                        if (changBefore != (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                            yingxiangNum++;
                        }
                    }
                    changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[7] + " from " + currentTable + " where " + list[0] + "='" + id + "'");
                    sql_3_1 = list[7] + "='" + (GridView1.Rows[e.RowIndex].Cells[9].Controls[0] as CheckBox).Checked.ToString() + "'";
                    if (changBefore != (GridView1.Rows[e.RowIndex].Cells[9].Controls[0] as CheckBox).Checked.ToString()) {
                        yingxiangNum++;
                    }
                    sqlQ = sql_1 + sql_3 + sql_3_1 + sql_2;
                    break;
                case "VideoType":
                    for (int i = 3; i < list.Count + addCloumns; i++) {

                        changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i - addCloumns] + " from " + currentTable + " where " + list[0] + "='" + id + "'");
                        if (i == list.Count + addCloumns - 1)
                            sql_3 += list[i - addCloumns] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "'";
                        else
                            sql_3 += list[i - addCloumns] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                        if (changBefore != (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                            yingxiangNum++;
                        }
                    }
                    sqlQ = sql_1 + sql_3 + sql_2;
                    break;
                case "VideoUrl":
                    for (int i = 3; i < list.Count + 1; i++) {
                        if (i >= 6)
                            changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i - 1] + " from " + currentTable + " where " + list[0] + "='" + id + "'");
                        else
                            changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i - addCloumns] + " from " + currentTable + " where " + list[0] + "='" + id + "'");
                        if (changBefore != (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                            yingxiangNum++;
                        }
                        if (i == list.Count)
                            sql_3 += list[i - 1] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "'";
                        else {
                            if (i >= 6)
                                sql_3 += list[i - 1] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";

                            else
                                sql_3 += list[i - addCloumns] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                        }
                    }
                    sqlQ = sql_1 + sql_3 + sql_2;
                    break;
            }
            if (yingxiangNum > 0)
                yingxiangHang = 1;

            if (SqlFunction.Sql_ReturnNumberENQ(sqlQ) > 0) {
                GridView1.EditIndex = -1;
                BindData(currentTable, pageNum);
                Layer(string.Format(@"layer.alert('修改成功,影响{0}行,共{1}个值', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            }})", yingxiangHang, yingxiangNum), Page);
            } else {
                GridView1.EditIndex = -1;
                BindData(currentTable, pageNum);
                LayerA("失败,未修改",5,Page);
            }
        }
        /// <summary>
        /// 点击取消编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            GridView1.EditIndex = -1;                 /*编辑索引赋值为-1，变回正常显示状态*/
            GridView1.DataSource = dsQ;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView1.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e) {
            currentTable = "Video";
            BindData(currentTable, pageNum);
            table_ddl.SelectedIndex = 0;
        }
        protected void Button2_Click(object sender, EventArgs e) {
            currentTable = "VideoUrl";
            BindData(currentTable, pageNum);
            table_ddl.SelectedIndex = 1;
        }
        protected void Button3_Click(object sender, EventArgs e) {
            currentTable = "VideoType";
            BindData(currentTable, pageNum);
            table_ddl.SelectedIndex = 2;
        }
        int seq = 0;
        /// <summary>
        /// gridview创建行发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e) {
            // 添加标题
            if (e.Row.RowType == DataControlRowType.Header) {
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                Literal lit = new Literal();
                lit.Text = @"<td colspan='12' align='center'><h3>" + currentTable + "表</h3></td>";
                TableHeaderCell thc = new TableHeaderCell();
                thc.Controls.Add(lit);
                gvr.Cells.Add(thc);
                GridView1.Controls[0].Controls.AddAt(0, gvr);
                
            }
            e.Row.Cells[0].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word;");
        }
        static List<int> list_pageID = new List<int>();
        static List<bool> list_ck_selected = new List<bool>();
        /// <summary>
        /// 当点击更换页面前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            if (e.NewPageIndex < 0) {
                GridView1.PageIndex = 0;
            } else {
                GridView1.PageIndex = e.NewPageIndex;
            }
        }
        /// <summary>
        /// 当点击更换页面后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanged(object sender, EventArgs e) {
            GridView1.DataSource = dsQ;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView1.DataBind();
        }
        int eq = 0;
        /// <summary>
        /// 当gridview绑定成功一行数据发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
            //首先判断是否是数据行  
            if (e.Row.RowType == DataControlRowType.DataRow) {
                //当鼠标停留时更改背景色  
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='rgba(0,0,0,0.2)'");
                //当鼠标移开时还原背景色  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //if (e.Row.RowIndex % 2 == 0) {
                //    e.Row.BackColor = System.Drawing.Color.FromArgb(0, 255, 255, 255);
                //} else {
                //    e.Row.BackColor = System.Drawing.Color.FromArgb(0xD2D2D2);
                //}
                int row_index = e.Row.RowIndex+2;
                e.Row.Attributes.Add("onclick", "getId(" + row_index + ");");
            }
        }
        /// <summary>
        /// 当下拉框更换表发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void table_ddl_SelectedIndexChanged(object sender, EventArgs e) {
            switch (table_ddl.SelectedValue) {
                case "Video": currentTable = "Video"; break;
                case "VideoUrl": currentTable = "VideoUrl"; break;
                case "VideoType":currentTable = "VideoType";break;
            }
            BindData(currentTable, pageNum);
            columns_ddl.Items.Clear();
            Select1.Items.Clear();
            DropDownList1.Items.Clear();
            for (int i = 0; i < list.Count; i++) {
                //columns_ddl.Items.Add(list[i]);
                switch (currentTable) {
                    case "Video": columns_ddl.Items.Add(new ListItem(video_columns[list[i]], list[i]));
                        Label4.Text = "名称表"; Select1.Items.Add(new ListItem(video_columns[list[i]], list[i]));
                        break;
                    case "VideoType": columns_ddl.Items.Add(new ListItem(videot_columns[list[i]], list[i]));
                        Label4.Text = "类型表"; Select1.Items.Add(new ListItem(videot_columns[list[i]], list[i]));
                        break;
                    case "VideoUrl": columns_ddl.Items.Add(new ListItem(videou_columns[list[i]], list[i]));
                        Label4.Text = "信道表"; Select1.Items.Add(new ListItem(videou_columns[list[i]], list[i]));
                        break;
                }
            };
            for (int i = 1; i < list.Count; i++) {
                switch (currentTable) {
                    case "Video": DropDownList1.Items.Add(new ListItem(video_columns[list[i]], list[i])); break;
                    case "VideoType": DropDownList1.Items.Add(new ListItem(videot_columns[list[i]], list[i])); break;
                    case "VideoUrl": DropDownList1.Items.Add(new ListItem(videou_columns[list[i]], list[i])); break;
                }
            }
            string isint4 = "";
            isint4 = SqlFunction.Sql_ReturnNumberES("select data_type from information_schema.columns where table_name='" + currentTable + "' and COLUMN_NAME='" + list[0] + "'");
            if (isint4 == "int") {
                chuxian_panduan.Visible = true;
                TextBox1.Width = 54;
            } else {
                chuxian_panduan.Visible = false;
                TextBox1.Width = 100;
            }
        }

        /// <summary>
        /// 点击锁定查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button4_Click(object sender, EventArgs e) {
            string sql = "";
            string sql_2 = "";
            string isint1 = "";
            isint1 = SqlFunction.Sql_ReturnNumberES("select data_type from information_schema.columns where table_name='" + currentTable + "' and COLUMN_NAME='" + columns_ddl.SelectedValue + "'");
            if (isint1 == "int") {
                //填写一个,或填写全部
                if (TextBox1.Text == "") {//没填写了A.填写了B
                    sql_2 = " where " + columns_ddl.Text + "<=" + TextBox3.Text;
                    sql = "select count(1) from " + table_ddl.SelectedValue + sql_2;
                } else {
                    if (TextBox3.Text == "") {
                        sql_2 = " where " + columns_ddl.Text + ">=" + TextBox1.Text;
                        sql = "select count(1) from " + table_ddl.SelectedValue + sql_2;
                    }//填写了A,没填写B
                    else { //全部填写
                        sql_2 = " where " + columns_ddl.Text + ">=" + TextBox1.Text + " and " + columns_ddl.Text + "<=" + TextBox3.Text;
                        sql = "select count(1) from " + table_ddl.SelectedValue + sql_2;
                    }
                }
                if (Convert.ToInt32(SqlFunction.Sql_ReturnNumberES(sql)) > 0) {
                    string sqll = "";
                    switch (table_ddl.SelectedValue) {
                        case "Video": sqll = "select Id ,Tid as 类型,Title as 标题,State as 状态 from " + table_ddl.SelectedValue + sql_2 + " order by id"; break;
                        case "VideoUrl": sqll = "select Id,Vid,Source,Path,State,Sort,CreateDate from " + table_ddl.SelectedValue + sql_2 + " order by id"; break;
                        case "VideoType": sqll = "select * from " + table_ddl.SelectedValue + sql_2 + " order by id"; break;
                    }
                    dsQ = SqlFunction.DataBind(sqll, pageNum, GridView1);
                } else {
                    Layer(@"layer.alert('没有查询到数据', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                }
            } else {
                sql = "select count(1) from " + table_ddl.SelectedValue + " where " + columns_ddl.Text + " like'%" + TextBox1.Text + "%'";
                int i = 0;
                i = Convert.ToInt32(SqlFunction.Sql_ReturnNumberES(sql));
                if (i > 0) {
                    string sqll = "";
                    switch (table_ddl.SelectedValue) {
                        case "Video": sqll = "select Id ,Tid as 类型,Title as 标题,State as 状态 from " + table_ddl.SelectedValue + " where " + columns_ddl.Text + " like'%" + TextBox1.Text + "%' order by id"; break;
                        case "VideoUrl": sqll = "select Id,Vid,Source,Path,State,Sort,CreateDate from " + table_ddl.SelectedValue + " where " + columns_ddl.Text + " like'%" + TextBox1.Text + "%' order by id"; break;
                        case "VideoType": sqll = "select * from " + table_ddl.SelectedValue + " where " + columns_ddl.Text + " like'%" + TextBox1.Text + "%' order by id"; break;
                    }
                    dsQ = SqlFunction.DataBind(sqll, pageNum, GridView1);
                } else {
                    Layer(@"layer.alert('没有查询到数据', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            })", Page);
                }
                this.ClientScript.RegisterStartupScript(this.GetType(), "", "closeloding()", true);
            }
        }
        
        /// <summary>
        /// gridview绑定数据完毕发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_DataBound(object sender, EventArgs e) {
            if (GridView1.Rows.Count > 0) {
                DropDownList listt = (DropDownList)GridView1.BottomPagerRow.FindControl("listPageCount");
                for (int i = 1; i <= GridView1.PageCount; i++) {
                    ListItem item = new ListItem(i.ToString());
                    if (i == GridView1.PageIndex + 1) {
                        item.Selected = true;
                    }
                    listt.Items.Add(item);
                }
                DropDownList dlist = (DropDownList)GridView1.BottomPagerRow.FindControl("pageNum");
                dlist.SelectedValue = pageNum.ToString();
            }


        }
        /// <summary>
        /// 点击模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void select_btn_Click(object sender, EventArgs e) {
                string sqll = "";
                string sql = "select count(1) from " + table_ddl.SelectedValue + " where " + list[0] + " like'%" + TextBox2.Value + "%'";
                switch (table_ddl.SelectedValue) {
                    case "Video":
                        sqll = "select Id ,Tid as 类型,Title as 标题,State as 状态 from " + table_ddl.SelectedValue + " where " + list[0] + " like'%" + TextBox2.Value + "%'";

                        for (int i = 1; i < list.Count; i++) {
                            sqll += " union select  Id ,Tid as 类型,Title as 标题,State as 状态 from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                            sql += " union select count(1) from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                        } sqll += " order by id"; break;
                    case "VideoUrl":
                        sqll = "select Id,Vid,Source,Path,State,Sort,CreateDate from " + table_ddl.SelectedValue + " where " + list[0] + " like'%" + TextBox2.Value + "%'";

                        for (int i = 1; i < list.Count; i++) {
                            if (i != 4)
                                sqll += " union select Id,Vid,Source,Path,State,Sort,CreateDate from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                            sql += " union select count(1) from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                        } sqll += " order by id"; break;
                    case "VideoType":
                        sqll = "select * from " + table_ddl.SelectedValue + " where " + list[0] + " like'%" + TextBox2.Value + "%'";
                        for (int i = 1; i < list.Count; i++) {
                            sqll += " union select * from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                            sql += " union select count(1) from " + table_ddl.SelectedValue + " where " + list[i] + " like'%" + TextBox2.Value + "%'";
                        } sqll += " order by id";
                        break;
                }
                //判断是否查询到记录，查到几条
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(sql);
                dt = ds.Tables[0];
                int num = 0;
                for (int i = 0; i < dt.Rows.Count; i++) {
                    num += Convert.ToInt32(dt.Rows[i][0]);
                }
                if (num > 0) {
                    dsQ.Clear();
                    dsQ = SqlFunction.Sql_DataAdapterToDS(sqll);
                    GridView1.DataSource = dsQ;
                    GridView1.PageIndex = 0;
                    DataTable dtt = new DataTable();
                    dtt = dsQ.Tables[0];
                    GridView1.DataBind();
                    
                } else {
                    Layer(@"layer.alert('没有查询到数据', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            })", Page);
                }
                this.ClientScript.RegisterStartupScript(this.GetType(), "", "closeloding()", true);
        }
        /// <summary>
        /// 当改变当前显示行数发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PageNum_SelectedIndexChanged(object sender, EventArgs e) {
            DropDownList list = (DropDownList)GridView1.BottomPagerRow.FindControl("pageNum");
            pageNum = Convert.ToInt32(list.SelectedValue);
            GridView1.DataSource = dsQ;
            GridView1.PageSize = pageNum;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView1.DataBind();
        }
        /// <summary>
        /// 跳转页面发生事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listPageCount_SelectedIndexChanged(object sender, EventArgs e) {
            DropDownList list = (DropDownList)GridView1.BottomPagerRow.FindControl("listPageCount");
            GridViewPageEventArgs arg = new GridViewPageEventArgs(list.SelectedIndex);
            GridView1_PageIndexChanging(null, arg);
            GridView1_PageIndexChanged(null, null);
        }
        /// <summary>
        /// 锁定查询,根据列名查询,当绑定列名的下拉框发生改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void columns_ddl_SelectedIndexChanged(object sender, EventArgs e) {
            string isint = "";
            isint = SqlFunction.Sql_ReturnNumberES("select data_type from information_schema.columns where table_name='" + currentTable + "' and COLUMN_NAME='" + columns_ddl.SelectedValue + "'");
            if (isint == "int") {
                chuxian_panduan.Visible = true;
                TextBox1.Width = 54;
            } else {
                chuxian_panduan.Visible = false;
                TextBox1.Width = 100;
            }
        }
        /// <summary>
        /// 点击修改,获取当前勾选了的复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e) {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "",
            //    "<script>openWindow('updateVideo.aspx?tbName=" + Server.UrlEncode(currentTable) + "',800,550);</script>");

            for (int i = 0; i < list_int.Count; i++)  //外循环是循环的次数
            {
                for (int j = list_int.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {
                    if (list_int[i] == list_int[j]) {
                        list_int.RemoveAt(j);
                    }
                }
            }
            if (list_int.Count == 0) {
                Layer("layer.alert('您未选中数据,修改请勾选复选框');", Page);
            } else {
                Label7.Text = "您当前选中" + list_int.Count + "条数据";
                Label8.Text = "列名:";
                Layer("layer.open({type: 1,area: ['500px', '320px'],shadeClose: true,content: $('#updata_PL')});", Page);
            }
        }
        static int ck_y_num = 0;
        /// <summary>
        /// 当复选框状态发生改变,记录当前ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbItem_CheckedChanged(object sender, EventArgs e) {
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++) {
                CheckBox ck = (CheckBox)GridView1.Rows[i].FindControl("cbItem");
                if (ck.Checked == true) {
                    int id = Convert.ToInt32(GridView1.DataKeys[i].Value);
                    list_int.Add(id);
                    ck_y_num++;
                } else {
                    int id = Convert.ToInt32(GridView1.DataKeys[i].Value);
                    list_int.Remove(id);
                }
            }
        }
        /// <summary>
        /// 点击确定修改数据按钮,执行修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button8_Click(object sender, EventArgs e) {

        }
        /// <summary>
        /// 点击取消修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button9_Click(object sender, EventArgs e) {
        }
        /// <summary>
        /// 点击删除图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e) {
            //CheckBox cba = (CheckBox)GridView1.HeaderRow.FindControl("cbAll");
            //CheckBox cbp = (CheckBox)GridView1.HeaderRow.FindControl("cbPage");
            //if (cba.Checked) {//全部选中

            //} else {
            //    if (cbp.Checked) {//选中当前页

            //    } else {//选中个别数据

        }
        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button10_Click(object sender, EventArgs e) {

        }
        /// <summary>
        /// 查询数据图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton4_Click(object sender, ImageClickEventArgs e) {

        }
        /// <summary>
        /// 增加数据图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton3_Click(object sender, ImageClickEventArgs e) {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "",
                "<script>openWindow('addVideo.aspx?tbName=" + Server.UrlEncode(currentTable) + "',1000,550);</script>");
        }
        private user uadmin = new user();
//        protected bool AccountRights(int i) {
//            uadmin = (user)Session["Adminu"];
//            if (uadmin.state == 1) {//账号正常
//                if (i == 1) {//有权限
//                    return true;
//                } else {//无权限
//                    Layer(@"layer.alert('实在抱歉,您的账号没有权限', {
//                              icon: 5,
//                              shadeClose: true,skin: 'layer-ext-moon' 
//                            })", Page);
//                    return false;
//                }
//            } else {//不正常
//                Layer(@"layer.alert('实在抱歉,您的账号处于异常阶段', {
//                              icon: 5,
//                              shadeClose: true,skin: 'layer-ext-moon' 
//                            })", Page);
//                return false;
//            }
//        }
        
        protected void Button5_Click(object sender, EventArgs e) {
                DateTime dt = DateTime.Now;
                //string idcount = "";
                ////int count = dsQ.Tables[0].Rows.Count;
                ////for (int i = 0; i < count; i++) {
                ////    idcount += (dsQ.Tables[0].Rows[i][0].ToString() + ",");
                ////}
                //idcount = idcount.Substring(0, idcount.Length - 1);
                u = (user)Session["Adminu"];
                string ip = HttpContext.Current.Request.UserHostAddress;
                Buckup.Buckup_DownloadExcel(u.name,ip, "当前格式");
                if(ToExcel.CreateExcel(dsQ, "当前格式", 1, currentTable, null, null))
                    LayerA("当前时间段没有信息,无法导出", 5, Page);
        }

        protected void Button6_Click(object sender, EventArgs e) {

        }
        static public string currentType = "老人专区,幼儿专区";
        protected void type1_SelectedIndexChanged(object sender, EventArgs e) {
            if (type1.Text == "全部") {
                type2.Items.Clear();
                type3.Items.Clear();
                type4.Items.Clear();
                type5.Items.Clear();
                type2.Attributes.Add("style", "display:none");
                type3.Attributes.Add("style", "display:none");
                type4.Attributes.Add("style", "display:none");
                type5.Attributes.Add("style", "display:none");
                string data = "老人专区','幼儿专区";
                currentType = "老人专区','幼儿专区";
                type_select(data);
            } else {
                currentType = type1.Text;
                if (add_dropChang.bingdDdl1(type2, type1.Text)) {
                    type2.Attributes.Add("style", "display:");
                } else {
                    type2.Attributes.Add("style", "display:none");
                }
                type_select(type1.Text);
            }
        }

        protected void type2_SelectedIndexChanged(object sender, EventArgs e) {
            if (type2.Text == "全部") {
                type3.Items.Clear();
                type4.Items.Clear();
                type5.Items.Clear();
                type3.Attributes.Add("style", "display:none");
                type4.Attributes.Add("style", "display:none");
                type5.Attributes.Add("style", "display:none");
                currentType = type1.Text;
                type_select(type1.Text);
            } else {
                if (add_dropChang.bingdDdl1(type3, type2.Text)) {
                    type3.Attributes.Add("style", "display:");

                } else
                    type3.Attributes.Add("style", "display:none");
                currentType = type2.Text;
                type_select(type2.Text);

            }
        }

        protected void type3_SelectedIndexChanged(object sender, EventArgs e) {
            if (type3.Text == "全部") {
                type4.Items.Clear();
                type5.Items.Clear();
                type4.Attributes.Add("style", "display:none");
                type5.Attributes.Add("style", "display:none");
                currentType = type2.Text;
                type_select(type2.Text);
            } else {
                if (add_dropChang.bingdDdl1(type4, type3.Text)) {
                    type4.Attributes.Add("style", "display:");

                } else
                    type4.Attributes.Add("style", "display:none");
                currentType = type3.Text;
                type_select(type3.Text);
            }
        }

        protected void type4_SelectedIndexChanged(object sender, EventArgs e) {
            if (type4.Text == "全部") {
                type5.Items.Clear();
                type5.Attributes.Add("style", "display:none");
                currentType = type3.Text;
                type_select(type3.Text);
            } else {
                if (add_dropChang.bingdDdl1(type5, type4.Text)) {
                    type5.Attributes.Add("style", "display:");
                } else {
                    type5.Attributes.Add("style", "display:none");
                }
                currentType = type4.Text;
                type_select(type4.Text);
            }
        }
        public void type_select(string a) {
            string sql = string.Format(@"select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in 
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select id from VideoType where tid in(select id from VideoType where Title in('{0}')))))))
                                 union 
                                 select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select id from VideoType where tid in(select id from VideoType where Title in('{0}'))))))
                                  union 
                                 select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in
                                (select Id from VideoType where tid in
                                (select Id from VideoType where tid in
                                (select id from VideoType where tid in (select id from VideoType where Title in('{0}')))))
                                 union 
                                 select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in
                                (select Id from VideoType where tid in
                                (select id from VideoType where tid in (select id from VideoType where Title in('{0}'))))
                                 union 
                                 select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in
                                (select id from VideoType where tid in (select id from VideoType where Title in('{0}')))
                                 union 
                                select Id ,Tid as 类型,Title as 标题,State as 状态 from Video where tid in (select id from VideoType where Title in('{0}'))order by id", a);
            dsQ = SqlFunction.DataBind(sql, pageNum, GridView1);

        }

        protected void Button10_Click1(object sender, EventArgs e) {

        }

        protected void ImageButton5_Click(object sender, ImageClickEventArgs e) {

        }

        protected void ImageButton6_Click(object sender, ImageClickEventArgs e) {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e) {
            string dex = "";
            switch (currentTable) {
                case "Video": dex = GridView1.SelectedRow.Cells[2].Text; break;
                case "VideoUrl": dex = GridView1.SelectedRow.Cells[3].Text; break;
                case "VideoType": dex = GridView1.SelectedRow.Cells[2].Text+"?"; break;
            }
            this.ClientScript.RegisterStartupScript(this.GetType(), "", "gofullinfo('" + dex + "')", true);
        }
        public static void Alert(string info, Page p) {
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {

        }


        protected void Button12_Click(object sender, EventArgs e) {

            bool[] cblist;
            cblist = new bool[30];
            int i = 0;
            for (int j = 0; i < 9 && i < 30; j++, i++) {
                cblist[i] = CheckBoxList4.Items[j].Selected;
            }
            for (int j = 0; j < 7 && i < 30; j++, i++) {
                cblist[i] = CheckBoxList1.Items[j].Selected;
            }
            for (int j = 0; j < 7 && i < 30; j++, i++) {
                cblist[i] = CheckBoxList2.Items[j].Selected;
            }
            for (int j = 0; j < 7 && i < 30; j++, i++) {
                cblist[i] = CheckBoxList3.Items[j].Selected;
            }
            DateTime dt = DateTime.Now;
            u = (user)Session["Adminu"];
            string ip = HttpContext.Current.Request.UserHostAddress;
            Buckup.Buckup_DownloadExcel(u.name,ip, "详细格式");
            if (ToExcel.CreateExcel(null, "详细格式", 2, currentType, cblist, ""))
                LayerA("当前时间段没有信息,无法导出", 5, Page);
        }
        public static void LayerA(string info, int icon, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>layer.alert('";
            script = script + info + "!', {title: false,icon:" + icon + ",skin: 'layer-ext-moon' })" + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        protected void Button11_Click(object sender, EventArgs e) {
            if (TextBox5.Text == "jlf@2") {
//                 Layer(@"layer.open({
//                    type: 2,
//                    title: '很多时候，我们想最大化看，比如像这个页面。',
//                    shadeClose: true,
//                    shade: false,
//                    maxmin: true, //开启最大化最小化按钮
//                    area: ['1000px', '600px'],
//                    content: 'Update/updateVideo.aspx'
//                });",Page);
            } else {
                //Layer("layer.msg('密码错误',function(){});",Page);
            }
        }

        protected void Button9_Click1(object sender, EventArgs e) {

                string s_utj = s_date_utj.Value;
                string e_utj = e_date_utj.Value;
                string sql = "";
                if (s_utj == null)
                    s_utj = "";
                if (e_utj == null)
                    e_utj = "";
                sql = " where clickTime>='" + s_utj + "' and clickTime<='" + e_utj + "'";
                if (sql == " where clickTime>='' and clickTime<=''")
                    sql = "";
                u = (user)Session["Adminu"];
                string ip = HttpContext.Current.Request.UserHostAddress;
                Buckup.Buckup_DownloadExcel(u.name,ip, "用户使用统计");
                if (ToExcel.CreateExcel(null, "用户使用统计", 3, "", null, sql))
                    LayerA("当前时间段没有信息,无法导出", 5, Page);
        }

        protected void zj_selectmh_Click(object sender, EventArgs e) {

        }

        protected void zj_selectsd_Click(object sender, EventArgs e) {

        }

        protected void user_ms_btn_Click(object sender, EventArgs e) {
            switch (select_table.Value) {
                case "信道表": ; break;
                case "视频表": ; break;
                case "类型表": ; break;
            }
        }

        protected void btn_update_Click(object sender, EventArgs e) {

            int successNum = 0;
            int lostNum = 0;
            string[] a;
            string txt9 = "";
            txt9 = TextBox9.Text.Trim();
            txt9 = txt9.Substring(0, txt9.Length - 1);
            a = txt9.Split(',');

            List<string> bpupdate = new List<string>();
            for (int i = 0; i < a.Length; i++) {
                string sql_bpdele = "select " + DropDownList1.Text + " from " + currentTable + " where ID=" + a[i];
                DataSet dss = new DataSet();
                dss = SqlFunction.Sql_DataAdapterToDS(sql_bpdele);
                bpupdate.Clear();
                bpupdate.Add(dss.Tables[0].Rows[0][0].ToString());
                DateTime changTime = DateTime.Now;
                u = (user)Session["Adminu"];
                string ip = HttpContext.Current.Request.UserHostAddress;
                if (!Buckup.UpdateBackup(currentTable, DropDownList1.Text, Convert.ToInt32(a[i]), bpupdate[0],
                    TextBox4.Text, changTime, u.name,ip)) {
                    Layer(@"layer.alert('当前备份失败,但是不影响修改操作', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            })", Page);
                }
                string sql = "update " + currentTable + " set " + DropDownList1.SelectedValue + "='" + TextBox4.Text +
                "' where id=" + a[i];
                //Response.Write(" "+sql);
                 if (SqlFunction.Sql_UpdatePL(sql) != 0)
                    successNum++;
                else
                    lostNum++;
            }
            list_int.Clear();
            Layer(string.Format(@"layer.alert('修改成功,影响到: {0}个数据,失败个数:{1}', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            }})", successNum, lostNum), Page);
            BindData(currentTable, pageNum);
        }

        protected void btn_delete_Click(object sender, EventArgs e) {
            string idjhe = "";
            idjhe = TextBox10.Text.Trim();
            idjhe = idjhe.Substring(0, idjhe.Length - 1);
            string[] id = idjhe.Split(',');
            int success = 0, lost = 0;


            List<string> bpdelete = new List<string>();
            u = (user)Session["Adminu"];
            for (int i = 0; i < id.Length; i++) {
                string sql_bpdele = "select * from " + currentTable + " where ID=" + id[i];
                DataSet dss = new DataSet();
                dss = SqlFunction.Sql_DataAdapterToDS(sql_bpdele);
                bpdelete.Clear();
                DateTime changTime = DateTime.Now;
                string ip = HttpContext.Current.Request.UserHostAddress;
                switch (currentTable) {
                    case "Video":
                        for (int j = 0; j < 10; j++) {

                            bpdelete.Add(dss.Tables[0].Rows[0][j].ToString());
                        }
                        Buckup.Buckup_Delete_video(currentTable, changTime, u.name, ip,
                           bpdelete[0], bpdelete[1],
                             bpdelete[2], bpdelete[3],
                             bpdelete[4], bpdelete[5],
                             bpdelete[6], bpdelete[7],
                             bpdelete[8], bpdelete[9]);
                        ; break;
                    case "VideoUrl":
                        for (int j = 0; j < 8; j++) {
                            bpdelete.Add(dss.Tables[0].Rows[0][j].ToString());
                        }
                        Buckup.Buckup_Delete_url(currentTable, changTime, u.name, ip,
                              bpdelete[0], bpdelete[1],
                                bpdelete[2], bpdelete[3],
                                bpdelete[4], bpdelete[5],
                                bpdelete[6], bpdelete[7]);
                        ; break;
                    case "VideoType":
                        for (int j = 0; j < 6; j++) {
                            bpdelete.Add(dss.Tables[0].Rows[0][j].ToString());
                        }
                        Buckup.Buckup_Delete_type(currentTable, changTime, u.name, ip,
                              bpdelete[0], bpdelete[1],
                                bpdelete[2], bpdelete[3],
                                bpdelete[4], bpdelete[5]);
                        ; break;
                }
                string sql = "delete from " + currentTable + " where id=" + id[i];
                if (SqlFunction.Sql_UpdatePL(sql) != 0)
                    success++;
                else
                    lost++;
            }

            Layer(string.Format(@"layer.alert('删除成功,成功删除:{0}个数据,失败个数:{1}', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            }})", success, lost), Page);
            BindData(currentTable, pageNum);


        }
    }
}
