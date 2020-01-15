namespace PackageStore
{
    partial class frmMain
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxPackageId = new System.Windows.Forms.TextBox();
      this.checkBoxForce = new System.Windows.Forms.CheckBox();
      this.buttonSearch = new System.Windows.Forms.Button();
      this.labelPackageId = new System.Windows.Forms.Label();
      this.listViewPackage = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.copyToURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.comboBoxPlatform = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxEnvironments = new System.Windows.Forms.ComboBox();
      this.menuStrip1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
      resources.ApplyResources(this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.githubToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
      // 
      // githubToolStripMenuItem
      // 
      this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
      resources.ApplyResources(this.githubToolStripMenuItem, "githubToolStripMenuItem");
      this.githubToolStripMenuItem.Click += new System.EventHandler(this.GithubToolStripMenuItem_Click);
      // 
      // textBoxPackageId
      // 
      resources.ApplyResources(this.textBoxPackageId, "textBoxPackageId");
      this.textBoxPackageId.Name = "textBoxPackageId";
      // 
      // checkBoxForce
      // 
      resources.ApplyResources(this.checkBoxForce, "checkBoxForce");
      this.checkBoxForce.Name = "checkBoxForce";
      this.checkBoxForce.UseVisualStyleBackColor = true;
      // 
      // buttonSearch
      // 
      resources.ApplyResources(this.buttonSearch, "buttonSearch");
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
      // 
      // labelPackageId
      // 
      resources.ApplyResources(this.labelPackageId, "labelPackageId");
      this.labelPackageId.Name = "labelPackageId";
      // 
      // listViewPackage
      // 
      resources.ApplyResources(this.listViewPackage, "listViewPackage");
      this.listViewPackage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
      this.listViewPackage.ContextMenuStrip = this.contextMenuStrip1;
      this.listViewPackage.FullRowSelect = true;
      this.listViewPackage.GridLines = true;
      this.listViewPackage.HideSelection = false;
      this.listViewPackage.Name = "listViewPackage";
      this.listViewPackage.UseCompatibleStateImageBehavior = false;
      this.listViewPackage.View = System.Windows.Forms.View.Details;
      this.listViewPackage.ItemActivate += new System.EventHandler(this.ListViewPackages_ItemActivate);
      // 
      // columnHeader1
      // 
      resources.ApplyResources(this.columnHeader1, "columnHeader1");
      // 
      // columnHeader2
      // 
      resources.ApplyResources(this.columnHeader2, "columnHeader2");
      // 
      // columnHeader3
      // 
      resources.ApplyResources(this.columnHeader3, "columnHeader3");
      // 
      // columnHeader4
      // 
      resources.ApplyResources(this.columnHeader4, "columnHeader4");
      // 
      // columnHeader5
      // 
      resources.ApplyResources(this.columnHeader5, "columnHeader5");
      // 
      // columnHeader6
      // 
      resources.ApplyResources(this.columnHeader6, "columnHeader6");
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToURLToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
      // 
      // copyToURLToolStripMenuItem
      // 
      this.copyToURLToolStripMenuItem.Name = "copyToURLToolStripMenuItem";
      resources.ApplyResources(this.copyToURLToolStripMenuItem, "copyToURLToolStripMenuItem");
      this.copyToURLToolStripMenuItem.Click += new System.EventHandler(this.CopyToURLToolStripMenuItem_Click);
      // 
      // statusStrip1
      // 
      this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
      resources.ApplyResources(this.statusStrip1, "statusStrip1");
      this.statusStrip1.Name = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
      this.toolStripStatusLabel2.Spring = true;
      // 
      // comboBoxPlatform
      // 
      this.comboBoxPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPlatform.FormattingEnabled = true;
      resources.ApplyResources(this.comboBoxPlatform, "comboBoxPlatform");
      this.comboBoxPlatform.Name = "comboBoxPlatform";
      this.comboBoxPlatform.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlatform_SelectedIndexChanged);
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // comboBoxEnvironments
      // 
      this.comboBoxEnvironments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEnvironments.FormattingEnabled = true;
      this.comboBoxEnvironments.Items.AddRange(new object[] {
            resources.GetString("comboBoxEnvironments.Items"),
            resources.GetString("comboBoxEnvironments.Items1"),
            resources.GetString("comboBoxEnvironments.Items2"),
            resources.GetString("comboBoxEnvironments.Items3"),
            resources.GetString("comboBoxEnvironments.Items4"),
            resources.GetString("comboBoxEnvironments.Items5"),
            resources.GetString("comboBoxEnvironments.Items6")});
      resources.ApplyResources(this.comboBoxEnvironments, "comboBoxEnvironments");
      this.comboBoxEnvironments.Name = "comboBoxEnvironments";
      // 
      // frmMain
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboBoxEnvironments);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.comboBoxPlatform);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.labelPackageId);
      this.Controls.Add(this.buttonSearch);
      this.Controls.Add(this.checkBoxForce);
      this.Controls.Add(this.textBoxPackageId);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.listViewPackage);
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.Name = "frmMain";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.contextMenuStrip1.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxPackageId;
        private System.Windows.Forms.CheckBox checkBoxForce;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelPackageId;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem copyToURLToolStripMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ComboBox comboBoxPlatform;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxEnvironments;
        private System.Windows.Forms.ListView listViewPackage;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}

