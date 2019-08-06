using InsetTencentUser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InsetTencentUser {
    public partial class Inset : Form {
        List<string> guidlist = new List<string>();
        public IAsyncResult res = null;
        dbEntities ve = new dbEntities();
        public Inset() {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.dataGridView1.AutoGenerateColumns = false;
            SelectAll();
        }
        private void button1_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult fd = fbd.ShowDialog();
            if (fd == DialogResult.OK) {
                this.textBox1.Text = fbd.SelectedPath;
                this.textBox1.Text = fbd.SelectedPath;
                InitFile();
                //Thread thread = new Thread(InitFile);
                //thread.IsBackground = true;
                //thread.Start();
            }
        }

        private void InitFile() {
            DirectoryInfo aDir = new DirectoryInfo(this.textBox1.Text);
            FileInfo[] fis = aDir.GetFiles("licence.sign.file[*].txt", SearchOption.AllDirectories);
            int num = 0;
            foreach (FileInfo item in fis) {
                num++;
                string GUID = GetValue(item.Name, "[", "]");
                StreamReader sr = item.OpenText();
                string License = sr.ReadToEnd();
                string Validation = "失败";
                sr.Close();
                if (GUID.Length > 0 && License.Length > 0) {
                    Validation = "OK";
                }
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells["Num"].Value = num;
                this.dataGridView1.Rows[index].Cells["GUID"].Value = GUID;
                this.dataGridView1.Rows[index].Cells["License"].Value = License;
                this.dataGridView1.Rows[index].Cells["Validation"].Value = Validation;
                this.dataGridView1.Rows[index].Cells["State"].Value = "";
            }
        }
        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns>
        public static string GetValue(string str, string s, string e) {
            int one = str.IndexOf(s) + 1;
            int end = str.IndexOf(e, one);
            return str.Substring(one, end - one);
        }

        private void button2_Click(object sender, EventArgs e) {
            List<TencentUser> tencentUserlist = new List<TencentUser>();
            foreach (DataGridViewRow item in this.dataGridView1.Rows) {
                string _GUID = item.Cells["GUID"].Value + "";
                string _License = item.Cells["License"].Value + "";
                string GUID = item.Cells["GUID"].Value + "";
                string _Version = version.Text;
                TencentUser tencentUser = new TencentUser() { OpenId = "", GUID = _GUID, License = _License, CreateDate = DateTime.Now, Version = _Version };
                if (guidlist.Contains(tencentUser.GUID)) {
                    tencentUserlist.Add(tencentUser);
                    continue;
                }
                if (item.Cells["State"].Value.ToString() == "OK") {
                    continue;
                }
                
            }
            if (tencentUserlist.Count != 0) {
                if (MessageBox.Show("数据库重复有" + tencentUserlist.Count + "条,是否继续录入没有重复的", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    foreach (DataGridViewRow item in this.dataGridView1.Rows) {
                        string _GUID = item.Cells["GUID"].Value + "";
                        string _License = item.Cells["License"].Value + "";
                        string GUID = item.Cells["GUID"].Value + "";
                        string _Version = version.Text;
                        TencentUser tencentUser = new TencentUser() { OpenId = "", GUID = _GUID, License = _License, CreateDate = DateTime.Now, Version = _Version };
                        if (guidlist.Contains(tencentUser.GUID)) {
                            continue;
                        }
                        if (item.Cells["State"].Value.ToString() == "OK") {
                            continue;
                        }
                        // 执行
                        ve.TencentUser.Add(tencentUser);
                        int num = ve.SaveChanges();
                        if (num > 0) {
                            item.Cells["State"].Value = "OK";
                        } else {
                            item.Cells["State"].Value = "NO";
                        }
                    }
                }
            } else {
                foreach (DataGridViewRow item in this.dataGridView1.Rows) {
                    string _GUID = item.Cells["GUID"].Value + "";
                    string _License = item.Cells["License"].Value + "";
                    string GUID = item.Cells["GUID"].Value + "";
                    string _Version = version.Text;
                    TencentUser tencentUser = new TencentUser() { OpenId = "", GUID = _GUID, License = _License, CreateDate = DateTime.Now, Version = _Version };
                    if (guidlist.Contains(tencentUser.GUID)) {
                        continue;
                    }
                    if (item.Cells["State"].Value.ToString() == "OK") {
                        continue;
                    }
                    // 执行
                    ve.TencentUser.Add(tencentUser);
                    int num = ve.SaveChanges();
                    if (num > 0) {
                        item.Cells["State"].Value = "OK";
                    } else {
                        item.Cells["State"].Value = "NO";
                    }

                }
            }
           
        }
        private void SelectAll() {
            SqlConnection conn = new SqlConnection("server=182.254.134.51;database=Ejiaqin;uid=MDB;pwd=Main@JLF955icox;Pooling=true;");
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            string sql = string.Format("select guid from [dbo].[TencentUser] ");
            cmd.CommandText = sql;
            SqlDataReader re = cmd.ExecuteReader();
            while (re.Read()) {
                guidlist.Add(re["Guid"].ToString());
            }
            if (re != null) {
                re.Close();
                re.Dispose();
            }
            if (conn != null) {
                conn.Close();
                conn.Dispose();
            }

        }
    }
}
