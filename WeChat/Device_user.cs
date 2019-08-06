using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Common;
using Me.Common.Data;
using Me.Common.Extension;
using Me.Common.Configuration;
using Me.Common.Cache;

namespace WeChat {
    public class Device_user {
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns>access_token</returns>
        public string GetToken() {
            string appid = WeChatConfig.AppIdChild;
            string secret =WeChatConfig .AppSecretChild;
            string data = requestGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret));
            var token = data.ToObj<Dictionary<string, object>>();
            return token["access_token"].ToString();
        }
        /// <summary>
        /// 根据open_id 得到device_id:设备ID
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public List<string> GetDevice_id(string open_id) {
            open_id = open_id.Replace("\n", "").Replace("\r", "").Replace(" ", "");
            string token = SelectToken();
            string url = string.Format("https://api.weixin.qq.com/device/get_bind_device?access_token={0}&openid={1}", token, open_id);
            string data = requestGet(url);
            var jsonData = data.ToObj<Dictionary<string, object>>();
            string device_list = jsonData["device_list"].ToString();
            if (device_list.Length<3) {
                return null;
            }
            //DBHelper.ExecuteNonQueryString("insert into xmlString values ('" + device_list + "')");
            device_list = device_list.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            var deviceId = device_list.ToObj<List<Dictionary<string, object>>>();
            List<string> list = new List<string>();
            foreach (var item in deviceId) {
                list.Add(item["device_id"].ToString());
            }
            return list;
        }
        /// <summary>
        /// 如token过期重新获取token
        /// </summary>
        /// <returns></returns>
        public string SelectToken() {
            var key =WeChatConfig.WeChildChatId;
           var token = CacheCore.Get<string>(key);
            if (string.IsNullOrEmpty(token))
            {
                token = GetToken();
                CacheCore.Set<string>(key, token, 60);
            }
            return token;
        }
        /// <summary>
        /// 设备授权获取二维码和设备ID
        /// </summary>
        /// <param name="product_id"></param>
        /// <returns></returns>
        public string GetQrticket(string product_id) {
            string token = SelectToken();
            string url = string.Format("https://api.weixin.qq.com/device/getqrcode?access_token={0}&product_id={1}", token, product_id);
            string data = requestGet(url);
            return data;
        }
        /// <summary>
        /// 设备授权，修改设备状态
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <returns></returns>
        public string UpdateDevice(string jsonData) {
            string url = "https://api.weixin.qq.com/device/authorize_device?access_token=" + SelectToken();
            string data = requestPost(url, jsonData);
            return data;
        }
        public void SendNews(string jsonData) {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + SelectToken();
            string data = requestPost(url, jsonData);
        }

        public void SendNews2(string jsonData, string token) {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token;
            string data = requestPost(url, jsonData);
        }

        public string GetUsers(string deviceId) {
            string url = string.Format("https://api.weixin.qq.com/device/get_openid?access_token={0}&device_type={1}&device_id={2}", SelectToken(), "gh_46052aa29512", deviceId);
            string data = requestGet(url);
            return data;
        }

        public string GetUserInformation(string jsonData) {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token=" + SelectToken();
            string data = requestPost(url, jsonData);
            return data;
        }
        public string GetUserInformation2(string jsonData, string token) {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token=" + token;
            string data = requestPost(url, jsonData);
            return data;
        }
        public static string requestGet(string url) {

            string result = "";
            try {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                    result = reader.ReadToEnd();
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }

        public static string requestPost(string url, string content) {
            string result = "";
            try {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                #region 添加Post 参数
                byte[] data = Encoding.UTF8.GetBytes(content);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream()) {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                #endregion

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                    result = reader.ReadToEnd();
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
    }
}
