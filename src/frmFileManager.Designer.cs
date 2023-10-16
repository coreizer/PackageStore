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
         this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderEstimatedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.contextMenuStripFile = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.OpenFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.DownloadCancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.toolStripStatusLabelDownloadQueue = new System.Windows.Forms.ToolStripStatusLabel();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.FilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.SaveDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.contextMenuStripFile.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // listViewPackage
         // 
         resources.ApplyResources(this.listViewPackage, "listViewPackage");
         this.listViewPackage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderStatus,
            this.columnHeaderSize,
            this.columnHeaderPercent,
            this.columnHeaderEstimatedTime});
         this.listViewPackage.ContextMenuStrip = this.contextMenuStripFile;
         this.listViewPackage.FullRowSelect = true;
         this.listViewPackage.GridLines = true;
         this.listViewPackage.HideSelection = false;
         this.listViewPackage.Name = "listViewPackage";
         this.listViewPackage.UseCompatibleStateImageBehavior = false;
         this.listViewPackage.View = System.Windows.Forms.View.Details;
         // 
         // columnHeaderFileName
         // 
         resources.ApplyResources(this.columnHeaderFileName, "columnHeaderFileName");
         // 
         // columnHeaderStatus
         // 
         resources.ApplyResources(this.columnHeaderStatus, "columnHeaderStatus");
         // 
         // columnHeaderSize
         // 
         resources.ApplyResources(this.columnHeaderSize, "columnHeaderSize");
         // 
         // columnHeaderPercent
         // 
         resources.ApplyResources(this.columnHeaderPercent, "columnHeaderPercent");
         // 
         // columnHeaderEstimatedTime
         // 
         resources.ApplyResources(this.columnHeaderEstimatedTime, "columnHeaderEstimatedTime");
         // 
         // contextMenuStripFile
         // 
         resources.ApplyResources(this.contextMenuStripFile, "contextMenuStripFile");
         this.contextMenuStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.DownloadCancelToolStripMenuItem});
         this.contextMenuStripFile.Name = "contextMenuStripFile";
         // 
         // OpenFileToolStripMenuItem
         // 
         resources.ApplyResources(this.OpenFileToolStripMenuItem, "OpenFileToolStripMenuItem");
         this.OpenFileToolStripMenuItem.Name = "OpenFileToolStripMenuItem";
         this.OpenFileToolStripMenuItem.Click += new System.EventHandler(this.OpenFileToolStripMenuItem_Click);
         // 
         // toolStripSeparator1
         // 
         resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         // 
         // DownloadCancelToolStripMenuItem
         // 
         resources.ApplyResources(this.DownloadCancelToolStripMenuItem, "DownloadCancelToolStripMenuItem");
         this.DownloadCancelToolStripMenuItem.Name = "DownloadCancelToolStripMenuItem";
         this.DownloadCancelToolStripMenuItem.Click += new System.EventHandler(this.DownloadCancelToolStripMenuItem_Click);
         // 
         // statusStrip1
         // 
         resources.ApplyResources(this.statusStrip1, "statusStrip1");
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDownloadQueue});
         this.statusStrip1.Name = "statusStrip1";
         // 
         // toolStripStatusLabelDownloadQueue
         // 
         resources.ApplyResources(this.toolStripStatusLabelDownloadQueue, "toolStripStatusLabelDownloadQueue");
         this.toolStripStatusLabelDownloadQueue.ActiveLinkColor = System.Drawing.Color.Transparent;
         this.toolStripStatusLabelDownloadQueue.ForeColor = System.Drawing.Color.Black;
         this.toolStripStatusLabelDownloadQueue.Name = "toolStripStatusLabelDownloadQueue";
         // 
         // menuStrip1
         // 
         resources.ApplyResources(this.menuStrip1, "menuStrip1");
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FilesToolStripMenuItem});
         this.menuStrip1.Name = "menuStrip1";
         // 
         // FilesToolStripMenuItem
         // 
         resources.ApplyResources(this.FilesToolStripMenuItem, "FilesToolStripMenuItem");
         this.FilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveDirectoryToolStripMenuItem});
         this.FilesToolStripMenuItem.Name = "FilesToolStripMenuItem";
         // 
         // SaveDirectoryToolStripMenuItem
         // 
         resources.ApplyResources(this.SaveDirectoryToolStripMenuItem, "SaveDirectoryToolStripMenuItem");
         this.SaveDirectoryToolStripMenuItem.Name = "SaveDirectoryToolStripMenuItem";
         this.SaveDirectoryToolStripMenuItem.Click += new System.EventHandler(this.SaveDirectoryToolStripMenuItem_Click);
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
      private System.Windows.Forms.ColumnHeader columnHeaderFileName;
      private System.Windows.Forms.ColumnHeader columnHeaderStatus;
      private System.Windows.Forms.ColumnHeader columnHeaderSize;
      private System.Windows.Forms.ColumnHeader columnHeaderPercent;
      private System.Windows.Forms.ColumnHeader columnHeaderEstimatedTime;
      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDownloadQueue;
      private System.Windows.Forms.ContextMenuStrip contextMenuStripFile;
      private System.Windows.Forms.ToolStripMenuItem DownloadCancelToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem OpenFileToolStripMenuItem;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem FilesToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem SaveDirectoryToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
   }
}