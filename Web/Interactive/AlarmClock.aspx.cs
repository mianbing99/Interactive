using Common;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat;

namespace Web.Interactive
{
    public partial class AlarmClock : System.Web.UI.Page
    {
        public static string openId;
        WeChatUser wcu = null;
        VideoEntities ve = DBContextFactory.GetDbContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["openId"] = "ocpAZ1hp_Egx33pvVie1Y4Fl3zik";
            try
            {
                if (!IsPostBack)
                {
                    if (Session["openId"] == null)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                        {
                            string code = Convert.ToString(Request.QueryString["code"]);
                            string returnStr = getOpenId(code);
                            JObject json = JObject.Parse(returnStr);
                            openId = json["openid"].ToString();
                            Session["openId"] = openId;
                            string DeviceId = ve.StudentDeviceBind.First(s=>s.openId==openId).deviceId;
                            List<StudentAlarmClock> mis = ve.StudentAlarmClock.Where(s=>s.deviceId==DeviceId).ToList();
                            RpList.DataSource = mis;
                            RpList.DataBind();
                        }
                    }
                }
                if (Session["openId"] != null)
                {
                    openId = Session["openId"].ToString();
                    string deviceId = ve.StudentDeviceBind.First(s => s.openId == openId & s.state == true & s.userPhone != null).deviceId;
                    List<StudentAlarmClock> miss = ve.StudentAlarmClock.Where(s=>s.deviceId==deviceId).ToList();
                    RpList.DataSource = miss;
                    RpList.DataBind();
                }
                if(Convert.ToString(Request["type"])=="updateState")
                {
                    SetState(Request["state"],Request["id"]);
                }
            }
            catch (Exception ex)
            {
                // Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新登录\");</script>");
                Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新进入\");</script>");
                //Response.Write(" <script type=\"text/javascript\"> window.close(); </script>");
                //Response.Write(" <script type=\"text/javascript\">history.go(-2);</script>");
            }
        }

        public string getrp(int state)
        {
            string i = string.Empty;
            if (state == 0)
            {
                i = "style=\"display:inline\";";
            }
            else if (state == 1)
            {
                i = " style=\"display:none\";";
            }
            return i;
        }
        public string getrp_g(int state)
        {
            string i = string.Empty;
            if (state == 0)
            {
                i = "style=\"display:none\";";
            }
            else if (state == 1)
            {
                i = "style=\"display:inline\";";
            }
            return i;
        }
        public string gettypes(string img)
        {
            string i = string.Empty;
            if (img.Equals("起床") || img.Equals("吃饭") || img.Equals("上学") || img.Equals("午休") || img.Equals("运动") || img.Equals("做作业") || img.Equals("洗澡") || img.Equals("睡觉"))
            {
                i = img;
            }
            else
            {

            }
            return i;
        }
        public string getimg(string img)
        {
            string str = string.Empty;
            if (img.Equals("起床") || img.Equals("吃饭") || img.Equals("上学") || img.Equals("午休") || img.Equals("运动") || img.Equals("做作业") || img.Equals("洗澡") || img.Equals("睡觉"))
            {
                str = "<img src='/Img/" + img + "@2x.png' style=' width:87%;'/>";
            }
            else
            {
                str = "<img src='/Img/自定义.png' style=' width:87%;'/>";
            }
            return str;
        }

        /// <summary>
        /// 获取Json值集合   
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public static string getOpenId(string code)
        {
            string studyUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect";

            studyUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxf9ade153117809bf&secret=c876b01424956a2f7f7d1bee9dbb0acd&code=" + code + "&grant_type=authorization_code ";

            return HttpGet(studyUrl, "");
        }
        public static string HttpGet(string Url, string postDataStr)
        {
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
        public bool SetState(string State, string ID)
        {
            int sta;
            bool bo = false;
            if (State == "关闭")
            {
                sta = 0;
            }
            else
            {
                sta = 1;
            }
            int id = Convert.ToInt32(ID);
            var alarmClock = ve.StudentAlarmClock.FirstOrDefault(s => s.id == id);
            alarmClock.state = sta;
            string token = QrInteractiveManager.getDeviceToken(alarmClock.deviceId);
            if (ve.SaveChanges()>0) 
            {
                bo = true;
                JObject content_json = new JObject();
                content_json.Add("flag", 2);
                content_json.Add("Id", id);
                content_json.Add("state", sta);
                JObject ms_json = new JObject();
                ms_json.Add("Title", "推送消息");
                ms_json.Add("Type", 13);
                ms_json.Add("OpenId", openId);
                ms_json.Add("Content", content_json);
                Messages ms = new Messages("幼儿伴侣", ms_json.ToString());
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                SendHelper sendHelper = new SendHelper(2100339782, "A513K1EBXG7A", "c17ed4bad21f480013b649f1c91d7ead");
                string returnStr = sendHelper.PushMsg(token, jsonSerializer.Serialize(ms));
            }
            return bo;
        }
    }
}