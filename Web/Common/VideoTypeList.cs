using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeChat;
using Me.Common.Data;
using System.Data;


namespace Web.Common {
    public class VideoTypeList {
        private VideoTypeList() {
        }
        public static VideoTypeList ls;//类型为VideoTypeList的全局字段
        /// <summary>
        /// 用于实例化全局静态字段的，并返回全局静态字段方法
        /// </summary>
        /// <returns></returns>
        public static VideoTypeList Instance() {
            if (ls == null) {
                ls = new VideoTypeList();
            }
            return ls;
        }

        /// <summary>
        /// 获取幼儿听一听列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeList() {
            string sql = string.Format("select * from [VideoType] where Id='63'or Id='56'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿唱一唱列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListtwo() {
            string sql = string.Format("select * from [VideoType] where TId='340'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();
        }
        /// <summary>
        /// 获取幼儿玩一玩列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListthree() {
            string sql = string.Format("select * from [VideoType] where Id='55'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();
        }
        /// <summary>
        /// 获取幼儿学一学列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListfour() {
            string sql = string.Format("select * from [VideoType] where Id='57'or id='58'or id='62'or id='365'or id='363'or id='364'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();
        }
        /// <summary>
        /// 获取幼儿早教乐园列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListfive() {
            string sql = string.Format("select * from [VideoType] where TId='367'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();
        }
        /// <summary>
        /// 获取幼儿育儿天地列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListsix() {
            string sql = string.Format("select * from [VideoType] where TId='66'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();
        }
        /// <summary>
        /// 获取幼儿语言列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListSevens() {
            string sql = string.Format("select * from [VideoType] where Id='365'or id='364'or id='63'or id='62'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿科学列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListEight() {
            string sql = string.Format("select * from [VideoType] where Id='363'or id='99'or id='346'or id='57'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿艺术列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListNine() {
            string sql = string.Format("select * from [VideoType] where Id='55'or id='56'or id='340'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿健康列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListTen() {
            string sql = string.Format("select * from [VideoType] where TId='395' or Tid='58'");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿社会列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListEleven() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=396");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿我的幼儿园列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeListTwelve() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=366");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        public List<VideoTypeS> GetVideoTypeListanquan() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=476");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿故事天地列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeStory() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=63 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿动漫儿歌列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeSinger() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=56 and state=1 order by Sort");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿艺术乐园列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeyishu() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=55 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
         /// <summary>
        /// 获取幼儿科学天地列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypekexue() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=57 and State = '1' ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿健康知识列表
        /// </summary>
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypejiankang() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=58 ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        public List<VideoTypeS> GetVideoTypecc(int id) {
            string sql = string.Format("select * from [dbo].[VideoType] where tid={0}",id);
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿国学诗词列表
        /// </summary>GetVideoTypesong
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeguoxue() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=62 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿启蒙数学列表
        /// </summary>GetVideoTypesong
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeshuxue() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=363 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿启蒙语文列表
        /// </summary>GetVideoTypesong
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeyuwen() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=365 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
        /// <summary>
        /// 获取幼儿启蒙英语列表
        /// </summary>GetVideoTypesong
        /// <returns></returns>
        public List<VideoTypeS> GetVideoTypeyingyu() {
            string sql = string.Format("select * from [dbo].[VideoType] where tid=364 order by Sort ");
            return SqlHelper.ExecuteReader(sql).ToList<VideoTypeS>();

        }
    }
}