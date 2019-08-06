using Models;
using Parsing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Web.Admin.Class;
using System.Data;
using System.IO;

namespace Web.Common
{
    public class VideoHandler
    {

        VideoEntities ve = DBContextFactory.GetDbContext();
        DataLogHandler dlh = new DataLogHandler();

        public object GetTypeListById(int id, ref int count) {
            var data = from a in ve.VideoType
                       where a.Id == id && a.State == 1
                       orderby a.Sort ascending
                       select new {
                           a.Id,
                           a.Tid,
                           a.Title,
                           a.Cover,
                           a.State
                       };
            count = data.Count();
            return data;
        }

        public object GetTypeList(int tid, ref int count)
        {
            var data = from a in ve.VideoType
                       where a.Tid == tid && a.State==1
                       orderby a.Sort ascending
                       select new
                       {
                           a.Id,
                           a.Tid,
                           a.Title,
                           a.Cover,
                           a.State
                       };
            count = data.Count();
            return data;
        }
        public object GetVideoPage(int index, int size, int tid,ref int count)
        {
            var data = from a in ve.Video
                       where a.Tid == tid && a.State==true
                       orderby a.Sort ascending,a.Id ascending
                       select new
                       {
                          a.Id,
                          a.Title,
                          a.Cover
                       };
            count = data.Count();
            return data.Skip(size * (index - 1)).Take(size);
        }
        public object SearchVideoPage(int index, int size, string keyWord, ref int count) {
            var data = from a in ve.Video
                       where a.Title.Contains(keyWord) && a.State == true
                       orderby a.Sort ascending, a.Id ascending
                       select new {
                           a.Id,
                           a.Title,
                           a.Cover
                       };
            count = data.Count();
            return data.Skip(size * (index - 1)).Take(size);
        }
        public object GetVideoUrl(int id, ref int count)
        {
            //UpdVideoUrl(id, 999);
            DataLog dl = new DataLog();
            dl.KeyId = id;
            dl.Type = (int)LogEnum.VIDEOLOG;
            dl.OpenId = "";
            dl.Describe = "";
            dl.CreateTime = DateTime.Now;
            var tempdata = from a in ve.VideoUrl
                           where a.Vid == id
                           orderby a.Sort ascending, a.Id ascending
                           select new
                           {
                               a.Id,
                               a.Source,
                               a.Path,
                               a.TempPath
                           };
            foreach (var item in tempdata.ToList())
            {
                string TempPath = item.Path;
                TempPath = TempPath.Replace("\t", "");
                TempPath = TempPath.Replace("\\t", "");
                string encryptPath = EncryptHelper.Encrypt(TempPath);
                if (string.IsNullOrEmpty((item.TempPath + "").Trim())
                    || (item.TempPath + "").Trim() == "7NTyKCzlI8jyXd7WRF1wFA=="
                    || !encryptPath.Equals((item.TempPath + "").Trim()))
                //if (string.IsNullOrEmpty((item.TempPath + "").Trim()) || (item.TempPath + "").Trim() == "7NTyKCzlI8jyXd7WRF1wFA==" || item.Source.Trim() == "youku")
                    {
                        //IParsing jx = null;  
                    switch (item.Source.Trim())
                        {
                            //case "youku":
                            //    jx = new YouKu();
                            //    break;
                            //case "360":
                            //    jx = new Yun360();
                            //    break;
                            //case "qq":
                            //    jx = new QQ();
                            //    break;
                            //case "Ku6":
                            //case "ku6":
                            //    jx = new Ku6();
                            //    break;
                            //case "QQYun":
                            //    TempPath = item.Path.Replace(",", "/,");
                            //    break;
                            //case "myfile":
                            //    TempPath = item.Path.Replace(",", "/,");
                            //    break;
                            case "AES_baby":
                            continue;
                                //return item.TempPath;
                        }

                    //if (jx != null)
                    //{
                    //    try
                    //    {
                    //        TempPath = jx.Parsing(item.Path);
                    //    }
                    //    catch
                    //    {
                    //        TempPath = item.Path;
                    //    }
                    //}
                        Models.VideoUrl model = new VideoUrl();
                        model.Id = item.Id;
                        model.TempPath = EncryptHelper.Encrypt(TempPath);
                        DbEntityEntry<VideoUrl> entry = ve.Entry<VideoUrl>(model);
                        entry.State = EntityState.Unchanged;
                        entry.Property("TempPath").IsModified = true;
                        ve.SaveChanges();
                    }
            }
            var data = from a in ve.VideoUrl
                       where a.Vid == id
                       orderby a.Sort ascending, a.Id ascending
                       select new
                       {
                           a.Id,
                           a.State,
                           a.Source,
                           a.Path,
                           a.TempPath
                       };
            count = data.Count();
            dlh.AddModel(dl);
            return data;
        }

        public object AESGetVideoUrl(int id,ref int count) {//拿到真实路径加密后传给客户端
            //"Id":156456,"State":0,"Source":"AES_baby","Path":"http://baby.icoxtech.com:90/国学诗词/AES_三字经/三字经-昔孟母...名俱扬.mp4","TempPath":"njnOsY26w7oLwpQu4xMffxJmGOc/zy2vRimQEk1aPXGZJjsP6mRQcjnjh9ynTdYb+h4YCIDwyyv9Adim38JO6blxeBgFqM1MxnQ/YyrrOpMZabd+usypm6719Ygp907C"
            string sql = "";
            List<string> path = new List<string>();
            string temppath = "";

            sql = "select * from videourl where vid='" + id + "' order by sort";
            DataSet ds = new DataSet();
            ds = SqlFunction.Sql_DataAdapterToDS(sql);
            count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++) {

                path.Add("'Id':" + ds.Tables[0].Rows[i]["Id"].ToString() 
                    + ",'State':" + ds.Tables[0].Rows[i]["State"].ToString()
                    + ",'Source':'" + ds.Tables[0].Rows[i]["Source"].ToString() + "'"
                    + ",'Path':'" + ds.Tables[0].Rows[i]["path"].ToString() + "'"
                    + ",'TempPath':'" + ds.Tables[0].Rows[i]["TempPath"].ToString() + "'");

                //temppath.Add(EncryptHelper.Encrypt(path[i]));
            }
            for(int j=0;j<path.Count;j++)
            {
                temppath += path[j];
            }
            temppath = temppath.Replace("'", "\"");
            temppath = temppath.Replace("\\", "");
            return temppath ;
        }


        public string UpdVideoUrl(int id, int state)
        {
            VideoUrl vu = ve.VideoUrl.Where(q => q.Vid == id).First();
            if (vu != null)
            {
                if (vu.State < state)
                {
                    //IParsing jx = null;
                    string TempPath = vu.Path;
                    TempPath = TempPath.Replace(" ", "");
                    TempPath = TempPath.Replace("\t", "");
                    TempPath = TempPath.Replace("\\t", "");
                    TempPath = TempPath.Replace("\\\t", "");
                    TempPath = TempPath.Replace("\\r", "");
                    TempPath = TempPath.Replace("\\n", "");
                    TempPath = TempPath.Replace("\\s", "");
                    switch (vu.Source.Trim())
                    {
                        //case "youku":
                        //    jx = new YouKu();
                        //    break;
                        //case "360":
                        //    jx = new Yun360();
                        //    break;
                        //case "qq":
                        //    jx = new QQ();
                        //    break;
                        //case "Ku6":
                        //    jx = new Ku6();
                        //    break;
                        case "QQYun":
                            TempPath = vu.Path.Replace(",", "/,");
                            break;
                        case "myfile":
                            TempPath = vu.Path.Replace(",","/,");;
                            break;
                        case "AES_baby":
                            return vu.TempPath;
                        default:
                            //TempPath = vu.Path;
                            break;
                    }
                    //if (jx != null)
                    //{
                    //    TempPath = jx.Parsing(vu.Path);
                    //}
                    vu.State += 1;
                    vu.TempPath = EncryptHelper.Encrypt(TempPath);
                    ve.SaveChanges();
                }
            }
            return vu.TempPath;
        }
        public void UploadCover(int id,string imgurl) {
          Video vi =   ve.Video.Where(q => q.Id == id).First();
          if (vi!=null)
          {
              vi.Cover = imgurl;
              ve.SaveChanges();
          }
        }
        public WeChatUser GetWeChatUser(string Token){
           return ve.WeChatUser.FirstOrDefault(q => q.Token == Token);
        }

        public TencentUser GetTencentUser(string openid, string devmodel,string version)
        {
            if (version == null || version == "") {
                version = "1";
            }
            TencentUser tu = ve.TencentUser.FirstOrDefault(q => q.OpenId == openid && q.Version == version);
            if (tu == null)
            {
                tu = ve.TencentUser.FirstOrDefault(q => (q.OpenId == null || q.OpenId == "") && q.Version ==  version);
                if (tu != null)
                {
                    tu.OpenId = openid;
                    tu.DevModel = devmodel;
                    tu.ActivateDate = DateTime.Now;
                    tu.Version = version;
                    ve.SaveChanges();
                }
            }
            return tu;
        }
        private static object obj = new object();
        /// <summary>
        /// 写Txt日志 到当前程序根目录
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLog)
        {
            lock (obj)
            {
                string sFilePath = HttpContext.Current.Server.MapPath("~/log");
                string sFileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
                if (!Directory.Exists(sFilePath))//验证路径是否存在
                {
                    Directory.CreateDirectory(sFilePath);
                    //不存在则创建
                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                //验证文件是否存在，有则追加，无则创建
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(strLog);
                sw.Close();
                fs.Close();
            }
        }
    }
}