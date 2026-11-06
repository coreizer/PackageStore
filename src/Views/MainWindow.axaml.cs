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

using System.Net.NetworkInformation;
using Avalonia.Controls;
using PackageStore.Services;
using PackageStore.ViewModels;

namespace PackageStore.Views;

/// <summary>
/// メインウィンドウ。左サイドバーでページ（検索 / ダウンロード / 設定）を切り替えます。
/// </summary>
public partial class MainWindow : Window {
  private readonly MainViewModel _viewModel;

  public MainWindow() {
    this.InitializeComponent();

    var settings = AppSettings.Load();
    var dialogs = new DialogService(() => this);
    this._viewModel = new MainViewModel(settings, dialogs) {
      ShowAboutRequested = () => new AboutWindow().ShowDialog(this)
    };
    this.DataContext = this._viewModel;
  }

  protected override async void OnOpened(EventArgs e) {
    base.OnOpened(e);

    // ネットワークが利用可能かどうかを確認します。
    if (!NetworkInterface.GetIsNetworkAvailable()) {
      await MessageDialog.ShowAsync(this, "Network", "Network is not available.", MessageKind.Warning);
    }
  }

  protected override void OnClosing(WindowClosingEventArgs e) {
    this._viewModel.SaveSettings();
    // 旧実装と同様、終了時に未完了のダウンロードはキャンセルされる
    this._viewModel.Downloads.CancelAll();
    base.OnClosing(e);
  }
}
