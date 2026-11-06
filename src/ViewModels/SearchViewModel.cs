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
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackageStore.Exceptions;
using PackageStore.Localization;
using PackageStore.Models;
using PackageStore.Services;

namespace PackageStore.ViewModels;

/// <summary>
/// 検索ページの ViewModel。(旧 frmMain)
/// </summary>
public partial class SearchViewModel : ObservableObject {
  private static readonly JsonSerializerOptions ExportOptions = new() { WriteIndented = true };

  private readonly AppSettings _settings;
  private readonly IDialogService _dialogs;
  private readonly SettingsViewModel _settingsVm;
  private readonly DownloadViewModel _downloads;
  private readonly PackageSearchService _search = new();

  public ObservableCollection<PackageModel> Packages { get; } = [];

  public ObservableCollection<string> Suggestions { get; } = [];

  /// <summary>
  /// ダウンロードをキューへ追加した後に呼ばれるコールバック。(ダウンロードページへの遷移に使用)
  /// </summary>
  public Action? DownloadsStarted { get; set; }

  [ObservableProperty]
  private string _title = AppInfo.Name;

  [ObservableProperty]
  private string _packageId = string.Empty;

  [ObservableProperty]
  private bool _useRedump;

  [ObservableProperty]
  [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
  private bool _isBusy;

  [ObservableProperty]
  private string _selectedText = Strings.Format("SelectedItems", 0);

  [ObservableProperty]
  [NotifyCanExecuteChangedFor(nameof(DownloadCommand))]
  [NotifyCanExecuteChangedFor(nameof(OpenXmlCommand))]
  [NotifyCanExecuteChangedFor(nameof(CopyCommand))]
  private IReadOnlyList<PackageModel> _selectedPackages = [];

  public string CopyrightText => Strings.Get("Copyright");

  public SearchViewModel(AppSettings settings, IDialogService dialogs, SettingsViewModel settingsVm, DownloadViewModel downloads) {
    this._settings = settings;
    this._dialogs = dialogs;
    this._settingsVm = settingsVm;
    this._downloads = downloads;
    this.PackageId = settings.LastPackageId;
    this.RefreshSuggestions();
    settingsVm.SuggestionsCleared += (_, _) => this.RefreshSuggestions();
    settingsVm.SettingsReset += (_, _) => this.PackageId = string.Empty;
  }

  private bool CanSearch => !this.IsBusy;

  [RelayCommand(CanExecute = nameof(CanSearch))]
  private async Task SearchAsync() {
    try {
      this.IsBusy = true;
      this.Title = AppInfo.Name;
      this.Packages.Clear();

      var packageId = this.PackageId.Trim();
      if (!PackageSearchService.IsValidPackageId(packageId)) {
        throw new InvalidPackageException($"The package id '{packageId}' is Invalid");
      }

      if (this.UseRedump) {
        var serialId = await this._search.ProbeForRedumpAsync(packageId);
        if (serialId is not null) {
          await this._dialogs.ShowMessageAsync("redump.org", $"Internal serial: Replace '{packageId}' to '{serialId}'");
          packageId = serialId;
        }
      }

      this.PackageId = packageId;

      var result = await this._search.SearchAsync(packageId);
      foreach (var pkg in result.Packages) {
        this.Packages.Add(pkg);
      }

      if (result.Title is not null) {
        this.Title = $"{AppInfo.Name} - {result.Title}";
      }

      foreach (var error in result.Errors) {
        await this._dialogs.ShowMessageAsync("Error", error, MessageKind.Error);
      }

      if (this.Packages.Count <= 0) {
        throw new PackageNotFoundException($"Nothing found for '{packageId}'\nplease try use the redump.org.");
      }

      this.AddSuggestion(packageId);
    }
    catch (PackageNotFoundException ex) {
      await this._dialogs.ShowMessageAsync("Package Not Found", ex.Message, MessageKind.Warning);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
    finally {
      this.IsBusy = false;
    }
  }

  private bool HasSelection => this.SelectedPackages.Count > 0;

  [RelayCommand(CanExecute = nameof(HasSelection))]
  private async Task DownloadAsync() {
    try {
      if (this.Packages.Count <= 0) {
        throw new InvalidOperationException("The package list is empty");
      }
      if (!this.HasSelection) {
        throw new InvalidOperationException("Please select at least one package");
      }

      if (!await this._settingsVm.EnsureSaveFolderAsync()) {
        return;
      }

      foreach (var package in this.SelectedPackages.ToArray()) {
        await this._downloads.AddAsync(package);
      }

      this.DownloadsStarted?.Invoke();
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  [RelayCommand(CanExecute = nameof(HasSelection))]
  private async Task OpenXmlAsync() {
    try {
      if (this.Packages.Count <= 0) {
        throw new InvalidOperationException("Package List is Empty");
      }
      var package = this.SelectedPackages.FirstOrDefault()
        ?? throw new InvalidOperationException("Please select at least one package");
      Shell.OpenUrl(package.XmlUrl);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  /// <summary>
  /// 選択中パッケージの指定プロパティをクリップボードへコピーします。
  /// </summary>
  /// <param name="propertyName">Url / Size / Version / SystemVersion / Hash</param>
  [RelayCommand(CanExecute = nameof(HasSelection))]
  private async Task CopyAsync(string propertyName) {
    try {
      if (this.Packages.Count <= 0) {
        throw new InvalidOperationException("Package List is Empty");
      }
      var package = this.SelectedPackages.FirstOrDefault()
        ?? throw new InvalidOperationException("Please select at least one package");
      await this._dialogs.SetClipboardTextAsync(package[propertyName]);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  private bool HasPackages => this.Packages.Count > 0;

  [RelayCommand]
  private async Task ExportToJsonAsync() {
    try {
      if (!this.HasPackages) {
        throw new InvalidOperationException("Package List is Empty");
      }

      var path = await this._dialogs.PickSaveFileAsync(
        $"Export-{this.PackageId}-{DateTime.Now:yyyyMMddHHmmss}", "JSON File", "json");
      if (string.IsNullOrEmpty(path)) {
        return;
      }

      var json = JsonSerializer.Serialize(new PackageExportModel([.. this.Packages]), ExportOptions);
      await File.WriteAllTextAsync(path, json, Encoding.UTF8);
    }
    catch (Exception ex) {
      await this._dialogs.ShowMessageAsync("Error", ex.Message, MessageKind.Error);
    }
  }

  /// <summary>
  /// View の選択変更を受け取ります。
  /// </summary>
  public void UpdateSelection(IEnumerable<PackageModel> selected) {
    this.SelectedPackages = selected.ToList();
    this.SelectedText = Strings.Format("SelectedItems", this.SelectedPackages.Count);
  }

  private void AddSuggestion(string packageId) {
    if (!this._settings.Suggestions.Contains(packageId)) {
      this._settings.Suggestions.Add(packageId);
    }
    this._settings.LastPackageId = packageId;
    this._settingsVm.Save();
    this._settingsVm.RefreshSuggestionCount();
    this.RefreshSuggestions();
  }

  private void RefreshSuggestions() {
    this.Suggestions.Clear();
    foreach (var s in this._settings.Suggestions) {
      this.Suggestions.Add(s);
    }
  }
}
