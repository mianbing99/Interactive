using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Web.Admin.Class;
using Web.Admin.MD5pwd;
using Web.Admin.model;

namespace Web.Admin
{
    public partial class AdminPage : System.Web.UI.MasterPage {
        public string _UserName;
        public string _LastTime;
        public string _PageTitle;
        static public int state;
        static public int select1,add,expore;
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                user u = new user();
                u = (user)Session["Adminu"];
                state = u.state;
                select1 = u.select;
                add = u.add;
                expore = u.export;
            }
        }

        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='Admin/js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }




    }
}