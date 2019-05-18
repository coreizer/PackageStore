/*
 * Copyright (c) 2017-2019 AlphaNyne
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
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using PackageStore.Snippet;
using PackageStore.Managed;
using ByteSizeLib;

namespace PackageStore
{
  public partial class FrmMain : Form
  {
    private readonly string[] ServerList = {
      "https://a0.ww.np.dl.playstation.net/tpl/np/",
      "http://b0.ww.np.dl.playstation.net/tppkg/np/",
      "https://sonycoment-1-ht.ocs.llnw.net/tppkg/np/",
      "http://b0.ww.prod-qa.dl.playstation.net/tppkg/prod-qa/",
    };

    private string packageId;
    private string packageName;

    private List<PackageData> packageItems;
    private readonly DownloadManager downloadManager = new DownloadManager();

    public new bool UseWaitCursor {
      get => base.UseWaitCursor;

      set {
        base.UseWaitCursor = value;

        this.buttonSearch.Enabled = !value;
        this.textBoxPackageId.Enabled = !value;
        this.checkBoxForce.Enabled = !value;
      }
    }

    public FrmMain()
    {
      this.InitializeComponent();

      this.MinimumSize = this.Size;

      ServicePointManager.Expect100Continue = false;
      ServicePointManager.DefaultConnectionLimit = 30;
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
    }

    private async void ButtonSearch_Click(object sender, EventArgs e)
    {
      try {
        this.Text = Application.ProductName;
        this.UseWaitCursor = true;
        this.packageId = this.textBoxPackageId.Text.ToUpper();

        this.listViewPackage.Items.Clear();

        if (!this.IsValid(this.packageId) && this.checkBoxForce.CheckState != CheckState.Checked) {
          throw new Exceptions.InvalidPackageException($"The package id '{this.textBoxPackageId.Text}' is Invalid");
        }

        this.packageItems = await Task.Run(() => this.PackageSearch(this.packageId));
        if (this.packageItems.Count <= 0) {
          throw new Exceptions.PackageNotFoundException($"Package not found: {this.textBoxPackageId.Text}");
        }

        this.Text = $"{Application.ProductName} - {this.packageName}";

        this.packageItems.ForEach(x => {
          ListViewItem viewItem = new ListViewItem { Text = x.Name };
          viewItem.SubItems.AddRange(new[] {
            ByteSize.FromBytes(double.Parse(x.Size)).ToString(),
            x.Version,
            x.SupportVersion,
            x.Hash
          });

          this.listViewPackage.Items.Add(viewItem);
        });
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally {
        this.UseWaitCursor = false;
      }
    }

    private List<PackageData> PackageSearch(string packageId)
    {
      List<PackageData> package = new List<PackageData>();

      foreach (string baseUrl in this.ServerList) {
        try {
          XmlTextReader reader = new XmlTextReader(baseUrl + packageId + "/" + packageId + "-ver.xml");

          do {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "package") {
              package.Add(this.MetaDataReader(ref reader));
            }
            else if (reader.NodeType == XmlNodeType.Text) {
              this.packageName = reader.Value;
            }
          } while (reader.Read());
        }
        catch (WebException ex) {
          if (ex.Status == WebExceptionStatus.ProtocolError) {
            HttpWebResponse response = (HttpWebResponse)ex.Response;

            switch (response.StatusCode) {
              case HttpStatusCode.NotFound:
              case HttpStatusCode.Forbidden:
                break;

              default:
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;
            }
          }

          if (ex.Status == WebExceptionStatus.Timeout) {
            MessageBox.Show("Check your connection and try again", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            return null;
          }
        }
        catch (Exception ex) {
          MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      return package;
    }

    private PackageData MetaDataReader(ref XmlTextReader reader)
    {
      PackageData package = new PackageData();

      for (int i = 0; i < reader.AttributeCount; i++) {

        reader.MoveToNextAttribute();

        switch (reader.Name) {
          case "version":
            package.Version = reader.Value;
            break;

          case "size":
            package.Size = reader.Value;
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

    private bool IsValid(string packageId)
    {
      if (packageId.Length == 9) {
        return System.Text.RegularExpressions.Regex.IsMatch(packageId, "^[A-Z0-9]");
      }

      return false;
    }

    private void ListViewPackages_ItemActivate(object sender, EventArgs e)
    {
      if (this.listViewPackage.SelectedIndices.Count == 0)
        return;

      FolderBrowserDialog FBD = new FolderBrowserDialog();

      if (FBD.ShowDialog() == DialogResult.OK) {

        DownloadManager.Directory = FBD.SelectedPath;

        foreach (ListViewItem item in this.listViewPackage.SelectedItems) {
          this.downloadManager.Add(new JobContainer(this.packageItems[item.Index]));
        }

        this.downloadManager.Start();
        this.downloadManager.Form.Show();
      }
    }

    private void DownloadManagerToolStripMenuItem_Click(object sender, EventArgs e) =>
      this.downloadManager.Form.Show();

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e) =>
      MessageBox.Show("Made by AlphaNyne", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void GithubToolStripMenuItem_Click(object sender, EventArgs e) =>
      Process.Start("https://www.github.com/AlphaNyne/PackageStore");

    private void CopyToURLToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.listViewPackage.SelectedIndices.Count == 0)
        return;

      try {
        Clipboard.SetText(this.packageItems[this.listViewPackage.SelectedItems[0].Index].Url.ToString());
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
