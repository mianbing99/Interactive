using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Web.Common;
using Web.Admin.MD5pwd;
using Web.Admin.Class;
using System.Data;
using Web.Admin.model;

namespace Web.Admin
{
    public partial class login : System.Web.UI.Page
    {
        VideoHandler vh = new VideoHandler();
        static public string loginok = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminLogin"] != null)
            {
                Response.Redirect("/Admin/NavigationTest.aspx", false);
            }
            string str = Request.QueryString["Action"];
            if (!string.IsNullOrEmpty(str))
            {
                if (str.ToLower() == "login")
                {
                    string drag_hk = Request.Form["drag_hk"];
                    string pid = Request.Form["TxbPid"];
                    string pwd = Request.Form["TxbPwd"];
                    pwd= MD5pwd.MD5zsgc.MD5Entry(pwd);
                    string clientIp = Request.UserHostAddress;
                    string sql = "";
                    sql = "select user_pwd from admin where User_Name='" + pid+"'";
                    string sql_pwd = SqlFunction.Sql_ReturnNumberES(sql);
                    if (sql_pwd == "") {
                        Response.Write("<script Language=\"javascript\">alert(\"帐号错误！\");</script>");
                    } else {
                        user ua = new user();
                        DataSet ds = new DataSet();
                        ds = SqlFunction.Sql_DataAdapterToDS("select * from admin where User_Name='" + pid + "'");
                        ua.state = Convert.ToInt32(ds.Tables[0].Rows[0][5]);
                        if (ua.state != 1) {
                            Response.Write("<script Language=\"javascript\">alert(\"您的帐号权限异常！\");</script>");
                        } else {
                            if (sql_pwd == pwd) {
                                Session["AdminLogin"] = pid + "|" + pwd;
                                ua.id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                ua.name = ds.Tables[0].Rows[0][1].ToString();
                                ua.pwd = ds.Tables[0].Rows[0][2].ToString();
                                ua.email = ds.Tables[0].Rows[0][3].ToString();
                                ua.phone = ds.Tables[0].Rows[0][4].ToString();
                                ua.add = Convert.ToInt32(ds.Tables[0].Rows[0][6]);
                                ua.update = Convert.ToInt32(ds.Tables[0].Rows[0][7]);
                                ua.select = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                                ua.delete = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                                ua.register = Convert.ToInt32(ds.Tables[0].Rows[0][10]);
                                ua.export = Convert.ToInt32(ds.Tables[0].Rows[0][11]);
                                Session["Adminu"] = ua;
                                Response.Redirect("/Admin/NavigationTest.aspx", false);
                            } else {
                                Response.Write("<script>alert(\"密码错误！\");</script>");
                            }
                        }
                    }
                }
            }
        }
    }
}