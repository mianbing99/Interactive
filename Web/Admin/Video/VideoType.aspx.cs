using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Video
{
    public partial class VideoType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    Master._PageTitle = "e家亲分类管理";
                }
            }
        }
    }
}