using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;
using WeChat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web {
    public partial class StudyStation : System.Web.UI.Page {
        public int cont1;
        public int cont2;
        public int cont3;
        public int cont4;
        public int cont5;
        public int ys1;
        public int sta;
        public string openId;
        private string code;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if(Session["OpenIdE"]==null){
                if (!string.IsNullOrEmpty(Request.QueryString["code"])) {
                    code = Convert.ToString(Request.QueryString["code"]);
                    string returnStr = getOpenId(code);
                    JObject json = JObject.Parse(returnStr);
                    openId = json["openid"].ToString();
                    Session["OpenIdE"] = openId;
                    string DeviceId = QrChildManager.GetChildDeviceId(openId);
                    if (DeviceId != "" || DeviceId != null) {//根据得到的token拿到用户表的DeviceId
                        Session["DeviceId"] = DeviceId;
                        InteractiveHelper.WriteLog("Session[\"DeviceId\"]=" + Session["DeviceId"].ToString());
                        string StudyDeviceId = DeviceId;//根据拿到用户表的DeviceId匹配学习情况表里的DeviceId
                        if (StudyDeviceId != null && StudyDeviceId.Length != 0) {
                            //今天学习内容列表
                            cont1 = (LearnSituation.Instance().setcont1(StudyDeviceId) / 15) + 1;
                            ys1 = 1;
                            List<LearnSit> LearnSitation = LearnSituation.Instance().GetAllLearnSit(StudyDeviceId, 0);
                            Rep_today.DataSource = LearnSitation;//设置数据源
                            Rep_today.DataBind();//绑定数据
                        } else {
                            Response.Write(" <script type=\"text/javascript\"> alert(\"当前没有绑定设备！\");</script>");
                        }
                    } else {
                        Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新进入\");</script>");
                    }
                }
                }
            }
            if(Session["DeviceId"] != null)
            {
                string StuDeviceId = Convert.ToString(Session["DeviceId"]);//根据拿到用户表的DeviceId匹配学习情况表里的DeviceId

                if (StuDeviceId != null && StuDeviceId.Length != 0)
                {
                    //今天学习内容列表
                    cont1 = (LearnSituation.Instance().setcont1(StuDeviceId) / 15) + 1;
                    ys1 = 1;
                    List<LearnSit> LearnSitation = LearnSituation.Instance().GetAllLearnSit(StuDeviceId, 0);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } 
            }
            

            if (!string.IsNullOrEmpty(Request.QueryString["yeshu1"])) {
                int yeshu = Convert.ToInt32(Request.QueryString["yeshu1"]);
                bangdingshuju(yeshu, 1);
            } else if (!string.IsNullOrEmpty(Request.QueryString["yeshu2"])) {
                int yeshu = Convert.ToInt32(Request.QueryString["yeshu2"]);
                bangdingshuju(yeshu, 2);
            } else if (!string.IsNullOrEmpty(Request.QueryString["yeshu3"])) {
                int yeshu = Convert.ToInt32(Request.QueryString["yeshu3"]);
                bangdingshuju(yeshu, 3);
            } else if (!string.IsNullOrEmpty(Request.QueryString["yeshu4"])) {
                int yeshu = Convert.ToInt32(Request.QueryString["yeshu4"]);
                bangdingshuju(yeshu, 4);
            } else if (!string.IsNullOrEmpty(Request.QueryString["yeshu5"])) {
                int yeshu = Convert.ToInt32(Request.QueryString["yeshu5"].ToString());
                bangdingshuju(yeshu, 5);
            }

        }


        public static string getOpenId(string code) {
            //// string i = "  https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1&connect_redirect=1#wechat_redirect";
            string studyUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect";

            studyUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx0e7b25b6f1553eea&secret=0b3f122b0b2b7a345f8cb4793bbcc6ad&code=" + code + "&grant_type=authorization_code ";

            return HttpGet(studyUrl, "");
        }
        public static string HttpGet(string Url, string postDataStr) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public int setysUp(int ys, int yeshu) {
            int i = ys + 1;
            if (ys < 1) {
                i = 1;
            } else if (ys > yeshu) {
                i = yeshu;
            }
            return i;
        }
        public int setysDown(int ys, int yeshu) {
            int i = ys - 1;
            if (ys < 1) {
                i = 1;
            } else if (ys > yeshu) {
                i = yeshu;
            }
            return i;
        }
        public void bangdingshuju(int yeshu, int state) {
            ys1 = yeshu;
            int nyeshu = yeshu - 1;
            string StudyDeviceId = Session["DeviceId"].ToString();
            if (StudyDeviceId != null && StudyDeviceId.Length != 0) {
                //今天学习内容列表
                if (state == 1) {
                    sta = 1;
                    cont1 = LearnSituation.Instance().setcont1(StudyDeviceId);
                    cont1 /= 15; cont1 += 1;
                    List<LearnSit> LearnSitation = LearnSituation.Instance().GetAllLearnSit(StudyDeviceId, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 2) {
                    sta = 2;
                    cont2 = LearnSituation.Instance().setcont2(StudyDeviceId);
                    cont2 /= 15; cont2 += 1;
                    List<LearnSit> LearnYesDay = LearnSituation.Instance().GetYesdLearn(StudyDeviceId, nyeshu);
                    Rep_today.DataSource = LearnYesDay;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 3) {
                    sta = 3;
                    cont3 = LearnSituation.Instance().setcont3(StudyDeviceId);
                    cont3 /= 15; cont3 += 1;
                    List<LearnSit> LearnWeek = LearnSituation.Instance().GetWeekLearn(StudyDeviceId, nyeshu);
                    Rep_today.DataSource = LearnWeek;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 4) {
                    sta = 4;
                    cont4 = LearnSituation.Instance().setcont4(StudyDeviceId);
                    cont4 /= 15; cont4 += 1;
                    List<LearnSit> LearnMoth = LearnSituation.Instance().GetMothLearn(StudyDeviceId, nyeshu);
                    Rep_today.DataSource = LearnMoth;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 5) {
                    sta = 5;
                    cont5 = LearnSituation.Instance().setcont5(StudyDeviceId);
                    cont5 /= 15; cont5 += 1;
                    List<LearnSit> LearnFortime = LearnSituation.Instance().GetForTimeLearn(StudyDeviceId, nyeshu);
                    Rep_today.DataSource = LearnFortime;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else {
                    Response.Write("<script>alert('设备ID为空');</script>");
                }
            }
        }
    }
}