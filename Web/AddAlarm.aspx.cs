using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat;
using Me.Common.Data;

namespace Web {
    public partial class AddAlarm : System.Web.UI.Page {
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {

            }
        }

        /// <summary>
        /// 添加闹钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void But_xiugai_Click(object sender, EventArgs e) {
            //还有一个闹钟类型要添加
            string time = Request.Form.Get("RTime");//提醒时间
            string content = this.Text_Content.Text;//提醒名称
            string Repeat = Request.Form.Get("Repeat");//是否重复
            int[] weekDay = new int[7];
            for (int i = 0; i < CheckBoxList1.Items.Count; i++) {
                if (CheckBoxList1.Items[i].Selected == true) {
                    weekDay[i] = 1;
                }
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
            string Frequency = Request["count"];//拿到重复次数
            string Interval = Request.Form.Get("interval");//拿到闹钟间隔
            string openId = Convert.ToString(Session["openId"]);
            string xingeToken = QrChildManager.GetXingeToken(openId);//得到token
            if (xingeToken != "" && xingeToken != null) {
                //string DeviceId ="b84c0210ec00";
                string DeviceId = QrChildManager.GetDeviceId(xingeToken);//根据得到的token拿到用户表的DeviceId
                if (time != null && time != "" && content != null && content != "" && Repeat != null && Repeat != "" && Frequency != null && Frequency != "" && Interval != null && Interval != "") {
                    string sql = string.Format("insert AlarmClock (ClockTime,DeviceId,[Content],[Repeat],RepeatDate,Frequency,Interval) values ('{0}','{1}','{2}','{3}','{4}','{5}','{5}')", time, DeviceId, content, Repeat, weekStr, Frequency, Interval);
                    SqlHelper.ExecuteNonQuery(sql);
                    ClientScript.RegisterStartupScript(this.GetType(), "幼儿伴侣提醒您！", "<script>alert('添加成功！'); location.href='https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect';</script>");
                    JObject content_json = new JObject();
                    string[] dts = time.Split(':');
                    string hour=dts[0];
                    string mitner=dts[1];
                    content_json.Add("flag", 3);
                    content_json.Add("hour", hour);
                    content_json.Add("mitner", mitner);
                    content_json.Add("content", content);
                    content_json.Add("Repeat", Repeat);
                    content_json.Add("weekStr", weekStr);
                    content_json.Add("Frequency", Frequency);
                    content_json.Add("Interval", Interval);
                    content_json.Add("DeviceId", DeviceId);
                    //string js = "{\"Title\":\"推送消息\",\"Type\":13,\"OpenId\":\"" + openId + "\",\"Content\":" + content_json.ToString() + "}";
                    //Message ms = new Message("幼儿伴侣", js);
                    //string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    ////string returnStr = XinGePush.PushMsg(xingeToken, JsonConvert.SerializeObject(ms));
                    //JObject json = JObject.Parse(returnStr);
                    //returnStr = json["ret_code"].ToString();
                    //Response.Write(returnStr);

                    JObject ms_json = new JObject();
                    ms_json.Add("Title", "推送消息");
                    ms_json.Add("Type", 13);
                    ms_json.Add("OpenId", openId);
                    ms_json.Add("Content", content_json);//string content_str = "{\"flag\":\"" + 3 + "\",\"hour\":" + hour + ",\"mitner\":\"" + mitner + "\",\"content\":\"" + content + "\",\"YesandNo\":\"" + YesandNo + ",\"weekStr\":\"" + weekStr + "\",\"Frequency\":\"" + Frequency + "\",\"Interval\":\"" + Interval + "\",\"DeviceId\":\"" + DeviceId + "\"}";
                    //string js = "{\"Title\":\"推送消息\",\"Type\":13,\"OpenId\":\"" + openId + "\",\"Content\":\"" + content_str + "\"}";
                    Message ms = new Message("e家亲幼儿伴侣", ms_json.ToString());
                        string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                        JObject json = JObject.Parse(returnStr);
                        string returnCode = json["ret_code"].ToString();
                        if (returnCode == "0") {
                            //Response.Write(returnStr + ", xingeToken = " + xingeToken + ", js = " + ms_json.ToString());
                        }
                } else {
                    ClientScript.RegisterStartupScript(this.GetType(), "幼儿伴侣提醒您！", "<script>alert('请填写完整信息！')</script>");
                    return;
                }
            } else {
                ClientScript.RegisterStartupScript(this.GetType(), "幼儿伴侣提醒您！", "<script>alert('请先绑定设备！')</script>");
                return;
            }
        }
    }
}