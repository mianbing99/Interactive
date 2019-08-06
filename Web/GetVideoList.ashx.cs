using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    /// <summary>
    /// GetVideoList 的摘要说明
    /// </summary>
    public class GetVideoList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
//            string sqlStr = @"
//            select (select Title from [dbo].[Video] where Id=a.Vid) as title,(
//            select top 1 (select top 1 [Title] from [dbo].[VideoType] where Id=b.Tid) from [dbo].[Video] as b) as [type],
//            [Source],[Path]
//            from [dbo].[VideoUrl] as a";
//            DataTable dtData =DBUtility.DbHelperSQL.GetTable(sqlStr,null,CommandType.Text);
            //DataTableToExcel(dtData, "imei");
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void DataTableToExcel(DataTable dt, string defaultFileName)
        {
            DataGrid dgd = new DataGrid();
            dgd.DataSource = dt;
            dgd.UseAccessibleHeader = true;//标题为粗体
            dgd.ItemStyle.HorizontalAlign = HorizontalAlign.Center;//显示信息居中
            dgd.Width = 900;  //显示宽度
            dgd.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;//标题居中
            dgd.HeaderStyle.ForeColor = System.Drawing.Color.Red;//设置标题字体颜色
            dgd.Font.Size = 12; //字体大小
            dgd.DataBind();
            if (dgd.Items.Count == 0)
            {
                throw (new Exception("无信息可打印"));
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + defaultFileName + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//防止中文乱码
                //设置输出文件类型为excel文件。
                CultureInfo myCItrad = new CultureInfo("ZH-CN", true);
                StringWriter oStringWriter = new StringWriter(myCItrad);
                HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
                dgd.RenderControl(oHtmlTextWriter);
                HttpContext.Current.Response.Write(oStringWriter.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }
}