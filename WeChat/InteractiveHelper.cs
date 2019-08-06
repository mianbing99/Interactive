using Common;
using Me.Common.Cache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WeChat
{
    public class InteractiveHelper
    {
        /// <summary>
        /// 客服接口 发消息
        /// </summary>
        /// <param name="jsonStr">发送内容</param>
        public static void SendMsg(string jsonStr)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token="+GetToken();
            string data = Submit.HttpPost(url,jsonStr);
        }

        /// <summary>
        /// 批量获取公众号用户信息
        /// </summary>
        /// <param name="jsonStr">openid</param>
        /// <returns></returns>
        public static string GetWeChatInformation(string jsonStr)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token=" + GetToken();
            return Submit.HttpPost(url, jsonStr);
        }
        public static string GetToken()
        {
            var key = WeChatInteractiveConfig.WeChatId;
            var token = CacheCore.Get<string>(key);
            if (string.IsNullOrEmpty(token))
            {
                token = WeChatInteractiveHelper.GetToken(WeChatInteractiveConfig.AppId, WeChatInteractiveConfig.AppSecret);
                CacheCore.Set<string>(key, token, 60);
            }
            return token;
        }
        public static void WriteLog(string text)
        {
            StreamWriter strem = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log/log" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true);
            strem.WriteLine(text);
            strem.Close();
        }
    }
}
