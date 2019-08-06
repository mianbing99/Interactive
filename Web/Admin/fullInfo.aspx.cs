using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;
using System.Text.RegularExpressions;


namespace Web.Admin {
    public partial class fullInfo : System.Web.UI.Page {
        static string id = "";
        static public DataSet dsQ = new DataSet();
        static public DataSet vdsQ = new DataSet();
        static public DataSet tdsQ = new DataSet();
        static List<String> list = new List<String>();//修改表操作的列名的集合数组
        static List<String> vlist = new List<String>();//修改表操作的列名的集合数组
        static List<String> tlist = new List<String>();//修改表操作的列名的集合数组
        string[] idc = new string[3];
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    id = Request.QueryString["index"];
                    idc = id.Split('?');
                    if (idc.Length == 1) {//视频表和路径表转过来的
                        select();
                        selectType();
                        selectXindao();
                    } else { //类型表转过来的
                        gv1.Attributes.Add("style", "display:none");
                        gv2.Attributes.Add("style", "display:none");
                        id = idc[0];
                        selectType();
                    }
                }
            }
        }

        //开始查找数据
        public void select() {
            string sql = "";
            sql = "select id as 编号,tid as 对应类型,Title as 标题,Cover as 标题图片,Describe as 描述,CreateDate as 建立时间,Sort as 优先级,State as 状态,Definition as 清晰度,Advertisement as 是否有广告,Describe as 原标题 from video where id=" + id;
            vdsQ=SqlFunction.DataBind(sql,10,GridView1);
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS("select name from syscolumns where id=object_id('Video')");
            vlist.Clear();
            int dc = ds.Tables[0].Rows.Count;
            for (int i = 0; i < dc; i++) {
                vlist.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }
        //开始查找类型
        public void selectType()  {
           string sql = "";
           string sqlf = "";
           
           DataTable dt = new DataTable();
           dt.Columns.Add("编号", Type.GetType("System.String"));
           dt.Columns.Add("tid", Type.GetType("System.String"));
           dt.Columns.Add("标题", Type.GetType("System.String"));
           dt.Columns.Add("图片", Type.GetType("System.String"));
           dt.Columns.Add("优先级", Type.GetType("System.String"));
           dt.Columns.Add("状态", Type.GetType("System.String"));
           DataSet dss = new DataSet();
           if (idc.Length == 1) {
               sqlf = "select tid from Video where id=" + id;
           } else {
               sqlf = "select tid from Videotype where id=" + id;
               dss = SqlFunction.Sql_DataAdapterToDS("select * from videotype where id="+id);
               if (dss.Tables[0].Rows.Count != 0) {
                   DataRow newRow;
                   newRow = dt.NewRow();
                   newRow["编号"] = dss.Tables[0].Rows[0][0];
                   newRow["tid"] = dss.Tables[0].Rows[0][1];
                   newRow["标题"] = dss.Tables[0].Rows[0][2];
                   newRow["图片"] = dss.Tables[0].Rows[0][3];
                   newRow["优先级"] = dss.Tables[0].Rows[0][4];
                   newRow["状态"] = dss.Tables[0].Rows[0][5];
                   dt.Rows.Add(newRow);
                   dss.Clear(); dss.Dispose();
               }
           }
           string relut = SqlFunction.Sql_ReturnNumberES(sqlf);
           dss = SqlFunction.Sql_DataAdapterToDS("select * from videotype where id=" + relut);
           if (dss.Tables[0].Rows.Count != 0) {
               DataRow newRow;
               newRow = dt.NewRow();
               newRow["编号"] = dss.Tables[0].Rows[0][0];
               newRow["tid"] = dss.Tables[0].Rows[0][1];
               newRow["标题"] = dss.Tables[0].Rows[0][2];
               newRow["图片"] = dss.Tables[0].Rows[0][3];
               newRow["优先级"] = dss.Tables[0].Rows[0][4];
               newRow["状态"] = dss.Tables[0].Rows[0][5];
               dt.Rows.Add(newRow);
               dss.Clear(); dss.Dispose(); 
           }
           for (int i=0; relut != ""; i++) {
               sql = "select tid from videotype where id=" + relut;
               DataSet ds = new DataSet();
               relut = SqlFunction.Sql_ReturnNumberES(sql);
               if (relut == "") {
                   if (dt.Rows.Count > 0) {
                       tdsQ.Tables.Clear(); tdsQ.Tables.Add(dt);
                       //判断是否查询到记录，查到几条
                       DataSet dstt = new DataSet();
                       dstt = SqlFunction.Sql_DataAdapterToDS("select name from syscolumns where id=object_id('VideoType')");
                       tlist.Clear();
                       int dc = dstt.Tables[0].Rows.Count;
                       for (int j = 0; j < dc; j++) {
                           tlist.Add(dstt.Tables[0].Rows[j][0].ToString());
                       }
                       this.GridView3.DataSource = dt;
                       this.GridView3.DataBind();
                   }
                   return;
               } else {
                   ds = SqlFunction.Sql_DataAdapterToDS("select * from videotype where id=" + relut);
                   if (ds.Tables[0].Rows.Count != 0) {
                       DataRow newRow;
                       newRow = dt.NewRow();
                       newRow["编号"] = ds.Tables[0].Rows[0][0];
                       newRow["tid"] = ds.Tables[0].Rows[0][1];
                       newRow["标题"] = ds.Tables[0].Rows[0][2];
                       newRow["图片"] = ds.Tables[0].Rows[0][3];
                       newRow["优先级"] = ds.Tables[0].Rows[0][4];
                       newRow["状态"] = ds.Tables[0].Rows[0][5];
                       dt.Rows.Add(newRow);
                       ds.Clear(); ds.Dispose();
                   }
               }
               this.GridView3.DataSource = dt;
               this.GridView3.DataBind();
           }
           if (dt.Rows.Count > 0) {
               tdsQ.Tables.Clear(); tdsQ.Tables.Add(dt);
               //判断是否查询到记录，查到几条
               DataSet ds = new DataSet();
               ds = SqlFunction.Sql_DataAdapterToDS("select name from syscolumns where id=object_id('VideoType')");
               tlist.Clear();
               int dc = ds.Tables[0].Rows.Count;
               for (int i = 0; i < dc; i++) {
                   tlist.Add(ds.Tables[0].Rows[i][0].ToString());
               }
           }
        }
        //开始查找信道
        public void selectXindao() {
            string sql = "";
            sql = "select id as 编号,vid as 对应名称,source as 来源,path as 路径,state as 状态,sort as 优先级,CreateDate as 建立时间,Definition as 清晰度,Advertisement as 是否有广告,Describe as 原标题 from videoUrl where vid=" + id;
            dsQ.Tables.Clear();
            dsQ= SqlFunction.DataBind(sql, 10, GridView2);
            //判断是否查询到记录，查到几条
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS("select name from syscolumns where id=object_id('VideoUrl')");
            list.Clear();
            int dc = ds.Tables[0].Rows.Count;
            for (int i = 0; i < dc; i++) {
                list.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e) {
            //首先判断是否是数据行  
            if (e.Row.RowType == DataControlRowType.DataRow) {
                //当鼠标停留时更改背景色  
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='rgba(0,0,0,0.1)'");
                //当鼠标移开时还原背景色  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }  
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e) {
            GridView2.DataKeyNames = new string[] { "编号" };
            GridView2.EditIndex = e.NewEditIndex;/*编辑操作，利用e.NewEditIndex获取当前编辑行索引*/
            GridView2.DataSource = dsQ;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView2.DataBind();
        }
        public int yingxiangHang = 0;
        public int yingxiangNum = 0;
        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            Int32 id = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Value);/*获取主键，需要设置 DataKeyNames，这里设为 id */
            string sql = "";
            string sql_1 = "update VideoUrl set ";
            string sql_2 = " where " + list[0] + "='" + id + "'";
            string sql_3 = "";
            yingxiangNum = 0;
            string changBefore = "";
            //获取被影响的个数
            for (int i = 2; i < list.Count; i++) {
                if (i >= 5)
                    changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i] + " from VideoUrl where " + list[0] + "='" + id + "'");
                else
                    changBefore = SqlFunction.Sql_ReturnNumberES("select  " + list[i - 1] + " from VideoUrl where " + list[0] + "='" + id + "'");
                if (changBefore != (GridView2.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                    yingxiangNum++;
                }
                if (i == list.Count - 1)
                    sql_3 += list[i] + "='" + (GridView2.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "'";
                else {
                    if (i >= 5)
                        sql_3 += list[i] + "='" + (GridView2.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                    else
                        sql_3 += list[i - 1] + "='" + (GridView2.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                }
            }
            sql = sql_1 + sql_3 + sql_2;

            if (yingxiangNum > 0) {
                yingxiangHang = 1;
                if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0) {
                    GridView2.EditIndex = -1;
                    string vid = SqlFunction.Sql_ReturnNumberES("select vid from videourl where id="+id);
                    sql = "select id as 编号,vid as 对应名称,source as 来源,path as 路径,state as 状态,sort as 优先级,CreateDate as 建立时间,Definition as 清晰度,Advertisement as 是否有广告,Describe as 原标题 from videoUrl where vid=" + vid;
                    dsQ.Tables.Clear();
                    dsQ = SqlFunction.DataBind(sql, 10, GridView2);
                    Layer(string.Format(@"layer.alert('修改成功,影响{0}行,共{1}个值', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", yingxiangHang, yingxiangNum), Page);
                } else {
                    GridView2.EditIndex = -1;
                    Layer(@"layer.alert('失败,未修改', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                }
            } else {
                if (yingxiangNum == 0)
                    Layer(@"layer.alert('您未改动数据', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                else
                Layer(@"layer.alert('程序BUG', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }
        }
        public void Alert(string info,Page p) {
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            GridView2.EditIndex = -1;                 /*编辑索引赋值为-1，变回正常显示状态*/
            GridView2.DataSource = dsQ;
            DataTable dtt = new DataTable();
            dtt = dsQ.Tables[0];
            GridView2.DataBind();
        }

        protected void GridView3_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            GridView3.EditIndex = -1;                 /*编辑索引赋值为-1，变回正常显示状态*/
            GridView3.DataSource = tdsQ;
            DataTable dtt = new DataTable();
            dtt = tdsQ.Tables[0];
            GridView3.DataBind();
        }

        protected void GridView3_RowEditing(object sender, GridViewEditEventArgs e) {
            GridView3.DataKeyNames = new string[] { "编号" };
            GridView3.EditIndex = e.NewEditIndex;/*编辑操作，利用e.NewEditIndex获取当前编辑行索引*/
            GridView3.DataSource = tdsQ;
            DataTable dtt = new DataTable();
            dtt = tdsQ.Tables[0];
            GridView3.DataBind();
        }

        protected void GridView3_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            Int32 id = Convert.ToInt32(GridView3.DataKeys[e.RowIndex].Value);/*获取主键，需要设置 DataKeyNames，这里设为 id */
            string sql = "";
            string sql_1 = "update VideoType set ";
            string sql_2 = " where " + tlist[0] + "='" + id + "'";
            string sql_3 = "";
            yingxiangNum = 0;
            string changBefore = "";
            //获取被影响的个数
            for (int i = 2; i < tlist.Count+1; i++) {
                    changBefore = SqlFunction.Sql_ReturnNumberES("select  " + tlist[i - 1] + " from VideoType where " + tlist[0] + "='" + id + "'");
                if (changBefore != (GridView3.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                    yingxiangNum++;
                }
                if (i == tlist.Count)
                    sql_3 += tlist[i-1] + "='" + (GridView3.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "'";
                else {
                   sql_3 += tlist[i-1] + "='" + (GridView3.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                }
            }
            sql = sql_1 + sql_3 + sql_2;
            if (yingxiangNum > 0) {
                yingxiangHang = 1;
                if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0) {
                    selectType();
                    GridView3.EditIndex = -1;                 /*编辑索引赋值为-1，变回正常显示状态*/
                    GridView3.DataSource = tdsQ;
                    DataTable dtt = new DataTable();
                    dtt = tdsQ.Tables[0];
                    GridView3.DataBind();
                    Layer(string.Format(@"layer.alert('修改成功,影响{0}行,共{1}个值', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", yingxiangHang,yingxiangNum), Page);
                } else {
                    GridView3.EditIndex = -1;
                    Layer(@"layer.alert('失败,未修改', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                }
            } else {
                if (yingxiangNum == 0)
                    Layer(@"layer.alert('您未改动数据', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                else
                    Layer(@"layer.alert('程序BUG', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e) {
            //首先判断是否是数据行  
            if (e.Row.RowType == DataControlRowType.DataRow) {
                //当鼠标停留时更改背景色  
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='rgba(0,0,0,0.1)'");
                //当鼠标移开时还原背景色  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }  
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            GridView1.EditIndex = -1;                 /*编辑索引赋值为-1，变回正常显示状态*/
            GridView1.DataSource = vdsQ;
            DataTable dtt = new DataTable();
            dtt = vdsQ.Tables[0];
            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e) {
            GridView1.DataKeyNames = new string[] { "编号" };
            GridView1.EditIndex = e.NewEditIndex;/*编辑操作，利用e.NewEditIndex获取当前编辑行索引*/
            GridView1.DataSource = vdsQ;
            DataTable dtt = new DataTable();
            dtt = vdsQ.Tables[0];
            GridView1.DataBind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            Int32 id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);/*获取主键，需要设置 DataKeyNames，这里设为 id */
            string sql = "";
            string sql_1 = "update Video set ";
            string sql_2 = " where " + vlist[0] + "='" + id + "'";
            string sql_3 = "";
            yingxiangNum = 0;
            string changBefore = "";
            //获取被影响的个数
            for (int i = 2; i < vlist.Count+1; i++) {
                changBefore = SqlFunction.Sql_ReturnNumberES("select  " + vlist[i - 1] + " from Video where " + vlist[0] + "='" + id + "'");
                if (i == 8) {
                    if (changBefore != (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as CheckBox).Checked.ToString()) {
                        yingxiangNum++;
                    }
                }else
                if (changBefore != (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString()) {
                    yingxiangNum++;
                }
                if (i == vlist.Count)
                    sql_3 += vlist[i - 1] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "'";
                else {
                    if(i==8)
                        sql_3 += vlist[i - 1] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as CheckBox).Checked.ToString() + "',";
                    else
                    sql_3 += vlist[i - 1] + "='" + (GridView1.Rows[e.RowIndex].Cells[i].Controls[0] as TextBox).Text.ToString() + "',";
                }
            }
            sql = sql_1 + sql_3 + sql_2;

            if (yingxiangNum > 0) {
                yingxiangHang = 1;
                if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0) {
                    GridView1.EditIndex = -1;
                    //string vid = SqlFunction.Sql_ReturnNumberES("select vid from videourl where id=" + id);
                    sql = "select id as 编号,tid as 对应类型,Title as 标题,Cover as 标题图片,Describe as 描述,CreateDate as 建立时间,Sort as 优先级,State as 状态,Definition as 清晰度,Advertisement as 是否有广告,Describe as 原标题 from video where id=" + id;
                    dsQ.Tables.Clear();
                    dsQ = SqlFunction.DataBind(sql, 10, GridView1);
                    Layer(string.Format(@"layer.alert('修改成功,影响{0}行,共{1}个值', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", yingxiangHang, yingxiangNum), Page);
                } else {
                    GridView1.EditIndex = -1;
                    Layer(@"layer.alert('失败,未修改', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                }
            } else {
                if (yingxiangNum == 0)
                    Layer(@"layer.alert('您未改动数据', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
                else
                    Layer(@"layer.alert('程序BUG', {
                              icon: 5,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
            //首先判断是否是数据行  
            if (e.Row.RowType == DataControlRowType.DataRow) {
                //当鼠标停留时更改背景色  
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='rgba(0,0,0,0.1)'");
                //当鼠标移开时还原背景色  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }  
        }
    }
}