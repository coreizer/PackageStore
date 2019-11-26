/*
 * Copyright (c) 2017-2019 Coreizer
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

using PackageStore.Enums;
using PackageStore.Snippet;

namespace PackageStore.Managed
{
	public class JobContainer
	{
		private readonly WebClient client;

		public string FileName {
			get {
				string path = Path.Combine(DownloadManager.Directory, this.Package.Name);
				if (File.Exists(path))
				{
					path = Path.Combine(DownloadManager.Directory, DateTime.Now.Ticks + "-" + this.Package.Name);
				}

				return path;
			}
		}

		public DownloadStatus Status {
			get;
			set;
		}

		public PackageData Package {
			get;
		}

		private ListViewItem _viewItem;
		public ListViewItem ViewItem {
			get {
				if (this._viewItem == null)
				{
					this._viewItem = new ListViewItem { Text = this.Package.Name };
					this._viewItem.SubItems.AddRange(new[] {
						EnumsNET.Enums.GetName(this.Status),
						"0"
				  });
				}

				return this._viewItem;
			}
		}

		public JobContainer(PackageData package)
		{
			this.client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
			this.client.DownloadProgressChanged += this.Client_DownloadProgressChanged;
			this.client.DownloadFileCompleted += this.DownloadFileCompleted;

			this.Package = package;
		}

		private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.ViewItem.SubItems[1].Text = Enum.GetName(typeof(DownloadStatus), this.Status);
			this.ViewItem.SubItems[2].Text = e.ProgressPercentage + "%";
		}

		private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			this.Status = (!e.Cancelled && e.Error == null) ? DownloadStatus.Successful : DownloadStatus.Failed;
			this.ViewItem.SubItems[1].Text = EnumsNET.Enums.GetName(this.Status);
		}

		public void DownloadAsync()
		{
			this.Status = DownloadStatus.Downloading;
			this.client.DownloadFileAsync(this.Package.Url, this.FileName);
		}

		public void CancelAsync()
		{
			this.Status = DownloadStatus.Failed;
			this.client.CancelAsync();
		}
	}
}
