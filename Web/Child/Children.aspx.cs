using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;

namespace Web.Child {
    public partial class Children : System.Web.UI.Page {
        public int ID;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            //if (!IsPostBack) {
                if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                    string openId = Convert.ToString(Session["openId"]);
                    ID = Convert.ToInt32(Request.QueryString["id"]);
                    //故事天地asdsadasdasdasdas
                    if (ID == 63) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeStory();
                        Rep_children.DataBind();
                    }
                    if(ID==492){
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypecc(ID);
                        Rep_children.DataBind();
                    }
                    //国学诗词
                    if (ID == 62) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeguoxue();
                        Rep_children.DataBind();
                    }
                    //动漫儿歌
                    if (ID == 56) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeSinger();
                        Rep_children.DataBind();
                    }
                    //艺术乐园
                    if (ID == 55) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeyishu();
                        Rep_children.DataBind();
                    }
                    //科学天地
                    if (ID == 57) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypekexue();
                        Rep_children.DataBind();
                    }
                    //健康知识
                    if (ID == 58) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypejiankang();
                        Rep_children.DataBind();
                    }
                    //小小歌唱家
                    if (ID == 340) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeListtwo();
                        Rep_children.DataBind();
                    }
                    //启蒙数学
                    if (ID == 363) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeshuxue();
                        Rep_children.DataBind();
                    }
                    //启蒙语文
                    if (ID == 365) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeyuwen();
                        Rep_children.DataBind();
                    }
                    //启蒙英语
                    if (ID == 364) {
                        Rep_children.DataSource = VideoTypeList.Instance().GetVideoTypeyingyu();
                        Rep_children.DataBind();
                    }
                }
            }
        }
    }
//}