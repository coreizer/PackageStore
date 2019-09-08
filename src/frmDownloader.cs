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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PackageStore.Managed;

namespace PackageStore
{
	public partial class frmDownloader : Form
	{
		public frmDownloader()
		{
			this.InitializeComponent();

			DownloadManager.JobAdd += this.DownloadManager_JobAdd;
		}

		private void DownloadManager_JobAdd(object sender, JobAddEventArgs e)
		{
			this.listViewPackage.Items.Clear();
			DownloadManager.JobItems.ForEach(x => this.listViewPackage.Items.Add(x.ViewItem));
		}

		private void FrmDownload_Load(object sender, EventArgs e)
		{
			DownloadManager.JobItems.ForEach(job => this.listViewPackage.Items.Add(job.ViewItem));
		}

		private void ListViewPackages_ItemActivate(object sender, EventArgs e)
		{
			if (this.listViewPackage.SelectedIndices.Count == 0)
				return;

			try
			{
				JobContainer job = DownloadManager.JobItems.First(x => x.FileName == this.listViewPackage.SelectedItems[0].Name);
				Process.Start(Directory.GetParent(job.FileName).FullName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
