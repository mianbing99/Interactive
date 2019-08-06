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

namespace Web.Interactive
{
    public partial class LearningRecord : System.Web.UI.Page
    {
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
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    code = Convert.ToString(Request.QueryString["code"]);
                    string returnStr = getOpenId(code);
                    JObject json = JObject.Parse(returnStr);
                    openId = json["openid"].ToString();
                    Session["openId"] = openId;
                    string DeviceId = QrInteractiveManager.getDeviceId(openId);
                    if (DeviceId != "" && DeviceId != null)
                    {//根据得到的token拿到用户表的DeviceId
                        DateTime date = DateTime.Now;
                        //今天学习内容列表
                        cont1 = (QrInteractiveManager.getLearnStCount(DeviceId, date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd")) / 15) + 1;
                        ys1 = 1;
                        List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(DeviceId, date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), 15, 0);
                        Rep_today.DataSource = LearnSitation;//设置数据源
                        Rep_today.DataBind();//绑定数据
                    }
                    else
                    {
                        Response.Write(" <script type=\"text/javascript\"> alert(\"当前没有绑定设备\");</script>");
                    }
                }
            }
            if (string.IsNullOrEmpty(Request.QueryString["yeshu1"]) && string.IsNullOrEmpty(Request.QueryString["yeshu2"]) && string.IsNullOrEmpty(Request.QueryString["yeshu3"]) && string.IsNullOrEmpty(Request.QueryString["yeshu4"]) && string.IsNullOrEmpty(Request.QueryString["yeshu5"]))
            {
                string openId = Convert.ToString(Session["openId"]);
                if (openId.Length > 0)
                {
                    string StuDeviceId = QrInteractiveManager.getDeviceId(openId);//根据拿到用户表的DeviceId匹配学习情况表里的DeviceId

                    if (StuDeviceId != null && StuDeviceId.Length != 0)
                    {
                        DateTime date = DateTime.Now;
                        //今天学习内容列表
                        cont1 = (QrInteractiveManager.getLearnStCount(StuDeviceId, date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd")) / 15) + 1;
                        ys1 = 1;
                        List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(StuDeviceId, date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), 15, 0);
                        Rep_today.DataSource = LearnSitation;//设置数据源
                        Rep_today.DataBind();//绑定数据
                    }
                }
                else 
                {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新进入\");</script>");
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
            // string i = "  https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1&connect_redirect=1#wechat_redirect";
            string studyUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect";

            studyUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxf9ade153117809bf&secret=c876b01424956a2f7f7d1bee9dbb0acd&code=" + code + "&grant_type=authorization_code ";

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
            string openId = Convert.ToString(Session["openId"]);
            if (openId.Length>0)
            {
            string StudyDeviceId = QrInteractiveManager.getDeviceId(openId);
            if (StudyDeviceId != null && StudyDeviceId.Length != 0) {
                DateTime date = DateTime.Now;
                string startDate = null;
                string endDate = null;
                //今天学习内容列表
                if (state == 1) {
                    startDate = date.ToString("yyyy-MM-dd");
                    endDate = date.AddDays(1).ToString("yyyy-MM-dd");
                    sta = 1;
                    cont1 = QrInteractiveManager.getLearnStCount(StudyDeviceId, startDate, endDate);
                    cont1 /= 15; cont1 += 1;
                    List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(StudyDeviceId, startDate, endDate, 15, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 2) {
                    startDate = date.AddDays(-1).ToString("yyyy-MM-dd");
                    endDate = date.ToString("yyyy-MM-dd");
                    sta = 2;
                    cont2 = QrInteractiveManager.getLearnStCount(StudyDeviceId, startDate, endDate);
                    cont2 /= 15; cont2 += 1;
                    List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(StudyDeviceId, startDate, endDate, 15, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 3) {
                    startDate = date.AddDays(-6).ToString("yyyy-MM-dd");
                    endDate = date.AddDays(1).ToString("yyyy-MM-dd");
                    sta = 3;
                    cont3 = QrInteractiveManager.getLearnStCount(StudyDeviceId, startDate, endDate);
                    cont3 /= 15; cont3 += 1;
                    List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(StudyDeviceId, startDate, endDate, 15, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 4) {
                    startDate = date.AddMonths(-1).ToString("yyyy-MM-dd");
                    endDate = date.AddDays(1).ToString("yyyy-MM-dd");
                    sta = 4;
                    cont4 = QrInteractiveManager.getLearnStCount(StudyDeviceId, startDate, endDate);
                    cont4 /= 15; cont4 += 1;
                    List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getLearnSt(StudyDeviceId, startDate, endDate, 15, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else if (state == 5) {
                    endDate = date.AddDays(1).ToString("yyyy-MM-dd");
                    sta = 5;
                    cont5 = QrInteractiveManager.getLearnStCount(StudyDeviceId, endDate);
                    cont5 /= 15; cont5 += 1;
                    List<Models.StudentLearnSt> LearnSitation = QrInteractiveManager.getAllLearnSt(StudyDeviceId, endDate, 15, nyeshu);
                    Rep_today.DataSource = LearnSitation;//设置数据源
                    Rep_today.DataBind();//绑定数据
                } else {
                    Response.Write("<script>alert('设备ID为空');</script>");
                }
            }
            }
        }
    }
}