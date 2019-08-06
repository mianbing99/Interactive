using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeChat;
using Me.Common.Data;

namespace Web.Common {
    /// <summary>
    /// 类型数据库类
    /// </summary>
    public class AlarmClocksLogic {
        /// <summary>
        /// 私有的构造函数
        /// </summary>
        private AlarmClocksLogic() {

        }
        private static AlarmClocksLogic Acl;//为类型AlarmClocksLogic的全局字段

        public static AlarmClocksLogic Instance() {
            if (Acl == null) {
                Acl = new AlarmClocksLogic();
            }
            return Acl;
        }

        /// <summary>
        /// 获取幼儿作息管理列表string DeviceId
        /// string sql = string.Format("select * from AlarmClock where DeviceId='{0}'", DeviceId);
        /// </summary>
        /// <returns></returns>
        public List<AlarmClocks> GetAlarmClock(string DeviceId) {
            string sql = string.Format("select * from AlarmClock where DeviceId='{0}'", DeviceId);
             List<AlarmClocks> ack =SqlHelper.ExecuteReader(sql).ToList<AlarmClocks>();
             if (ack == null || ack.Count() <= 0)
             {
                 _insert(DeviceId, "07:00", "该起床了", "起床");
                 _insert(DeviceId, "07:20", "该吃饭了", "吃饭");
                 _insert(DeviceId, "07:40", "该上学了", "上学");
                 _insert(DeviceId, "12:00", "该午休了", "午休");
                 _insert(DeviceId, "17:00", "该运动了", "运动");
                 _insert(DeviceId, "19:00", "该做作业了", "做作业");
                 _insert(DeviceId, "21:00", "该洗澡了", "洗澡");
                 _insert(DeviceId, "22:00", "该睡觉了", "睡觉");
                 ack = SqlHelper.ExecuteReader(sql).ToList<AlarmClocks>();
             }
             return ack;
        }
        /// <summary>
        /// 删除闹钟时间
        /// </summary>
        /// <param name="prodcutid"></param>
        /// <returns></returns>
        public bool DeleteClock(int ID) {
            string sql = string.Format("delete from AlarmClock where Id='{0}'", ID);//删除语句
            return SqlHelper.ExecuteNonQuery(sql)>0;
        }
        public bool _insert(string DeviceId,string time,string cont,string type) {
            AlarmClocks al = new AlarmClocks();
            al.ClockTime = time;
            al.DeviceId = DeviceId;
            al.Content = cont;
            al.Repeat = "0";
            al.RepeatDate = "1,1,1,1,1,0,0";
            al.AlarmType = type;
            al.State = 0;
            al.Frequency = 1;
            al.Interval = 3;
           return insertAlarm(al);
        }
        /// <summary>
        /// 传值，无参
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static AlarmClocks getAllgoodconten(string content, string deviceId) {
            string sql = string.Format("select  * from [AlarmClock] where content ='{0}' and deviceId='{1}' ", content, deviceId);
            return SqlHelper.ExecuteReader(sql).To<AlarmClocks>();
        }
        public static List<AlarmClocks> getAllgoodlist(string deviceid) {
            string sql = string.Format("select  * from [AlarmClock] where  deviceid='{0}'",  deviceid);
            return SqlHelper.ExecuteReader(sql).ToList<AlarmClocks>();
        }
        public static AlarmClocks getAllgoodID(int ID) {
            string sql = string.Format("SELECT * FROM [AlarmClock] where id='{0}'", ID);
            return SqlHelper.ExecuteReader(sql).To<AlarmClocks>();
        }
        public static AlarmClocks getAllgoodIDDeviceid(int ID, string Deviceid) {
            string sql = string.Format("SELECT * FROM [AlarmClock] where id='{0}' and Deviceid={1}", ID, Deviceid);
            return SqlHelper.ExecuteReader(sql).To<AlarmClocks>();
        }
        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool updateAlarm(AlarmClocks ID) {
            string sql = string.Format("update AlarmClock set ClockTime='{0}',[Content]='{1}',[Repeat]='{2}',RepeatDate='{3}',Frequency='{4}',Interval='{5}',alarmtype='{6}',state='{7}' where [id]='{8}'", ID.ClockTime, ID.Content, ID.Repeat, ID.RepeatDate, ID.Frequency, ID.Interval,ID.AlarmType,ID.State,ID.Id);
            return SqlHelper.ExecuteNonQuery(sql)>0;
        }
        public static bool insertAlarm(AlarmClocks ID) {
            string sql = string.Format("insert into AlarmClock values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", ID.ClockTime,ID.DeviceId, ID.Content, ID.Repeat, ID.RepeatDate, ID.Frequency, ID.Interval, ID.AlarmType, ID.State);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
        public static bool  UpdateState(int state,int id) {
            string sql = string.Format("update [dbo].[AlarmClock] set [state] = {0} where id ={1} ", state, id);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
        public static bool deleteAlarm(int id) {
            string sql = string.Format("delete [dbo].[AlarmClock] where id = {0}",id);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
        public static bool UpdateByID(AlarmClocks a) {
            string sql = string.Format("update [AlarmClock] set ClockTime='{0}',content='{1}',[Repeat]='{2}',RepeatDate='{3}',Frequency='{4}',Interval='{5}',alarmtype='{6}',[State]='{7}' where id='{8}'",a.ClockTime,a.Content,a.Repeat,a.RepeatDate,a.Frequency,a.Interval,a.AlarmType,a.State,a.Id);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
        public static bool UpdateByIDDeviceid(AlarmClocks a) {
            string sql = string.Format("update [AlarmClock] set ClockTime='{0}',content='{1}',[Repeat]='{2}',RepeatDate='{3}',Frequency='{4}',Interval='{5}',alarmtype='{6}',[State]='{7}' where id='{8}' and Deviceid = '{9}'", a.ClockTime, a.Content, a.Repeat, a.RepeatDate, a.Frequency, a.Interval, a.AlarmType, a.State, a.Id,a.DeviceId);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
         public static string insert_id(AlarmClocks ID) {
             string sql = string.Format("insert into AlarmClock values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') SELECT @@IDENTITY", ID.ClockTime, ID.DeviceId, ID.Content, ID.Repeat, ID.RepeatDate, ID.Frequency, ID.Interval, ID.AlarmType, ID.State);
             return SqlHelper.ExecuteScalar(sql)+"";
        }
         public static bool deleteAlarmdeviceid(int id, string deviceid) {
             string sql = string.Format("delete [dbo].[AlarmClock] where id = {0} and deviceid='{1}'", id, deviceid);
             return SqlHelper.ExecuteNonQuery(sql) > 0;
         }
    }
}