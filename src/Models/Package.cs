#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2023 coreizer
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

using ByteSizeLib;
using System.Text.Json.Serialization;

namespace PackageStore.Models {
  public record Package {
    [JsonPropertyName("name")]
    public string Name { get; set; } = PackageStore.Environment.DefaultString;

    [JsonPropertyName("version")]
    public string Version { get; set; } = PackageStore.Environment.DefaultString;

    [JsonPropertyName("ps3_system_ver")]
    public string PS3SystemVer { get; set; } = PackageStore.Environment.DefaultString;

    [JsonPropertyName("psp_system_ver")]
    public string PSPSystemVer { get; set; } = PackageStore.Environment.DefaultString;

    [JsonPropertyName("formated_size")]
    public ByteSize Size { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; } = PackageStore.Environment.DefaultString;

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("xml")]
    public string XmlUrl { get; set; } = PackageStore.Environment.DefaultString;

    public string this[string propertyName] {
      get {
        if (propertyName == "SP_SYS") {
          return this.PS3SystemVer != PackageStore.Environment.DefaultString ? this.PS3SystemVer : this.PSPSystemVer;
        }
        return typeof(Package).GetProperty(propertyName)?.GetValue(this)?.ToString(); // NOTE: Because is 'ByteSizeLib ' cannot be cast, use ToString()
      }
    }
  }
}