using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Me.Common.Data;
using System.IO;

namespace WeChat {
    public class QrChildManager {
        private const int TOTAL_COUNT = 100000;//数据10万限制场景ID最大为10万

        public static void WriteLog(string text)
        {
            StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~") + "/log.txt", true);
            sw.WriteLine(text);
            sw.Close();//写入
        }
        public static Byte[] getQrUrl(string deviceId, string xingeToken, string GetChildToken) {

            string Sql = string.Format("select count(*) num from ChildBind");
            int Count = Convert.ToInt32(SqlHelper.ExecuteScalar(Sql));
            if (Count > 0) {
                string sqlStr = string.Format("select top 1 sceneId from ChildBind order by ID desc");
                int SceneId = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlStr));
                if (SceneId >= TOTAL_COUNT) {
                    SceneId = 0;
                }
               
                int top_count = TOTAL_COUNT / 10;
                if (Count < TOTAL_COUNT && Count >= 0) {
                    Sql = string.Format("insert ChildBind (deviceId,token,sceneId) VALUES ('{0}', '{1}','{2}') ", deviceId, xingeToken, SceneId + 1);
                    SqlHelper.ExecuteNonQuery(Sql);
                } else {
                    Sql = string.Format("delete ChildBind where ID in (select top " + top_count + " ID from ChildBind order by ID )");
                    SqlHelper.ExecuteNonQuery(Sql);
                    Sql = string.Format("insert ChildBind (deviceId,token,sceneId) VALUES ('{0}', '{1}','{2}') ", deviceId, xingeToken, SceneId + 1);
                    SqlHelper.ExecuteNonQuery(Sql);
                }
                WriteLog("getQrUrl");
                Byte[] bytes = QrCodeManager.GenerateTemp(GetChildToken, SceneId + 1);

                return bytes;
            } else {
                string sqlStr = string.Format("insert ChildBind (deviceId,token,sceneId) VALUES ('{0}', '{1}','{2}') ", deviceId, xingeToken, 1);
                SqlHelper.ExecuteNonQuery(sqlStr);
                Byte[] bytes = QrCodeManager.GenerateTemp(GetChildToken, 1);

                return bytes;
            }
        }
        /// <summary>
        /// 想微信幼儿用户表添加用户
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="BindToken"></param>
        public static void ChildUser(string BindToken, string deviceId, string mac, string qrticket) {
            string Sql = "";
            Sql = string.Format("insert ChildChatUser (OpenId,token,deviceId,state,mac,qrticket,userPhone,deviceName) values ('','{0}','{1}',0,'{2}','{3}','','{1}')", BindToken, deviceId, mac, qrticket);
            SqlHelper.ExecuteNonQuery(Sql);
        }
        /// <summary>
        /// 远程管理菜单
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="xingeToken"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean ManageMenu(string openId, string xingeToken, int type) {
            ////序列化对象
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string js = "{\"Title\":\"推送消息\",\"Type\":" + type + ",\"OpenId\":\"" + openId + "\",\"Content\":\"\"}";
            Message ms = new Message("e家亲幼儿伴侣", js);
            string returnStr = XinGePush.PushMsg(xingeToken, jsonSerializer.Serialize(ms));
            JObject json = JObject.Parse(returnStr);
            returnStr = json["ret_code"].ToString();
            if (returnStr == "0") {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据openId获取推送Token
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public static string GetXingeToken(string OpenId) {
            string Sql = string.Format("select token from ChildChatUser where OpenId='{0}' and state=1 and userphone != ''", OpenId);
            string token = SqlHelper.ExecuteScalar(Sql)+"";
            return token;
        }

        /// <summary>
        /// 根据Token获取获取DeviceId
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public static string GetDeviceId(string xingeToken) {
            string Sql = string.Format("select deviceId from ChildChatUser where Token='{0}' and state=1", xingeToken);
            string deviceId = SqlHelper.ExecuteScalar(Sql) + "";
            return deviceId;
        }
        public static DataTable GetOpenId(string deviceId) {
            string Sql = string.Format("select OpenId, deviceName from ChildChatUser where deviceId='{0}' order by CreateDate", deviceId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            return dt;
        }

        public static string GetQrticket(string mac, string token) {
            string Sql = string.Format("select qrticket from ChildChatUser where mac='{0}'", mac);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            string qrticket = "";
            if (dt != null&&dt.Rows.Count > 0) {
                qrticket = Convert.ToString(dt.Rows[0]["qrticket"]);
            } else {
                return null;
            }
            SqlHelper.ExecuteNonQuery(string.Format("update ChildChatUser set token = '{0}' where mac = '{1}'", token, mac));
            return qrticket;
        }

        public static string Get_xingeToken(string device_id) {
            string Sql = string.Format("select token from ChildChatUser where deviceId='{0}'", device_id);
            DataTable dt = SqlHelper.ExecuteTable(Sql);


            string token = "";
            if (dt.Rows.Count > 0) {
                token = Convert.ToString(dt.Rows[0]["token"]);
            } else {

            }
            return token;
        }

        /// <summary>
        /// 根据传送的XingeToken获取推送Token
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public static string GetUserToken(string Token) {
            string Sql = string.Format("select token from ChildChatUser where OpenId='{0}' and state=1", Token);
            DataTable dt = SqlHelper.ExecuteTable(Sql);

            string token = "";
            if (dt.Rows.Count > 0) {
                token = Convert.ToString(dt.Rows[0]["token"]);
            } else {

            }
            return token;
        }
        public static List<Dictionary<string, string>> GetDevices(string openId) {
            string Sql = string.Format("select * from ChildChatUser where OpenId='{0}' and userphone != ''  order by CreateDate desc", openId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt == null || dt.Rows.Count == 0) {
                return null;
            }
            List<Dictionary<string, string>> devices = new List<Dictionary<string, string>>();
            if (dt.Rows.Count > 0) {
                foreach (DataRow row in dt.Rows) {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("deviceId", row["deviceId"].ToString());
                    dic.Add("deviceName", row["deviceName"].ToString());
                    dic.Add("qrticket", row["qrticket"].ToString());
                    dic.Add("state", row["state"].ToString());
                    devices.Add(dic);
                }
            }
            return devices;
        }
        public static void UpdateState(string deviceId, string state, string openId) {
            string Sql = string.Format("update ChildChatUser set state={0} where deviceId = '{1}' and openId = '{2}'", state, deviceId, openId);
            SqlHelper.ExecuteNonQuery(Sql);
        }
        /// <summary>
        /// 删除取消关注的用户
        /// </summary>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public static void DeleteOpenId(string OpenId) {
            string sql = string.Format("delete from ChildChatUser where OpenId = '{0}'", OpenId);
            SqlHelper.ExecuteNonQuery(sql);
            /*string sql = string.Format("select * from ChildChatUser where OpenId = '{0}'", OpenId);
            //DBHelper.ExecuteNonQueryString("insert into xmlString values('" + OpenId + "')");
            DataTable dt = DBHelper.Instance().GetDataTableBySql(sql);
            foreach (DataRow row in dt.Rows) {
                sql = "select * from ChildChatUser where deviceId = '" + row["deviceId"] + "'";
                DataTable dt2 = DBHelper.Instance().GetDataTableBySql(sql);
                if (dt2.Rows.Count > 1) {
                    sql = string.Format("delete from ChildChatUser where Id = {0}", row["Id"]);
                    DBHelper.Instance().ExecuteNonQuery(sql);
                } else {
                    sql = string.Format("update ChildChatUser set OpenId = '', state=0 where Id = {0}", row["Id"]);
                    DBHelper.Instance().ExecuteNonQuery(sql);
                }
            }*/
        }
        public static void DeleteOpenId(string OpenId, string deviceId) {
            string sql = string.Format("delete from ChildChatUser where OpenId = '{0}' and deviceId = '{1}'", OpenId, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
        }
        public static bool DeleteDevice(string deviceId) {
            string sql = string.Format("delete from ChildChatUser where deviceId = '{0}'", deviceId);
            return SqlHelper.ExecuteNonQuery(sql)>0;
        }
        public static void UpdateOpenId(string OpenId, List<string> deviceId) {
            int i = 0;
            foreach (var item in deviceId) {
                string sql = string.Format("select * from ChildChatUser where deviceId = '{0}'", item);
                DataTable dt = SqlHelper.ExecuteTable(sql);
                if (dt != null && dt.Rows.Count > 0) {
                    foreach (DataRow row in dt.Rows) {
                        if (row["OpenId"]+"" == "") {
                            i++;
                            if (i == 1) {
                                sql = string.Format("update ChildChatUser set OpenId = '{0}', state=1 where deviceId = '{1}' and OpenId = ''", OpenId, item);
                            } else {
                                sql = string.Format("update ChildChatUser set OpenId = '{0}', state=0 where deviceId = '{1}' and OpenId = ''", OpenId, item);
                            }
                            SqlHelper.ExecuteNonQuery(sql);
                        } else {
                            sql = string.Format("select * from ChildChatUser where deviceId = '{0}' and OpenId = '{1}'", item, OpenId);
                            DataTable dt2 = SqlHelper.ExecuteTable(sql);
                            //DBHelper.ExecuteNonQueryString("insert into xmlString values ('" + item + OpenId + "')");
                            if (dt2.Rows.Count == 0) {
                                if (row["OpenId"].ToString() != OpenId) {
                                    sql = string.Format("insert ChildChatUser (OpenId,token,deviceId,state,mac,qrticket,userPhone,deviceName) values ('{0}','{1}','{2}',1,'{3}','{4}','','{2}')", OpenId, row["token"], item, row["mac"], row["qrticket"]);
                                    SqlHelper.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void UpdatePhone(string openId, string phone) {
            string sql = string.Format("update ChildChatUser set userPhone = '{0}' where OpenId = '{1}'", phone, openId);
            SqlHelper.ExecuteNonQuery(sql);
        }
        public static bool UpdateDeviceName(string openId, string deviceName) {
            string sql = string.Format("update ChildChatUser set deviceName = '{0}' where OpenId = '{1}' and state = 1", deviceName, openId);
            return SqlHelper.ExecuteNonQuery(sql)>0;

        }
        public static void UpdateState2(string openId, string deviceName) {
            string sql = string.Format("update ChildChatUser set state = 0 where OpenId = '{0}'", openId);
            SqlHelper.ExecuteNonQuery(sql);
            sql = string.Format("update ChildChatUser set state = 1 where OpenId = '{0}' and deviceName = '{1}'", openId, deviceName);
            SqlHelper.ExecuteNonQuery(sql);
        }
        public static void UpdateState3(string openId, string deviceId) {
            string sql = string.Format("update ChildChatUser set state = 0 where OpenId = '{0}'", openId);
            SqlHelper.ExecuteNonQuery(sql);
            sql = string.Format("update ChildChatUser set state = 1 where OpenId = '{0}' and deviceId = '{1}'", openId, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
        }
        public static void DelDervice(string openId, string deviceId) {
            string sql = string.Format("delete from ChildChatUser where OpenId = '{0}' and deviceId = '{1}'", openId, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
            sql = string.Format("select * from ChildChatUser where openId = '{0}' order by CreateDate desc", openId);
            DataTable dt = SqlHelper.ExecuteTable(sql);
            if (dt != null && dt.Rows.Count > 0) {
                sql = string.Format("select * from ChildChatUser where openId = '{0}' and state = 1", openId);
                DataTable dt2 = SqlHelper.ExecuteTable(sql);
                if (dt2 != null && dt2.Rows.Count > 0) {
                    return;
                } else {
                    sql = string.Format("update ChildChatUser set state = 1 where Id = {0}", dt.Rows[0]["Id"]);
                    SqlHelper.ExecuteNonQuery(sql);
                }
            }
        }
        public static bool GetD(string openId, string deviceName) {
            string Sql = string.Format("select * from ChildChatUser where OpenId='{0}' and state=1 and deviceName = '{1}'", openId, deviceName);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                return true;
            } else {
                return false;
            } 
        }
        /// <summary>
        /// 获取openId判断存储的值是否去数据库对应
        /// </summary>
        /// <param name="OpenId"></param>
        public static string GetXingeOpenId(string OpenId) {
            string Sql = string.Format("select OpenId from ChildChatUser where OpenId='{0}' and state=1 and userphone != ''", OpenId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                string openId = Convert.ToString(dt.Rows[0]["OpenId"]);
                return openId;
            } else {
                return "";
            }           
        }

        /// <summary>
        /// 根据openId获取token
        /// </summary>
        /// <param name="OpenId"></param>
        public static string GetToken(string OpenId) {
            string Sql = string.Format("select token  from ChildChatUser where OpenId='{0}' and state=1 and userphone != ''", OpenId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                string token = Convert.ToString(dt.Rows[0]["token"]);
                return token;
            } else {
                return "";
            }
        }


        /// <summary>
        /// 根据openId获取deviceId
        /// </summary>
        /// <param name="OpenId"></param>
        public static string GetChildDeviceId(string OpenId) {
            string Sql = string.Format("select deviceId  from ChildChatUser where OpenId='{0}' and state=1 and userphone is not null ", OpenId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                string deviceId = Convert.ToString(dt.Rows[0]["deviceId"]);
                return deviceId;
            } else {
                return "";
            }
        }

        public static string GetChildDeviceName(string OpenId) {
            string Sql = string.Format("select deviceName from ChildChatUser where OpenId='{0}' and state=1", OpenId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                string deviceName = Convert.ToString(dt.Rows[0]["deviceName"]);
                return deviceName;
            } else {
                return "";
            }
        }

        /// <summary>
        /// 根据openId获取deviceId
        /// </summary>
        /// <param name="OpenId"></param>
        public static string GetStudyDeviceId(string DeviceId) {
            string Sql = string.Format("select deviceId  from ChildChatUser where DeviceId='{0}' and state=1", DeviceId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            if (dt.Rows.Count > 0) {
                string deviceId = Convert.ToString(dt.Rows[0]["deviceId"]);
                return deviceId;
            } else {
                return "";
            }
        }

        public static Dictionary<string, object> OldBind(string SceneId) {
            string Sql = string.Format("select token ,deviceId from ChildBind where sceneId='{0}'", SceneId);
            DataTable dt = SqlHelper.ExecuteTable(Sql);
            string token = Convert.ToString(dt.Rows[0]["token"]);
            string deviceId = Convert.ToString(dt.Rows[0]["deviceId"]);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("token", token);
            dic.Add("deviceId", deviceId);
            return dic;
        }
        public static bool OldChildUser(string OpenId, string BindToken, string deviceId) {
            string Sql = "";
            Sql = string.Format("select * from ChildChatUser where OpenId='{0}' and deviceId = '{1}'", OpenId, deviceId);
            DataTable ds = SqlHelper.ExecuteTable(Sql);
            if (ds.Rows.Count > 0) {
                Sql = string.Format("update ChildChatUser set Token = '{0}' where deviceId = '{1}'", BindToken, deviceId);
                SqlHelper.ExecuteNonQuery(Sql);
                return false;
            }
            string sql = string.Format("insert into ChildChatUser (Token, OpenId, deviceId, state, userPhone, deviceName, qrticket) values ('{1}','{0}','{2}', 1, '', '{2}', '../Img/jqr.png')", OpenId, BindToken, deviceId);
            if (SqlHelper.ExecuteNonQuery(sql) > 0)
            {
                UpdateState3(OpenId, deviceId);
                return true;
            }
            return false;
        }

        public static void UpdateHeadImg(string openId, string deviceId, string imgUrl)
        {
            string sql = string.Format("update ChildChatUser set qrticket = '{0}' where OpenId = '{1}' and deviceId = '{2}'", imgUrl, openId, deviceId);
            SqlHelper.ExecuteNonQuery(sql);
        }
    }
}
