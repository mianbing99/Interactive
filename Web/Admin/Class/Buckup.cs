using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Admin.Class {
    public class Buckup {
        public static int getBeforeInfo(string tb, string column) {
            int i = 0;
            try {
                switch (tb) {
                    case "Video":
                        switch (column) {
                            case "Tid": i = 3; break;
                            case "Title": i = 4; break;
                            case "Cover": i = 5; break;
                            case "Describe": i = 6; break;
                            case "CreateDate": i = 7; break;
                            case "Sort": i = 8; break;
                            case "State": i = 9; break;
                            case "Definition": i = 10; break;
                            case "Advertisement": i = 11; break;
                        }; break;
                    case "VideoType":
                        switch (column) {
                            case "Tid": i = 3; break;
                            case "Title": i = 4; break;
                            case "Cover": i = 5; break;
                            case "Sort": i = 6; break;
                            case "State": i = 7; break;
                        }; break;
                    case "VideoUrl":
                        switch (column) {
                            case "Vid": i = 3; break;
                            case "Source": i = 4; break;
                            case "Path": i = 5; break;
                            case "TempPath": i = 6; break;
                            case "State": i = 7; break;
                            case "Sort": i = 8; break;
                            case "CreateDate": i = 9; break;
                            case "Describe": i = 10; break;
                        }; break;

                }
            } catch {
                return 0;
            }
            return i;
        }
        public static bool UpdateBackup(string tb, string column, int update_id, string beforeInfo, string afterInfo, DateTime changTime,string or,string ip) {

            string sql = "";
            try {
                sql = "insert into Backup_update_tb Values('" + tb + "'," +
                                                          "'" + column + "'," +
                                                          "'" + update_id + "'," +
                                                          "'" + beforeInfo + "'," +
                                                          "'" + afterInfo + "'," +
                                                          "'" + changTime + "'," +
                                                          "'" + or + "'," +
                                                          "'" + ip + "')";
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }
        static public bool Buckup_Delete_video(string tb, DateTime addtime, string opator,
          string ip,string c_id, string columnV2, string columnV3, string columnV4, string columnV5,
           string columnV6, string columnV7, string columnV8, string columnV9, string columnV10) {
            string sql = "";
            try {
                sql = "insert into Backup_delete_tb(tb,time,operator,ip,column_value1,column_value2," +
                      "column_value3,column_value4,column_value5,column_value6,column_value7,column_value8,column_value9,column_value10)" +
                      "values("+
                      "'" + tb + "'," +
                      "'" + addtime + "'," +
                      "'" + opator + "'," +
                      "'" + ip + "'," +
                      "'" + c_id + "'," +
                      "'" + columnV2 + "'," +
                      "'" + columnV3 + "'," +
                      "'" + columnV4 + "'," +
                      "'" + columnV5 + "'," +
                      "'" + columnV6 + "'," +
                      "'" + columnV7 + "'," +
                      "'" + columnV8 + "'," +
                      "'" + columnV9 + "'," +
                      "'" + columnV10 + "')";
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }

        }
        static public bool Buckup_Delete_type(string tb, DateTime addtime, string opator,
            string ip,string c_id, string columnV2, string columnV3, string columnV4,
            string columnV5,string columnV6) {
            string sql = "";
            try {
                sql = "insert into Backup_delete_tb(tb,time,operator,ip," +
                      "column_value1,column_value2,column_value3,column_value4,column_value5,column_value6)" +
                      "values(" +
                      "'" + tb + "'," +
                      "'" + addtime + "'," +
                      "'" + opator + "'," +
                      "'" + ip + "'," +
                      "'" + c_id + "'," +
                      "'" + columnV2 + "'," +
                      "'" + columnV3 + "'," +
                      "'" + columnV4 + "'," +
                      "'" + columnV5 + "'," +
                      "'" + columnV6 + "')";
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }

        static public bool Buckup_Delete_url(string tb, DateTime c_time, string opator,
            string ip,string c_id, string columnV2, string columnV3, string columnV4,
            string columnV5, string columnV6,string columnV7, string columnV8) {
            string sql = "";
            try {
                sql = "insert into Backup_delete_tb(tb,time,operator,ip," +
                      "column_value1,column_value2,column_value3,column_value4,column_value5,column_value6,column_value7,column_value8)" +
                      "values("+
                      "'" + tb + "'," +
                      "'" + c_time + "'," +
                      "'" + opator + "'," +
                      "'" + ip + "'," +
                      "'" + c_id + "'," +
                      "'" + columnV2 + "'," +
                      "'" + columnV3 + "'," +
                      "'" + columnV4 + "'," +
                      "'" + columnV5 + "'," +
                      "'" + columnV6 + "'," +
                      "'" + columnV7 + "'," +
                      "'" + columnV8 + "')";
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }

        }
        static public bool Buckup_DownloadExcel(string admin,string ip,string type) {
            try {
                DateTime dt = DateTime.Now;
                string sql = string.Format(@"insert into Backup_downloadExcel
                            (admin,ip,type,time) 
                            values('{0}','{1}','{2}','{3}')", admin, ip, type, dt.ToString());
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }
        static public bool Buckup_AccountOperation(string operation, string admin,string ip,
            string username,string info) {
            try {
                DateTime dt = DateTime.Now;
                string sql = string.Format(@"insert into Backup_AccountOperation
                            (operation,admin,ip,time,username,info) 
                            values('{0}','{1}','{2}','{3}','{4}','{5}')", operation, admin,ip, dt.ToString(), username, info);
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }
        static public bool Buckup_Add(string tb, string admin, string ip, int addnum) {
            try {
                DateTime dt = DateTime.Now;
                string sql = string.Format(@"insert into Backup_add_tb
                            (tb,time,admin,ip,addnum)
                            values('{0}','{1}','{2}','{3}','{4}')", tb, dt.ToString(), admin, ip, addnum);
                if (SqlFunction.Sql_UpdatePL(sql) > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }
    }
}