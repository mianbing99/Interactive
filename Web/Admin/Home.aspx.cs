using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Admin.Class;
using System.Data;
using System.Text;
using Web.Admin.model;
using System.Web.Services;

namespace Web.Admin {
    public partial class Home : System.Web.UI.Page {
        static public string Fullinfo = "",id = "",CustomerName = "";
        static public string[]infotitle, infocontent,result;
        static public string MVPtitle="", MVPtitlec="";
        static public string MVPtype = "", MVPtypec="";
        static public string MVPacc = "",MVPpwd="", MVPipc="";
        static public string MVPaddr = "", MVPaddrc="";
        static public string onlinecc = "",clickcc="",alluser="";
        static public int addu =0;
        public static int state, export, add, delete, update;
        public user u = new user();
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (Session["AdminLogin"] == null) {
                    Response.Write(@"<script type='text/javascript' src='../js/jquery-1.8.3.js'></script><script type='text/javascript' src='../js/layer/layer.js'></script><script>
                    layer.alert('当前页面已失效,请重新登录', {{
                              icon: 6,title: false,
                              shadeClose: true,skin: 'layer-ext-moon'  
                            }})
                    </script>");
                    Response.Write("<script>window.close();</script>");
                    Response.Redirect("/Admin/login.aspx");
                } else {
                    getMVP();
                    //getMVP2();

                    //try {
                    //    DateTime currentTime = DateTime.Now.AddDays(-15);
                    //    DataSet ds = new DataSet();
                    //    ds = SqlFunction.Sql_DataAdapterToDS("select * from CustomerSuggestions where FullinfoTime>='" + currentTime.ToString() + "' order by FullinfoTime desc");
                    //    int count=ds.Tables[0].Rows.Count;
                    //    if (count<=0) {
                    //        result = new string[1];
                    //        result[0] = "<div class='media'><a href=''>客户反馈信息</a><span class='drawer-close'>×</span></div><div class='overflow' style='height: 254px; overflow: hidden; outline: none;' tabindex='5001'><div>最近15天还没有客户反应情况</div>";
                    //    } else {
                    //        infotitle = new string[count];
                    //        infocontent = new string[count];
                    //        result = new string[count];
                    //        string isReply="";
                    //        int picn = 1;
                    //        string color = "";
                    //        string noreply = "";
                    //        for (int i = 0; i < count;i++ ) {
                    //            id = ds.Tables[0].Rows[i]["id"].ToString();
                    //            CustomerName = ds.Tables[0].Rows[i]["CustomerName"].ToString();
                    //            CustomerAccount = ds.Tables[0].Rows[i]["CustomerAccount"].ToString();
                    //            CustomerPwd = ds.Tables[0].Rows[i]["CustomerPwd"].ToString();
                    //            Ip = ds.Tables[0].Rows[i]["Ip"].ToString();
                    //            Fraction = ds.Tables[0].Rows[i]["Fraction"].ToString();
                    //            Fullinfo = ds.Tables[0].Rows[i]["Fullinfo"].ToString();
                    //            FullinfoTime = ds.Tables[0].Rows[i]["FullinfoTime"].ToString();
                    //            Admin = ds.Tables[0].Rows[i]["Admin"].ToString();
                    //            Replyinfo = ds.Tables[0].Rows[i]["Replyinfo"].ToString();
                    //            ReplyinfoTime = ds.Tables[0].Rows[i]["ReplyinfoTime"].ToString();
                    //            Replyinfo = ds.Tables[0].Rows[i]["Replyinfo"].ToString();
                    //            if (Replyinfo == null || Replyinfo == "") {
                    //                isReply = "未回复";
                    //                color = " style='color:yellow' ";
                    //                noreply = "<a id='i"+id+"' href='javascript:;' onclick='showreply("+id+")'>回复</a>";
                    //                Admin = "";
                    //            } else {
                    //                isReply = "已回复";
                    //                color="";
                    //                noreply = "";
                    //                Admin = Admin + "--" + ReplyinfoTime;
                    //            }
                    //            string title = "";

                    //            Random rNumber = new Random();//实例化一个随机数对象
                    //            picn= rNumber.Next(1, 6);//产生一个1到6之间的任意一个数
                    //            result[i] = string.Format(@"<div  class='media'><div class='pull-left'><img width='40' src='css/img/profile-pics/{0}.jpg' alt=''></div><div class='media-body'><small id='t{15}' class='text-muted'>{15}--{2}--{1}分--{3}--{4}--{5}--{6}---</small><a id='lab_r'  runat='server' href='javascript:;' {7} onclick='selectReply({9})'>{8}</a><br/><label id='c{15}' class='t-overflow' >{10}</label><div id='oneinfo{11}' class='none'><small  class='text-muted'>{12}{13}</small><br/><a class='t-overflow' href=''>{14}</a></div></div></div>"
                    //                , picn, Fraction, CustomerName, CustomerAccount, CustomerPwd,
                    //                           FullinfoTime, Ip, color, isReply, i, Fullinfo, i, Admin,noreply, Replyinfo,id );
                    //        }

                    //        result[0] = "<div class='media'><a href=''>客户反馈信息</a><span class='drawer-close'>×</span></div><div class='overflow' style='height: 254px; overflow: hidden; outline: none;' tabindex='5001'>" + result[0];
                    //        result[result.Length - 1] = result[result.Length - 1] + "</div>";

                    //    }
                    //} catch (Exception ee) {

                    //}
                    //this.ClientScript.RegisterStartupScript(this.GetType(), "", "addhtml('" + result + "')", true);
                    u = (user)Session["Adminu"];
                    state = u.state;
                    export = u.export;
                    add = u.add;
                    delete = u.delete;
                    update = u.update;
                }
                
            }
        }
        /// <summary>
        /// 获取播放量最大的视频,类型,地域,IP
        /// </summary>
        private void getMVP() {
            string sqltype = @"select top 1  videoType,countNum from UserAccessType where videoType!='' order by countNum desc";
            DataSet dstype = new DataSet();
            dstype = SqlFunction.Sql_DataAdapterToDS(sqltype);
            MVPtype = dstype.Tables[0].Rows[0]["videoType"].ToString();
            try {
                string[] temp = MVPtype.Split('_');
                MVPtype = temp[temp.Length-1];
            }  catch {
            }
            MVPtypec = dstype.Tables[0].Rows[0]["countNum"].ToString();
            h2_typec.InnerText = MVPtypec;
            
            lab_type.InnerText = MVPtype;
            string sqladdr = @"select top 1 addr,count(*)as c1 from UserAccessRecord group by addr order by c1 desc";
            DataSet dsaddr = new DataSet();
            dsaddr = SqlFunction.Sql_DataAdapterToDS(sqladdr);
            MVPaddr = dsaddr.Tables[0].Rows[0]["addr"].ToString();
            MVPaddrc = dsaddr.Tables[0].Rows[0]["c1"].ToString();
            h2_addrc.InnerText = MVPaddrc;
            lab_addr.InnerText = MVPaddr;
            string sqlip = @"select top 1 account,pwd,COUNT(account)as c1 from UserAccessRecord  group by account,pwd order by c1 desc";
            DataSet dsip = new DataSet();
            dsip = SqlFunction.Sql_DataAdapterToDS(sqlip);
            MVPacc = dsip.Tables[0].Rows[0]["account"].ToString();
            MVPpwd = dsip.Tables[0].Rows[0]["pwd"].ToString();
            MVPipc = dsip.Tables[0].Rows[0]["c1"].ToString();
            if (MVPacc == "") {
                MVPacc = "null";
            }
            if (MVPipc == "") {
                MVPipc = "null";
            }
            if (MVPpwd == "") {
                MVPpwd = "null";
            }
            h2_ipc.InnerText = MVPipc;
            lab_ip.InnerText = MVPacc.ToLower();
            lab_ip2.InnerText = MVPpwd;
            string sqltitle = @"select top 1 (select title from Video where id = vid)as title ,count(*) as c
from UserAccessRecord  group by vid order by c desc";
            DataSet dstitle = new DataSet();
            dstitle = SqlFunction.Sql_DataAdapterToDS(sqltitle);
            MVPtitle = dstitle.Tables[0].Rows[0]["title"].ToString();
            MVPtitlec = dstitle.Tables[0].Rows[0]["c"].ToString();
            h2_titlec.InnerText = MVPtitlec;
            if (MVPtitle.Length > 23) {
                lab_t.Attributes.Add("font-size", "14px");
            } else {
                lab_t.Attributes.Add("font-size", "16px");
            }
            lab_t.InnerText = MVPtitle;
        }
        /// <summary>
        /// 获取昨日,历史最大点击量,在线人数,新增用户,总用户
        /// </summary>
        /*private void getMVP2() {
            string thisdate = DateTime.Now.ToString("yyyy-MM-dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string yesterday2 = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            string sqlcc =string.Format(@"select nums from DailyHits where date='{0}'", yesterday);
            clickcc = SqlFunction.Sql_ReturnNumberES(sqlcc);
            string sqloc = string.Format(@"select nums from dailyonlinenum where date='{0}'", yesterday);
            onlinecc = SqlFunction.Sql_ReturnNumberES(sqloc);
            string sqlnewu=string.Format(@"select COUNT(a.date)as 历史总人数 from 
                                           (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                           substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                           group by a.date order by date", thisdate);
            alluser = SqlFunction.Sql_ReturnNumberES(sqlnewu);
            string ydayaddu = "", ydayaddu2="";
            string sqlydaddu = string.Format(@"select COUNT(a.date)as 昨日总人数 from 
                                               (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                               substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                               group by a.date order by date", yesterday);
            string sqlydaddu2 = string.Format(@"select COUNT(a.date)as 前日总人数 from 
                                                (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                                substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                                group by a.date order by date", yesterday2);
            ydayaddu = SqlFunction.Sql_ReturnNumberES(sqlydaddu);
            ydayaddu2 = SqlFunction.Sql_ReturnNumberES(sqlydaddu2);
            if (ydayaddu == "") {
                ydayaddu = "0";
            }
            if (ydayaddu2 == "") {
                ydayaddu2 = "0";
            }
            //昨日对比前日总人数新增了多少
            addu = Convert.ToInt32(ydayaddu) - Convert.ToInt32(ydayaddu2);
            
        }*/
        [WebMethod]
        public static string getMVP2() {
            string thisdate = DateTime.Now.ToString("yyyy-MM-dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string yesterday2 = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            string sqlcc = string.Format(@"select nums from DailyHits where date='{0}'", yesterday);
            clickcc = SqlFunction.Sql_ReturnNumberES(sqlcc);
            string sqloc = string.Format(@"select nums from dailyonlinenum where date='{0}'", yesterday);
            onlinecc = SqlFunction.Sql_ReturnNumberES(sqloc);
            string ydayaddu = "", ydayaddu2 = "";
            string sqlydaddu = string.Format(@"select COUNT(a.date)as 昨日总人数 from 
                                               (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                               substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                               group by a.date order by date", yesterday);
            string sqlydaddu2 = string.Format(@"select COUNT(a.date)as 前日总人数 from 
                                                (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                                substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                                group by a.date order by date", yesterday2);
            ydayaddu = SqlFunction.Sql_ReturnNumberES(sqlydaddu);
            ydayaddu2 = SqlFunction.Sql_ReturnNumberES(sqlydaddu2);
            if (ydayaddu == "") {
                ydayaddu = "0";
            }
            if (ydayaddu2 == "") {
                ydayaddu2 = "0";
            }
            //昨日对比前日总人数新增了多少
            addu = Convert.ToInt32(ydayaddu) - Convert.ToInt32(ydayaddu2);
            return addu.ToString();
        }
        [WebMethod]
        public static string getMVP3() {
            string thisdate = DateTime.Now.ToString("yyyy-MM-dd");
            string sqlnewu = string.Format(@"select COUNT(a.date)as 历史总人数 from 
                                           (select substring(time,1,10)as date,account,pwd from UserAccessRecord where
                                           substring(time,1,10)<='{0}' group by substring(time,1,10),account,pwd) a
                                           group by a.date order by date", thisdate);
            alluser = SqlFunction.Sql_ReturnNumberES(sqlnewu);
            return alluser;
        }

        protected void btn_deleteUser_Click(object sender, EventArgs e) {

        }

        protected void btn_reply_Click(object sender, EventArgs e) {
            
        }

        protected void gor_Click(object sender, EventArgs e) {
           // TextBox tb = (TextBox)this.FindControl("reply_txt");
            user u = new user();
            u = (user)Session["Adminu"];
            string sql = "";
            sql = string.Format(@"update CustomerSuggestions set Admin='{0}',
Replyinfo='{1}',ReplyinfoTime='{2}' where Id='{3}'", u.name, info_i.Value, DateTime.Now, kid.Value);
            if (SqlFunction.Sql_ReturnNumberENQ(sql) == 0) {
                Layer(@"layer.alert('回复失败', {
                              icon: 0,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            })", Page);
            } else {
                Layer(@"layer.alert('回复成功', {
                              icon: 1,title: false,
                              shadeClose: true,skin: 'layer-ext-moon' 
                            })", Page);
            }
        }
        public static void Layer(string info, Page p) {
            string script = "<script type='text/javascript' src='js/jquery-1.11.3.js'></script><script type='text/javascript' src='js/layer/layer.js'></script><script>";
            script = script + info + "</script>";
            p.ClientScript.RegisterStartupScript(p.GetType(), "", script);
        }

        protected void Button1_Click(object sender, EventArgs e) {
            string ssql = "select Vid,Path,Definition,Advertisement,Describe from VideoUrl where Source='QQYun'";
            DataSet sds = new DataSet();
            sds = SqlFunction.Sql_DataAdapterToDS(ssql);
            //http://qqyun.icoxtech.com:86/儿童专区\舞蹈学院\幼儿舞蹈\01.爱的主打歌.FLV
            string path="";
            int c = 0;
            for (int  i=0;i<sds.Tables[0].Rows.Count ;i++ ) {
                path=sds.Tables[0].Rows[i]["path"].ToString().Replace("qqyun.icoxtech.com:86","ejq.esmartbag.com:81");
                string isql = string.Format(@"insert into VideoUrl values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    sds.Tables[0].Rows[i]["vid"].ToString(),
                    "WY_R430",
                    path,
                    "",
                    1,1,
                    DateTime.Now.ToString(),
                    sds.Tables[0].Rows[i]["Definition"].ToString(),
                    sds.Tables[0].Rows[i]["Advertisement"].ToString(),
                    sds.Tables[0].Rows[i]["Describe"].ToString());
                if (SqlFunction.Sql_ReturnNumberENQ(isql) > 0) {
                    c++;
                }
            }
        }
    }
}