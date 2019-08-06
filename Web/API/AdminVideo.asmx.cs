using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Web.Common;

namespace Web.API
{
    /// <summary>
    /// AdminVideo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class AdminVideo : System.Web.Services.WebService
    {
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        VideoEntities ve = DBContextFactory.GetDbContext();
        HttpContext context;
        private bool IsLogin()
        {
            if (context.Session["AdminLogin"] != null)
                return true;
            return false;
        }

        private void Write(string str)
        {
            context.Response.ContentType = "text/json";
            context.Response.Write(str);
            context.Response.End();
        }
        [WebMethod(EnableSession = true)]
        public void Home()
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                Dictionary<string, object> dicsys = new Dictionary<string, object>();
                dicsys.Add("Name", Server.MachineName);//服务器名称
                dicsys.Add("Ip", Context.Request.ServerVariables["LOCAL_ADDR"]);//服务器IP地址  
                dicsys.Add("DomainName", Context.Request.ServerVariables["SERVER_NAME"]);//服务器域名  
                dicsys.Add("NetVersion", ".NET CLR" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision);//.NET解释引擎版本
                dicsys.Add("OsVersion", Environment.OSVersion.ToString());//服务器操作系统版本  
                dicsys.Add("IisVersion", Context.Request.ServerVariables["SERVER_SOFTWARE"]);//服务器IIS版本  
                dicsys.Add("HttpPort", Context.Request.ServerVariables["SERVER_PORT"]);//HTTP访问端口  
                dicsys.Add("VirtualPath", Context.Request.ServerVariables["APPL_RHYSICAL_PATH"]);//虚拟目录的绝对路径  
                dicsys.Add("RealPath", Context.Request.ServerVariables["PATH_TRANSLATED"]);//执行文件的绝对路径    
                dicsys.Add("SessionCount", Session.Contents.Count.ToString()); //虚拟目录Session总数  
                dicsys.Add("ApplicationCount", Application.Contents.Count.ToString()); //虚拟目录Application总数 
                dicsys.Add("DomainHost", Context.Request.ServerVariables["HTTP_HOST"]); //域名主机  
                dicsys.Add("HostLanguage", Context.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]); //服务器区域语言  
                dicsys.Add("UserInfo", Context.Request.ServerVariables["HTTP_USER_AGENT"]); //用户信息
                dicsys.Add("CpuCount", Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));//CPU个数  
                dicsys.Add("CpuType", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")); //CPU类型  
                dic.Add("StateCode", StateCode.OK);
                dic.Add("Content", dicsys);
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }


        [WebMethod(EnableSession = true)]
        public void GetVideoList(int Tid, int Index, int Size)
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                PageVideo pv = new PageVideo();
                if (Tid == 0)
                {
                    var data = from v in ve.Video
                               orderby v.Id
                               select new
                               {
                                   v.Id,
                                   v.Title,
                                   Type = ve.VideoType.FirstOrDefault(q => q.Id == v.Tid).Title,
                                   v.Sort,
                                   v.State
                               };
                    pv.Count = data.Count();
                    pv.Videos = data.Skip(Size * (Index - 1)).Take(Size);
                }
                else
                {
                    var data = from v in ve.Video
                               where v.Tid == Tid
                               orderby v.Sort, v.Id
                               select new
                               {
                                   v.Id,
                                   v.Title,
                                   Type = ve.VideoType.FirstOrDefault(q => q.Id == v.Tid).Title,
                                   v.Sort,
                                   v.State
                               };
                    pv.Count = data.Count();
                    pv.Videos = data.Skip(Size * (Index - 1)).Take(Size);
                }
                if (pv.Count > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", pv);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }

        [WebMethod(EnableSession = true)]
        public void GetVideoUrl(int vid)
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                var data = from q in ve.VideoUrl
                           where q.Vid == vid
                           orderby q.Sort
                           select new
                           {
                               q.Id,
                               q.Source,
                               q.Path,
                               q.Sort
                           };
                if (data.Count() > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", data);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }

        [WebMethod(EnableSession = true)]
        public void DelVideoUrl(int vid)
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                //VideoUrl vu = new VideoUrl() { Id = vid };
                //ve.VideoUrl.Attach(vu);
                //ve.VideoUrl.Remove(vu);
                //ve.SaveChanges();
                dic.Add("StateCode", StateCode.OK);
                dic.Add("Content", "删除成功！");
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }
        /// <summary>
        /// 获取所有的来源及所有分类和所有的排序规则
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void GetSource()
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                var data = from q in ve.VideoUrl
                           group q by q.Source into g
                           select new
                           {
                               Source = g.Key
                           };
                dic.Add("StateCode", StateCode.OK);
                dic.Add("Content", data);
            }
            else
            {
                dic.Add("StateCode",0);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void GetAllVideoType()
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                var data = from q in ve.VideoType
                           orderby q.Sort
                           select new
                           {
                               id = q.Id,
                               name = q.Title,
                               pId = q.Tid
                           };
                if (data.Count() > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", data);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }
        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="tid"></param>
        [WebMethod(EnableSession = true)]
        public void GetVideoType(int tid)
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                var data = from q in ve.VideoType
                           where q.Tid == tid
                           orderby q.Sort
                           select new
                           {
                               q.Id,
                               q.Title
                           };
                if (data.Count() > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", data);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }

        /// <summary>
        /// 获取分类详情
        /// </summary>
        /// <param name="id"></param>
        [WebMethod(EnableSession = true)]
        public void GetTypeInfo(int id)
        {
            context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                var data = from q in ve.VideoType
                           where q.Id==id
                           orderby q.Sort
                           select new
                           {
                               id = q.Id,
                               name = q.Title,
                               pId = q.Tid
                           };
                if (data.Count() > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", data);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }

        [WebMethod(EnableSession = true)]
        public void GetLog(string openid,int type,DateTime ondt,DateTime enddt) { 
           context = Context;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (IsLogin())
            {
                
                var data = from q in ve.DataLog
                           where q.Type == type &&
                           q.CreateTime > ondt &&
                           q.CreateTime < enddt &&
                           q.OpenId == openid
                           orderby q.CreateTime
                           group q by q.KeyId into g
                           select new
                           {
                               Id = g.Key,
                               Title=ve.Video.FirstOrDefault(q=>q.Id==g.Key).Title,
                               Count = g.Count()
                           };
                if (data.Count() > 0)
                {
                    dic.Add("StateCode", StateCode.OK);
                    dic.Add("Content", data);
                }
                else
                {
                    dic.Add("StateCode", StateCode.EMPTYDATA);
                    dic.Add("Content", "Empty data!");
                }
            }
            else
            {
                dic.Add("StateCode", StateCode.NOLOGIN);
                dic.Add("Content", "You didn't log in!");
            }
            string str = jsonSerializer.Serialize(dic);
            this.Write(str);
        }
    }
    public class PageVideo {
        public int Count { get; set; }
        public object Videos { get; set; }
    }
    public enum StateCode
    {
        OK=0,
        NOLOGIN = 200,
       EMPTYDATA = 201
    }
}
