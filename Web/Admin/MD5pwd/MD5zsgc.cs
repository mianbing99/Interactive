using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Web.Admin.MD5pwd {
    public class MD5zsgc {
        public static string MD5Entry(string s) {
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] ss = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            ss.Reverse();

            for (int i = 3; i < ss.Length - 1; i++) {
                pwd = pwd + (ss[i] < 198 ? ss[i] + 28 : ss[i]).ToString("X");
            }
            return pwd;
        }
    }
}