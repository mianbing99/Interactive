using Common;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;
using WeChat;

namespace Web {
    public partial class AlarmClockE : System.Web.UI.Page {
        private string POSTCODEURL = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private string POSTTOKENURL = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private string SCOPE = "snsapi_base";
        public int ID;
        public static string openId;
        WeChatUser wcu = null;
        VideoEntities ve = DBContextFactory.GetDbContext();

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["OpenIdE"] == null) {
            //    GetOpenId();
            //}
            //openId = Session["OpenIdE"].ToString();//得到OpenId
            //wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == openId && x.State == true);
            ////xingeToken = QrChildManager.GetXingeToken(openId);//得到token
            //if (wcu.Token != "" || wcu.Token != null) {
            //    string DeviceId = wcu.DeviceId;//根据得到的token拿到用户表的DeviceId
            //    if (DeviceId != "" || DeviceId != null) {
            //        string StudyDeviceId = DeviceId;//根据拿到用户表的DeviceId匹配学习情况表里的DeviceId
            //        if (StudyDeviceId != null && StudyDeviceId.Length != 0) {
            //            List<AlarmClocks> Almc = AlarmClocksLogic.Instance().GetAlarmClock(DeviceId);
            //            Rep_operation.DataSource = Almc;
            //            Rep_operation.DataBind();
            //        }
            //    } else {
            //        ClientScript.RegisterStartupScript(this.GetType(), "e家亲提醒您！", "<script>alert('您暂时没有添加闹钟！')</script>");
            //        return;
            //    }
            //} else {
            //    ClientScript.RegisterStartupScript(this.GetType(), "e家亲提醒您！", "<script>alert('您暂时没有添加闹钟！')</script>");
            //    return;
            //} string sql = string.Format("select ClockTime,deviceId,[Content],[Repeat],RepeatDate,Frequency,Interval from AlarmClock where id='{0}'", ID);
            //DBHelper db = new DBHelper();
            //DataTable dt = db.GetDataTableBySql(sql);
            //string ClockTime = Convert.ToString(dt.Rows[0]["ClockTime"]);
            //string deviceId = Convert.ToString(dt.Rows[0]["deviceId"]);
            //string content = Convert.ToString(dt.Rows[0]["Content"]);
            //string Repeat = Convert.ToString(dt.Rows[0]["Repeat"]);
            //string weekStr = Convert.ToString(dt.Rows[0]["RepeatDate"]);
            //string Frequency = Convert.ToString(dt.Rows[0]["Frequency"]);
            //string Interval = Convert.ToString(dt.Rows[0]["Interval"]);
            ////获取字符串长度
            //int length = weekStr.Length;
            ////截取除最后一位的前面所有字符
            //weekStr = weekStr.Substring(0, length - 1);
            //string[] dts = ClockTime.Split(':');
            //string hour = dts[0];
            //string mitner = dts[1];
            //JObject contents = new JObject();
            //contents.Add("flag", 1);
            //contents.Add("hour", hour);
            //contents.Add("mitner", mitner);
            //contents.Add("deviceId", deviceId);
            //contents.Add("Repeat", Repeat);
            //contents.Add("content", content);
            //contents.Add("weekStr", weekStr);
            //contents.Add("Frequency", Frequency);
            //contents.Add("Interval", Interval);
            //JObject ms_json = new JObject();
            //ms_json.Add("Title", "推送消息");
            //ms_json.Add("Type", 13);
            //ms_json.Add("OpenId", openId);
            //ms_json.Add("Content", contents);
            //if (AlarmClocksLogic.Instance().DeleteClock(ID)) {
            //    ClientScript.RegisterStartupScript(this.GetType(), "e家亲提醒您！", "<script>alert('删除成功！'); location.href='https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect';</script>");
            //    //string js = "{\"Title\":\"推送消息\",\"Type\":13,\"OpenId\":\"" + openId + "\",\"Content\":\"" + content.ToString() + "\"}";
            //    //window.history.go(-1);
            //    Message ms = new Message("e家亲", ms_json.ToString());
            //    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
            //    JObject json = JObject.Parse(returnStr);
            //    returnStr = json["ret_code"].ToString();
            //} else {
            //    ClientScript.RegisterStartupScript(this.GetType(), "e家亲提醒您！", "<script>alert('删除失败！')</script>");
            }
        }
        private void GetOpenId() {
            string _Code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(_Code)) {
                string url = string.Format(POSTCODEURL + "?appid={0}&redirect_uri={1}&response_type=code&scope={2}#wechat_redirect", WeChatConfig.AppId, HttpUtility.UrlEncode(Request.Url.AbsoluteUri), SCOPE);
                Response.Redirect(url);
            } else {
                string url = string.Format(POSTTOKENURL + "?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatConfig.AppId, WeChatConfig.AppSecret, _Code);
                string str = Submit.HttpGet(url);
                JObject json = JObject.Parse(str);
                string OpenId = json["openid"].ToString();
                Session["OpenIdE"] = OpenId;
            }
        }
    }
}