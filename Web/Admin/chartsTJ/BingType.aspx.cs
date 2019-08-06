using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Web.Admin.Class;


namespace Web.Admin.chartsTJ {
    public partial class BingType : System.Web.UI.Page {
        protected string[] Ytype, Otype;//幼儿和老人的分类
        protected double[] YZBdhp, YZBysl, YZBdme, YZBkxt, YZBjkz, YZBqwy, YZByyw, YZBgxq, YZBgst, YZBwdx, YZByet;//幼儿专区下子类的百分比集合
        protected double[] OZBxqz, OZBxsz, OZBxpz, OZBgcw, OZBysj, OZByq;//老人专区下子类的百分比集合
        protected double bdhpc = 0.00f, byslc = 0.00f, bdmec = 0.00f, bkxtc = 0.00f, bjkzc = 0.00f, bqwyc = 0.00f, byywc = 0.00f, bgxqc = 0.00f, bgstc = 0.00f, bwdxc = 0.00f, byetc = 0.00f;//幼儿各个分类所占的百分比
        protected double bxqzc = 0.00f, bxszc = 0.00f, bxpzc = 0.00f, bgcwc = 0.00f, bysjc = 0.00f, byqc = 0.00f;//老人各个分类的百分比;
        protected List<string> Ydhpz,Yyslz,Ydmez,Ykxtz,Yjkzz,Yqwyz,Yyywz,Ygxqz,Ygstz,Ywdxz,Yyetz;//幼儿专区下分类中的子类
        protected List<string> Oxqzz, Oxszz, Oxpzz, Ogcwz, Oysjz, Oyqz;//老人专区下分类中的子类
        protected double OAll;//老人专区占总数的百分比
        protected double YAll;//幼儿专区占总数的百分比
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["AdminLogin"] == null) {
                Response.Redirect("/Admin/login.aspx");
            } else {
                if (!IsPostBack) {
                    getYType();
                }
            }
        }
        public void getYType() {
            string sqly = "", sqlo = "", sqlyz = "";
            sqly = "select title from videotype where tid=2 and state!=0 and Id<67";
            
            sqlo = "select title from videotype where tid=1 and state!=0";
            sqlyz = @"select * from(select Title,(select top 1 Title from videotype where id=a.tid) c1,Id
 from VideoType a where tid in
(select Id from VideoType where title in
(select Title from videotype where Tid!=0  and id not in(317,340,363,364,365,366,367,395,396))) and State=1) b order by id";
            DataSet dsyz = new DataSet();
            dsyz = SqlFunction.Sql_DataAdapterToDS(sqlyz);
            DataSet dsy = new DataSet();
            dsy=SqlFunction.Sql_DataAdapterToDS(sqly);

            DataSet dso = new DataSet();
            dso = SqlFunction.Sql_DataAdapterToDS(sqlo);
            int cy = dsy.Tables[0].Rows.Count;
            int cyz = dsyz.Tables[0].Rows.Count;
            int co = dso.Tables[0].Rows.Count;
            Ytype = new string[cy];
            Otype = new string[co];
            YZBdhp=new double [15]; YZBysl=new double [6]; YZBdme=new double [10];
            YZBkxt=new double [11]; YZBjkz=new double [6]; YZBqwy=new double [10]; YZByyw=new double [5];
            YZBgxq=new double [13]; YZBgst=new double [20]; YZBwdx=new double [4]; YZByet=new double [5];
            OZBxqz=new double [27]; OZBxsz=new double [10]; OZBxpz=new double [12]; OZBgcw=new double [5]; 
            OZBysj=new double [9]; OZByq=new double [10];
            Ydhpz = new List<string>();
           Yyslz = new List<string>();
           Ydmez = new List<string>();
           Ykxtz = new List<string>();
           Yjkzz = new List<string>();
           Yqwyz = new List<string>();
           Yyywz = new List<string>();
           Ygxqz = new List<string>();
           Ygstz = new List<string>();
           Ywdxz = new List<string>();
           Yyetz = new List<string>();
           Oxqzz = new List<string>();
           Oxszz = new List<string>();
           Oxpzz = new List<string>();
           Ogcwz = new List<string>();
           Oysjz = new List<string>();
           Oyqz = new List<string>();
           
            for (int i=0;i<cy ;i++ ) {
                Ytype[i] = dsy.Tables[0].Rows[i][0].ToString();
            }
            for (int j = 0; j < co; j++) {
                Otype[j] = dso.Tables[0].Rows[j][0].ToString();
            }
            for (int n = 0; n < cyz; n++) {
                switch (dsyz.Tables[0].Rows[n][1].ToString()) {
                    case "艺术乐园": Yyslz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "动漫儿歌": Ydmez.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "科学天地": Ykxtz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "健康知识": Yjkzz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "趣味英语": Yqwyz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "语言文字": Yyywz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "国学启蒙": Ygxqz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "故事天地": Ygstz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "动画片"  : Ydhpz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "舞蹈学院": Ywdxz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "育儿天地": Yyetz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;

                    case "戏曲专区": Oxqzz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "相声专区": Oxszz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "小品专区": Oxpzz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "广场舞"  : Ogcwz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "养生健体": Oysjz.Add(dsyz.Tables[0].Rows[n][0].ToString());  break;
                    case "乐曲"    : Oyqz.Add(dsyz.Tables[0].Rows[n][0].ToString());   break;
                }
                
            }
            dsy.Dispose(); dso.Dispose(); dsyz.Dispose();
            string sqlyb = "select  * from UserAccessType order by countNum desc";
            DataSet dsyb = new DataSet();
            dsyb = SqlFunction.Sql_DataAdapterToDS(sqlyb);
            int cm = dsyb.Tables[0].Rows.Count;
            Dictionary<string, int> YBT = new Dictionary<string, int>();
            float dhpc = 0, yslc = 0, dmec = 0, kxtc = 0, jkzc = 0, qwyc = 0, yywc = 0, gxqc = 0, gstc = 0, wdxc = 0, yetc = 0;//幼儿各个分类的个数
            float xqzc = 0, xszc = 0, xpzc = 0, gcwc = 0, ysjc = 0, yqc = 0;//老人各个分类的个数
            float ybc=0, obc=0;//幼儿和老人的点击总数
            string[] b = new string[5];
            string d = b[2];
            for (int m=0;m<cm ;m++ ) {
                if (dsyb.Tables[0].Rows[m]["videoType"].ToString() != null && dsyb.Tables[0].Rows[m]["videoType"].ToString() != "" && dsyb.Tables[0].Rows[m]["videoType"].ToString().Substring(0, 1) != "1" && dsyb.Tables[0].Rows[m]["videoType"].ToString().Substring(0, 1) != "2")
                {
                    b = dsyb.Tables[0].Rows[m]["videoType"].ToString().Split('_');
                    int count = Convert.ToInt32(dsyb.Tables[0].Rows[m]["countNum"]);
                    switch (b[1]) {
                        case "动画片": dhpc += count;
                            switch (b[2]) {
                                case "可可小爱": YZBdhp[0] += count; break;
                                case "大耳朵图图": YZBdhp[1] += count; break;
                                case "小马宝莉": YZBdhp[2] += count; break;
                                case "喜洋洋与灰太狼": YZBdhp[3] += count; break;
                                case "加菲猫的幸福生活": YZBdhp[4] += count; break;
                                case "巧虎来啦": YZBdhp[5] += count; break;
                                case "小熊维尼与跳跳虎": YZBdhp[6] += count; break;
                                case "熊出没": YZBdhp[7] += count; break;
                                case "西游记": YZBdhp[8] += count; break;
                                case "摩尔庄园": YZBdhp[9] += count; break;
                                case "蓝猫淘气三千问": YZBdhp[10] += count; break;
                                case "蜡笔小新": YZBdhp[11] += count; break;
                                case "哆啦A梦": YZBdhp[12] += count; break;
                                case "爱探险的朵拉": YZBdhp[13] += count; break;
                                case "小猪佩奇": YZBdhp[14] += count; break;
                            }
                            break;
                        case "艺术乐园": yslc += count;
                            switch (b[2]) {
                                case "简笔画": YZBysl[0] += count; break;
                                case "绘画技巧": YZBysl[1] += count; break;
                                case "创意绘画": YZBysl[2] += count; break;
                                case "学画画": YZBysl[3] += count; break;
                                case "儿童画教学": YZBysl[4] += count; break;
                                case "智象画室": YZBysl[5] += count; break;
                            }break;
                        case "动漫儿歌": dmec += count;
                            switch (b[2]) {
                                case "生活儿歌": YZBdme[0] += count; break;
                                case "经典儿歌": YZBdme[1] += count; break;
                                case "英文儿歌": YZBdme[2] += count; break;
                                case "儿童童谣": YZBdme[3] += count; break;
                                case "亲宝儿歌": YZBdme[4] += count; break;
                                case "巧虎儿歌": YZBdme[5] += count; break;
                                case "起跑线儿歌": YZBdme[6] += count; break;
                                case "智慧树儿歌": YZBdme[7] += count; break;
                                case "3D儿歌大全": YZBdme[8] += count; break;
                                case "贝瓦儿歌": YZBdme[9] += count; break;
                            }break;
                        case "科学天地": kxtc += count;
                            switch (b[2]) {
                                case "安全宝典": YZBkxt[0] += count; break;
                                case "十万个为什么": YZBkxt[1] += count; break;
                                case "洛宝贝爱科学": YZBkxt[2] += count; break;
                                case "米卡幼幼版": YZBkxt[3] += count; break;
                                case "智力开发": YZBkxt[4] += count; break;
                                case "自然科学": YZBkxt[5] += count; break;
                                case "动物科学": YZBkxt[6] += count; break;
                                case "物理科学": YZBkxt[7] += count; break;
                                case "植物科学": YZBkxt[8] += count; break;
                                case "地理科学": YZBkxt[9] += count; break;
                                case "青青世界": YZBkxt[10] += count; break;
                            }break;
                        case "健康知识": jkzc += count;
                            switch (b[2]) {
                                case "幼儿健康": YZBjkz[0]+=count; break;
                                case "急救超人": YZBjkz[1] += count; break;
                                case "健康特攻队": YZBjkz[2] += count; break;
                                case "布奇奇乐园": YZBjkz[3] += count; break;
                                case "火星娃健康成长": YZBjkz[4] += count; break;
                                case "变形警车伯利": YZBjkz[5] += count; break;
                            }break;
                        case "趣味英语": qwyc += count;
                            switch (b[2]) {
                                case "趣味字母": YZBqwy[0] += count ; break;
                                case "儿童英语": YZBqwy[1] += count; break;
                                case "色拉英语": YZBqwy[2] += count; break;
                                case "英语小天才": YZBqwy[3] += count; break;
                                case "英语顺口溜": YZBqwy[4] += count; break;
                                case "快乐英语": YZBqwy[5]+= count; break;
                                case "旅游学英语": YZBqwy[6] += count; break;
                                case "英语一点通": YZBqwy[7] += count; break;
                                case "神奇校车": YZBqwy[8] += count; break;
                                case "棒棒趣味英语": YZBqwy[9] += count; break;
                            }break;
                        case "语言文字": yywc += count;
                            switch (b[2]) {
                                case "声母": YZByyw[0] += count ; break;
                                case "韵母": YZByyw[1] += count; break;
                                case "拼音童谣": YZByyw[2] += count; break;
                                case "趣味学汉字": YZByyw[3] += count; break;
                                case "语言直通车": YZByyw[4] += count; break;
                            }break;
                        case "国学启蒙": gxqc += count;
                            switch (b[2]) {
                                case "弟子规": YZBgxq[0] += count ; break;
                                case "三字经": YZBgxq[1] += count; break;
                                case "贝瓦唐诗": YZBgxq[2] += count; break;
                                case "亲宝诗词": YZBgxq[3] += count; break;
                                case "唐诗": YZBgxq[4] += count; break;
                                case "宋词精选": YZBgxq[5] += count; break;
                                case "百家姓": YZBgxq[6] += count; break;
                                case "千字文": YZBgxq[7] += count; break;
                                case "老子": YZBgxq[8] += count; break;
                                case "论语": YZBgxq[9] += count; break;
                                case "增广贤文": YZBgxq[10] += count; break;
                                case "诗经": YZBgxq[11] += count; break;
                                case "庄子": YZBgxq[12] += count; break;
                            }break;
                        case "故事天地": gstc += count;
                            switch (b[2]) {
                                case "寓言故事":YZBgst[0]+=count ; break;
                                case "名人故事": YZBgst[1] += count; break;
                                case "成语故事": YZBgst[2] += count; break;
                                case "佛理故事": YZBgst[3] += count; break;
                                case "历史故事": YZBgst[4] += count; break;
                                case "双语故事": YZBgst[5] += count; break;
                                case "益智故事": YZBgst[6] += count; break;
                                case "经典故事": YZBgst[7] += count; break;
                                case "起跑线故事": YZBgst[8] += count; break;
                                case "贝瓦故事": YZBgst[9] += count; break;
                                case "童话故事": YZBgst[10] += count; break;
                                case "睡前故事": YZBgst[11] += count; break;
                                case "安徒生童话": YZBgst[12] += count; break;
                                case "一千零一夜": YZBgst[13] += count; break;
                                case "伊索寓言": YZBgst[14] += count; break;
                                case "格林童话": YZBgst[15] += count; break;
                                case "西游记": YZBgst[16] += count; break;
                                case "水浒传": YZBgst[17] += count; break;
                                case "三国演义": YZBgst[18] += count; break;
                                case "红楼梦": YZBgst[19] += count; break;
                            }break;
                        case "舞蹈学院": wdxc += count;
                            switch (b[2]) {
                                case "幼儿舞蹈": YZBwdx[0] += count ; break;
                                case "小儿舞蹈": YZBwdx[1] += count; break;
                                case "儿童舞蹈": YZBwdx[2] += count; break;
                                case "舞蹈大赛": YZBwdx[3] += count; break;
                            }break;
                        case "育儿天地": yetc += count;
                            switch (b[2]) {
                                case "健康育儿":YZByet[0]+=count ; break;
                                case "科学育儿": YZByet[1] += count; break;
                                case "育婴先锋": YZByet[2] += count; break;
                                case "婴儿画报": YZByet[3] += count; break;
                                case "育儿之道": YZByet[4] += count; break;
                            }break;
                            
                        case "戏曲专区": xqzc += count;
                            switch (b[2]) {
                                case "昆曲": OZBxqz[0] += count; ; break;
                                case "京剧":OZBxqz[1] += count; break;
                                case "越剧":OZBxqz[2] += count; break;
                                case "粤剧":OZBxqz[3] += count; break;
                                case "黄梅戏":OZBxqz[4] += count; break;
                                case "秦腔":OZBxqz[5] += count; break;
                                case "晋剧":OZBxqz[6] += count; break;
                                case "潮剧":OZBxqz[7] += count; break;
                                case "曲剧":OZBxqz[8] += count; break;
                                case "豫剧":OZBxqz[9] += count; break;
                                case "锡剧":OZBxqz[10] += count; break;
                                case "淮剧":OZBxqz[11] += count; break;
                                case "绍剧":OZBxqz[12] += count; break;
                                case "扬剧":OZBxqz[13] += count; break;
                                case "琼剧":OZBxqz[14] += count; break;
                                case "柳琴戏":OZBxqz[15] += count; break;
                                case "蒲剧":OZBxqz[16] += count; break;
                                case "二人转":OZBxqz[17] += count; break;
                                case "坠子":OZBxqz[18] += count; break;
                                case "琴书":OZBxqz[19] += count; break;
                                case "苏州评弹":OZBxqz[20] += count; break;
                                case "沂蒙小调":OZBxqz[21] += count; break;
                                case "湘剧":OZBxqz[22] += count; break;
                                case "走进大戏台":OZBxqz[23] += count; break;
                                case "芗剧":OZBxqz[24] += count; break;
                                case "闽剧":OZBxqz[25] += count; break;
                            }break;
                        case "相声专区": xszc += count;
                            switch (b[2]) {
                                case "相声荟萃":OZBxsz[0] += count; break;
                                case "马三立":OZBxsz[1] += count; break;
                                case "刘宝端":OZBxsz[2] += count; break;
                                case "郭德纲":OZBxsz[3] += count; break;
                                case "奇志大兵":OZBxsz[4] += count; break;
                                case "侯宝林郭全宝":OZBxsz[5] += count; break;
                                case "2012龙缘相声":OZBxsz[6] += count; break;
                                case "CCTV相声大赛":OZBxsz[7] += count; break;
                                case "2013年春晚相声":OZBxsz[8] += count; break;
                                case "高晓攀":OZBxsz[9] += count; break;
                            }break;
                        case "小品专区": xpzc += count;
                            switch (b[2]) {
                                case "小品荟萃":OZBxpz[0] += count; break;
                                case "赵本山小品":OZBxpz[1] += count; break;
                                case "小沈阳小品":OZBxpz[2] += count; break;
                                case "郭冬临小品":OZBxpz[3] += count; break;
                                case "范伟小品":OZBxpz[4] += count; break;
                                case "蔡明小品":OZBxpz[5] += count; break;
                                case "黄宏小品":OZBxpz[6] += count; break;
                                case "CCTV电视小品":OZBxpz[7] += count; break;
                                case "春晚小品":OZBxpz[8] += count; break;
                                case "巩汉林小品":OZBxpz[9] += count; break;
                                case "潘长江小品":OZBxpz[10] += count; break;
                                case "陈佩斯小品":OZBxpz[11] += count; break;
                            }break;
                        case "广场舞": gcwc += count;
                            switch (b[2]) {
                                case "广场舞荟萃":OZBgcw[0] += count; break;
                                case "热门舞队":OZBgcw[1] += count; break;
                                case "广场舞教学":OZBgcw[2] += count; break;
                                case "舞种风格":OZBgcw[3] += count; break;
                                case "最新热门广场舞":OZBgcw[4] += count; break;
                               
                            }break;
                        case "养生健体": ysjc += count;
                            switch (b[2]) {
                                case "瑜伽":OZBysj[0] += count; break;
                                case "太极":OZBysj[1] += count; break;
                                case "健身操":OZBysj[2] += count; break;
                                case "生活养生":OZBysj[3] += count; break;
                                case "饮食养生":OZBysj[4] += count; break;
                                case "养生堂":OZBysj[5] += count; break;
                                case "评书":OZBysj[6] += count; break;
                                case "茶道":OZBysj[7] += count; break;
                                case "书画欣赏":OZBysj[8] += count; break;
                            }break;
                        case "乐曲": yqc += count;
                            switch (b[2]) {
                                case "经典歌曲":OZByq[0] += count; break;
                                case "怀旧歌曲":OZByq[1] += count; break;
                                case "热门歌曲":OZByq[2] += count; break;
                                case "DJ舞曲":OZByq[3] += count; break;
                                case "影视歌曲":OZByq[4] += count; break;
                                case "民族歌曲":OZByq[5] += count; break;
                                case "革命红歌":OZByq[6] += count; break;
                                case "歌星专辑":OZByq[7] += count; break;
                                case "情歌对唱":OZByq[8] += count; break;
                                case "闽南语":OZByq[9] += count; break;
                            }break;
                        default: ; break;
                    }
                }
               
            }
            ybc = dhpc + yslc + dmec + kxtc + jkzc + qwyc + yywc + gxqc + gstc + wdxc + yetc;
            obc = xqzc + xszc + xpzc + gcwc + ysjc + yqc;
            float allcount = ybc + obc;
            YAll = Math.Round(ybc/allcount, 2);
            OAll = Math.Round(obc / allcount, 2);
            bdhpc = Math.Round(((dhpc / ybc) * 100), 2);
            byslc = Math.Round(((yslc / ybc) * 100), 2);
            bdmec = Math.Round(((dmec / ybc) * 100), 2);
            bkxtc = Math.Round(((kxtc / ybc) * 100), 2);
            bjkzc = Math.Round(((jkzc / ybc) * 100), 2);
            bqwyc = Math.Round(((qwyc / ybc) * 100), 2);
            byywc = Math.Round(((yywc / ybc) * 100), 2);
            bgxqc = Math.Round(((gxqc / ybc) * 100), 2);
            bgstc = Math.Round(((gstc / ybc) * 100), 2);
            bwdxc = Math.Round(((wdxc / ybc) * 100), 2);
            byetc = Math.Round(((yetc / ybc) * 100), 2);

            bxqzc = Math.Round(((xqzc / obc) * 100), 2);
            bxszc = Math.Round(((xszc / obc) * 100), 2);
            bxpzc = Math.Round(((xpzc / obc) * 100), 2);
            bgcwc = Math.Round(((gcwc / obc) * 100), 2);
            bysjc = Math.Round(((ysjc / obc) * 100), 2);
            byqc = Math.Round(((yqc / obc) * 100), 2);
            //sqlyz = "select title from videotype where id in(select tid from video where id="++")";
            
            //type = VidtoTitle.Vid_to_Type(Id);
            for (int i=0;i<YZBysl.Length ;i++ )
                YZBysl[i] = Math.Round(YZBysl[i] / yslc * byslc, 2);
            for (int i = 0; i < YZBdme.Length; i++)
                YZBdme[i] = Math.Round(YZBdme[i] / dmec*bdmec, 2);
            for (int i = 0; i < YZBkxt.Length; i++)
                YZBkxt[i] = Math.Round(YZBkxt[i] / kxtc*bkxtc, 2);
            for (int i = 0; i < YZBjkz.Length; i++)
                YZBjkz[i] = Math.Round(YZBjkz[i] / jkzc*bjkzc, 2);
            for (int i = 0; i < YZBqwy.Length; i++)
                YZBqwy[i] = Math.Round(YZBqwy[i] / qwyc* bqwyc, 2) ;
            for (int i = 0; i < YZByyw.Length; i++)
                YZByyw[i] = Math.Round(YZByyw[i] / qwyc* bqwyc, 2) ;
            for (int i = 0; i < YZBgxq.Length; i++)
                YZBgxq[i] = Math.Round(YZBgxq[i] / gxqc* bgxqc, 2) ;
            for (int i = 0; i < YZBgst.Length; i++)
                YZBgst[i] = Math.Round(YZBgst[i] / gstc* bgstc, 2) ;
            for (int i = 0; i < YZBdhp.Length; i++)
                YZBdhp[i] = Math.Round(YZBdhp[i] / dhpc* bdhpc, 2) ;
            for (int i = 0; i < YZBwdx.Length; i++)
                YZBwdx[i] = Math.Round(YZBwdx[i] / wdxc* bwdxc, 2) ;
            for (int i = 0; i < YZByet.Length; i++)
                YZByet[i] = Math.Round(YZByet[i] / yetc* byetc, 2) ;
            for (int i = 0; i < OZBxqz.Length; i++)
                OZBxqz[i] = Math.Round(OZBxqz[i] / xqzc* bxqzc, 2) ;
            for (int i = 0; i < OZBxsz.Length; i++)
                OZBxsz[i] = Math.Round(OZBxsz[i] / xszc* bxszc, 2) ;
            for (int i = 0; i < OZBxpz.Length; i++)
                OZBxpz[i] = Math.Round(OZBxpz[i] / xpzc* bxpzc, 2) ;
            for (int i = 0; i < OZBgcw.Length; i++)
                OZBgcw[i] = Math.Round(OZBgcw[i] / gcwc* bgcwc, 2) ;
            for (int i = 0; i < OZBysj.Length; i++)
                OZBysj[i] = Math.Round(OZBysj[i] / ysjc* bysjc, 2) ;
            for (int i = 0; i < OZByq.Length; i++)
                OZByq[i] = Math.Round(OZByq[i] / yqc* byqc, 2) ;

        }

       
    }
}