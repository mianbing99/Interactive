using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Admin.Class;
using Web.Admin.model;

namespace Web.Admin.titleOperation {
    public partial class Register : System.Web.UI.Page {
        public static user ua = new user();
        public static int state;
        public static int register;
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] != null) {
                ua = (user)Session["Adminu"];
                state = ua.state;
                register = ua.register;
            } else {
                Response.Redirect("/Admin/login.aspx");
            }
        }

        //注册
        protected void btn_r_ok_Click(object sender, EventArgs e) {
            string info = check();
            if (Convert.ToInt32(SqlFunction.Sql_ReturnNumberES("select count(*) from admin where user_name='" + txt_name.Value + "'")) != 0) {
                LayerA("注册失败,已存在此用户", 5, Page);
                return;
            }
            if (info == "") {//检查注册信息
                int[] bl;
                bl = new int[6];
                if (cb_add.Checked == true) bl[0] = 1; else bl[0] = 0;
                if (cb_update.Checked == true) bl[2] = 1; else bl[1] = 0;
                if (cb_select.Checked == true) bl[3] = 1; else bl[2] = 0;
                if (cb_dele.Checked == true) bl[1] = 1; else bl[3] = 0;
                if (cb_regedit.Checked == true) bl[4] = 1; else bl[4] = 0;
                if (cb_export.Checked == true) bl[5] = 1; else bl[5] = 0;
                string sql = "";
                DateTime dt = DateTime.Now;
                sql = string.Format(@"insert into Admin (User_Name,User_Pwd,
                    User_Email,phone,a_state,isadd,isupdate,isselect,isdelete,isregedit,isexport,creatTime)
                    values(" + "'" + txt_name.Value + "',"
                                 + "'" + MD5pwd.MD5zsgc.MD5Entry(this.txt_pwd.Value) + "',"
                                 + "'" + txt_email.Value + "',"
                                 + "'" + txt_phone.Value + "',"
                                 + "'" + ddl_s.Text + "',"
                                 + "'" + bl[0] + "',"
                                 + "'" + bl[1] + "',"
                                 + "'" + bl[2] + "',"
                                 + "'" + bl[3] + "',"
                                 + "'" + bl[4] + "',"
                                 + "'" + bl[5] + "',"
                                 + "'" + dt.ToString() + "')");
                if (SqlFunction.Sql_ReturnNumberENQ(sql) == 1) {
                    string power="";
                    power=string.Format( "[增:{0}],[改:{1}],[查:{2}],[删:{3}],[注册:{4}],[导出:{5}];1表示有权限0表示无权限"
                        ,bl[0],bl[1],bl[2],bl[3],bl[4],bl[5]);
                    ua = (user)Session["Adminu"];
                    string info2 = "";//备份的注册信息
                    info2 = string.Format("[密码:{0}][状态:{1}]{2}",
                    MD5pwd.MD5zsgc.MD5Entry(txt_pwd.Value), ddl_s.SelectedValue, power);
                    string clientIP = HttpContext.Current.Request.UserHostAddress;
                    Buckup.Buckup_AccountOperation("注册用户", ua.name,clientIP, txt_name.Value, info2);
                    LayerA("注册成功", 6, Page);
                } else {
                    LayerA("注册失败", 5, Page);
                }
            } else { //检查不规范
                LayerA(info,5,Page);
            }
        }
        public string check() {
            if (txt_name.Value == "" || txt_pwd.Value == "") {
                return "用户名或密码不能为空";
            }
            if (txt_pwd.Value != txt_pwd_qr.Value) {
                return "两次密码输入不相同";
            }
            //if (cb_dele.Checked == true) {
            //    if (txt_phone.Value == "") {
            //        Layer("layer.msg('具备删除和注册功能必须填写手机!')", Page);
            //    }
            //}
            return "";
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
        public static void LayerA(string info, int icon, Page p) {
            string script = "<script type='text/javascript' src='../js/jquery-1.11.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>layer.alert('";
            script = script + info + "!', {title: false,icon:" + icon + ",skin: 'layer-ext-moon' })" + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }
    }
}