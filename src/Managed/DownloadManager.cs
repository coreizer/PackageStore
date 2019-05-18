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
using System.Collections.Generic;

using PackageStore.Enums;

namespace PackageStore.Managed
{
  public class DownloadManager
  {
    public static event EventHandler<JobAddEventArgs> JobAdd;

    private static List<JobContainer> _jobItems;

    public static List<JobContainer> JobItems {
      get {
        if (_jobItems == null) {
          _jobItems = new List<JobContainer>();
        }
        return _jobItems;
      }
      set => _jobItems = value;
    }

    public static string Directory {
      get;
      set;
    }

    private frmDownloader _instance;
    public frmDownloader Form {
      get {
        if (this._instance == null || this._instance.IsDisposed) {
          this._instance = new frmDownloader();
        }

        this._instance.Activate();
        this._instance.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

        return this._instance;
      }
      set => this._instance = value;
    }

    public void Start() =>
      JobItems.ForEach(x => {
        if (x.Status != DownloadStatus.Successful)
          x.DownloadAsync();
      });

    public void Add(JobContainer job)
    {
      JobItems.Add(job);
      this.OnJobItemAdd(job);
    }

    protected virtual void OnJobItemAdd(JobContainer job) =>
      JobAdd?.Invoke(this, new JobAddEventArgs(job));
  }
}
