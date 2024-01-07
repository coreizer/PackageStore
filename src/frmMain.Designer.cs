#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2023 coreizer
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

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
         var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
         this.menuStrip1 = new MenuStrip();
         this.FilesToolStripMenuItem = new ToolStripMenuItem();
         this.SaveAsJsonStripMenuItem = new ToolStripMenuItem();
         this.SaveDirectoryToolStripMenuItem = new ToolStripMenuItem();
         this.helpToolStripMenuItem = new ToolStripMenuItem();
         this.ResetSuggestionToolStripMenuItem = new ToolStripMenuItem();
         this.aboutToolStripMenuItem = new ToolStripMenuItem();
         this.toolStripSeparator1 = new ToolStripSeparator();
         this.githubToolStripMenuItem = new ToolStripMenuItem();
         this.textBoxPackageId = new TextBox();
         this.buttonSearch = new Button();
         this.labelPackageId = new Label();
         this.ListViewPackage = new ListView();
         this.columnHeaderFileName = new ColumnHeader();
         this.columnHeaderSize = new ColumnHeader();
         this.columnHeaderVersion = new ColumnHeader();
         this.columnHeaderSystem = new ColumnHeader();
         this.columnHeaderHash = new ColumnHeader();
         this.contextMenuStrip1 = new ContextMenuStrip(this.components);
         this.DownloadToolStripMenuItem = new ToolStripMenuItem();
         this.OpenXMLToolStripMenuItem = new ToolStripMenuItem();
         this.toolStripSeparator2 = new ToolStripSeparator();
         this.copyToURLToolStripMenuItem = new ToolStripMenuItem();
         this.CopyToSizeToolStripMenuItem = new ToolStripMenuItem();
         this.CopyToVersionToolStripMenuItem = new ToolStripMenuItem();
         this.CopyToSystemVersionToolStripMenuItem = new ToolStripMenuItem();
         this.CopyToHashToolStripMenuItem = new ToolStripMenuItem();
         this.statusStrip1 = new StatusStrip();
         this.toolStripStatusLabelSelected = new ToolStripStatusLabel();
         this.toolStripStatusLabelCopyright = new ToolStripStatusLabel();
         this.checkBoxRedump = new CheckBox();
         this.menuStrip1.SuspendLayout();
         this.contextMenuStrip1.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // menuStrip1
         // 
         resources.ApplyResources(this.menuStrip1, "menuStrip1");
         this.menuStrip1.ImageScalingSize = new Size(24, 24);
         this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.FilesToolStripMenuItem, this.helpToolStripMenuItem });
         this.menuStrip1.Name = "menuStrip1";
         // 
         // FilesToolStripMenuItem
         // 
         resources.ApplyResources(this.FilesToolStripMenuItem, "FilesToolStripMenuItem");
         this.FilesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.SaveAsJsonStripMenuItem, this.SaveDirectoryToolStripMenuItem });
         this.FilesToolStripMenuItem.Name = "FilesToolStripMenuItem";
         // 
         // SaveAsJsonStripMenuItem
         // 
         resources.ApplyResources(this.SaveAsJsonStripMenuItem, "SaveAsJsonStripMenuItem");
         this.SaveAsJsonStripMenuItem.Name = "SaveAsJsonStripMenuItem";
         this.SaveAsJsonStripMenuItem.Click += this.ExportJsonStripMenuItem_Click;
         // 
         // SaveDirectoryToolStripMenuItem
         // 
         resources.ApplyResources(this.SaveDirectoryToolStripMenuItem, "SaveDirectoryToolStripMenuItem");
         this.SaveDirectoryToolStripMenuItem.Name = "SaveDirectoryToolStripMenuItem";
         this.SaveDirectoryToolStripMenuItem.Click += this.SaveDirectoryToolStripMenuItem_Click;
         // 
         // helpToolStripMenuItem
         // 
         resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
         this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.ResetSuggestionToolStripMenuItem, this.aboutToolStripMenuItem, this.toolStripSeparator1, this.githubToolStripMenuItem });
         this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
         // 
         // ResetSuggestionToolStripMenuItem
         // 
         resources.ApplyResources(this.ResetSuggestionToolStripMenuItem, "ResetSuggestionToolStripMenuItem");
         this.ResetSuggestionToolStripMenuItem.Name = "ResetSuggestionToolStripMenuItem";
         this.ResetSuggestionToolStripMenuItem.Click += this.ResetSuggestionToolStripMenuItem_Click;
         // 
         // aboutToolStripMenuItem
         // 
         resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
         this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
         this.aboutToolStripMenuItem.Click += this.AboutToolStripMenuItem_Click;
         // 
         // toolStripSeparator1
         // 
         resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         // 
         // githubToolStripMenuItem
         // 
         resources.ApplyResources(this.githubToolStripMenuItem, "githubToolStripMenuItem");
         this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
         this.githubToolStripMenuItem.Click += this.GitHubToolStripMenuItem_Click;
         // 
         // textBoxPackageId
         // 
         resources.ApplyResources(this.textBoxPackageId, "textBoxPackageId");
         this.textBoxPackageId.AutoCompleteMode = AutoCompleteMode.Suggest;
         this.textBoxPackageId.AutoCompleteSource = AutoCompleteSource.CustomSource;
         this.textBoxPackageId.Name = "textBoxPackageId";
         // 
         // buttonSearch
         // 
         resources.ApplyResources(this.buttonSearch, "buttonSearch");
         this.buttonSearch.Name = "buttonSearch";
         this.buttonSearch.UseVisualStyleBackColor = true;
         this.buttonSearch.Click += this.ButtonSearch_Click;
         // 
         // labelPackageId
         // 
         resources.ApplyResources(this.labelPackageId, "labelPackageId");
         this.labelPackageId.Name = "labelPackageId";
         // 
         // ListViewPackage
         // 
         resources.ApplyResources(this.ListViewPackage, "ListViewPackage");
         this.ListViewPackage.Columns.AddRange(new ColumnHeader[] { this.columnHeaderFileName, this.columnHeaderSize, this.columnHeaderVersion, this.columnHeaderSystem, this.columnHeaderHash });
         this.ListViewPackage.ContextMenuStrip = this.contextMenuStrip1;
         this.ListViewPackage.FullRowSelect = true;
         this.ListViewPackage.GridLines = true;
         this.ListViewPackage.Name = "ListViewPackage";
         this.ListViewPackage.UseCompatibleStateImageBehavior = false;
         this.ListViewPackage.View = View.Details;
         this.ListViewPackage.ItemActivate += this.ListViewPackages_ItemActivate;
         this.ListViewPackage.SelectedIndexChanged += this.ListViewPackage_SelectedIndexChanged;
         this.ListViewPackage.KeyDown += this.ListViewPackage_KeyDown;
         // 
         // columnHeaderFileName
         // 
         this.columnHeaderFileName.Tag = "";
         resources.ApplyResources(this.columnHeaderFileName, "columnHeaderFileName");
         // 
         // columnHeaderSize
         // 
         resources.ApplyResources(this.columnHeaderSize, "columnHeaderSize");
         // 
         // columnHeaderVersion
         // 
         resources.ApplyResources(this.columnHeaderVersion, "columnHeaderVersion");
         // 
         // columnHeaderSystem
         // 
         resources.ApplyResources(this.columnHeaderSystem, "columnHeaderSystem");
         // 
         // columnHeaderHash
         // 
         resources.ApplyResources(this.columnHeaderHash, "columnHeaderHash");
         // 
         // contextMenuStrip1
         // 
         resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
         this.contextMenuStrip1.ImageScalingSize = new Size(24, 24);
         this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.DownloadToolStripMenuItem, this.OpenXMLToolStripMenuItem, this.toolStripSeparator2, this.copyToURLToolStripMenuItem, this.CopyToSizeToolStripMenuItem, this.CopyToVersionToolStripMenuItem, this.CopyToSystemVersionToolStripMenuItem, this.CopyToHashToolStripMenuItem });
         this.contextMenuStrip1.Name = "contextMenuStrip1";
         // 
         // DownloadToolStripMenuItem
         // 
         resources.ApplyResources(this.DownloadToolStripMenuItem, "DownloadToolStripMenuItem");
         this.DownloadToolStripMenuItem.Name = "DownloadToolStripMenuItem";
         this.DownloadToolStripMenuItem.Click += this.DownloadToolStripMenuItem_Click;
         // 
         // OpenXMLToolStripMenuItem
         // 
         resources.ApplyResources(this.OpenXMLToolStripMenuItem, "OpenXMLToolStripMenuItem");
         this.OpenXMLToolStripMenuItem.Name = "OpenXMLToolStripMenuItem";
         this.OpenXMLToolStripMenuItem.Click += this.OpenXMLToolStripMenuItem_Click;
         // 
         // toolStripSeparator2
         // 
         resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
         this.toolStripSeparator2.Name = "toolStripSeparator2";
         // 
         // copyToURLToolStripMenuItem
         // 
         resources.ApplyResources(this.copyToURLToolStripMenuItem, "copyToURLToolStripMenuItem");
         this.copyToURLToolStripMenuItem.Name = "copyToURLToolStripMenuItem";
         this.copyToURLToolStripMenuItem.Tag = "Url";
         this.copyToURLToolStripMenuItem.Click += this.CopyToClipboardToolStripMenuItem_Click;
         // 
         // CopyToSizeToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToSizeToolStripMenuItem, "CopyToSizeToolStripMenuItem");
         this.CopyToSizeToolStripMenuItem.Name = "CopyToSizeToolStripMenuItem";
         this.CopyToSizeToolStripMenuItem.Tag = "Size";
         this.CopyToSizeToolStripMenuItem.Click += this.CopyToClipboardToolStripMenuItem_Click;
         // 
         // CopyToVersionToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToVersionToolStripMenuItem, "CopyToVersionToolStripMenuItem");
         this.CopyToVersionToolStripMenuItem.Name = "CopyToVersionToolStripMenuItem";
         this.CopyToVersionToolStripMenuItem.Tag = "Version";
         this.CopyToVersionToolStripMenuItem.Click += this.CopyToClipboardToolStripMenuItem_Click;
         // 
         // CopyToSystemVersionToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToSystemVersionToolStripMenuItem, "CopyToSystemVersionToolStripMenuItem");
         this.CopyToSystemVersionToolStripMenuItem.Name = "CopyToSystemVersionToolStripMenuItem";
         this.CopyToSystemVersionToolStripMenuItem.Tag = "SP_SYS";
         this.CopyToSystemVersionToolStripMenuItem.Click += this.CopyToClipboardToolStripMenuItem_Click;
         // 
         // CopyToHashToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToHashToolStripMenuItem, "CopyToHashToolStripMenuItem");
         this.CopyToHashToolStripMenuItem.Name = "CopyToHashToolStripMenuItem";
         this.CopyToHashToolStripMenuItem.Tag = "Hash";
         this.CopyToHashToolStripMenuItem.Click += this.CopyToClipboardToolStripMenuItem_Click;
         // 
         // statusStrip1
         // 
         resources.ApplyResources(this.statusStrip1, "statusStrip1");
         this.statusStrip1.ImageScalingSize = new Size(24, 24);
         this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabelSelected, this.toolStripStatusLabelCopyright });
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.SizingGrip = false;
         // 
         // toolStripStatusLabelSelected
         // 
         resources.ApplyResources(this.toolStripStatusLabelSelected, "toolStripStatusLabelSelected");
         this.toolStripStatusLabelSelected.ForeColor = Color.Black;
         this.toolStripStatusLabelSelected.Name = "toolStripStatusLabelSelected";
         // 
         // toolStripStatusLabelCopyright
         // 
         resources.ApplyResources(this.toolStripStatusLabelCopyright, "toolStripStatusLabelCopyright");
         this.toolStripStatusLabelCopyright.Name = "toolStripStatusLabelCopyright";
         this.toolStripStatusLabelCopyright.Spring = true;
         // 
         // checkBoxRedump
         // 
         resources.ApplyResources(this.checkBoxRedump, "checkBoxRedump");
         this.checkBoxRedump.Name = "checkBoxRedump";
         this.checkBoxRedump.UseVisualStyleBackColor = true;
         // 
         // frmMain
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = AutoScaleMode.Font;
         this.Controls.Add(this.checkBoxRedump);
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.labelPackageId);
         this.Controls.Add(this.buttonSearch);
         this.Controls.Add(this.textBoxPackageId);
         this.Controls.Add(this.menuStrip1);
         this.Controls.Add(this.ListViewPackage);
         this.MainMenuStrip = this.menuStrip1;
         this.MaximizeBox = false;
         this.Name = "frmMain";
         this.SizeGripStyle = SizeGripStyle.Hide;
         this.FormClosing += this.frmMain_FormClosing;
         this.Load += this.frmMain_Load;
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
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelPackageId;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderVersion;
        private System.Windows.Forms.ColumnHeader columnHeaderSystem;
        private System.Windows.Forms.ColumnHeader columnHeaderHash;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem copyToURLToolStripMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSelected;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCopyright;
        private System.Windows.Forms.ListView ListViewPackage;
      private System.Windows.Forms.ToolStripMenuItem ResetSuggestionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem FilesToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem SaveAsJsonStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToSizeToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToVersionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToSystemVersionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToHashToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem OpenXMLToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.CheckBox checkBoxRedump;
      private System.Windows.Forms.ToolStripMenuItem DownloadToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem SaveDirectoryToolStripMenuItem;
   }
}

