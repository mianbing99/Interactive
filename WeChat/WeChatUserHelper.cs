using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WeChat
{
    public class WeChatUserHelper
    {
        public static string GetUserInfo(string token, string openid)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token, openid);
            string str = Submit.HttpGet(url);
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //WeChatUserInfo wui = jsonSerializer.Deserialize<WeChatUserInfo>(str);
            //return wui;
            return str;
        }
        public static string GetChildUserInfo(string token, string openid) {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token, openid);
            string str = Submit.HttpGet(url);
            //return wui;
            return str;
        }
    }


    //public class WeChatUserInfo
    //{
    //    public int subscribe { get; set; }
    //    public string openid { get; set; }
    //    public string nickname { get; set; }
    //    public int sex { get; set; }
    //    public string language { get; set; }
    //    public string city { get; set; }
    //    public string province { get; set; }
    //    public string country { get; set; }
    //    public string headimgurl { get; set; }
    //    public long subscribe_time { get; set; }
    //    public string unionid { get; set; }
    //    public string remark { get; set; }
    //    public int groupid { get; set; }
    //}
}
