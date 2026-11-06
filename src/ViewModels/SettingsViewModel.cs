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
using PackageStore.Localization;
using PackageStore.Services;

namespace PackageStore.ViewModels;

/// <summary>
/// 設定ページの ViewModel。保存先フォルダーと検索履歴を管理します。
/// </summary>
public partial class SettingsViewModel : ObservableObject {
  private readonly AppSettings _settings;
  private readonly IDialogService _dialogs;

  /// <summary>
  /// 検索履歴がリセットされたときに発生します。
  /// </summary>
  public event EventHandler? SuggestionsCleared;

  /// <summary>
  /// すべての設定が初期化されたときに発生します。
  /// </summary>
  public event EventHandler? SettingsReset;

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(HasSaveFolder))]
  [NotifyCanExecuteChangedFor(nameof(OpenSaveFolderCommand))]
  private string _saveFolderPath = string.Empty;

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(SuggestionCountText))]
  [NotifyCanExecuteChangedFor(nameof(ResetSuggestionsCommand))]
  private int _suggestionCount;

  public bool HasSaveFolder => this._settings.HasSaveFolder;

  public string SuggestionCountText => Strings.Format("SuggestionsCount", this.SuggestionCount);

  public string SettingsFilePath => AppSettings.FilePath;

  public SettingsViewModel(AppSettings settings, IDialogService dialogs) {
    this._settings = settings;
    this._dialogs = dialogs;
    this.SaveFolderPath = settings.SaveFolderPath;
    this.RefreshSuggestionCount();
  }

  /// <summary>
  /// 保存先フォルダーを設定して保存します。
  /// </summary>
  public void SetSaveFolder(string path) {
    this._settings.SaveFolderPath = path;
    this.Save();
    this.SaveFolderPath = path;
  }

  /// <summary>
  /// 保存先フォルダーが未設定なら選択ダイアログを表示します。(旧 Common.SelectDirectoryPath)
  /// </summary>
  public async Task<bool> EnsureSaveFolderAsync() {
    if (this._settings.HasSaveFolder) {
      return true;
    }
    var path = await this._dialogs.PickFolderAsync();
    if (string.IsNullOrEmpty(path)) {
      return false;
    }
    this.SetSaveFolder(path);
    return true;
  }

  public void RefreshSuggestionCount() => this.SuggestionCount = this._settings.Suggestions.Count;

  public void Save() {
    try {
      this._settings.Save();
    }
    catch {
      // 保存失敗は無視する
    }
  }

  [RelayCommand]
  private async Task BrowseSaveFolderAsync() {
    var path = await this._dialogs.PickFolderAsync();
    if (!string.IsNullOrEmpty(path)) {
      this.SetSaveFolder(path);
    }
  }

  [RelayCommand(CanExecute = nameof(HasSaveFolder))]
  private async Task OpenSaveFolderAsync() {
    try {
      Shell.OpenFolder(this._settings.SaveFolderPath);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  /// <summary>
  /// 設定ファイルの場所をファイルマネージャーで開きます。(未保存ならまず保存してファイルを作る)
  /// </summary>
  [RelayCommand]
  private async Task OpenSettingsFileAsync() {
    try {
      if (!File.Exists(AppSettings.FilePath)) {
        this._settings.Save();
      }
      Shell.RevealInFileManager(AppSettings.FilePath);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  private bool HasSuggestions => this.SuggestionCount > 0;

  [RelayCommand(CanExecute = nameof(HasSuggestions))]
  private async Task ResetSuggestionsAsync() {
    this._settings.Suggestions.Clear();
    this.Save();
    this.RefreshSuggestionCount();
    this.SuggestionsCleared?.Invoke(this, EventArgs.Empty);
    await this._dialogs.ShowMessageAsync("Suggestions", "has been removed from the suggestions list");
  }

  /// <summary>
  /// すべての設定を初期値に戻して保存します。
  /// </summary>
  [RelayCommand]
  private async Task ResetSettingsAsync() {
    var confirmed = await this._dialogs.ConfirmAsync(Strings.Get("ResetSettings"), Strings.Get("ResetSettingsConfirm"));
    if (!confirmed) {
      return;
    }

    this._settings.Reset();
    this.Save();
    this.SaveFolderPath = this._settings.SaveFolderPath;
    this.OnPropertyChanged(nameof(this.HasSaveFolder));
    this.OpenSaveFolderCommand.NotifyCanExecuteChanged();
    this.RefreshSuggestionCount();
    this.SuggestionsCleared?.Invoke(this, EventArgs.Empty);
    this.SettingsReset?.Invoke(this, EventArgs.Empty);
    await this._dialogs.ShowMessageAsync(Strings.Get("ResetSettings"), Strings.Get("ResetSettingsDone"));
  }
}
