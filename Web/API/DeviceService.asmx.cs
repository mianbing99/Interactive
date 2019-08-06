using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using WeChat;
using Me.Common.Data;

namespace Web.API {
    /// <summary>
    /// DeviceService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DeviceService : System.Web.Services.WebService {
        Device_user du = new Device_user();
        [WebMethod]
        public void SendNews(string device_id, string openId, string type, string content, string content2) {
            //string openId = QrChildManager.GetOpenId(device_id);
            if (openId == "" || openId == null) {
                HttpContext.Current.Response.Write("设备未被绑定！"); //return "设备未被绑定！";
            }
            string data = "";
            switch (type) {
                case "text":
                    data = "{\"touser\":\""+ openId +"\",\"msgtype\":\"text\",\"text\":{\"content\":\""+ content +"\"}}";
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
            du.SendNews(data);
            HttpContext.Current.Response.Write("消息发送成功！");
            //return "消息发送成功！";
        }
        //获得设备二维码和设备ID
        public Dictionary<string, object> GetQrticket(string product_id) {

            //HttpContext.Current.Response.Write(du.GetQrticket(product_id));
            string json = du.GetQrticket(product_id);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dic;
        }

        [WebMethod]
        public void GetToken() {

            HttpContext.Current.Response.Write(du.SelectToken());
            //return du.GetQrticket(product_id);
        }
        [WebMethod]
        public void GetQrticketByMac(string mac, string token) {
            if (token == "" || token == null || token == "0") {
                HttpContext.Current.Response.Write("token is null");
                return;
            } else {
                string qrticket = null;
                if (qrticket == null) {
                    Dictionary<string, object> dic = GetQrticket("42321");
                    string json = UpdateDevice(dic["deviceid"].ToString(), mac, token, dic["qrticket"].ToString());
                    Dictionary<string, List<Dictionary<string, object>>> dic2 = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(json);
                    if (dic2["resp"][0]["errcode"].ToString() == "0" && dic2["resp"][0]["errmsg"].ToString().ToUpper() == "OK") {
                        qrticket = QrChildManager.GetQrticket(mac, token);
                    }
                }
                HttpContext.Current.Response.Write(qrticket);
            }
        }
        //为设备授权，可以绑定
        public string UpdateDevice(string device_id, string mac, string xingeToken, string qrticket) {
            try {
                QrChildManager.ChildUser(xingeToken, device_id, mac, qrticket);
            } catch (Exception ex) {
                HttpContext.Current.Response.Write(ex.Message);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("device_num", "1");
            Dictionary<string, string> dic2 = new Dictionary<string, string>();
            //string json = "[{id:" + device_id + ",mac:" + mac + ",connect_protocol:4,auth_key:1234567890ABCDEF1234567890ABCDEF,close_strategy:1,conn_strategy:1,crypt_method:0,auth_ver:0,manu_mac_pos:-1,ser_mac_pos:-2}]";
            dic2.Add("id", device_id);
            dic2.Add("mac", mac);
            dic2.Add("connect_protocol", "4");
            dic2.Add("auth_key", "1234567890ABCDEF1234567890ABCDEF");
            dic2.Add("close_strategy", "1");
            dic2.Add("conn_strategy", "1");
            dic2.Add("crypt_method", "0");
            dic2.Add("auth_ver", "0");
            dic2.Add("manu_mac_pos", "-1");
            dic2.Add("ser_mac_pos", "-2");
            dic.Add("device_list", dic2);
            dic.Add("op_type", "1");
            string jsonData = LitJson.JsonMapper.ToJson(dic);
            jsonData = jsonData.Insert(jsonData.LastIndexOf('{'), "[").Insert(jsonData.IndexOf('}') + 2, "]");
            return du.UpdateDevice(jsonData);
            //return du.UpdateDevice(jsonData);
        }

        [WebMethod]
        public void Get_UserInformation(string deviceId, string token) {
            DataTable dt = QrChildManager.GetOpenId(deviceId);
            if (dt.Rows.Count == 0) {
                HttpContext.Current.Response.Write("null");
                return;
            }
            string sql = string.Format("update ChildChatUser set Token = '{0}' where deviceId = '{1}'", token, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
                List<string> list = new List<string>();
                foreach (DataRow item in dt.Rows) {
                    list.Add(item["OpenId"].ToString());
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
                HttpContext.Current.Response.Write(data2);
        }
        
        [WebMethod]
        public void Send_Online_News(string deviceId) {
            DataTable dt = QrChildManager.GetOpenId(deviceId);
            if (dt == null || dt.Rows.Count == 0) {
                HttpContext.Current.Response.Write("flase");
            } else {
                foreach (DataRow row in dt.Rows) {
                    du.SendNews("{\"touser\":\"" + row["OpenId"] + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"你的设备：" + row["deviceName"] + "。已上线！\"}}");
                }
                HttpContext.Current.Response.Write("true");
            }
        }
    }
}
