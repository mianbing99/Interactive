using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web.Common
{
    public class EncryptHelper
    {
        static string _Key = "MVIDEO506ICOXEDU";
        static string _NewKey ="85ML1rtCMcMFvxAO";
        #region Md5加密,生成16位或32位,生成的密文都是大写
        public static string Md5To16(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5To32(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-", "");
        }
        #endregion

        #region AES加密
        /// <summary>
        /// AES加密
        /// </summary>
        public static string Encrypt(string Str)
        {
            byte[] keyArray = UTF8Encoding.ASCII.GetBytes(_Key);
            byte[] toEncryptArray =Encoding.UTF8.GetBytes(Str);
            RijndaelManaged rDel = new RijndaelManaged();//using System.Security.Cryptography;    
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;//using System.Security.Cryptography;    
            rDel.Padding = PaddingMode.PKCS7;//using System.Security.Cryptography;    
            ICryptoTransform cTransform = rDel.CreateEncryptor();//using System.Security.Cryptography;    
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray); 
        }
        public static string NewEncrypt(string Str) {
            byte[] keyArray = UTF8Encoding.ASCII.GetBytes(_NewKey);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(Str);
            RijndaelManaged rDel = new RijndaelManaged();//using System.Security.Cryptography;    
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;//using System.Security.Cryptography;    
            rDel.Padding = PaddingMode.PKCS7;//using System.Security.Cryptography;    
            ICryptoTransform cTransform = rDel.CreateEncryptor();//using System.Security.Cryptography;    
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }
        #endregion AES加密

        //#region AES解密
        ///// <summary>
        ///// AES解密
        ///// </summary>
        ///// <param name="toDecrypt">要解密的内容</param>
        ///// <param name="strKey">密钥（16或者32位）</param>
        ///// <returns>解密后的明文</returns>
        //public static string Decrypt(string Str) {
        //    try {
        //        byte[] keyArray = UTF8Encoding.ASCII.GetBytes(_Key);
        //        byte[] toEncryptArray = Encoding.UTF8.GetBytes(Str);
        //        RijndaelManaged rDel = new RijndaelManaged();
        //        rDel.Key = keyArray;
        //        rDel.Mode = CipherMode.ECB;
        //        rDel.Padding = PaddingMode.PKCS7;
        //        ICryptoTransform cTransform = rDel.CreateDecryptor();
        //        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        //        return Convert.ToBase64String(resultArray);
        //    } catch (Exception ex) {
        //        return ex.Message;
        //    }
        //}
        //#endregion

        public static string AESDecrypt(string toDecrypt) {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(_NewKey);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #region 加密解密函数

        /// <summary>
        /// 字符串的加密
        /// </summary>
        /// <param name="Value">要加密的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>加密后的字符串</returns>
        public string EncryptString(string Value, string sKey, string sIV)
        {
            try
            {
                SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.IV = Convert.FromBase64String(sIV);
                //指定加密的运算模式
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                //获取或设置加密算法的填充模式
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ICryptoTransform ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                byte[] byt = Encoding.UTF8.GetBytes(Value);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ("Error：" + ex.Message);
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">加密后的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>解密后的字符串</returns>
        public string DecryptString(string Value, string sKey, string sIV)
        {
            try
            {
                SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
                //将3DES的密钥转换成byte
                mCSP.Key = Convert.FromBase64String(sKey);
                //将3DES的向量转换成byte
                mCSP.IV = Convert.FromBase64String(sIV);
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ICryptoTransform ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                byte[] byt = Convert.FromBase64String(Value);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "出现异常", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ("Error：" + ex.Message);
            }
        }
        #endregion

    }
}