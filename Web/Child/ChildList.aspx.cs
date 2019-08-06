using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;

namespace Web.Child {
    public partial class ChildList : System.Web.UI.Page {
        public int ID;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                    string openId = Convert.ToString(Session["openId"]);
                    ID = Convert.ToInt32(Request.QueryString["id"]);
                    //故事天地
                    if (ID == 63) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeStory();
                        Rep_lit.DataBind();
                    }
                    //动漫儿歌
                    if (ID == 56) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeSinger();
                        Rep_lit.DataBind();
                    }
                    //艺术乐园
                    if (ID == 55) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeyishu();
                        Rep_lit.DataBind();
                    }
                    //科学天地
                    if (ID == 57) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypekexue();
                        Rep_lit.DataBind();
                    }
                    //健康知识
                    if (ID == 58) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypejiankang();
                        Rep_lit.DataBind();
                    }
                    //国学诗词
                    if (ID == 62) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeguoxue();
                        Rep_lit.DataBind();
                    }
                    //启蒙数学
                    if (ID == 363) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeshuxue();
                        Rep_lit.DataBind();
                    }
                    //启蒙语文
                    if (ID == 365) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeyuwen();
                        Rep_lit.DataBind();
                    }
                    //启蒙英语
                    if (ID == 364) {
                        Rep_lit.DataSource = VideoTypeList.Instance().GetVideoTypeyingyu();
                        Rep_lit.DataBind();
                    }
                }
            }
        }
    }
}