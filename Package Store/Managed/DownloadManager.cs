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
using System.Collections.Generic;

namespace PackageStore.Managed
{
  public class DownloadManager
  {
    #region フィールド

    private FrmDownload _instanceForms;

    public static List<Scheduler> _scheduleList;

    #endregion

    #region プロパティ

    public FrmDownload DownloadForms {
      get {
        if (this._instanceForms == null || this._instanceForms.IsDisposed) {
          this._instanceForms = new FrmDownload();
        }

        this._instanceForms.Activate();
        return this._instanceForms;
      }
      set {
        this._instanceForms = value;
      }
    }

    public static List<Scheduler> Schedules {
      get {
        if (_scheduleList == null) {
          _scheduleList = new List<Scheduler>();
        }
        return _scheduleList;
      }
      set {
        _scheduleList = value;
      }
    }

    #endregion

    #region イベント

    public static event EventHandler<SchedulerAddEventArgs> SchedulerAdd;

    #endregion

    protected virtual void OnSchedulerAdd(Scheduler scheduler)
    {
      SchedulerAdd?.Invoke(this, new SchedulerAddEventArgs(scheduler));
    }

    public void Add(Scheduler scheduler)
    {
      Schedules.Add(scheduler);
      OnSchedulerAdd(scheduler);
    }

    public void QueueDownload()
    {
      Schedules.ForEach(x => {
        if (x.Status == DownloadStatus.Pending)
          x.DownloadAsync();
      });
    }
  }
}
