using Common;
using Me.Common.Data;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace WeChat
{
    public class MessageInteractive
    {

        VideoEntities ve = DBContextFactory.GetDbContext();

        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        SendHelper sendHelper = new SendHelper(2100339782, "A513K1EBXG7A", "c17ed4bad21f480013b649f1c91d7ead");
        //接收消息
        public string ReturnMessage(string postStr) 
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(new MemoryStream(Encoding.UTF8.GetBytes(postStr)));
            string ResponseContent = string.Empty;
            XmlNode MsgType = xmldoc.SelectSingleNode("/xml/MsgType");
            if (MsgType!=null)
            {
                switch(MsgType.InnerText.ToLower())
                {
                    case "event":
                        //事件消息
                        ResponseContent = EventHandler(xmldoc);
                        break;
                    case "text":
                        //文本消息
                        ResponseContent = TextHandler(xmldoc);
                        break;
                    case "image":
                        //图片消息
                        ResponseContent = ImageHandler(xmldoc);
                        break;
                    case "voice":
                        //语音消息
                        ResponseContent = VoiceHandler(xmldoc);
                        break;
                    case "video":
                        //视频消息
                        ResponseContent = VideoHandler(xmldoc);
                        break;
                    default:
                        break;
                }
            }
            return ResponseContent;
        }

        //事件
        public string EventHandler(XmlDocument xmlDocument)
        {
            string responseContent = string.Empty;
            XmlNode Event = xmlDocument.SelectSingleNode("/xml/Event");
            XmlNode ToUserName = xmlDocument.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmlDocument.SelectSingleNode("/xml/FromUserName");
            XmlNode EventKey = xmlDocument.SelectSingleNode("/xml/EventKey");
            if(Event!=null)
            {
                StringBuilder sb = new StringBuilder();
                string openId = FromUserName.InnerText;
                string wechatPublic = ToUserName.InnerText;
                switch(Event.InnerText.ToLower())
                {
                    //关注事件
                    case "subscribe":
                        if(!string.IsNullOrEmpty(EventKey.InnerText))
                        {
                            string SceneId = EventKey.InnerText;
                            SceneId = SceneId.Split('_')[1];
                            sb.Append(BindDevice(openId, SceneId));
                        }
                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, sb.ToString());
                        break;
                    case "unsubscribe":
                        //判断用户是否有绑定设备信息
                        //删除解除绑定用户的设备记录
                        //通知信息给其他绑定设备的用户
                        List<StudentDeviceBind> sd = ve.StudentDeviceBind.Where(bind => bind.openId == openId).ToList();
                        if (sd != null)
                        {
                            ve.StudentDeviceBind.RemoveRange(sd);
                            ve.SaveChanges();
                            foreach(var item in sd)
                            {
                                if(!item.userPhone.Equals("null"))
                                {
                                    string js = "{\"Title\":\"推送消息\",\"Type\":\"7\",\"OpenId\":\""+openId+"\",\"Content\":\"\"}";
                                    Messages msg = new Messages("互动平台",js);
                                    sendHelper.PushMsg(item.token, jsonSerializer.Serialize(msg));
                                }
                            }
                        }
                        break;
                    case "scan":
                        try
                        {
                            if (!string.IsNullOrEmpty(EventKey.InnerText))
                            {
                                string sceneId = EventKey.InnerText;
                                sb.Append(BindDevice(openId, sceneId));
                            }
                        }
                        catch (Exception)
                        {
                            sb = new StringBuilder("请勿重复扫描二维码绑定设备！");
                            throw;
                        }
                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, sb.ToString());
                        break;
                    case "click":
                        string evkey = EventKey.InnerText;
                        /*
                        * V1001_GOOD 一键解锁 14
                        * V1002_GOOD 一键锁屏 12
                        * V1003_GOOD 远程定位 11
                        * V1004_GOOD 图片抓拍 8
                        * V1005_GOOD 视频抓拍 10
                        */
                        StudentDeviceBind deviceInfo = studentDeviceBindInfo(openId);
                        if (deviceInfo!=null)
                        {
                            string showText = "请求失败，请稍后重试";
                            switch (evkey)
                            {
                                case "V1001_GOOD":
                                    if (PushTypeMessage(14, openId, deviceInfo.token) == "0")
                                    {
                                        showText = "正在解锁.../:sun";
                                    }
                                    break;
                                case "V1002_GOOD":
                                    if(PushTypeMessage(12,openId,deviceInfo.token)=="0")
                                    {
                                        showText = "正在锁屏.../:sun";
                                    }
                                    break;
                                case "V1003_GOOD":
                                    if(PushTypeMessage(11,openId,deviceInfo.token)=="0")
                                    {
                                        showText = "定位请求已发送，等待回复（如果远程设备无响应，则无回复消息）";
                                    }
                                    break;
                                case "V1004_GOOD":
                                    if (PushTypeMessage(8, openId, deviceInfo.token) == "0")
                                    {
                                        showText = "图片抓拍请求已发送，等待回复（如果远程设备无响应，则无回复消息）";
                                    }
                                    break;
                                case "V1005_GOOD":
                                    if (PushTypeMessage(10, openId, deviceInfo.token) == "0")
                                    {
                                        showText = "视频抓拍请求已发送，等待回复（如果远程设备无响应，则无回复消息）";
                                    }
                                    break;
                                case "V1006_GOOD":
                                    //string deviceId = ve.StudentDeviceBind.First(bind => bind.openId == openId && bind.state == true).deviceId;
                                    //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/StudyStation.aspx?DeviceId=" + deviceId + "?response_type=code&scope=snsapi_base&state=1#wechat_redirect";
                                    //HttpContext.Current.Response.Redirect(url);
                                    showText = "功能尚未完成";
                                    break;
                            }
                            responseContent = string.Format(ReplyTyper.Message_Text,openId,wechatPublic,DateTime.Now.Ticks,showText);
                        }
                        else 
                        {
                            responseContent = string.Format(ReplyTyper.Message_Text,openId,wechatPublic,DateTime.Now.Ticks,"亲，您还没有绑定设备哦/:,@-D");
                        }
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }

        //接收文本消息
        public string TextHandler(XmlDocument xmlDocument)
        {
            string responseContent = string.Empty;
            XmlNode ToUserName = xmlDocument.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmlDocument.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmlDocument.SelectSingleNode("/xml/Content");
            if(Content!=null)
            {
                string str = Content.InnerText;
                string text = string.Empty;
                if(!str.StartsWith("@"))
                {
                    string openId = FromUserName.InnerText;
                    string wechatPublic = ToUserName.InnerText;
                    StudentDeviceBind studentDeviceBind = ve.StudentDeviceBind.FirstOrDefault(bind=>bind.openId==openId&&bind.state==true);
                    if (studentDeviceBind!=null)
                    {
                        StudentDeviceBind deviceInfo = studentDeviceBindInfo(openId);
                        if(str.StartsWith("绑定设备"))
                        {
                            if (deviceInfo != null)
                            {
                                responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "你已绑定设备名称" + deviceInfo.deviceName + "设备！");
                                return responseContent;
                            }
                            Regex regex = new Regex("绑定设备\\d{11}$");
                            if (regex.IsMatch(str))
                            {
                                string phone = str.Substring(4, 11);
                                studentDeviceBind.userPhone = phone;
                                ve.SaveChanges();
                                responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "恭喜您绑定设备成功！当前设备名为：" + studentDeviceBind.deviceName + "\n在下方回复：“修改设备名称+您指定的设备名称”\n如：修改设备名称大儿子\n即可修改设备名称便于多设备管理！");
                                string js = "{\"Title\":\"推送消息\",\"Type\":\"6\",\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                                Messages msg = new Messages("互动平台", js);
                                sendHelper.PushMsg(studentDeviceBind.token, JsonConvert.SerializeObject(msg));
                            }
                            else
                            {
                                responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "输入格式有误，请重新输入！");
                            }
                        }
                        else if (str.StartsWith("修改设备名称") || str.StartsWith("切换设备"))
                        {
                            if (deviceInfo!=null)
                            {
                                if (str.StartsWith("修改设备名称"))
                                {
                                    string deviceName = str.Substring(6, str.Length - 6).Trim();
                                    if (deviceName.Length>0)
                                    {
                                        int countInfo = ve.StudentDeviceBind.Where(bind => bind.openId == openId && bind.deviceName == deviceName).Count();
                                        if (countInfo > 0)
                                        {
                                            responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "不能修改相同设备名称呦");
                                            return responseContent;
                                        }
                                        studentDeviceBind.deviceName = deviceName;
                                        ve.SaveChanges();
                                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "修改设备名称成功，当前设备名称为：" + studentDeviceBind.deviceName + "\n在下方回复：“切换设备+设备名称”\n如：切换设备大儿子\n即可切换到设备大儿子！");
                                    }
                                    else
                                    {
                                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "输入格式有误，请重新输入！");
                                    }
                                }
                                else if (str.StartsWith("切换设备"))
                                {
                                    string deviceName = str.Substring(4, str.Length - 4);
                                    studentDeviceBind = ve.StudentDeviceBind.FirstOrDefault(bind => bind.openId == openId && bind.deviceName == deviceName);
                                    if (studentDeviceBind != null)
                                    {
                                        List<StudentDeviceBind> list = ve.StudentDeviceBind.Where(bind => bind.openId == openId).ToList();
                                        foreach (var item in list)
                                        {
                                            item.state = false;
                                            if (item.deviceName.Equals(deviceName))
                                            {
                                                item.state = true;
                                            }
                                        }
                                        ve.SaveChanges();
                                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "切换设备成功，当前设备为：" + deviceName);
                                    }
                                    else
                                    {
                                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "切换设备失败，设备名称有误！");
                                    }
                                }
                            }
                            else 
                            {
                                responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "亲，您还没有绑定设备哦/:,@-D");
                            }
                        }
                        else
                        {
                            if (deviceInfo != null)
                            {
                                string js = "{\"Title\":\"推送消息\",\"Type\":1,\"OpenId\":\"" + studentDeviceBind.openId + "\",\"Content\":\"" + str + "\"}";
                                Messages msg = new Messages("互动平台", js);
                                string returnStr = sendHelper.PushMsg(studentDeviceBind.token, jsonSerializer.Serialize(msg));
                                JObject json = JObject.Parse(returnStr);
                                returnStr = json["ret_code"].ToString();
                                if (returnStr == "0")
                                {
                                    responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "你向设备【" + studentDeviceBind.deviceName + "】推送消息成功/:sun");
                                }
                                else
                                {
                                    responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "推送消息失败！/:bome");
                                }
                            }
                            else 
                            {
                                responseContent = string.Format(ReplyTyper.Message_Text,openId,wechatPublic,DateTime.Now.Ticks,"亲，您还没有绑定设备哦！/:,@-D");
                            }
                        }
                    }
                    else
                    {
                        responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "亲，您还没有扫描设备二维码哦");
                    }
                    //string content = Content.InnerText + "openId:" + FromUserName.InnerText;
                    //responseContent = string.Format(ReplyTyper.Message_Text,openId, wechatPublic, DateTime.Now.Ticks, content);
                }
            }
            return responseContent;
        }

        //接收图片消息
        public string ImageHandler(XmlDocument xmlDocument) 
        {
            string responseContent = string.Empty;
            XmlNode ToUserName = xmlDocument.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmlDocument.SelectSingleNode("/xml/FromUserName");
            string wechatPublic = ToUserName.InnerText;
            string openId = FromUserName.InnerText;
            StudentDeviceBind bind = studentDeviceBindInfo(openId);
            if (bind != null)
            {
                XmlNode PicUrl = xmlDocument.SelectSingleNode("/xml/PicUrl");
                string js = "{\"url\":\"" + PicUrl.InnerText + "\",\"format\":\"" + "png" + "\"}";
                js = "{\"Title\":\"推送图片\",\"Type\":2,\"OpenId\":\""+openId+"\",\"Content\":"+js+"}";
                Messages msg = new Messages("互动平台",js.Replace("&","@1").Replace("=","@2").Replace("%","@3"));
                string returnStr = sendHelper.PushMsg(bind.token, jsonSerializer.Serialize(msg));
                JObject json = JObject.Parse(returnStr);
                returnStr = json["ret_code"].ToString();
                if (returnStr == "0")
                {
                    responseContent = string.Format(ReplyTyper.Message_News_Main, openId, wechatPublic, DateTime.Now.Ticks, 1, string.Format(ReplyTyper.Message_News_Item, "智能学习互动平台", "您向设备【" + bind.deviceName + "】推送一张图片！", PicUrl.InnerText, ""));
                }
                else 
                {
                    responseContent = string.Format(ReplyTyper.Message_Text,openId,wechatPublic,DateTime.Now.Ticks,"推送图片失败!/:bome");
                }
            }
            else 
            {
                responseContent = string.Format(ReplyTyper.Message_Text, openId, wechatPublic, DateTime.Now.Ticks, "亲，您还没有绑定设备哦/:,@-D");
            }
            //string content = "图片openId："+FromUserName.InnerText;
            //responseContent = string.Format(ReplyTyper.Message_Text,FromUserName.InnerText,ToUserName.InnerText,DateTime.Now.Ticks,content);
            return responseContent;
        }

        //接收语音消息
        public string VoiceHandler(XmlDocument xmlDocument)
        {
            string responseContent = string.Empty;
            XmlNode ToUserName = xmlDocument.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmlDocument.SelectSingleNode("/xml/FromUserName");
            StudentDeviceBind bind = studentDeviceBindInfo(FromUserName.InnerText);
            if (bind != null)
            {
                XmlNode MediaId = xmlDocument.SelectSingleNode("/xml/MediaId");
                XmlNode Format = xmlDocument.SelectSingleNode("/xml/Format");
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}",InteractiveHelper.GetToken(),MediaId.InnerText);
                string js = "{\"url\":\""+url+"\",\"format\":\""+Format.InnerText+"\"}";
                js = "{\"Title\":\"推送语音\",\"Type\":3,\"OpenId\":\""+FromUserName.InnerText+"\",\"Content\":"+js+"}";
                Messages msg = new Messages("互动平台",js.Replace("&","@1").Replace("=","@2").Replace("%","@3"));
                string returnStr = sendHelper.PushMsg(bind.token, jsonSerializer.Serialize(msg));
                JObject json = JObject.Parse(returnStr);
                returnStr = json["ret_code"].ToString();
                if (returnStr == "0")
                {
                    responseContent = string.Format(ReplyTyper.Message_Text, FromUserName.InnerText, ToUserName.InnerText, DateTime.Now.Ticks, "您向设备【" + bind.deviceName + "】推送一条语音！");
                }
                else 
                {
                    responseContent = string.Format(ReplyTyper.Message_Text,FromUserName.InnerText,ToUserName.InnerText,DateTime.Now.Ticks,"推送语音失败！/:bome");
                }
            }
            else 
            {
                responseContent = string.Format(ReplyTyper.Message_Text, FromUserName.InnerText, ToUserName.InnerText, DateTime.Now.Ticks, "亲，您还没有绑定设备哦/:,@-D");
            }
            //string content = "语音openId:"+FromUserName.InnerText;
            //responseContent = string.Format(ReplyTyper.Message_Text,FromUserName.InnerText,ToUserName.InnerText,DateTime.Now.Ticks,content);
            return responseContent;
        }

        //接收视频消息
        public string VideoHandler(XmlDocument xmlDocument)
        {
            string responseContent = string.Empty;
            XmlNode ToUserName = xmlDocument.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmlDocument.SelectSingleNode("/xml/FromUserName");
            StudentDeviceBind bind = studentDeviceBindInfo(FromUserName.InnerText);
            if (bind != null)
            {
                XmlNode MediaId = xmlDocument.SelectSingleNode("/xml/MediaId");
                XmlNode ThumbMediaId = xmlDocument.SelectSingleNode("/xml/ThumbMediaId");
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", InteractiveHelper.GetToken(), MediaId.InnerText);
                string img = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", InteractiveHelper.GetToken(), ThumbMediaId.InnerText);
                string js = "{\"url\":\"" + url + "\",\"img\":\"" + img + "\"}";
                js = "{\"Title\":\"推送视频\",\"Type\":4,\"OpenId\":\"" + FromUserName.InnerText + "\",\"Content\":" + js + "}";
                Messages msg = new Messages("互动平台", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
                string returnStr = sendHelper.PushMsg(bind.token, jsonSerializer.Serialize(msg));
                JObject json = JObject.Parse(returnStr);
                returnStr = json["ret_code"].ToString();
                if (returnStr == "0")
                {
                    responseContent = string.Format(ReplyTyper.Message_Text, FromUserName.InnerText, ToUserName.InnerText, DateTime.Now.Ticks, "您向设备【" + bind.deviceName + "】推送一段视频！");
                }
                else
                {
                    responseContent = string.Format(ReplyTyper.Message_Text, FromUserName.InnerText, ToUserName.InnerText, DateTime.Now.Ticks, "推送视频失败！/:bome");
                }
            }
            else
            {
                responseContent = string.Format(ReplyTyper.Message_Text, FromUserName.InnerText, ToUserName.InnerText, DateTime.Now.Ticks, "亲，您还没有绑定设备哦/:,@-D");
            }
            //string content = "视频openId:" + FromUserName.InnerText;
            //responseContent = string.Format(ReplyTyper.Message_Text,FromUserName.InnerText,ToUserName.InnerText,DateTime.Now.Ticks,content);
            return responseContent;
        }

        public string BindDevice(string openId,string sceneId) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("欢迎使用智能学习互动平台\n");
            StudentDeviceQRcode dqr = ve.StudentDeviceQRcode.FirstOrDefault(qr=>qr.sceneId.Equals(sceneId));
            StudentDeviceBind bind = null;
            if (dqr != null)
            {
                bind = ve.StudentDeviceBind.FirstOrDefault(sd => sd.deviceId == dqr.deviceId&&sd.openId==openId);
                if (bind != null)
                {
                    if (bind.userPhone != null)
                    {
                        sb = new StringBuilder("你已绑定【" + bind.deviceName + "】设备，不可重复绑定哦");
                        return sb.ToString();
                    }
                    else
                    {
                        List<StudentDeviceBind> list = ve.StudentDeviceBind.Where(ch => ch.openId == openId).ToList();
                        if (list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                if(item.deviceId==bind.deviceId)
                                {
                                    item.state = true;
                                    continue;
                                }
                                item.state = false;
                            }
                        }
                        ve.SaveChanges();
                    }
                }
                else
                {
                    List<StudentDeviceBind> list = ve.StudentDeviceBind.Where(ch => ch.openId == openId).ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            item.state = false;
                        }
                    }
                    bind = new StudentDeviceBind();
                    bind.openId = openId;
                    bind.token = dqr.token;
                    bind.deviceId = dqr.deviceId;
                    bind.deviceName = dqr.deviceId;
                    bind.createTime = DateTime.Now;
                    bind.deviceAvatar = "../Img/jqr.png";
                    bind.state = true;
                    ve.StudentDeviceBind.Add(bind);
                    ve.SaveChanges();
                }
                sb.Append("\n请在下方回复“绑定设备+手机号”\n如：绑定设备13212345678\n即可激活设备！");
            }
            else 
            {
                sb.Append("请扫描设备上的二维码");
            }
            return sb.ToString();
        }

        public string PushTypeMessage(int type,string openId,string token) 
        {
            string js = "{\"Title\":\"推送消息\",\"Type\":"+type+",\"OpenId\":\""+openId+"\",\"Content\":\"\"}";
            Messages msg = new Messages("互动平台",js);
            string returnStr = sendHelper.PushMsg(token,jsonSerializer.Serialize(msg));
            JObject json = JObject.Parse(returnStr);
            returnStr = json["ret_code"].ToString();
            if(returnStr!=null)
            {
                return returnStr;
            }
            return null;
        }


        public StudentDeviceBind studentDeviceBindInfo(string openId)
        {
            StudentDeviceBind studentDeviceBind = ve.StudentDeviceBind.FirstOrDefault(bind => bind.openId == openId && bind.state == true && bind.userPhone != null);
            if(studentDeviceBind!=null)
            {
                return studentDeviceBind;
            }
            return null;
        }

        public void WriteLog(string text) 
        {
            StreamWriter strem = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log/log" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
            strem.WriteLine(text);
            strem.Close();
        }
        
    }
}
