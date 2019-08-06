using System;
using System.Text;
namespace Common
{
    public class TextStr
    {
        /// <summary> 
        /// 生成随机字符串 
        /// </summary> 
        /// <param name="length">目标字符串的长度</param> 
        /// <param name="useNum">是否包含数字，true=包含，默认为包含</param> 
        /// <param name="useLow">是否包含小写字母，true=包含，默认为包含</param> 
        /// <param name="useUpp">是否包含大写字母，true=包含，默认为包含</param> 
        /// <param name="useSpe">是否包含特殊字符，true=包含，默认为不包含</param> 
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param> 
        /// <returns>指定长度的随机字符串</returns> 
        public static string RandomText(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="Number">随机数个数</param>
        /// <param name="minNum">随机数下限</param>
        /// <param name="maxNum">随机数上限</param>
        /// <returns></returns>
        public static int[] RandomArray(int Number, int minNum, int maxNum)
        {
            int j;
            int[] b = new int[Number];
            Random r = new Random();
            for (j = 0; j < Number; j++)
            {
                int i = r.Next(minNum, maxNum + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (b[k] == i)
                    {
                        num = num + 1;
                    }
                }
                if (num == 0)
                {
                    b[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }
            return b;
        }
        /// <summary>
        /// 截取等宽中英文字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="length">要截取的字符长度</param>
        /// <param name="appendStr">截取后后追加的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutStr(object str, int length, string appendStr)
        {
            string temp = str.ToString();
            if (str == null)
                return string.Empty;

            int len = length * 2;
            int j = 0, k = 0;
            Encoding encoding = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < temp.Length; i++)
            {
                byte[] bytes = encoding.GetBytes(temp.Substring(i, 1));
                if (bytes.Length == 2)//不是英文  
                    j += 2;
                else
                    j++;

                if (j <= len)
                    k += 1;

                if (j >= len)
                    return temp.Substring(0, k) + appendStr;
            }
            return temp;
        }
        /// <summary>
        /// 去掉HTML标签
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string ParseTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
