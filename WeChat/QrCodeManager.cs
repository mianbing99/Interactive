using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace WeChat
{
    public class QrCodeManager
    {
        /// <summary>
        /// 临时二维码地址
        /// </summary>
        /// 使用string.format时，报：字符串格式错误，因为其中有{
        //private const string TEMP_URL = "{\"expire_seconds\": 1800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": {0}}}}";
        /// <summary>
        /// 解决办法，将原有字符串中的一个{用两个{代替
        /// </summary>
        private const string TEMP_JSON_DATA = "{{\"expire_seconds\": 259200, \"action_name\": \"QR_SCENE\", \"action_info\": {{\"scene\": {{\"scene_id\": {0}}}}}}}";
        /// <summary>
        /// 获取ticket的URL
        /// </summary>
        private const string GET_TICKET_URL = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        /// <summary>
        /// 获取二维码URL
        /// </summary>
        private const string GET_CODE_URL = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
        /// <summary>
        /// 根据场景ID获取ticket
        /// </summary>
        /// <param name="sceneID">场景ID</param>
        /// <returns></returns>
        private static string GetTicket(string token, int sceneID)
        {
            WriteLog("GetTicket");
            if (sceneID < 1 && sceneID > 100000)
                return null;
            string data = string.Format(TEMP_JSON_DATA, sceneID.ToString());
            string returnStr = Submit.HttpPost(string.Format(GET_TICKET_URL, token), data);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dic = (Dictionary<string, object>)serializer.DeserializeObject(returnStr);
            return dic["ticket"] + "";
        }
        public static void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
         ///<summary>
        /// 创建临时二维码
        /// </summary>
        /// <param name="sceneID">场景id，int类型</param>
        /// <returns></returns>
        public static Byte[] GenerateTemp(string token, int sceneID)
        {
            WriteLog("GenerateTemp");
            string ticket = GetTicket(token, sceneID);
            if (ticket == null)
            {
                return null;
            }
            HttpClient client = new HttpClient();
            Byte[] bytes = client.GetByteArrayAsync(string.Format(GET_CODE_URL, HttpUtility.UrlEncode(ticket))).Result;
            return bytes;
        }
    }
}
