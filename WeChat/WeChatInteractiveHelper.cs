using Common;
using Me.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WeChat
{
    public class WeChatInteractiveHelper
    {

        public static bool CheckSignature(string token,string signature,string timestamp,string nonce) 
        {
            //将token timestamp nonce三个参数进行字典序排序
            string[] ArrTmp = { token,timestamp,nonce};
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("",ArrTmp);
            //加密sha1对比signature字符串
            if (GetSHA1(tmpStr).ToLower() == signature)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// SHA1字符串加密
        /// </summary>
        /// <param name="str">须加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string GetSHA1(string str) 
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-","");
        }

        public static string GetToken(string appid, string secret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            string returnStr = Submit.HttpGet(url);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = (Dictionary<string, object>)serializer.DeserializeObject(returnStr);
            return dictionary["access_token"].ToString();
        }
    }
}
