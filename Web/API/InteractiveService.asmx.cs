using Common;
using Me.Common.Cache;
using Me.Common.Data;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using WeChat;

namespace Web.API
{
    /// <summary>
    /// InteractiveService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class InteractiveService : System.Web.Services.WebService
    {
        VideoEntities ve = new VideoEntities();
        SendHelper sendHelper = new SendHelper(2100339782, "A513K1EBXG7A", "c17ed4bad21f480013b649f1c91d7ead");
        //公众号回复接口
        [WebMethod]
        public void SendMsg(string device_id, string openId, string type, string content, string content2)
        {
            if (openId == "" || openId == null)
            {
                HttpContext.Current.Response.Write("设备未被绑定！"); //return "设备未被绑定！";
            }
            string data = "";
            switch (type)
            {
                case "text":
                    data = "{\"touser\":\"" + openId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
                    break;
                case "image":
                    data = "{\"touser\":\"" + openId + "\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"" + content + "\"}}";
                    break;
                case "voice":
                    data = "{\"touser\":\"" + openId + "\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"" + content + "\"}}";
                    break;
                case "video":
                    data = "{\"touser\":\"" + openId + "\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"" + content + "\", \"thumb_media_id\":\"" + content2 + "\"}}";
                    break;
                default:
                    break;
            }
            InteractiveHelper.SendMsg(data);
            HttpContext.Current.Response.Write("消息发送成功！");
            //return "消息发送成功！";
        }
        
        /// <summary>
        /// 显示绑定设备的用户
        /// </summary>
        /// <param name="deviceId">设备号</param>
        /// <param name="token">信鸽token</param>
        [WebMethod]
        public void Get_UserInformation(string deviceId, string token)
        {
            List<string> liststr = QrInteractiveManager.getOpenId(deviceId);
            if(liststr==null)
            {
                HttpContext.Current.Response.Write("null");
                return;
            }
            QrInteractiveManager.updateToken(deviceId,token);
            Dictionary<string, List<Dictionary<string, string>>> list = new Dictionary<string, List<Dictionary<string, string>>>();
            List<Dictionary<string, string>> userList = new List<Dictionary<string, string>>();
            foreach (var item in liststr)
            {
                Dictionary<string, string> userInfo = new Dictionary<string, string>();
                userInfo.Add("openid", item);
                userInfo.Add("lang", "zh_CN");
                userList.Add(userInfo);
            }
            list.Add("user_list", userList);
            string json = InteractiveHelper.GetWeChatInformation(JsonConvert.SerializeObject(list));
            HttpContext.Current.Response.Write(json);
        }
        

        //ASE加密 access_token
        [WebMethod]
        public void GetToken()
        {
            Write(InteractiveHelper.GetToken());
        }

        /// <summary>
        /// 设备上线公众号通知绑定过该设备的用户
        /// </summary>
        /// <param name="deviceId">设备号</param>
        [WebMethod]
        public void Send_Online_Msg(string deviceId)
        {
            DataTable dt = QrInteractiveManager.getDeviceBindOpIdAndDeId(deviceId);
            if (dt == null || dt.Rows.Count == 0)
            {
                HttpContext.Current.Response.Write("flase");
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    InteractiveHelper.SendMsg("{\"touser\":\"" + row["openId"] + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"你的设备：" + row["deviceName"] + "。已上线！\"}}");
                }
                HttpContext.Current.Response.Write("true");
            }
        }

        //用户管理模块

        //获取所有绑定过设备的用户
        [WebMethod(EnableSession=true)]
        public void GetMemberList()
        {
            if (Session["openId"]!=null)
            {
                //设备表查看openid当前使用设备的序号
                //在设备表获取绑定过指定设备上的所有用户
                //微信公众号写入openid获取用户的基本信息
                //传个前端
                string openId = Session["openId"].ToString();
                string deviceId = QrInteractiveManager.getDeviceId(openId);
                if (deviceId.Length>0)
                {
                    List<string> openlist = QrInteractiveManager.getOpenId(deviceId);
                    Dictionary<string, List<Dictionary<string, string>>> list = new Dictionary<string, List<Dictionary<string, string>>>();
                    List<Dictionary<string, string>> userlist = new List<Dictionary<string, string>>();
                    foreach(var item in openlist)
                    {
                        Dictionary<string, string> userInfo = new Dictionary<string, string>();
                        userInfo.Add("openid",item);
                        userInfo.Add("lang","zh_CN");
                        userlist.Add(userInfo);
                    }
                    list.Add("user_list",userlist);
                    string json = InteractiveHelper.GetWeChatInformation(JsonConvert.SerializeObject(list));
                    HttpContext.Current.Response.Write(json);
                }
            }
        }

        [WebMethod(EnableSession=true)]
        public void GetQrCodeImg()
        {
            if (Context.Session["openId"] != null)
            {
                string openId = Context.Session["openId"].ToString();
                StudentDeviceBind bind = ve.StudentDeviceBind.FirstOrDefault(sd => sd.openId == openId && sd.state == true && sd.userPhone != null);
                int sceneId = Convert.ToInt32(ve.StudentDeviceQRcode.OrderByDescending(qr=>qr.sceneId).FirstOrDefault().sceneId);
                sceneId = 1+sceneId;
                StudentDeviceQRcode code = new StudentDeviceQRcode();
                code.deviceId = bind.deviceId;
                code.token = bind.token;
                code.sceneId = sceneId.ToString();
                code.createTime = DateTime.Now;
                ve.StudentDeviceQRcode.Add(code);
                ve.SaveChanges();
                Byte[] imgBytes = QrCodeManager.GenerateTemp(InteractiveHelper.GetToken(),Convert.ToInt32(sceneId));
                WriteLog("GetEwm END:" + imgBytes.Length);
                Context.Response.Clear();
                Context.Response.ContentType = "image/jpg";
                Context.Response.BinaryWrite(imgBytes);
                Context.Response.End();
            }
            
        }

        [WebMethod(EnableSession=true)]
        public void GetDeviceName()
        {
            string openId = Convert.ToString(Session["openId"]);
            if (openId!=null&&openId!="")
            {
                string deviceName = QrInteractiveManager.getDeviceName(openId);
                var str = new { deviceName=deviceName };
                Write(JsonConvert.SerializeObject(str));
            }
        }

        [WebMethod(EnableSession=true)]
        public void IsAdmin() 
        {
            if(Session["openId"]!=null)
            {
                string openId = Session["openId"].ToString();
                string deviceId = QrInteractiveManager.getDeviceId(openId);
                string adminOpenId = QrInteractiveManager.getDeiveceFirstUser(deviceId);
                if(openId==adminOpenId)
                {
                    Write("true");
                }
                Write("你不是管理员，没有权限删除成员！");
            }
        }

        [WebMethod(EnableSession=true)]
        public void DeleteMember(string deleteOpenId)
        {
            if(Session["openId"]!=null)
            {
                string deviceId = QrInteractiveManager.getDeviceId(Session["openId"].ToString());
                string adminOpenId = QrInteractiveManager.getDeiveceFirstUser(deviceId);
                if (adminOpenId == Session["openId"].ToString())
                {
                    if(adminOpenId!=deleteOpenId)
                    {
                        string token = QrInteractiveManager.getDeviceToken(deviceId);
                        if (QrInteractiveManager.deleteMember(deleteOpenId, deviceId) > 0)
                        {
                            string js = "{\"Title\":\"推送消息\",\"Type\":\"7\",\"OpenId\":\"" + deleteOpenId + "\",\"Content\":\"\"}";
                            Messages msg = new Messages("互动平台", js);
                            sendHelper.PushMsg(token, JsonConvert.SerializeObject(msg));
                            Write("true");
                        }
                    }
                }
            }
        }




        //设备管理模块

        //获取用户下所有设备
        [WebMethod(EnableSession = true)]
        public void GetDeviceList() 
        {
            string openId = Convert.ToString(Session["openId"]);
            if(openId!=null)
            {
                
                List<StudentDeviceBind> list = ve.StudentDeviceBind.Where(bind=>bind.openId==openId&&bind.userPhone!=null).ToList();
                if (list.Count == 0)
                {
                    Write("null");
                }
                Write(JsonConvert.SerializeObject(list));
            }
        }

        //点击切换设备
        [WebMethod (EnableSession=true)]
        public void updateDeviceState(string deviceId)
        {
            string openId = Convert.ToString(Session["openId"]);
            if (Session["openId"] != null)
            {
                List<StudentDeviceBind> bindlist = ve.StudentDeviceBind.Where(bind => bind.openId == openId).ToList();
                foreach(var item in bindlist)
                {
                    if(item.deviceId==deviceId)
                    {
                        item.state = true;
                        continue;
                    }
                    item.state = false;
                }
                ve.SaveChanges();
                Write("true");
            }
        }

        //点击删除设备
        [WebMethod(EnableSession = true)]
        public void deleteDeviceInfo(string deviceId)
        {
            
            string openId = Session["openId"].ToString();
            if (openId != null)
            {
                var db = ve.StudentDeviceBind.FirstOrDefault(bind => bind.openId == openId && bind.deviceId == deviceId);
                ve.StudentDeviceBind.Remove(db);
                if (ve.SaveChanges() > 0)
                {
                    int id = Convert.ToInt32((from d in ve.StudentDeviceBind where d.openId == openId orderby d.createTime descending select d.id).FirstOrDefault());
                    if (id>0)
                    {
                        var item = ve.StudentDeviceBind.FirstOrDefault(fi => fi.id == id);
                        item.state = true;
                        ve.SaveChanges();
                    }
                    string js = "{\"Title\":\"推送消息\",\"Type\":\"7\",\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
                    Messages msg = new Messages("互动平台", js);
                    sendHelper.PushMsg(db.token, JsonConvert.SerializeObject(msg));
                    Write("true");
                }
            }
        }

        //点击修改设备头像
        [WebMethod(EnableSession = true)]
        public void updateDeviceAvatar(string image, string deviceId)
        {
            string openid = Session["openId"].ToString();
            if (openid != null)
            {
                string header = "data:image/jpeg;base64,";
                if (image.IndexOf(header) != 0)
                {
                    return;
                }
                image = image.Substring(header.Length);
                try
                {
                    byte[] bt = Convert.FromBase64String(image);
                    MemoryStream stream = new MemoryStream(bt);
                    Bitmap bit = new Bitmap(stream);
                    string uid = Guid.NewGuid().ToString();
                    //string fileName = string.Format("D:/webSites/www_root/Img/deviceHeadImg/{0}.jpg", uid);
                    string fileName = string.Format("D:/webSites/www_root/Img/deviceHeadImg/{0}.jpg", uid);
                    bit.Save(fileName);
                    Image srcImg = Image.FromFile(fileName);
                    try
                    {
                        Bitmap b = new Bitmap(500, 500);
                        Graphics g = Graphics.FromImage(b);
                        g.InterpolationMode = InterpolationMode.Default;
                        g.DrawImage(srcImg, new Rectangle(0, 0, 500, 500), new Rectangle(0, 0, srcImg.Width, srcImg.Height), GraphicsUnit.Pixel);
                        g.Dispose();
                        b.Save(string.Format("D:/webSites/www_root/Img/deviceHeadImg/imgs/{0}.jpg", uid));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    QrInteractiveManager.updateDeviceAvatar(openid, deviceId, string.Format("http://v.icoxtech.com/Img/deviceHeadImg/imgs/{0}.jpg", uid));

                }
                catch (Exception e)
                {
                    WriteLog(DateTime.Now + "InteractiveService.error:" + e.Message);
                }
                Write("true");
            }
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
