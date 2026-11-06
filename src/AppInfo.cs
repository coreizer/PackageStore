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

using System.Reflection;

namespace PackageStore;

/// <summary>
/// アプリケーション共通の定数。(旧 Environment クラス)
/// </summary>
public static class AppInfo {
  public const string DefaultString = "Unknown";
  public const string Name = "Package Store";
  public const string RepositoryUrl = "https://github.com/coreizer/PackageStore";

  public static string Version {
    get {
      var assembly = typeof(AppInfo).Assembly;
      return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? assembly.GetName().Version?.ToString()
        ?? DefaultString;
    }
  }
}
