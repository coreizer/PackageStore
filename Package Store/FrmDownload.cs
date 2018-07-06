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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PackageStore.Managed;

namespace PackageStore
{
  public partial class FrmDownload : Form
  {
    public FrmDownload()
    {
      InitializeComponent();

      DownloadManager.SchedulerAdd += this.DownloadManager_SchedulerAdd;
    }

    private void DownloadManager_SchedulerAdd(object sender, SchedulerAddEventArgs e)
    {
      this.listViewPackages.Items.Clear();
      DownloadManager.Schedules.ForEach(x => this.listViewPackages.Items.Add(x.Item));
    }

    private void FrmDownload_Load(object sender, EventArgs e)
    {
      DownloadManager.Schedules.ForEach(x => this.listViewPackages.Items.Add(x.Item));
    }

    private void ListViewPackages_ItemActivate(object sender, EventArgs e)
    {
      if (this.listViewPackages.SelectedIndices.Count == 0)
        return;

      try {
        string name = this.listViewPackages.SelectedItems[0].Text;
        Scheduler scheduler = DownloadManager.Schedules.Where(x => x.Package.FileName == name).First();
        Process.Start(Directory.GetParent(scheduler.Directory).FullName);
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
