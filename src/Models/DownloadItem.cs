#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2026 coreizer
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

using CommunityToolkit.Mvvm.ComponentModel;

namespace PackageStore.Models;

public enum DownloadStatus {
  Waiting,
  Downloading,
  Completed,
  Canceled,
  Failed
}

/// <summary>
/// ダウンロードキューの 1 アイテム。(旧 frmFileManager.FileItem)
/// </summary>
public partial class DownloadItem : ObservableObject, IDisposable {
  private readonly CancellationTokenSource _source = new();

  public PackageModel Package { get; }

  public string FileName => this.Package.Name;

  public string SizeText => this.Package.Size.ToString();

  public CancellationToken Token => this._source.Token;

  public DateTime StartTime { get; set; }

  public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

  [ObservableProperty]
  private DownloadStatus _status = DownloadStatus.Waiting;

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(PercentText))]
  private int _percent;

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(EstimatedTimeText))]
  private TimeSpan _estimatedTime = TimeSpan.Zero;

  public string PercentText => $"{this.Percent}%";

  public string EstimatedTimeText => $"{this.EstimatedTime.Minutes}:{this.EstimatedTime.Seconds:00}(s)";

  public bool IsActive => this.Status is DownloadStatus.Waiting or DownloadStatus.Downloading;

  public DownloadItem(PackageModel package) {
    this.Package = package;
  }

  public void Cancel() {
    if (!this._source.IsCancellationRequested) {
      this._source.Cancel();
    }
  }

  internal void Reset() {
    this.Percent = this.Percent > 1 ? this.Percent : 0;
    this.ElapsedTime = TimeSpan.Zero;
    this.EstimatedTime = TimeSpan.Zero;
  }

  public void Dispose() => this._source.Dispose();
}
