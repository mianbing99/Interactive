using Common;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using Me.Common.Data;

namespace WeChat
{
    public class MessageHelp
    {
        public string ToKen = "";
        VideoEntities ve = DBContextFactory.GetDbContext();

        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //返回消息
        public string ReturnMessage(string postStr)
        {
            string responseContent = "";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new MemoryStream(Encoding.UTF8.GetBytes(postStr)));
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType != null)
            {
                switch (MsgType.InnerText.ToLower())
                {
                    case "event":
                        responseContent = EventHandle(xmldoc);//事件处理
                        break;
                    case "text":
                        responseContent = TextHandle(xmldoc);//接受文本消息处理
                        break;
                    case "image":
                        responseContent = ImageHandle(xmldoc);//接受图片消息处理
                        break;
                    case "voice":
                        responseContent = VoiceHandle(xmldoc);//接受语音消息处理
                        break;
                    case "video":
                        responseContent = VideoHandle(xmldoc);//接受视频消息处理
                        break;
                    case "shortvideo":
                        responseContent = VideoHandle(xmldoc);//接受视频消息处理
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
        //事件
        public string EventHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            XmlNode EventKey = xmldoc.SelectSingleNode("/xml/EventKey");
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");        
            if (Event != null)
            {
                StringBuilder str = new StringBuilder();
                WeChatUser wcu = null;
                switch (Event.InnerText.ToLower())
                {
                    case "subscribe":
                        str.Append("欢迎关注金龙锋【e家亲】服务号/:hug");
                        if (!string.IsNullOrEmpty(EventKey.InnerText))
                        {
                            str.Append("\n");
                            string SceneId = EventKey.InnerText;
                            SceneId = SceneId.Split('_')[1];
                            str.Append(BindWeChat(FromUserName.InnerText, SceneId));
                        }
                        responseContent = string.Format(ReplyType.Message_Text,
                                          FromUserName.InnerText,
                                          ToUserName.InnerText,
                                          DateTime.Now.Ticks,
                                          str.ToString());
                        break;
                    case "unsubscribe":
                        //取消关注
                        wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText);
                        ve.WeChatUser.RemoveRange(ve.WeChatUser.Where(q => q.OpenId == FromUserName.InnerText));
                        ve.SaveChanges();
                        if (wcu != null)
                        {
                            string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"\"}";
                            Message ms = new Message("e家亲", js);
                            PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                        }
                        break;
                    case "scan":
                        try {
                            str.Append("欢迎关注金龙锋【e家亲】服务号/:hug");
                            if (!string.IsNullOrEmpty(EventKey.InnerText)) {
                                string SceneId = EventKey.InnerText;
                                str.Append(BindWeChat(FromUserName.InnerText, SceneId));
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
                        if (EventKey.InnerText.Equals("V1001_GOOD"))//点击视频抓拍选项
                        {
                            //DBHelper.ExecuteNonQueryString("insert into xmlString values('" + FromUserName.InnerText + "')");
                            try {
                                wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                                if (wcu != null) {
                                    string showStr = "";
                                    string js = "{\"Title\":\"推送消息\",\"Type\":10,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"\"}";
                                    Message ms = new Message("e家亲", js);
                                    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                                    JObject json = JObject.Parse(returnStr);
                                    returnStr = json["ret_code"].ToString();
                                    if (returnStr == "0") {
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
                                    responseContent = string.Format(ReplyType.Message_Text,
                                       FromUserName.InnerText,
                                       ToUserName.InnerText,
                                       DateTime.Now.Ticks,
                                       "你还未绑定设备/:bome");
                                }
                            } catch (Exception ex) {
                              SqlHelper.ExecuteNonQuery(  "insert into xmlString values('" + ex.Message + "')",null,System.Data.CommandType.Text);
                            }
                            
                        } else if (EventKey.InnerText.Equals("V1002_GOOD"))//点击远程定位选项
                        {
                            wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                            if (wcu != null) {
                                string showStr = "";
                                string js = "{\"Title\":\"推送消息\",\"Type\":11,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"\"}";
                                Message ms = new Message("e家亲", js);
                                string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                                JObject json = JObject.Parse(returnStr);
                                returnStr = json["ret_code"].ToString();
                                if (returnStr == "0") {
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
                            wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                            if (wcu != null) {
                                string showStr = "";
                                string js = "{\"Title\":\"推送消息\",\"Type\":8,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"\"}";
                                Message ms = new Message("e家亲", js);
                                string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                                JObject json = JObject.Parse(returnStr);
                                returnStr = json["ret_code"].ToString();
                                if (returnStr == "0") {
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
                        } else {
                            responseContent = string.Format(ReplyTyper.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "未知错误！");
                        }
                        //菜单单击事件
                        /*if (EventKey.InnerText.Equals("T12"))//click_one
                        {
                            wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText);
                            if (wcu != null)
                            {
                                responseContent = string.Format(ReplyType.Message_Text,
                                FromUserName.InnerText,
                                ToUserName.InnerText,
                                DateTime.Now.Ticks,
                                "你已绑定设备【" + wcu.Token + "】,可以正常使用推送功能！/:sun\n\n推送消息格式：@消息/::)");
                            }
                            else
                            {
                                responseContent = string.Format(ReplyType.Message_Text,
                                   FromUserName.InnerText,
                                   ToUserName.InnerText,
                                   DateTime.Now.Ticks,
                                   "你还未绑定设备/:bome");
                            }
                        }
                        else if (EventKey.InnerText.Equals("T13"))
                        {
                            var data = ve.Video.Where(q=>q.State==true).OrderByDescending(q=>q.CreateDate).Take(10);
                            string tempstr = "";
                            foreach (Video item in data)
                            {
                                tempstr += string.Format(ReplyType.Message_News_Item, 
                                    item.Title,
                                    "",
                                    item.Cover,
                                    item.Id);
                            }
                            responseContent = string.Format(ReplyType.Message_News_Main,
                                  FromUserName.InnerText,
                                  ToUserName.InnerText,
                                  DateTime.Now.Ticks,
                                  data.Count(),
                                 tempstr);
                        }*/
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
        //接受文本消息
        public string TextHandle(XmlDocument xmldoc) {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            //WriteLog("ToUserName:" + ToUserName.InnerText + "---FromUserName:" + FromUserName.InnerText + "---Content:" + Content.InnerText);
            if (Content != null) {
                string str = Content.InnerText;
                if (!str.StartsWith("@")) {
                    WeChatUser wcu = ve.WeChatUser.OrderByDescending(x => x.Id).FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                    if (wcu != null) {
                        if (str.StartsWith("绑定设备") || str.StartsWith("修改设备名称") || str.StartsWith("切换设备")) {
                            Regex regex = new Regex("^绑定设备\\d{11}$");
                            if (regex.IsMatch(str)) {
                                string phone = str.Substring(4, 11);
                                try {
                                    wcu.UserPhone = phone;
                                    ve.SaveChanges();
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                    FromUserName.InnerText,
                                                                    ToUserName.InnerText,
                                                                    DateTime.Now.Ticks,
                                                                    "恭喜您绑定设备成功！当前设备名为：" + wcu.DeviceName + "\n在下方回复：“修改设备名称+您指定的设备名称”\n如：修改设备名称大儿子\n即可修改设备名称便于多设备管理！");
                                } catch (Exception) {
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "绑定失败，请重新绑定！");
                                    return responseContent;
                                }
                            } else {
                                responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "输入格式有误，请重新输入！");
                            }
                            if (str.StartsWith("修改设备名称")) {
                                string deviceName3 = str.Substring(6, str.Length - 6);
                                try {
                                    wcu.DeviceName = deviceName3;
                                    ve.SaveChanges();
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "修改设备名称成功，当前设备名称为：" + wcu.DeviceName + "\n在下方回复：“切换设备+设备名称”\n如：切换设备大儿子\n即可切换到设备大儿子！");
                                } catch (Exception) {
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "修改设备名称失败，设备名称有误！");
                                    return responseContent;
                                }
                            }
                            if (str.StartsWith("切换设备")) {
                                string deviceName4 = str.Substring(4, str.Length - 4);
                                try {
                                    List<WeChatUser> list = ve.WeChatUser.Where(x => x.OpenId == ToUserName.InnerText).OrderBy(x => x.CreateDate).ToList();
                                    foreach (var item in list) {
                                        item.State = false;
                                        if (item.DeviceName == deviceName4) {
                                            item.State = true;
                                        }
                                    }
                                    ve.SaveChanges();
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "切换设备成功，当前设备为：" + wcu.DeviceName);
                                } catch (Exception) {
                                    responseContent = string.Format(ReplyTyper.Message_Text,
                                                                FromUserName.InnerText,
                                                                ToUserName.InnerText,
                                                                DateTime.Now.Ticks,
                                                                "切换设备失败，设备名称有误！");
                                    return responseContent;
                                }
                            }
                        } else {
                            string js = "{\"Title\":\"推送消息\",\"Type\":1,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"" + str + "\"}";
                            Message ms = new Message("e家亲", js);
                            string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                            JObject json = JObject.Parse(returnStr);
                            returnStr = json["ret_code"].ToString();
                            if (returnStr == "0") {
                                responseContent = string.Format(ReplyType.Message_Text,
                                                                          FromUserName.InnerText,
                                                                          ToUserName.InnerText,
                                                                          DateTime.Now.Ticks,
                                                                          "你向设备【" + wcu.DeviceName + "】推送消息成功!/:sun");
                            } else {
                                responseContent = string.Format(ReplyType.Message_Text,
                                                             FromUserName.InnerText,
                                                             ToUserName.InnerText,
                                                             DateTime.Now.Ticks,
                                                             "推送消息失败！/:bome");
                            }
                        }
                        
                    } else {
                        responseContent = string.Format(ReplyType.Message_Text,
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
        private string HandleOther(string str, string fromuser, string touser)
        {
            string response = string.Empty;
            string tempstr = string.Empty;
            string url = string.Format("http://www.tuling123.com/openapi/api?key={0}&info={1}", TULINKEY, str);
            string responseStr = Submit.HttpGet(url);
            JObject json = JObject.Parse(responseStr);
            string code = json["code"].ToString();
            if (!string.IsNullOrEmpty(code))
            {
                int type = Convert.ToInt32(code);
                switch (type)
                {
                    case 100000:
                        response = string.Format(ReplyType.Message_Text,
                                            fromuser,
                                            touser,
                                            DateTime.Now.Ticks,
                                            (json["text"] + "").Replace("<br>", "\n"));
                        break;
                    case 200000:
                        response = string.Format(ReplyType.Message_Text,
                                         fromuser,
                                         touser,
                                         DateTime.Now.Ticks,
                                         json["text"] + "\n" + json["url"]);
                        break;
                    case 302000:
                        tempstr = "";
                        JToken jtk302 = json["list"];
                        for (int i = 0; i < jtk302.Count(); i++)
                        {
                            tempstr += string.Format(ReplyType.Message_News_Item,
                                                     jtk302[i]["article"],
                                                     jtk302[i]["source"],
                                                     jtk302[i]["icon"],
                                                     jtk302[i]["detailurl"]);
                        }
                        response = string.Format(ReplyType.Message_News_Main,
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
                        for (int i = 0; i < jtk305.Count(); i++)
                        {
                            tempstr += "车次：" + jtk305[i]["trainnum"] + "\n";
                            tempstr += "起始站：" + jtk305[i]["start"] + "\n";
                            tempstr += "到达站：" + jtk305[i]["terminal"] + "\n";
                            tempstr += "开车时间：" + jtk305[i]["starttime"] + "\n";
                            tempstr += "到达时间：" + jtk305[i]["endtime"] + "\n";
                            if (i < jtk305.Count() - 1)
                            {
                                tempstr += "\n";
                            }
                        }
                        response = string.Format(ReplyType.Message_Text,
                                              fromuser,
                                              touser,
                                              DateTime.Now.Ticks,
                                             tempstr);
                        break;
                    case 308000:
                        tempstr = "";
                        JToken jtk308 = json["list"];
                        tempstr += json["text"] + "\n\n";
                        for (int i = 0; i < jtk308.Count(); i++)
                        {
                            tempstr += "名称：" + jtk308[i]["name"] + "\n";
                            tempstr += "详情：" + jtk308[i]["info"] + "\n";
                            if (i < jtk308.Count() - 1)
                            {
                                tempstr += "\n";
                            }
                        }
                        response = string.Format(ReplyType.Message_Text,
                                              fromuser,
                                              touser,
                                              DateTime.Now.Ticks,
                                             tempstr);
                        break;
                    default:
                        response = string.Format(ReplyType.Message_Text,
                                           fromuser,
                                           touser,
                                           DateTime.Now.Ticks,
                                           json["code"] + "");
                        break;
                }
            }


            //if (requestContent.Contains("你好") || requestContent.Contains("您好"))
            //{
            //    response = "您也好~";
            //}
            //else if (requestContent.Contains("傻"))
            //{
            //    response = "我不傻！哼~ ";
            //}
            //else if (requestContent.Contains("逼") || requestContent.Contains("操"))
            //{
            //    response = "哼，你说脏话！ ";
            //}
            //else if (requestContent.Contains("是谁"))
            //{
            //    response = "我是大哥大，有什么能帮您的吗？~";
            //}
            //else if (requestContent.Contains("再见"))
            //{
            //    response = "再见！";
            //}
            //else if (requestContent.Contains("bye"))
            //{
            //    response = "Bye！";
            //}
            //else if (requestContent.Contains("谢谢"))
            //{
            //    response = "不客气！嘿嘿";
            //}
            //else if (requestContent == "h" || requestContent == "H" || requestContent.Contains("帮助"))
            //{
            //    response = @"查询天气，输入tq 城市名称\拼音\首字母";
            //}
            //else
            //{
            //    response = "您说的，可惜，我没明白啊，试试其他关键字吧。";
            //}

            return response;
        }


        //接受图片消息 备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string ImageHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode PicUrl = xmldoc.SelectSingleNode("/xml/PicUrl");
            if (PicUrl != null)
            {
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                if (wcu != null)
                {
                    string js = "{\"Title\":\"推送图片\",\"Type\":2,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":\"" + PicUrl.InnerText + "\"}";
                    Message ms = new Message("e家亲", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0")
                    {
                        responseContent = string.Format(ReplyType.Message_News_Main,
                                                   FromUserName.InnerText,
                                                   ToUserName.InnerText,
                                                   DateTime.Now.Ticks,
                                                   "1",
                                                   string.Format(ReplyType.Message_News_Item, "e家亲", "你向设备【" + wcu.DeviceName + "】推送了一张图片！",
                                                   PicUrl.InnerText, ""));
                    }
                    else
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送图片失败！/:bome");
                    }
                }
                else
                {
                    responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }
        //接受语音消息  备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string VoiceHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode Format = xmldoc.SelectSingleNode("/xml/Format");
            if (MediaId != null)
            {
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                if (wcu != null)
                {
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, MediaId.InnerText);
                    string js = "{\"url\":\"" + url + "\",\"format\":\"" + Format.InnerText + "\"}";
                    js = "{\"Title\":\"推送语音\",\"Type\":3,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":" + js + "}";
                    Message ms = new Message("e家亲", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string str = jsonSerializer.Serialize(ms);
                    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0")
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                                                      FromUserName.InnerText,
                                                      ToUserName.InnerText,
                                                      DateTime.Now.Ticks,
                                                      "你向设备【" + wcu.DeviceName + "】推送了一段语音！");
                    }
                    else
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送语音失败！/:bome");
                    }
                }
                else
                {
                    responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }

        //接受视频消息  备注："&"=>"@1"  "="=>"@2"  "%"=>"@3"
        public string VideoHandle(XmlDocument xmldoc)
        {
            string responseContent = "";
            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode MediaId = xmldoc.SelectSingleNode("/xml/MediaId");
            XmlNode ThumbMediaId = xmldoc.SelectSingleNode("/xml/ThumbMediaId");
            if (MediaId != null)
            {
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == FromUserName.InnerText && q.State == true);
                if (wcu != null)
                {
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, MediaId.InnerText);
                    string img = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ToKen, ThumbMediaId.InnerText);
                    string js = "{\"url\":\"" + url + "\",\"img\":\"" + img + "\"}";
                    js = "{\"Title\":\"推送视频\",\"Type\":4,\"OpenId\":\"" + wcu.OpenId + "\",\"Content\":" + js + "}";
                    Message ms = new Message("e家亲", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0")
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                                                      FromUserName.InnerText,
                                                      ToUserName.InnerText,
                                                      DateTime.Now.Ticks,
                                                      "你向设备【" + wcu.DeviceName + "】推送了一段视频！");
                    }
                    else
                    {
                        responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "推送视频失败！/:bome");
                    }
                }
                else
                {
                    responseContent = string.Format(ReplyType.Message_Text,
                                                    FromUserName.InnerText,
                                                    ToUserName.InnerText,
                                                    DateTime.Now.Ticks,
                                                    "请先绑定设备！/::)");
                }
            }
            return responseContent;
        }

        public void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }

        private string BindWeChat(string OpenId, string SceneId)
        {
            StringBuilder str = new StringBuilder();
            TempBind tb = ve.TempBind.FirstOrDefault(q => q.SceneId == SceneId);
            if (tb != null)
            {
                WeChatUser tempwcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == OpenId && q.DeviceId == tb.DeviceId);
                if (tempwcu != null)
                {
                    if (tb.Token != tempwcu.Token) {
                        tempwcu.Token = tb.Token;
                        ve.SaveChanges();
                    }
                    str.Append("你的帐号已绑定【" + tempwcu.DeviceName + "】该设备！");
                }
                else
                {
                    string js = "";
                    Message ms = null;
                    WeChatUser wcu = new WeChatUser();
                    wcu.OpenId = OpenId;
                    wcu.Token = tb.Token;
                    wcu.DeviceId = tb.DeviceId;
                    wcu.DeviceName = tb.DeviceId;
                    wcu.State = true;
                    wcu.UserPhone = "";
                    wcu.HeadImg = "../Img/ejq.jpg";
                    wcu.CreateDate = DateTime.Now;
                    ve.WeChatUser.Add(wcu);
                    ve.SaveChanges();
                    List<WeChatUser> list = ve.WeChatUser.Where(x => x.OpenId == OpenId).ToList();
                    foreach (var item in list) {
                        if (item.DeviceId == wcu.DeviceId) {
                            continue;
                        }
                        item.State = false;
                        ve.SaveChanges();
                    }
                    str.Append("\n请在下方回复“绑定设备+手机号”\n如：绑定设备13212345678\n即可激活设备！");
                    js = "{\"Title\":\"绑定推送\",\"Type\":6,\"OpenId\":\"" + OpenId + "\",\"Content\":\"\"}";
                    ms = new Message("e家亲", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                    string returnStr = PushHelper.PushMsg(tb.Token, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();

                }
            }
            return str.ToString();
        }
    }


    //回复类型
    public class ReplyType
    {
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }
        /// <summary>
        /// 图文消息主体
        /// </summary>
        public static string Message_News_Main
        {
            get
            {
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
        public static string Message_News_Item
        {
            get
            {
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
