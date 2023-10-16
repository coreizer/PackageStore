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
   partial class frmFileManager
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileManager));
         this.listViewPackage = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.contextMenuStripFile = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.キャンセルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.toolStripStatusLabelDownloadQueue = new System.Windows.Forms.ToolStripStatusLabel();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.SaveFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.contextMenuStripFile.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
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
         this.listViewPackage.ContextMenuStrip = this.contextMenuStripFile;
         this.listViewPackage.FullRowSelect = true;
         this.listViewPackage.GridLines = true;
         this.listViewPackage.HideSelection = false;
         this.listViewPackage.Name = "listViewPackage";
         this.listViewPackage.UseCompatibleStateImageBehavior = false;
         this.listViewPackage.View = System.Windows.Forms.View.Details;
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
         // contextMenuStripFile
         // 
         this.contextMenuStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開くToolStripMenuItem,
            this.キャンセルToolStripMenuItem});
         this.contextMenuStripFile.Name = "contextMenuStripFile";
         resources.ApplyResources(this.contextMenuStripFile, "contextMenuStripFile");
         // 
         // 開くToolStripMenuItem
         // 
         this.開くToolStripMenuItem.Name = "開くToolStripMenuItem";
         resources.ApplyResources(this.開くToolStripMenuItem, "開くToolStripMenuItem");
         // 
         // キャンセルToolStripMenuItem
         // 
         this.キャンセルToolStripMenuItem.Name = "キャンセルToolStripMenuItem";
         resources.ApplyResources(this.キャンセルToolStripMenuItem, "キャンセルToolStripMenuItem");
         this.キャンセルToolStripMenuItem.Click += new System.EventHandler(this.CancelToolStripMenuItem_Click);
         // 
         // statusStrip1
         // 
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDownloadQueue});
         resources.ApplyResources(this.statusStrip1, "statusStrip1");
         this.statusStrip1.Name = "statusStrip1";
         // 
         // toolStripStatusLabelDownloadQueue
         // 
         this.toolStripStatusLabelDownloadQueue.ActiveLinkColor = System.Drawing.Color.Transparent;
         this.toolStripStatusLabelDownloadQueue.ForeColor = System.Drawing.Color.Black;
         this.toolStripStatusLabelDownloadQueue.Name = "toolStripStatusLabelDownloadQueue";
         resources.ApplyResources(this.toolStripStatusLabelDownloadQueue, "toolStripStatusLabelDownloadQueue");
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem});
         resources.ApplyResources(this.menuStrip1, "menuStrip1");
         this.menuStrip1.Name = "menuStrip1";
         // 
         // ファイルToolStripMenuItem
         // 
         this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveFolderToolStripMenuItem});
         this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
         resources.ApplyResources(this.ファイルToolStripMenuItem, "ファイルToolStripMenuItem");
         // 
         // SaveFolderToolStripMenuItem
         // 
         this.SaveFolderToolStripMenuItem.Name = "SaveFolderToolStripMenuItem";
         resources.ApplyResources(this.SaveFolderToolStripMenuItem, "SaveFolderToolStripMenuItem");
         this.SaveFolderToolStripMenuItem.Click += new System.EventHandler(this.SaveFolderToolStripMenuItem_Click);
         // 
         // frmFileManager
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.menuStrip1);
         this.Controls.Add(this.listViewPackage);
         this.MainMenuStrip = this.menuStrip1;
         this.MaximizeBox = false;
         this.Name = "frmFileManager";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFileManager_FormClosing);
         this.Load += new System.EventHandler(this.frmFileManager_Load);
         this.contextMenuStripFile.ResumeLayout(false);
         this.statusStrip1.ResumeLayout(false);
         this.statusStrip1.PerformLayout();
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ListView listViewPackage;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ColumnHeader columnHeader3;
      private System.Windows.Forms.ColumnHeader columnHeader4;
      private System.Windows.Forms.ColumnHeader columnHeader5;
      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDownloadQueue;
      private System.Windows.Forms.ContextMenuStrip contextMenuStripFile;
      private System.Windows.Forms.ToolStripMenuItem キャンセルToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem 開くToolStripMenuItem;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem SaveFolderToolStripMenuItem;
   }
}