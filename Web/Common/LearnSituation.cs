using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeChat;
using Me.Common.Data;
using System.Data;

namespace Web.Common {
    /// <summary>
    /// 类型数据库类
    /// </summary>
  
    public class LearnSituation {
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private LearnSituation() {
        }
        public static LearnSituation ls;//类型为ProductLogic的全局字段
        /// <summary>
        /// 用于实例化全局静态字段的，并返回全局静态字段方法
        /// </summary>
        /// <returns></returns>
        public static LearnSituation Instance() {
            if (ls == null) {
                ls = new LearnSituation();
            }
            return ls;
        }

        string startDate;
        string endDate;



        /// <summary>
        /// 获取幼儿学习内容和时间的方法
        /// </summary>
        /// <returns>List<Category>集合</returns>
        public List<LearnSit> GetAllLearnSit(string deviceID, int yeshu)
        {
            DateTime date = DateTime.Now;
            yeshu = 15 * yeshu;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by Id desc)number,* from LearnSituation where CreateTime between '{1}' and '{2}' and DeviceId = '{3}') ls where ls.number >{4}", 15, date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), deviceID, yeshu);//查询当天时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }
        /// <summary>
        /// 查询昨天的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetYesdLearn(string deviceID, int yeshu)
        {
            DateTime date = DateTime.Now;
            startDate = date.AddDays(-1).ToString("yyyy-MM-dd");
            endDate = date.ToString("yyyy-MM-dd");
            yeshu = 15 * yeshu;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by Id desc)number,* from LearnSituation where CreateTime between '{1}' and '{2}' and DeviceId = '{3}') ls where ls.number >{4}", 15, startDate, endDate, deviceID, yeshu);
            //string sql = string.Format("select * from [dbo].[LearnSituation] where deviceID='{0}' and convert(varchar(11),createtime,120)='{1}' order by createtime desc", deviceID, Yesdate);//查询昨天时间
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }

        /// <summary>
        /// 查询一周的学习情况
        /// </summary>
        public List<LearnSit> GetWeekLearn(string deviceID, int yeshu)
        {
            DateTime date = DateTime.Now;
            startDate = date.AddDays(-6).ToString("yyyy-MM-dd");
            endDate = date.AddDays(1).ToString("yyyy-MM-dd");
            yeshu = 15 * yeshu;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by Id desc)number,* from LearnSituation where CreateTime between '{1}' and '{2}' and DeviceId = '{3}') ls where ls.number >{4}", 15, startDate, endDate, deviceID, yeshu);
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();

        }

        /// <summary>
        /// 查询一个月的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetMothLearn(string deviceID, int yeshu)
        {
            DateTime date = DateTime.Now;
            yeshu = 15 * yeshu;
            startDate = date.AddMonths(-1).ToString("yyyy-MM-dd");
            endDate = date.AddDays(1).ToString("yyyy-MM-dd");
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by Id desc)number,* from LearnSituation where CreateTime between '{1}' and '{2}' and DeviceId = '{3}') ls where ls.number >{4}", 15, startDate, endDate, deviceID, yeshu);
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }

        /// <summary>
        /// 查询更久之前的学习情况
        /// </summary>
        /// <returns></returns>
        public List<LearnSit> GetForTimeLearn(string deviceID, int yeshu)
        {
            DateTime date = DateTime.Now;
            endDate = date.AddDays(1).ToString("yyyy-MM-dd");
            yeshu = 15 * yeshu;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by id desc) number,* from LearnSituation where createTime < '{1}' and deviceId = '{2}') ls where ls.number >{3}", 15, endDate, deviceID, yeshu);
            return SqlHelper.ExecuteReader(sql).ToList<LearnSit>();
        }


        public int setcont1(string deviceID)
        {  //获取今天的总条数
            DateTime date = DateTime.Now;
            string sql = string.Format("select COUNT(1) from LearnSituation where CreateTIme between '{0}' and '{1}' and DeviceId = '{2}' ",date.ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), deviceID);//查询当天时间
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }

        public int setcont2(string deviceID)
        { //获取昨天的总条数
            DateTime date = DateTime.Now;
            string sql = string.Format("select COUNT(1) from LearnSituation where CreateTIme between '{0}' and '{1}' and DeviceId = '{2}' ",date.AddDays(-1).ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"), deviceID);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont3(string deviceID)
        { //获取一周的总条数
            DateTime date = DateTime.Now;
            string sql = string.Format("select COUNT(1) from LearnSituation where CreateTIme between '{0}' and '{1}' and DeviceId = '{2}' ",date.AddDays(-6).ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), deviceID);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont4(string deviceID)
        { //获取一个月的总条数
            DateTime date = DateTime.Now;
            string sql = string.Format("select COUNT(1) from LearnSituation where CreateTIme between '{0}' and '{1}' and DeviceId = '{2}' ", date.AddMonths(-1).ToString("yyyy-MM-dd"), date.AddDays(1).ToString("yyyy-MM-dd"), deviceID);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
        public int setcont5(string deviceID)
        { //获取更久的总条数
            DateTime date = DateTime.Now;
            string sql = string.Format("select COUNT(1) from LearnSituation where convert(varchar(11),createTime,120)<'{0}' and DeviceId = '{1}'", date.AddDays(1).ToString("yyyy-MM-dd"), deviceID);
            return SqlHelper.ExecuteScalar(sql).ToInt();
        }
    }
}