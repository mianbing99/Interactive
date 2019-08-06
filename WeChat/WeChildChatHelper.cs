using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WeChat {
    public class WeChildChatHelper {
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns>验证是否通过</returns> 
        public static bool CheckBabySignature(string signature, string timestamp, string nonce) {
            string[] ArrTmp = { WeChatConfig.TokenChild, timestamp, nonce };
            Array.Sort(ArrTmp);//字典排序
            string tmpStr = string.Join("", ArrTmp);
            if (GetSHA1(tmpStr).ToLower() == signature)
                return true;
            return false;
        }
        /// <summary>
        /// SHA1字符串加密
        /// </summary>
        /// <param name="str">须加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        private static string GetSHA1(string str) {
            byte[] cleanBytes = Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        public static Dictionary<string, object> GetBabyToken() {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WeChatConfig.AppIdChild, WeChatConfig.AppSecretChild);
            string returnStr = Submit.HttpGet(url);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dic = (Dictionary<string, object>)serializer.DeserializeObject(returnStr);
            return dic;
        }
    }
}
