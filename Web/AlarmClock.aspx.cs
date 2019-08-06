using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Web.Script.Serialization;
using Models;
using Common;

namespace Web {
    public partial class AlarmClock : System.Web.UI.Page {
        public static string openId;
        WeChatUser wcu = null;
        VideoEntities ve = DBContextFactory.GetDbContext();

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
                try {
                    if(!IsPostBack){
                        if (Session["OpenIdE"] == null) {
                            if (!string.IsNullOrEmpty(Request.QueryString["code"])) {
                                string code = Convert.ToString(Request.QueryString["code"]);
                                string returnStr = getOpenId(code);
                                JObject json = JObject.Parse(returnStr);
                                openId = json["openid"].ToString();
                                Session["OpenIdE"] = openId;
                                string DeviceId = QrChildManager.GetChildDeviceId(openId);
                                Session["DeviceId"] = DeviceId;
                                List<AlarmClocks> mis = AlarmClocksLogic.Instance().GetAlarmClock(DeviceId);
                                RpList.DataSource = mis;
                                RpList.DataBind();
                            }
                        } else if (Session["OpenIdE"] != null) {
                            openId = Session["OpenIdE"].ToString();//得到OpenId  
                            string DeviceId = QrChildManager.GetChildDeviceId(openId);//得到token
                            if (DeviceId != "" || DeviceId != null) {
                                Session["DeviceId"] = DeviceId;
                                if (DeviceId.Length != 0) {
                                    List<AlarmClocks> Almc = AlarmClocksLogic.Instance().GetAlarmClock(DeviceId);
                                    RpList.DataSource = Almc;
                                    RpList.DataBind();
                                }
                            }
                        }
                    }
                    string deviceId = Session["DeviceId"].ToString();
                    List<AlarmClocks> miss = AlarmClocksLogic.Instance().GetAlarmClock(deviceId);
                    RpList.DataSource = miss;
                    RpList.DataBind();
                    
                } catch (Exception ex) {
                   // Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新登录\");</script>");
                    Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新进入\");</script>");
                    //Response.Write(" <script type=\"text/javascript\"> window.close(); </script>");
                    //Response.Write(" <script type=\"text/javascript\">history.go(-2);</script>");
                }
        }

        public string getrp(int state) {
            string i = string.Empty;
            if (state == 0) {
                i = "style=\"display:inline\";";
            } else if (state == 1) {
                i = " style=\"display:none\";";
            }
            return i;
        }
        public string getrp_g(int state) {
            string i = string.Empty;
            if (state == 0) {
                i = "style=\"display:none\";";
            } else if (state == 1) {
                i = "style=\"display:inline\";";
            }
            return i;
        }
        public string gettypes(string img) {
            string i = string.Empty;
            if (img.Equals("起床") || img.Equals("吃饭") || img.Equals("上学") || img.Equals("午休") || img.Equals("运动") || img.Equals("做作业") || img.Equals("洗澡") || img.Equals("睡觉")) {
                i = img;
            } else {
             
            }
            return i;
        }
        public string getimg(string img) {
            string str = string.Empty;
            if (img.Equals("起床") || img.Equals("吃饭") || img.Equals("上学") || img.Equals("午休") || img.Equals("运动") || img.Equals("做作业") || img.Equals("洗澡") || img.Equals("睡觉")) {
                str = "<img src='Img/" + img + "@2x.png' style=' width:87%;'/>";
            } else {
                str = "<img src='Img/自定义.png' style=' width:87%;'/>";
            }
            return str;
        }

        /// <summary>
        /// 获取Json值集合   
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public static string getOpenId(string code) {
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
        public bool SetState(string State, string ID) {
            int sta;
            if (State == "关闭") {
                sta = 0;
            } else {
                sta = 1;
            }
            int id = Convert.ToInt32(ID);
            return AlarmClocksLogic.UpdateState(sta, id);
        }
        //private void GetOpenId() {
        //    string _Code = Request.QueryString["code"];
        //    if (string.IsNullOrEmpty(_Code)) {
        //        string url = string.Format(POSTCODEURL + "?appid={0}&redirect_uri={1}&response_type=code&scope={2}#wechat_redirect", WeChatConfig.AppId, HttpUtility.UrlEncode(Request.Url.AbsoluteUri), SCOPE);
        //        Response.Redirect(url);
        //    } else {
        //        string url = string.Format(POSTTOKENURL + "?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatConfig.AppId, WeChatConfig.AppSecret, _Code);
        //        string str = Submit.HttpGet(url);
        //        JObject json = JObject.Parse(str);
        //        string OpenId = json["openid"].ToString();
        //        Session["OpenIdE"] = OpenId;
        //    }
        //}
    }
}