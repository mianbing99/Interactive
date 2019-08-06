using Common;
using Me.Common.Data;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WeChat
{
    public class QrInteractiveManager
    {
        public static string getDeviceId(string openId)
        {
            string sql = "select deviceId from StudentDeviceBind where openId='"+openId+"'and userPhone is not null and state=1";
            string deviceId = Convert.ToString(SqlHelper.ExecuteScalar(sql));
            return deviceId;
        }

        public static string getDeviceName(string openId)
        {
            string sql = "select deviceName from StudentDeviceBind where openId='" + openId + "'and userPhone is not null and state=1";
            string deviceId = Convert.ToString(SqlHelper.ExecuteScalar(sql));
            return deviceId;
        }

        public static List<string> getOpenId(string deviceId)
        {
            string sql = "select openId from StudentDeviceBind where deviceId='"+deviceId+"' and userPhone is not null";
            DataTable dt = SqlHelper.ExecuteTable(sql);
            if(dt.Rows.Count==0)
            {
                return null;
            }
            List<string> liststr = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                liststr.Add(item["openId"].ToString());
            }
            return liststr;
        }

        public static int deleteMember(string openId,string deviceId)
        {
            
           
            string sql = string.Format("delete from StudentDeviceBind where openId='{0}' and deviceId='{1}'",openId,deviceId);
            int num = SqlHelper.ExecuteNonQuery(sql);
            string deId = getUserFirstDeviceId(openId);
            if (deId.Length>0)
            {
                sql = string.Format("update StudentDeviceBind set state=1 where openId='{0}' and deviceId='{1}'",openId,deId);
                return SqlHelper.ExecuteNonQuery(sql);
            }
            return num;
        }

        public static string getDeviceToken(string deviceId)
        {
            string sql = "select token from StudentDeviceBind where deviceId = '"+deviceId+"'";
            return SqlHelper.ExecuteScalar(sql).ToString();
        }
        
        public static string getUserFirstDeviceId(string openId)
        {
            string sql = "select deviceId from StudentDeviceBind where openId='"+openId+"' and userPhone is not null order by createTime";
            return Convert.ToString(SqlHelper.ExecuteScalar(sql));
        }

        public static string getDeiveceFirstUser(string deviceId)
        {
            string sql = "select openId from StudentDeviceBind where deviceId = '"+deviceId+"' order by createTime";
            if (SqlHelper.ExecuteScalar(sql)!=null)
            {
                return SqlHelper.ExecuteScalar(sql).ToString();
            }
            return null;
            
        }

        public static int addQrcodeInfo(StudentDeviceQRcode qr)
        {
            string sql = string.Format("insert into StudentDeviceQRcode values('{0}','{1}','{2}','{3}')",qr.deviceId,qr.token,qr.sceneId,qr.createTime);
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public static void updateToken(string deviceId,string token)
        {
            string sql = "update StudentDeviceBind set token='"+token+"' where deviceId='"+deviceId+"'";
            SqlHelper.ExecuteNonQuery(sql);
        }

        public static DataTable getDeviceBindOpIdAndDeId(string deviceId)
        {
            string sql = "select openId,deviceName from StudentDeviceBind where deviceId='"+deviceId+"'";
            return SqlHelper.ExecuteTable(sql);
        }

        public static DataTable getDeviceBindDeIdAndToken(string openId)
        {
            string sql = "select deviceId,token from StudentDeviceBind where openId='"+openId+"' and state=1 and userPhone is not null";
            return SqlHelper.ExecuteTable(sql);
        }

        public static void updateDeviceAvatar(string openId,string deviceId,string path)
        {
            string sql = string.Format("update StudentDeviceBind set deviceAvatar='{0}' where openId='{1}' and deviceId='{2}'", path, openId, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
        }

        

        //使用记录
        public static int getLearnStCount(string deviceId,string endDate)
        {
            string sql = string.Format("select COUNT(1) from StudentLearnSt where convert(varchar(11),createTime,120) < '{0}' and deviceId = '{1}'", endDate, deviceId);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
        }

        public static int getLearnStCount(string deviceId,string startDate,string endDate)
        {
            string sql = string.Format("select COUNT(1) from StudentLearnSt where createTime between '{0}' and '{1}' and deviceId = '{2}'", startDate, endDate, deviceId);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
        }

        public static List<StudentLearnSt> getAllLearnSt(string deviceId, string endDate, int pageSize, int pageIndex)
        {
            pageIndex = pageSize*pageIndex;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by id desc) number,* from StudentLearnSt where createTime < '{1}' and deviceId = '{2}') ls where ls.number >{3}", pageSize, endDate, deviceId, pageIndex);
            List<StudentLearnSt> list = SqlHelper.ExecuteReader(sql).ToList<StudentLearnSt>();
            return list;
        }

        public static List<StudentLearnSt> getLearnSt(string deviceId, string startDate, string endDate, int pageSize, int pageIndex)
        {
            pageIndex = pageIndex * pageSize;
            string sql = string.Format("select top {0} * from (select ROW_NUMBER() OVER(order by id desc)"+
            "number,* from StudentLearnSt where createTime between '{1}' and '{2}' and deviceId = '{3}') ls where ls.number >{4}", pageSize, startDate, endDate, deviceId, pageIndex);
            List<StudentLearnSt> list = SqlHelper.ExecuteReader(sql).ToList<StudentLearnSt>();
            return list;
        }




        public static int addAlarmClock(StudentAlarmClock al)
        {
            string sql = string.Format("insert into StudentAlarmClock values('{0}','{1}','{2}',{3},'{4}',{5},{6},'{7}',{8}) select @@identity"
                , al.clockTime, al.deviceId, al.label, al.repeat, al.repeatDate, al.frequency, al.interval, al.alarmType, al.state);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
        }

        public static int updateAlarmClock(StudentAlarmClock al)
        {
            string sql = string.Format("update StudentAlarmClock set clockTIme='{0}',label='{1}',repeat='{2}',repeatDate='{3}',frequency={4},interval={5},alarmType='{6}',state={7} where id={8}", al.clockTime, al.label, al.repeat, al.repeatDate, al.frequency, al.interval, al.alarmType, al.state, al.id);
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public static int deleteAlarmClock(int id)
        {
            string sql = "delete from StudentAlarmClock where Id="+id+"";
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public static List<StudentAlarmClock> getAllAlarmClock(string deviceId)
        {
            string sql = "select * from StudentAlarmClock where deviceId='"+deviceId+"'";
            return SqlHelper.ExecuteReader(sql).ToList<StudentAlarmClock>();
        }

        public static StudentAlarmClock getAlarmClock(int id)
        {
            string sql = "select * from StudentAlarmClock where id="+id+"";
            SqlDataReader reader = SqlHelper.ExecuteReader(sql);
            StudentAlarmClock ac = null;
            if(reader.Read())
            {
                ac = new StudentAlarmClock();
                ac.id = Convert.ToInt32(reader["id"]);
                ac.deviceId = reader["deviceId"].ToString();
                ac.clockTime = reader["clockTime"].ToString();
                ac.label = reader["label"].ToString();
                ac.repeat = reader["repeat"].ToString();
                ac.repeatDate = reader["repeatDate"].ToString();
                ac.frequency = Convert.ToInt32(reader["frequency"]);
                ac.interval = Convert.ToInt32(reader["interval"]);
                ac.alarmType = reader["AlarmType"].ToString();
                ac.state = Convert.ToInt32(reader["state"]);
            }
            return ac;
        }

    }
}
