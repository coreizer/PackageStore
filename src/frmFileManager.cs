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

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using EnumsNET;
using PackageStore.Models;

namespace PackageStore
{
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

    private Properties.Settings Settings {
      get => Properties.Settings.Default;
    }

    internal static HttpClient _http = new HttpClient();

    private readonly ObservableCollection<FileItem> _files = new();

    private class FileItem : ListViewItem
    {
      private readonly CancellationTokenSource _source = new();

      public string FileName {
        get => this.Package.Name;
      }

      public string Path { get; set; }

      public CancellationToken Token => (CancellationToken)(this._source?.Token);

      public DateTime StartTime { get; set; }

      public Package Package { get; }

      public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

      private TimeSpan _estimatedTime = TimeSpan.Zero;
      public TimeSpan EstimatedTime {
        get => this._estimatedTime;
        set {
          if (this.ListView != null && this.Index != -1) {
            this.SubItems[4].Text = $"{value:m\\:ss}(s)";
          }
          this._estimatedTime = value;
        }
      }

      private int _percent = 0;
      public int Percent {
        get => this._percent;
        set {
          if (this.ListView != null && this.Index != -1) {
            this.SubItems[3].Text = $"{value}%";
          }
          this._percent = value;
        }
      }

      private DownloadStatus _status = DownloadStatus.Waiting;
      public DownloadStatus Status {
        get => this._status;
        set {
          if (this.ListView != null && this.Index != -1) {
            this.SubItems[1].Text = value.ToString();

            switch (value) {
              case DownloadStatus.Canceled:
                this.SubItems[1].ForeColor = SystemColors.WindowFrame;
                break;
              case DownloadStatus.Waiting:
              case DownloadStatus.Downloading:
                this.SubItems[1].ForeColor = Color.Black;
                break;
              case DownloadStatus.Completed:
                this.SubItems[1].ForeColor = Color.Green;
                break;
              case DownloadStatus.Failed:
                this.SubItems[1].ForeColor = Color.Red;
                break;
            }
          }
          this._status = value;
        }
      }

      public FileItem(Package package) {
        this.Package = package;
        this.Tag = this;
        this.UseItemStyleForSubItems = false;
      }

      ~FileItem() {
        this._source?.Dispose();
      }

      public void Cancel() {
        if (this._source != null && !this._source.IsCancellationRequested) {
          this._source?.Cancel();
        }
      }

      internal void Reset() {
        if (this.ListView != null && this.Index != -1) {
          this.Percent = this.Percent > 1 ? this.Percent : 0;
          this.ElapsedTime = TimeSpan.Zero;
          this.EstimatedTime = TimeSpan.Zero;
        }
        this._source?.Dispose();
      }
    }

    public frmFileManager() {
      this.InitializeComponent();

      this._files.CollectionChanged += this.OnCollectionChanged;
      this.Text = $"{Environment.Name} - Downloader(ALPHA)";
      this.DoubleBuffered = true;
      var pi = this.listViewPackage.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
      pi?.SetValue(this.listViewPackage, true, null);
    }

    private void frmFileManager_Load(object sender, EventArgs e) => this.OnListAddItems();

    private void frmFileManager_FormClosing(object sender, FormClosingEventArgs e) {
      try {
        foreach (var file in this._files) {
          if (file.Status != DownloadStatus.Completed) {
            file.Cancel();
          }
        }
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
      if (e.NewItems is null) return;

      var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
      foreach (FileItem item in e.NewItems) {
        if (item.Status == DownloadStatus.Waiting) {
          Task.Factory.StartNew(() => this.FileDownload(item), item.Token, TaskCreationOptions.None, taskScheduler);
        }
      }
    }

    public void Add(Package package) {
      var filePath = Path.Combine(this.Settings.SaveFolderPath, package.Name);
      if (File.Exists(filePath)) {
        var result = TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Heading = package.Name,
          Text = $"File already exists. Do you want to overwrite it?",
          Buttons = {
                  TaskDialogButton.Yes,
                  TaskDialogButton.No,
               },
          Icon = TaskDialogIcon.Warning,
        });
        if (result != TaskDialogButton.Yes) return;

        try {
          File.Delete(filePath);
        }
        catch (Exception ex) {
          TaskDialog.ShowDialog(this, new TaskDialogPage() {
            Icon = TaskDialogIcon.Error,
            Text = ex.Message,
            Caption = Environment.Name,
            Heading = "Error",
            Buttons = {
                     TaskDialogButton.OK
                  }
          });
        }
      }

      var item = new FileItem(package) { Text = package.Name };
      item.SubItems.AddRange(new[] {
            DownloadStatus.Waiting.ToString(), // 状態
            package.Size.ToString(), // サイズ
            "0%", // パーセント
            "0:00(s)" // 予定時間
           }
      );

      this._files.Add(item);
      this.OnListAddItems();
      this.UpdateQueue();
    }

    private void OnListAddItems() {
      foreach (ListViewItem item in this._files) {
        if (!this.listViewPackage.Items.Contains(item)) {
          this.listViewPackage.Items.Add(item);
        }
      }
    }

    private void DownloadCancelToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        if (this.listViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
        ((FileItem)this.listViewPackage.SelectedItems[0].Tag).Cancel();
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private async Task FileDownload(FileItem file) {
      file.StartTime = DateTime.Now;
      file.Status = DownloadStatus.Waiting;

      try {
        using var request = new HttpRequestMessage(HttpMethod.Get, file.Package.Url);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (response.StatusCode == HttpStatusCode.OK) {
          using var fileStream = File.Create(Path.Combine(Properties.Settings.Default.SaveFolderPath, file.Package.Name));
          var writer = new BinaryWriter(fileStream);
          var contentLength = response.Content.Headers.ContentLength;
          var buffer = new byte[1024 * 1024];
          double totalRead = 0;

          using (var readStream = await response.Content.ReadAsStreamAsync()) {
            file.Status = DownloadStatus.Downloading;

            do {
              file.Token.ThrowIfCancellationRequested();
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
      catch (OperationCanceledException) {
        file.Status = DownloadStatus.Canceled;
      }
      catch (Exception ex) {
        file.Status = DownloadStatus.Failed;
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
      finally {
        file.Reset();
        this.UpdateQueue();
      }
    }

    private void UpdateQueue() {
      try {
        this.toolStripStatusLabelDownloadQueue.Text = $"Download Queue(s) : {this._files.Count(x => (DownloadStatus.Waiting | DownloadStatus.Downloading).HasAllFlags(x.Status))}";
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void SaveDirectoryToolStripMenuItem_Click(object sender, EventArgs e) => Common.SelectDirectoryPath(true);

    private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        if (this.listViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
        var filePath = Path.Combine(
           this.Settings.SaveFolderPath,
           ((FileItem)this.listViewPackage.SelectedItems[0].Tag).FileName
        );
        Process.Start("explorer.exe", $"/select, \"{filePath}\"");
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }
  }
}