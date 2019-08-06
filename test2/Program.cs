using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WeChat;

namespace test2 {
    class Program {
        static void Main(string[] args) {
            VideoEntities ve = DBContextFactory.GetDbContext();
            WeChatUser wcu = ve.WeChatUser.FirstOrDefault(x => x.OpenId == "oFnmZwYL8BjNShvunRE_gR-KKxTM" && x.DeviceId == "1111111");
            if (wcu != null) {
                wcu.HeadImg = string.Format("http://v.icoxtech.com/Img/deviceHeadImg/{0}.jpg", "da79e6cf-6dee-4cec-b123-4e9f756cfb23");
                try {
                    ve.SaveChanges();
                } catch (Exception ex) {
                    
                    throw ex;
                }
            }
            Console.ReadKey();
        }
    }
}
