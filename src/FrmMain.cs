/*
 * Copyright (c) 2017-2018 AlphaNyne
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

using PackageStore.Managed;

namespace PackageStore
{
  public partial class FrmMain : Form
  {
    private readonly string[] SizePrefix = {
      "K", "KB", "MB",
      "GB", "TB", "PB",
      "EB", "ZB", "YB"
    };

    private readonly string[] Servers = {
      "https://a0.ww.np.dl.playstation.net/tpl/np/",
      "http://b0.ww.np.dl.playstation.net/tppkg/np/",
      "https://sonycoment-1-ht.ocs.llnw.net/tppkg/np/",
      "http://b0.ww.prod-qa.dl.playstation.net/tppkg/prod-qa/"
    };

    private string packageId;
    private string packageName;

    private List<PackageInfo> packages = new List<PackageInfo>();
    private DownloadManager downloader = new DownloadManager();

    public FrmMain()
    {
      InitializeComponent();

      ServicePointManager.DefaultConnectionLimit = 30;
    }

    private async void ButtonSearch_Click(object sender, EventArgs e)
    {
      try {
        this.textBoxPackageId.Text = this.textBoxPackageId.Text.ToUpper();
        this.packageId = this.textBoxPackageId.Text.ToUpper();

        if (IsValid(this.packageId) || this.checkBoxForce.Checked) {
          this.buttonSearch.Enabled = false;
          this.listViewPackages.Items.Clear();

          this.packages.Clear();
          this.packages = await Task<List<PackageInfo>>.Factory.StartNew(() => {
            return PackageSearch(this.packageId);
          });

          if (this.packages != null && this.packages.Count != 0) {
            this.packages.ForEach(x => {
              ListViewItem item = new ListViewItem { Text = x.FileName };
              item.SubItems.AddRange(new[] { SizeOf(x.Size), x.Version, x.SupportVersion, x.Hash });
              this.listViewPackages.Items.Add(item);
            });
          }
          else {
            MessageBox.Show($"Package not found: {this.textBoxPackageId.Text}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }
        }
        else {
          MessageBox.Show($"The package id '{this.textBoxPackageId.Text}' is Invalid", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally {
        this.buttonSearch.Enabled = true;
      }
    }

    private bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

    private List<PackageInfo> PackageSearch(string packageId)
    {
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.OnRemoteCertificateValidationCallback);
      List<PackageInfo> packages = new List<PackageInfo>();

      foreach (string server in this.Servers) {
        try {
          XmlTextReader xml = new XmlTextReader(server + packageId + "/" + packageId + "-ver.xml");
          do {
            if (xml.NodeType == XmlNodeType.Element && xml.Name == "package") {
              packages.Add(XmlToPackageConvert(ref xml));
            }
            else if (xml.NodeType == XmlNodeType.Text) {
              this.packageName = xml.Value;
            }
          } while (xml.Read());
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
            MessageBox.Show("Please check your network and try again!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
        catch (Exception ex) {
          MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      return packages;
    }

    private PackageInfo XmlToPackageConvert(ref XmlTextReader reader)
    {
      PackageInfo package = new PackageInfo();
      for (int i = 0; i < reader.AttributeCount; i++) {
        reader.MoveToNextAttribute();
        switch (reader.Name) {
          case "version":
            package.Version = reader.Value;
            break;
          case "size":
            package.Size = long.Parse(reader.Value);
            break;
          case "sha1sum":
            package.Hash = reader.Value;
            break;
          case "ps3_system_ver":
            package.SupportVersion = reader.Value;
            break;
          case "url":
            package.FileName = Path.GetFileName(reader.Value);
            package.Address = new Uri(reader.Value);
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

    private string SizeOf(long size)
    {
      if (size <= 0) {
        return "0";
      }

      int i = (int)Math.Log(size, 1024);
      decimal received = (decimal)size / ((long)1 << (i * 10));
      return $"{received:N1}{this.SizePrefix[i]}";
    }

    private void ListViewPackages_ItemActivate(object sender, EventArgs e)
    {
      if (this.listViewPackages.SelectedIndices.Count == 0)
        return;

      FolderBrowserDialog FBD = new FolderBrowserDialog();
      if (FBD.ShowDialog() == DialogResult.OK) {
        foreach (ListViewItem item in this.listViewPackages.SelectedItems) {
          this.downloader.Add(new Scheduler(this.packages[item.Index], FBD.SelectedPath));
        }

        this.downloader.QueueDownload();
        this.downloader.Form.Show();
      }
    }

    private void DownloadManagerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.downloader.Form.Show();
    }

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Made by AlphaNyne", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Process.Start("https://www.github.com/AlphaNyne/Package-Store");
    }
  }
}
