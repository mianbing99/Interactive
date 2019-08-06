namespace AdminManager
{
    partial class Home
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.资源管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.资源列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑资源ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分类管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.资源下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.播放验证ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.资源管理ToolStripMenuItem,
            this.分类管理ToolStripMenuItem,
            this.资源下载ToolStripMenuItem,
            this.播放验证ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(667, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 资源管理ToolStripMenuItem
            // 
            this.资源管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.资源列表ToolStripMenuItem,
            this.编辑资源ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.资源管理ToolStripMenuItem.Name = "资源管理ToolStripMenuItem";
            this.资源管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.资源管理ToolStripMenuItem.Text = "资源管理";
            // 
            // 资源列表ToolStripMenuItem
            // 
            this.资源列表ToolStripMenuItem.Name = "资源列表ToolStripMenuItem";
            this.资源列表ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.资源列表ToolStripMenuItem.Text = "新增";
            // 
            // 编辑资源ToolStripMenuItem
            // 
            this.编辑资源ToolStripMenuItem.Name = "编辑资源ToolStripMenuItem";
            this.编辑资源ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.编辑资源ToolStripMenuItem.Text = "编辑";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 分类管理ToolStripMenuItem
            // 
            this.分类管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.删除ToolStripMenuItem1});
            this.分类管理ToolStripMenuItem.Name = "分类管理ToolStripMenuItem";
            this.分类管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.分类管理ToolStripMenuItem.Text = "分类管理";
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            // 
            // 资源下载ToolStripMenuItem
            // 
            this.资源下载ToolStripMenuItem.Name = "资源下载ToolStripMenuItem";
            this.资源下载ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.资源下载ToolStripMenuItem.Text = "资源下载";
            this.资源下载ToolStripMenuItem.Click += new System.EventHandler(this.资源下载ToolStripMenuItem_Click);
            // 
            // 播放验证ToolStripMenuItem
            // 
            this.播放验证ToolStripMenuItem.Name = "播放验证ToolStripMenuItem";
            this.播放验证ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.播放验证ToolStripMenuItem.Text = "播放验证";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 391);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "后台管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 资源管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 资源列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑资源ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 资源下载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 播放验证ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分类管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
    }
}

