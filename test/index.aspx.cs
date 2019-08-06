using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin
{
    public partial class index : System.Web.UI.Page
    {
        public WebInfo WEB;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master._PageTitle = "e家亲管理首页";
                if (WEB == null)
                {
                    WEB = new WebInfo();
                    WEB.Name = Server.MachineName;
                    WEB.IP = Request.ServerVariables["LOCAL_ADDR"];
                    WEB.WebName = Request.ServerVariables["SERVER_NAME"];
                    WEB.NetVersion = ".NET CLR" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
                    WEB.SystemVersion = Environment.OSVersion.ToString();
                    WEB.IISVersion = Request.ServerVariables["SERVER_SOFTWARE"];
                    WEB.HttpPort = Request.ServerVariables["SERVER_PORT"];
                    WEB.Path1 = Request.ServerVariables["APPL_RHYSICAL_PATH"];
                    WEB.Path2 = Request.ServerVariables["PATH_TRANSLATED"];
                    WEB.SessionCount = Session.Contents.Count.ToString();
                    WEB.ApplicationCount = Application.Contents.Count.ToString();
                    WEB.DomainHosting = Request.ServerVariables["HTTP_HOST"];
                    WEB.Language = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
                    WEB.UserInfo = Request.ServerVariables["HTTP_USER_AGENT"];
                }
            }
        }

        public class WebInfo
        {
            /// <summary>
            /// 服务器名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 服务器IP地址
            /// </summary>
            public string IP;
            /// <summary>
            /// 服务器域名
            /// </summary>
            public string WebName;
            /// <summary>
            /// .NET解释引擎版本
            /// </summary>
            public string NetVersion;
            /// <summary>
            /// 服务器操作系统版本
            /// </summary>   
            public string SystemVersion;
            /// <summary>
            /// 服务器IIS版本
            /// </summary>   
            public string IISVersion;
            /// <summary>
            /// HTTP访问端口
            /// </summary>
            public string HttpPort;
            /// <summary>
            /// 虚拟目录的绝对路径
            /// </summary>
            public string Path1;
            /// <summary>
            /// 执行文件的绝对路径
            /// </summary>
            public string Path2;
            /// <summary>
            /// 虚拟目录Session总数
            /// </summary>
            public string SessionCount;
            /// <summary>
            /// 虚拟目录Application总数
            /// </summary>
            public string ApplicationCount;
            /// <summary>
            /// 域名主机
            /// </summary>
            public string DomainHosting;
            /// <summary>
            /// 服务器区域语言
            /// </summary>
            public string Language;
            /// <summary>
            /// 用户信息
            /// </summary>
            public string UserInfo;
        }
    }
}