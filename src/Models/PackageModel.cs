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

using System.Text.Json.Serialization;
using ByteSizeLib;

namespace PackageStore.Models;

public record PackageModel {
  [JsonPropertyName("name")]
  public string Name { get; set; } = AppInfo.DefaultString;

  [JsonPropertyName("version")]
  public string Version { get; set; } = AppInfo.DefaultString;

  [JsonPropertyName("ps3_system_ver")]
  public string PS3SystemVer { get; set; } = AppInfo.DefaultString;

  [JsonPropertyName("psp_system_ver")]
  public string PSPSystemVer { get; set; } = AppInfo.DefaultString;

  [JsonPropertyName("formated_size")]
  public ByteSize Size { get; set; }

  [JsonPropertyName("hash")]
  public string Hash { get; set; } = AppInfo.DefaultString;

  [JsonPropertyName("url")]
  public Uri? Url { get; set; }

  [JsonPropertyName("xml")]
  public string XmlUrl { get; set; } = AppInfo.DefaultString;

  /// <summary>
  /// PS3 または PSP のシステムバージョン（表示用）。
  /// </summary>
  [JsonIgnore]
  public string SystemVersion => this.PS3SystemVer != AppInfo.DefaultString ? this.PS3SystemVer : this.PSPSystemVer;

  /// <summary>
  /// 人が読める形式のサイズ（表示用）。
  /// </summary>
  [JsonIgnore]
  public string SizeText => this.Size.ToString();

  /// <summary>
  /// クリップボードコピー用にプロパティ名から値を取得します。
  /// </summary>
  public string this[string propertyName] => propertyName switch {
    "Url" => this.Url?.ToString() ?? AppInfo.DefaultString,
    "Size" => this.SizeText,
    "Version" => this.Version,
    "SystemVersion" or "SP_SYS" => this.SystemVersion,
    "Hash" => this.Hash,
    "Name" => this.Name,
    "XmlUrl" => this.XmlUrl,
    _ => typeof(PackageModel).GetProperty(propertyName)?.GetValue(this)?.ToString() ?? AppInfo.DefaultString
  };
}
