using Common;
using Me.Common.Cache;
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
using System.Web;
using System.Web.Services;
using Web.Common;
using WeChat;

namespace Web.API
{
    /// <summary>
    /// WeChatInteractive 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WeChatInteractive : System.Web.Services.WebService
    {

        VideoEntities ve = new VideoEntities();
        [WebMethod]
        public void IndexInteractive() 
        {
            //获取微信请求参数
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            string echostr = HttpContext.Current.Request.QueryString["echostr"];
            string token = WeChatInteractiveConfig.Token;
            if (string.IsNullOrEmpty(echostr))
            {
                if(WeChatInteractiveHelper.CheckSignature(token,signature,timestamp,nonce))
                {
                    string postStr = string.Empty;
                    if(HttpContext.Current.Request.HttpMethod.ToUpper()=="POST")
                    {
                        using(Stream stream = HttpContext.Current.Request.InputStream)
                        {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes,0,(int)stream.Length);
                            postStr = Encoding.UTF8.GetString(postBytes);
                            MessageInteractive msg = new MessageInteractive();
                            string responseContent = string.Empty;
                            bool IsAes = HttpContext.Current.Request.QueryString["encrypt_type"] == "aes"?true:false;
                            
                            if (IsAes)
                            {
                                string msg_signature = HttpContext.Current.Request.QueryString["msg_signature"];
                                WXBizMsgCrypt wmc = new WXBizMsgCrypt(token,WeChatInteractiveConfig.EncodingAESKey,WeChatInteractiveConfig.AppId);
                                int decnum = wmc.DecryptMsg(msg_signature,timestamp,nonce,postStr,ref postStr);
                                if(decnum==0)
                                {
                                    wmc.EncryptMsg(msg.ReturnMessage(postStr),timestamp,nonce,ref responseContent);
                                }
                            }
                            else
                            {
                                responseContent = msg.ReturnMessage(postStr);
                            }
                            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                            HttpContext.Current.Response.Write(responseContent);
                        }
                    }
                }
            }
            else 
            {
                //加密sha1对比signature字符串
                if(WeChatInteractiveHelper.CheckSignature(token,signature,timestamp,nonce))
                {
                    //返回echostr参数内容
                    Write(echostr);
                }
            }
        }

        //公众号菜单栏
        [WebMethod]
        public void UpdateMenu()
        {
            /*
             * V1001_GOOD 一键解锁
             * V1002_GOOD 一键锁屏
             * V1003_GOOD 远程定位
             * V1004_GOOD 一键抓拍
             * V1005_GOOD 视频抓拍
             */
            string str = "{\"button\":[{\"type\":\"view\",\"name\":\"名师课堂\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://ftv.icoxtech.com?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},"
                + "{\"name\":\"亲子互动\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键解锁\",\"key\":\"V1001_GOOD\"},{\"type\":\"click\",\"name\":\"一键锁屏\",\"key\":\"V1002_GOOD\"},{\"type\":\"click\",\"name\":\"远程定位\",\"key\":\"V1003_GOOD\"},{\"type\":\"view\",\"name\":\"习惯养成\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"},{\"type\":\"view\",\"name\":\"使用轨迹\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/LearningRecord.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]},"
                + "{\"name\":\"远程管理\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键抓拍\",\"key\":\"V1004_GOOD\"},{\"type\":\"click\",\"name\":\"视频抓拍\",\"key\":\"V1005_GOOD\"}," +
                "{\"type\":\"view\",\"name\":\"设备管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/DeviceManagement.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}," +
                "{\"type\":\"view\",\"name\":\"成员管理\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf9ade153117809bf&redirect_uri=http://v.icoxtech.com/Interactive/MemberManagement.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]}]}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", GetToken());
            Write(Submit.HttpPost(url, str));
        }

        

        //[WebMethod]
        //public void GetWeChatToken()
        //{
        //    Write(EncryptHelper.Encrypt(GetToken()));
        //}

        [WebMethod]
        public void send(string openId,string token,string content)
        {
            SendHelper sendHelper = new SendHelper(2100339782, "A513K1EBXG7A", "c17ed4bad21f480013b649f1c91d7ead");
            //string js = "{\"Title\":\"推送消息\",\"Type\":1,\"OpenId\":\""+openId+"\",\"Content\":\""+content+"\"}";
            //Messages msg = new Messages("互动平台", js);

            //string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", InteractiveHelper.GetToken(), MediaId.InnerText);
            string img = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", InteractiveHelper.GetToken(), "JnPEkPGmb524ex304tWoI0TbMeXWMpsbp2IJK-846FVYbb_awwWyKhrmqP4g7D-y");
            string js = "{\"url\":\"" + img + "\",\"format\":\"\"}";
            js = "{\"Title\":\"推送图片\",\"Type\":2,\"OpenId\":\"" + "ocpAZ1hp_Egx33pvVie1Y4Fl3zik" + "\",\"Content\":" + js + "}";
            Messages msg = new Messages("互动平台", js.Replace("&", "@1").Replace("=", "@2").Replace("%", "@3"));
            string returnStr = sendHelper.PushMsg("7afae4da3a1dfc75e90c1ed4e0fb0d4db5264df2", JsonConvert.SerializeObject(msg));
            JObject json = JObject.Parse(returnStr);
            returnStr = json["ret_code"].ToString();
            Write("ret_code："+returnStr);
        }

        
        [WebMethod]
        public void GetQrCodeImg(string deviceId,string xingetoken)
        {
            if(xingetoken==null||xingetoken.Length==0)
            {
                Write("xinge is null");
            }
            string sql = "select sceneId from StudentDeviceQRcode order by sceneId desc";
            int num = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
            if (num>0)
            {
                num = 1 + num;
            }
            else 
            {
                num = 50000;
            }
            QrInteractiveManager.updateToken(deviceId,xingetoken);
            StudentDeviceQRcode bind = new StudentDeviceQRcode();
            bind.sceneId = num.ToString();
            bind.token = xingetoken;
            bind.deviceId = deviceId;
            bind.createTime = DateTime.Now;
            if(QrInteractiveManager.addQrcodeInfo(bind)>0)
            {
                WriteImg(QrCodeManager.GenerateTemp(GetToken(), num));
            }
        }

        [WebMethod]
        public void DeviceReleaseBind(string deviceId)
        {
            if (deviceId != null && deviceId.Length>0)
            {
                var item = ve.StudentDeviceBind.Where(bind => bind.deviceId == deviceId).ToArray();
                if (item.Length>0)
                {
                    ve.StudentDeviceBind.RemoveRange(item);
                    bool bo = false;
                    if(ve.SaveChanges()>0)
                    {
                        bo = true;
                    }
                    Write(bo.ToString());
                }
                
            }
            
        }


        //使用轨迹 记录点击的应用
        [WebMethod]
        public void AddLearnSt(string deviceId, string appName)
        {
            StudentLearnSt sl = new StudentLearnSt();
            sl.deviceId = deviceId;
            sl.appName = appName;
            sl.createTime = DateTime.Now;
            ve.StudentLearnSt.Add(sl);
            ve.SaveChanges();
        }
        

        [WebMethod]
        public void GetWeChatUserInfo(string openId) 
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",GetToken(),openId);
            string str = Submit.HttpGet(url);
            Write(str);
        }

        public string GetToken() 
        {
            return InteractiveHelper.GetToken();
        }

        private void Write(string str) 
        {
            Context.Response.Clear();
            Context.Response.ContentEncoding = Encoding.UTF8;
            Context.Response.ContentType = "text/plain";
            Context.Response.Write(str);
            Context.Response.End();
        }

        private void WriteImg(Byte[] str) 
        {
            Context.Response.Clear();
            Context.Response.ContentType = "image/jpg";
            Context.Response.BinaryWrite(str);
            Context.Response.End();
        }

        public void WriteLog(string text)
        {
            StreamWriter strem = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log/log"+DateTime.Now.ToString("yyyyMMdd")+".txt", true);
            strem.WriteLine(text);
            strem.Close();
        }

    }
}
