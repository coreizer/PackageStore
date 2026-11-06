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

using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PackageStore.Services;

/// <summary>
/// ユーザー設定。(旧 Properties.Settings の置き換え)
/// %APPDATA%\PackageStore\settings.json に JSON で保存します。
/// </summary>
public sealed class AppSettings {
  private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

  public static string FilePath { get; } = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "PackageStore",
    "settings.json");

  public List<string> Suggestions { get; set; } = [];

  public string LastPackageId { get; set; } = string.Empty;

  /// <summary>
  /// 保存先フォルダーの初期値。ユーザーの「ダウンロード」フォルダーです。
  /// </summary>
  public static string DefaultSaveFolderPath { get; } = GetDownloadsFolder();

  public string SaveFolderPath { get; set; } = DefaultSaveFolderPath;

  [JsonIgnore]
  public bool HasSaveFolder => !string.IsNullOrEmpty(this.SaveFolderPath) && Directory.Exists(this.SaveFolderPath);

  /// <summary>
  /// すべての設定を初期値に戻します。(ファイルへの保存は行いません)
  /// </summary>
  public void Reset() {
    this.Suggestions.Clear();
    this.LastPackageId = string.Empty;
    this.SaveFolderPath = DefaultSaveFolderPath;
  }

  public static AppSettings Load() {
    try {
      if (File.Exists(FilePath)) {
        var json = File.ReadAllText(FilePath);
        var settings = JsonSerializer.Deserialize<AppSettings>(json, Options) ?? new AppSettings();
        // 旧バージョンの設定ファイルで保存先が空のままなら初期値（ダウンロード）を使う
        if (string.IsNullOrWhiteSpace(settings.SaveFolderPath)) {
          settings.SaveFolderPath = DefaultSaveFolderPath;
        }
        return settings;
      }
    }
    catch {
      // 壊れた設定ファイルは無視して初期値を使う
    }
    return new AppSettings();
  }

  public void Save() {
    var dir = Path.GetDirectoryName(FilePath);
    if (!string.IsNullOrEmpty(dir)) {
      Directory.CreateDirectory(dir);
    }
    File.WriteAllText(FilePath, JsonSerializer.Serialize(this, Options));
  }

  /// <summary>
  /// ユーザーの「ダウンロード」フォルダーのパスを返します。
  /// Windows では既知フォルダー (FOLDERID_Downloads) を参照するため、移動されていても追従します。
  /// 取得できない場合はホーム直下の Downloads を返します。
  /// </summary>
  private static string GetDownloadsFolder() {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      try {
        var downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        if (SHGetKnownFolderPath(downloads, 0, IntPtr.Zero, out var path) == 0 && !string.IsNullOrEmpty(path)) {
          return path;
        }
      }
      catch {
        // フォールバックへ
      }
    }
    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
  }

  [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true)]
  private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] out string pszPath);
}
