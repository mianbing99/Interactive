using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;

namespace Web.Child {
    public partial class ChildrenList : System.Web.UI.Page {
        public int ID;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                //获取4-6岁的视频信息
                if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                    string openId = Convert.ToString(Session["openId"]);
                    ID = Convert.ToInt32(Request.QueryString["id"]);
                    if (ID == 7) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListSevens();
                        Rep_child.DataBind();
                    }
                    if (ID == 8) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListEight();
                        Rep_child.DataBind();
                    }
                    if (ID == 9) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListNine();
                        Rep_child.DataBind();
                    }
                    if (ID == 10) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListTen();
                        Rep_child.DataBind();
                    }
                    if (ID == 11) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListEleven();
                        Rep_child.DataBind();
                    }
                    if (ID == 12) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListTwelve();
                        Rep_child.DataBind();
                    }
                    if (ID == 476) {
                        Rep_child.DataSource = VideoTypeList.Instance().GetVideoTypeListanquan();
                        Rep_child.DataBind();
                    }
                }
            }
        }
    }
}