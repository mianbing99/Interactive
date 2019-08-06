using AdminManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminManager {
    public partial class DownFile : Form {
        public DownFile() {
            InitializeComponent();
        }
        dbEntities db = new dbEntities();
        private void DownFile_Load(object sender, EventArgs e) {
            InitComboBox();
            try {
                TreeNode tn = new TreeNode();
                tn.Tag = 0;
                tn.Text = "家庭宝资源分类";
                BinTreeView(tn, 0);
                this.tvVideoType.Nodes.Add(tn);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void BinTreeView(TreeNode p_tn, int lev) {
            lev++;
            if (lev < 2) {
                p_tn.Nodes.Clear();
                int id = (int)p_tn.Tag;
                List<VideoType> list = db.VideoType.Where(q => q.Tid == id).ToList();
                foreach (VideoType item in list) {
                    TreeNode tn = new TreeNode();
                    tn.Tag = item.Id;
                    tn.Text = item.Title;
                    BinTreeView(tn, lev);
                    p_tn.Nodes.Add(tn);
                }
            }
        }

        public void InitComboBox() {
            //ComboBox
            tscbState.Items.Add("全部");
            tscbState.Items.Add("显示");
            tscbState.Items.Add("隐藏");
            tscbState.SelectedIndex = 0;
            tscbSource.Items.Add("全部");
            tscbSource.Items.Add("360云盘");
            tscbSource.Items.Add("优酷");
            tscbSource.SelectedIndex = 0;
            tscbPageSize.Items.Add("50");
            tscbPageSize.Items.Add("100");
            tscbPageSize.Items.Add("300");
            tscbPageSize.Items.Add("500");
            tscbPageSize.Items.Add("1000");
            tscbPageSize.Items.Add("3000");
            tscbPageSize.Items.Add("5000");
            tscbPageSize.SelectedIndex = 0;
            tscbPageNum.Items.Add("1/1");
            tscbPageNum.SelectedIndex = 0;
        }
        private void tvVideoType_AfterSelect(object sender, TreeViewEventArgs e) {
            BinTreeView(e.Node, 0);
            int id = Convert.ToInt32(e.Node.Tag);
            bool? IsState;
            int PageSize;
            int PageIndex=1;
            if (tscbState.SelectedText == "全部") {
                IsState = null;
            } else if (tscbState.SelectedText == "显示") {
                IsState = true;
            } else if (tscbState.SelectedText == "隐藏") {
                IsState = false;
            }
            PageSize = Convert.ToInt32( tscbPageSize.SelectedText);
            //db.Video.Where(q => q.Tid == id&&q.State==1).Select();
        }
    }
}
