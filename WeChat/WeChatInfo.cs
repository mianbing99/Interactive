using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat
{
    public class WeChatToken
    {
        public WeChatToken(string token, int expires)
        {
            Token = token;
            Expires = expires;
            CreateTime = DateTime.Now;
        }
        public string Token { get; set; }
        public DateTime CreateTime { get; set; }
        public int Expires { get; set; }
    }
}
