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
using CommunityToolkit.Mvvm.Input;
using PackageStore.Services;

namespace PackageStore.ViewModels;

public enum AppPage {
  Search,
  Downloads,
  Settings
}

/// <summary>
/// メインウィンドウの ViewModel。ページ切り替え（ナビゲーション）と各ページの ViewModel を保持します。
/// </summary>
public partial class MainViewModel : ObservableObject {
  private readonly IDialogService _dialogs;

  public SearchViewModel Search { get; }

  public DownloadViewModel Downloads { get; }

  public SettingsViewModel Settings { get; }

  /// <summary>
  /// バージョン情報ダイアログを表示するための View 側コールバック。
  /// </summary>
  public Func<Task>? ShowAboutRequested { get; set; }

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(IsSearchPage), nameof(IsDownloadsPage), nameof(IsSettingsPage))]
  private AppPage _currentPage = AppPage.Search;

  public bool IsSearchPage => this.CurrentPage == AppPage.Search;

  public bool IsDownloadsPage => this.CurrentPage == AppPage.Downloads;

  public bool IsSettingsPage => this.CurrentPage == AppPage.Settings;

  /// <summary>
  /// アプリ全体の進捗バーを表示するかどうか。(検索中またはダウンロード中)
  /// </summary>
  public bool IsProgressVisible => this.Search.IsBusy || this.Downloads.ActiveCount > 0;

  /// <summary>
  /// 検索中は終了時刻が分からないため不確定表示にします。
  /// </summary>
  public bool IsProgressIndeterminate => this.Search.IsBusy;

  /// <summary>
  /// ダウンロード全体の進捗 (0-100)。
  /// </summary>
  public double ProgressValue => this.Downloads.OverallPercent;

  public MainViewModel(AppSettings settings, IDialogService dialogs) {
    this._dialogs = dialogs;
    this.Settings = new SettingsViewModel(settings, dialogs);
    this.Downloads = new DownloadViewModel(settings, dialogs);
    this.Search = new SearchViewModel(settings, dialogs, this.Settings, this.Downloads) {
      // 旧実装でダウンローダーウィンドウを開いていた動作に合わせ、ダウンロードページへ遷移する
      DownloadsStarted = () => this.CurrentPage = AppPage.Downloads
    };

    this.Search.PropertyChanged += (_, e) => {
      if (e.PropertyName == nameof(SearchViewModel.IsBusy)) {
        this.OnPropertyChanged(nameof(this.IsProgressVisible));
        this.OnPropertyChanged(nameof(this.IsProgressIndeterminate));
      }
    };
    this.Downloads.PropertyChanged += (_, e) => {
      switch (e.PropertyName) {
        case nameof(DownloadViewModel.ActiveCount):
          this.OnPropertyChanged(nameof(this.IsProgressVisible));
          break;
        case nameof(DownloadViewModel.OverallPercent):
          this.OnPropertyChanged(nameof(this.ProgressValue));
          break;
      }
    };
  }

  public void SaveSettings() => this.Settings.Save();

  [RelayCommand]
  private void Navigate(AppPage page) => this.CurrentPage = page;

  [RelayCommand]
  private async Task ShowAboutAsync() {
    if (this.ShowAboutRequested is not null) {
      await this.ShowAboutRequested();
    }
  }

  [RelayCommand]
  private async Task OpenGitHubAsync() {
    try {
      Shell.OpenUrl(AppInfo.RepositoryUrl);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }
}
