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
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Net;
   using System.Net.Http;
   using System.Reflection;
   using System.Threading;
   using System.Threading.Tasks;
   using System.Windows.Forms;
   using EnumsNET;
   using PackageStore.Models;

   public partial class frmFileManager : Form
   {
      [Flags]
      public enum DownloadStatus
      {
         Waiting = 0,
         Downloading = 1,
         Completed = 2,
         Canceled = 3,
         Failed = 4
      }

      internal static HttpClient _http = new HttpClient();

      private readonly List<FileItem> _files = new List<FileItem>();

      private class FileItem : ListViewItem
      {
         private readonly CancellationTokenSource _source = new CancellationTokenSource();

         public string Path { get; set; }

         public CancellationToken CancellationToken => (CancellationToken)(this._source?.Token);

         public DateTime StartTime { get; set; }

         public Package Package { get; }

         public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

         private TimeSpan _estimatedTime = TimeSpan.Zero;
         public TimeSpan EstimatedTime
         {
            get => this._estimatedTime;
            set {
               if (this.ListView != null && this.Index != -1) {
                  this.SubItems[4].Text = $"{(value == TimeSpan.Zero ? "0" : value.ToString("m\\:ss"))}秒";
               }
               this._estimatedTime = value;
            }
         }

         private int _percent = 0;
         public int Percent
         {
            get => this._percent;
            set {
               if (this.ListView != null && this.Index != -1) {
                  this.SubItems[3].Text = $"{value}%";
               }
               this._percent = value;
            }
         }

         private DownloadStatus _status = DownloadStatus.Waiting;
         public DownloadStatus Status
         {
            get => this._status;
            set {
               if (this.ListView != null && this.Index != -1) {
                  this.SubItems[1].Text = value.ToString();

                  switch (value) {
                     case DownloadStatus.Canceled:
                        this.SubItems[1].ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                     case DownloadStatus.Waiting:
                     case DownloadStatus.Downloading:
                        this.SubItems[1].ForeColor = System.Drawing.Color.Black;
                        break;
                     case DownloadStatus.Completed:
                        this.SubItems[1].ForeColor = System.Drawing.Color.Green;
                        break;
                     case DownloadStatus.Failed:
                        this.SubItems[1].ForeColor = System.Drawing.Color.Red;
                        break;
                  }
               }
               this._status = value;
            }
         }

         public FileItem(Package package)
         {
            this.Package = package;
            this.Tag = this;
            this.UseItemStyleForSubItems = false;
         }

         ~FileItem()
         {
            this._source?.Dispose();
         }

         public void Cancel()
         {
            if (this._source != null && !this._source.IsCancellationRequested) {
               this._source?.Cancel();
            }
         }

         internal void Reset()
         {
            if (this.ListView != null && this.Index != -1) {
               this.Percent = 0;
               this.ElapsedTime = TimeSpan.Zero;
               this.EstimatedTime = TimeSpan.Zero;
            }
            this._source?.Dispose();
         }
      }

      public frmFileManager()
      {
         this.InitializeComponent();

         this.Text = $"{Environment.Name} - Downloader";
         this.DoubleBuffered = true;
         var pi = this.listViewPackage.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
         pi.SetValue(this.listViewPackage, true, null);
      }

      private void SelectFolder()
      {
         if (string.IsNullOrEmpty(Properties.Settings.Default.DirectoryPath)) {
            using (var FBD = new FolderBrowserDialog()) {
               if (FBD.ShowDialog() != DialogResult.OK) {
                  this.Close();
                  return;
               }
               Properties.Settings.Default.DirectoryPath = FBD.SelectedPath;
            }
         }
      }

      public void AddRange(IList<Package> packages)
      {
         this.SelectFolder();
         foreach (var package in packages)
            this.Add(package);
      }

      public void Add(Package package)
      {
         var path = Path.Combine(Properties.Settings.Default.DirectoryPath, package.Name);
         if (File.Exists(path)) {
            var result = MessageBox.Show($"'{package.Name}' already exists. Do you want to overwrite it?", Environment.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try {
               File.Delete(path);
            }
            catch (Exception ex) {
               MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
         }

         var item = new FileItem(package) { Text = package.Name };
         item.SubItems.AddRange(new[] {
            DownloadStatus.Waiting.ToString(), // 状態
            package.Size.ToString(), // サイズ
            "0%", // パーセント
            "0秒" // 予定時間
         });
         this.listViewPackage.Items.Add(item);
         this._files.Add(item);
         this.DownloadState();
      }

      private void キャンセルToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try {
            if (this.listViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
            ((FileItem)this.listViewPackage.SelectedItems[0].Tag).Cancel();
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void Start()
      {
         var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
         this._files.FindAll(x => x.Status == DownloadStatus.Waiting).ForEach(file => Task.Factory.StartNew(() => this.FileDownload(file), file.CancellationToken, TaskCreationOptions.None, taskScheduler));
      }

      private async Task FileDownload(FileItem file)
      {
         file.StartTime = DateTime.Now;
         file.Status = DownloadStatus.Waiting;

         try {
            using (var request = new HttpRequestMessage(HttpMethod.Get, file.Package.Url))
            using (var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
               if (response.StatusCode == HttpStatusCode.OK) {
                  using (var fileStream = File.Create(Path.Combine(Properties.Settings.Default.DirectoryPath, file.Package.Name))) {
                     var writer = new BinaryWriter(fileStream);
                     var contentLength = response.Content.Headers.ContentLength;
                     var buffer = new byte[1024 * 1024];
                     double totalRead = 0;

                     using (var readStream = await response.Content.ReadAsStreamAsync()) {
                        file.Status = DownloadStatus.Downloading;

                        do {
                           file.CancellationToken.ThrowIfCancellationRequested();
                           if (await readStream.ReadAsync(buffer, 0, buffer.Length) is var read && read == 0) break;
                           totalRead += read;
                           writer.Write(buffer, 0, read);

                           file.Percent = (int)(totalRead * 100 / contentLength);
                           file.ElapsedTime = DateTime.Now - file.StartTime;
                           file.EstimatedTime = TimeSpan.FromSeconds((((double)contentLength - totalRead) / (totalRead / file.ElapsedTime.TotalSeconds)));
                        }
                        while (true);
                     }
                     file.Status = DownloadStatus.Completed;
                  }
               }
            }
         }
         catch (OperationCanceledException) {
            file.Status = DownloadStatus.Canceled;
         }
         catch (Exception ex) {
            file.Status = DownloadStatus.Failed;
            MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally {
            file.Reset();
            this.DownloadState();
         }
      }

      private void frmFileManager_FormClosing(object sender, FormClosingEventArgs e)
      {
         try {
            foreach (var file in this._files) {
               if (file.Status != DownloadStatus.Completed) {
                  file.Cancel();
               }
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void DownloadState()
      {
         try {
            this.toolStripStatusLabelDownload.Text = $"Downloading: {this._files.Count(x => (DownloadStatus.Waiting | DownloadStatus.Downloading).HasAllFlags(x.Status))}";
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Environment.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}