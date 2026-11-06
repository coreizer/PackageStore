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
using Avalonia.Interactivity;
using Avalonia.Media;
using PackageStore.Services;

namespace PackageStore.Views;

/// <summary>
/// WinForms の TaskDialog に相当する簡易メッセージダイアログ。
/// </summary>
public partial class MessageDialog : Window {
  public MessageDialog() {
    this.InitializeComponent();
  }

  private void Configure(string heading, string text, MessageKind kind, bool confirm) {
    this.Title = AppInfo.Name;
    this.HeadingText.Text = heading;
    this.MessageText.Text = text;

    var (glyph, color) = kind switch {
      MessageKind.Warning => ("!", "#F59E0B"),
      MessageKind.Error => ("✕", "#EF4444"),
      _ => ("i", "#3B82F6")
    };

    this.IconGlyph.Text = glyph;
    this.IconGlyph.Foreground = Brushes.White;
    this.IconBadge.Background = new SolidColorBrush(Color.Parse(color));

    if (confirm) {
      this.OkButton.Content = "Yes";
      this.NoButton.IsVisible = true;
    }
  }

  private void OnOkClick(object? sender, RoutedEventArgs e) => this.Close(true);

  private void OnNoClick(object? sender, RoutedEventArgs e) => this.Close(false);

  public static async Task ShowAsync(Window owner, string heading, string text, MessageKind kind = MessageKind.Information) {
    var dialog = new MessageDialog();
    dialog.Configure(heading, text, kind, confirm: false);
    await dialog.ShowDialog(owner);
  }

  public static async Task<bool> ConfirmAsync(Window owner, string heading, string text, MessageKind kind = MessageKind.Warning) {
    var dialog = new MessageDialog();
    dialog.Configure(heading, text, kind, confirm: true);
    var result = await dialog.ShowDialog<bool?>(owner);
    return result == true;
  }
}
