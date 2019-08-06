using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat
{
    public class WeChatConfig
    {
        private static NameValueCollection WeiXin = (NameValueCollection)ConfigurationManager.GetSection("WeiXin");
        private static NameValueCollection WeiXinChild = (NameValueCollection)ConfigurationManager.GetSection("WeiXinChild");
        public static string WeChatId
        {
            get { return WeiXin["WeChatId"]; }
        }
        public static string AppId
        {
            get { return WeiXin["AppId"]; }
        }
        public static string Token
        {
            get { return WeiXin["Token"]; }
        }
        public static string AppSecret
        {
            get { return WeiXin["AppSecret"]; }
        }
        public static string EncodingAESKey
        {
            get { return WeiXin["EncodingAESKey"]; }
        }

        public static string WeChildChatId {
            get { return WeiXinChild["WeChildChatId"]; }
        }
        public static string AppIdChild {
            get { return WeiXinChild["AppIdChild"]; }
        }
        public static string TokenChild {
            get { return WeiXinChild["TokenChild"]; }
        }
        public static string AppSecretChild {
            get { return WeiXinChild["AppSecretChild"]; }
        }
        public static string EncodingAESKeyChild {
            get { return WeiXinChild["EncodingAESKeyChild"]; }
        }
    }
}
