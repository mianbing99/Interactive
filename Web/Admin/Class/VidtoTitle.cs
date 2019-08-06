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
using System.Data.SqlClient;



namespace Web.Admin.Class {
    public class VidtoTitle {
        /// <summary>  
        /// Unicode转字符串  
        /// </summary>  
        /// <param name="source">经过Unicode编码的字符串</param>  
        /// <returns>正常字符串</returns>  
        public static string Unicode2String(string source) {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        static public DataSet ip_to_Addr(DataSet ip) {
            DataSet dataaddr = new DataSet();
            dataaddr.Tables.Add(new DataTable());
            dataaddr.Tables[0].Columns.Add(new DataColumn("ip"));

            try {
                Int32 count = ip.Tables[0].Rows.Count;
                string a = "";
                for (int i = 0; i < count; i++) {
                    a = ip.Tables[0].Rows[i][0].ToString();
                    dataaddr.Tables[0].Rows.Add(new string[] { GetIpAddRess(a) });

                }
                //string mtype = title.Substring(0, 4); ;
                //string vtype = title.Substring(4, title.Length - 4);
                //SqlFunction.Sql_ReturnNumberENQ("insert into TempUserusejl(videoId,user_IP,uAddr,clickTime,vtype) values('"
                //    + Id + "','" + clientIP + "','" + adrr + "','" + dt.ToString() + "','" + title + "')");
            } catch (Exception e) {
                
            }

            return dataaddr;
        }
        static public string Vid_to_Type(int id) {

            string sql = "";
            string sqlb = "" ;
            string sqlf = "(select tid from Video where id="+id+"))";
            string relut = "a";
            List<string> end = new List<string>();
            
            for (string sqlh = "(select Title from videotype where id=("; relut != "";) {
                sqlf += ")";
                sql = sqlh + sqlb +sqlf;
                relut = SqlFunction.Sql_ReturnNumberES(sql);
                end.Add(relut);
                sqlb += "(select Tid from videotype where id=";
            }
            for(int i=end.Count-1;i>=0;i--)
            {
                relut += (end[i]+ "_");
            }
            if(relut!="" && relut.Length>=2)
                relut = relut.Substring(1,relut.Length-2);
            return relut;
        }
        public static string GetIpAddRess(string IP) {
            WebRequest request = WebRequest.Create("http://ip.taobao.com/service/getIpInfo.php?ip=" + IP);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
            string read = reader.ReadToEnd();
            read = Unicode2String(read);

            response.Close();
            reader.Close();
            string[] adr = new string[4];
            //int m1 = read.IndexOf("country") + 10;
            int m2 = read.IndexOf("region") + 9;
            //int m3 = read.IndexOf("city") + 7;
            //int m4 = read.IndexOf("isp") +6;
            //int n1 = read.IndexOf("country_id")-3;
            int n2 = read.IndexOf("region_id") - 3;
            //int n3 = read.IndexOf("city_id")-3;
            //int n4 = read.IndexOf("isp_id")-3;
            try {
                //adr[0] = read.Substring(m1, n1 - m1);
                adr[1] = read.Substring(m2, n2 - m2);
                //adr[2] = read.Substring(m3, n3 - m3);
                //adr[3] = read.Substring(m4, n4 - m4);
            } catch {
                //adr[0] = "";
                adr[1] = "";
                //adr[2] = "";
                // adr[3] = "";
            }
            return adr[1];

        }
    }
}