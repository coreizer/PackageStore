/*
 * Copyright (c) 2017-2019 AlphaNyne
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
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PackageStore.Managed
{
  public class Scheduler
  {
    private readonly WebClient client;

    public DownloadStatus Status {
      get;
      private set;
    }

    public ListViewItem Item {
      get;
    }

    public PackageInfo Package {
      get;
    }

    public string Directory {
      get;
      private set;
    }

    public Scheduler(PackageInfo package, string fileName)
    {
      if (string.IsNullOrWhiteSpace(fileName)) {
        throw new ArgumentNullException();
      }

      this.Package = package;
      this.Directory = fileName;

      ListViewItem newItem = new ListViewItem { Text = package.FileName };
      newItem.SubItems.AddRange(new[] {
        Enum.GetName(typeof(DownloadStatus), this.Status),
        "0"
      });
      this.Item = newItem;

      this.client = new WebClient();
      this.client.DownloadFileCompleted += this.DownloadFileCompleted;
      this.client.DownloadProgressChanged += this.DownloadProgressChanged;
    }

    private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
      this.Item.SubItems[1].Text = Enum.GetName(typeof(DownloadStatus), this.Status);
      this.Item.SubItems[2].Text = e.ProgressPercentage + "%";
    }

    private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
      this.Status = (!e.Cancelled && e.Error == null) ? DownloadStatus.Completed : DownloadStatus.Failed;
      this.Item.SubItems[1].Text = Enum.GetName(typeof(DownloadStatus), this.Status);
    }

    public void DownloadAsync()
    {
      this.Status = DownloadStatus.Downloading;

      if (!File.Exists(this.Directory)) {
        this.Directory = Path.Combine(this.Directory, this.Package.FileName);
      }
      this.client.DownloadFileAsync(this.Package.Url, this.Directory);
    }

    public void CancelAsync()
    {
      this.Status = DownloadStatus.Cancelled;
      this.client.CancelAsync();
    }
  }
}
