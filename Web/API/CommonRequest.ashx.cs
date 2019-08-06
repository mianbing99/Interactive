using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat;

namespace Web.API {
    /// <summary>
    /// CommonRequest 的摘要说明
    /// </summary>
    public class CommonRequest : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            int flag = Convert.ToInt32(context.Request.QueryString["flag"]);
            string deviceId = context.Request.QueryString["deviceId"];//设备ID
            string studyName = context.Request.QueryString["studyName"];//学习内容
            if (flag == 1) {
                AddStudy(deviceId, studyName);
                return;
            }
        }
        /// <summary>
        /// 添加幼儿学习情况记录
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="studyName"></param>
        private void AddStudy(string deviceId, string studyName) {
            if (deviceId != "") {
                MessageChild.AddStudyRecord(deviceId, studyName);
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}