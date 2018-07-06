/*
 * Copyright (c) 2017-2018 AlphaNyne
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
        #region フィールド

        private string _fileName;

        private WebClient _httpClient;
        private ListViewItem _viewItem;
        private PackageInfo _packageInfo;

        private DownloadStatus _status;

        #endregion

        public DownloadStatus Status {
            get {
                return this._status;
            }
        }

        public ListViewItem Item {
            get {
                return this._viewItem;
            }
        }

        public PackageInfo Package {
            get {
                return this._packageInfo;
            }
        }

        public string Directory {
            get {
                return this._fileName;
            }
        }

        public Scheduler(PackageInfo packageInfo, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) {
                throw new ArgumentNullException();
            }

            this._packageInfo = packageInfo;
            this._fileName = fileName;

            var newItem = new ListViewItem { Text = packageInfo.FileName };
            newItem.SubItems.AddRange(new[] {
                Enum.GetName(typeof(DownloadStatus), this._status),
                "0"
            });
            this._viewItem = newItem;

            this._httpClient = new WebClient();
            this._httpClient.DownloadFileCompleted += this.DownloadFileCompleted;
            this._httpClient.DownloadProgressChanged += this.DownloadProgressChanged;
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this._viewItem.SubItems[1].Text = Enum.GetName(typeof(DownloadStatus), this._status);
            this._viewItem.SubItems[2].Text = e.ProgressPercentage + "%";
        }

        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this._status = (!e.Cancelled && e.Error == null) ? DownloadStatus.Completed : DownloadStatus.Failed;
            this._viewItem.SubItems[1].Text = Enum.GetName(typeof(DownloadStatus), this._status);
        }

        public void DownloadAsync()
        {
            this._status = DownloadStatus.Downloading;

            if (!File.Exists(this._fileName)) {
                this._fileName = Path.Combine(this._fileName, this._packageInfo.FileName);
            }
            this._httpClient.DownloadFileAsync(this._packageInfo.Address, this._fileName);
        }

        public void CancelAsync()
        {
            this._status = DownloadStatus.Cancel;
            this._httpClient.CancelAsync();
        }
    }
}
