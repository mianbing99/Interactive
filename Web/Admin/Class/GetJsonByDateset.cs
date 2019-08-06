using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using System.Data;

namespace Web.Admin.Class {
    public class GetJsonByDateset {
        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public static string GetJsonByDataset(DataSet ds) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) {
                //如果查询到的数据为空则返回标记ok:false
                dic.Add("msg", "null");
                return JsonMapper.ToJson(dic);
            }
            List<Dictionary<string, object>> arrList = new List<Dictionary<string, object>>();
            int i = 0;
            foreach (DataTable dt in ds.Tables) {
                foreach (DataRow dr in dt.Rows) {
                    Dictionary<string, object> tableDic = new Dictionary<string, object>();
                    for (int j = 0; j < dt.Columns.Count; j++) {
                        tableDic.Add(dt.Columns[j].ColumnName, Convert.ToString(dt.Rows[i][j]));
                    }
                    arrList.Add(tableDic);
                    i++;
                }
            }
            dic.Add("msg", "ok");
            dic.Add("data", arrList);

            return JsonMapper.ToJson(dic);
        }
    }
}