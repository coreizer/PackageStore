#region License Information (GPL v3)

/**
 * Copyright (C) 2017-2022 coreizer
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

namespace PackageStore
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.IO;
   using System.Linq;
   using System.Net;
   using System.Text.Json;
   using System.Text.RegularExpressions;
   using System.Threading.Tasks;
   using System.Windows.Forms;
   using System.Xml;
   using AngleSharp;
   using ByteSizeLib;
   using PackageStore.Exceptions;
   using PackageStore.Models;

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

      private List<Package> _items;

      private readonly AutoCompleteStringCollection _autoComplete = new AutoCompleteStringCollection();

      public new bool UseWaitCursor
      {
         get => base.UseWaitCursor;
         set {
            base.UseWaitCursor = value;
            this.buttonSearch.Enabled = !value;
            this.textBoxPackageId.Enabled = !value;
            this.listViewPackage.Enabled = !value;
         }
      }

      public frmMain()
      {
         this.InitializeComponent();
         this.SetAutoCompleteSource();

         // SSL: Certificate Revocation Allow
         ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
            (object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors) => true
         );
      }

      private async void ButtonSearch_Click(object sender, EventArgs e)
      {
         try {
            this.UseWaitCursor = true;
            this.Text = Application.ProductName;
            this.listViewPackage.Items.Clear();

            var packageId = this.textBoxPackageId.Text.Trim();
            if (this.checkBoxRedump.Checked) {
               var redump = await this.GetInternalSerial(packageId);
               if (redump != null) {
                  var internalSerial = redump.Replace("-", "").Trim();
                  MessageBox.Show($"Internal serial: Replace '{packageId}' to '{internalSerial}'", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  packageId = internalSerial;
               }
            }

            this.textBoxPackageId.Text = packageId;

            if (!this.IsValid(packageId)) {
               throw new InvalidPackageException($"The package id '{packageId}' is Invalid");
            }

            await Task.Run(() => {
               this._items = this.XMLParser(packageId);
               foreach (var pkg in this._items) {
                  var newItem = new ListViewItem { Text = pkg.Name };
                  newItem.SubItems.Add(pkg.Size.ToString());
                  newItem.SubItems.Add(pkg.Version);
                  newItem.SubItems.Add(pkg.PS3SystemVer != Environment.DefaultString ? pkg.PS3SystemVer : pkg.PSPSystemVer); /* PS3 or PSP */
                  newItem.SubItems.Add(pkg.Hash);

                  this.Invoke((Action)(() => this.listViewPackage.Items.Add(newItem)));
               }
            });

            if (this._items != null && this._items.Count >= 1) {
               this.AddSuggestion(packageId);
            }
         }
         catch (PackageNotFoundException ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally {
            this.UseWaitCursor = false;
         }
      }

      private async Task<string> GetInternalSerial(string packageId)
      {
         try {
            var titleId = Regex.Replace(packageId, @"[^0-9]", "");
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync($"http://redump.org/discs/quicksearch/{titleId}");
            var tbody = document.QuerySelector("table.gamecomments tbody");

            if (tbody != null) {
               var internalSerial = "";
               foreach (var child in tbody.ChildNodes) {
                  if (child.TextContent.Contains("Internal Serial")) {
                     internalSerial = child.TextContent.Split('\n').First();
                     break;
                  }
               }

               var texts = internalSerial.Split(':');
               return texts.Last();
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         return null;
      }

      private List<Package> XMLParser(string id)
      {
         var items = new List<Package>();
         var titleId = id.Trim();

         foreach (var url in this.Environments) {
            try {
               var xmlUrl = url + titleId + "/" + titleId + "-ver.xml";
               Trace.WriteLine(xmlUrl, "URL");

               var reader = new XmlTextReader(xmlUrl);
               do {
                  if (reader.NodeType == XmlNodeType.Element && reader.Name == "package") {
                     items.Add(this.AttributeReaderPS3(xmlUrl, ref reader));
                  }
               } while (reader.Read());
            }
            catch (WebException ex) {
               var response = (HttpWebResponse)ex.Response;
               switch (response.StatusCode) {
                  case HttpStatusCode.NotFound:
                  case HttpStatusCode.Forbidden:
                     break;

                  default:
                     MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                     break;
               }
            }
            catch (Exception ex) {
               MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }

         return items;
      }

      private void AddSuggestion(string titleId)
      {
         try {
            var suggestions = new HashSet<string>(Properties.Settings.Default.Suggestions.Cast<string>().ToArray()) { titleId };
            Properties.Settings.Default.Suggestions.Clear();
            Properties.Settings.Default.Suggestions.AddRange(suggestions.ToArray());
         }
         catch(Exception ex) {
            MessageBox.Show($"Suggestion Error: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
         finally {
            Properties.Settings.Default.Save();
         }

         this.SetAutoCompleteSource();
      }

      private Package AttributeReaderPS3(string xmlUrl, ref XmlTextReader reader)
      {
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

      private bool IsValid(string name) => string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException() : Regex.IsMatch(name, "^[A-Z0-9]");

      private void ListViewPackages_ItemActivate(object sender, EventArgs e)
      {
         if (this.listViewPackage.SelectedIndices.Count == 0) return;

         try {
            foreach (ListViewItem item in this.listViewPackage.SelectedItems) {
               Process.Start(this._items[item.Index].Url.ToString());
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void AboutToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Made by coreizer", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);

      private void GithubToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://www.github.com/coreizer/PackageStore");

      private void CopyToURLToolStripMenuItem_Click(object sender, EventArgs e) => this.ToClipboard("Url");

      private void CopyToSizeToolStripMenuItem_Click(object sender, EventArgs e) => this.ToClipboard("Size");

      private void CopyToVersionToolStripMenuItem_Click(object sender, EventArgs e) => this.ToClipboard("Version");

      private void CopyToSystemVersionToolStripMenuItem_Click(object sender, EventArgs e) => this.ToClipboard("SP_SYS");

      private void CopyToHashToolStripMenuItem_Click(object sender, EventArgs e) => this.ToClipboard("Hash");

      private void SetAutoCompleteSource()
      {
         try {
            this._autoComplete.Clear();
            this._autoComplete.AddRange(
               Properties.Settings.Default.Suggestions.Cast<string>().ToArray()
            );
            this.textBoxPackageId.AutoCompleteCustomSource = this._autoComplete;
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
      {
         try {
            Properties.Settings.Default.Save();
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void resetSuggestionToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try {
            Properties.Settings.Default.Suggestions.Clear();
            this.SetAutoCompleteSource();
         }
         finally {
            Properties.Settings.Default.Save();
            MessageBox.Show("Suggestions have been removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void SaveAsJSONStripMenuItem_Click(object sender, EventArgs e)
      {
         try {
            if (this._items != null && this._items.Count <= 0) throw new FileNotFoundException("Not found: Package List");

            using (var SFD = new SaveFileDialog()) {
               SFD.FileName = $"Export-{this.textBoxPackageId.Text}-{DateTime.Now:yyyyMMddHHmmss}";
               SFD.Filter = "JSON File|*.json";
               var result = SFD.ShowDialog();
               if (result == DialogResult.OK) {
                  var jsonString = JsonSerializer.Serialize(
                     new PackageExport(this._items),
                     new JsonSerializerOptions { WriteIndented = true }
                  );
                  File.WriteAllText(SFD.FileName, jsonString, System.Text.Encoding.UTF8);
               }
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void ToClipboard(string propertyName)
      {
         try {
            if (this.listViewPackage.SelectedIndices.Count >= 1) {
               var selectedItem = this.listViewPackage.SelectedItems[0];
               Clipboard.SetText(this._items[selectedItem.Index][propertyName]);
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void OpenXMLToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try {
            if (this.listViewPackage.SelectedIndices.Count >= 1) {
               var selectedItem = this.listViewPackage.SelectedItems[0];
               Process.Start(this._items[selectedItem.Index].XmlUrl);
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}
