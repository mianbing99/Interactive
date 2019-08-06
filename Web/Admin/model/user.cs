using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Admin.model {
    public class user {
        public int id { get; set; }
        public string name { get; set; }
        public string pwd { get; set; }
        public int state { get; set; }
        public int add { get; set; }
        public int delete { get; set; }
        public int update { get; set; }
        public int select { get; set; }
        public int register { get; set; }//注册
        public int export { get; set; }//导出
        public string phone { get; set; }
        public string email { get; set; }
    }
}