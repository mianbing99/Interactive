using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Admin.Class;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Web.Admin.ashx {
    /// <summary>
    /// checkYK 的摘要说明
    /// </summary>
    public class checkYK : IHttpHandler {
        string api = "",cc;
        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";
                api = System.Web.HttpContext.Current.Request.Form["api"];
                switch (api) {
                    case "yi":
                        try {
                            cc = System.Web.HttpContext.Current.Request.Form["c"];
                            string ssql = @"select top 100 Id,Vid,Path from VideoUrl where Source='youku' and Vid in(select Id from Video where State=1)";
                            DataSet ds = new DataSet();
                            ds = SqlFunction.Sql_DataAdapterToDS(ssql);
                            
                            DataSet ds2 = new DataSet();
                            string ssql2 = @"select top 1 Id,Vid,Path from VideoUrl where Source='y2ouku'";
                            ds2 = SqlFunction.Sql_DataAdapterToDS(ssql2);
                            ds2.Tables[0].Columns.Add("jg", typeof(string));
                            string jg = "";
                            for (int i=0;i<ds.Tables[0].Rows.Count ;i++ ) {
                                jg = checkYouku(ds.Tables[0].Rows[i]["path"].ToString());
                                if (jg != "ok") {
                                    DataRow newRow;
                                    newRow = ds2.Tables[0].NewRow();
                                    newRow["id"] = ds.Tables[0].Rows[i]["id"].ToString();
                                    newRow["vid"] = ds.Tables[0].Rows[i]["vid"].ToString();
                                    newRow["path"] = ds.Tables[0].Rows[i]["path"].ToString();
                                    newRow["jg"] = jg;
                                    ds2.Tables[0].Rows.Add(newRow);
                                }
                            }
                            context.Response.Write(GetJsonByDateset.GetJsonByDataset(ds2));
                        } catch (Exception e) {
                            context.Response.Write(e.Message);
                        }
                        break;
                }

            
        }
        public string checkYouku(string path) {
            string url2 = string.Format("http://www.zsctc-api.com:8001/ok?url={0}", path);
            string htmlSource = GetHtmlSource(url2);
            JObject jObject = JObject.Parse(htmlSource);
            if (!htmlSource.Contains("\"null\"")) {
                
                if (jObject["videoUrl"] != null) {
                    if (jObject["videoUrl"].ToString().Length < 20) {
                        return "videoUrl:" + jObject["videoUrl"].ToString() + "playurl:" + jObject["playurl"].ToString();
                    } else {
                        return "ok";
                    }

                } else {
                    return "null";
                }

            } else {
                return "videoUrl:" + jObject["videoUrl"].ToString() + "playurl:" + jObject["playurl"].ToString();
            }
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
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}