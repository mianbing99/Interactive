using Common;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Web.Common;
using WeChat;

namespace Web.API {
    /// <summary>
    /// ChildPartner 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class ChildPartner : System.Web.Services.WebService {
        VideoEntities ve = DBContextFactory.GetDbContext();
        [WebMethod]
        public void IndexChild() {
            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            if (string.IsNullOrEmpty(echoString)) {
                if (WeChildChatHelper.CheckBabySignature(signature, timestamp, nonce)) {
                    string postString = string.Empty;
                    if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST") {
                        //读取流消息
                        using (Stream stream = HttpContext.Current.Request.InputStream) {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes, 0, (Int32)stream.Length);
                            postString = Encoding.UTF8.GetString(postBytes);
                            MessageChild help = new MessageChild();
                            help.ToKen = GetChildToken();
                            string responseContent = string.Empty;
                            //判断是否加密
                            bool IsAes = HttpContext.Current.Request.QueryString["encrypt_type"] == "aes" ? true : false;
                            if (IsAes) {
                                string msg_signature = HttpContext.Current.Request.QueryString["msg_signature"];
                                WXBizMsgCrypt wmc = new WXBizMsgCrypt(WeChatConfig.TokenChild, WeChatConfig.EncodingAESKeyChild, WeChatConfig.AppIdChild);
                                string decmsg = string.Empty; //解密后
                                int decnum = wmc.DecryptMsg(msg_signature, timestamp, nonce, postString, ref postString);
                                if (decnum == 0) {
                                    wmc.EncryptMsg(help.ReturnMessageChild(postString), timestamp, nonce, ref responseContent);
                                    //wmc.EncryptMsg(postString, timestamp, nonce, ref responseContent);
                                }
                            } else {
                                responseContent = help.ReturnMessageChild(postString);
                                //responseContent = postString;
                            }
                            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                            HttpContext.Current.Response.Write(responseContent);
                        }
                    }
                }
            } else {
                if (WeChildChatHelper.CheckBabySignature(signature, timestamp, nonce)) {
                    Write(echoString);
                }
            }
        }
        [WebMethod]
        public void GetQrCodeImg(string deviceId, string xingeToken) {
            if (xingeToken == null || xingeToken == "" || xingeToken == "0") {
                Write("xingeToken is null");
            } else {
                var time = Stopwatch.StartNew();
                Byte[] imgBytes = QrChildManager.getQrUrl(deviceId, xingeToken, GetChildToken());
                time.Stop();
                WriteLog(string.Format( "GetQrCodeImg 耗时：{0}，{1}",time.Elapsed.TotalSeconds, imgBytes.Length));
                WriteImg(imgBytes);
            }
            ////string token = QrChildManager.BindXingeToken("9");
            //QrChildManager.ChildUser("o_w1Kw4kn8-m1MZJP_B68t2k0qLQ", "ed34e5f471d45f49492a9a17db2f47ecd6ad468d");

            //string xingeToken1 = QrChildManager.GetXingeToken("o_w1Kw4kn8-m1MZJP_B68t2k0qLQ");
            //Write(xingeToken1);
        }

        [WebMethod]
        public void DelDeviceBind(string deviceId) {
            Write(QrChildManager.DeleteDevice(deviceId).ToString());
        }

        [WebMethod]
        public void GetWechatToken() {
            Write(EncryptHelper.Encrypt(GetChildToken()));
        }

        [WebMethod]
        public void GetWeChatUserInfo(string OpenId) {
            Write(WeChatUserHelper.GetChildUserInfo(GetChildToken(), OpenId));
        }

        [WebMethod]
        public void tuisong() {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string js = "{\"Title\":\"推送消息\",\"Type\":1,\"OpenId\":\"" + 666 + "\",\"Content\":\"" + 666 + "\"}";
            Message ms = new Message("e家亲幼儿伴侣", js);
            string returnStr = XinGePush.PushMsg("4a0b5d279b404c1700cb443f0111a22361917ceb", jsonSerializer.Serialize(ms));
        }
        [WebMethod]
        //更改菜单
        public void UpdateMenu() {
            //string studyUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect";
            //{"button": [{"type": "view", "name": "云推送", "url": "http://v.icoxtech.com/IndexPage.aspx"}, {"name": "亲子互动", "sub_button": [{"type": "view", "name": "一键锁屏", "url": "http://www.soso.com"}, {"type": "view", "name": "作息管理", "url": "http://v.qq.com"}, {"type": "click", "name": "学习情况", "key": "V1001_GOOD"}]}, {"type": "view", "name": "最新资源", "url": "http://v.icoxtech.com?a=-1"}]}
            string param = "{\"button\":[{\"type\":\"view\",\"name\":\"云推送\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/Child/ChildIndex.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},"
                + "{\"name\":\"亲子互动\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键解锁\",\"key\":\"V1006_GOOD\"},{\"type\":\"click\",\"name\":\"一键锁屏\",\"key\":\"V1005_GOOD\"},{\"type\":\"click\",\"name\":\"远程定位\",\"key\":\"V1002_GOOD\"},{\"type\":\"view\",\"name\":\"作息管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},{\"type\":\"view\",\"name\":\"使用轨迹\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/StudyStation.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]},"
                + "{\"name\":\"远程管理\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键截图\",\"key\":\"V1004_GOOD\"},{\"type\":\"click\",\"name\":\"一键抓拍\",\"key\":\"V1003_GOOD\"},{\"type\":\"click\",\"name\":\"视频抓拍\",\"key\":\"V1001_GOOD\"},"+
                "{\"type\":\"view\",\"name\":\"设备管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/Child/ChildDeviceList.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}," +
                "{\"type\":\"view\",\"name\":\"成员管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0e7b25b6f1553eea&redirect_uri=http://v.icoxtech.com/Child/ChildUsers.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]}]}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", GetChildToken());
            Write(Submit.HttpPost(url, param));
        }

        public void SetMsgChild(string openId, string Msg) {
            string param = "{\"touser\":\"" + openId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + Msg + "\"}}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", GetChildToken());
            Submit.HttpPost(url, param);
        } 
        public string GetChildToken() {
            /*WeChildToken wct = null;
            Application.Lock();
            if (Application["ChildToken"] != null) {
                wct = Application["ChildToken"] as WeChildToken;
                if (wct != null && (DateTime.Now.Subtract(wct.CreateTime)).TotalSeconds < wct.Expires - 200) {
                    return wct.ChildToken;
                }
            }
            Dictionary<string, object> dic = WeChildChatHelper.GetBabyToken();
            wct = new WeChildToken(dic["access_token"] + "", Convert.ToInt32(dic["expires_in"] + ""));
            Application["ChildToken"] = wct;
            Application.UnLock();
            return wct.ChildToken;*/
            Device_user du = new Device_user();
            return du.SelectToken();
        }
        public void WriteLog(string text) {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
        private void Write(string str) {
            Context.Response.Clear();
            Context.Response.ContentEncoding = Encoding.UTF8;
            Context.Response.ContentType = "text/plain";
            Context.Response.Write(str);
            Context.Response.End();
        }
        private void WriteImg(Byte[] str) {
            Context.Response.Clear();
            Context.Response.ContentType = "image/jpg";
            Context.Response.BinaryWrite(str);
            Context.Response.End();
        }

    }

}
