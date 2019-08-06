using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Web.Common
{
    public class LogUtil
    {
        private static readonly object writeFile = new object();

        /// <summary>
        /// 在本地写入错误日志
        /// </summary>
        /// <param name="exception"></param> 
        public static void WriteLog(string debugstr)
        {
            lock (writeFile)
            {
                FileStream fs = null;
                StreamReader sr = null;
                StreamWriter sw = null;

                try
                {
                    string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    //服务器中日志目录
                    string folder = HttpContext.Current.Server.MapPath("~/Log");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    fs = new FileStream(folder + "/" + filename, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.Write(debugstr);
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Dispose();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Dispose();
                        fs = null;
                    }
                    if (sr != null)
                    {
                        sr.Dispose();
                        sr = null;
                    }
                }
            }
        }

    }
}
