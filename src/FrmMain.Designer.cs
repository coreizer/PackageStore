namespace PackageStore
{
    partial class FrmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.downloadManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxPackageId = new System.Windows.Forms.TextBox();
      this.checkBoxForce = new System.Windows.Forms.CheckBox();
      this.buttonSearch = new System.Windows.Forms.Button();
      this.labelPackageId = new System.Windows.Forms.Label();
      this.listViewPackages = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(705, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadManagerToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
      this.toolsToolStripMenuItem.Text = "Tools";
      // 
      // downloadManagerToolStripMenuItem
      // 
      this.downloadManagerToolStripMenuItem.Name = "downloadManagerToolStripMenuItem";
      this.downloadManagerToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
      this.downloadManagerToolStripMenuItem.Text = "Download Manager";
      this.downloadManagerToolStripMenuItem.Click += new System.EventHandler(this.DownloadManagerToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.githubToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
      // 
      // githubToolStripMenuItem
      // 
      this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
      this.githubToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.githubToolStripMenuItem.Text = "Github";
      this.githubToolStripMenuItem.Click += new System.EventHandler(this.GithubToolStripMenuItem_Click);
      // 
      // textBoxPackageId
      // 
      this.textBoxPackageId.Location = new System.Drawing.Point(87, 41);
      this.textBoxPackageId.Name = "textBoxPackageId";
      this.textBoxPackageId.Size = new System.Drawing.Size(216, 19);
      this.textBoxPackageId.TabIndex = 1;
      // 
      // checkBoxForce
      // 
      this.checkBoxForce.AutoSize = true;
      this.checkBoxForce.Location = new System.Drawing.Point(309, 44);
      this.checkBoxForce.Name = "checkBoxForce";
      this.checkBoxForce.Size = new System.Drawing.Size(53, 16);
      this.checkBoxForce.TabIndex = 2;
      this.checkBoxForce.Text = "Force";
      this.checkBoxForce.UseVisualStyleBackColor = true;
      // 
      // buttonSearch
      // 
      this.buttonSearch.Location = new System.Drawing.Point(574, 38);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(119, 25);
      this.buttonSearch.TabIndex = 3;
      this.buttonSearch.Text = "Search";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
      // 
      // labelPackageId
      // 
      this.labelPackageId.AutoSize = true;
      this.labelPackageId.Location = new System.Drawing.Point(10, 45);
      this.labelPackageId.Name = "labelPackageId";
      this.labelPackageId.Size = new System.Drawing.Size(71, 12);
      this.labelPackageId.TabIndex = 4;
      this.labelPackageId.Text = "Package Id : ";
      // 
      // listViewPackages
      // 
      this.listViewPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
      this.listViewPackages.FullRowSelect = true;
      this.listViewPackages.GridLines = true;
      this.listViewPackages.Location = new System.Drawing.Point(12, 69);
      this.listViewPackages.Name = "listViewPackages";
      this.listViewPackages.Size = new System.Drawing.Size(681, 310);
      this.listViewPackages.TabIndex = 5;
      this.listViewPackages.UseCompatibleStateImageBehavior = false;
      this.listViewPackages.View = System.Windows.Forms.View.Details;
      this.listViewPackages.ItemActivate += new System.EventHandler(this.ListViewPackages_ItemActivate);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "FileName";
      this.columnHeader1.Width = 185;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Size";
      this.columnHeader2.Width = 130;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Version";
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Support Version";
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Hash";
      this.columnHeader5.Width = 242;
      // 
      // FrmMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(705, 391);
      this.Controls.Add(this.listViewPackages);
      this.Controls.Add(this.labelPackageId);
      this.Controls.Add(this.buttonSearch);
      this.Controls.Add(this.checkBoxForce);
      this.Controls.Add(this.textBoxPackageId);
      this.Controls.Add(this.menuStrip1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.Name = "FrmMain";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Package Store";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxPackageId;
        private System.Windows.Forms.CheckBox checkBoxForce;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelPackageId;
        private System.Windows.Forms.ListView listViewPackages;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

