using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat {
    public class WeChildToken {
        public WeChildToken(string token, int expires)
        {
            ChildToken = token;
            Expires = expires;
            CreateTime = DateTime.Now;
        }
        public string ChildToken { get; set; }
        public DateTime CreateTime { get; set; }
        public int Expires { get; set; }
    }
}
