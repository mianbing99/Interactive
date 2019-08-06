using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Web.Common;
using Models;
using Web.API;
using WeChat;
using Web.Admin.Class;
using System.Threading;
using Newtonsoft.Json.Linq;
using Parsing;
using Me.Common;
using Me.Common.Redis;
using Me.Common.Data;

namespace Web
{
    /// <summary>
    /// OnlineVideo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://v.icoxedu.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OnlineVideo : System.Web.Services.WebService
    {
        VideoHandler vh = new VideoHandler();
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetType(int tid)
        {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.GetTypeList(tid, ref count));
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetTypeById(int id) {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.GetTypeListById(id, ref count));
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVideoPage(int IndexPage, int PageSize, int TypeId)
        {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.GetVideoPage(IndexPage, PageSize, TypeId, ref count));
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SearchVideoPage(int IndexPage, int PageSize, string KeyWord) {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.SearchVideoPage(IndexPage, PageSize, KeyWord, ref count));
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUser(string vid, string acc, string pwd, string getType) {
            if (acc == "" || pwd == "" || acc == null || pwd == null) {
                return;
            } 
            try {
                string thisdate = DateTime.Now.ToString("yyyy/MM/dd");
                //准备记录详细数据
                string clientIP = HttpContext.Current.Request.UserHostAddress;//"120.38.201.249";
                string region = "", addr = "", isp = "", type = "";
                try {
                    addr = GetAPI(clientIP);
                    region = GetValue(addr, "region\":\"", "\",\"region_id");
                    isp = GetValue(addr, "isp\":\"", "\",\"isp_id");
                } catch (Exception e) {
                    region = e.Message;
                    isp = e.Message;
                }
                string sqlxxsj = string.Format(@" insert into UserAccessRecord(vid,account,pwd,ip,time,addr,isp,getType) 
                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ",
                        vid, acc, pwd, clientIP, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff dddd"), region, isp, getType);

                //准备记录每日点击次数

                string sqlscn = string.Format(" select nums from DailyHits where date ='{0}' ", thisdate);//查询当日是否有记录次数,有则更新数据,无则新增数据
                string cnums = SqlFunction.Sql_ReturnNumberES(sqlscn);
                string sqludh = "";
                if (cnums != "") {
                    sqludh = string.Format(" update DailyHits set nums='{0}' where date='{1}' ",
                        Convert.ToInt32(cnums) + 1, thisdate);//开始更新个数,个数+1
                } else {
                    sqludh = string.Format(" insert into DailyHits(date,nums) values('{0}','{1}')", thisdate, "1");
                }

                //准备记录每日在线人数
                //查询当前用户是否出现过在今日点击,出现则不增加用户数,没出现则增加用户数
                 
                string sqlsup = string.Format("select count(*) from UserAccessRecord where account='{0}' and pwd='{1}'", acc, pwd);
                string ishave = SqlFunction.Sql_ReturnNumberES(sqlsup);
                string sqlidon = "";
                if (ishave == "0") {
                    string sqlsdon = string.Format("select nums from DailyOnlineNum where date='{0}'", thisdate);
                    string isno1 = SqlFunction.Sql_ReturnNumberES(sqlsdon);
                    if (isno1 != "") {
                        sqlidon = string.Format(" update DailyOnlineNum set nums='{0}' where date='{1}' ",
                                                Convert.ToInt32(isno1) + 1, thisdate);
                    } else {
                        sqlidon = string.Format(" insert into DailyOnlineNum(date,nums) values('{0}','{1}') ", thisdate, "1");
                    }
                }

                //准备记录类型占比
                try {
                    type = VidtoTitle.Vid_to_Type(Convert.ToInt32(vid));
                } catch (Exception e) {
                    type = "1:" + e.Message;
                }
                string sqlt = "";
                string sqlst = string.Format("select top 1 Id,countNum from UserAccessType where videoType='{0}'", type);
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(sqlst);
                if (ds.Tables[0].Rows.Count != 0) {
                    sqlt = string.Format("update UserAccessType set countNum='{0}' where id='{1}'",
                        Convert.ToInt32(ds.Tables[0].Rows[0]["countNum"]) + 1, ds.Tables[0].Rows[0]["id"].ToString());

                } else {
                    sqlt = string.Format("insert into UserAccessType values('{0}','{1}')", type, 1);
                }
                ds.Clear(); ds.Dispose();
                try {
                    string zxsql = sqlxxsj + " " + sqludh + " " + sqlidon + " " + sqlt;
                    SqlFunction.Sql_ReturnNumberENQ(zxsql);
                } catch (Exception e){
                    string errorsql = string.Format("insert into UserAccessRecord(time) values('{0}')",e.Message);
                    SqlFunction.Sql_ReturnNumberENQ(errorsql);
                }
            } catch (Exception e) {
                string errorsql = string.Format("insert into UserAccessRecord(time) values('{0}')", e.Message);
                SqlFunction.Sql_ReturnNumberENQ(errorsql);
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVideoPath(int Id) {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.GetVideoUrl(Id, ref count));
            json = json.Replace(" ", "");
            json = json.Replace("\\t", "");
            json = json.Replace("\\r", "");
            json = json.Replace("\\n", "");
            json = json.Replace("\\s", "");
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            Write(json);
            //setms(Id);
        }
        private void setms(int Id) {
            try {
                string clientIP = HttpContext.Current.Request.UserHostAddress;//"120.38.201.249";

                string type = "";
                try {
                    type = VidtoTitle.Vid_to_Type(Id);
                } catch (Exception e) {
                    type = "1:" + e.Message;
                }
                //记录类型
                string sqlt = "";
                string sqlst = string.Format("select top 1 Id,countNum from UserAccessType where videoType='{0}'", type);
                DataSet ds = new DataSet();
                ds = SqlFunction.Sql_DataAdapterToDS(sqlst);
                if (ds.Tables[0].Rows.Count != 0) {
                    sqlt = string.Format("update UserAccessType set countNum='{0}' where id='{1}'",
                        Convert.ToInt32(ds.Tables[0].Rows[0]["countNum"]) + 1, ds.Tables[0].Rows[0]["id"].ToString());

                } else {
                    sqlt = string.Format("insert into UserAccessType values('{0}','{1}')", type, 1);
                }
                //记录IP
                string sqli = "";
                string sqlsi = string.Format("select top 1 Id,ipNum from UserAccessIp where userIp='{0}'", clientIP);
                DataSet dsi = new DataSet();
                dsi = SqlFunction.Sql_DataAdapterToDS(sqlsi);


                //{"region":"河北省","region_id":"130000","city":"唐山市","city_id":"130200","county":"","county_id":"-1","isp":"移动","isp_id":"100025","ip":"183.197.61.247"}}
                if (dsi.Tables[0].Rows.Count != 0) {
                    sqli = string.Format("update UserAccessIp set ipNum='{0}' where id='{1}'",
                        Convert.ToInt32(dsi.Tables[0].Rows[0]["ipNum"]) + 1, dsi.Tables[0].Rows[0]["id"].ToString());
                } else {
                    string addr = "", region = "";
                    try {
                        addr = GetAPI(clientIP);
                        region = GetValue(addr, "region\":\"", "\",\"region_id");
                    } catch (Exception e) {
                        region = e.Message;
                    }
                    sqli = string.Format("insert into UserAccessIp (userip,ipnum,addr) values('{0}','{1}','{2}')", clientIP, 1, region);
                }
                //记录时间
                string sql = "";
                sql = string.Format("insert into UserAccessTj(vid,clickTime,clickdate) values('{0}','{1}','{2}')", Id, DateTime.Now.ToString(), DateTime.Now.ToString("yyyy-MM-dd")) + " " + sqlt + " " + sqli;
                SqlFunction.Sql_ReturnNumberENQ(sql);
            } catch {

            }
        }
        /// <summary>
        /// 请求淘宝API，获取返回的Json结果
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns></returns>
        string GetAPI(string ipAddress) {
            var web = new WebClient();
            var result = web.DownloadString("http://ip.taobao.com/service/getIpInfo.php?ip=" + ipAddress);
            return UnicodeToChinese(result);
        }
        /// <summary>
        /// 将Unicode转换为中文
        /// </summary>
        /// <param name="str">Unicode原文</param>
        /// <returns></returns>
        string UnicodeToChinese(string str) {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, m1 => {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }
        /// <summary>  
        /// 指定开始字符串和结束字符串,截取中间的字符  
        /// </summary>  
        /// <param name="str">要截取的字符串</param>  
        /// <param name="s">开始字符串</param>  
        /// <param name="e">结束字符串</param>  
        /// <returns></returns>  
        public static string GetValue(string str, string s, string e) {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void AESGetVideoPath(int Id) {
            int count = 0;
            string json = jsonSerializer.Serialize(vh.GetVideoUrl(Id, ref count));
            json = json.Replace(" ", "");
            json = json.Replace("\\t", "");
            json = json.Replace("\\r", "");
            json = json.Replace("\\n", "");
            json = json.Replace("\\s", "");
            json = "{\"Count\":\"" + count + "\",\"Data\":" + json + "}";
            json = EncryptHelper.NewEncrypt(json);
            Write(json);
            //setms(Id);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ParseVideoPath(string path) {
            path = (path + "").Trim();
            var result = RedisCore.Get<string>(path);
            if (string.IsNullOrEmpty(result))
            {
                int code = 0;
                string data = "解析失败";
                try
                {
                    if (path.Contains("v.youku.com"))
                    {
                        string url = string.Format("http://video.mc54.cn/v.g?i={0}", path);
                        if (!string.IsNullOrEmpty(url))
                        {
                            data = EncryptHelper.Encrypt(url);
                            code = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    code = -1;
                    data = ex.Message;
                    VideoHandler.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                    VideoHandler.WriteLog("-----------------");
                }
                result = new { code = code, parse = data }.ToJsonStr();
                RedisCore.Set(path, result, 525600);
            }
            Write(result);

            //int code = 0;
            //string text3 = "";
            //try
            //{
            //    if (path.Contains("v.youku.com"))
            //    {
            //        string url2 = string.Format("http://www.zsctc-api.com:8001/ok?url={0}", path);
            //        string htmlSource = GetHtmlSource(url2);
            //        if (!htmlSource.Contains("\"null\""))
            //        {
            //            JObject jObject = JObject.Parse(htmlSource);
            //            if (jObject["videoUrl"] != null)
            //            {
            //                text3 = jObject["videoUrl"].ToString();
            //                if (text3 != "")
            //                {
            //                    text3 = EncryptHelper.Encrypt(text3);
            //                    code = 1;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            code = 0;
            //            text3 = "解析失败";
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    code = -1;
            //    text3 = e.Message;
            //    VideoHandler.WriteLog(e.Message+ "\r\n"+e.StackTrace);
            //    VideoHandler.WriteLog("-----------------");
            //}
            //string json = "{\"code\":" + code + ",\"parse\":\"" + text3 + "\"}";
            //Write(json);
        }
        public string GetVid(string url)
         {
            string strRegex = "(?<=id_)(\\w+)";
            Regex reg = new Regex(strRegex);
            Match match = reg.Match(url);
            return match.ToString();
        }
        internal static string GetHtmlSource(string url) {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "*/*";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.Referer = url;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            responseStream.Close();
            return result;
        }
        //public class Worker {
        //    // This method will be called when the thread is started.
        //    public void DoWork() {
        //        while (!_shouldStop) {
        //            Console.WriteLine("worker thread: working...");
        //        }
        //        Console.WriteLine("worker thread: terminating gracefully.");
        //    }
        //    public void RequestStop() {
        //        _shouldStop = true;
        //    }
        //    // Volatile is used as hint to the compiler that this data
        //    // member will be accessed by multiple threads.
        //    private volatile bool _shouldStop;
        //}



        //public string GetIpAddRess(string  IP) {
        //    WebRequest request = WebRequest.Create("http://ip.taobao.com/service/getIpInfo.php?ip=" + IP);
        //    WebResponse response = request.GetResponse();
        //    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
        //    string read = reader.ReadToEnd();
        //    read = Unicode2String(read);
            
        //    response.Close();
        //    reader.Close();
        //    string []adr=new string[4];
        //    //int m1 = read.IndexOf("country") + 10;
        //    int m2 = read.IndexOf("region") + 9;
        //    //int m3 = read.IndexOf("city") + 7;
        //    //int m4 = read.IndexOf("isp") +6;
        //    //int n1 = read.IndexOf("country_id")-3;
        //    int n2 = read.IndexOf("region_id")-3;
        //    //int n3 = read.IndexOf("city_id")-3;
        //    //int n4 = read.IndexOf("isp_id")-3;
        //    try {
        //        //adr[0] = read.Substring(m1, n1 - m1);
        //        adr[1] = read.Substring(m2, n2 - m2);
        //        //adr[2] = read.Substring(m3, n3 - m3);
        //        //adr[3] = read.Substring(m4, n4 - m4);
        //    } catch {
        //        //adr[0] = "";
        //        adr[1] = "";
        //        //adr[2] = "";
        //       // adr[3] = "";
        //    }
        //    return adr[1];

        //}
        ///// <summary>  
        ///// Unicode转字符串  
        ///// </summary>  
        ///// <param name="source">经过Unicode编码的字符串</param>  
        ///// <returns>正常字符串</returns>  
        //public static string Unicode2String(string source) {
        //    return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
        //                 source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        //} 
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdVideoPath(int Id, int State)
        {
            string str = vh.UpdVideoUrl(Id, State);
            Write(str);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void FileUploadImage(int Id, string ImgStr)
        {
            string isSave = "false";
            try
            {
                if (ImgStr != "")
                {
                    string filepath = "/Img/cover/" + Id + ".jpg";
                    string path = Server.MapPath("~") + filepath;
                    File.Delete(path);
                    StringToFile(ImgStr, path);
                    if (File.Exists(path))
                    {
                        vh.UploadCover(Id, "http://v.icoxedu.cn" + filepath);
                        isSave = "true";
                    }
                    else
                    {
                        isSave = "false";
                    }
                }
            }
            catch (Exception e)
            {
                isSave = "false";
            }
            Write(isSave);
        }
      


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void VideoIsShow(int Id)
        {
            //VideoUrlInfo vui = vus.GetModel(Id);
            //VideoInfo vi= vs.GetModel(vui.Vid);
            //vi.Sort = 999;
            //vs.UpdModel(vi);
            //LogUtil.WriteLog(Id + "");
            Write("true");
        }


        [WebMethod]
        public void PushWeChatMsg(string Token, string Msg)
        {
            if (!string.IsNullOrEmpty(Msg.Trim()))
            {
                WeChatUser wcu = vh.GetWeChatUser(Token);
                if (wcu != null)
                {
                    new WeChat1().SetMsg(wcu.OpenId, Msg);
                    Write("ok");
                }
                else
                {
                    Write("-1");
                }
            }
        }

        [WebMethod]
        public void PushChildMsg(string Token, string Msg) {
            if (!string.IsNullOrEmpty(Msg.Trim())) {
                string XingeToken = QrChildManager.GetUserToken(Token);
                string xingeOpenId = QrChildManager.GetXingeOpenId(Token);
                //WeChatUser wcu = vh.GetWeChatUser(Token);
                if (XingeToken != null) {
                    new ChildPartner().SetMsgChild(xingeOpenId, Msg);
                    //new WeChat1().SetMsg(wcu.OpenId, Msg);
                    Write("ok");
                } else {
                    Write("-1");
                }
            }
        }
        [WebMethod]
        public void TencentUserLicense(string OpenId,string DevModel,string Version)
        {
                if (Version == null || Version == "")
                    Version = "1";
                TencentUser tu = vh.GetTencentUser(OpenId, DevModel, Version);
                if (tu != null) {
                    Write(jsonSerializer.Serialize(tu));
                } else {
                    Write("");
                }
        }
        [WebMethod]
        public void TencentUser(string OpenId, string DevModel) {
            TencentUserLicense(OpenId, DevModel, "1");
        }
     

        /// <summary>  
        /// 把经过base64编码的字符串保存为文件  
        /// </summary>  
        /// <param name="base64String">经base64加码后的字符串 </param>  
        /// <param name="fileName">保存文件的路径和文件名 </param>  
        /// <returns>保存文件是否成功 </returns>  
        public void StringToFile(string base64String, string fileName)
        {
            //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + @"/beapp/" + fileName;  

            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            if (!string.IsNullOrEmpty(base64String) && File.Exists(fileName))
            {
                bw.Write(Convert.FromBase64String(base64String));
            }
            bw.Close();
            fs.Close();
        }

        private void Write(string a)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(a);
        }
    }
}
