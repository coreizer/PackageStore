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

using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackageStore.Localization;
using PackageStore.Models;
using PackageStore.Services;

namespace PackageStore.ViewModels;

/// <summary>
/// ダウンローダーウィンドウの ViewModel。(旧 frmFileManager)
/// </summary>
public partial class DownloadViewModel : ObservableObject {
  private static readonly HttpClient Http = new();

  private readonly AppSettings _settings;
  private readonly IDialogService _dialogs;

  public ObservableCollection<DownloadItem> Files { get; } = [];

  [ObservableProperty]
  private string _queueText = Strings.Format("DownloadQueue", 0);

  /// <summary>
  /// 待機中 + ダウンロード中の件数。
  /// </summary>
  [ObservableProperty]
  private int _activeCount;

  /// <summary>
  /// 進行中のダウンロード全体の進捗 (0-100)。アプリ全体の進捗バーに使用します。
  /// </summary>
  [ObservableProperty]
  private double _overallPercent;

  /// <summary>
  /// 完了・失敗・キャンセル済みのアイテムがあるかどうか。
  /// </summary>
  [ObservableProperty]
  [NotifyCanExecuteChangedFor(nameof(ClearFinishedCommand))]
  private bool _hasFinished;

  [ObservableProperty]
  [NotifyCanExecuteChangedFor(nameof(CancelDownloadCommand))]
  [NotifyCanExecuteChangedFor(nameof(OpenFileCommand))]
  private DownloadItem? _selectedItem;

  public DownloadViewModel(AppSettings settings, IDialogService dialogs) {
    this._settings = settings;
    this._dialogs = dialogs;
  }

  /// <summary>
  /// パッケージをキューへ追加し、ダウンロードを開始します。
  /// </summary>
  public async Task AddAsync(PackageModel package) {
    var filePath = Path.Combine(this._settings.SaveFolderPath, package.Name);
    if (File.Exists(filePath)) {
      var overwrite = await this._dialogs.ConfirmAsync(package.Name, "File already exists. Do you want to overwrite it?");
      if (!overwrite) {
        return;
      }

      try {
        File.Delete(filePath);
      }
      catch (Exception ex) {
        await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
        return;
      }
    }

    var item = new DownloadItem(package);
    this.Files.Add(item);
    this.UpdateQueue();

    // UI スレッド上で開始し、await の継続も UI スレッドへ戻るため、プロパティ更新はそのままバインディングへ反映される。
    _ = this.DownloadAsync(item);
  }

  /// <summary>
  /// 進行中のダウンロードをすべてキャンセルします。(ウィンドウを閉じるときに使用)
  /// </summary>
  public void CancelAll() {
    foreach (var file in this.Files) {
      if (file.Status != DownloadStatus.Completed) {
        file.Cancel();
      }
    }
  }

  private bool HasSelection => this.SelectedItem is not null;

  [RelayCommand(CanExecute = nameof(HasSelection))]
  private void CancelDownload() => this.SelectedItem?.Cancel();

  [RelayCommand(CanExecute = nameof(HasSelection))]
  private async Task OpenFileAsync() {
    try {
      if (this.SelectedItem is null) {
        throw new InvalidOperationException("Please select at least one package");
      }
      Shell.RevealInFileManager(Path.Combine(this._settings.SaveFolderPath, this.SelectedItem.FileName));
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  /// <summary>
  /// 完了・失敗・キャンセル済みのアイテムをリストから取り除きます。
  /// </summary>
  [RelayCommand(CanExecute = nameof(HasFinished))]
  private void ClearFinished() {
    foreach (var item in this.Files.Where(x => !x.IsActive).ToArray()) {
      this.Files.Remove(item);
      item.Dispose();
    }
    this.UpdateQueue();
  }

  private async Task DownloadAsync(DownloadItem file) {
    file.StartTime = DateTime.Now;
    file.Status = DownloadStatus.Waiting;

    try {
      using var request = new HttpRequestMessage(HttpMethod.Get, file.Package.Url);
      using var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, file.Token);
      if (response.StatusCode != HttpStatusCode.OK) {
        throw new HttpRequestException($"Server returned {(int)response.StatusCode} {response.ReasonPhrase}", null, response.StatusCode);
      }

      var destination = Path.Combine(this._settings.SaveFolderPath, file.Package.Name);
      await using var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 64, useAsync: true);
      await using var readStream = await response.Content.ReadAsStreamAsync(file.Token);

      var contentLength = response.Content.Headers.ContentLength ?? (long)file.Package.Size.Bytes;
      var buffer = new byte[1024 * 1024];
      double totalRead = 0;
      var lastUpdate = DateTime.MinValue;

      file.Status = DownloadStatus.Downloading;

      while (true) {
        file.Token.ThrowIfCancellationRequested();
        var read = await readStream.ReadAsync(buffer, file.Token);
        if (read == 0) {
          break;
        }

        totalRead += read;
        await fileStream.WriteAsync(buffer.AsMemory(0, read), file.Token);

        // UI 更新は 200ms ごとに間引く
        var now = DateTime.Now;
        if ((now - lastUpdate).TotalMilliseconds < 200) {
          continue;
        }
        lastUpdate = now;

        file.ElapsedTime = now - file.StartTime;
        if (contentLength > 0) {
          file.Percent = (int)(totalRead * 100 / contentLength);
          var speed = totalRead / Math.Max(file.ElapsedTime.TotalSeconds, 0.001);
          file.EstimatedTime = TimeSpan.FromSeconds((contentLength - totalRead) / speed);
        }
        this.UpdateOverallProgress();
      }

      file.Percent = 100;
      file.Status = DownloadStatus.Completed;
    }
    catch (OperationCanceledException) {
      file.Status = DownloadStatus.Canceled;
    }
    catch (Exception ex) {
      file.Status = DownloadStatus.Failed;
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
    finally {
      file.Reset();
      this.UpdateQueue();
    }
  }

  private void UpdateQueue() {
    this.ActiveCount = this.Files.Count(x => x.IsActive);
    this.HasFinished = this.Files.Any(x => !x.IsActive);
    this.QueueText = Strings.Format("DownloadQueue", this.ActiveCount);
    this.UpdateOverallProgress();
  }

  /// <summary>
  /// 進行中アイテムの平均進捗を全体の進捗として算出します。
  /// </summary>
  private void UpdateOverallProgress() {
    var active = this.Files.Where(x => x.IsActive).ToArray();
    this.OverallPercent = active.Length == 0 ? 0 : active.Average(x => x.Percent);
  }
}
