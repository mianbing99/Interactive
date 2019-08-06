using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat
{
    public class WeChatInteractiveConfig
    {
        private static NameValueCollection weixinInteractive = (NameValueCollection)ConfigurationManager.GetSection("WeiXinInteractive");

        public static string WeChatId 
        {
            get { return weixinInteractive["WeChatId"]; }
        }

        public static string AppId 
        {
            get { return weixinInteractive["AppId"];}
        }

        public static string Token 
        {
            get { return weixinInteractive["Token"]; }
        }

        public static string AppSecret 
        {
            get { return weixinInteractive["AppSecret"]; }
        }

        public static string EncodingAESKey 
        {
            get { return weixinInteractive["EncodingAESKey"]; }
        }
    }
}
