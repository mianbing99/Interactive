using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Admin.Class;
using Web.Admin.model;

namespace Web.Admin.titleOperation {
    public partial class ChangePwd : System.Web.UI.Page {
        public static user ua = new user();
        public static int state;
        public static int register;
        public static string name;
        protected void Page_Load(object sender, EventArgs e) {

            if (Session["AdminLogin"] != null) {
                ua = (user)Session["Adminu"];
                state = ua.state;
                register = ua.register;
                name = ua.name;
            } else {
                Response.Redirect("/Admin/login.aspx");
            }
        }
        protected void btn_changepwd_Click(object sender, EventArgs e) {
            ua = (user)Session["Adminu"];
            string clientIP = HttpContext.Current.Request.UserHostAddress;
            string pwd =MD5pwd.MD5zsgc.MD5Entry(old_pwd.Value);
            string newpwd = MD5pwd.MD5zsgc.MD5Entry(new_pwd.Value);
            if (SqlFunction.Sql_ReturnNumberES("select user_pwd from admin where user_name='" + ua.name + "'") == pwd) {
                int jg=SqlFunction.Sql_ReturnNumberENQ("update admin set user_pwd='" + newpwd + "' where user_name='" + ua.name + "'");
                if (jg ==1) {
                    Buckup.Buckup_AccountOperation("更改密码", ua.name,ua.name, clientIP, string.Format("将原密码{0},更改成{1}", pwd, newpwd));
                    LayerA("修改成功", 6, Page);
                }
            } else {
                LayerA("原密码错误",5,Page);
            }

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