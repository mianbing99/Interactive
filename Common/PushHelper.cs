using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class PushHelper
    {
        public static string RESTAPI_PUSHSINGLEDEVICE = "http://openapi.xg.qq.com/v2/push/single_device";
        public static string HTTP_POST = "POST";

        public static long ACCESSID = 2100149571;
        public static string ACCESSKEY = "AQ61284NWPFF";
        public static string SECRETKEY = "a32f65d343370948b5b629f17a2d3095";
        public static string PushMsg(string token, string str)
        {
            Dictionary<string, object> parasDic = new Dictionary<string, object>();
            parasDic.Add("access_id", ACCESSID);
            parasDic.Add("timestamp", GetTimeStamp());
            parasDic.Add("message_type", 2);
            parasDic.Add("device_token", token);
            parasDic.Add("expire_time", 86400);
            parasDic.Add("message", str);
            string Sign = PushHelper.GenerateSign(HTTP_POST, RESTAPI_PUSHSINGLEDEVICE, parasDic);
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


        public static string GenerateSign(string method, string url, Dictionary<string, object> paras)
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
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
    
}
