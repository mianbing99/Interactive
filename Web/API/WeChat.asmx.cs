using Common;
using Me.Common.Cache;
using Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using Web.Common;
using WeChat;

namespace Web.API
{
    /// <summary>
    /// WeChat1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WeChat1 : System.Web.Services.WebService
    {

        [WebMethod]
        public void QrCodeImg(string Token, string deviceId)
        {
            int num = 0;
            do
            {
                num = TextStr.RandomArray(1, 50000, 100000)[0];
                string str = num + "";
            } while (false);
            WriteImg(QrCodeManager.GenerateTemp(GetToken(), num));
        }

        VideoEntities ve = DBContextFactory.GetDbContext();
        [WebMethod]
        public void Index()
        {
            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            //WriteLog("echoString：" + echoString + "---signature:" + signature + "---timestamp:" + timestamp + "---nonce:" + nonce);
            if (string.IsNullOrEmpty(echoString))
            {
                if (WeChatHelper.CheckSignature(signature, timestamp, nonce))
                {
                    string postString = string.Empty;
                    if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
                    {
                        //读取流消息
                        using (Stream stream = HttpContext.Current.Request.InputStream)
                        {
                            Byte[] postBytes = new Byte[stream.Length];
                            stream.Read(postBytes, 0, (Int32)stream.Length);
                            postString = Encoding.UTF8.GetString(postBytes);
                            MessageHelp help = new MessageHelp();
                            help.ToKen = GetToken();
                            string responseContent = string.Empty;
                            //判断是否加密
                            bool IsAes = HttpContext.Current.Request.QueryString["encrypt_type"] == "aes" ? true : false;
                            if (IsAes)
                            {
                                string msg_signature = HttpContext.Current.Request.QueryString["msg_signature"];
                                WXBizMsgCrypt wmc = new WXBizMsgCrypt(WeChatConfig.Token, WeChatConfig.EncodingAESKey, WeChatConfig.AppId);
                                string decmsg = string.Empty; //解密后
                                int decnum = wmc.DecryptMsg(msg_signature, timestamp, nonce, postString, ref postString);
                                if (decnum == 0)
                                {
                                    wmc.EncryptMsg(help.ReturnMessage(postString), timestamp, nonce, ref responseContent);
                                }
                            }
                            else
                            {
                                responseContent = help.ReturnMessage(postString);
                            }
                            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                            HttpContext.Current.Response.Write(responseContent);
                        }
                    }
                }
            }
            else
            {
                if (WeChatHelper.CheckSignature(signature, timestamp, nonce))
                {
                    Write(echoString);
                }
            }
        }
        [WebMethod]
        public void DelDeviceBind(string deviceId) {
            ve.WeChatUser.RemoveRange(ve.WeChatUser.Where(x => x.DeviceId == deviceId).ToList());
            Write("true");
        }

        [WebMethod]
        public void GetQrCodeImg(string Token, string deviceId)
        {
            TempBind tb = null;
            int num = 0;
            do
            {
                num = TextStr.RandomArray(1, 50000, 100000)[0];
                string str = num + "";
                tb = ve.TempBind.FirstOrDefault(q => q.SceneId ==str );
            } while (tb!=null);
            tb = new TempBind();
            tb.Token = Token;
            tb.SceneId = num+"";
            tb.CreateDate = DateTime.Now;
            tb.DeviceId = deviceId;
            ve.TempBind.Add(tb);
            ve.SaveChanges();
            WriteImg(QrCodeManager.GenerateTemp(GetToken(),num));
        }

        
        [WebMethod]
        public void GetWechatToken()
        {
            Write(EncryptHelper.Encrypt(GetToken()));
        }

        [WebMethod]
        public void GetWeChatUserInfo(string OpenId) {
          Write(WeChatUserHelper.GetUserInfo(GetToken(), OpenId));
        }

        [WebMethod]
        public void UpdateMenu()
        {
            string param = "{\"button\":[{\"type\":\"view\",\"name\":\"云推送\",\"url\":\"http://v.icoxtech.com\"},"
             + "{\"name\":\"我的设备\",\"sub_button\":[{\"type\":\"view\",\"name\":\"设备管理\",\"url\":\"http://v.icoxtech.com/Child/EjqDeviceList.aspx\"}," +
                "{\"type\":\"view\",\"name\":\"成员管理\",\"url\":\"http://v.icoxtech.com/Child/EjqUsers.aspx\"}]},"
             + "{\"name\":\"远程管理\",\"sub_button\":[{\"type\":\"click\",\"name\":\"一键抓拍\",\"key\":\"V1003_GOOD\"},{\"type\":\"click\",\"name\":\"视频抓拍\",\"key\":\"V1001_GOOD\"},"+
                "{\"type\":\"click\",\"name\":\"远程定位\",\"key\":\"V1002_GOOD\"},{\"type\":\"view\",\"name\":\"习惯养成\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0299348550ae02ce&redirect_uri=http://v.icoxtech.com/AlarmClock.aspx?response_type=code&scope=snsapi_base&state=1#wechat_redirect\"}]}]}";//{\"type\":\"click\",\"name\":\"我的设备\",\"key\":\"T12\"},{\"type\":\"view\",\"name\":\"最新资源\",\"url\":\"http://v.icoxtech.com?a=-1\"}]}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", GetToken());
            Write(  Submit.HttpPost(url, param));
        }

        public void SetMsg(string OpenId, string Msg)
        {
            string param = "{\"touser\":\"" + OpenId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + Msg + "\"}}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", GetToken());
            Submit.HttpPost(url, param);
        }

        //[WebMethod]
        //public void SetFile()
        //{
        //    WebClient wc = new WebClient();
        //    string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", GetToken(), "image");
        //    byte[] responseArray = wc.UploadFile(url, "F:/1.png");
        //    Write(Encoding.GetEncoding("UTF-8").GetString(responseArray));
        //}
        private string GetToken()
        {
            var key = WeChatConfig.WeChatId;
            var token = CacheCore.Get<string>(key);
            if (string.IsNullOrEmpty(token))
            {
                var dic = WeChatHelper.GetToken();
                token = dic["access_token"].ToString();
                CacheCore.Set<string>(key, token, 60);
            }
            return token;
        }
        public void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
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
    }
}
