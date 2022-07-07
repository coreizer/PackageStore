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

      private List<Package> PackageItems = new List<Package>();

      public new bool UseWaitCursor
      {
         get {
            return base.UseWaitCursor;
         }

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

            this.PackageItems = await Task.Run(() => this.PackageSearch(this.textBoxPackageId.Text));
            this.PackageItems.ForEach(x =>
            {
               ListViewItem package = new ListViewItem { Text = x.Name };
               package.SubItems.Add(x.Size.ToString());
               package.SubItems.Add(x.Version);
               package.SubItems.Add(!string.IsNullOrEmpty(x.SystemVersion) ? x.SystemVersion : x.SupportVersion);
               package.SubItems.Add(x.Hash);
               package.SubItems.Add(x.Digest);

               this.listViewPackage.Items.Add(package);
            });
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

      private List<Package> PackageSearch(string name, string environments = "NP")
      {
         List<Package> items = new List<Package>();

         foreach (string url in this.Environments) {
            try {
               Trace.WriteLine(url + name + "/" + name + "-ver.xml", "URL");
               XmlTextReader reader = new XmlTextReader(url + name + "/" + name + "-ver.xml");
               do {
                  if (reader.NodeType == XmlNodeType.Element && reader.Name == "package")
                     items.Add(this.AttributeReaderPS3(ref reader));
               } while (reader.Read());
            }
            catch (WebException ex) {
               HttpWebResponse response = (HttpWebResponse)ex.Response;
               switch (response.StatusCode) {
                  case HttpStatusCode.NotFound:
                  case HttpStatusCode.Forbidden:
                     break;

                  default: {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                     }
               }
            }
            catch (Exception ex) {
               MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }

         return items;
      }

      private Package AttributeReaderPS3(ref XmlTextReader reader)
      {
         Package package = new Package();

         for (int i = 0; i < reader.AttributeCount; i++) {
            reader.MoveToNextAttribute();
            switch (reader.Name) {
               case "version": {
                     package.Version = reader.Value;
                     break;
                  }

               case "size": {
                     package.Size = ByteSize.FromBytes(double.Parse(reader.Value.ToString()));
                     break;
                  }

               case "sha1sum": {
                     package.Hash = reader.Value;
                     break;
                  }

               case "ps3_system_ver": {
                     package.SupportVersion = reader.Value;
                     break;
                  }

               case "url": {
                     package.Name = Path.GetFileName(reader.Value);
                     package.Url = new Uri(reader.Value);
                     break;
                  }
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
         if (this.listViewPackage.SelectedIndices.Count == 0) {
            return;
         }

         try {
            foreach (ListViewItem item in this.listViewPackage.SelectedItems) {
               Process.Start(this.PackageItems[item.Index].Url.ToString());
            }
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         MessageBox.Show("Made by Coreizer", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Process.Start("https://www.github.com/coreizer/PackageStore");
      }

      private void CopyToURLToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (this.listViewPackage.SelectedIndices.Count == 0) return;

         try {
            Clipboard.SetText(this.PackageItems[this.listViewPackage.SelectedItems[0].Index].Url.ToString());
         }
         catch (Exception ex) {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}
