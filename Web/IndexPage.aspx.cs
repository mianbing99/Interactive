using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat;

namespace Web {
    public partial class IndexPage : System.Web.UI.Page {

        private string postToken = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private string postTokenNurl = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private string scope = "snsapi_base";

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["OpenId"] == null) {
                GetOpenId();
            }
        }


        private void GetOpenId() {
            string _Code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(_Code)) {
                string url = string.Format(postToken + "?appid={0}&redirect_uri={1}&response_type=code&scope={2}#wechat_redirect", WeChatConfig.AppIdChild, HttpUtility.UrlEncode(Request.Url.AbsoluteUri), scope);
                Response.Redirect(url);
            } else {
                string url = string.Format(postTokenNurl + "?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatConfig.AppIdChild, WeChatConfig.AppSecretChild, _Code);
                string str = Submit.HttpGet(url);
                JObject json = JObject.Parse(str);
                string OpenId = json["openid"].ToString();
                Session["OpenId"] = OpenId;
            }
        }
    }
}