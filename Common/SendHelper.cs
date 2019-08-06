using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class SendHelper
    {

        public static string RESTAPI_PUSHSINGLEDEVICE = "http://openapi.xg.qq.com/v2/push/single_device";

        public static string HTTP_POST = "POST";

        public long ACCESSID;

        public string ACCESSKEY;

        public string SECRETKEY;

        public SendHelper()
        {

        }

        public SendHelper(long accessId, string accessKey, string secretKey)
        {
            this.ACCESSID = accessId;
            this.ACCESSKEY = accessKey;
            this.SECRETKEY = secretKey;
        }
        public string PushMsg(string token, string str)
        {
            Dictionary<string, object> parasDic = new Dictionary<string, object>();
            parasDic.Add("access_id", ACCESSID);
            parasDic.Add("timestamp", GetTimeStamp());
            parasDic.Add("message_type", 2);
            parasDic.Add("device_token", token);
            parasDic.Add("expire_time", 86400);
            parasDic.Add("message", str);
            string Sign = GenerateSign(HTTP_POST, RESTAPI_PUSHSINGLEDEVICE, parasDic);
            parasDic.Add("sign", Sign);
            string paras = "";
            foreach (string item in parasDic.Keys)
            {
                paras += item + "=" + parasDic[item] + "&";
            }
            paras = paras.Length > 0 ? paras.Substring(0, paras.Length - 1) : paras;
            string returnStr = Submit.HttpPost(RESTAPI_PUSHSINGLEDEVICE, paras);
            return returnStr;
        }


        public string GenerateSign(string method, string url, Dictionary<string, object> paras)
        {
            List<KeyValuePair<string, object>> paraList = paras.ToList();
            paraList.Sort((firstPair, nextPair) =>
            {
                return firstPair.Key.CompareTo(nextPair.Key);
            });
            string paramStr = string.Empty;
            paraList.ForEach(x =>
            {
                paramStr += x.Key + "=" + x.Value.ToString();
            });
            WebRequest request = WebRequest.Create(url);
            string text = method + request.RequestUri.Host + request.RequestUri.AbsolutePath + paramStr + SECRETKEY;
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        public long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
