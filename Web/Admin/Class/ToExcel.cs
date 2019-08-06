using System;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using Web.Admin.Class;

namespace Web.Admin.Class {

    public class ToExcel {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="b">导出的类型</param>
        /// <param name="info">导出消息</param>
        /// <param name="cbl">传递的数组</param>
        static public bool CreateExcel(DataSet dsQ, string FileName, int b,string info,bool []cbl,string str) {
            DataGrid dgd = new DataGrid();
            DateTime dtime = DateTime.Now;
            switch (b) {//1为导出当前个格式,2为根据分类导出详细格式3为导出用户使用记录
                case 1:
                    //string vsql = "select * from "+info+" where id in("+str+")";
                    dgd.DataSource = dsQ.Tables[0] ; break;
                case 2: dcxx(info, cbl, dgd); ; break;
                case 3:
                    //type = VidtoTitle.Vid_to_Type(Id);
                      //,(select top 1 source from VideoUrl where vid in(A.videoId) order by sort) as 信道1
//                      ,(select top 1 source from VideoUrl where vid in(A.videoId) and Source not in (select top 1 Source from VideoUrl where vid in(A.videoId)order by sort) order by sort) as 信道2
//,(select top 1 source from VideoUrl where vid in(A.videoId) and Source not in (select top 2 Source from VideoUrl where vid in(A.videoId)order by sort) order by sort) as 信道3
                    string sql_utj = @"select id,videoId as 视频id, user_IP as 用户IP,uAddr as 省份  
 ,(select title from Video where id in(A.videoId))as 标题
, clickTime as 点击时间, vtype as 视频分类
from TempUserusejl as A "+str+" order by a.id";
                    DataSet ds = new DataSet();
                    ds = SqlFunction.Sql_DataAdapterToDS(sql_utj);
                    Int32 count = ds.Tables[0].Rows.Count;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt = ds.Tables[0];
                    dt.Columns.Add("一级分类",typeof(string));
                    string temp="";
                    for (int i = 0; i < count; i++) {
                        temp=ds.Tables[0].Rows[i][6].ToString();
                        if (temp != "" && temp != null) {
                            try {
                                dt.Rows[i][7] = temp.Substring(0, 4);
                            } catch {
                                dt.Rows[i][7] = temp;
                            }
                        } else
                            dt.Rows[i][7] = "";
                    }
                    dgd.DataSource = dt;
                    ; break;
            }
            dgd.UseAccessibleHeader = true;//标题为粗体
            dgd.ItemStyle.HorizontalAlign = HorizontalAlign.Center;//显示信息居中
            dgd.Width = 900;  //显示宽度
            dgd.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;//标题居中
            dgd.HeaderStyle.ForeColor = System.Drawing.Color.Red;//设置标题字体颜色
            dgd.Font.Size = 10; //字体大小 
            dgd.DataBind();
            if (dgd.Items.Count == 0) {
                return false;
            } else {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "gb2312";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + dtime.ToString()+ ".xls");
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");//防止中文乱码
                CultureInfo myCItrad = new CultureInfo("ZH-CN", true);
                StringWriter oStringWriter = new StringWriter(myCItrad);
                HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                dgd.RenderControl(oHtmlTextWriter);
                HttpContext.Current.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + oStringWriter.ToString());
                HttpContext.Current.Response.End();
                return true;
            }
        }
        /// <summary>
        ///导出详细格式
        /// </summary>
        static public void dcxx(string info, bool[] cbl, DataGrid dgd) {
            if (info == "" || info == "老人专区,幼儿专区")
                info = "老人专区','幼儿专区";
            string sql_temp = "select ";
            if (cbl[0] == true) { sql_temp += "Id ,"; } 
            if (cbl[1] == true) { sql_temp += "Title as 标题,"; }
            if (cbl[2] == true) { sql_temp += "Cover as 图片,"; } 
            if (cbl[3] == true) { sql_temp += "Tid as 类型,"; }
            if (cbl[4] == true) { sql_temp += "( select title from videoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=a.tid)))))as 一级菜单,"; }
            if (cbl[5] == true) { sql_temp += "( select title from videoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=a.tid))))as 二级菜单,"; }
            if (cbl[6] == true) { sql_temp += "( select title from videoType where id=(select Tid from VideoType where id=(select Tid from VideoType where id=a.tid)))as 三级菜单,";}
            if (cbl[7] == true) { sql_temp += "( select title from videoType where id=(select Tid from VideoType where id=a.tid))as 四级菜单,";}
            if (cbl[8] == true) { sql_temp += "( select title from videoType where id=a.tid)as 五级菜单,"; }
            if (cbl[9] == true) { sql_temp += "( select Source from(select  Source , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 信道1,"; }
            if (cbl[10] == true) { sql_temp += "( select sort from(select  sort , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 优先1,"; }
            if (cbl[11] == true) { sql_temp += "( select describe from (select  describe , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 原标题1,"; }
            if (cbl[12] == true) { sql_temp += "( select path from (select  path , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 路径1,"; }
            if (cbl[13] == true) { sql_temp += "( select state from (select  state , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 状态1,"; }
            if (cbl[14] == true) { sql_temp += "( select definition from (select  definition , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 清晰度1,"; }
            if (cbl[15] == true) { sql_temp += "( select advertisement from (select  advertisement , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 1)as 广告1,"; }
            if (cbl[16] == true) { sql_temp += "( select Source from(select  Source , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 信道2,"; }
            if (cbl[17] == true) { sql_temp += "( select sort from(select  sort , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 优先2,"; }
            if (cbl[18] == true) { sql_temp += "( select describe from (select  describe , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 原标题2,"; }
            if (cbl[19] == true) { sql_temp += "( select path from (select  path , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 路径2,"; }
            if (cbl[20] == true) { sql_temp += "( select state from (select  state , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 状态2,"; }
            if (cbl[21] == true) { sql_temp += "( select definition from (select  definition , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 清晰度2,"; }
            if (cbl[22] == true) { sql_temp += "( select advertisement from (select  advertisement , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 2)as 广告2,"; }
            if (cbl[23] == true) { sql_temp += "( select Source from(select  Source , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 信道3,"; }
            if (cbl[24] == true) { sql_temp += "( select sort from(select  sort , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 优先3,"; }
            if (cbl[25] == true) { sql_temp += "( select describe from (select  describe , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 原标题3,"; }
            if (cbl[26] == true) { sql_temp += "( select path from (select  path , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 路径3,"; }
            if (cbl[27] == true) { sql_temp += "( select state from (select  state , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 状态3,"; }
            if (cbl[28] == true) { sql_temp += "( select definition from (select  definition , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 清晰度3,"; }
            if (cbl[29] == true) { sql_temp += "( select advertisement from (select  advertisement , 信道 = row_number() over(order by id desc) from videoUrl where vid=a.Id) m where 信道 = 3)as 广告3,"; }
            try {
                sql_temp = sql_temp.Substring(0, sql_temp.Length - 1);
            } catch { 
            
            }
            sql_temp += string.Format(@"
                from Video as A where Tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in(select id from videotype where title in('{0}')))))))
                    or tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in(select id from videotype where title in('{0}'))))))
                    or  tid in
                (select id from videotype where tid in
                (select id from videotype where tid in
                (select id from videotype where tid in (select id from videotype where title in('{0}')))))
                    or tid in
                (select id from videotype where tid in
                (select id from videotype where tid in (select id from videotype where title in('{0}'))))
                    or tid in
                (select id from videotype where tid in (select id from videotype where title in('{0}')))
                    or tid in  (select id from videotype where title in('{0}'))
        ", info);

            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql_temp);
            dgd.DataSource = ds.Tables[0];
        }
    }
}