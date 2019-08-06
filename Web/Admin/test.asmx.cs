using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using LitJson;
using Web.Admin.Class;


namespace Web.Admin {
    /// <summary>
    /// courseVideo202 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class test : System.Web.Services.WebService {
        static public int dataNum = 0;
        static public int Index = 0;
        static public int num = 0;
        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }

        [WebMethod(Description = "根据字段查询courses表数据")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCourses(string name) {
            SqlConnection con = new SqlConnection("server=117.48.195.219;database=CourseVideo;uid=MDB;pwd=Main@JLF955icox;Pooling=true;");
            DataTable dt = new DataTable();
            dt.Columns.Add("title");
            dt.Columns.Add("path");
            DataSet dss = new DataSet();
            DataSet ds = new DataSet();
            if (con.State == ConnectionState.Closed)
                con.Open();
            string sql = "";
            sql = "select * from courseVideo_tb where book_title like'%"+name.Trim()+"%'";
            SqlDataAdapter sda = new SqlDataAdapter(sql,con);
            sda.Fill(ds);
            int count = ds.Tables[0].Rows.Count;
            string path = "", fullname="";
            for (int i=0;i<count ;i++ ) {
                path = ds.Tables[0].Rows[0][9].ToString();
                fullname = "参考:" +
                     returncloumns(i, 2, ds) + 
                     returncloumns(i, 1, ds) + 
                     returncloumns(i, 3, ds) + 
                     returncloumns(i, 4, ds) + 
                     returncloumns(i, 5, ds) + 
                     returncloumns(i, 6, ds) + 
                     returncloumns(i, 7, ds) + 
                     returncloumns(i, 8, ds) ;
                DataRow dr;
                dr = dt.NewRow();
                dr["title"] = fullname;
                dr["path"] = path;
                dt.Rows.Add(dr);
            }
            dss.Tables.Add(dt);

            if (con.State == ConnectionState.Open)
                con.Close();
            Write(GetJsonByDataset(dss));
        }
        public string returncloumns(int row,int cell,DataSet ds) {
            string gapstr="";
            if (ds.Tables[0].Rows[row][cell].ToString() == "")
                gapstr = "";
            else
                gapstr =  ds.Tables[0].Rows[row][cell].ToString();
            return gapstr;
        }
        [WebMethod(Description = "")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTrole(string roleName) {
            SqlConnection con = new SqlConnection("server=117.48.195.219;database=DDS;uid=MDB;pwd=Main@JLF955icox;Pooling=true;");
            string sql = "select * from t_role where role_name like'%" + roleName + "%'";
            SqlDataAdapter sda = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sda.Fill(ds, "data");
            if (con.State == ConnectionState.Open)
                con.Close();
            return GetJsonByDataset(ds);

        }

        //ConfigurationManager.AppSettings["CourseWareHttpQ"]+fileUrl
        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public static string GetJsonByDataset(DataSet ds) {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) {
                //如果查询到的数据为空则返回标记ok:false
                dic.Add("count", 0);
                dic.Add("return_count", 0);
                return JsonMapper.ToJson(dic);
            }

            List<Dictionary<string, object>> arrList = new List<Dictionary<string, object>>();
            int i = 0;

            foreach (DataTable dt in ds.Tables) {
                foreach (DataRow dr in dt.Rows) {
                    Dictionary<string, object> tableDic = new Dictionary<string, object>();

                    for (int j = 0; j < dt.Columns.Count; j++) {
                        if (dt.Columns[j].ColumnName.Equals("file_url")) {
                            tableDic.Add(dt.Columns[j].ColumnName, ConfigurationManager.AppSettings["CourseWareHttpQ"] + Convert.ToString(dt.Rows[i][j]));
                        } else {
                            tableDic.Add(dt.Columns[j].ColumnName, Convert.ToString(dt.Rows[i][j]));
                        }

                    }
                    arrList.Add(tableDic);
                    i++;

                }
                //dic.Add("return_count", dt.Rows.Count);
            }

            dic.Add("return_count", arrList.Count);

            dic.Add("count", dataNum);

            dic.Add("data", arrList);
            return JsonMapper.ToJson(dic);
        }

        //public static string GetJsonByDataset(DataSet ds) {
        //if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) {
        //    //如果查询到的数据为空则返回标记ok:false
        //    return "{\"ok\":false,没有查询到数据}";
        //}
        //    StringBuilder sb = new StringBuilder();
        //    string tips="";
        //    //if ((num * (Index - 1) + 1) > dataNum || (num * (Index - 1) + num) > dataNum)
        //    //    tips = "查询页数已超过最大值,显示最后" + num + "条数据";
        //    //else
        //        tips = "共搜索到"+dataNum+"条数据,显示"+ds.Tables[0].Rows.Count+"条数据,";
        //        sb.Append("{\"count\":" + dataNum + ",\"return_count\":" + ds.Tables[0].Rows.Count
        //            + ",\"url\":\"" + ConfigurationManager.AppSettings["CourseWareHttpQ"]+"\",");
        //    foreach (DataTable dt in ds.Tables) {
        //        sb.Append(string.Format("\"{0}\":[", dt.TableName));

        //foreach (DataRow dr in dt.Rows) {
        //    sb.Append("{");
        //    for (int i = 0; i < dr.Table.Columns.Count; i++) {
        //        sb.AppendFormat("\"{0}\":\"{1}\",",
        //        dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"),
        //        ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13),
        //        "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
        //    }
        //    sb.Remove(sb.ToString().LastIndexOf(','), 1);
        //    sb.Append("},");
        //}

        //        sb.Remove(sb.ToString().LastIndexOf(','), 1);
        //        sb.Append("],");
        //    }
        //    sb.Remove(sb.ToString().LastIndexOf(','), 1);
        //    sb.Append("}");
        //    return sb.ToString();
        //}
        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob) {
            if (ob == null) {
                return string.Empty;
            } else
                return ob.ToString();
        }
        private void Write(string a) {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(a);
        }
    }
}
