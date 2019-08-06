using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Messages
    {

        public Messages(string _title, string _content)
        {
            title = _title;
            content = _content;
            n_id = -1;
            icon_type = 1;
            icon_res = "http://v.icoxedu.cn/Img/jtb.png";
            small_icon = "http://v.icoxedu.cn/Img/jtb.png";
        }
        public string title { get; set; }
        public string content { get; set; }
        public int n_id { get; set; }
        public int icon_type { get; set; }
        public string icon_res { get; set; }
        public string small_icon { get; set; }
    }
}
