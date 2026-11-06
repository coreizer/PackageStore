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

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PackageStore.Services;

public enum MessageKind {
  Information,
  Warning,
  Error
}

/// <summary>
/// ViewModel から View（ダイアログ・ピッカー・クリップボード）へアクセスするための抽象化。
/// </summary>
public interface IDialogService {
  Task ShowMessageAsync(string heading, string text, MessageKind kind = MessageKind.Information);

  Task<bool> ConfirmAsync(string heading, string text, MessageKind kind = MessageKind.Warning);

  Task<string?> PickFolderAsync();

  Task<string?> PickSaveFileAsync(string suggestedFileName, string filterName, string extension);

  Task SetClipboardTextAsync(string text);
}

public static class Shell {
  /// <summary>
  /// 既定のブラウザで URL を開きます。
  /// </summary>
  public static void OpenUrl(string url) =>
    Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });

  /// <summary>
  /// ファイルマネージャーでフォルダーを開きます。
  /// </summary>
  public static void OpenFolder(string folderPath) {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Process.Start("explorer.exe", $"\"{folderPath}\"");
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
      Process.Start("open", [folderPath]);
    }
    else {
      Process.Start("xdg-open", [folderPath]);
    }
  }

  /// <summary>
  /// ファイルマネージャーで対象ファイルを選択状態で開きます。
  /// </summary>
  public static void RevealInFileManager(string filePath) {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Process.Start("explorer.exe", $"/select,\"{filePath}\"");
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
      Process.Start("open", ["-R", filePath]);
    }
    else {
      Process.Start("xdg-open", [Path.GetDirectoryName(filePath) ?? filePath]);
    }
  }
}
