using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web {
    public partial class WebForm1 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Button1_Click(object sender, EventArgs e) {
            //DateTime now = DateTime.Now;
            string Yesdate = DateTime.Now.AddDays(-1).ToShortDateString();//昨天
            //string date = DateTime.Now.Date.ToShortDateString();//今天
            //string d1 = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            Response.Write(Yesdate);
        }
    }
}