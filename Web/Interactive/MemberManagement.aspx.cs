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

namespace Web.Interactive
{
    public partial class MemberManagement : System.Web.UI.Page
    {
        public int ID;
        public string code;
        public static string openId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    code = Convert.ToString(Request.QueryString["code"]);
                    Session["code"] = code;
                    string returnStr = getOpenId(code);
                    JObject json = JObject.Parse(returnStr);
                    openId = json["openid"].ToString();//得到OpenId
                    Session["openId"] = openId;
                }
            }
        }

        /// <summary>
        /// 获取Json值集合
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getOpenId(string code)
        {
            string studyUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxf9ade153117809bf&secret=c876b01424956a2f7f7d1bee9dbb0acd&code=" + code + "&grant_type=authorization_code ";
            return HttpGet(studyUrl, "");
        }
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}