using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;

namespace Web.Child {
    public partial class Listen : System.Web.UI.Page {
        public int ID;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                    string openId = Convert.ToString(Session["openId"]);
                    ID = Convert.ToInt32(Request.QueryString["id"]);
                    if (ID == 1) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeList();
                        Rep_listen.DataBind();
                    }
                    if (ID == 2) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListtwo();
                        Rep_listen.DataBind();
                    }
                    if (ID == 3) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListthree();
                        Rep_listen.DataBind();
                    }
                    if (ID == 4) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListfour();
                        Rep_listen.DataBind();
                    }
                    if (ID == 5) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListfive();
                        Rep_listen.DataBind();
                    }
                    if (ID == 6) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListsix();
                        Rep_listen.DataBind();
                    }
                    if (ID == 7) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListSevens();
                        Rep_listen.DataBind();
                    }
                    if (ID == 8) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListEight();
                        Rep_listen.DataBind();
                    }
                    if (ID == 9) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListNine();
                        Rep_listen.DataBind();
                    }
                    if (ID == 10) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListTen();
                        Rep_listen.DataBind();
                    }
                    if (ID == 11) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListEleven();
                        Rep_listen.DataBind();
                    }
                    if (ID == 12) {
                        Rep_listen.DataSource = VideoTypeList.Instance().GetVideoTypeListTwelve();
                        Rep_listen.DataBind();
                    }
                }
            }
        }
    }
}