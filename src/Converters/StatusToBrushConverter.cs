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

using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using PackageStore.Models;

namespace PackageStore.Converters;

/// <summary>
/// ダウンロード状態を文字色へ変換します。(旧 FileItem.Status の ForeColor 切り替え)
/// </summary>
public sealed class StatusToBrushConverter : IValueConverter {
  public static readonly StatusToBrushConverter Instance = new();

  private static readonly IBrush Completed = new SolidColorBrush(Color.Parse("#4ADE80"));
  private static readonly IBrush Failed = new SolidColorBrush(Color.Parse("#F87171"));
  private static readonly IBrush Canceled = new SolidColorBrush(Color.Parse("#9CA3AF"));
  private static readonly IBrush Downloading = new SolidColorBrush(Color.Parse("#60A5FA"));
  private static readonly IBrush Normal = new SolidColorBrush(Color.Parse("#E5E7EB"));

  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch {
    DownloadStatus.Completed => Completed,
    DownloadStatus.Failed => Failed,
    DownloadStatus.Canceled => Canceled,
    DownloadStatus.Downloading => Downloading,
    _ => Normal
  };

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    throw new NotSupportedException();
}
