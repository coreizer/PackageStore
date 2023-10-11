﻿#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2022 coreizer
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.SaveAsJsonStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.resetSuggestionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.textBoxPackageId = new System.Windows.Forms.TextBox();
         this.buttonSearch = new System.Windows.Forms.Button();
         this.labelPackageId = new System.Windows.Forms.Label();
         this.listViewPackage = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.OpenXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         this.copyToURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.CopyToSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.CopyToVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.CopyToSystemVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.CopyToHashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
         this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
         this.checkBoxRedump = new System.Windows.Forms.CheckBox();
         this.menuStrip1.SuspendLayout();
         this.contextMenuStrip1.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // menuStrip1
         // 
         resources.ApplyResources(this.menuStrip1, "menuStrip1");
         this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.helpToolStripMenuItem});
         this.menuStrip1.Name = "menuStrip1";
         // 
         // ファイルToolStripMenuItem
         // 
         resources.ApplyResources(this.ファイルToolStripMenuItem, "ファイルToolStripMenuItem");
         this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsJsonStripMenuItem});
         this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
         // 
         // SaveAsJsonStripMenuItem
         // 
         resources.ApplyResources(this.SaveAsJsonStripMenuItem, "SaveAsJsonStripMenuItem");
         this.SaveAsJsonStripMenuItem.Name = "SaveAsJsonStripMenuItem";
         this.SaveAsJsonStripMenuItem.Click += new System.EventHandler(this.SaveAsJSONStripMenuItem_Click);
         // 
         // helpToolStripMenuItem
         // 
         resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
         this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetSuggestionToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.githubToolStripMenuItem});
         this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
         // 
         // resetSuggestionToolStripMenuItem
         // 
         resources.ApplyResources(this.resetSuggestionToolStripMenuItem, "resetSuggestionToolStripMenuItem");
         this.resetSuggestionToolStripMenuItem.Name = "resetSuggestionToolStripMenuItem";
         this.resetSuggestionToolStripMenuItem.Click += new System.EventHandler(this.resetSuggestionToolStripMenuItem_Click);
         // 
         // aboutToolStripMenuItem
         // 
         resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
         this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
         this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
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
         this.githubToolStripMenuItem.Click += new System.EventHandler(this.GithubToolStripMenuItem_Click);
         // 
         // textBoxPackageId
         // 
         resources.ApplyResources(this.textBoxPackageId, "textBoxPackageId");
         this.textBoxPackageId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.textBoxPackageId.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
         this.textBoxPackageId.Name = "textBoxPackageId";
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
            this.columnHeader5});
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
         // contextMenuStrip1
         // 
         resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
         this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenXMLToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyToURLToolStripMenuItem,
            this.CopyToSizeToolStripMenuItem,
            this.CopyToVersionToolStripMenuItem,
            this.CopyToSystemVersionToolStripMenuItem,
            this.CopyToHashToolStripMenuItem});
         this.contextMenuStrip1.Name = "contextMenuStrip1";
         // 
         // OpenXMLToolStripMenuItem
         // 
         resources.ApplyResources(this.OpenXMLToolStripMenuItem, "OpenXMLToolStripMenuItem");
         this.OpenXMLToolStripMenuItem.Name = "OpenXMLToolStripMenuItem";
         this.OpenXMLToolStripMenuItem.Click += new System.EventHandler(this.OpenXMLToolStripMenuItem_Click);
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
         this.copyToURLToolStripMenuItem.Click += new System.EventHandler(this.CopyToURLToolStripMenuItem_Click);
         // 
         // CopyToSizeToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToSizeToolStripMenuItem, "CopyToSizeToolStripMenuItem");
         this.CopyToSizeToolStripMenuItem.Name = "CopyToSizeToolStripMenuItem";
         this.CopyToSizeToolStripMenuItem.Click += new System.EventHandler(this.CopyToSizeToolStripMenuItem_Click);
         // 
         // CopyToVersionToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToVersionToolStripMenuItem, "CopyToVersionToolStripMenuItem");
         this.CopyToVersionToolStripMenuItem.Name = "CopyToVersionToolStripMenuItem";
         this.CopyToVersionToolStripMenuItem.Click += new System.EventHandler(this.CopyToVersionToolStripMenuItem_Click);
         // 
         // CopyToSystemVersionToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToSystemVersionToolStripMenuItem, "CopyToSystemVersionToolStripMenuItem");
         this.CopyToSystemVersionToolStripMenuItem.Name = "CopyToSystemVersionToolStripMenuItem";
         this.CopyToSystemVersionToolStripMenuItem.Click += new System.EventHandler(this.CopyToSystemVersionToolStripMenuItem_Click);
         // 
         // CopyToHashToolStripMenuItem
         // 
         resources.ApplyResources(this.CopyToHashToolStripMenuItem, "CopyToHashToolStripMenuItem");
         this.CopyToHashToolStripMenuItem.Name = "CopyToHashToolStripMenuItem";
         this.CopyToHashToolStripMenuItem.Click += new System.EventHandler(this.CopyToHashToolStripMenuItem_Click);
         // 
         // statusStrip1
         // 
         resources.ApplyResources(this.statusStrip1, "statusStrip1");
         this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.SizingGrip = false;
         // 
         // toolStripStatusLabel1
         // 
         resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
         this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
         // 
         // toolStripStatusLabel2
         // 
         resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
         this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
         this.toolStripStatusLabel2.Spring = true;
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
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.checkBoxRedump);
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.labelPackageId);
         this.Controls.Add(this.buttonSearch);
         this.Controls.Add(this.textBoxPackageId);
         this.Controls.Add(this.menuStrip1);
         this.Controls.Add(this.listViewPackage);
         this.MainMenuStrip = this.menuStrip1;
         this.MaximizeBox = false;
         this.Name = "frmMain";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
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
        private System.Windows.Forms.ListView listViewPackage;
      private System.Windows.Forms.ToolStripMenuItem resetSuggestionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem SaveAsJsonStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToSizeToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToVersionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToSystemVersionToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem CopyToHashToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem OpenXMLToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.CheckBox checkBoxRedump;
   }
}

