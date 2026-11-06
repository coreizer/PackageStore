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

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using PackageStore.Models;
using PackageStore.ViewModels;

namespace PackageStore.Views;

/// <summary>
/// 検索ページ。(旧 frmMain)
/// </summary>
public partial class SearchPage : UserControl {
  public SearchPage() {
    this.InitializeComponent();
  }

  private SearchViewModel? ViewModel => this.DataContext as SearchViewModel;

  private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e) =>
    this.ViewModel?.UpdateSelection(this.PackageGrid.SelectedItems.OfType<PackageModel>());

  private void OnDoubleTapped(object? sender, TappedEventArgs e) {
    // ヘッダーのダブルクリックでは何もしない
    if ((e.Source as Avalonia.Visual)?.FindAncestorOfType<DataGridRow>() is null) {
      return;
    }
    if (this.ViewModel is { } vm && vm.DownloadCommand.CanExecute(null)) {
      vm.DownloadCommand.Execute(null);
    }
  }

  private void OnPackageIdKeyDown(object? sender, KeyEventArgs e) {
    if (e.Key == Key.Enter && !this.PackageIdBox.IsDropDownOpen) {
      if (this.ViewModel is { } vm && vm.SearchCommand.CanExecute(null)) {
        vm.SearchCommand.Execute(null);
      }
      e.Handled = true;
    }
  }
}
