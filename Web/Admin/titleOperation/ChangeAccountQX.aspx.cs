using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Web.Admin.Class;
using Web.Admin.Data;
using Web.Admin.model;

namespace Web.Admin.titleOperation {
    public partial class ChangeAccountQX : System.Web.UI.Page {
        public bool b;
        public static user ua = new user();
        public static int state;
        public static int register;
        static string clientIP = HttpContext.Current.Request.UserHostAddress;
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                ua = (user)Session["Adminu"];
                state = ua.state;
                register = ua.register;
                if (IsPostBack) {
                    CreateControl();
                    b = true;
                }
            }
        }

        private void CreateControl() {
            if (b == false) {
                b = true;
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS("select * from admin");
                int count = ds.Tables[0].Rows.Count;
                TableRow row = new TableRow();
                for (int y = 0; y < count; y++) {
                    TableCell cell = new TableCell();
                    Button bt = new Button();
                    bt.Text = ds.Tables[0].Rows[y][1].ToString();
                    bt.Command += new CommandEventHandler(this.bt_Click);//预定事件  
                    //bt.Attributes["onclick"] = "javascript:loadinfo();";
                    bt.CssClass = "layui-btn layui-btn-primary layui-btn-small";
                    cell.Controls.Add(bt);
                    row.Cells.Add(cell);
                }
                btn_jihe.Controls.Add(row);

            }
            b = false;
            //这里还可以写成：
            //Page.Form.Controls.Add(row);



        }
       static public object[] info = new object[10];
       public static string goname = "";//js需要,删除用户,获取用户名
        void bt_Click(object sender, EventArgs e) {
            
            updateuser_div.Attributes.Add("style","display:");
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS("select * from admin where user_name='" + ((Button)sender).Text + "'");
            info[0] = txt_name.Text = goname = ds.Tables[0].Rows[0][1].ToString();
            info[1] = txt_email.Value = ds.Tables[0].Rows[0][3].ToString();
            info[2] = txt_phone.Value = ds.Tables[0].Rows[0][4].ToString();
            info[3] = ddl_s.SelectedIndex = Convert.ToInt32(ds.Tables[0].Rows[0][5]);
            info[4] = cb_add.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][6]);
            info[5] = cb_update.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][7]);
            info[6] = cb_select.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][8]);
            info[7] = cb_dele.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][9]);
            info[8] = cb_regedit.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][10]);
            info[9] = cb_export.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0][11]);
            info[4] = info[4].ToString().ToLower();
            info[5] = info[5].ToString().ToLower();
            info[6] = info[6].ToString().ToLower();
            info[7] = info[7].ToString().ToLower();
            info[8] = info[8].ToString().ToLower();
            info[9] = info[9].ToString().ToLower();
        }
        protected void getuser_btn_Click(object sender, EventArgs e) {
            CreateControl();
        }
        public static void LayerA(string info,int icon, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>layer.alert('";
            script = script + info + "!', {title: false,icon:" + icon + ",skin: 'layer-ext-moon' })" + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        protected void btn_okchang_Click(object sender, EventArgs e) {
            try {
                ua = (user)Session["Adminu"];
                //Buckup.Buckup_ChangePower(ua.name, txt_name.Text, Hidden1.Value);
                Buckup.Buckup_AccountOperation("修改账号权限", ua.name, clientIP,txt_name.Text ,Hidden1.Value.Trim());
                string id = SqlFunction.Sql_ReturnNumberES("select id from admin where user_name='"+txt_name.Text+"'");
                string sql = "";
                sql = string.Format(@"update admin set User_Name='{0}', User_Email='{1}', 
                phone='{2}', a_state='{3}', isadd='{4}', isupdate='{5}', isselect='{6}', 
                isdelete='{7}', isregedit='{8}', isexport='{9}' where id=" + id,
                    txt_name.Text, txt_email.Value, txt_phone.Value, ddl_s.SelectedIndex,
                    Convert.ToInt32(cb_add.Checked), Convert.ToInt32(cb_update.Checked), Convert.ToInt32(cb_select.Checked),
                    Convert.ToInt32(cb_dele.Checked), Convert.ToInt32(cb_regedit.Checked), Convert.ToInt32(cb_export.Checked));
                int r=SqlFunction.Sql_ReturnNumberENQ(sql);
                if ( r== 1) {
                    LayerA("修改成功!",6,Page);
                } else {
                    LayerA("修改失败!错误信息:"+r, 5, Page);
                }
            } catch (Exception ee) {
                LayerA("修改失败!错误信息:"+ee.Message, 5, Page);
            }
        }

        protected void btn_deleteUser_Click(object sender, EventArgs e) {
            try {
                ua = (user)Session["Adminu"];

                Buckup.Buckup_AccountOperation("删除用户", ua.name, clientIP, txt_name.Text, "");
                int r = SqlFunction.Sql_ReturnNumberENQ("delete from admin where user_name='" + info[0] + "'");
                if (r == 1) {
                    LayerA("删除成功!", 6, Page);
                } else {
                    LayerA("删除失败!错误信息:" + r, 5, Page);
                }
            } catch (Exception ee) {
                LayerA("删除失败!错误信息:" + ee.Message, 5, Page);
            }
        }
    }
}