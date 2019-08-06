using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Web.Common;

namespace Web.API {
    /// <summary>
    /// AlarmApi 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class AlarmApi : System.Web.Services.WebService {

        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SelectByID(string id, string Deviceid) {
            //String json = "{\"Count\":\"" + count + "\",\"Data\":" + data + "}";
            if (id != null && Deviceid != "") {
                try {
                    AlarmClocks acls = AlarmClocksLogic.getAllgoodIDDeviceid(Convert.ToInt32(id), Deviceid);
                if (acls.ClockTime != null) {
                    Write(jsonSerializer.Serialize(acls));
                } else {
                    Write("{\"code\":201,\"messge\":\"无数据\"}");
                }
                } 
                catch (Exception e) {
                    Write("{\"code\":202,\"messge\":\"输入参数有误\"}");
                     } 
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SelectByDeviceid(string Deviceid) {
            //String json = "{\"Count\":\"" + count + "\",\"Data\":" + data + "}";
            if (Deviceid != null) {
                List<AlarmClocks> acls = AlarmClocksLogic.getAllgoodlist(Deviceid);
                if (acls.Count !=0) {
                    Write(jsonSerializer.Serialize(acls));
                } else {
                    Write("{\"code\":201,\"messge\":\"无数据\"}");
                }
            }
          
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Insert(string ClockTime, string Deviceid, string Content, string Repeat, string RepeatDate, string Frequency, string Interval, string AlarmType, string State) {
            if (Deviceid != null) {
               
                    AlarmClocks acls = new AlarmClocks();
                    acls.ClockTime = ClockTime;
                    acls.DeviceId = Deviceid;
                    acls.Content = Content;
                    acls.Repeat = Repeat;
                    acls.RepeatDate = RepeatDate;
                    if (Frequency != null) { acls.Frequency = Convert.ToInt32(Frequency); }
                    if (Interval != null) { acls.Interval = Convert.ToInt32(Interval); }
                    if (AlarmType != null) { acls.AlarmType = AlarmType; }
                    if (State != null) { acls.State =Convert.ToInt32( State); }
                    try {
                    string jg = AlarmClocksLogic.insert_id(acls);
                    if (jg != null) {
                        Write("{\"code\":200,\"messge\":\"" + jg + "\"}");
                    } else {
                        Write("{\"code\":201,\"messge\":\"添加失败\"}");
                    }
                } catch (Exception e) {
                    Write("{\"code\":202,\"messge\":\"输入参数有误\"}");
                }
                
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DeleteByID(string id, string deviceid) {
            if (id != null && deviceid !=null) {
                try {
                    if (AlarmClocksLogic.deleteAlarmdeviceid(Convert.ToInt32(id), deviceid)) {
                        Write("{\"code\":200,\"messge\":\"删除成功\"}");
                    } else {
                        Write("{\"code\":201,\"messge\":\"删除失败\"}");
                    }
                } catch (Exception e) {
                    Write("{\"code\":202,\"messge\":\"输入参数有误\"}");
                }
               
            }
           
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateByID(string id,string ClockTime,string Deviceid, string Content, string Repeat,string RepeatDate,string Frequency,string Interval,string AlarmType,string State) {
            if (!id.Equals(null) || id != "" && Deviceid !="") {
                AlarmClocks acls = new AlarmClocks();
                AlarmClocks acl = new AlarmClocks();
                acl.DeviceId = Deviceid;
                acls = AlarmClocksLogic.getAllgoodID(Convert.ToInt32(id));
                if (ClockTime.Equals(null) || ClockTime == "") { acl.ClockTime = acls.ClockTime; } else { acl.ClockTime = ClockTime; }
                if (Content.Equals(null) || Content == "") { acl.Content = acls.Content; } else { acl.Content = Content; }
                if (Repeat.Equals(null) || Repeat == "") { acl.Repeat = acls.Repeat; } else { acl.Repeat = Repeat; }
                if (RepeatDate.Equals(null) || RepeatDate == "") { acl.RepeatDate = acls.RepeatDate; } else { acl.RepeatDate = RepeatDate; }
                if (Frequency.Equals(null) || Frequency=="") { acl.Frequency = acls.Frequency; } else { acl.Frequency = Convert.ToInt32(Frequency); }
                if (Interval.Equals(null) || Interval == "") { acl.Interval = acls.Interval; } else { acl.Interval = Convert.ToInt32(Interval); }
                if (AlarmType.Equals(null) || AlarmType == "") { acl.AlarmType = acls.AlarmType; } else { acl.AlarmType = AlarmType; }
                if (State.Equals(null) || State == "") { acl.State = acls.State; } else { acl.State = Convert.ToInt32(State); }
                acl.Id = Convert.ToInt32(id);
                if (AlarmClocksLogic.UpdateByID(acl)) {
                    Write("{\"code\":200,\"messge\":\"修改成功\"}");
                } else {
                    Write("{\"code\":201,\"messge\":\"修改失败\"}");
                }
            } else { Write("{\"code\":202,\"messge\":\"输入参数有误\"}"); }
            
        }

        private void Write(string a) {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(a);
        }
    }
}
