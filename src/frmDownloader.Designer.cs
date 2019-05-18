namespace PackageStore
{
    partial class frmDownloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloader));
      this.listViewPackage = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // listViewPackage
      // 
      this.listViewPackage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      resources.ApplyResources(this.listViewPackage, "listViewPackage");
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
      // frmDownloader
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listViewPackage);
      this.DoubleBuffered = true;
      this.Name = "frmDownloader";
      this.Load += new System.EventHandler(this.FrmDownload_Load);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewPackage;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}