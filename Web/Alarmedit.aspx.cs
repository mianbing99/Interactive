using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;
using WeChat;

namespace Web {

    public partial class Alarmedit : System.Web.UI.Page {
        public int ID;
        public int state = 0;
        public string stat;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (!string.IsNullOrEmpty(Request.QueryString["tj"])) {
                    stat = "true";
                } else {
                    Response.Write(" <script type=\"text/javascript\">window.alert = function(name){var iframe = document.createElement(\"IFRAME\");iframe.style.display=\"none\"; iframe.setAttribute(\"src\", 'data:text/plain,');document.documentElement.appendChild(iframe);window.frames[0].window.alert(name);iframe.parentNode.removeChild(iframe);}</script>");
                    if (Session["DeviceId"] != null) {
                        string DeviceId = Context.Session["DeviceId"].ToString();//"o_w1Kw8Uxh4dEoRDCe-HNYnyYlhY";//
                        if (!string.IsNullOrEmpty(Request.QueryString["id"])) { //判断上个页面参数是否为空
                            ID = Convert.ToInt32(Request.QueryString["id"]);
                            AlarmClocks mis = AlarmClocksLogic.getAllgoodID(ID);  //根据deviceid和闹钟名字查询闹钟信息  闹钟名字唯一
                            if (mis.Id != 0) {
                                Text_ClockTime.Value = mis.ClockTime;
                                //Text_ClockTime.Value = mis.ClockTime;  //给文本框添加数据库查询到的值
                                Text_content.Value = mis.Content;
                                string type = mis.AlarmType.ToString();
                                if (type.Equals("起床") || type.Equals("吃饭") || type.Equals("上学") || type.Equals("午休") || type.Equals("运动") || type.Equals("做作业") || type.Equals("洗澡") || type.Equals("睡觉")) {
                                    Drop_type.SelectedValue = type;
                                } else {
                                    Drop_type.SelectedValue = "自定义";
                                    Text_zidingyi.Text = type;
                                }

                                if (mis.Repeat == "0") {
                                    Drop_repater.Items.FindByValue("0").Selected = true;
                                } else {
                                    Drop_repater.Items.FindByValue("1").Selected = true;
                                }
                                string arroy = Convert.ToString(mis.RepeatDate);
                                string[] weekArr = arroy.Split(',');//分割字符串
                                for (int i = 0; i < Chbke_Week.Items.Count; i++) {
                                    if (weekArr[i].Equals("0")) {
                                        Chbke_Week.Items[i].Selected = false;
                                    } else {
                                        Chbke_Week.Items[i].Selected = true;
                                    }
                                }
                            }
                        }

                    } else {
                        Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新登录\");</script>");
                        Response.Write(" <script type=\"text/javascript\">history.go(-2);</script>");
                    }
                    
                }
            }
            Response.Write(" <script type=\"text/javascript\">window.alert = function(name){var iframe = document.createElement(\"IFRAME\");iframe.style.display=\"none\"; iframe.setAttribute(\"src\", 'data:text/plain,');document.documentElement.appendChild(iframe);window.frames[0].window.alert(name);iframe.parentNode.removeChild(iframe);}</script>");
        }
        /// <summary>
        /// 修改的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void But_xiugai_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                AlarmClocks acs = new AlarmClocks();
                acs.Id = Convert.ToInt32(Request.QueryString["id"]);
                if (Text_ClockTime.Value == "" || Text_ClockTime.Value == null) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请选择时间！\");</script>");
                    return;
                }
                acs.ClockTime = Text_ClockTime.Value;
                string text = Convert.ToString(Drop_type.Text);
                if (text.Equals("自定义")) {
                    if (Text_zidingyi.Text == null || Text_zidingyi.Text == "") {
                        acs.AlarmType = "自定义";
                    } else {
                        acs.AlarmType = Text_zidingyi.Text;
                    }
                } else {
                    acs.AlarmType = text;
                }
               acs.Content = Text_content.Value;  
                acs.Repeat = Convert.ToString(Drop_repater.Text);
                int[] weekDay = new int[7];
                int num = 0;
                for (int i = 0; i < Chbke_Week.Items.Count; i++) {
                    if (Chbke_Week.Items[i].Selected == true) {
                        weekDay[i] = 1;
                    }
                    num += weekDay[i];
                }
                if(num<=0)
                {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请选择日期！\");</script>");
                    return;
                }
                string weekStr = "";
                for (int i = 0; i < weekDay.Length; i++) {
                    if (weekDay[i] == 1) {
                        weekStr += "1";
                    } else {
                        weekStr += "0";
                    }
                    weekStr += ",";
                }
                weekStr.Remove(weekStr.Length - 1);
                //获取字符串长度
                int length = weekStr.Length;
                //截取除最后一位的前面所有字符
                weekStr = weekStr.Substring(0, length - 1);
                acs.RepeatDate = weekStr;
                acs.State = 1;
                acs.Frequency = Convert.ToInt32(Drop_Frequency.SelectedValue);
                acs.Interval = Convert.ToInt32(Drop_Interval.SelectedValue);
                string openId = Convert.ToString(Session["OpenIdE"]);
                string xingeToken = QrChildManager.GetXingeToken(openId);//得到token
                string DeviceId = Context.Session["DeviceId"].ToString();//根据得到的token拿到用户表的DeviceId
                if (AlarmClocksLogic.updateAlarm(acs)) {
                    string[] dts = acs.ClockTime.Split(':');
                    string hour = dts[0];
                    string mitner = dts[1];
                    JObject content_json = new JObject();
                    content_json.Add("flag", 2);
                    content_json.Add("Id", acs.Id);
                    content_json.Add("content", acs.AlarmType);
                    content_json.Add("hour", hour);
                    content_json.Add("mitner", mitner);
                    content_json.Add("state", acs.State);
                    content_json.Add("AlarmType", acs.Content);
                    content_json.Add("Repeat", acs.Repeat);
                    content_json.Add("weekStr", acs.RepeatDate);
                    content_json.Add("Frequency", acs.Frequency);
                    content_json.Add("Interval", acs.Interval);
                    content_json.Add("DeviceId", DeviceId);
                    JObject ms_json = new JObject();
                    ms_json.Add("Title", "推送消息");
                    ms_json.Add("Type", 13);
                    ms_json.Add("OpenId", openId);
                    ms_json.Add("Content", content_json);
                    Response.Write(" <script type=\"text/javascript\"> alert(\"修改成功！\");</script>");
                    Response.Write(" <script type=\"text/javascript\">  window.location.href =\" AlarmClock.aspx\"</script>");
                    //string js = "{\"Title\":\"推送消息\",\"Type\":13,\"OpenId\":\"" + openId + "\",\"Content\":\"" + content_json.ToString() + "\"}";
                    Message ms = new Message("幼儿伴侣", ms_json.ToString());
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    //string returnStr = XinGePush.PushMsg(xingeToken, JsonConvert.SerializeObject(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                } else {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"修改失败！\");</script>");
                    Response.Write(" <script type=\"text/javascript\"> history.go(-2);</script>");
                }
            } else
            //添加事件
            {
                AlarmClocks acs = new AlarmClocks();
                if (Text_ClockTime.Value == "" || Text_ClockTime.Value == null) {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请选择时间！\");</script>");
                    Response.Write(" <script type=\"text/javascript\"> history.go(-1);</script>");
                    return;
                }
                acs.ClockTime = Text_ClockTime.Value;
                acs.Repeat = Convert.ToString(Drop_repater.Text);
                string text = Convert.ToString(Drop_type.Text);
                if (text.Equals("自定义")) {
                    if (Text_zidingyi.Text == null || Text_zidingyi.Text == "") {
                        acs.AlarmType = "自定义";
                    } else {
                        acs.AlarmType = Text_zidingyi.Text;
                    }
                } else {
                    acs.AlarmType = text;
                }
                int[] weekDay = new int[7];
                int num = 0;
                for (int i = 0; i < Chbke_Week.Items.Count; i++) {
                    if (Chbke_Week.Items[i].Selected == true) {
                        weekDay[i] = 1;
                    }
                    num += weekDay[i];
                }
                if(num<=0)
                {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"请选择日期！\");</script>");
                    Response.Write(" <script type=\"text/javascript\"> history.go(-1);</script>");
                    return;
                }
               acs.Content = Text_content.Value;  
                acs.State = 1;
                
                string weekStr = "";
                for (int i = 0; i < weekDay.Length; i++) {
                    if (weekDay[i] == 1) {
                        weekStr += "1";
                    } else {
                        weekStr += "0";
                    }
                    weekStr += ",";
                }
                weekStr.Remove(weekStr.Length - 1);
                //获取字符串长度
                int length = weekStr.Length;
                //截取除最后一位的前面所有字符
                weekStr = weekStr.Substring(0, length - 1);
                acs.RepeatDate = weekStr;
                acs.Frequency = Convert.ToInt32(Drop_Frequency.SelectedValue);
                acs.Interval = Convert.ToInt32(Drop_Interval.SelectedValue);
                string openId = Convert.ToString(Session["OpenIdE"]);
                string xingeToken = QrChildManager.GetXingeToken(openId);//得到token
                string DeviceId = Context.Session["DeviceId"].ToString();//根据得到的token拿到用户表的DeviceId
                if (DeviceId != "" || DeviceId != null) {
                    acs.DeviceId = DeviceId;
                   int  insertid=Convert.ToInt32(AlarmClocksLogic.insert_id(acs));
                   if (insertid != 0) {
                        string[] dts = acs.ClockTime.Split(':');
                        string hour = dts[0];
                        string mitner = dts[1];
                        JObject content_json = new JObject();
                        content_json.Add("flag", 3);
                        content_json.Add("Id", insertid);
                        content_json.Add("hour", hour);
                        content_json.Add("content", acs.AlarmType);
                        content_json.Add("mitner", mitner);
                        content_json.Add("state", acs.State);
                        content_json.Add("AlarmType", acs.Content);
                        content_json.Add("Repeat", acs.Repeat);
                        content_json.Add("weekStr", acs.RepeatDate);
                        content_json.Add("Frequency", acs.Frequency);
                        content_json.Add("Interval", acs.Interval);
                        content_json.Add("DeviceId", DeviceId);
                        JObject ms_json = new JObject();
                        ms_json.Add("Title", "推送消息");
                        ms_json.Add("Type", 13);
                        ms_json.Add("OpenId", openId);
                        ms_json.Add("Content", content_json);
                        Response.Write(" <script type=\"text/javascript\"> alert(\"添加成功！\");</script>");
                        Response.Write(" <script type=\"text/javascript\"> window.location.href =\" AlarmClock.aspx\"</script>");
                        //string js = "{\"Title\":\"推送消息\",\"Type\":13,\"OpenId\":\"" + openId + "\",\"Content\":\"" + content_json.ToString() + "\"}";
                        Message ms = new Message("幼儿伴侣", ms_json.ToString());
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                        JObject json = JObject.Parse(returnStr);
                        returnStr = json["ret_code"].ToString();
                    } else {
                       
                       Response.Write(" <script type=\"text/javascript\"> alert(\"添加失败！\");</script>");
                       Response.Write(" <script type=\"text/javascript\"> history.go(-2);</script>");
                    }
                } else {
                    Response.Write(" <script type=\"text/javascript\"> alert(\"登录过期,请重新登录\");</script>");
                    Response.Write(" <script type=\"text/javascript\"> history.go(-2);</script>");
                }

            }
        }

        protected void But_quxiao_Click(object sender, EventArgs e) {
            Response.Write("<script language=javascript>window.location.href =\" AlarmClock.aspx\";</script>");
        }

        protected void But_delete_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                if (!string.IsNullOrEmpty(Session["OpenIdE"].ToString())) {
                    string openId = Convert.ToString(Session["OpenIdE"]);
                    string xingeToken = QrChildManager.GetXingeToken(openId);//得到token
                    string DeviceId = QrChildManager.GetDeviceId(xingeToken);
                    if (AlarmClocksLogic.deleteAlarm(id)) {
                        JObject content_json = new JObject();
                        content_json.Add("flag", 1);
                        content_json.Add("Id", id);
                        JObject ms_json = new JObject();
                        ms_json.Add("Title", "推送消息");
                        ms_json.Add("Type", 13);
                        ms_json.Add("OpenId", openId);
                        ms_json.Add("Content", content_json);
                        Response.Write(" <script type=\"text/javascript\"> alert(\"删除成功！\");</script>");
                        Response.Write(" <script type=\"text/javascript\"> window.location.href =\" AlarmClock.aspx\";</script>");
                        Message ms = new Message("幼儿伴侣", ms_json.ToString());
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                        JObject json = JObject.Parse(returnStr);
                        returnStr = json["ret_code"].ToString();
                    }
                }

            }
        }
    }
}