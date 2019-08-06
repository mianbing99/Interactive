using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Web.Admin.Class;

namespace Web.Admin {
    public partial class NavigationTest : System.Web.UI.Page {
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
            
            }
        }

    }
}