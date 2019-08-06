using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Admin.Class;
using System.Data;

namespace Web.Admin {
    public partial class TEST : System.Web.UI.Page {
        static public string[] time;
        static public int c;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
            }
        }
        /// <summary>  
        /// 提交数据  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        protected void Button1_Click(object sender, EventArgs e) {
            //创建一个加密key  
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            byte[] ByteKey = System.Text.UTF8Encoding.UTF8.GetBytes(guid.ToCharArray());
            System.Security.Cryptography.AesManaged Aes = new System.Security.Cryptography.AesManaged();
            var encode = Aes.CreateEncryptor(ByteKey, ByteKey.Take(16).ToArray());


            byte[] byteArray = new byte[FileUpload1.PostedFile.InputStream.Length];
            FileUpload1.PostedFile.InputStream.Read(byteArray, 0, byteArray.Length);


            ///加密  
            var list = encode.TransformFinalBlock(byteArray, 0, byteArray.Length).ToList();

            //写入加密KEY  
            for (int i = 31; i >= 0; i--) {
                //加入到集合  
                list.Insert(0, ByteKey[i]);
            }

            Response.Write(guid);

            //转换成Array  
            byteArray = list.ToArray();



            System.IO.File.WriteAllBytes(Server.MapPath("./temp可删除/") + System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName) + ".Encode", byteArray);


            Response.Write("<br/>" + System.Text.UTF8Encoding.UTF8.GetString(byteArray.Take(32).ToArray()));
        }
        /// <summary>  
    /// 解密数据  
    /// </summary>  
    /// <param name="sender"></param>  
    /// <param name="e"></param>  
        protected void Button2_Click(object sender, EventArgs e) {

            //写新解密文  
            using (System.IO.FileStream writeFile = System.IO.File.Create(Server.MapPath("./temp可删除/") + System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName) + ".Dencode")) {

                //读取加密码key  
                using (System.IO.Stream sr = FileUpload2.PostedFile.InputStream) {

                    System.Security.Cryptography.AesManaged Aes = new System.Security.Cryptography.AesManaged();
                    byte[] ByteKey = new byte[32];
                    sr.Read(ByteKey, 0, ByteKey.Length);
                    //读取KEY  
                    using (System.Security.Cryptography.ICryptoTransform dencode = Aes.CreateDecryptor(ByteKey, ByteKey.Take(16).ToArray())) {
                        //开始解密一次性  
                        //byte[] result = dencode.TransformFinalBlock(reads, 0, len);  
                        //使用DES流解密  
                        using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(writeFile, dencode, System.Security.Cryptography.CryptoStreamMode.Write)) {

                            while (sr.CanRead) {

                                byte[] reads = new byte[2048];
                                //读取的有效长度  
                                int len = sr.Read(reads, 0, reads.Length);
                                if (len == 0) { break; }
                                cs.Write(reads, 0, len);

                            }
                            cs.Close();
                        }

                    }


                    sr.Close();
                }


                writeFile.Close();
            }
        }
    }
}