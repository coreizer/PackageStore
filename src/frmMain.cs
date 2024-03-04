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

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using AngleSharp;
using ByteSizeLib;
using PackageStore.Exceptions;
using PackageStore.Models;

namespace PackageStore
{
  public partial class frmMain : Form
  {
    // https://www.psdevwiki.com/ps3/Environments
    private readonly string[] Environments = {
         "https://a0.ww.np.dl.playstation.net/tpl/np/",
         "http://b0.ww.np.dl.playstation.net/tppkg/np/",
         "https://a0.ww.sp-int.dl.playstation.net/tpl/sp-int/",
         "http://b0.ww.sp-int.dl.playstation.net/tppkg/sp-int/",
         "https://a0.ww.prod-qa.dl.playstation.net/tpl/prod-qa/",
         "http://b0.ww.prod-qa.dl.playstation.net/tppkg/prod-qa/"
      };

    private readonly HttpClient _http;
    private readonly AutoCompleteStringCollection _autoComplete = new();
    private readonly List<Package> _items = new();

    private static Properties.Settings Settings {
      get => Properties.Settings.Default;
    }

    public new bool UseWaitCursor {
      get => base.UseWaitCursor;
      set {
        base.UseWaitCursor = value;
        this.buttonSearch.Enabled = !value;
        this.textBoxPackageId.Enabled = !value;
        this.ListViewPackage.Enabled = !value;
      }
    }

    private frmFileManager _fileManager;
    public frmFileManager FileManager {
      get {
        if (this._fileManager == null || this._fileManager.IsDisposed)
          this._fileManager = new frmFileManager();
        return this._fileManager;
      }
      set => this._fileManager = value;
    }

    public frmMain() {
      this.InitializeComponent();
      this.SetAutoCompleteSource();

      // Signore SSL errors.
      var handler = new HttpClientHandler();
      handler.ServerCertificateCustomValidationCallback += (s, a, b, e) => true;

      // Creating instance of HttpClient.
      this._http = new HttpClient(handler);
    }

    private void frmMain_Load(object sender, EventArgs e) {
      this.Text = Environment.Name;
      this.textBoxPackageId.Text = Settings.LastPackageId;
    }

    private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
      try {
        Settings.Save();
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private async void ButtonSearch_Click(object sender, EventArgs e) {
      try {
        this.UseWaitCursor = true;
        this.Text = Environment.Name;
        this.ListViewPackage.Items.Clear();
        this._items.Clear();

        var packageId = this.textBoxPackageId.Text.Trim();
        if (!this.IsValid(packageId)) throw new InvalidPackageException($"The package id '{packageId}' is Invalid");
        if (this.checkBoxRedump.Checked) {
          var serialId = await this.ProbeForRedump(packageId);
          if (serialId != null) {
            serialId = serialId.Replace("-", "").Trim();
            TaskDialog.ShowDialog(this, new TaskDialogPage() {
              Icon = TaskDialogIcon.Information,
              Text = $"Internal serial: Replace '{packageId}' to '{serialId}'",
              Caption = Environment.Name,
              Heading = "redump.org",
              Buttons = {
                        TaskDialogButton.OK
                     }
            });
            packageId = serialId;
          }
        }

        this.textBoxPackageId.Text = packageId;

        await Task.Run(async () => {
          await this.XMLParser(this._items, packageId);
          foreach (var pkg in this._items) {
            this.Invoke(() => {
              var addItem = new ListViewItem { Text = pkg.Name, Tag = pkg };
              addItem.SubItems.AddRange(new[] {
                        pkg.Size.ToString(),
                        pkg.Version,
                        pkg.PS3SystemVer != Environment.DefaultString ? pkg.PS3SystemVer : pkg.PSPSystemVer, /* PS3 or PSP */
                        pkg.Hash
                     });
              this.ListViewPackage.Items.Add(addItem);
            });
          }
        });

        if (this._items.Count <= 0) throw new PackageNotFoundException($"Nothing found for '{packageId}'\n\rplease try use the redump.org.");
        this.AddSuggestion(packageId);
      }
      catch (PackageNotFoundException ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Warning,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Package Not Found",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
      finally {
        this.UseWaitCursor = false;
      }
    }

    private async Task<string> ProbeForRedump(string packageId) {
      try {
        var id = Regex.Replace(packageId, @"[^0-9]", "");
        if (string.IsNullOrEmpty(id)) throw new InvalidOperationException($"Error: Invalid Id from redump.");
        var config = Configuration.Default.WithDefaultLoader();
        var document = await BrowsingContext.New(config).OpenAsync($"http://redump.org/discs/quicksearch/{id}");
        var tbody = document.QuerySelector("table.gamecomments tbody");

        // 結果にtbodyが存在する場合のみ継続する
        if (tbody is not null) {
          var internalSerial = "";
          foreach (var child in tbody.ChildNodes) {
            if (child.TextContent.Contains("Internal Serial")) {
              internalSerial = child.TextContent.Split('\n').First();
            }
          }
          return internalSerial.Split(':').Last();
        }
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
      return null;
    }

    private async Task XMLParser(List<Package> items, string packageId) {
      foreach (var url in this.Environments) {
        try {
          var xmlUrl = url + packageId + "/" + packageId + "-ver.xml";
          Trace.WriteLine(xmlUrl, "URL");
          var reader = XmlReader.Create(await this._http.GetStreamAsync(xmlUrl));
          do {
            if (reader.NodeType != XmlNodeType.Element) continue;

            switch (reader.Name) {
              case "package":
                items.Add(this.AttributeReaderPS3(xmlUrl, ref reader));
                break;
              case "TITLE":
                this.Invoke((Action)(() => this.Text = $"{Environment.Name} - {reader.ReadInnerXml()}"));
                break;
            }
          } while (reader.Read());
        }
        catch (HttpRequestException ex) {
          switch (ex.StatusCode) {
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Forbidden:
              break;
            default:
              TaskDialog.ShowDialog(this, new TaskDialogPage() {
                Icon = TaskDialogIcon.Error,
                Text = ex.Message,
                Caption = Environment.Name,
                Heading = "Error",
                Buttons = {
                           TaskDialogButton.OK
                        }
              });
              break;
          }
        }
        catch (Exception ex) {
          TaskDialog.ShowDialog(this, new TaskDialogPage() {
            Icon = TaskDialogIcon.Error,
            Text = ex.Message,
            Caption = Environment.Name,
            Heading = "Error",
            Buttons = {
                     TaskDialogButton.OK
                  }
          });
        }
      }
    }

    private void AddSuggestion(string packageId) {
      try {
        Settings.Suggestions.Clear();
        Settings.Suggestions.AddRange(new HashSet<string>(Settings.Suggestions.Cast<string>()) { packageId }.ToArray());
        Properties.Settings.Default.LastPackageId = packageId;
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = $"Suggestion Error: {ex.Message}",
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
      finally {
        Properties.Settings.Default.Save();
        this.SetAutoCompleteSource();
      }
    }

    private Package AttributeReaderPS3(string xmlUrl, ref XmlReader reader) {
      ArgumentNullException.ThrowIfNull(xmlUrl);
      ArgumentNullException.ThrowIfNull(reader);
      var package = new Package() { XmlUrl = xmlUrl };
      for (var i = 0; i < reader.AttributeCount; i++) {
        reader.MoveToNextAttribute();
        switch (reader.Name) {
          case "version":
            package.Version = reader.Value;
            break;
          case "size":
            package.Size = ByteSize.FromBytes(double.Parse(reader.Value.ToString()));
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
      return package;
    }

    private bool IsValid(string name) => string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : Regex.IsMatch(name, "^[A-Z0-9]");

    private void ListViewPackages_ItemActivate(object sender, EventArgs e) => this.AddToFileManager();

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {
      using var frm = new frmAbout();
      frm.ShowDialog();
    }

    private void GitHubToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        Process.Start(new ProcessStartInfo {
          FileName = "https://github.com/coreizer/PackageStore",
          UseShellExecute = true,
        });
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void CopyToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        if (this._items.Count <= 0) throw new InvalidOperationException("Package List is Empty");
        if (this.ListViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
        Clipboard.SetText(((Package)this.ListViewPackage.SelectedItems[0].Tag)[(string)((ToolStripMenuItem)sender).Tag]);
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void SetAutoCompleteSource() {
      try {
        this._autoComplete.Clear();
        this._autoComplete.AddRange(Settings.Suggestions.Cast<string>().ToArray());
        this.textBoxPackageId.AutoCompleteCustomSource = this._autoComplete;
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void ResetSuggestionToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        Settings.Suggestions.Clear();
        this.SetAutoCompleteSource();
      }
      finally {
        Properties.Settings.Default.Save();
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Information,
          Text = "has been removed from the suggestions list",
          Caption = Environment.Name,
          Heading = "Suggestions",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void ExportJsonStripMenuItem_Click(object sender, EventArgs e) {
      try {
        if (this._items.Count <= 0) throw new InvalidOperationException("Package List is Empty");
        using var SFD = new SaveFileDialog();
        SFD.FileName = $"Export-{this.textBoxPackageId.Text}-{DateTime.Now:yyyyMMddHHmmss}";
        SFD.Filter = "JSON File|*.json";
        var result = SFD.ShowDialog();
        if (result != DialogResult.OK) return;
        var jsonString = JsonSerializer.Serialize(
           new PackageExport(this._items),
           new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(SFD.FileName, jsonString, System.Text.Encoding.UTF8);
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void OpenXMLToolStripMenuItem_Click(object sender, EventArgs e) {
      try {
        if (this._items.Count <= 0) throw new InvalidOperationException("Package List is Empty");
        if (this.ListViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
        Process.Start(((Package)this.ListViewPackage.SelectedItems[0].Tag).XmlUrl);
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void ListViewPackage_SelectedIndexChanged(object sender, EventArgs e) =>
       this.toolStripStatusLabelSelected.Text = $"Selected item(s): {this.ListViewPackage.SelectedItems.Count}";

    private void DownloadToolStripMenuItem_Click(object sender, EventArgs e) => this.AddToFileManager();

    private void AddToFileManager() {
      try {
        if (this._items.Count <= 0) throw new InvalidOperationException("The package list is empty");
        if (this.ListViewPackage.SelectedIndices.Count < 1) throw new InvalidOperationException("Please select at least one package");
        if (string.IsNullOrEmpty(Common.SelectDirectoryPath())) return;
        foreach (ListViewItem item in this.ListViewPackage.SelectedItems) {
          this.FileManager.Add((Package)item.Tag);
        }
        this.FileManager.Show();
        this.FileManager.Activate();
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void ListViewPackage_KeyDown(object sender, KeyEventArgs e) {
      try {
        // Ctrl + A を検知して、全てのアイテムを選択状態にする
        if (e.KeyCode == Keys.A && e.Control) {
          foreach (ListViewItem item in this.ListViewPackage.Items) item.Selected = true;
        }
      }
      catch (Exception ex) {
        TaskDialog.ShowDialog(this, new TaskDialogPage() {
          Icon = TaskDialogIcon.Error,
          Text = ex.Message,
          Caption = Environment.Name,
          Heading = "Error",
          Buttons = {
                  TaskDialogButton.OK
               }
        });
      }
    }

    private void SaveDirectoryToolStripMenuItem_Click(object sender, EventArgs e) => Common.SelectDirectoryPath(true);
  }
}
