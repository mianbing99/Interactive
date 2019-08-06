using Common;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using Me.Common.Data;

namespace WeChat {
    public class MessageChild {
        //定义Token
        public string ToKen = "";
        //调用数据库VideoEntities表
        VideoEntities ve = DBContextFactory.GetDbContext();
        //序列化对象
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        Device_user device_user = new Device_user();
        //返回消息
        public string ReturnMessageChild(string postStr) {
            string ResponseContent = "";
            XmlDocument xmldoc = new XmlDocument();  
            xmldoc.Load(new MemoryStream(Encoding.UTF8.GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null) {
                switch (MsgType.InnerText.ToLower()) {
                    case "event":
                        ResponseContent = EventHandler(xmldoc);//事件处理
                        break;
                    case "text":
                        ResponseContent = TextHandler(xmldoc);//接受文本消息处理
                        break;
                    case "image":
                        ResponseContent = ImageHandler(xmldoc);//接受图片消息处理
                        break;
                    case "voice":
                        ResponseContent = VoiceHandler(xmldoc);//接受语音消息处理 
                        break;
                    case "video":
                        ResponseContent = VideoHandler(xmldoc);//接受视频消息处理
                        break;
                    case "shortvideo":
                        ResponseContent = VideoHandler(xmldoc);//接受视频消息处理
                        break;
                    default:
                        break;
                }
            }
            return ResponseContent;
        }

        //事件
        public string EventHandler(XmlDocument xmldoc) {
            string responseContent = "hello";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            if (Event != null) {
                StringBuilder str = new StringBuilder();
                string openId = FromUserName.InnerText;
                string xingeToken = "";
                switch (Event.InnerText.ToLower()) {
                    case "subscribe":
                        str.Append("欢迎使用微信物联智能学习陪伴机器人系统；");
                        if (!string.IsNullOrEmpty(EventKey.InnerText)) {
                            string SceneId = EventKey.InnerText;
                            SceneId = SceneId.Split('_')[1];
                            str.Append(BindDevice(openId, SceneId));
                        } else {
                            str.Append(BindWeChat(openId));
                        }
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                              FromUserName.InnerText,
                                              ToUserName.InnerText,
                                              DateTime.Now.Ticks,
                                              str.ToString());
                        break;
                    case "unsubscribe":
                        xingeToken = QrChildManager.GetXingeToken(openId);
                        if (xingeToken != null && xingeToken.Length != 0) {
                            QrChildManager.DeleteOpenId(openId);
                            string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                            Message ms = new Message("e家亲幼儿伴侣", js);
                            PushHelper.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                        }
                        break;
                    case "scan":
                        try {
                            str.Append("欢迎使用微信物联智能学习陪伴机器人系统；");
                            if (!string.IsNullOrEmpty(EventKey.InnerText)) {
                                string SceneId = EventKey.InnerText;
                                str.Append(BindDevice(openId, SceneId));
                            } else {
                                str.Append(BindWeChat(openId));
                            }
                        } catch (Exception) {
                            str = new StringBuilder("请勿重复扫描二维码绑定设备！");
                            throw;
                        }
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                              FromUserName.InnerText,
                                              ToUserName.InnerText,
                                              DateTime.Now.Ticks,
                                              str.ToString());
                        break;
                    case "click":
                        //菜单单击事件
                        if (EventKey.InnerText.Equals("V1001_GOOD"))//点击视频抓拍选项
                        {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 10)) {
                                    showStr = "视频抓拍请求已发送，等待回复（如果远程设备无响应，则无回复信息）";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }

                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1002_GOOD"))//点击远程定位选项
                        {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 11)) {
                                    showStr = "定位请求已发送，等待回复（如果远程设备无响应，则无回复信息）";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }

                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1003_GOOD"))//点击一键抓拍选项
                        {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 8)) {
                                    showStr = "图片抓拍请求已发送，等待回复（如果远程设备无响应，则无回复信息）";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }

                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1004_GOOD")) {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 9)) {
                                    showStr = "正在截图，请稍候.../:sun";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1005_GOOD")) {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 12)) {
                                    showStr = "锁屏成功.../:sun";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }

                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1006_GOOD")) {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            if (xingeToken != null && xingeToken.Length != 0) {
                                string showStr = "";
                                if (QrChildManager.ManageMenu(openId, xingeToken, 14)) {
                                    showStr = "解锁成功.../:sun";
                                } else {
                                    showStr = "请求失败，请稍后重试！";
                                }

                                responseContent = string.Format(ReplyTyper.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1007_GOOD")) {
                            xingeToken = QrChildManager.GetXingeToken(openId);
                            string DeviceId = QrChildManager.GetDeviceId(xingeToken);
                            if (DeviceId != null && DeviceId.Length != 0) {
                                //string showStr = "该功能暂未开放！敬请期待/:hug";

                                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/StudyStation.aspx?DeviceId=" + DeviceId +"&response_type=code&scope=snsapi_base&state=1#wechat_redirect";

                                System.Web.HttpContext.Current.Response.Redirect(url);
                                //Response.Redirect("http://v.icoxtech.com/StudyStation.aspx?DeviceId='DeviceId'");
                                //responseContent = string.Format(ReplyTyper.Message_Text,
                                //FromUserName.InnerText,
                                //ToUserName.InnerText,
                                //DateTime.Now.Ticks,
                                //showStr);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        } else if (EventKey.InnerText.Equals("V1008_GOOD")) {
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   qhsb(openId));
                        }
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }

        public string qhsb(string openId) {
            List<Dictionary<string, string>> list = QrChildManager.GetDevices(openId);
            if (list.Count == 0) {
                return "你还未绑定设备/:bome";
            } else {
                for (int i = 0; i < list.Count; i++) {
                    if (list[i]["state"].ToLower() == "true") {
                        QrChildManager.UpdateState(list[i]["deviceId"], "0", openId);
                        if (i == list.Count - 1) {
                            QrChildManager.UpdateState(list[0]["deviceId"], "1", openId);
                            return "切换设备成功，当前设备为" + list[0]["deviceName"];
                        } else {
                            QrChildManager.UpdateState(list[i + 1]["deviceId"], "1", openId);
                            return "切换设备成功，当前设备为" + list[i + 1]["deviceName"];
                        }
                    }
                }
            }

            return "";
        }
        //接受文本消息
        public string TextHandler(XmlDocument xmldoc) {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");

            //WriteLog("ToUserName:" + ToUserName.InnerText + "---FromUserName:" + FromUserName.InnerText + "---Content:" + Content.InnerText);
            if (Content != null) {
                string str = Content.InnerText;
                if (!str.StartsWith("@")) {
                    string openId = FromUserName.InnerText;
                    //DBHelper.ExecuteNonQueryString("insert into xmlString values('{" + BindWeChat(openId) + "}')");
                    if (BindWeChat(openId) == "") {
                        WriteLog("请先绑定设备:" + openId);
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                        FromUserName.InnerText,
                                                        ToUserName.InnerText,
                                                        DateTime.Now.Ticks,
                                                        "请先绑定设备！/::)");
                    } else if (str.StartsWith("绑定设备") || str.StartsWith("修改设备名称") || str.StartsWith("切换设备")) {
                        Regex regex = new Regex("^绑定设备\\d{11}$");
                        if (regex.IsMatch(str)) {
                            string phone = str.Substring(4, 11);
                            QrChildManager.UpdatePhone(openId, phone);
                            string deviceName2 = QrChildManager.GetChildDeviceName(openId);
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "恭喜您绑定设备成功！当前设备名为：" + deviceName2 + "\n在下方回复：“修改设备名称+您指定的设备名称”\n如：修改设备名称大儿子\n即可修改设备名称便于多设备管理！");
                            
                        } else {
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "输入格式有误，请重新输入！");
                        }
                        if (str.StartsWith("修改设备名称")) {
                            string deviceName3 = str.Substring(6, str.Length - 6);
                            if (QrChildManager.UpdateDeviceName(openId, deviceName3)) {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "修改设备名称成功，当前设备名称为：" + deviceName3 + "\n在下方回复：“切换设备+设备名称”\n如：切换设备大儿子\n即可切换到设备大儿子！");
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "修改设备名称失败，设备名称有误！");
                            }
                        }
                        if (str.StartsWith("切换设备")) {
                            string deviceName4 = str.Substring(4, str.Length - 4);
                            if (QrChildManager.GetD(openId, deviceName4)) {
                                QrChildManager.UpdateState2(openId, deviceName4);
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "切换设备成功，当前设备为：" + deviceName4);
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                                            FromUserName.InnerText,
                                                            ToUserName.InnerText,
                                                            DateTime.Now.Ticks,
                                                            "切换设备失败，设备名称有误！");
                            }
                        }
                        
                        return responseContent;
                    }
                    string xingeToken = QrChildManager.GetXingeToken(openId);
                    string deviceName = QrChildManager.GetChildDeviceName(openId);
                    if (xingeToken != null && xingeToken.Length != 0) {
                        //str = str.Substring(1, str.Length - 1);
                        string js = "{\"Title\":\"推送消息\",\"Type\":1,\"OpenId\":\"" + openId + "\",\"Content\":\"" + str + "\"}";
                        Message ms = new Message("e家亲幼儿伴侣", js);
                        string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                        JObject json = JObject.Parse(returnStr);
                        returnStr = json["ret_code"].ToString();
                        if (returnStr == "0") {
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                                                      FromUserName.InnerText,
                                                                      ToUserName.InnerText,
                                                                      DateTime.Now.Ticks,
                                                                      "你向设备【" + deviceName + "】推送消息成功!/:sun");
                        } else {
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                                         FromUserName.InnerText,
                                                         ToUserName.InnerText,
                                                         DateTime.Now.Ticks,
                                                         "推送消息失败！/:bome");
                        }
                    } else {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                        FromUserName.InnerText,
                                                        ToUserName.InnerText,
                                                        DateTime.Now.Ticks,
                                                        "请先绑定设备！/::)");
                    }
                } else {
                    responseContent = HandleOther(str, FromUserName.InnerText, ToUserName.InnerText);
                }
            }
            return responseContent;
        }

        //图灵
        private string TULINKEY = "a6629457597eb511cf75a67e6fe19607";
        /// <summary>
        /// 机器人
        /// </summary>
        /// <param name="requestContent"></param>
        /// <returns></returns>
        private string HandleOther(string str, string fromuser, string touser) {
            string response = string.Empty;
            string tempstr = string.Empty;
            string url = string.Format("http://www.tuling123.com/openapi/api?key={0}&info={1}", TULINKEY, str);
            string responseStr = Submit.HttpGet(url);
            JObject json = JObject.Parse(responseStr);
            string code = json["code"].ToString();
            if (!string.IsNullOrEmpty(code)) {
                int type = Convert.ToInt32(code);
                switch (type) {
                    case 100000:
                        response = string.Format(ReplyTyper.Message_Text,
                                            fromuser,
                                            touser,
                                            DateTime.Now.Ticks,
                                            (json["text"] + "").Replace("<br>", "\n"));
                        break;
                    case 200000:
                        response = string.Format(ReplyTyper.Message_Text,
                                         fromuser,
                                         touser,
                                         DateTime.Now.Ticks,
                                         json["text"] + "\n" + json["url"]);
                        break;
                    case 302000:
                        tempstr = "";
                        JToken jtk302 = json["list"];
                        for (int i = 0; i < jtk302.Count(); i++) {
                            tempstr += string.Format(ReplyTyper.Message_News_Item,
                                                     jtk302[i]["article"],
                                                     jtk302[i]["source"],
                                                     jtk302[i]["icon"],
                                                     jtk302[i]["detailurl"]);
                        }
                        response = string.Format(ReplyTyper.Message_News_Main,
                                        fromuser,
                                        touser,
                                        DateTime.Now.Ticks,
                                        jtk302.Count(),
                                          tempstr);
                        break;
                    case 305000:
                        tempstr = "";
                        JToken jtk305 = json["list"];
                        tempstr += json["text"] + "\n\n";
                        for (int i = 0; i < jtk305.Count(); i++) {
                            tempstr += "车次：" + jtk305[i]["trainnum"] + "\n";
                            tempstr += "起始站：" + jtk305[i]["start"] + "\n";
                            tempstr += "到达站：" + jtk305[i]["terminal"] + "\n";
                            tempstr += "开车时间：" + jtk305[i]["starttime"] + "\n";
                            tempstr += "到达时间：" + jtk305[i]["endtime"] + "\n";
                            if (i < jtk305.Count() - 1) {
                                tempstr += "\n";
                            }
                        }
                        response = string.Format(ReplyTyper.Message_Text,
                                              fromuser,
                                              touser,
                                              DateTime.Now.Ticks,
                                             tempstr);
                        break;
                    case 308000:
                        tempstr = "";
                        JToken jtk308 = json["list"];
                        tempstr += json["text"] + "\n\n";
                        for (int i = 0; i < jtk308.Count(); i++) {
                            tempstr += "名称：" + jtk308[i]["name"] + "\n";
                            tempstr += "详情：" + jtk308[i]["info"] + "\n";
                            if (i < jtk308.Count() - 1) {
                                tempstr += "\n";
                            }
                        }
                        response = string.Format(ReplyTyper.Message_Text,
                                              fromuser,
                                              touser,
                                              DateTime.Now.Ticks,
                                             tempstr);
                        break;
                    default:
                        response = string.Format(ReplyTyper.Message_Text,
                                           fromuser,
                                           touser,
                                           DateTime.Now.Ticks,
                                           json["code"] + "");
                        break;
                }
            }
            return response;
        }


        //接受图片消息 备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string ImageHandler(XmlDocument xmldoc) {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode PicUrl = xmldoc.SelectSingleNode("/xml/PicUrl");
            if (PicUrl != null) {
                string openId = FromUserName.InnerText;
                if (BindWeChat(openId) == "") {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
                string xingeToken = QrChildManager.GetXingeToken(openId);
                string deviceName = QrChildManager.GetChildDeviceName(openId);
                if (xingeToken != null && xingeToken.Length != 0) {
                    string js = "{\"url\":\"" + PicUrl.InnerText + "\",\"format\":\"" + "png" + "\"}";
                    js = "{\"Title\":\"推送图片\",\"Type\":2,\"OpenId\":\"" + openId + "\",\"Content\":" + js + "}";
                    Message ms = new Message("幼儿伴侣", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0") {
                        responseContent = string.Format(ReplyTyper.Message_News_Main,
                                                   FromUserName.InnerText,
                                                   ToUserName.InnerText,
                                                   DateTime.Now.Ticks,
                                                   "1",
                                                   string.Format(ReplyTyper.Message_News_Item, "智能学习陪伴机器人", "你向设备【" + deviceName + "】推送了一张图片！",
                                                   PicUrl.InnerText, ""));
                    } else {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送图片失败！/:bome");
                    }
                } else {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }
        //接受语音消息  备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string VoiceHandler(XmlDocument xmldoc) {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode Format = xmldoc.SelectSingleNode("/xml/Format");
            if (MediaId != null) {
                string openId = FromUserName.InnerText;
                if (BindWeChat(openId) == "") {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
                string xingeToken = QrChildManager.GetXingeToken(openId);
                string deviceName = QrChildManager.GetChildDeviceName(openId);
                if (xingeToken != null && xingeToken.Length != 0) {
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, MediaId.InnerText);
                    string js = "{\"url\":\"" + url + "\",\"format\":\"" + Format.InnerText + "\"}";
                    js = "{\"Title\":\"推送语音\",\"Type\":3,\"OpenId\":\"" + openId + "\",\"Content\":" + js + "}";
                    Message ms = new Message("幼儿伴侣", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string str = jsonSerializer.Serialize(ms);
                    string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0") {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                      FromUserName.InnerText,
                                                      ToUserName.InnerText,
                                                      DateTime.Now.Ticks,
                                                      "你向设备【" + deviceName + "】推送了一段语音！");
                    } else {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送语音失败！/:bome");
                    }
                } else {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }

        //接受视频消息  备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string VideoHandler(XmlDocument xmldoc) {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode ThumbMediaId = xmldoc.SelectSingleNode("/xml/ThumbMediaId");
            if (MediaId != null) {
                string openId = FromUserName.InnerText;
                if (BindWeChat(openId) == "") {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
                string xingeToken = QrChildManager.GetXingeToken(openId);
                string deviceName = QrChildManager.GetChildDeviceName(openId);
                if (xingeToken != null && xingeToken.Length != 0) {
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, MediaId.InnerText);
                    string img = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, ThumbMediaId.InnerText);
                    string js = "{\"url\":\"" + url + "\",\"img\":\"" + img + "\"}";
                    js = "{\"Title\":\"推送视频\",\"Type\":4,\"OpenId\":\"" + openId + "\",\"Content\":" + js + "}";
                    Message ms = new Message("幼儿伴侣", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0") {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                      FromUserName.InnerText,
                                                      ToUserName.InnerText,
                                                      DateTime.Now.Ticks,
                                                      "你向设备【" + deviceName + "】推送了一段视频！");
                    } else {
                        responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送视频失败！/:bome");
                    }
                } else {
                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }

        public void WriteLog(string text) {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
        /// <summary>
        /// 添加幼儿在线状态记录
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="studyName"></param>
        /// <returns></returns>
        public static void AddStudyRecord(string deviceId, string studyName) {
            string sql = string.Format("insert [dbo].[LearnSituation] (DeviceId,StudyName) values ('{0}','{1}') ", deviceId, studyName);
            SqlHelper.ExecuteNonQuery(sql,null,CommandType.Text);
        }
    
        
        public string BindDevice(string openId, string sendId) {
            StringBuilder str = new StringBuilder();
            Dictionary<string, object> dic = QrChildManager.OldBind(sendId);
            string BindToken = Convert.ToString(dic["token"]);
            string deviceId = Convert.ToString(dic["deviceId"]);
            if (BindToken != null && BindToken != "") {
                str.Append("\n请在下方回复“绑定设备+手机号”\n如：绑定设备13212345678\n即可激活设备！");
                
                if (!QrChildManager.OldChildUser(openId, BindToken, deviceId)) {
                    str.Append("\n你已绑定此设备无需重复绑定！");
                    return str.ToString();
                }
                SendNews(deviceId, openId);
                string js = "";
                Message ms = null;
                js = "{\"Title\":\"绑定推送\",\"Type\":6,\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                ms = new Message("e家亲幼儿伴侣", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                string returnStr = XinGePush.PushMsg(BindToken, jsonSerializer.Serialize(ms));
                JObject json = JObject.Parse(returnStr);
                returnStr = json["ret_code"].ToString();
            }
            return str.ToString();
        }
        public void SendNews(string deviceId, string openId) {
            DataTable dt = QrChildManager.GetOpenId(deviceId);
            Device_user du = new Device_user();
            Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
            List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
            Dictionary<string, string> dic3 = new Dictionary<string, string>();
            dic3.Add("openid", openId);
            dic3.Add("lang", "zh_CN");
            list2.Add(dic3);
            dic2.Add("user_list", list2);
            string data2 = du.GetUserInformation(JsonConvert.SerializeObject(dic2));
            Dictionary<string, List<Dictionary<string, object>>> list3 = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(data2);
            string user = "";
            foreach (var item in list3["user_info_list"]) {
                if (item["openid"].ToString() == openId) {
                    user = item["nickname"].ToString();
                }
            }
            foreach (DataRow row in dt.Rows) {
                string content = string.Format("欢迎成员{0}加入！", user);
                string data = "{\"touser\":\"" + row["OpenId"] + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
                du.SendNews(data);
            }
        }
        private string BindWeChat(string openId) {
            Device_user du = new Device_user();
            string device = QrChildManager.GetChildDeviceId(openId);
            if (device == null && device == "") {
                return "";
            }
            return "1";
        }
    }


    //回复类型
    public class ReplyTyper {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text {
            get {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }

        public static string Message_Device {
            get {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[{3}]]></MsgType>
                            <Event><![CDATA[{4}]]></Event>
                            <DeviceType><![CDATA[{5}]]></DeviceType>
                            <DeviceID><![CDATA[{6}]]></DeviceID>
                            <Content><![CDATA[{7}]]></Content>
                            <SessionID>{8}</SessionID>
                            <OpenID><![CDATA[{9}]]></OpenID>
                        </xml>";
            }
        }
        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main {
            get {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{3}</ArticleCount>
                            <Articles>
                            {4}
                            </Articles>
                            </xml> ";
            }
        }
        /// <summary>
        /// 图文消息项
        /// </summary>
        public static string Message_News_Item {
            get {
                return @"<item>
                            <Title><![CDATA[{0}]]></Title> 
                            <Description><![CDATA[{1}]]></Description>
                            <PicUrl><![CDATA[{2}]]></PicUrl>
                            <Url><![CDATA[{3}]]></Url>
                            </item>";
            }
        }
    }
}
