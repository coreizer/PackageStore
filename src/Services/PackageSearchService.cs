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
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using AngleSharp;
using ByteSizeLib;
using PackageStore.Models;

namespace PackageStore.Services;

public sealed record SearchResult(IReadOnlyList<PackageModel> Packages, string? Title, IReadOnlyList<string> Errors);

/// <summary>
/// PlayStation のアップデートサーバーからパッケージ情報を取得します。(旧 frmMain の XMLParser / ProbeForRedump)
/// </summary>
public sealed partial class PackageSearchService : IDisposable {
  // https://www.psdevwiki.com/ps3/Environments
  private static readonly string[] Environments = [
    "https://a0.ww.np.dl.playstation.net/tpl/np/",
    "http://b0.ww.np.dl.playstation.net/tppkg/np/",
    "https://a0.ww.sp-int.dl.playstation.net/tpl/sp-int/",
    "http://b0.ww.sp-int.dl.playstation.net/tppkg/sp-int/",
    "https://a0.ww.prod-qa.dl.playstation.net/tpl/prod-qa/",
    "http://b0.ww.prod-qa.dl.playstation.net/tppkg/prod-qa/"
  ];

  private readonly HttpClient _http;

  public PackageSearchService() {
    // SSL エラーを無視します（アップデートサーバーの証明書は正当に検証できないため）。
    var handler = new HttpClientHandler {
      ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    };
    this._http = new HttpClient(handler);
  }

  public static bool IsValidPackageId(string? packageId) =>
    !string.IsNullOrWhiteSpace(packageId) && PackageIdRegex().IsMatch(packageId);

  /// <summary>
  /// redump.org のデータベースから内部シリアルを取得します。見つからない場合は null。
  /// </summary>
  public async Task<string?> ProbeForRedumpAsync(string packageId) {
    var id = DigitsOnlyRegex().Replace(packageId, "");
    if (string.IsNullOrEmpty(id)) {
      throw new InvalidOperationException("Error: Invalid Id from redump.");
    }

    var config = Configuration.Default.WithDefaultLoader();
    using var context = BrowsingContext.New(config);
    using var document = await context.OpenAsync($"http://redump.org/discs/quicksearch/{id}");
    var tbody = document.QuerySelector("table.gamecomments tbody");

    // 結果に tbody が存在する場合のみ継続する
    if (tbody is null) {
      return null;
    }

    var internalSerial = "";
    foreach (var child in tbody.ChildNodes) {
      if (child.TextContent.Contains("Internal Serial")) {
        internalSerial = child.TextContent.Split('\n').First();
      }
    }

    var serial = internalSerial.Split(':').Last().Replace("-", "").Trim();
    return string.IsNullOrEmpty(serial) ? null : serial;
  }

  public async Task<SearchResult> SearchAsync(string packageId, CancellationToken cancellationToken = default) {
    var packages = new List<PackageModel>();
    var errors = new List<string>();
    string? title = null;

    foreach (var url in Environments) {
      var xmlUrl = $"{url}{packageId}/{packageId}-ver.xml";
      Trace.WriteLine(xmlUrl, "URL");

      try {
        await using var stream = await this._http.GetStreamAsync(xmlUrl, cancellationToken);
        using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });

        var read = await reader.ReadAsync();
        while (read) {
          if (reader.NodeType == XmlNodeType.Element) {
            switch (reader.Name) {
              case "package":
                packages.Add(ReadPackage(xmlUrl, reader));
                break;
              case "TITLE":
                // ReadElementContentAsStringAsync は要素の次のノードまで進むため、ここでは Read しない
                title = await reader.ReadElementContentAsStringAsync();
                continue;
            }
          }
          read = await reader.ReadAsync();
        }
      }
      catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden) {
        // この環境にはパッケージが存在しない
      }
      catch (OperationCanceledException) {
        throw;
      }
      catch (Exception ex) {
        errors.Add(ex.Message);
      }
    }

    return new SearchResult(packages, title, errors);
  }

  private static PackageModel ReadPackage(string xmlUrl, XmlReader reader) {
    var package = new PackageModel { XmlUrl = xmlUrl };
    while (reader.MoveToNextAttribute()) {
      switch (reader.Name) {
        case "version":
          package.Version = reader.Value;
          break;
        case "size":
          package.Size = ByteSize.FromBytes(double.Parse(reader.Value, CultureInfo.InvariantCulture));
          break;
        case "sha1sum":
          package.Hash = reader.Value;
          break;
        case "ps3_system_ver":
          package.PS3SystemVer = reader.Value;
          break;
        case "psp_system_ver":
          package.PSPSystemVer = reader.Value;
          break;
        case "url":
          package.Name = Path.GetFileName(reader.Value);
          package.Url = new Uri(reader.Value);
          break;
      }
    }
    reader.MoveToElement();
    return package;
  }

  public void Dispose() => this._http.Dispose();

  [GeneratedRegex("^[A-Z0-9]")]
  private static partial Regex PackageIdRegex();

  [GeneratedRegex("[^0-9]")]
  private static partial Regex DigitsOnlyRegex();
}
