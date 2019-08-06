using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat;

namespace Web.Child {
    public partial class EjqUsers : System.Web.UI.Page {
        private string POSTCODEURL = "https://open.weixin.qq.com/connect/oauth2/authorize";
        private string POSTTOKENURL = "https://api.weixin.qq.com/sns/oauth2/access_token";
        private string SCOPE = "snsapi_base";

        protected void Page_Load(object sender, EventArgs e) {
            //Session["OpenIdE"] = "oFnmZwYL8BjNShvunRE_gR-KKxTM";
            if (Session["OpenIdE"] == null) {
                GetOpenId();
            }
        }
        private void GetOpenId() {
            string _Code = Request.QueryString["code"];
            if (string.IsNullOrEmpty(_Code)) {
                string url = string.Format(POSTCODEURL + "?appid={0}&redirect_uri={1}&response_type=code&scope={2}#wechat_redirect", WeChatConfig.AppId, HttpUtility.UrlEncode(Request.Url.AbsoluteUri), SCOPE);
                Response.Redirect(url);
            } else {
                string url = string.Format(POSTTOKENURL + "?appid={0}&secret={1}&code={2}&grant_type=authorization_code", WeChatConfig.AppId, WeChatConfig.AppSecret, _Code);
                string str = Submit.HttpGet(url);
                JObject json = JObject.Parse(str);
                string OpenId = json["openid"].ToString();
                Session["OpenIdE"] = OpenId;
            }
        }
    }
}