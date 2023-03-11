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
   using System.Net.Security;
   using System.Security.Cryptography.X509Certificates;
   using System.Text.RegularExpressions;
   using System.Threading.Tasks;
   using System.Windows.Forms;
   using System.Xml;
   using ByteSizeLib;
   using PackageStore.Exceptions;

   public partial class frmMain : Form
   {
      // https://www.psdevwiki.com/ps3/Environments
      private readonly string[] Environments = {
      "https://a0.ww.np.dl.playstation.net/tpl/np/",
      "http://b0.ww.np.dl.playstation.net/tppkg/np/",
      "https://a0.ww.sp-int.dl.playstation.net/tpl/sp-int/",
      "http://b0.ww.sp-int.dl.playstation.net/tppkg/sp-int/",
      "https://a0.ww.prod-qa.dl.playstation.net/tpl/prod-qa/"
    };

      private List<Package> _items = new List<Package>();

      private readonly AutoCompleteStringCollection _autoComplete = new AutoCompleteStringCollection();

      public new bool UseWaitCursor
      {
         get => base.UseWaitCursor;

         set {
            base.UseWaitCursor = value;

            this.buttonSearch.Enabled = !value;
            this.textBoxPackageId.Enabled = !value;
            this.checkBoxForce.Enabled = !value;
            this.listViewPackage.Enabled = !value;
         }
      }

      public frmMain()
      {
         this.InitializeComponent();
         this.SetAutoCompleteSource();
         ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
      }

      private async void ButtonSearch_Click(object sender, EventArgs e)
      {
         try {
            this.Text = Application.ProductName;
            this.UseWaitCursor = true;
            this.listViewPackage.Items.Clear();

            if (!this.IsValid(this.textBoxPackageId.Text)) {
               throw new InvalidPackageException($"The package id '{this.textBoxPackageId.Text}' is Invalid");
            }

            await Task.Run(() => {
               this._items = this.PackageSearch(this.textBoxPackageId.Text);
               foreach (var package in this._items) {
                  var newItem = new ListViewItem { Text = package.Name };
                  newItem.SubItems.Add(package.Size.ToString());
                  newItem.SubItems.Add(package.Version);
                  newItem.SubItems.Add(!string.IsNullOrEmpty(package.SystemVersion) ? package.SystemVersion : package.SupportVersion);
                  newItem.SubItems.Add(package.Hash);
                  newItem.SubItems.Add(package.Digest);

                  this.Invoke((Action)(() => this.listViewPackage.Items.Add(newItem)));
               }
            });

            if (this._items.Count >= 1) {
               this.AddSuggestion(this.textBoxPackageId.Text);
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

      private List<Package> PackageSearch(string id, string environments = "NP")
      {
         var items = new List<Package>();
         var titleId = id.Trim();

         foreach (var url in this.Environments) {
            try {
               Trace.WriteLine(url + titleId + "/" + titleId + "-ver.xml", "URL");
               var reader = new XmlTextReader(url + titleId + "/" + titleId + "-ver.xml");

               do {
                  if (reader.NodeType == XmlNodeType.Element && reader.Name == "package") {
                     items.Add(this.AttributeReaderPS3(ref reader));
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
            var suggestions = new HashSet<string>(Properties.Settings.Default.Suggestions.Cast<string>().ToArray()) {
               titleId
            };

            Properties.Settings.Default.Suggestions.Clear();
            Properties.Settings.Default.Suggestions.AddRange(suggestions.ToArray());
         }
         finally {
            Properties.Settings.Default.Save();
         }

         this.SetAutoCompleteSource();
      }

      private Package AttributeReaderPS3(ref XmlTextReader reader)
      {
         var package = new Package();

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
                  package.SupportVersion = reader.Value;
                  break;

               case "url":
                  package.Name = Path.GetFileName(reader.Value);
                  package.Url = new Uri(reader.Value);
                  break;

            }
         }

         return package;
      }

      private bool IsValid(string name)
      {
         if (string.IsNullOrWhiteSpace(name)) {
            throw new ArgumentNullException();
         }

         return Regex.IsMatch(name, "^[A-Z0-9]"); ;
      }

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

      private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         MessageBox.Show("Made by coreizer", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Process.Start("https://www.github.com/coreizer/PackageStore");
      }

      private void CopyToURLToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try {
            if (this.listViewPackage.SelectedIndices.Count >= 1) {
               var selectedItem = this.listViewPackage.SelectedItems[0];
               Clipboard.SetText(this._items[selectedItem.Index].Url.ToString());
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void SetAutoCompleteSource(string addSuggestion = null)
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
   }
}
