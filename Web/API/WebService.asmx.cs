using Common;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Web.Common;
using WeChat;
using Me.Common.Cache;

namespace Web.API {
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService {
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        VideoEntities ve = DBContextFactory.GetDbContext();


        [WebMethod]
        public void GetVideoType(int typeid) {
            var list = from q in ve.VideoType
                       where q.Tid == typeid && (q.State == 1 || q.State == 2)
                       orderby q.Sort, q.Id
                       select new {
                           q.Id,
                           q.Title,
                           q.Cover
                       };

            string json = jsonSerializer.Serialize(list);
            Write(json);
        }
        [WebMethod]
        public void GetChildVideo(int typeid) {
            var list = from q in ve.VideoType
                       where q.Tid == typeid && q.State == 1
                       orderby q.Sort, q.Id
                       select new {
                           q.Id,
                           q.Title,
                           q.Cover
                       };

            string json = jsonSerializer.Serialize(list);
            Write(json);
        }
        [WebMethod]
        public void GetTypeTitle(int typeid) {
            var list = from q in ve.VideoType
                       where q.Id == typeid
                       select new {
                           q.Title
                       };

            string json = jsonSerializer.Serialize(list);
            Write(json);
        }
        [WebMethod]
        public void GetVideoPage(int IndexPage, int PageSize, int TypeId) {
            int count = 0;
            string json = "";
            if (TypeId > 0) {
                var q1 = from a in ve.Video
                         where a.Tid == TypeId && a.State == true
                         orderby a.Sort ascending, a.Id ascending
                         select new {
                             a.Id,
                             a.Title
                         };
                count = q1.Count();
                var z = q1.Skip(PageSize * (IndexPage - 1)).Take(PageSize);
                json = jsonSerializer.Serialize(z);
            } else {
                var data = ve.Video.Where(q => q.State == true).OrderByDescending(q => q.CreateDate);
                var q2 = from a in ve.Video
                         where a.State == true
                         orderby a.CreateDate descending, a.Id descending
                         select new {
                             a.Id,
                             a.Title
                         };
                count = q2.Count();
                var z = q2.Skip(PageSize * (IndexPage - 1)).Take(PageSize);
                json = jsonSerializer.Serialize(z);
            }
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
        }

        //写入日志
        [WebMethod]
        public void WriteLog(string text) {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }

        [WebMethod(EnableSession = true)]
        public void WeChatPushMsg(string msg, string post) {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(q => q.OpenId == OpenId && q.State == true);
                if (wcu != null) {
                    int id = Convert.ToInt32(msg);
                    Video vi = ve.Video.FirstOrDefault(q => q.Id == id);
                    VideoType vt = ve.VideoType.FirstOrDefault(q => q.Id == vi.Tid);
                    string js = "{\"video_id\":" + id + ",\"video_name\":\"" + vi.Title + "\",\"position\":" + post + ",\"type_id\":" + vi.Tid + ",\"type_title\":\"" + vt.Title + "\",\"type_cover\":\"" + vt.Cover + "\"}";
                    js = "{\"Title\":\"e家亲推送\",\"Type\":5,\"Content\":" + js + "}";
                    Messages ms = new Messages("e家亲", js);
                    string returnStr = PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    if (returnStr == "0") {
                        new WeChat1().SetMsg(wcu.OpenId, "【" + vt.Title + "-" + vi.Title + "】推送成功！/:sun");
                        Write("推送成功！");
                    } else {
                        Write("推送失败！" + returnStr);
                    }
                } else {
                    Write("您没绑定设备！");
                }

            } else {
                Write("没有登陆,推送失败！");
            }
        }

        /// <summary>
        /// 幼儿伴侣推送
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="post"></param>
        [WebMethod(EnableSession = true)]
        public void WeChildPushMsg(string msg, string post) {
            if (Context.Session["OpenId"] != null) {
                //string openId = FromUserName.InnerText;
                //string xingeToken = QrChildManager.GetXingeToken(openId);                
                string OpenId = Context.Session["OpenId"].ToString();
                string xingeOpenId = QrChildManager.GetXingeOpenId(OpenId);
                string xingeToken = QrChildManager.GetToken(OpenId);
                if (xingeOpenId != null && xingeOpenId != "") {
                    int id = Convert.ToInt32(msg);
                    Video vi = ve.Video.FirstOrDefault(q => q.Id == id);
                    VideoType vt = ve.VideoType.FirstOrDefault(q => q.Id == vi.Tid);
                    string js = "{\"video_id\":" + id + ",\"video_name\":\"" + vi.Title + "\",\"position\":" + post + ",\"type_id\":" + vi.Tid + ",\"type_title\":\"" + vt.Title + "\",\"type_cover\":\"" + vt.Cover + "\"}";
                    js = "{\"Title\":\"幼儿伴侣推送\",\"Type\":5,\"Content\":" + js + "}";
                    Messages ms = new Messages("幼儿伴侣", js);
                    string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    JObject json = JObject.Parse(returnStr);
                    returnStr = json["ret_code"].ToString();
                    //Write("推送成功！");
                    if (returnStr == "0") {
                        new ChildPartner().SetMsgChild(xingeOpenId, "【" + vt.Title + "-" + vi.Title + "】推送成功！/:sun");
                        Write("推送成功!");
                    } else {
                        Write("推送失败!" + returnStr);
                    }
                } else {
                    Write("您没绑定设备");
                }
            } else {
                Write("您还没有登陆,推送失败");
            }
        }
        [WebMethod(EnableSession = true)]
        public void GetUsers() { 
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();//"o_w1Kw8Uxh4dEoRDCe-HNYnyYlhY";//
                string deviceId = QrChildManager.GetChildDeviceId(OpenId);
                if (deviceId == "" || deviceId == null) {
                    return;
                }
                DataTable dt = QrChildManager.GetOpenId(deviceId);
                Device_user du = new Device_user();
                List<string> list = new List<string>();
                foreach (DataRow row in dt.Rows) {
                    list.Add(row["OpenId"].ToString());
                }
                Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
                List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
                foreach (var item in list) {
                    Dictionary<string, string> dic3 = new Dictionary<string, string>();
                    dic3.Add("openid", item);
                    dic3.Add("lang", "zh_CN");
                    list2.Add(dic3);
                }
                dic2.Add("user_list", list2);
                string data2 = du.GetUserInformation(JsonConvert.SerializeObject(dic2));
                Write(data2);
            }
        }
        [WebMethod(EnableSession = true)]
        public void GetEwm() {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                string deviceId = QrChildManager.GetChildDeviceId(OpenId);
                string token = QrChildManager.Get_xingeToken(deviceId);

                /*Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("deviceId", deviceId);
                dic.Add("token", token);  xiiuagiasdasdadsdadasd
                Write(JsonConvert.SerializeObject(dic));*/
                Device_user du = new Device_user();
                Byte[] imgBytes = QrChildManager.getQrUrl(deviceId, token, du.SelectToken());
                WriteLog("GetEwm END:" + imgBytes.Length);
                Context.Response.Clear();
                Context.Response.ContentType = "image/jpg";
                Context.Response.BinaryWrite(imgBytes);
                Context.Response.End();
            }
        }
        [WebMethod(EnableSession = true)]
        public void DelUsers(string openId) {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                string device = QrChildManager.GetChildDeviceId(OpenId);
                DataTable dt = QrChildManager.GetOpenId(device);
                if (OpenId == dt.Rows[0]["OpenId"].ToString()) {
                    Device_user du = new Device_user();
                    List<string> list = new List<string>();
                    foreach (DataRow row in dt.Rows) {
                        list.Add(row["OpenId"].ToString());
                    }
                    Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
                    List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
                    foreach (var item in list) {
                        Dictionary<string, string> dic3 = new Dictionary<string, string>();
                        dic3.Add("openid", item);
                        dic3.Add("lang", "zh_CN");
                        list2.Add(dic3);
                    }
                    dic2.Add("user_list", list2);
                    string data2 = du.GetUserInformation(JsonConvert.SerializeObject(dic2));
                    Dictionary<string, List<Dictionary<string, object>>> list3 = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(data2);
                    string admin = "";
                    string user = "";
                    foreach (var item in list3["user_info_list"]) {
                        if (item["openid"].ToString() == OpenId) {
                            admin = item["nickname"].ToString();
                        }
                        if (item["openid"].ToString() == openId) {
                            user = item["nickname"].ToString();
                        }
                    }
                    foreach (var item in list3["user_info_list"]) {
                        if (user == admin) {
                            return;
                        } else {
                            string content = string.Format("成员{0}已被管理员{1}删除绑定！", user, admin);
                            if (item["openid"].ToString() == openId) {
                                content = string.Format("你已被管理员{0}删除绑定！", admin);
                            }
                            string data = "{\"touser\":\"" + item["openid"] + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
                            du.SendNews(data);
                        }
                    }
                    string xingeToken = QrChildManager.GetXingeToken(openId);
                    QrChildManager.DeleteOpenId(openId, device);
                    string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                    Message ms = new Message("e家亲幼儿伴侣", js);
                    XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                    Write("true");
                } else {
                    Write("你不是管理员，没有权限删除成员！");
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void CheckUser() {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                string device = QrChildManager.GetChildDeviceId(OpenId);
                DataTable dt = QrChildManager.GetOpenId(device);
                if (OpenId == dt.Rows[0]["OpenId"].ToString()) {
                    Write("true");
                } else {
                    Write("你不是管理员，没有权限删除成员！");
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public void GetDevices() {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();//"o_w1Kw8Uxh4dEoRDCe-HNYnyYlhY";
                List<Dictionary<string, string>> list = QrChildManager.GetDevices(OpenId);
                if (list == null) {
                    Write("null");
                    return;
                }
                Write(JsonConvert.SerializeObject(list));
            }
        }
        [WebMethod(EnableSession = true)]
        public void QHDevices(string deviceId) {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                QrChildManager.UpdateState3(OpenId, deviceId);
                Write("true");
            }
        }
        [WebMethod(EnableSession = true)]
        public void DelDevice(string deviceId) {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                string xingeToken = QrChildManager.GetXingeToken(OpenId);
                QrChildManager.DelDervice(OpenId, deviceId);
                string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + OpenId + "\",\"Content\":\"\"}";
                Message ms = new Message("e家亲幼儿伴侣", js);
                XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
                Write("true");
            }
        }

        [WebMethod(EnableSession = true)]
        public void Images(string image, string deviceId) {
            if (Context.Session["OpenId"] != null) {
                string OpenId = Context.Session["OpenId"].ToString();
                String header = "data:image/jpeg;base64,";
                if (image.IndexOf(header) != 0) {
                    return;
                }
                // 去掉头部  
                image = image.Substring(header.Length);
                try {
                    byte[] bt = Convert.FromBase64String(image);
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(bt);
                    Bitmap bitmap = new Bitmap(stream);
                    string uid = Guid.NewGuid().ToString();
                    string fileName = string.Format("D:/webSites/www_root/Img/deviceHeadImg/{0}.jpg", uid);
                    //http://v.icoxtech.com/Img/deviceHeadImg/
                    bitmap.Save(fileName);
                    Image srcImage = Image.FromFile(fileName);
                    try {
                        Bitmap b = new Bitmap(500, 500);
                        Graphics g = Graphics.FromImage(b);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                        g.DrawImage(srcImage, new Rectangle(0, 0, 500, 500), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                        g.Dispose();
                        b.Save(string.Format("D:/webSites/www_root/Img/deviceHeadImg/imgs/{0}.jpg", uid));
                    } catch (Exception ex) {
                        throw ex;
                    }
                    QrChildManager.UpdateHeadImg(OpenId, deviceId, string.Format("http://v.icoxtech.com/Img/deviceHeadImg/imgs/{0}.jpg", uid));
                    //return bitmap;
                } catch (Exception e) {
                    Write(e.Message);
                }
                Write("true");
            }
        }

        [WebMethod(EnableSession = true)]
        public void EGetUsers() {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.State == true);
                if (wcu == null) {
                    return;
                }
                List<WeChatUser> wcus = ve.WeChatUser.Where(x => x.DeviceId == wcu.DeviceId).OrderBy(x => x.CreateDate).ToList();
                Device_user du = new Device_user();
                Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
                List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
                foreach (var item in wcus) {
                    Dictionary<string, string> dic3 = new Dictionary<string, string>();
                    dic3.Add("openid", item.OpenId);
                    dic3.Add("lang", "zh_CN");
                    list2.Add(dic3);
                }
                dic2.Add("user_list", list2);
                string data2 = du.GetUserInformation2(JsonConvert.SerializeObject(dic2), GetToken());
                Write(data2);
            }
        }
        [WebMethod(EnableSession = true)]
        public void EGetEwm() {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.State == true);
                TempBind tb = null;
                int num = 0;
                do {
                    num = TextStr.RandomArray(1, 50000, 100000)[0];
                    string str = num + "";
                    tb = ve.TempBind.FirstOrDefault(q => q.SceneId == str);
                } while (tb != null);
                tb = new TempBind();
                tb.Token = wcu.Token;
                tb.SceneId = num + "";
                tb.CreateDate = DateTime.Now;
                tb.DeviceId = wcu.DeviceId;
                ve.TempBind.Add(tb);
                ve.SaveChanges();
                Context.Response.Clear();
                Context.Response.ContentType = "image/jpg";
                Context.Response.BinaryWrite(QrCodeManager.GenerateTemp(GetToken(), num));
                Context.Response.End();
            }
        }
        private string GetToken() {
            var key = WeChatConfig.WeChatId;
           var token= CacheCore.Get<string>(key);
           if (string.IsNullOrEmpty(token))
           {
               var dic = WeChatHelper.GetToken();
               token = dic["access_token"] + "";
               CacheCore.Set<string>(key, token, 60);
           }
           return token;
        }
        [WebMethod(EnableSession = true)]
        public void DelToken() {
            Application.Lock();
            if (Application["Token"] != null) {
                Application["Token"] = null;
                Write("true");
            }
            Write("null");
        }
        [WebMethod(EnableSession = true)]
        public void EDelUsers(string openId) {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.State == true);
                List<WeChatUser> list = ve.WeChatUser.Where(x => x.DeviceId == wcu.DeviceId).OrderBy(x => x.CreateDate).ToList();
                if (OpenId == list[0].OpenId) {
                    Device_user du = new Device_user();
                    Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
                    List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
                    foreach (var item in list) {
                        Dictionary<string, string> dic3 = new Dictionary<string, string>();
                        dic3.Add("openid", item.OpenId);
                        dic3.Add("lang", "zh_CN");
                        list2.Add(dic3);
                    }
                    dic2.Add("user_list", list2);
                    string data2 = du.GetUserInformation2(JsonConvert.SerializeObject(dic2), GetToken());
                    Dictionary<string, List<Dictionary<string, object>>> list3 = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(data2);
                    //string[] ids = openId.Split(',');
                    //foreach (var id in ids) {
                    string admin = "";
                    string user = "";
                    foreach (var item in list3["user_info_list"]) {
                        if (item["openid"].ToString() == OpenId) {
                            admin = item["nickname"].ToString();
                        }
                        if (item["openid"].ToString() == openId) {
                            user = item["nickname"].ToString();
                        }
                    }
                    foreach (var item in list3["user_info_list"]) {
                        string content = string.Format("成员{0}已被管理员{1}删除绑定！", user, admin);
                        if (item["openid"].ToString() == openId) {
                            content = string.Format("你已被管理员{0}删除绑定！", admin);
                        }
                        string data = "{\"touser\":\"" + item["openid"] + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
                        du.SendNews2(data, GetToken());
                    }
                    WeChatUser wcu2 = ve.WeChatUser.FirstOrDefault(x => x.OpenId == openId && x.DeviceId == wcu.DeviceId);
                    string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                    Message ms = new Message("e家亲", js);
                    PushHelper.PushMsg(wcu2.Token, jsonSerializer.Serialize(ms));
                    ve.WeChatUser.Remove(wcu2);
                    ve.SaveChanges();
                    //}
                    Write("true");
                } else {
                    Write("你不是管理员，没有权限删除成员！");
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void ECheckUser() {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.State == true);
                List<WeChatUser> wcus = ve.WeChatUser.Where(x => x.DeviceId == wcu.DeviceId).OrderBy(x => x.CreateDate).ToList();
                if (OpenId == wcus[0].OpenId) {
                    Write("true");
                } else {
                    Write("你不是管理员，没有权限删除成员！");
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public void EGetDevices() {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();//"oFnmZwYL8BjNShvunRE_gR-KKxTM";
                List<WeChatUser> list = ve.WeChatUser.Where(x => x.OpenId == OpenId).OrderBy(x => x.CreateDate).ToList();
                if (list == null || list.Count == 0) {
                    Write("null");
                    return;
                }
                Write(JsonConvert.SerializeObject(list));
            }
        }
        [WebMethod(EnableSession = true)]
        public void EQHDevices(string deviceId) {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                List<WeChatUser> list = ve.WeChatUser.Where(x => x.OpenId == OpenId).OrderBy(x => x.CreateDate).ToList();
                foreach (var item in list) {
                    item.State = false;
                    if (item.DeviceId == deviceId) {
                        item.State = true;
                    }
                }
                ve.SaveChanges();
                Write("true");
            }
        }
        [WebMethod(EnableSession = true)]
        public void EDelDevice(string deviceId) {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.DeviceId == deviceId);
                string js = "{\"Title\":\"推送消息\",\"Type\":7,\"OpenId\":\"" + OpenId + "\",\"Content\":\"\"}";
                Message ms = new Message("e家亲", js);
                PushHelper.PushMsg(wcu.Token, jsonSerializer.Serialize(ms));
                ve.WeChatUser.Remove(wcu);
                ve.SaveChanges();
                Write("true");
            }
        }

        [WebMethod(EnableSession = true)]
        public void EImage(string image, string deviceId) {
            if (Context.Session["OpenIdE"] != null) {
                string OpenId = Context.Session["OpenIdE"].ToString();
                String header = "data:image/jpeg;base64,";
                if (image.IndexOf(header) != 0) {
                    return;
                }
                // 去掉头部  
                image = image.Substring(header.Length);
                try {
                    byte[] bt = Convert.FromBase64String(image);
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(bt);
                    Bitmap bitmap = new Bitmap(stream);
                    string uid = Guid.NewGuid().ToString();
                    string fileName = string.Format("D:/webSites/www_root/Img/deviceHeadImg/{0}.jpg", uid);
                    //http://v.icoxtech.com/Img/deviceHeadImg/
                    bitmap.Save(fileName);
                    Image srcImage = Image.FromFile(fileName);
                    try {
                        Bitmap b = new Bitmap(500, 500);
                        Graphics g = Graphics.FromImage(b);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                        g.DrawImage(srcImage, new Rectangle(0, 0, 500, 500), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                        g.Dispose();
                        b.Save(string.Format("D:/webSites/www_root/Img/deviceHeadImg/imgs/{0}.jpg", uid));
                    } catch (Exception ex) {
                        throw ex;
                    }
                    WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == OpenId && x.DeviceId == deviceId);
                    if (wcu != null) {
                        wcu.HeadImg = string.Format("http://v.icoxtech.com/Img/deviceHeadImg/imgs/{0}.jpg", uid);
                        ve.SaveChanges();
                    }
                    //return bitmap;
                } catch (Exception e) {
                    Write(e.Message);
                }
                Write("true");
            }
        }

        [WebMethod]
        public void Get_UserInformation(string deviceId, string token) {
            VideoEntities ve = DBContextFactory.GetDbContext();
            Device_user du = new Device_user();
            List<WeChatUser> wlist = ve.WeChatUser.Where(x => x.DeviceId == deviceId).ToList();
            if (wlist.Count == 0) {
                HttpContext.Current.Response.Write("null");
                return;
            }
            foreach (var item in wlist) {
                item.Token = token;
                ve.SaveChanges();
            }
            Dictionary<string, List<Dictionary<string, string>>> dic2 = new Dictionary<string, List<Dictionary<string, string>>>();
            List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
            foreach (var item in wlist) {
                Dictionary<string, string> dic3 = new Dictionary<string, string>();
                dic3.Add("openid", item.OpenId);
                dic3.Add("lang", "zh_CN");
                list2.Add(dic3);
            }
            dic2.Add("user_list", list2);
            string data2 = du.GetUserInformation2(JsonConvert.SerializeObject(dic2), GetToken());
            HttpContext.Current.Response.Write(data2);
        }
        private void Write(string str) {
            Context.Response.Clear();
            Context.Response.ContentEncoding = Encoding.UTF8;
            Context.Response.ContentType = "text/plain";
            Context.Response.Write(str);
            Context.Response.End();
        }
        [WebMethod(EnableSession = true)]
        public void SetState_k(string ID) {
            int id = Convert.ToInt32(ID);
            AlarmClocksLogic.UpdateState(1, id);
        }
        [WebMethod(EnableSession = true)]
        public void SetState_g(string ID) {
            int id = Convert.ToInt32(ID);
            AlarmClocksLogic.UpdateState(0, id);
        }
    }
}
