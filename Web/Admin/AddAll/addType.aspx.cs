using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Web.Admin.Class;
using System.IO;
using Web.Admin.model;

namespace Web.Admin.AddAll {
    public partial class addType : System.Web.UI.Page {
        DataTable dt = new DataTable();
        static public user u = new user();
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    Button3.CssClass = "layui-btn layui-btn-primary layui-btn-mini";
                    Button3.Enabled = false;
                    Button2.CssClass = "layui-btn layui-btn-big layui-btn-primary layui-btn-radius layui-btn-disabled";
                    Button2.Enabled = false;
                    bing();
                }
            }
        }
        private void bing() {
            try {
                string sql = @"
 select id,Tid,Title,Cover,sort,state from videotype where state=1";
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(sql);
                dt.Clear();
                TreeView1.Nodes.Clear();
                dt = ds.Tables[0];
                bindData("0", TreeView1.Nodes, "根目录");
            } catch (Exception e) {
                Layer(string.Format(@"layer.alert('出现异常:{0}', {{
                              icon: 2,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})", e.Message.Replace("'", "〞")), Page);
            } finally { }
        }
        private void bindData(string parentid, TreeNodeCollection tnc, string path) {
            DataRow[] ary_row = dt.Select("Tid=" + parentid, "sort");
            foreach (DataRow item in ary_row) {
                TreeNode node = new TreeNode();
                string txtpath = path + "//" + item["Title"].ToString();
                node.Text = string.Format("<font style=\"cursor:pointer;\">{0}</font>", item["Title"].ToString());
                node.Value = item["Id"].ToString();
                node.Expanded = false;//是否展开
                //node.ShowCheckBox = true;//是否显示选择框
                //node.SelectAction = TreeNodeSelectAction.None;
                //node.NavigateUrl = "javascript:selval('" + txtpath + "');";//连接路径
                //node.SelectAction = TreeNodeSelectAction.Expand;//选择事件
                tnc.Add(node);
                bindData(item["Id"].ToString(), tnc[tnc.Count - 1].ChildNodes, txtpath);
                
            }
        }
        public void Alert(string info, Page p) {
            //FileUpload1.FileContent.Dispose();
            //DeleteFolder(Server.MapPath("~/UpdateExcel/"));
            string script = "<script>alert('" + info + "')</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }


        protected void Button2_Click(object sender, EventArgs e) {
            if (TreeView1.SelectedValue == "") {
                Layer(@"layer.alert('请选中左侧节点类型', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            })", Page);
            } else {
                if (title.Value != "" && img.Value != "" && sort.Value != "") {
                    if (TreeView1.SelectedNode.ChildNodes.Count == 0) {//判断选择节点下是否有类型,没有则将原来tid替换成新的tid
                        string sql = "insert into  videotype (Tid,Title,Cover,Sort,State) Values(" + TreeView1.SelectedValue + ",'" + title.Value + "','" + img.Value + "'," + sort.Value + ",1 )";
                        if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0) {
                            string clientIP = HttpContext.Current.Request.UserHostAddress;
                            u = (user)Session["Adminu"];
                            Buckup.Buckup_Add("添加类型", u.name, clientIP, 1);
                            string rtid = "select  id from videotype where title='" + title.Value + "'";
                            Text1.Value = SqlFunction.Sql_ReturnNumberES(rtid);
                            if (SqlFunction.Sql_ReturnNumberENQ("update video set tid=(" + rtid + ") where tid=" + TreeView1.SelectedValue) > 0) {
                                bing();
                                Layer(@"layer.alert('类型添加成功,原类型转移成功', {
                                      icon: 6,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                            } else {
                                bing();
                                Layer(@"layer.alert('类型添加成功,但原类型转移失败,请通知开发者转移', {
                                      icon: 0,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                            }
                        } else {
                            Layer(@"layer.alert('添加失败', {
                                      icon: 5,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                        }
                    } else {
                        string sql = "insert into  videotype (Tid,Title,Cover,Sort,State) Values(" + TreeView1.SelectedValue + ",'" + title.Value + "','" + img.Value + "'," + sort.Value + ",0 )";
                        if (SqlFunction.Sql_ReturnNumberENQ(sql) > 0) {
                            string rtid = "select  id from videotype where title='" + title.Value + "'";
                            Text1.Value=SqlFunction.Sql_ReturnNumberES(rtid); bing();
                            Layer(@"layer.alert('添加成功', {
                                      icon: 6,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                        } else {
                            bing();
                            Layer(@"layer.alert('添加失败', {
                                      icon: 5,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                        }
                    }
                } else {
                    Layer(@"layer.alert('有数据未填写', {
                                      icon: 0,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                }
            }
        }
        public object returnpath() {
            string index2 = TreeView1.SelectedValue;
            string newcover2 = "";
            if (index2 != "") {
                string cover = SqlFunction.Sql_ReturnNumberES("select cover from videotype where id=" + index2);
                //string temp = FileUpload1.FileName;
                cover = cover.Replace("\\", "/");
                //temp = temp.Replace("\\", "\\\\");
                string[] aselect = cover.Split('/');
                string[] newselect = new string[aselect.Length - 1];
                for (int i = 0; i < aselect.Length - 1; i++) {
                    newselect[i] = aselect[i] + "/";
                    newcover2 += newselect[i];
                }
            } else { }
            return newcover2;
        }
        public object getflpath() {
            string index = TreeView1.SelectedValue;
            string newcover = "";
            if (index == "") {
                Layer(@"layer.alert('请点击左侧节点', {
                                      icon: 0,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                return "false";
            } else {
                if (Convert.ToInt32(index) < 3) {

                    Layer(@"layer.alert('不支持在一级目录下上传', {
                                      icon: 2,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
                    return "no";
                } else {
                    //string viersortsql = "select Title,Sort from videotype where tid=(select Tid from VideoType where id="+index+") and state=1 order by Sort ";
                    //DataSet dssort = new DataSet();
                    //dssort = SqlFunction.Sql_DataAdapterToDS(viersortsql);
                    //string dicsort ="";
                    //int sortcount=dssort.Tables[0].Rows.Count;
                    //for (int m = 0; m < sortcount; m++)
                    //{
                    //    dicsort += dssort.Tables[0].Rows[m][0].ToString()+":"+dssort.Tables[0].Rows[m][1].ToString();
                    //    this.ClientScript.RegisterStartupScript(this.GetType(), "", "createlabel('" + dicsort + "')", true);
                    //    dicsort = "";
                    //}
                    
                    string fname = uploadimg(index);
                    //显示图片
                    string cover = SqlFunction.Sql_ReturnNumberES("select cover from videotype where id=" + index);
                    cover = cover.Replace("\\", "/");
                    string[] aselect = cover.Split('/');
                    string[] newselect = new string[aselect.Length - 1];
                    int lc = 0;
                    if (aselect.Length > 6) {
                        lc = aselect.Length - 2;
                    } else {
                        lc = aselect.Length - 1;
                    }
                    for (int i = 0; i < lc; i++) {
                        newcover += aselect[i] + "/";
                    }
                    img.Value = img_c.Src = newcover + fname;
                    Button3.Enabled = false;
                    Button3.CssClass = "layui-btn layui-btn-primary layui-btn-mini layui-btn-disabled";
                    Button2.Enabled = true;
                    Button2.CssClass = "layui-btn layui-btn-big layui-btn-primary layui-btn-radius";
                    //保存其它统计类型的优先级以便查看
                    string sql = "";
                    string sort = "";
                    sql = "select Title,Sort from videotype where tid='"
                        + TreeView1.SelectedValue + "'" + " order by Sort";
                    DataSet dss = new DataSet();
                    dss = SqlFunction.Sql_DataAdapterToDS(sql);
                    GridView1.DataSource = dss.Tables[0];
                    GridView1.DataBind();
                }
            }
            return newcover;
        }
        private string uploadimg(string inde) {
            string path = "";
            string ptxt = "";
            try {
                //string tid = SqlFunction.Sql_ReturnNumberES("select tid from videotype where id=" + inde);
                switch (inde) {
                    case "3": path = "laoren/xiqu/"; break;
                    case "4": path = "laoren/xiangsheng/"; break;
                    case "5": path = "laoren/xiaopin/"; break;
                    case "6": path = "laoren/wudao/"; break;
                    case "75": path = "laoren/qiangshen/"; break;
                    case "76": path = "laoren/gequ/"; break;
                    case "55": path = "youer/yishuleyuan/"; break;
                    case "56": path = "youer/dongmanerge/"; break;
                    case "57": path = "youer/kexuetiandi/"; break;
                    case "58": path = "youer/jiankangzhishi/"; break;
                    case "59": path = "youer/quweiyingyu/"; break;
                    case "60": path = "youer/yuyanwenxue/"; break;
                    case "62": path = "youer/guoxueqimeng/"; break;
                    case "63": path = "youer/gushitiandi/"; break;
                    case "64": path = "youer/donghuapian/"; break;
                    case "65": path = "youer/wudaoxueyuan/"; break;
                    case "66": path = "youer/yuertiandi/"; break;
                    case "317": path = "youer/dula/"; break;
                    //case "340": path = "youer/dula/"; break;幼儿伴侣:小小歌唱家模块下:中文.英文.经典儿歌和爸妈最爱,不支持添加类型
                    case "363": path = "youer/qimengshuxue/"; break;
                    case "364": path = "youer/qimengyingyu/"; break;
                    case "365": path = "youer/qimengyuwen/"; break;
                    case "366": path = "youer/wodeyoueryuan/"; break;
                    case "367": path = "youer/zaojiaoleyuan/"; break;
                    case "395": path = "youer/jiankang/"; break;
                    case "396": path = "youer/shehui/"; break;
                    default:
                        string ztid = SqlFunction.Sql_ReturnNumberES("select tid from videotype where tid=" + inde);
                        if (ztid == "") {
                            string scover = SqlFunction.Sql_ReturnNumberES("select cover from videotype where id=" + inde);
                            scover = scover.Replace("\\", "/");
                            string[] stxt = scover.Split('/');
                            string zhongbupath = "";
                            for (int m = 4; m < stxt.Length - 1; m++) {
                                zhongbupath += (stxt[m] + "/");
                            }
                            string titletxt = SqlFunction.Sql_ReturnNumberES("select title from videotype where id=" + inde);
                            string a = inde + NPinyin.Pinyin.GetInitials(titletxt).ToLowerInvariant() + "/";
                            path = path + zhongbupath + a;
                            Directory.CreateDirectory(Server.MapPath("~/Img/" + path));//创建文件夹
                        } else {
                            string zcover = SqlFunction.Sql_ReturnNumberES("select cover from videotype where tid=" + ztid);
                            zcover = zcover.Replace("\\", "/");
                            string[] tfj = zcover.Split('/');
                            for (int i = 4; i < tfj.Length - 1; i++) {
                                path += (tfj[i] + "/");

                            }
                            //Directory.CreateDirectory(Server.MapPath("~/Img/" + path));//创建文件夹
                        }
                        break;
                }
                FileUpload1.SaveAs(Server.MapPath("~/Img/" + path) + FileUpload1.FileName);//把图片上传至文件夹
                
                string[] jfj = (path + FileUpload1.FileName).Split('/');
                for (int j = 1; j < jfj.Length; j++) {
                    ptxt += jfj[j] + "/";
                }
                try {
                    ptxt = ptxt.Substring(0, ptxt.Length - 1);
                } catch {

                }
            } catch (Exception e) {
                return e.Message;
            }
            return ptxt;
        }

        protected void Button3_Click(object sender, EventArgs e) {
            if (img.Value!="" || FileUpload1.HasFile)
                getflpath();
            else
                Layer(@"layer.alert('请选择图片', {
                                      icon: 0,title: false,
                                      shadeClose: true,skin: 'layer-ext-moon'  
                                    })", Page);
        }


        
    }
}